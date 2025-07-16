import pandas as pd
from app.core.asset_manager_engine import AssetManagerEngine
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.redis_service import RedisService
from app.core.program_office_api import ProgramOffice
from app.dependencies import Container
from app.tsa_generator.schemas.tsa_generator_output import TSAGeneratorOutput
from app.tsa_generator.services.asset_manager import AssetManager
from dependency_injector.wiring import Provide, inject
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step

from app.tsa_generator.schemas.tsa_generator_output import TSAGeneratorOutput
from app.tsa_generator.services.ai_search import AISearch
from app.tsa_generator.services.workflow.events.get_title_event import (
    GetTitleEvent,
)
from app.tsa_generator.services.workflow.events.scope_generation_event import (
    ScopeGenerationEvent,
)
from app.tsa_generator.services.workflow.steps.get_title import (
    get_title,
)
from app.tsa_generator.services.workflow.steps.generate_scope import (
    generate_scope,
)
from app.tsa_generator.services.workflow.steps.tsa_formating_scope import (
    tsa_formating,
)
from dependency_injector.wiring import Provide, inject
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step
from app.core.logging_config import LoggerConfig

class ScopeGeneratorWorkflow(Workflow):
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
        self.logger.info("ScopeGeneratorWorkflow: Workflow instance initialized")

    @step
    async def get_title_and_disposition(
        self, event: StartEvent, context: Context
    ) -> GetTitleEvent | StopEvent:
        instance_id = event.instance_id
        self.logger.info(f"ScopeGeneratorWorkflow: Starting workflow for instanceId: {instance_id}")
        
        await context.set("instanceId", instance_id)
        if self.redis.is_workflow_aborted(instance_id, TSAGeneratorOutput):
            self.logger.warning(f"ScopeGeneratorWorkflow: Workflow aborted for instanceId: {instance_id}")
            return StopEvent(result="TSA aborted")
            
        self.logger.info(f"ScopeGeneratorWorkflow: Calling get_title step for instanceId: {instance_id}")
        response = await get_title(self, event, context)
        
        await context.set("team",event.team)
        await context.set("project_details",event.project_details)
        team = event.team
        self.logger.info(f"ScopeGeneratorWorkflow: Get title for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")

        project_details = await context.get("project_details")
        if project_details:
            self.logger.info(
                f"ScopeGeneratorWorkflow: Project Details - Sector: {getattr(project_details, 'sector', 'N/A')}, "
                f"SubSector: {getattr(project_details, 'subSector', 'N/A')}, "
                f"TransactionType: {getattr(project_details, 'transactionType', 'N/A')}"
            )
        else:
            self.logger.info("ScopeGeneratorWorkflow: Project Details not found in context after input processing.")

        
        self.logger.info("ScopeGeneratorWorkflow: Returning SearchTeamEvent, workflow will branch into multiple steps")
        
        return response


    # -----------
    @step
    async def step_generate_tsa_scope(
        self, event: GetTitleEvent, context: Context
    ) -> ScopeGenerationEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"ScopeGeneratorWorkflow: Consolidation step - Generating scope content for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        self.logger.info(f"ScopeGeneratorWorkflow: Generating scope with event type: {type(event).__name__}")
        
        # Log a summary of the context data we have
        self.logger.info("ScopeGeneratorWorkflow: Context data summary for TSA generation:")
        self.logger.info(f"ScopeGeneratorWorkflow: - Project Team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        try:
            project_details = await context.get("project_details")
            self.logger.info(
                f"ScopeGeneratorWorkflow: - Project Details: Sector={getattr(project_details, 'sector', 'N/A')}, "
                f"SubSector={getattr(project_details, 'subSector', 'N/A')}, "
                f"Type={getattr(project_details, 'transactionType', 'N/A')}"
            )
        except ValueError:
            self.logger.info("ScopeGeneratorWorkflow: - Project Details: Not available")
                
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"ScopeGeneratorWorkflow: Workflow aborted during TSA generation for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
        self.logger.info("ScopeGeneratorWorkflow: Calling generate_tsa to consolidate all data sources")
        response = await generate_scope(self, event, context, self.llm)
        
        if response:
            result_content = getattr(response, 'result', '')
            result_length = len(result_content) if result_content else 0
            self.logger.info(f"ScopeGeneratorWorkflow: TSA content generated successfully, content length: {result_length}")
        else:
            self.logger.warning("ScopeGeneratorWorkflow: No TSA content was generated")
            
        return response

    @step
    async def step_format_tsa(
        self, event: ScopeGenerationEvent, context: Context
    ) -> StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"ScopeGeneratorWorkflow: Final step - Formatting TSA for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"ScopeGeneratorWorkflow: Workflow aborted during TSA formatting for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
        
        result_content = getattr(event, 'result', '')
        result_length = len(result_content) if result_content else 0
        self.logger.info(f"ScopeGeneratorWorkflow: Formatting TSA content of length: {result_length}")
        
        response = await tsa_formating(event, context)
        self.logger.info(f"ScopeGeneratorWorkflow: TSA formatting completed, result: {type(response).__name__}")
        
        # Log completion of the entire workflow
        self.logger.info(f"ScopeGeneratorWorkflow: Workflow completed successfully for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        return response
