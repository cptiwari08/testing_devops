from datetime import datetime, timezone
from typing import Any
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.schemas import RuntimeStatus
from app.project_charter.schemas.project_charter_output import (
    ProjectCharterOutput,
    Response,
    Output,
)
from app.core.program_office_api import ProgramOffice
from app.status_report.config import ProgramOfficeConfig
from app.core.copilot_api import CopilotApi
from app.core.logging_config import LoggerConfig
from app.project_charter.services.ai_search import AISearch
from app.core.redis_service import RedisService
from app.dependencies import Container
from app.project_charter.schemas.project_charter_input import ProjectCharterInput
from app.project_charter.services.workflow.project_charter_workflow import (
    ProjectCharterWorkflow,
)
from dependency_injector.wiring import Provide, inject
from app.project_charter.schemas.project_charter_input import (
    ProjectDetails,
    ProjectTeam,
    ProjectCharterInput,
)

class ProjectCharterGenerator:
    @inject
    def __init__(
        self,
        input_: ProjectCharterInput,
        redis: RedisService,        
        logger: LoggerConfig,
        llm: AzureSyncLLM = Provide[Container.llm],
        embed_model: AzureEmbeddings = Provide[Container.embed_model],
        copilot_api: CopilotApi = Provide[Container.copilot_api],
        program_office: ProgramOffice = Provide[Container.program_office],
        program_office_config: ProgramOfficeConfig = Provide[Container.program_office_config],
        ai_search: AISearch = Provide[Container.ai_search],
    ) -> None:
        self.input_: ProjectCharterInput = input_
        self.logger: LoggerConfig = logger
        self.redis: RedisService = redis
        self.ai_search = ai_search
        self.llm: AzureSyncLLM = llm
        self.embed_model: AzureEmbeddings = embed_model
        self.program_office: ProgramOffice = program_office
        self.program_office_config: ProgramOfficeConfig = program_office_config
        self.copilot_api: CopilotApi = copilot_api

    async def generate(self) -> None:

        """
        Data: 
        -Project Team
        -Project Details
        -Sections! (index)
        Datasources:
            -Project Documents: IDs to retrieve from AI Search
            -EYIP Templates: IDs to retrieve from SQL EYIP Templates
            -Uploaded Documents: IDs to retrieve from Assistant API
        Challenge:
            -Project document might include information from multiple teams. We need to extract and consolidate.
        """
        
        instance_id = str(self.input_.instanceId)
        team: ProjectTeam = self.input_.projectTeam
        project_details: ProjectDetails = self.input_.projectDetails      
        sections = self.input_.sections # ["Objective", "InScope", "OutofScope", "Risks/Issues", "Interdependecies"  ]
        project_doc = self.input_.projectDocs        

        ##Add the ey_ip ids
        eyip_ids = self.input_.eyIP
        
        w = ProjectCharterWorkflow(
            llm=self.llm,
            embed_model=self.embed_model,
            ai_search=self.ai_search,
            redis=self.redis,
        )
        errorMessage = None        
        current_charter = None
        try: 
            result = await w.run(
                instance_id=instance_id,
                team=team,
                project_details=project_details,
                sections=sections,
                project_doc=project_doc,
                eyip_ids=eyip_ids,
            )                 
                        
            current_charter: ProjectCharterOutput = self.redis.get(
                key=instance_id, m=ProjectCharterOutput
            )  # type: ignore            
            
            if current_charter.runtimeStatus == RuntimeStatus.aborted:
                self.logger.warning("Charter aborted")                
                return            
            else:                
                for item in result:
                    current_charter.output.response.append(Response(**item))                                                    
                runtimeStatus = RuntimeStatus.completed  
                current_charter.lastUpdatedTime = datetime.now(timezone.utc)

        except Exception as e:
            self.logger.error(f"Charter generation failed: {e}")
            runtimeStatus = RuntimeStatus.failed
            errorMessage = str(e)              
       
        if not current_charter:
            # this is the default workplan if no data is retrieved
            # from redis
            self.logger.error("Charter not found")
            charter = ProjectCharterOutput(
                name="ProjectCharterGenerator",
                instanceId=self.input_.instanceId,
                runtimeStatus=RuntimeStatus.failed,
                input=self.input_,
                output=Output(),
                lastUpdatedTime=datetime.now(timezone.utc),
                createdTime=datetime.now(timezone.utc),
            )
            self.redis.update(instance_id, charter)
            return             
        
        current_charter.runtimeStatus = runtimeStatus
        current_charter.errorMessage = errorMessage        
        self.redis.update(instance_id, current_charter)
        self.logger.info(f"PMO charter finished with runtimeStatus: {runtimeStatus}")




        


        
