from app.core.program_office_api import ProgramOfficeResponse
from app.status_report.services.workflow.events.overall_status_results import (
    OverallStatusResults,
)
from app.status_report.services.workflow.events.overall_status_run_sql import (
    OverallStatusRunSQL,
)
from llama_index.core.workflow import Context


async def overall_status_run_sql(
    workflow,
    ctx: Context,
    ev: OverallStatusRunSQL,
) -> OverallStatusResults:
    query_and_rules = ev.query_and_rules
    table = ev.table
    query = (
        query_and_rules["sqlQuery"]
        .replace("\n", "")
        .replace("{{ProjectTeam}}", ev.project_team)
        .replace("{{periodStartDate}}", "'"+str(ev.start_date)+"'")
        .replace("{{periodEndDate}}", "'"+str(ev.end_date)+"'")
    )
    response: ProgramOfficeResponse = await workflow.program_office.run_sql(query)
    output = " \n".join(str(response_item) for response_item in response.data[:100])
    source_value = {
        "tableName": table,
        "rowCount": len(response.data),
        "sqlQuery": query,
        "status": response.status_code,
    }
    return OverallStatusResults(
        project_team=ev.project_team,
        source=workflow.sections["OVERALL_STATUS"],
        response=output,
        rules=query_and_rules["description"],
        source_value=source_value,
        table=table,
    )
