import asyncio
import re
import time
from app.core.config import MemorySize
from app.core.interfaces import IBaseLogger
from app.core.memory import MemoryManager
from app.core.pydantic_models import LLMModels, MessageRequest, MetricInput
from app.metrics.rag_evaluator import RagEvaluator
from app.metrics.strategy_context import Context
from app.core.pydantic_models import LLMModels, MessageRequest
from app.core.utils import (
    extract_claim,
    extract_relevant_content,
    clean_title,
    clear_special_characters,
)
from app.project_docs.services.ai_search import AISearch
from app.project_docs.services.followup_question import generate_followup_questions
from azure.search.documents.models import VectorizedQuery
from fastapi import HTTPException, status
from fastapi.encoders import jsonable_encoder
from llama_index.core import VectorStoreIndex, SummaryIndex
from app.core.prompt_manager import create_prompt_manager
import asyncio
import numpy as np
from sklearn.cluster import KMeans
from sklearn.metrics import silhouette_score
from llama_index.core.tools import QueryEngineTool, ToolMetadata
from llama_index.agent.openai import OpenAIAgent
from llama_index.core.objects import ObjectIndex, ObjectRetriever
from sentence_transformers import SentenceTransformer
from llama_index.core.query_engine import SubQuestionQueryEngine
from llama_index.core.schema import QueryBundle
from llama_index.core.postprocessor import LLMRerank


class CustomObjectRetriever(ObjectRetriever):
    def __init__(
        self,
        retriever,
        object_node_mapping,
        node_postprocessors=None,
        llm=None,
        prompt_manager=None,
    ):
        self._retriever = retriever
        self._object_node_mapping = object_node_mapping
        self._llm = llm
        self._node_postprocessors = node_postprocessors or []
        self._prompt_manager = prompt_manager

    def retrieve(self, query_bundle):
        if isinstance(query_bundle, str):
            query_bundle = QueryBundle(query_str=query_bundle)

        nodes = self._retriever.retrieve(query_bundle)
        for processor in self._node_postprocessors:
            nodes = processor.postprocess_nodes(nodes, query_bundle=query_bundle)
        tools = [self._object_node_mapping.from_node(n.node) for n in nodes]

        sub_question_engine = SubQuestionQueryEngine.from_defaults(
            query_engine_tools=tools, llm=self._llm
        )
        
        # Get the prompt for compare tool description from YAML
        sub_question_description = self._prompt_manager.get_prompt_sync(
            agent="project_docs",
            key="sub_question_tool_description"
        )
        
        sub_question_tool = QueryEngineTool(
            query_engine=sub_question_engine,
            metadata=ToolMetadata(
                name="compare_tool", description=sub_question_description
            ),
        )

        return tools + [sub_question_tool]


