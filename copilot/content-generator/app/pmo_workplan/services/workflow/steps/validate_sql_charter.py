import pandas as pd
from app.core.program_office_api import ProgramOfficeResponse
from app.pmo_workplan.services.workflow.events.search_team_event import SearchTeamEvent
from app.pmo_workplan.services.workflow.events.good_sql_pmo_event import GoodSQLPMOEvent
from app.core.prompt_manager import create_prompt_manager
from tenacity import retry, stop_after_attempt, wait_fixed


prompt_manager = create_prompt_manager()


@retry(stop=stop_after_attempt(3), wait=wait_fixed(3))
async def validate_sql_charter(workflow, event: SearchTeamEvent, context, logger) -> GoodSQLPMOEvent:
    """Validate the SQL generated over program office"""
    target_team = await context.get("team")
    logger.info(f"Initialized step validate_sql_charter for {target_team}")
    team_id = await context.get("team_id")    
    project_charter_ids = await context.get("project_charters")    
    project_charter_ids_str = ",".join(str(i) for i in project_charter_ids) if project_charter_ids else "NULL"

    query_templates = prompt_manager.get_prompt_sync(
        agent="pmo_workplan",
        key="project_charter_query",
        prompt_parameters={"project_team_id": team_id,
                            "project_charter_ids": project_charter_ids_str},
    )
    response_pmo: ProgramOfficeResponse = await workflow.program_office.run_sql(query_templates.rstrip('\n')
    )
    data = response_pmo.data
    if data:
        pd_data = pd.DataFrame(data)
        pd_data = pd_data.groupby('Category')['Title'].apply(list).to_dict()
    else:
        pd_data = {}
    logger.info(f"Finalized step validate_sql_charter for {target_team}")
    return GoodSQLPMOEvent(sql_output=pd_data)
