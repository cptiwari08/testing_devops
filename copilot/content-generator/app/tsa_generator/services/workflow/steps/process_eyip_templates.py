import json

from app.core.prompt_manager import create_prompt_manager
from app.tsa_generator.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.tsa_generator.services.workflow.events.good_sql_event import GoodSQLEvent
from llama_index.core.workflow import Context

import pandas as pd


prompt_manager = create_prompt_manager()


async def process_eyip_templates(
    workflow, event: GoodSQLEvent, context: Context, llm
) -> EYIPTemplatesEvent:
    """Adapt SQL output to a json string."""
    eyip_templates = event.sql_output.to_dict(orient="records")
    sqlQuery = await context.get("sql_query")

    ey_ip_templates = {
        'eyip': eyip_templates,
        'sql_query': sqlQuery
    }

    await context.set("ey_ip_templates", ey_ip_templates)

    return EYIPTemplatesEvent(result=str(eyip_templates))
