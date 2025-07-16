import json

from app.core.prompt_manager import create_prompt_manager
from app.project_charter.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.project_charter.services.workflow.events.good_sql_event import GoodSQLEvent
from app.project_charter.utils import flatten_team_documents
from llama_index.core import PromptTemplate
from llama_index.core.workflow import Context

import pandas as pd


prompt_manager = create_prompt_manager()


async def process_eyip_templates(
    workflow, event: GoodSQLEvent, context: Context, llm
) -> EYIPTemplatesEvent:

    example_structure = prompt_manager.get_prompt_sync(
        agent="project_charter",
        key="example_charter_structure",
    )

    generation_task = prompt_manager.get_prompt_sync(
        agent="project_charter",
        key="ey_ip_data_consolidation_charter",
    )
    generation_task_template = PromptTemplate(generation_task)

    eyip_templates = event.sql_output.to_dict(orient="records")

    team = context.data.get("team").title
    details = context.data.get("project_details")
    sector = details.sector
    sub_sector = details.subSector
    transaction_type = details.transactionType
    sections_list = context.data.get("sections")
    sections = ', '.join(sections_list)

    sqlQuery = await context.get("sql_query")
    eyip_flat = eyip_templates

    response = await llm.acomplete(
        generation_task_template.format(
            sector=sector,
            subSector=sub_sector,
            target_team=team,
            ey_ip_data=eyip_flat,
            transaction_type=transaction_type,
            sections=sections,
            example_structure=example_structure
        )
    )

    ey_ip_templates = {
        'eyip': eyip_templates,
        'sql_query': sqlQuery
    }

    await context.set("ey_ip_templates", ey_ip_templates)

    return EYIPTemplatesEvent(result=response.text)
