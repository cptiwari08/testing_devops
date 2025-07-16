from app.core.ai_search import BaseAISearch
from app.core.nltk import NLTKManager
from azure.search.documents.models import VectorizedQuery
from llama_index.core import Document, VectorStoreIndex


class AISearchManager(BaseAISearch):

    async def context_retriever(self, query_str, similarity_top_k: int = 5) -> dict:
        """
        Initializes a context retriever based on documents extracted from AI
        Search.

        The similarity search is configured to return the top 3 similar items.

        Returns:
            An instance of a retriever object configured to perform similarity
            searches on the created VectorStoreIndex.

        """

        embedding = self.llm_models.embed_model.get_text_embedding(query_str)

        vector_query = VectorizedQuery(
            vector=embedding, k_nearest_neighbors=3, fields="embedding"
        )

        nltk_ = NLTKManager()
        async with self.async_client as client:
            results = await client.search(
                search_text=nltk_.clean_and_tokenize_text(query_str),
                vector_queries=[vector_query],
                query_type="semantic",
                semantic_configuration_name="mySemanticConfig",
                top=similarity_top_k,
            )
            # Assuming result is not empty,  if results is empty evaluate returning 204
            new_doc = []
            text_docs = []
            async for result in results:
                if (
                    result["@search.reranker_score"]
                    and result["@search.reranker_score"] > 0
                ):
                    metadata = {"tableName": result["tableName"], "tag": result["tag"]}
                    doc = Document(
                        doc_id=result["doc_id"],
                        embedding=result["embedding"],
                        text=result["chunk"],
                        metadata=metadata,
                    )
                    new_doc.append(doc)
                    text_docs.append(result["chunk"])

        index = VectorStoreIndex.from_documents(
            new_doc, embed_model=self.llm_models.embed_model
        )
        retrieved_index = index.as_retriever(
            similarity_top_k=similarity_top_k, llm=self.llm_models.llm
        )
        return {
            "retrieved_index": retrieved_index,
            "retrieved_docs": text_docs,
        }
