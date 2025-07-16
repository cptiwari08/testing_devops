from app.core.ai_search import BaseAISearch
from app.core.nltk import NLTKManager
from llama_index.core import Document, VectorStoreIndex


class AISearch(BaseAISearch):
    """
    A class for managing the Azure AI Search service.
    This class provides methods for creating a retriever from filters.
    """

    async def get_documents(
        self, question: str, filters: str, vector_query, similarity_top_k: int = 1, num_documents: int = 1
    ) -> dict:
        """
        Creates a retriever from filters.
        This method creates a retriever from the given metadata fields and filters.

        Args:
            metadata_fields (dict): Metadata fields.
            filters (MetadataFilters): Metadata filters.
            similarity_top_k (int, optional): Defaults to 1.

        Returns:
            Retriever: Return a vector retriver
        """
        nltk_ = NLTKManager()
        clean_question = nltk_.clean_and_tokenize_text(question)
        top_chunks = self.calculate_top_k(
            num_documents=num_documents, initial_top_k=similarity_top_k
        )   
        async with self.async_client as client:
            results = await client.search(
                search_text=clean_question,
                filter=filters,
                vector_queries=[vector_query],
                semantic_configuration_name="semantic-search",
                query_type="semantic",
                top=top_chunks,
            )
            new_doc = []
            text_docs = []
            docs_ids = []
            async for result in results:
                docs_ids.append(result["external_document_uuid"])
                if (
                    result["@search.reranker_score"]
                    and result["@search.reranker_score"] > 1.5
                ):
                    metadata = {
                        "documentId": result["external_document_uuid"],
                        "documentName": result["File_name"],
                        "chunk_id": result["chunk_id"],
                    }
                    doc = Document(
                        doc_id=result["external_document_uuid"],
                        embedding=result["chunk_embeddings"],
                        text=result["chunk_text"],
                        metadata=metadata,
                    )
                    new_doc.append(doc)
                    text_docs.append(result["chunk_text"])

        filtered_index = VectorStoreIndex.from_documents(
            new_doc, embed_model=self.llm_models.embed_model
        )

        return {
            "filtered_index": filtered_index.as_retriever(
                similarity_top_k=similarity_top_k, llm=self.llm_models.llm
            ),
            "rerieved_chunks": text_docs,
        }

    def calculate_top_k(self, num_documents, initial_top_k, min_documents = 20, incremental_factor = 500,
        upper_limit = 200):
        """
        Calculate the number of chunks to retrieve based on the number of documents.
        Args:
            num_documents (int): Number of documents.
            initial_top_k (int): Initial top k.
            min_documents (int, optional): Defaults to 20.
            incremental_factor (int, optional): Defaults to 500.
            upper_limit (int, optional): Defaults to 200.
        """
        if num_documents <= min_documents:
            return initial_top_k

        additional_chunks = (num_documents - min_documents) // incremental_factor
        chunks = initial_top_k + additional_chunks
        return min(chunks, upper_limit)


    async def get_docs(
            self, question: str, vector_query, doc_id: str, similarity_top_k: int = 1, num_chunks: int = 30
        ) -> dict:
            """
            Creates a retriever from filters.
            This method creates a retriever from the given metadata fields and filters.

            Args:
                metadata_fields (dict): Metadata fields.
                filters (MetadataFilters): Metadata filters.
                similarity_top_k (int, optional): Defaults to 1.

            Returns:
                Retriever: Return a vector retriver
            """
            nltk_ = NLTKManager()
            clean_question = nltk_.clean_and_tokenize_text(question)
            async with self.async_client as client:
                results = await client.search(
                    search_text=clean_question,
                    filter=doc_id,
                    vector_queries=[vector_query],
                    semantic_configuration_name="semantic-search",
                    query_type="semantic",
                    top=num_chunks,
                )
                new_doc = []
                document_name = []
                docs_ids = []
                metadata = {}
                chunks = []
                async for result in results:
                    docs_ids.append(result["external_document_uuid"])
                    if (
                        result["@search.reranker_score"]
                        and result["@search.reranker_score"] > 1.2
                    ):
                        metadata = {
                            "documentId": result["external_document_uuid"],
                            "documentName": result["File_name"],
                            "chunk_id": result["chunk_id"],
                        }
                        doc = Document(
                            doc_id=result["external_document_uuid"],
                            embedding=result["chunk_embeddings"],
                            text=result["chunk_text"],
                            metadata=metadata,
                        )
                        new_doc.append(doc)
                        chunks.append(result["chunk_text"])


            return {
                "nodes": new_doc,
                "chunks": chunks,
            }
    
    async def get_best_docs(
            self, question: str, doc_id: str, similarity_top_k: int = 1, num_chunks: int = 30
        ) -> dict:
            """
            Creates a retriever from filters.
            This method creates a retriever from the given metadata fields and filters.

            Args:
                metadata_fields (dict): Metadata fields.
                filters (MetadataFilters): Metadata filters.
                similarity_top_k (int, optional): Defaults to 1.

            Returns:
                Retriever: Return a vector retriver
            """
            async with self.async_client as client:
                results = await client.search(
                    search_text=question,
                    filter=doc_id,
                    # vector_queries=[vector_query],
                    semantic_configuration_name="semantic-search",
                    query_type="semantic",
                    top=num_chunks,
                )
                new_doc = []
                documents = {}
                docs_ids = []
                metadata = {}
                chunks = []
                async for result in results:
                    
                    docs_ids.append(result["external_document_uuid"])
                    if (
                        result["@search.reranker_score"]
                        and result["@search.reranker_score"] > 1.5
                    ):
                        metadata = {
                            "documentId": result["external_document_uuid"],
                            "documentName": result["File_name"],
                            "chunk_id": result["chunk_id"],
                        }
                        doc = Document(
                            doc_id=result["external_document_uuid"],
                            embedding=result["chunk_embeddings"],
                            text=result["chunk_text"],
                            metadata=metadata,
                        )
                        if result["external_document_uuid"] not in documents.keys():
                            documents[result["external_document_uuid"]] = {"docs":[]}
                            documents[result["external_document_uuid"]] = {"chunks":[]}
                        else:
                            documents[result["external_document_uuid"]]['docs'].append(doc)
                            documents[result["external_document_uuid"]]['chunks'].append(result["chunk_text"])
                        new_doc.append(doc)
                        chunks.append(result["chunk_text"])
            

            return {
                "documents": documents
            }
