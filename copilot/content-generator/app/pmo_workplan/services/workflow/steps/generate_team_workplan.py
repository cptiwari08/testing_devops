from app.pmo_workplan.services.workflow.events.wp_timeline_event import (
    WPTeamTimelineEvent)
from app.pmo_workplan.utils import format_output_json, transform_data_nested_structure
from llama_index.core.workflow import StopEvent


async def generate_team_workplan(event: WPTeamTimelineEvent, context, logger) -> StopEvent:
    target_team = await context.get("team")
    logger.info(f"Initialized step generate_team_workplan for {target_team}")
    result = event.result
    id: int = context.data.get("workplan_params", None).get("id", None)
    title: str = context.data.get("original_team", None)
    project_docs = await context.get("original_doc_splits", None)
    eyip_template = await context.get("eyip_template", None)
    sql_query = await context.get("sql_query", None)
    sql_okay = await context.get("sql_okay", None)
    templates_dict = {"eyip": eyip_template, 
                      "sql_query": sql_query,
                      "sql_okay": sql_okay}
    output = format_output_json(id, title, result, project_docs, templates_dict)
    logger.info(f"Finalized step generate_team_workplan for {target_team}")
    return StopEvent(result=output)