class ServiceAgent:
    """
    A class used to generate responses to questions about project documents.
    This class provides a method to execute the AI search and chat engine to generate a response
    """

    def __init__(self, logger: IBaseLogger) -> None:
        super().__init__()
        self._logger = logger
        self.embed_model_cluster = SentenceTransformer(
            "all-mpnet-base-v2"
        )  ## Este deberia instanciarse desde afuera
        self.prompt_manager = create_prompt_manager(self._logger)
        self._logger.info("ServiceAgent initialized", extra_dims={"component": "Project Docs Agent", "service": "document_qa"})

    async def extract_documents(
        self,
        documents,
        question,
        vector_query,
        llm_models,
        index_name,
        aisearch_instance_name,
    ):
        start_time = time.time()
        self._logger.info("Starting document extraction", extra_dims={"operation": "extract_documents"})
        search_in_start = "search.in(external_document_uuid, "
        document_list = []
        async def process_document(document):
            dict_document = {}
            ai_search = AISearch(llm_models=llm_models)
            await ai_search.async_set_client(index_name, aisearch_instance_name)
            filter_id = f"{search_in_start}'{document}')"
            retrieved_elements = await ai_search.get_docs(
                question=question,
                doc_id=filter_id,
                vector_query=vector_query,
                similarity_top_k=10,
                num_chunks=30,
            )
            summary_index = SummaryIndex.from_documents(retrieved_elements["nodes"])
            summary_query_engine = summary_index.as_query_engine(
                llm=llm_models.llm, response_mode="tree_summarize"
            )
            

            summarize_documents_prompt = await self.prompt_manager.get_prompt(
                agent="project_docs",
                key="summarize_documents_prompt"
            )
            # Execute the query with the prompt
            summary = await summary_query_engine.aquery(summarize_documents_prompt)
            summary = str(summary)


            if retrieved_elements["nodes"]:
                dict_document["document"] = retrieved_elements
                doc_name = clean_title(
                    retrieved_elements["nodes"][0].metadata["documentName"]
                )
                dict_document["document_name"] = doc_name
                dict_document["summary"] = summary
                return dict_document
            return None

        tasks = [process_document(document) for document in documents]

        results = await asyncio.gather(*tasks)
        document_list = [result for result in results if result is not None]

        duration = round((time.time() - start_time) * 1000, 2)
        self._logger.info("Completed document extraction", extra_dims={"document_count": len(document_list), "duration_ms": duration})
        return document_list

    async def cluster_documents(self, document_list, texts):
        start_time = time.time()
        self._logger.info("Starting document clustering", extra_dims={"operation": "cluster_documents", "documents": len(texts)})

        embed_model_cluster = self.embed_model_cluster

        def calculate_number_clusters(
            num_documents,
            num_clusters=10,
            min_documents=20,
            incremental_factor=200,
            upper_limit=50,
        ):
            """
            Calculate the number of chunks to retrieve based on the number of documents.
            Args:
                num_documents (int): Number of documents.
                initial_top_k (int): Initial top k.
                min_documents (int, optional): Defaults to 20.
                incremental_factor (int, optional): Defaults to 500.
                upper_limit (int, optional): Defaults to 200.
            """
            if num_documents <= num_clusters:
                return num_documents - 1

            if num_documents <= min_documents:
                return num_clusters

            incremental_factor = (num_documents - min_documents) // incremental_factor
            chunks = num_clusters + incremental_factor
            return min(chunks, upper_limit)

        def create_agent_cluster(model, texts):

            embeddings = model.encode(texts)
            if len(texts) >= 50:
                max_clusters = calculate_number_clusters(len(texts))
                silhouette_scores = []
                min_clusters = 10
                for i in range(min_clusters, max_clusters):
                    kmeans = KMeans(n_clusters=i)
                    labels = kmeans.fit_predict(embeddings)
                    silhouette_avg = silhouette_score(embeddings, labels)
                    silhouette_scores.append(silhouette_avg)

                optimal_clusters = (
                    np.argmax(silhouette_scores) + min_clusters
                )  # +2 because we started from 2
                num_clusters = optimal_clusters
            else:
                num_clusters = 1 if len(texts) == 1 else len(texts) // 2

            clustering_model = KMeans(n_clusters=num_clusters)
            clustering_model.fit(embeddings)
            cluster_assignment = clustering_model.labels_

            return cluster_assignment

        team_indices = {}
        for index, team in enumerate(create_agent_cluster(embed_model_cluster, texts)):
            if team not in team_indices:
                team_indices[team] = []
            team_indices[team].append(index)

        team_elements = {}

        for team, indices in team_indices.items():
            team_elements[team] = {}
            team_elements[team]["titles"] = [texts[i] for i in indices]
            original_docs = [document_list[i] for i in indices]
            team_elements[team]["documents"] = [
                document
                for documents in original_docs
                for document in documents["document"]["nodes"]
            ]

        duration = round((time.time() - start_time) * 1000, 2)
        self._logger.info("Completed document clustering", extra_dims={"cluster_count": len(team_elements), "duration_ms": duration})
        return team_elements

    async def create_doc_agents(self, cluster_list, llm_models):
        start_time = time.time()
        self._logger.info("Starting creation of document-specific agents", extra_dims={"operation": "create_doc_agents", "cluster_count": len(cluster_list)})

        async def build_agent_per_doc(documents):

            vector_index = VectorStoreIndex.from_documents(
                documents["documents"], embed_model=llm_models.embed_model
            )
            summary_index = SummaryIndex.from_documents(documents["documents"])

            vector_query_engine = vector_index.as_query_engine(
                llm=llm_models.llm, similarity_top_k=10
            )
            summary_query_engine = summary_index.as_query_engine(
                llm=llm_models.llm, response_mode="tree_summarize"
            )
            num_lines = len(documents["titles"])
            
            generate_description_prompt = await self.prompt_manager.get_prompt(
                agent="project_docs",
                key="generate_description_prompt"
            )
            
            # Execute the query with the prompt
            summary = await summary_query_engine.aquery(generate_description_prompt)
            summary = str(summary)

            # Get prompt for title generation
            title_generation_prompt = await self.prompt_manager.get_prompt(
                agent="project_docs",
                key="title_generation_prompt"
            )
            
            # Execute the query with the prompt
            title = await summary_query_engine.aquery(title_generation_prompt)
            title = str(title)

            # Get prompt for summary tool description
            summary_tool_prompt = await self.prompt_manager.get_prompt(
                agent="project_docs",
                key="summary_tool_description",
                prompt_parameters={
                    "document_titles": ", ".join(documents["titles"])
                }
            )
            
            summary_tool = QueryEngineTool.from_defaults(
                query_engine=summary_query_engine,
                description=summary_tool_prompt,
            )

            # Get prompt for vector tool description
            vector_tool_prompt = await self.prompt_manager.get_prompt(
                agent="project_docs",
                key="vector_tool_description"
            )
            
            vector_tool = QueryEngineTool.from_defaults(
                query_engine=vector_query_engine,
                description=vector_tool_prompt,
            )

            doc_names = documents.get("titles")

            # Get prompt for agent system prompt
            cluster_agent_prompt = await self.prompt_manager.get_prompt(
                agent="project_docs",
                key="cluster_agent_prompt",
                prompt_parameters={
                    "doc_names": doc_names
                }
            )
            
            agent = OpenAIAgent.from_tools(
                tools=[summary_tool, vector_tool],
                llm=llm_models.llm,
                verbose=True,
                system_prompt=cluster_agent_prompt,
            )

            return agent, summary, title

        agents_dict = {}
        extra_info_dict = {}
        # iterar por grupo de documentos
        for index, value in cluster_list.items():

            agent, summary, title = await build_agent_per_doc(value)
            agent_name = f"agent_{index}"
            agents_dict[agent_name] = agent
            extra_info_dict[agent_name] = {
                "summary": summary,
                "documents": value,
                "title": title,
            }
        duration = round((time.time() - start_time) * 1000, 2)
        self._logger.info("Completed creation of document-specific agents", extra_dims={"agents_created": len(agents_dict), "duration_ms": duration})
        return agents_dict, extra_info_dict

    async def create_top_level_agent(self, agents_dict, extra_info_dict, llm_models, message_request):
        """
        Create a top-level agent that orchestrates the document-specific agents.
        
        Args:
            agents_dict: Dictionary of document-specific agents
            extra_info_dict: Additional information about each agent
            llm_models: LLM models for generating embeddings and responses
            message_request: The original message request containing chat history
            
        Returns:
            An OpenAI agent configured to use the document-specific agents as tools
        """

        start_time = time.time()
        self._logger.info("Starting creation of the top-level agent", extra_dims={"operation": "create_top_level_agent"})
        ## Load additional information
        chat_history = MemoryManager.generate_chat_messages(
            message_request.chatHistory,  # type: ignore
            size=MemorySize.project_docs,
        )
        
        project_description_context = ""
        if message_request.context.projectDescription:
            project_description_context = extract_relevant_content(
                message_request.context.projectDescription,
                message_request.question,
                llm_models,
            )

        # Get prompt using the async prompt manager method
        top_level_agent_prompt = await self.prompt_manager.get_prompt(
            agent="project_docs",
            key="top_level_agent",
            prompt_parameters={
                "additional_context": project_description_context
            }
        )
        
        all_tools = []
        indice = 1
        for file_base, agent in agents_dict.items():
            indice += 1
            summary = extra_info_dict[file_base]["summary"]
            title = extra_info_dict[file_base]["title"]
            cleaned_title = clear_special_characters(title)
            doc_tool = QueryEngineTool(
                query_engine=agent,
                metadata=ToolMetadata(
                    name=f"{cleaned_title}",
                    description=summary,
                ),
            )
            all_tools.append(doc_tool)

        obj_index = ObjectIndex.from_objects(
            all_tools, index_cls=VectorStoreIndex, embed_model=llm_models.embed_model
        )
        vector_node_retriever = obj_index.as_node_retriever(
            similarity_top_k=15,
        )

        custom_obj_retriever = CustomObjectRetriever(
            vector_node_retriever,
            obj_index.object_node_mapping,
            node_postprocessors=[LLMRerank(llm=llm_models.llm, top_n=10)],
            llm=llm_models.llm,
            prompt_manager=self.prompt_manager,
        )

        top_agent = OpenAIAgent.from_tools(
            tool_retriever=custom_obj_retriever,
            verbose=True,
            llm=llm_models.llm,
            system_prompt=top_level_agent_prompt,
            chat_history=chat_history,
        )
        duration = round((time.time() - start_time) * 1000, 2)
        self._logger.info("Completed creation of the top-level agent", extra_dims={"duration_ms": duration})
        return top_agent

    async def execute(
        self, message_request: MessageRequest, authorization: str, llm_models: LLMModels
    ) -> dict:
        """
        Generates a response to a question about project documents.
        This method uses the AI search and chat engine to generate a response to the question
        in the MessageRequest object. The response is returned as a dictionary.

        Args:
            message_request (MessageRequest): A MessageRequest object containing the question and documents IDs

        Returns:
            dict: A dictionary containing the response generated by the chat engine
        """
        start_time = time.time()
        self._logger.info("Starting ServiceAgent execution", extra_dims={"operation": "execute"})
        documents = message_request.context.documents
        if not documents:
            raise HTTPException(
                status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
                detail="No documents IDs in request payload",
            )

        self._logger.info("Agentic project document execution started")
        ### Retrieve original documents
        token = authorization[len("Bearer ") :]
        self._logger.info("Extracting search instance and index from token", extra_dims={"operation": "extract_claims"})
        index_name = extract_claim(token, "docs_index_name")
        aisearch_instance_name = extract_claim(token, "ai_search_instance_name")
        if not aisearch_instance_name:
            self._logger.error("Invalid AI Search instance name", extra_dims={"operation": "token_validation"})
            raise HTTPException(
                status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
                detail="Invalid AI Search instance name",
            )
        self._logger.info("Extracting original documents")
        embedding = llm_models.embed_model.get_text_embedding(message_request.question)
        vector_query = VectorizedQuery(
            vector=embedding, k_nearest_neighbors=3, fields="chunk_embeddings"
        )
        
        original_documents = await self.extract_documents(
            documents,
            message_request.question,
            vector_query,
            llm_models,
            index_name,
            aisearch_instance_name,
        )
        ### Extract summary text from documents
        if not original_documents:
            raise HTTPException(
                status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
                detail="No documents found",
            )
        self._logger.info("Extracting summaries from original documents")
        text = [document["summary"] for document in original_documents]
        ### Cluster documents
        self._logger.info("Clustering extracted documents")
        team_elements = await self.cluster_documents(original_documents, text)
        ### Create agents for each cluster
        self._logger.info("Creating document-specific agents")
        agents_dict, extra_info_dict = await self.create_doc_agents(
            team_elements, llm_models
        )
        ### Create top level agent
        self._logger.info("Creating top-level agent")
        agent = await self.create_top_level_agent(
            agents_dict, extra_info_dict, llm_models, message_request
        )
        ### Build response
        self._logger.info("Building final response")
        raw_response = await agent.achat(message_request.question)
        # citing_sources
        citing_sources = {
            "sourceName": "project-docs",
            "sourceType": "documents",
            "sourceValue": [],
        }
        # We cannot have duplicated citing sources, and this is happening,
        # probably due to the top_k used. Therefore, we need to remove the duplicated
        # citing sources. We accomplish is by keeping track chunks IDs and only
        # using that chunk to build the citing sources if it is the first time that
        # has seen
        seen_chunk_ids = set()
        nodes_metadata = []
        chunks = []
        for node in raw_response.source_nodes:
            chunk_id = node.metadata.get("chunk_id", None)
            if chunk_id and chunk_id not in seen_chunk_ids:
                pattern = r"END for Page Number - (\d+)"
                matches = re.findall(pattern, node.text)
                pages = [int(match) for match in matches]
                node.metadata["pages"] = pages
                node.metadata["chunk_text"] = node.text
                chunks.append(node.text)
                nodes_metadata.append(node.metadata)
                seen_chunk_ids.add(chunk_id)
        citing_sources["sourceValue"] = nodes_metadata
        chat_history = MemoryManager.generate_chat_messages(
            message_request.chatHistory,  # type: ignore
            size=MemorySize.project_docs,
        )
        followup_questions = await generate_followup_questions(
            message_request.question,
            llm_models,
            chat_history,
            chunks,
        )

        response = {
            "backend": "project-docs",
            "status_code": 200,
            "chainOfThoughts": ""
        }
        if nodes_metadata:
            self._logger.info("Updating response with citing sources", extra_dims={"source_count": len(nodes_metadata)})
            response.update(
                {
                    "response": raw_response.response,
                    "citingSources": [citing_sources],
                    "rawResponse": str(raw_response),
                    "score": 1,
                    "followUpSuggestions": followup_questions,
                }
            )
        else:
            try:
                self._logger.info("No valid sources found, still updating response with available data")
                response.update(
                    {
                        "response": raw_response.response,
                        "citingSources": [citing_sources],
                        "rawResponse": str(raw_response),
                        "score": 1,
                        "followUpSuggestions": followup_questions,
                    }
                )
            except Exception as e:
                self._logger.info("There wasn't any source available")
                response.update({"status_code": 204})
        encoded_response = jsonable_encoder(response)
        total_time = round((time.time() - start_time) * 1000, 2)
        self._logger.info("Completed ServiceAgent execution", extra_dims={"total_execution_time_ms": total_time})
        return encoded_response
