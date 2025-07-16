from app.status_report.services.workflow.events.general_results import GeneralResults
from llama_index.core.workflow import Context, StopEvent


def join_data(final_result):
    response = []
    for sources in final_result:
        response.append(sources.source)
    return {"response":response}


async def join_reports(workflow, ctx: Context, ev: GeneralResults) -> StopEvent | None:

    ready = ctx.collect_events(ev, [GeneralResults] * 4)
    workflow.logger.info("Events collected")
    if ready is not None:
        response = join_data(ready)
        workflow.logger.info("Response generated")
        return StopEvent(result=response)

    return None
