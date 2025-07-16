from app.core.prompt_manager import create_prompt_manager
from app.project_charter.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.project_charter.services.workflow.events.search_docs_event import SearchDocsEvent
from app.project_charter.services.workflow.events.wp_teamtasks_event import (
    WPTeamTasksEvent,
)
from llama_index.core import PromptTemplate


prompt_manager = create_prompt_manager()


async def generate_charter(
    workflow, event: SearchDocsEvent | EYIPTemplatesEvent, context, llm
) -> WPTeamTasksEvent | None:
    """
    Generate team tasks based on input data. This input data is:
        - If no documents are provided, only EY IP data is used.
        - If documents are provided, the project outline and team
          data is extracted from them and used in addition to the EY IP data.
    """

    data = context.collect_events(event, [SearchDocsEvent, EYIPTemplatesEvent]) # Collect both events

    if data is None:
        return None

    search_docs, sql_result = data
    example_structure = prompt_manager.get_prompt_sync(
        agent="project_charter",
        key="example_charter_structure")
    generate_charter = prompt_manager.get_prompt_sync(
        agent="project_charter",
        key="charter_generation")

    project_details = context.data.get(
        "project_details", {}
    )  # Use a default empty dict if None

    generate_charter_template = PromptTemplate(generate_charter)
    response = await llm.acomplete(
        generate_charter_template.format(
            target_team=context.data.get("team"),
            sections=context.data.get("sections"),
            eyip_adapted_data=sql_result,
            search_results=search_docs,
            sector=project_details.sector,
            subSector=project_details.subSector,
            example_structure=example_structure,
            transaction_type=project_details.transactionType,
            sections_and_definitions=context.data.get("sections_and_definitions"),
        ),
    )      
    return WPTeamTasksEvent(result=response.text)
