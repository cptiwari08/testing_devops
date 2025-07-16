import os, asyncio, openai
from functools import partial
from app.op_model.services.workflow.events.search_docs_event import SearchDocsEvent
from app.op_model.services.workflow.events.project_docs_event import ProjectDocsEvent
from llama_index.core.workflow import Context, Workflow
from llama_index.core import PromptTemplate
from app.core.prompt_manager import create_prompt_manager

prompt_manager = create_prompt_manager()
llm_sem = asyncio.Semaphore(25)
# These calls in this process normally take well under 10 seconds, so the timeout of 20 (default) should be sufficient. 
# Semaphore values is pretty high because the calls are short and in bursts, and we don't want to slow the process down.
log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "


async def search_project_docs(
    workflow: Workflow, event: ProjectDocsEvent, context: Context
) -> SearchDocsEvent | None:
    """
    1. Pull docs from workflow/context.
    2. Ask the LLM which chunks are useful – in parallel but never more than LLM_CONCURRENCY at once.
    3. Return the selected chunks and stash them in the context.
    """
    logger, llm = workflow.logger, workflow.llm

    documents = await context.get("project_doc")

    async def classify_table(chunk: dict) -> dict | None:
        """
        Classify a chunk of text to determine if it contains useful tabular data.
        Args:
            chunk (dict): A dictionary containing the text chunk to classify.
        Returns:
            dict: The original chunk if it is classified as useful, None otherwise.
        """
        global log_mes_base
        async with llm_sem:
            try:
                filter_table_prompt = filter_table_tmpl.format(content=chunk.get("chunk_text", ""))
                response = await llm.acomplete(filter_table_prompt)
                response_text = response.text 
            except openai.APITimeoutError as e:
                logger.error(f"{log_mes_base} API Timeout Error occurred while classifying table. Skipping search result. Prompt:\n {filter_table_prompt}")
                response_text = None
            except Exception as e:  
                logger.error(f"{log_mes_base} Error: {str(e)},\n input: {chunk}")
                response_text = None

        return chunk if "useful" in response_text else None

    # Search for chunks with "Tabular data :" in their text
    filters = f"search.in(external_document_uuid, '{','.join(map(str, documents))}')"
    original_doc_chunks = await workflow.ai_search.perform_full_text_query(
        search_text='chunk_text:"Tabular data :"',
        filters=filters,
    )

    # Prepare prompt 
    tmpl_str = await asyncio.to_thread(
        partial(prompt_manager.get_prompt_sync,
                agent="op_model",
                key="filter_project_docs_table")
    )
    filter_table_tmpl = PromptTemplate(tmpl_str)

    # LLM classification in parallel, bounded by semaphore 
    selected_chunks = []
    async with asyncio.TaskGroup() as tg:
        classification_tasks = [tg.create_task(classify_table(table_chunk)) for table_chunk in original_doc_chunks]

    selected_chunks = [task.result() for task in classification_tasks if task.result() is not None]

    logger.warning(
        f"{log_mes_base}Filtered {len(original_doc_chunks)} original chunks down to { len(selected_chunks)} useful ones"
    )

    await context.set("original_doc_splits", selected_chunks)
    return selected_chunks
