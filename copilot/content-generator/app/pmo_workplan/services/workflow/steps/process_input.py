from app.core.utils import clean_and_load_json
from app.core.prompt_manager import create_prompt_manager
from app.pmo_workplan.services.workflow.events.search_team_event import SearchTeamEvent
from llama_index.core import PromptTemplate
from llama_index.core.workflow import Context, StartEvent
from tenacity import retry, stop_after_attempt, wait_fixed


@retry(stop=stop_after_attempt(3), wait=wait_fixed(3))
async def process_input(
    workflow, event: StartEvent, context: Context, llm, logger
) -> SearchTeamEvent:
    target_team = event.value["team"]
    logger.info(f"Initialized step process_input for {target_team}")
    await context.set("teams", event.value["teams"])
    await context.set("project_outline", event.value["project_outline"])
    await context.set("document_ids", event.value["document_ids"])
    await context.set("original_team", event.value["team"])
    await context.set("team", event.value["team"])
    await context.set("team_id", event.value["id"])
    await context.set("project_details", event.value["project_details"])
    await context.set("duration_in_month", event.value["duration_in_month"])
    await context.set("workplan_params", event.value)
    await context.set("op_model_data", event.value["op_model_data"])  
    await context.set("project_charters", event.value["project_charters"])  
    mapped_teams = event.value["mapped_teams"]
    logger.info(f"Finalized step process_input for {target_team}")
    return SearchTeamEvent(project_team=mapped_teams)
