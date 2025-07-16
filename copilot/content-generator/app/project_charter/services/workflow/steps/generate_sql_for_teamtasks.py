from app.project_charter.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.project_charter.services.workflow.events.search_team_event import SearchTeamEvent


async def generate_sql_for_teamtasks(
    workflow, event: SearchTeamEvent, context
) -> GeneratedSQLEvent:
    """Generate SQL based on the project teams"""    
    await context.set("project_team", event.project_team)    
    params = {
        "project_details": await context.get("project_details"),
        "eyip_ids": await context.get("eyip_ids") ##Inclide ey ip ids in the search
    }
    sql_params = params.copy()
    sql_params |= {"team": event.project_team}
    param_sql = workflow.asset_manager
    query, query_params = param_sql.build_query(sql_params)
    await context.set("sql_query", query)
    return GeneratedSQLEvent(
        sql_generated=query, query_params=query_params, sql_object=param_sql
    )
