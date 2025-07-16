import asyncio
from datetime import datetime, timezone
from typing import Any, List
from uuid import UUID

from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.asset_manager import AssetManager
from app.core.copilot_api import CopilotApi
from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.core.program_office_api import ProgramOffice
from app.pmo_workplan.schemas.workplan_input import (
    ProjectDetails,
    ProjectTeams,
    WorkplanInput
)
from app.pmo_workplan.schemas.workplan_output import (
    CitingSources,
    Output,
    ResponseOutput,
    WorkplanOutput,
)
from app.core.ai_search import AISearch
from app.pmo_workplan.services.process_project_outline import process_project_outline
from app.pmo_workplan.services.process_multiple_teams import process_multiple_teams
from app.pmo_workplan.services.process_op_model_data import process_op_model_data_v2

from app.pmo_workplan.services.workflow.workplan_generator_workflow import (
    WorkplanGeneratorWorkflow,
)
from app.pmo_workplan.utils import create_citing_sources_prj_dcs, merge_unique_dicts
from dependency_injector.wiring import Provide, inject
from yaml import safe_load

from llama_index.core import PromptTemplate
from app.core.utils import clean_and_load_json
from app.core.prompt_manager import create_prompt_manager


class WorkplanGenerator:
    @inject
    def __init__(
            self,
            input_: WorkplanInput,
            redis: RedisService,
            logger: LoggerConfig = Provide[Container.logger],
            llm: AzureSyncLLM = Provide[Container.llm],
            asset_manager: AssetManager = Provide[Container.asset_manager_engine],
            embed_model: AzureEmbeddings = Provide[Container.embed_model],
            ai_search: AISearch = Provide[Container.ai_search],
            copilot_api: CopilotApi = Provide[Container.copilot_api],
            program_office: ProgramOffice = Provide[Container.program_office],
    ) -> None:
        self.input_: WorkplanInput = input_
        self.logger: LoggerConfig = logger
        self.redis: RedisService = redis
        self.ai_search: AISearch = ai_search
        self.llm: AzureSyncLLM = llm
        self.asset_manager = AssetManager(engine=asset_manager)
        self.embed_model: AzureEmbeddings = embed_model
        self.copilot_api: CopilotApi = copilot_api
        self.program_office: ProgramOffice = program_office

    async def generate(self) -> None:
        instance_id = str(self.input_.instanceId)
        teams: list[ProjectTeams] = self.input_.projectTeams
        project_details: ProjectDetails = self.input_.projectDetails
        document_ids: list[UUID] = self.input_.projectDocs
        duration_in_month: int | None = self.input_.durationInMonth
        raw_documents_list = []
        original_documents_list = []
        project_context = await self.copilot_api.get_project_context()
        ceapps = self.input_.ceApps or []
        project_charter_app = next((app for app in ceapps if getattr(app, "name", None) == "projectCharter"), None)
        project_charters = getattr(getattr(project_charter_app, "param", None), "charterIds", [])

        # -----------------------------------------------------------------------------------------------
        # 1 project outline
        # -----------------------------------------------------------------------------------------------
        if document_ids:
            self.logger.info("Creating project outline")
            with open("app/pmo_workplan/templates/project_outline_keys.yaml") as f:
                question_list = safe_load(f.read())

            tasks = [
                self.ai_search.perform_search(question, document_ids)
                for question in question_list
            ]
            tasks.append(process_multiple_teams(self.llm.get_model(), teams, self.asset_manager))

            project_outline_documents = await asyncio.gather(*tasks)
            mapped_teams = project_outline_documents.pop()
            for item in project_outline_documents:
                raw_documents_list.append(item['raw_documents'])
                original_documents_list.append(item['original_documents'])
            raw_documents_list.append(project_context)
            project_outline = await process_project_outline(
                self.llm.get_model(), *raw_documents_list
            )
            self.logger.info("Project outline created")
        else:
            self.logger.info("Project outline missing")
            mapped_teams = await process_multiple_teams(self.llm.get_model(), teams, self.asset_manager)
            project_outline = {}

        tasks: list[Any] = []

        # -----------------------------------------------------------------------------------------------
        # 1 opmodel
        # -----------------------------------------------------------------------------------------------
        self.logger.info("Creating op model")
        op_model_app = next((app for app in ceapps if getattr(app, "name", None) == "opModel"), None)
        dict_mappings = {} 

        if op_model_app is not None: 
            try:                 
                node_mappings = getattr(getattr(op_model_app, "param", None), "nodeMappings", []) or []
                             
                for nodes in node_mappings:
                    op_model_data_aligned = {}  
                    all_ids = nodes.mappedNodeId
                    op_model_data = {}
                    op_model_data = await process_op_model_data_v2(self.program_office, all_ids)
                    if op_model_data:
                        prompt_manager = create_prompt_manager()
                        op_model_build_hierarchy = prompt_manager.get_prompt_sync(
                            agent="pmo_workplan",
                            key="op_model_build_hierarchy"
                        )
                        example_task_structure_op_model = prompt_manager.get_prompt_sync(
                            agent="pmo_workplan",
                            key="example_task_structure_build_op_model_hierarchy"
                        )

                        op_model_build_hierarchy_template = PromptTemplate(op_model_build_hierarchy)            
                        response = await self.llm.get_model().acomplete(
                            op_model_build_hierarchy_template.format(
                                op_model_data=op_model_data,
                                example_structure=example_task_structure_op_model,                    
                                transaction_type=project_details.transactionType                    
                            ),
                        )                     
                        op_model_data_aligned = clean_and_load_json(response.text)
                    
                    dict_mappings[nodes.projectTeamId] = op_model_data_aligned
                

            except Exception as e:
                self.logger.error(f"Op model generation failed: {e}")

        # -----------------------------------------------------------------------------------------------
        # 2 generate workplan by team
        # -----------------------------------------------------------------------------------------------
        self.logger.info("Generating workplan by team")

        for team in teams:
            w = WorkplanGeneratorWorkflow(
                llm=self.llm,
                embed_model=self.embed_model,
                ai_search=self.ai_search,
                redis=self.redis,
                logger=self.logger,
                asset_manager_engine=self.asset_manager
            )
            tasks.append(
                w.run(
                    value={
                        "teams": teams,
                        "mapped_teams": mapped_teams.get(team.title, []),
                        "instance_id": instance_id,
                        "id": team.id,
                        "team": team.title,
                        "project_outline": project_outline,
                        "document_ids": document_ids,
                        "project_details": project_details,
                        "duration_in_month": duration_in_month,
                        "op_model_data": dict_mappings[team.id] if dict_mappings else {},
                        "project_charters": project_charters,
                    }
                )
            )

        errorMessage = None
        runtimeStatus = RuntimeStatus.completed
        for task in asyncio.as_completed(tasks):
            result = await task
            current_workplan: WorkplanOutput = self.redis.get(
                key=instance_id, m=WorkplanOutput
            )  # type: ignore
            if current_workplan.runtimeStatus == RuntimeStatus.aborted:
                break

            try:
                current_workplan.output.response.append(ResponseOutput(**result))
                current_workplan.lastUpdatedTime = datetime.now(timezone.utc)
                self.redis.update(key=instance_id, value=current_workplan)
            except Exception as e:
                self.logger.error(f"Workplan generation failed: {e}, result: {result}")
                runtimeStatus = RuntimeStatus.failed
                errorMessage = result if isinstance(result, str) else str(e)

        original_documents = merge_unique_dicts(original_documents_list)
        citing_sources = create_citing_sources_prj_dcs(original_documents, "all teams", "project-specific-project-docs")
        existing_workplan: WorkplanOutput | None = self.redis.get(
            instance_id, WorkplanOutput
        )

        if not existing_workplan:
            # this is the default workplan if no data is retrieved
            # from redis
            self.logger.error("Workplan not found")
            workplan = WorkplanOutput(
                name="WorkPlanGenerator",
                instanceId=self.input_.instanceId,
                runtimeStatus=RuntimeStatus.failed,
                input=self.input_,
                output=Output(),
                lastUpdatedTime=datetime.now(timezone.utc),
            )
            self.redis.update(instance_id, workplan)
            return

        existing_workplan.lastUpdatedTime = datetime.now(timezone.utc)
        if existing_workplan.runtimeStatus == RuntimeStatus.aborted:
            self.logger.warning("Workplan aborted")
            self.redis.update(instance_id, existing_workplan)
            return

        existing_workplan.runtimeStatus = runtimeStatus
        existing_workplan.errorMessage = errorMessage
        existing_workplan.output.citingSources = [CitingSources(**citing_sources)]
        self.redis.update(instance_id, existing_workplan)
        self.logger.info(f"PMO Workplan finished with runtimeStatus: {runtimeStatus}")