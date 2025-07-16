
from app.op_model.services.workflow.events.search_docs_event import SearchDocsEvent
from app.op_model.services.workflow.events.postprocess_project_docs import PostprocessProjectDocsEvent
from app.core.azure_llm_models import AzureSyncLLM
from app.core.prompt_manager import create_prompt_manager
from llama_index.core.workflow import Workflow, Context
from llama_index.core import PromptTemplate
import openai
import re, asyncio, functools, os
from app.op_model.utils import loads_clean_json_response

prompt_manager = create_prompt_manager()
get_prompt = functools.partial(prompt_manager.get_prompt_sync, agent="op_model")
llm_sem = asyncio.Semaphore(25)
# llm_sem will limit the number of concurrent LLM calls. There will always only be as many LLM calls running at the same time
# These calls in this process normally take well under 10 seconds, so the timeout of 20 should be sufficient. 
# Semaphore values is pretty high because the calls are short and in bursts, and we don't want to slow the process down.

log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "

async def postprocess_project_docs(
        workflow: Workflow, event: SearchDocsEvent, context: Context
        ) -> PostprocessProjectDocsEvent:
    # SHOULD NEVER MAKE IT HERE IF SELECTED IS EMPTY

    async def clean_table(llm: AzureSyncLLM, table_info_var: dict) -> str | None:
        # llm_sem manages the number of concurrent LLM calls. 
        async with llm_sem:  
            try: # back-pressure
                clean_prompt = clean_template.format(
                        table_title=table_info_var["title"] or "not available",
                        table_text=table_info_var["table"] or "a placeholder"
                        )
                resp = await llm.acomplete(
                    clean_prompt,
                    timeout=20   
                )   
            except openai.APITimeoutError as e:
                logger.error(f"{log_mes_base} API Timeout Error occurred while cleaning table. Skipping search result. Prompt:\n {clean_prompt}")
                resp = None
            except Exception as e:  
                logger.error(f"{log_mes_base} Error: {str(e)},\n input: {table_info_var}")
                resp = None
            return resp
            
    async def extract_json(completion_json: dict) -> str | None:
        # llm_sem manages the number of concurrent LLM calls. 
        async with llm_sem:   # back-pressure
            try:
                extraction_prompt = extract_template.format(
                        search_query=search_keys,
                        parent_function=completion_json.get("parent_function",""),
                        table_text= completion_json.get("markdown_table",""),
                        output_structure=output_structure.template,
                    )
                resp = await llm.acomplete(
                    extraction_prompt,
                    timeout = 20
                )  
            except openai.APITimeoutError as e:
                logger.error(f"{log_mes_base} API Timeout Error occurred while extracting table.Prompt: +{extraction_prompt}")
                resp = None
            except Exception as e:  
                logger.error(f"{log_mes_base} Error: {str(e)},\n input: {extraction_prompt}")
                resp = None
            return resp

    # Set Up
    logger, llm = workflow.logger, workflow.llm
    project_details = context.data["project_details"]
    search_keys = f"{project_details.sector}-{project_details.subSector}"
    selected = event.search_result
    output_structure, clean_template, extract_template = await fetch_table_prompts() 

    # Separate table and title from seleted search results 
    table_infos = [get_table_and_title(chunk["chunk_text"]) for chunk in selected]
    if table_infos is None or len(table_infos) == 0:
        logger.warning(f"{log_mes_base} No tables found in the selected chunks.")
        table_infos = []
    
    # LLM calls to makes sense of the info and prepare it for extraction
    table_cleanup_tasks = []
    async with asyncio.TaskGroup() as tg:
        table_cleanup_tasks = [tg.create_task(clean_table(llm, table_info)) for table_info in table_infos]
    table_cleanup_responses = [cleanup_task.result() for cleanup_task in table_cleanup_tasks if cleanup_task.result() is not None]
    
    clean_mkdown_tables = []
    for response in table_cleanup_responses:
        try:
            clean_mkdown_tables.append(loads_clean_json_response(response))
        except Exception as e:
            logger.error(f"{log_mes_base} Table cleaning response could not be parsed. Error {e}\n")
            logger.error(f"{log_mes_base} Problematic JSON: {response}")

    # LLM calls to extract the JSON hierarchy from the touched up tables
    async with asyncio.TaskGroup() as tg:
        extraction_tasks = [tg.create_task(extract_json(markdown_table)) for markdown_table in clean_mkdown_tables]
    extraction_results = [task.result() for task in extraction_tasks]

    # Parse and collect per table jsons
    final_json_list = []
    for response in extraction_results:
        try:
            final_json = loads_clean_json_response(response)
            final_json_list.append(final_json)
        except Exception as e:
            logger.error(f"{log_mes_base} Table cleaning response could not be parsed. Error: {e}\n")
            logger.error(f"{log_mes_base} Problematic JSON: {response}")
        
    logger.warning(f"{log_mes_base} {len(final_json_list)} out of {len(selected)} tables processed successfully.")

    # Make sure there are no major duplicated in the tables
    bf_list = get_unique_business_function_list(final_json_list)
    logger.info(f"{log_mes_base} Complete list of {len(bf_list)} business functions: {bf_list}")

    for item in bf_list:
        business_function_key = item["business_function"]
        item["process_groups"] = collect_process_groups_by_business_function(final_json_list, business_function_key)

    return PostprocessProjectDocsEvent(result=final_json_list)


