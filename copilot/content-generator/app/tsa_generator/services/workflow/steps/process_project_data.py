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
    pass