import pandas as pd
from app.core.prompt_manager import create_prompt_manager
from app.core.program_office_api import ProgramOfficeResponse
from app.status_report.services.workflow.events.general_results import GeneralResults
from app.status_report.services.workflow.events.overall_status import OverallStatus
from llama_index.core.workflow import Context, StopEvent


status_rules = {"Behind Schedule":{"Behind Schedule":0.2},
                "At Risk":{"Behind Schedule":0.1,"At Risk":0.3}}
prompt_manager = create_prompt_manager()


def calcute_score_per_status(data):
    df = pd.DataFrame.from_dict(data)
    count_per_status = df.StatusTitle.value_counts(normalize=True).to_dict()
    return count_per_status

def check_status(status, response):
    final_status = "On Track"
    for rule in status_rules[status].keys():
            if rule in response:
                if response[rule]>=status_rules[status][rule]:
                    final_status = status
                    break
    return final_status


async def overall_status(
    workflow, ctx: Context, ev: OverallStatus
) -> GeneralResults | StopEvent:
    response = calcute_score_per_status(ev.sql_results)
    if len(response) == 0:
        final_status = None
        return StopEvent(result="No overall status found")

    final_status = "On Track"

    for key in status_rules.keys():
         if final_status=="On Track":
              final_status = check_status(key,response)
        

    status_id: ProgramOfficeResponse = await workflow.program_office.run_sql(
        sql_query=prompt_manager.get_prompt_sync(
            agent="status_report",
            key="status_id_query"
        ).replace("{{StatusTitle}}", final_status)
    )

    status: str = "200"
    if status_id is None and final_status is None:
        status = "204"

    source_name = f"project-status-{workflow.sections['OVERALL_STATUS']}"
    source = {
        "sourceName": source_name,
        "content": {
            "id": str(status_id.data[0]["Id"]),
            "title": final_status,
        },
        "status": status,
        "citingSources": ev.citing_sources
    }
    return GeneralResults(
        source=source,
    )
