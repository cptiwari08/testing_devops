from app.pmo_workplan.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.pmo_workplan.services.workflow.events.search_team_event import SearchTeamEvent


async def generate_sql_for_teamtasks(
    workflow, event: SearchTeamEvent, context
) -> GeneratedSQLEvent:
    """Generate SQL based on the project teams"""
    await context.set("retry_count", 0)
    await context.set("project_team", event.project_team)
    params = await context.get("workplan_params")
    sql_params = params.copy()
    sql_params |= {"team": event.project_team}
    param_sql = workflow.asset_manager
    query, query_params, query_replaced = param_sql.build_query(sql_params)
    await context.set("sql_query", query_replaced)
    # Execute the query and get the results
    return GeneratedSQLEvent(
        sql_generated=query, query_params=query_params, sql_object=param_sql
    )
