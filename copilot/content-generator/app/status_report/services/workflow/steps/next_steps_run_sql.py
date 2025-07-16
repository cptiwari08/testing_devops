from app.core.program_office_api import ProgramOfficeResponse
from app.status_report.services.workflow.events.next_steps_results import (
    NextStepsResults,
)
from app.status_report.services.workflow.events.next_steps_run_sql import (
    NextStepsRunSQL,
)
from llama_index.core.workflow import Context


async def next_steps_run_sql(workflow, ctx: Context, ev: NextStepsRunSQL) -> NextStepsResults:
    query_and_rules = ev.query_and_rules
    table = ev.table
    query = (
        query_and_rules["sqlQuery"]
        .replace("\n", "")
        .replace("{{ProjectTeam}}", ev.project_team)
        .replace("{{periodStartDate}}", "'"+str(ev.start_date)+"'")
        .replace("{{periodEndDate}}", "'"+str(ev.end_date)+"'")
    )
    response: ProgramOfficeResponse = await workflow.program_office.run_sql(sql_query=query)
    output = " \n".join(str(response_item) for response_item in response.data[:100])
    source_value = {
        "tableName": table,
        "rowCount": len(response.data),
        "sqlQuery": query,
        "status": response.status_code,
    }
    return NextStepsResults(
        project_team=ev.project_team,
        source=workflow.sections["NEXT_STEPS"],
        response=output,
        rules=query_and_rules["description"],
        source_value=source_value,
        table=table,
    )
