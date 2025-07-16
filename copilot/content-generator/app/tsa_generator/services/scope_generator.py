import asyncio
from datetime import datetime, timezone
from typing import Any
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.tsa_generator.services.asset_manager import AssetManager
from app.core.schemas import RuntimeStatus
from app.tsa_generator.schemas.tsa_generator_output import (
    TSAGeneratorOutput,
    ResponseScope,
    Output,
)
from app.core.program_office_api import ProgramOffice
from app.status_report.config import ProgramOfficeConfig
from app.core.copilot_api import CopilotApi
from app.core.logging_config import LoggerConfig
from app.tsa_generator.services.ai_search import AISearch
from app.core.redis_service import RedisService
from app.dependencies import Container
from app.tsa_generator.schemas.scope_generator_input import ScopeGeneratorInput
from app.tsa_generator.services.workflow.scope_generator_workflow import (
    ScopeGeneratorWorkflow,
)
from dependency_injector.wiring import Provide, inject
from app.tsa_generator.schemas.tsa_generator_input import (
    ProjectDetails,
    ProjectTeam,
)
from yaml import safe_load


class ScopeGenerator:
    @inject
    def __init__(
        self,
        input_: ScopeGeneratorInput,
        redis: RedisService,
        logger: LoggerConfig = Provide[Container.logger],
        llm: AzureSyncLLM = Provide[Container.llm],
        asset_manager: AssetManager = Provide[Container.asset_manager_engine],
        embed_model: AzureEmbeddings = Provide[Container.embed_model],
        copilot_api: CopilotApi = Provide[Container.copilot_api],
        program_office: ProgramOffice = Provide[Container.program_office],
        program_office_config: ProgramOfficeConfig = Provide[Container.program_office_config],
        ai_search: AISearch = Provide[Container.ai_search],
    ) -> None:
        self.input_: ScopeGeneratorInput = input_
        self.logger: LoggerConfig = logger
        self.redis: RedisService = redis
        self.ai_search = ai_search
        self.llm: AzureSyncLLM = llm
        self.asset_manager = AssetManager(engine=asset_manager)
        self.embed_model: AzureEmbeddings = embed_model
        self.program_office: ProgramOffice = program_office
        self.program_office_config: ProgramOfficeConfig = program_office_config
        self.copilot_api: CopilotApi = copilot_api
        self.logger.info(f"ScopeGenerator: Initialized with instanceId: {input_.instanceId}")

    async def generate(self) -> None:
        instance_id = str(self.input_.instanceId)
        self.logger.info(f"ScopeGenerator: Starting generation process for instanceId: {instance_id}")

        teams: list[ProjectTeam] = self.input_.projectTeams
        self.logger.info(f"ScopeGenerator: Processing {len(teams)} teams")
        project_details: ProjectDetails = self.input_.projectDetails
        self.logger.info(f"ScopeGenerator: Project details - Sector: {project_details.sector}, SubSector: {project_details.subSector}, Transaction Type: {project_details.transactionType}")      

        item_data = self.input_.itemData.id

        tasks: list[Any] = []
        self.logger.info(f"ScopeGenerator: Creating workflow tasks for {len(teams)} teams")

        for i, team in enumerate(teams):
            self.logger.info(f"ScopeGenerator: Initializing workflow for team {i+1}/{len(teams)}: {team.title} (ID: {team.id})")
            w = ScopeGeneratorWorkflow(
                llm=self.llm,
                embed_model=self.embed_model,
                ai_search=self.ai_search,
                redis=self.redis,
            )
            errorMessage = None
            current_tsa = None

            self.logger.info(f"ScopeGenerator: Scheduling run for team {team.title} (ID: {team.id})")
            tasks.append(
                w.run(instance_id=instance_id,
                      team=team,
                      project_details=project_details,
                      item_data=item_data)
            )

        self.logger.info(f"ScopeGenerator: Created {len(tasks)} workflow tasks, now waiting for completion")

        errorMessage = None
        runtimeStatus = RuntimeStatus.completed
        completed_tasks = 0

        self.logger.info(f"ScopeGenerator: Starting to process workflow results as they complete")
        for task in asyncio.as_completed(tasks):
            completed_tasks += 1
            self.logger.info(f"ScopeGenerator: Processing result from task {completed_tasks}/{len(tasks)}")
            try:
                result = await task
                self.logger.info(f"ScopeGenerator: Task {completed_tasks} completed with result type: {type(result)}")

                current_tsa: TSAGeneratorOutput = self.redis.get(
                    key=instance_id, m=TSAGeneratorOutput
                )  # type: ignore

                if current_tsa.runtimeStatus == RuntimeStatus.aborted:
                    self.logger.warning(f"ScopeGenerator: TSA process aborted for instanceId: {instance_id}")   
                    break

                try:
                    self.logger.info(f"ScopeGenerator: Adding workflow result to TSA output for task {completed_tasks}")
                    current_tsa.output.response.append(ResponseScope(**result))
                    current_tsa.lastUpdatedTime = datetime.now(timezone.utc)
                    self.redis.update(key=instance_id, value=current_tsa)
                    self.logger.info(f"ScopeGenerator: Successfully updated Redis with result from task {completed_tasks}")
                except Exception as e:
                    self.logger.error(f"ScopeGenerator: Failed to process result from task {completed_tasks}: {e}")
                    self.logger.error(f"ScopeGenerator: Result data: {result}")
                    runtimeStatus = RuntimeStatus.failed
                    errorMessage = result if isinstance(result, str) else str(e)
            except Exception as e:
                self.logger.error(f"ScopeGenerator: Task {completed_tasks} raised an exception: {str(e)}")
                runtimeStatus = RuntimeStatus.failed
                errorMessage = str(e)

        self.logger.info(f"ScopeGenerator: All {len(tasks)} tasks processed, finalizing TSA output")

        if not current_tsa:
            # this is the default tsa if no data is retrieved
            # from redis
            self.logger.error(f"ScopeGenerator: TSA output not found in Redis for instanceId: {instance_id}")
            tsa = TSAGeneratorOutput(
                name="TSAGenerator",
                instanceId=self.input_.instanceId,
                runtimeStatus=RuntimeStatus.failed,
                input=self.input_,
                output=Output(),
                lastUpdatedTime=datetime.now(timezone.utc),
                createdTime=datetime.now(timezone.utc),
            )
            self.logger.info(f"ScopeGenerator: Creating default failed TSA output for instanceId: {instance_id}")
            self.redis.update(instance_id, tsa)
            return             

        self.logger.info(f"ScopeGenerator: Updating final status to {runtimeStatus} for instanceId: {instance_id}")
        current_tsa.runtimeStatus = runtimeStatus
        current_tsa.errorMessage = errorMessage

        if errorMessage:
            self.logger.error(f"ScopeGenerator: Final error message: {errorMessage}")

        self.redis.update(instance_id, current_tsa)
        self.logger.info(f"ScopeGenerator: Scope generation finished with runtimeStatus: {runtimeStatus} for instanceId: {instance_id}")
        if current_tsa.output and current_tsa.output.response:
            self.logger.info(f"ScopeGenerator: Generated {len(current_tsa.output.response)} response items")