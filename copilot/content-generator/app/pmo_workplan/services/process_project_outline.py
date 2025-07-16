from llama_index.core import PromptTemplate

from app.core.utils import clean_and_load_json
from app.core.prompt_manager import create_prompt_manager


async def process_project_outline(
    llm,
    goal_elements,
    objective_elements,
    project_description_elements,
    success_factors_elements,
    milestone_elements,
    project_context,
) -> str:

    prompt_manager = create_prompt_manager()
    enrich_prj_outline = prompt_manager.get_prompt_sync(
       agent="pmo_workplan",
       key="project_outline_enrichment")

    enrich_prj_outline_template = PromptTemplate(enrich_prj_outline)
    response = await llm.acomplete(
        enrich_prj_outline_template.format(
            goal_elements=goal_elements,
            objective_elements=objective_elements,
            project_description_elements=project_description_elements,
            success_factors_elements=success_factors_elements,
            milestone_elements=milestone_elements,
            project_context=project_context.get('value', {}),
        )
    )

    return clean_and_load_json(response.text)
