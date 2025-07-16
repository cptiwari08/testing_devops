import pandas as pd
from app.core.asset_manager_engine import AssetManagerEngine
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.redis_service import RedisService
from app.core.program_office_api import ProgramOffice
from app.dependencies import Container
from app.project_charter.schemas.project_charter_output import ProjectCharterOutput
from app.project_charter.services.asset_manager import AssetManager
from dependency_injector.wiring import Provide, inject
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step

# -------adaptation of the code from app/pmo_workplan/services/workplan_generator.py
from app.project_charter.schemas.project_charter_output import ProjectCharterOutput
from app.project_charter.services.ai_search import AISearch
from app.project_charter.services.workflow.events.bad_sql_event import BadSQLEvent
from app.project_charter.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.project_charter.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.project_charter.services.workflow.events.good_sql_event import GoodSQLEvent
from app.project_charter.services.workflow.events.search_docs_event import (
    SearchDocsEvent,
)
from app.project_charter.services.workflow.events.search_team_event import (
    SearchTeamEvent,
)
from app.project_charter.services.workflow.events.wp_teamtasks_event import (
    WPTeamTasksEvent,
)
from app.project_charter.services.workflow.steps.fix_sql import fix_sql
from app.project_charter.services.workflow.steps.generate_sql_for_teamtasks import (
    generate_sql_for_teamtasks,
)
from app.project_charter.services.workflow.steps.generate_charter import (
    generate_charter,
)
from app.project_charter.services.workflow.steps.charter_formating import (
    charter_formating,
)
from app.project_charter.services.workflow.steps.process_eyip_templates import (
    process_eyip_templates,
)
from app.project_charter.services.workflow.steps.process_input import process_input
from app.project_charter.services.workflow.steps.search_docs import search_docs
from app.project_charter.services.workflow.steps.validate_sql import validate_sql
from dependency_injector.wiring import Provide, inject
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step
from app.core.logging_config import LoggerConfig


class ProjectCharterWorkflow(Workflow):
    @inject
    def __init__(
        self,
        llm: AzureSyncLLM,
        embed_model: AzureEmbeddings,
        ai_search: AISearch,
        redis: RedisService,
        logger: LoggerConfig = Provide[Container.logger],
        program_office: ProgramOffice = Provide[Container.program_office],
        asset_manager_engine: AssetManagerEngine = Provide[
            Container.asset_manager_engine
        ],
    ) -> None:
        super().__init__(timeout=None, verbose=True)

        self.ai_search = ai_search
        self.redis = redis
        self.asset_manager = AssetManager(engine=asset_manager_engine)
        self.llm = llm.get_model()
        self.embed_model = embed_model.get_model()
        self.program_office = program_office
        self.logger = logger

    @step
    async def step_process_input(
        self, event: StartEvent, context: Context
    ) -> SearchTeamEvent | StopEvent:
        instance_id = event.instance_id
        await context.set("instanceId", instance_id)
        if self.redis.is_workflow_aborted(instance_id, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        response = await process_input(self, event, context, self.llm)
        return response

    ### Branch 1
    @step
    async def step_generate_sql_for_teamtasks(
        self, event: SearchTeamEvent, context: Context
    ) -> GeneratedSQLEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        if await context.get("eyip_ids") == []:
            return GeneratedSQLEvent(sql_generated="", query_params={}, sql_object=None)
        response = await generate_sql_for_teamtasks(self, event, context)
        return response

    @step
    async def step_validate_sql(
        self, event: GeneratedSQLEvent, context: Context
    ) -> GoodSQLEvent | BadSQLEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        if await context.get("eyip_ids") == []:
            return GoodSQLEvent(sql_output=pd.DataFrame())
        try:
            response = await validate_sql(event, context)
        except Exception as e:
            return BadSQLEvent(
                sql_generated=event.sql_generated, feedback="Error executing query"
            )
        return response

    @step
    async def step_fix_sql(
        self, event: BadSQLEvent, context: Context
    ) -> GeneratedSQLEvent | GoodSQLEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        if await context.get("eyip_ids") == []:
            return GeneratedSQLEvent(sql_generated="", query_params={}, sql_object=None)
        retry = await context.get("retry")
        if retry:
            return GoodSQLEvent(sql_output=pd.DataFrame([]))
        else:
            await context.set("retry", True)
            response = await fix_sql(self, event, context)
        return response

    @step
    async def step_process_eyip_templates(
        self, event: GoodSQLEvent, context: Context
    ) -> EYIPTemplatesEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        if await context.get("eyip_ids") == []:
            return EYIPTemplatesEvent(result="")
        response = await process_eyip_templates(self, event, context, self.llm)
        return response

    # -----------
    # Branch 2
    @step
    async def step_search_docs(
        self, event: SearchTeamEvent, context: Context
    ) -> SearchDocsEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        if await context.get("project_doc") == []:
            return SearchDocsEvent(search_results=[])
        response = await search_docs(self, event, context, self.embed_model, self.llm)
        return response

    # -----------
    @step
    async def step_generate_charter(
        self, event: SearchDocsEvent | EYIPTemplatesEvent, context: Context
    ) -> WPTeamTasksEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        response = await generate_charter(self, event, context, self.llm)
        return response

    @step
    async def step_format_charter(
        self, event: WPTeamTasksEvent, context: Context
    ) -> StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, ProjectCharterOutput):
            return StopEvent(result="Charter aborted")
        response = await charter_formating(event, context)
        return response
