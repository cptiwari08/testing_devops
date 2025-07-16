from app.pmo_workplan.services.workflow.events.search_docs_event import SearchDocsEvent
from app.pmo_workplan.services.workflow.events.search_team_event import SearchTeamEvent
from azure.search.documents.models import VectorizedQuery


async def search_docs(
    workflow, event: SearchTeamEvent, context, embed_model, logger
) -> SearchDocsEvent:
    target_team = await context.get("team")
    logger.info(f"Initialized step search_docs for {target_team}")
    team = context.data.get("workplan_params", None).get("team", None)
    documents = await context.get("document_ids", None)
    embedding = embed_model.get_text_embedding(team)
    vector_query = VectorizedQuery(
        vector=embedding, k_nearest_neighbors=3, fields="chunk_embeddings"
    )

    original_doc_splits = []
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
    await context.set("original_doc_splits", original_doc_splits)
    logger.info(f"Finalized step search_docs for {target_team}")
    return SearchDocsEvent(search_results=original_doc_splits)
