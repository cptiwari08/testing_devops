import asyncio
from datetime import datetime, timezone
from typing import Any
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.tsa_generator.services.asset_manager import AssetManager
from app.core.schemas import RuntimeStatus
from app.tsa_generator.schemas.tsa_generator_output import (
    TSAGeneratorOutput,
    ResponseTSA,
    Output,
)
from app.core.program_office_api import ProgramOffice
from app.status_report.config import ProgramOfficeConfig
from app.core.copilot_api import CopilotApi
from app.core.logging_config import LoggerConfig
from app.tsa_generator.services.ai_search import AISearch
from app.core.redis_service import RedisService
from app.dependencies import Container
from app.tsa_generator.schemas.tsa_generator_input import TSAGeneratorInput
from app.tsa_generator.schemas.scope_generator_input import ScopeGeneratorInput
from app.tsa_generator.services.workflow.tsa_generator_workflow import (
    TSAGeneratorWorkflow,
)
from dependency_injector.wiring import Provide, inject
from app.tsa_generator.schemas.tsa_generator_input import (
    ProjectDetails,
    ProjectTeam,
    TSAGeneratorInput,
)
from yaml import safe_load


class TSAGenerator:
    @inject
    def __init__(
        self,
        input_: TSAGeneratorInput,
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
        self.input_: TSAGeneratorInput | ScopeGeneratorInput = input_
        self.logger: LoggerConfig = logger
        self.redis: RedisService = redis
        self.ai_search = ai_search
        self.llm: AzureSyncLLM = llm
        self.asset_manager = AssetManager(engine=asset_manager)
        self.embed_model: AzureEmbeddings = embed_model
        self.program_office: ProgramOffice = program_office
        self.program_office_config: ProgramOfficeConfig = program_office_config
        self.copilot_api: CopilotApi = copilot_api
        self.logger.info(f"TSAGenerator: Initialized with instanceId: {input_.instanceId}")
        self.logger.info(f"TSAGenerator: Configuration summary - Teams count: {len(input_.projectTeams)}, " +
                        f"Project docs count: {len(input_.projectDocs) if input_.projectDocs else 0}, " +
                        f"EYIP IDs count: {len(input_.eyIP) if input_.eyIP else 0}")

    async def generate(self) -> None:

        """
        Data: 
        -Project Team
        -Project Details
        Datasources:
            -Project Documents: IDs to retrieve from AI Search
            -EYIP Templates: IDs to retrieve from SQL EYIP Templates
            -Uploaded Documents: IDs to retrieve from Assistant API
        Challenge:
            -Project document might include information from multiple teams. We need to extract and consolidate.
        """

        instance_id = str(self.input_.instanceId)
        self.logger.info(f"TSAGenerator: Starting generation process for instanceId: {instance_id}")

        teams: list[ProjectTeam] = self.input_.projectTeams
        self.logger.info(f"TSAGenerator: Processing {len(teams)} teams")
        project_details: ProjectDetails = self.input_.projectDetails
        self.logger.info(f"TSAGenerator: Project details - Sector: {project_details.sector}, SubSector: {project_details.subSector}, Transaction Type: {project_details.transactionType}")      

        project_doc = self.input_.projectDocs
        self.logger.info(f"TSAGenerator: Project docs count: {len(project_doc) if project_doc else 0}")

        project_documentation = []
        if project_doc:
            self.logger.info(f"TSAGenerator: Processing project documents, count: {len(project_doc)}")
            try:
                with open("app/tsa_generator/templates/project_document_keys.yaml") as f:
                    question_list = safe_load(f.read())
                self.logger.info(f"TSAGenerator: Loaded {len(question_list)} questions from project_document_keys.yaml")

                self.logger.info(f"TSAGenerator: Creating AI search tasks for {len(question_list)} questions")
                tasks = [
                    self.ai_search.perform_search(question, project_doc)
                    for question in question_list
                ]
                self.logger.info(f"TSAGenerator: Executing AI search batch with {len(tasks)} tasks")
                project_outline_documents = await asyncio.gather(*tasks)
                self.logger.info(f"TSAGenerator: Received {len(project_outline_documents)} document results from AI search")

                for item in project_outline_documents:
                    project_documentation.append(item['raw_documents'])
                self.logger.info(f"TSAGenerator: Processed project documentation, count: {len(project_documentation)}")
            except Exception as e:
                self.logger.error(f"TSAGenerator: Error processing project documents: {str(e)}")
                raise
        else:
            self.logger.info("TSAGenerator: No project documents provided, skipping document processing")
            project_documentation = []

        # Log detailed information about input parameters
        self.logger.info("TSAGenerator: ====== Input parameters for workflow execution ======")
        ##Add the ey_ip ids
        eyip_ids = self.input_.eyIP
        self.logger.info(f"TSAGenerator: EYIP IDs count: {len(eyip_ids) if eyip_ids else 0}")
        
        if self.input_.ceApps == []:
            op_models = []
            self.logger.info(f"TSAGenerator: Operating model included components count: 0")
            ait_bool = False
            self.logger.info(f"TSAGenerator: Application Inventory Tracker included: {ait_bool}")
            op_models_ids = []
            self.logger.info(f"TSAGenerator: Operating model node mappings count: 0")
            self.logger.info("TSAGenerator: ================================================")
        else:
            try:
                op_models = [x for x in self.input_.ceApps if x["name"]=="opModel"][0]["param"]["includedComponents"]
                self.logger.info(f"TSAGenerator: Operating model included components count: {len(op_models) if op_models else 0}")

                op_models_ids = [x for x in self.input_.ceApps if x["name"]=="opModel"][0]["param"]["nodeMappings"]
                self.logger.info(f"TSAGenerator: Operating model node mappings count: {len(op_models_ids) if op_models_ids else 0}")
                self.logger.info("TSAGenerator: ================================================")
            except:
                op_models = []
                self.logger.info(f"TSAGenerator: Operating model included components count: 0")
                op_models_ids = []
                self.logger.info(f"TSAGenerator: Operating model node mappings count: 0")
                self.logger.info("TSAGenerator: ================================================")

            try:
                ait_bool = [x for x in self.input_.ceApps if x["name"]=="appInventoryTracker"][0]["param"]["included"]
                self.logger.info(f"TSAGenerator: Application Inventory Tracker included: {ait_bool}")
            except:
                ait_bool = False
                self.logger.info(f"TSAGenerator: Application Inventory Tracker included: {ait_bool}")
        
        tasks: list[Any] = []
        self.logger.info(f"TSAGenerator: Creating workflow tasks for {len(teams)} teams")

        for i, team in enumerate(teams):
            self.logger.info(f"TSAGenerator: Initializing workflow for team {i+1}/{len(teams)}: {team.title} (ID: {team.id})")
            w = TSAGeneratorWorkflow(
                llm=self.llm,
                embed_model=self.embed_model,
                ai_search=self.ai_search,
                redis=self.redis,
            )
            errorMessage = None        
            current_tsa = None

            self.logger.info(f"TSAGenerator: Scheduling run for team {team.title} (ID: {team.id})")
            tasks.append(
                w.run(instance_id= instance_id,
                        team= team,
                        project_doc= project_doc,
                        eyip_ids= eyip_ids,
                        project_details= project_details,
                        op_models= op_models,
                        op_models_ids= op_models_ids,
                        ait_bool= ait_bool,
                        project_documentation=project_documentation
                )
            )
        
        self.logger.info(f"TSAGenerator: Created {len(tasks)} workflow tasks, now waiting for completion")
        
        errorMessage = None
        runtimeStatus = RuntimeStatus.completed
        completed_tasks = 0
        
        self.logger.info(f"TSAGenerator: Starting to process workflow results as they complete")
        for task in asyncio.as_completed(tasks):
            completed_tasks += 1
            self.logger.info(f"TSAGenerator: Processing result from task {completed_tasks}/{len(tasks)}")
            try:
                result = await task
                self.logger.info(f"TSAGenerator: Task {completed_tasks} completed with result type: {type(result)}")
                
                current_tsa: TSAGeneratorOutput = self.redis.get(
                    key=instance_id, m=TSAGeneratorOutput
                )  # type: ignore
                
                if current_tsa.runtimeStatus == RuntimeStatus.aborted:
                    self.logger.warning(f"TSAGenerator: TSA process aborted for instanceId: {instance_id}")   
                    break

                try:
                    self.logger.info(f"TSAGenerator: Adding workflow result to TSA output for task {completed_tasks}")
                    current_tsa.output.response.append(ResponseTSA(**result))
                    current_tsa.lastUpdatedTime = datetime.now(timezone.utc)
                    self.redis.update(key=instance_id, value=current_tsa)
                    self.logger.info(f"TSAGenerator: Successfully updated Redis with result from task {completed_tasks}")
                except Exception as e:
                    self.logger.error(f"TSAGenerator: Failed to process result from task {completed_tasks}: {e}")
                    self.logger.error(f"TSAGenerator: Result data: {result}")
                    runtimeStatus = RuntimeStatus.failed
                    errorMessage = result if isinstance(result, str) else str(e)
            except Exception as e:
                self.logger.error(f"TSAGenerator: Task {completed_tasks} raised an exception: {str(e)}")
                runtimeStatus = RuntimeStatus.failed
                errorMessage = str(e)
       
        self.logger.info(f"TSAGenerator: All {len(tasks)} tasks processed, finalizing TSA output")
        
        if not current_tsa:
            # this is the default tsa if no data is retrieved
            # from redis
            self.logger.error(f"TSAGenerator: TSA output not found in Redis for instanceId: {instance_id}")
            tsa = TSAGeneratorOutput(
                name="TSAGenerator",
                instanceId=self.input_.instanceId,
                runtimeStatus=RuntimeStatus.failed,
                input=self.input_,
                output=Output(),
                lastUpdatedTime=datetime.now(timezone.utc),
                createdTime=datetime.now(timezone.utc),
            )
            self.logger.info(f"TSAGenerator: Creating default failed TSA output for instanceId: {instance_id}")
            self.redis.update(instance_id, tsa)
            return             
        
        self.logger.info(f"TSAGenerator: Updating final status to {runtimeStatus} for instanceId: {instance_id}")
        current_tsa.runtimeStatus = runtimeStatus
        current_tsa.errorMessage = errorMessage
        
        if errorMessage:
            self.logger.error(f"TSAGenerator: Final error message: {errorMessage}")
            
        self.redis.update(instance_id, current_tsa)
        self.logger.info(f"TSAGenerator: TSA generation finished with runtimeStatus: {runtimeStatus} for instanceId: {instance_id}")
        if current_tsa.output and current_tsa.output.response:
            self.logger.info(f"TSAGenerator: Generated {len(current_tsa.output.response)} response items")
