from app.core.prompt_manager import create_prompt_manager
from app.op_model.services.workflow.events.search_docs_event import SearchDocsEvent
from app.op_model.services.workflow.events.eyip_data_event import EYIPDataEvent
from app.op_model.services.workflow.events.no_data_event import NoDataEvent
from app.op_model.services.workflow.events.eyip_event import EyIpEvent
from app.op_model.services.workflow.events.project_docs_event import ProjectDocsEvent
from llama_index.core.workflow import Context, StartEvent, Workflow
from app.core.program_office_api import ProgramOfficeResponse
import os
log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "

prompt_manager = create_prompt_manager()
async def process_input(
    workflow: Workflow, event: StartEvent, context: Context

) -> list[NoDataEvent | EyIpEvent | ProjectDocsEvent]:
    await context.set("retry", False)
    await context.set("project_details", event.project_details)
    await context.set("project_doc", event.project_doc)
    await context.set("business_entity", event.business_entity)
    await context.set("eyip_ids", event.eyip_ids)

    logger = workflow.logger
    
    # Fetch default business functions from Program Office
    business_functions_query = "SELECT [Title] FROM [dbo].[Functions]"
    program_office = workflow.program_office
    try:
        response: ProgramOfficeResponse = await program_office.run_sql(sql_query = business_functions_query)
        default_bf_list = [title_dict['Title'] for title_dict in response.data]
        await context.set("default_business_functions", default_bf_list)
    except:
        logger.error(f"{log_mes_base}Error fetching business functions from Program Office")
        await context.set("default_business_functions", ["HR", "Finance", "IT", "Operations", "Marketing", "Sales"])

    
    project_doc = event.project_doc or []
    eyip_ids = event.eyip_ids or []
    events_to_send = []
    events_to_collect = []

    if not project_doc and not eyip_ids:
        events_to_send.append( NoDataEvent() ) 
        events_to_collect.append( NoDataEvent )
    if eyip_ids:
        events_to_send.append( EyIpEvent() )
        events_to_collect.append( EYIPDataEvent )
    if project_doc:
        events_to_send.append( ProjectDocsEvent() )
        events_to_collect.append( SearchDocsEvent )
    
    await context.set("event_to_collect", events_to_collect)
 
    return events_to_send
