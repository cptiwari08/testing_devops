from app.project_charter.services.workflow.events.search_docs_event import SearchDocsEvent
from app.project_charter.services.workflow.events.search_team_event import SearchTeamEvent
from azure.search.documents.models import VectorizedQuery
from app.core.prompt_manager import create_prompt_manager
from llama_index.core import PromptTemplate
from app.project_charter.utils import string_to_json
 
prompt_manager = create_prompt_manager()
 
 
async def search_docs(
    workflow, event: SearchTeamEvent, context, embed_model, llm
) -> SearchDocsEvent:
 
    team = context.data.get("team").title
    documents = context.data.get("project_doc")
    sections_list = context.data.get("sections")
    sections = ', '.join(sections_list)
    embedding = embed_model.get_text_embedding(sections)
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
 
        example_structure = prompt_manager.get_prompt_sync(
            agent="project_charter",
            key="example_charter_structure"
        )
 
        generation_task = prompt_manager.get_prompt_sync(
            agent="project_charter",
            key="project_docs_data_consolidation"
        )
        generation_task_template = PromptTemplate(generation_task)
        chunk_texts = [item['chunk_text'] for item in original_doc_splits]
        response = await llm.acomplete(
            generation_task_template.format(
                target_team=team,
                text_chunks=chunk_texts,
                sections=sections,
                example_structure=example_structure
            )
        )
        result_json = string_to_json(response.text)   
        await context.set("original_doc_splits", original_doc_splits)
        return SearchDocsEvent(search_results=result_json)
    else:
        await context.set("original_doc_splits", original_doc_splits)
        return SearchDocsEvent(search_results=[])
