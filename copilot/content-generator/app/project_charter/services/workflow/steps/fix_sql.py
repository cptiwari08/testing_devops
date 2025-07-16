from app.project_charter.services.workflow.events.bad_sql_event import BadSQLEvent
from app.project_charter.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.project_charter.services.workflow.events.good_sql_event import GoodSQLEvent

async def fix_sql(workflow, event: BadSQLEvent, context) -> GeneratedSQLEvent | GoodSQLEvent | None:
    """Generate SQL based on the project teams"""
    sql = "select * from ProjectCharterDetails"
    return GeneratedSQLEvent(
        sql_generated=sql, query_params={}, sql_object=workflow.asset_manager
    )
