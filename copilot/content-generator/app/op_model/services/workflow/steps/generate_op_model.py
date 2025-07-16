from app.core.prompt_manager import create_prompt_manager
from app.op_model.services.workflow.events.eyip_data_event import (
    EYIPDataEvent,
)
from app.op_model.services.workflow.events.no_data_event import NoDataEvent
from app.op_model.services.workflow.events.postprocess_project_docs import PostprocessProjectDocsEvent
from app.op_model.services.workflow.events.om_process_event import (
    OMProcessEvent,
)
from llama_index.core.workflow import Workflow, Context
from llama_index.core import PromptTemplate
import re, os, json
import openai
import asyncio
from app.op_model.utils import loads_clean_json_response, clean_json_response
 
prompt_manager = create_prompt_manager()
llm_sem = asyncio.Semaphore(25)

async def generate_op_model(
    workflow: Workflow, event: PostprocessProjectDocsEvent | EYIPDataEvent | NoDataEvent, context: Context
) -> OMProcessEvent:
    """
    Generate processes based on input data. This input data is:
    - EY IP data (if available)
    - Project documents (if available)
    The function will:
    1. Collect the necessary events based on the context.
    2. If EY IP data is available, it will be used to generate the
       operating model.
    3. If project documents are provided, they will be processed to
       extract the project outline and team data.
    4. If neither EY IP data or project documents are available,
       it will work without over a general context.
    The function will return an OMProcessEvent containing the generated
    operating model.
    """
    log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "
    llm = workflow.llm
    logger = workflow.logger

    # Have all dependent steps finished?
    events_to_collect = await context.get("event_to_collect", [])
    if not events_to_collect:
        raise ValueError("No events to collect specified in context.")
    data = context.collect_events(event, events_to_collect)
    # No, so exit
    if data is None: 
        return None 
    
    # Yes, so collect available data
    search_docs_result = []
    eyip_result = []
    for event in data:
        if isinstance(event, PostprocessProjectDocsEvent):
            search_docs_result = event.result
        if isinstance(event, EYIPDataEvent):
            eyip_result = event.result


    # Organize business functions we'll generate for
    # Fetch business functions from all sources
    default_business_functions = await context.get("default_business_functions", [])
    pd_business_functions = [entry.get("business_function") for entry in search_docs_result]
    ey_ip_busienss_functions = [entry.get("process_lvl_1") for entry in eyip_result]
    # Remove duplicates with LLM help
    unify_bf_template = prompt_manager.get_prompt_sync(
        agent="op_model",
        key="select_business_functions"
        )
    
    unify_bf_prompt = unify_bf_template.format(
                eyip_data= ey_ip_busienss_functions,
                project_docs_data= pd_business_functions,
                default_data= default_business_functions
            )
    response = await safe_llm_call(unify_bf_prompt, llm, 20, logger, log_mes_base)

    logger.info(f"{log_mes_base} BF alignment prompt:\n {unify_bf_prompt}")
    logger.info(f"{log_mes_base} BF alignment response:\n {response}")
    # Clean the response and parse it as JSON
   
    try:
        bf_dict = loads_clean_json_response(response)
        concatenated_bfs = list(bf_dict.keys())
        logger.warning(f"{log_mes_base} Business function grouping: {bf_dict}")
    except Exception as e:
        logger.error(f"{log_mes_base} Error parsing business functions JSON response: {e}")
        logger.error(f"{log_mes_base} Failed to interpred reponse:\n {clean_response}")   
        logger.error(f"{log_mes_base} Defaulting to standard business function list")   
        concatenated_bfs = default_business_functions     


    responses = []
    prompts = []
    project_details = context.data.get( "project_details", {})
    example_structure_template = prompt_manager.get_prompt_sync(
        agent="op_model",
        key="example_op_model_structure")

    for business_function in concatenated_bfs:
        logger.info(f"{log_mes_base} Processing business function: {business_function}")
        # retrieve business function data from project docs and eyip
        bf_project_data = [pjd_dict for pjd_dict in search_docs_result if clean_spaces(pjd_dict.get("business_function")) in bf_dict[business_function] ]
        bf_eyip_data = [eyip_dict for eyip_dict in eyip_result if clean_spaces(eyip_dict.get("process_lvl_1")) in bf_dict[business_function] ]
        logger.debug(f"{log_mes_base} Matching project docs bf: {bf_project_data} \n Matching eyip bf: {bf_eyip_data}")
        
        # Generate the operating model based on the available data
        if bf_project_data and bf_eyip_data:
            generate_op_model_template = prompt_manager.get_prompt_sync(
                agent="op_model",
                key="generate_op_model_all_datasources"
                )
            example_structure = example_structure_template.format(
                business_function=business_function
            )
            generate_op_model_template = PromptTemplate(generate_op_model_template)
            generate_op_model_prompt = generate_op_model_template.format(
                sector=project_details.sector,
                subSector=project_details.subSector,
                transaction_type=project_details.transactionType,
                example_structure=example_structure,
                project_docs_data=bf_project_data,
                eyip_data=bf_eyip_data,
                business_function=business_function
            )
        
        elif bf_project_data and not bf_eyip_data:
            generate_op_model_template = prompt_manager.get_prompt_sync(
                agent="op_model",
                key="generate_op_model_single_datasource"
                )
            generate_op_model_template = PromptTemplate(generate_op_model_template)
            example_structure = example_structure_template.format(
                business_function=business_function
            )
            generate_op_model_prompt = generate_op_model_template.format(
                sector=project_details.sector,
                subSector=project_details.subSector,
                transaction_type=project_details.transactionType,
                example_structure=example_structure,
                relevant_data = bf_project_data,
                business_function=business_function
            )

        elif not bf_project_data and bf_eyip_data:
            generate_op_model_template = prompt_manager.get_prompt_sync(
                agent="op_model",
                key="generate_op_model_single_datasource"
                )
            generate_op_model_template = PromptTemplate(generate_op_model_template)
            example_structure = example_structure_template.format(
                business_function=business_function
            )
            generate_op_model_prompt = generate_op_model_template.format(
                sector=project_details.sector,
                subSector=project_details.subSector,
                transaction_type=project_details.transactionType,
                example_structure=example_structure,
                relevant_data = bf_eyip_data,
                business_function=business_function
            )

        else:
            generate_op_model_template = prompt_manager.get_prompt_sync(
                agent="op_model",
                key="generate_op_model_no_datasource"
                )
            generate_op_model_template = PromptTemplate(generate_op_model_template)
            example_structure = example_structure_template.format(
                business_function=business_function
            )
            generate_op_model_prompt = generate_op_model_template.format(
                sector=project_details.sector,
                subSector=project_details.subSector,
                transaction_type=project_details.transactionType,
                example_structure=example_structure,
                business_function=business_function
            )

        prompts.append((business_function, generate_op_model_prompt))
    
    # Concurrently execute LLM calls
    async with asyncio.TaskGroup() as tg:
        generation_tasks = [tg.create_task(safe_llm_call(prompt, llm, 60, logger, log_mes_base)) for  _,prompt in prompts]
    generation_results = [task.result() for task in generation_tasks]

    # Process results
    for (business_function, _), response in zip(prompts, generation_results):

        clean_response = clean_json_response(response)

        # If LLM actually returned a list of process_lvl_1
        count = clean_response.count("process_lvl_1")
        if count > 1:
            clean_response = '['+clean_response+']'
            logger.warning(f"{log_mes_base} 'process_lvl_1' appears {count} times in the response for business function '{business_function}'. Attempting to treat it as an array.")
        
        # Try to parse the response as JSON
        try:
            json_response = json.loads(clean_response)
        except Exception as e:
            logger.error(f"{log_mes_base} Error parsing business function {business_function} JSON response: {e}")
            logger.error(f"{log_mes_base} Failed to decode text: {clean_response} \n\n Defaulting to empty JSON response")
            json_response = {business_function: []}

        # If json_response is a list, wrap it in a dict with the business_function as key
        if isinstance(json_response, list):
            for item in json_response:
                responses.append((item, business_function))
                logger.warning(f"{log_mes_base} {business_function} was split into {len(json_response)}: {item}")
            responses.append((json_response, business_function))
        else:
            responses.append((json_response, business_function))

    return OMProcessEvent(result=responses)


def clean_spaces(text):
    """
    Clean up extra spaces in the text.
    """
    return re.sub(r'\s+', ' ', text).strip()


async def safe_llm_call(prompt, llm, timeout, logger, log_mes_base):
    # llm_sem manages the number of concurrent LLM calls. 
    async with llm_sem:   # back-pressure
        try:
            resp = await llm.acomplete(
                prompt,
                timeout=timeout
            )
        except openai.APITimeoutError as e:
            logger.error(f"{log_mes_base} API Timeout Error occurred. Prompt:\n {prompt}")
            resp = None
        except Exception as e:
            logger.error(f"{log_mes_base} Error: {str(e)},\n input: {prompt}")
            resp = None
        return resp

 