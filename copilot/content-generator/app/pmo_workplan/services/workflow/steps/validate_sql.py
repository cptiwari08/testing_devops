import pandas as pd
from app.pmo_workplan.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.pmo_workplan.services.workflow.events.good_sql_event import GoodSQLEvent
from tenacity import retry, stop_after_attempt, wait_fixed


@retry(stop=stop_after_attempt(3), wait=wait_fixed(3))
async def validate_sql(event: GeneratedSQLEvent, context, logger) -> GoodSQLEvent:
    """Validate the SQL generated over program office"""
    target_team = await context.get("team")
    logger.info(f"Initialized step validate_sql for {target_team}")
    query = event.sql_generated
    sql_params = event.query_params
    sql_object = event.sql_object
    df_results = await sql_object.execute_query(query, sql_params)
    result = pd.DataFrame(
        df_results,
        columns=["title", "parenttask", "projectteam", "workplantasktype"],
    )
    logger.info(f"Finalized step validate_sql for {target_team}")
    return GoodSQLEvent(sql_output=result)

