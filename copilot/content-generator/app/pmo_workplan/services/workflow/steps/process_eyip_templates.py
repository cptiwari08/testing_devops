import json

from app.core.prompt_manager import create_prompt_manager
from app.core.prompt_manager import create_prompt_manager
from app.pmo_workplan.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.pmo_workplan.services.workflow.events.good_sql_event import GoodSQLEvent
from app.pmo_workplan.services.workflow.events.good_sql_pmo_event import GoodSQLPMOEvent
from app.pmo_workplan.utils import flatten_team_documents
from llama_index.core import PromptTemplate
from llama_index.core.workflow import Context


prompt_manager = create_prompt_manager()


async def process_eyip_templates(
    workflow, event: GoodSQLEvent | GoodSQLPMOEvent, context: Context, llm
) -> EYIPTemplatesEvent | None:
    data = context.collect_events(event, [GoodSQLEvent, GoodSQLPMOEvent])

    if data is None:
        return None
    
    sql_output, project_charter_data = data

    example_structure = prompt_manager.get_prompt_sync(
        agent="pmo_workplan", key="example_task_structure")
    
    generation_task = prompt_manager.get_prompt_sync(
        agent="pmo_workplan", key="ey_ip_data_consolidation")
    
    generation_task_template = PromptTemplate(generation_task)

    eyip_templates = sql_output.sql_output.to_dict(orient="records")

    try:
        eyip_flat = flatten_team_documents(eyip_templates)
    except Exception:
        eyip_flat = eyip_templates


    # detect team level: 1 team, 2 subteam,
    classify_team = prompt_manager.get_prompt_sync(
        agent="pmo_workplan", key="detect_mismatch")
    classify_team_template = PromptTemplate(classify_team)
    team_level_classification = await llm.acomplete(
        classify_team_template.format(
            target_team=context.data.get("team"),
            eyip_project_teams= context.data['project_team']
        ),
    )
    # if there is a mismatch in team level, filter out subteam data
    if '1' in team_level_classification.text:
        # filter out subteam data
        filter_team_data = prompt_manager.get_prompt_sync(
            agent="pmo_workplan", key="filter_eyip_team_data")
        filter_team_data_template = PromptTemplate(filter_team_data)
        team_level_info = await llm.acomplete(
            filter_team_data_template.format(
                target_team=context.data.get("team"),
                eyip_data= eyip_flat,
                example_structure=example_structure
            ),
        )   
        eyip_flat = team_level_info.text

    await context.set("eyip_template", eyip_flat)

    # include or exclude based on payload 
    if not project_charter_data.sql_output:
        outline = await context.get("project_outline")
        if not outline:
            eyip_flat = json.dumps(eyip_flat)
            return EYIPTemplatesEvent(result=eyip_flat)  
    else:
        outline = project_charter_data.sql_output

    response = await llm.acomplete(
        generation_task_template.format(
            target_team=context.data.get("team"),
            project_outline=outline,
            ey_ip_data=eyip_flat,
            example_structure=example_structure,
        ),
    )

    return EYIPTemplatesEvent(result=response.text)
