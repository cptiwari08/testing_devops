from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.config import AISearchConfig
from azure.core.credentials import AzureKeyCredential
from azure.search.documents.aio import SearchClient
from azure.search.documents.models import VectorizedQuery
from llama_index.core import Document


class AISearch:

    def __init__(
        self,
        ai_search_config: AISearchConfig,
        llm: AzureSyncLLM,
        embed_model: AzureEmbeddings,
    ) -> None:
        self.ai_search_config = ai_search_config
        self.llm = llm
        self.embed_model = embed_model

    async def get_documents(
        self, question: str, filters: str, vector_query, similarity_top_k: int = 1
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
        ai_search_client = SearchClient(
            endpoint=self.ai_search_config.endpoint,
            index_name=self.ai_search_config.index_name,
            credential=AzureKeyCredential(self.ai_search_config.api_key),
        )
        async with ai_search_client as client:
            results = await client.search(
                search_text=question,
                filter=filters,
                vector_queries=[vector_query],
                semantic_configuration_name="semantic-search",
                query_type="semantic",
                top=similarity_top_k,
            )
            new_doc = []
            text_docs = []
            async for result in results:
                if (
                    result["@search.reranker_score"]
                    and result["@search.reranker_score"] > 1.5
                ):
                    doc = {'documentId': result["external_document_uuid"],
                            'chunk_id': result["chunk_id"],
                            'documentName': result["File_name"],
                            'chunk_text': result["chunk_text"],}
                    new_doc.append(doc)
                    text_docs.append(result["chunk_text"])

        return {"docs": new_doc, "retrieved_chunks": text_docs}

    async def get_originals_docs(
        self, team: str, filters: str, vector_query, similarity_top_k: int = 1
    ) -> list[Document]:
        """
        Creates a retriever from filters.
        """
        ai_search_client = SearchClient(
            endpoint=self.ai_search_config.endpoint,
            index_name=self.ai_search_config.index_name,
            credential=AzureKeyCredential(self.ai_search_config.api_key),
        )
        async with ai_search_client as client:
            results = await client.search(
                search_text=team,
                filter=filters,
                vector_queries=[vector_query],
                semantic_configuration_name="semantic-search",
                query_type="semantic",
                top=similarity_top_k,
            )

            search_results = []
            async for result in results:
                text_doc = {
                    "documentId": result["external_document_uuid"],
                    "documentName": result["File_name"],
                    "chunk_id": result["chunk_id"],
                    "chunk_text": result["chunk_text"],
                    "data_chunk": result["data_chunk"],
                    "search_score": round(result["@search.score"], 2),
                    "search_reranker_score": round(result["@search.reranker_score"], 2),
                }
                search_results.append(text_doc)

            # Select the top 1 similar chunk from the search results to retrieve charter
            top_documents = search_results[:1]

            # Extract the unique IDs from the top chunks
            top_similar_doc_ids = {
                doc.get("documentId")
                for doc in top_documents
                if doc.get("documentId") is not None
            }

            # Filter the chunks by the extracted IDs
            filtered_documents = [
                doc
                for doc in search_results
                if doc.get("documentId") in top_similar_doc_ids
            ]

            # Organize the chunks to aproximate the original document order
            original_doc_splits = sorted(
                filtered_documents,
                key=lambda doc: (doc.get("documentId"), doc.get("data_chunk", "")),
            )
        return original_doc_splits

    async def perform_search(self, question, documents) -> dict:
        embedding = self.embed_model.get_model().get_text_embedding(question)
        vector_query = VectorizedQuery(
            vector=embedding, k_nearest_neighbors=3, fields="chunk_embeddings"
        )
        search_in_start = "search.in(external_document_uuid, "
        document_ids = ",".join([str(doc) for doc in documents])
        filters = f"{search_in_start}'{document_ids}')"
        search_results = await self.get_documents(
            question=question,
            filters=filters,
            vector_query=vector_query,
            similarity_top_k=6,
        )
        documents = " ".join(search_results["retrieved_chunks"])

        return {"raw_documents": documents, "original_documents": search_results["docs"]}
