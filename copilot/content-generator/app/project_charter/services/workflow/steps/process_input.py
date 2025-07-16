from app.core.utils import clean_and_load_json
from app.core.prompt_manager import create_prompt_manager
from app.core.program_office_api import ProgramOfficeResponse
from app.project_charter.services.workflow.events.search_team_event import SearchTeamEvent
from llama_index.core import PromptTemplate
from llama_index.core.workflow import Context, StartEvent
import json

prompt_manager = create_prompt_manager()
async def process_input(
    workflow, event: StartEvent, context: Context, llm
) -> SearchTeamEvent:
    
    # TODO
    generation_task = prompt_manager.get_prompt_sync(
        agent="project_charter",
        key="process_input",
    )
    await context.set("retry", False)
    await context.set("team", event.team)
    await context.set("project_details", event.project_details)
    await context.set("project_doc", event["project_doc"])  
    if await context.get("project_doc") == []:
        await context.set("original_doc_splits", None)
    await context.set("sections", event["sections"])       
    
    # fetch section's description from Program Office
    sections_list = event["sections"]        
    conditions = " OR ".join([f"[key] LIKE '%{section}%'" for section in sections_list])
    query_sec_desc = f"""SELECT * FROM ProjectCharterDetailCategories WHERE {conditions}"""
    tables = ["ProjectCharterDetailCategories"]    

    try:
        response: ProgramOfficeResponse = await workflow.program_office.run_sql(sql_query=query_sec_desc, tables=tables)
        result = [{"section": i.get("Title", None),"description": i.get("Description", None)} for i in response.data]
        await context.set("sections_and_definitions", json.dumps(result))
    except:        
        await context.set("sections_and_definitions", {})

    #Include EY IP ids to include in SQL
    await context.set("eyip_ids", event.eyip_ids)
    if await context.get("eyip_ids") == []:
        await context.set("ey_ip_templates", None)
    target_team = event.team
    query = "SELECT DISTINCT projectcharter as projectteam FROM ProjectCharterDetails"
    result = await workflow.asset_manager.execute_query(query)
    team_list = [row[0] for row in result]

    geration_task_template = PromptTemplate(generation_task)
    response = await llm.acomplete(
        geration_task_template.format(target=target_team, inputs=str(team_list)),
        max_tokens=50,
    )
    projectteam_value = clean_and_load_json(response.text)
    return SearchTeamEvent(project_team=projectteam_value)
