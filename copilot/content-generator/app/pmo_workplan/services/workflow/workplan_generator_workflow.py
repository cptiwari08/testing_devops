from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.program_office_api import ProgramOffice
from app.core.redis_service import RedisService
from app.dependencies import Container
from app.pmo_workplan.schemas.workplan_output import WorkplanOutput
from app.core.ai_search import AISearch
from app.core.asset_manager import AssetManager
from app.pmo_workplan.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.pmo_workplan.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.pmo_workplan.services.workflow.events.good_sql_pmo_event import GoodSQLPMOEvent
from app.pmo_workplan.services.workflow.events.good_sql_event import GoodSQLEvent
from app.pmo_workplan.services.workflow.events.search_docs_event import SearchDocsEvent
from app.pmo_workplan.services.workflow.events.search_team_event import SearchTeamEvent
from app.pmo_workplan.services.workflow.events.wp_teamtasks_event import (
    WPTeamTasksEvent,
)
from app.pmo_workplan.services.workflow.events.wp_teamtasksom_event import (
    WPTeamTasksOMEvent,
) 
from app.pmo_workplan.services.workflow.events.wp_timeline_event import (
    WPTeamTimelineEvent)
from app.pmo_workplan.services.workflow.steps.generate_sql_for_teamtasks import (
    generate_sql_for_teamtasks,
)
from app.pmo_workplan.services.workflow.steps.generate_tasks import generate_tasks
from app.pmo_workplan.services.workflow.steps.add_timeline import add_timeline
from app.pmo_workplan.services.workflow.steps.add_timeline_op_model import add_timeline_op_model

from app.pmo_workplan.services.workflow.steps.generate_team_workplan import (
    generate_team_workplan,
)
from app.pmo_workplan.services.workflow.steps.process_eyip_templates import (
    process_eyip_templates,
)
from app.pmo_workplan.services.workflow.steps.process_input import process_input
from app.pmo_workplan.services.workflow.steps.search_docs import search_docs
from app.pmo_workplan.services.workflow.steps.validate_sql import validate_sql
from app.pmo_workplan.services.workflow.steps.validate_sql_charter import validate_sql_charter
from dependency_injector.wiring import Provide, inject
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step
from app.core.logging_config import LoggerConfig
from app.pmo_workplan.utils import string_to_json_base
import pandas as pd
import json
import datetime

class WorkplanGeneratorWorkflow(Workflow):
    @inject
    def __init__(
        self,
        llm: AzureSyncLLM,
        embed_model: AzureEmbeddings,
        ai_search: AISearch,
        redis: RedisService,
        logger: LoggerConfig = Provide[Container.logger],
        program_office: ProgramOffice = Provide[Container.program_office],
        asset_manager_engine: AssetManager = Provide[Container.asset_manager_engine],
    ) -> None:
        super().__init__(timeout=None, verbose=True)

        self.ai_search = ai_search
        self.redis = redis
        self.asset_manager = asset_manager_engine
        self.llm = llm.get_model()
        self.embed_model = embed_model.get_model()
        self.program_office = program_office
        self.logger = logger

    @step
    async def step_process_input(
        self, event: StartEvent, context: Context
    ) -> SearchTeamEvent | StopEvent:
        instance_id = event.value["instance_id"]
        await context.set("instanceId", instance_id)
        await context.set("sql_okay", True)
        if self.redis.is_workflow_aborted(instance_id, WorkplanOutput):
            return StopEvent(result="Workflow aborted")
        try:
            response = await process_input(self, event, context, self.llm, self.logger)
        except Exception as e:
            response = SearchTeamEvent(project_team=[''])
            await context.set("sql_okay", False)
        return response

    ### Branch 1
    @step
    async def step_generate_sql_for_teamtasks(
        self, event: SearchTeamEvent, context: Context
    ) -> GeneratedSQLEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")

        response = await generate_sql_for_teamtasks(self, event, context)
        return response

    @step
    async def step_validate_sql(
        self, event: GeneratedSQLEvent, context: Context
    ) -> GoodSQLEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")
        try:
            response = await validate_sql(event, context, self.logger)
        except Exception as e:
            response = GoodSQLEvent(sql_output=pd.DataFrame([]))
            await context.set("sql_okay", False)
        return response

    @step
    async def step_process_eyip_templates(
        self, event: GoodSQLEvent | GoodSQLPMOEvent, context: Context
    ) -> EYIPTemplatesEvent | StopEvent | None:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")

        response = await process_eyip_templates(self, event, context, self.llm)
        return response

    # -----------
    # Branch 2
    @step
    async def step_search_docs(
        self, event: SearchTeamEvent, context: Context
    ) -> SearchDocsEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")

        response = await search_docs(self, event, context, self.embed_model, self.logger)
        return response

    #-----------
    ## Branch 3
    @step
    async def step_validate_sql_charter(
        self, event: SearchTeamEvent, context: Context
    ) -> GoodSQLPMOEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")
        try:
            response = await validate_sql_charter(self, event, context, self.logger)
        except Exception as e:
            response = GoodSQLPMOEvent(sql_output={})
        return response
    
    #-----------
    ## Branch 4

    @step
    async def step_generate_tasks(
        self, event: SearchDocsEvent | EYIPTemplatesEvent , context: Context
    ) -> WPTeamTasksEvent | WPTeamTasksOMEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")

        response = await generate_tasks(self, event, context, self.llm, self.logger)
        return response
    
    @step
    async def step_add_timeline(
        self, event: WPTeamTasksEvent, context: Context
    ) -> WPTeamTimelineEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")

        response = await add_timeline(self, event, context, self.llm)
        return response
    
    @step
    async def step_add_timeline_op_model(
        self, event: WPTeamTasksOMEvent, context: Context
    ) -> WPTeamTimelineEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")

        response = await add_timeline_op_model(self, event, context, self.llm)
        return response

    @step
    async def step_generate_team_workplan(
        self, event: WPTeamTimelineEvent, context: Context
    ) -> StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, WorkplanOutput):
            return StopEvent(result="Workflow aborted")

        response = await generate_team_workplan(event, context, self.logger)
        return response