async def fetch_table_prompts() -> tuple[PromptTemplate, PromptTemplate, PromptTemplate]:
    """
    Fetch prompts for table processing concurrently.
    
    Returns:
        tuple: A tuple containing output structure, clean table, and extract table prompt templates
    """
    # Fetch prompts concurrently
    output_structure, clean_table_prompt, extract_table_prompt = await asyncio.gather(
        *(asyncio.to_thread(get_prompt, key=k) for k in (
            "project_docs_output_structure",
            "postproc_docs_clean_table_and_get_parent",
            "postproc_docs_extract_table"
        ))
    )
    output_structure = PromptTemplate(output_structure)
    clean_template = PromptTemplate(clean_table_prompt)
    extract_template = PromptTemplate(extract_table_prompt)

    return output_structure, clean_template, extract_template

def get_unique_business_function_list(data):
    business_functions = []
    for item in data:
        if "business_function" in item:
            business_functions.append(item.get("business_function"))
    business_functions = list(set(business_functions))
    initialized_dict = [{"business_function": bf, "process_groups": []} for bf in business_functions]
    return initialized_dict

def collect_process_groups_by_business_function(data: list[dict], business_function_key: str) -> list[dict]:
    """
    Collects and merges process groups for a specific business function.
    
    Args:
        data: List of dictionaries containing business function data
        business_function_key: The business function to filter by
        
    Returns:
        A list of dictionaries containing merged process groups
    """
    result: list[dict] = []
    for item in data:
        if item.get("business_function") == business_function_key:
            for process_group in item.get("process_groups", []):
                existing_group = next((pg for pg in result if pg["process_group"] == process_group["process_group"]), None)
                if existing_group:
                    # Merge unique elements for existing group
                    existing_group["processes"] = list(set(existing_group["processes"] + process_group.get("processes", [])))
                    existing_group["systems"] = list(set(existing_group["systems"] + process_group.get("systems", [])))
                    existing_group["tpas"] = list(set(existing_group["tpas"] + process_group.get("tpas", [])))
                else:
                    # Add new process group
                    result.append({
                        "process_group": process_group["process_group"],
                        "processes": process_group.get("processes", []),
                        "systems": process_group.get("systems", []),
                        "tpas": process_group.get("tpas", [])
                    })
    return result

def get_table_and_title(input_string: str):
    """
    Extracts the table content, title of the table, and page number from the input string using regex.

    Args:
        input_string (str): The input string containing the table, title, and page number.

    Returns:
        dict   : A dictionary with keys 'table', 'title', and 'page_number'.
    """
    table_label = "Tabular data :"
    title_label = ", Title of table :"
    page_number_label = ", END for Page Number -"
    input_string = input_string.encode().decode('unicode_escape')
    table_regex = rf"{re.escape(table_label)}(.*?){re.escape(title_label)}"
    title_regex = rf"{re.escape(title_label)}(.*?){re.escape(page_number_label)}"
    page_number_regex = rf"{re.escape(page_number_label)} (\d+)"

    table_match = re.search(table_regex, input_string, re.DOTALL)
    table = table_match.group(1).strip() if table_match else ""
    
    page_number_match = re.search(page_number_regex, input_string)
    page_number = page_number_match.group(1).strip() if page_number_match else ""

    title_match = re.search(title_regex, input_string, re.DOTALL)
    title = title_match.group(1).strip() if title_match else ""
    if len(title) > 80: #value chosen to contain issues and give ample room for long titles
        title_regex = rf"{re.escape(title_label)}(.*?)(\s#\s+|\n+)"
        title_match = re.search(title_regex, input_string, re.DOTALL)
        title = title_match.group(1).strip() if title_match else ""


    return {
        "table": table,
        "title": title,
        "page_number": page_number
    }
