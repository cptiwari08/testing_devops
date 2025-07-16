from app.tsa_generator.services.workflow.events.search_docs_event import SearchDocsEvent
from azure.search.documents.models import VectorizedQuery


async def search_docs(
    workflow, context, embed_model
) -> SearchDocsEvent:
    team = context.data.get("team").title
    documents = context.data.get("project_doc")
    embedding = embed_model.get_text_embedding(team)
    vector_query = VectorizedQuery(
        vector=embedding, k_nearest_neighbors=3, fields="chunk_embeddings"
    )

    original_doc_splits = []
    chunk_texts = []
    if documents:
        search_in_start = "search.in(external_document_uuid, "
        document_ids = ",".join([str(doc) for doc in documents])
        filters = f"{search_in_start}'{document_ids}')"

        original_doc_splits = await workflow.ai_search.get_originals_docs(
            team=team,
            filters=filters,
            vector_query=vector_query,
            similarity_top_k=20,
        )
        chunk_texts = [item['chunk_text'] for item in original_doc_splits]

    await context.set("original_doc_splits", original_doc_splits)
    await context.set("chunk_text", chunk_texts)
    return SearchDocsEvent(search_results=chunk_texts)

 