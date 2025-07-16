from app.core.prompt_manager import create_prompt_manager
from app.tsa_generator.services.workflow.events.get_title_event import (
    GetTitleEvent,
)
from app.tsa_generator.services.workflow.events.scope_generation_event import (
    ScopeGenerationEvent,
)
from llama_index.core import PromptTemplate


prompt_manager = create_prompt_manager()


async def generate_scope(
    workflow, event: GetTitleEvent, context, llm
) -> ScopeGenerationEvent | None:
    """
    Generate team tasks based on input data. This input data is:
        - If no documents are provided, only EY IP data is used.
        - If documents are provided, the project outline and team
          data is extracted from them and used in addition to the EY IP data.
    """

    generate_tsa = prompt_manager.get_prompt_sync(
        agent="tsa_generator",
        key="scope_generation")

    project_details = context.data.get(
        "project_details", {}
    )  # Use a default empty dict if None

    generate_tsa_template = PromptTemplate(generate_tsa)
    response = await llm.acomplete(
        generate_tsa_template.format(
            team=context.data.get("team"),
            sector=project_details.sector,
            sub_sector=project_details.subSector,
            transaction_type=project_details.transactionType,
            tsa_data=event.title,
        ),
    )      
    return ScopeGenerationEvent(result=response.text)
