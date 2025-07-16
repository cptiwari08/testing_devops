from app.core.ai_search import BaseAISearch as AISearch
from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import extract_claim
from azure.search.documents.models import VectorizedQuery
from fastapi import HTTPException
from llama_index.core import Document, QueryBundle, VectorStoreIndex
from llama_index.core.base.base_retriever import BaseRetriever
from llama_index.core.postprocessor import LLMRerank
from llama_index.core.retrievers import VectorIndexRetriever
from starlette import status


async def citing_sources_retriever(query_str, context: QueryPipelineContext) -> BaseRetriever:
    """
    Initializes a retriever based on citing_sources description.

    The similarity search is configured to return the top 3 similar items.

    Returns:
        An instance of a retriever object configured to perform similarity
        searches on the created VectorStoreIndex.
    """
    context.logger.info("I moved on to execute the context retriever function component, which helps in understanding the surrounding context of the data")
    auth_token = context.token
    index_name = extract_claim(auth_token, "metadata_index_name")
    if not index_name:
        return []
    aisearch_instance_name = extract_claim(auth_token, "ai_search_instance_name")
    if not aisearch_instance_name:
        raise HTTPException(
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
            detail="Invalid AI Search instance name",
        )

    embedding = context.llm_models.embed_model.get_text_embedding(query_str)

    vector_query = VectorizedQuery(
        vector=embedding, k_nearest_neighbors=3, fields="embedding"
    )

    ai_search = AISearch(context.llm_models)
    await ai_search.async_set_client(index_name, aisearch_instance_name)

    query_bundle = QueryBundle(query_str)
    app_name = context.message_request.context.appInfo.name or None
    prj_intention = "prjdatacitingsources"
    filters = f"metadata/{prj_intention}/AppName eq '{app_name}' and indexType eq '{prj_intention}'"
    async with ai_search.async_client as client:
        results = await client.search(
            search_text=query_bundle,
            vector_queries=[vector_query],
            filter=filters,
            query_type="semantic",
            top=10,
            semantic_configuration_name="mySemanticConfig",
        )
        # Assuming result is not empty,  if results is empty evaluate returning 204
        new_doc = []
        async for result in results:
            if (
                result["@search.reranker_score"]
                and result["@search.reranker_score"] > 0.6
            ):
                metadata = {
                    "appId": result["metadata"].get(prj_intention, {})["AppId"],
                    "appName": result["metadata"].get(prj_intention, {})["AppName"],
                    "key": result["metadata"].get(prj_intention, {})["PageKey"],
                    "name": result["metadata"].get(prj_intention, {})["PageHeader"],
                    "href": result["metadata"].get(prj_intention, {})["HREF"],
                    "securityKey": result["metadata"].get(prj_intention, {}).get("SecurityKey", ""),
                }
                doc = Document(
                    doc_id=result["id"],
                    embedding=result["embedding"],
                    text=result["chunk"],
                    metadata=metadata,
                )
                new_doc.append(doc)

    index = VectorStoreIndex.from_documents(
        new_doc, embed_model=context.llm_models.embed_model
    )
    # configure retriever
    retriever = VectorIndexRetriever(
        index=index,
        similarity_top_k=10,
    )
    retrieved_nodes = retriever.retrieve(query_bundle)
    filtered_nodes = []
    # configure reranker

    for _ in range(3):
        try:
            reranker = LLMRerank(
                choice_batch_size=2,
                top_n=5,
            )
            resulting_retrieved_nodes = reranker.postprocess_nodes(
                retrieved_nodes, query_bundle
            )
            break
        except Exception as e:
            resulting_retrieved_nodes = []

    if not resulting_retrieved_nodes:
        return resulting_retrieved_nodes

    for node in resulting_retrieved_nodes:
        if node.score >= 6:
            filtered_nodes.append(node)

    return filtered_nodes
