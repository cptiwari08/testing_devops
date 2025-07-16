import pandas as pd
from app.tsa_generator.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.tsa_generator.services.workflow.events.good_sql_event import GoodSQLEvent
from tenacity import retry, stop_after_attempt, wait_fixed


@retry(stop=stop_after_attempt(3), wait=wait_fixed(3))
async def validate_sql(event: GeneratedSQLEvent, context) -> GoodSQLEvent:
    """Validate the SQL generated over program office"""
    query = event.sql_generated
    sql_params = event.query_params
    sql_object = event.sql_object
    df_results = await sql_object.execute_query(query, sql_params)
    df = pd.DataFrame(df_results)
    target_columns = ["title","serviceinscopedescription"]
    for col in target_columns:
        if col not in df.columns:
            df[col] = None          
    result = df[target_columns]
    await context.set("ey_ip_templates", result)

    return GoodSQLEvent(sql_output=result)
