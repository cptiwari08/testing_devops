from datetime import datetime, timezone
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.schemas import RuntimeStatus
from app.op_model.schemas.op_model_output import (
    OPModelOutput,
    Response,
    Output,
)
from app.core.program_office_api import ProgramOffice
from app.status_report.config import ProgramOfficeConfig
from app.core.copilot_api import CopilotApi
from app.core.logging_config import LoggerConfig
from app.core.ai_search import AISearch
from app.core.redis_service import RedisService
from app.dependencies import Container
from app.op_model.schemas.op_model_input import OPModelInput
from app.op_model.services.workflow.op_model_workflow import (
    OpModelWorkflow,
)
from dependency_injector.wiring import Provide, inject
from app.op_model.schemas.op_model_input import (
    ProjectDetails,
    OPModelInput,
)
import os
log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "

class OPModelGenerator:
    @inject
    def __init__(
        self,
        input_: OPModelInput,
        redis: RedisService,        
        logger: LoggerConfig,
        llm: AzureSyncLLM = Provide[Container.llm],
        embed_model: AzureEmbeddings = Provide[Container.embed_model],
        copilot_api: CopilotApi = Provide[Container.copilot_api],
        program_office: ProgramOffice = Provide[Container.program_office],
        program_office_config: ProgramOfficeConfig = Provide[Container.program_office_config],
        ai_search: AISearch = Provide[Container.ai_search],
    ) -> None:
        self.input_: OPModelInput = input_
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
        project_details: ProjectDetails = self.input_.projectDetails
        project_doc = self.input_.projectDocs        
        business_entity = self.input_.businessEntity

        # Add the ey_ip ids
        eyip_ids = self.input_.eyIP
        
        w = OpModelWorkflow(
            llm=self.llm,
            embed_model=self.embed_model,
            ai_search=self.ai_search,
            redis=self.redis,
        )
        errorMessage = None        
        current_op_model = None
        try: 
            result = await w.run(
                instance_id=instance_id,
                project_details=project_details,
                project_doc=project_doc,
                eyip_ids=eyip_ids,
                business_entity=business_entity
            )                 
                        
            current_op_model: OPModelOutput = self.redis.get(
                key=instance_id, m=OPModelOutput
            )  # type: ignore            
            
            if current_op_model.runtimeStatus == RuntimeStatus.aborted:
                self.logger.warning(f"{log_mes_base} OP Model aborted")
                return
            else:
                for item in result:
                    current_op_model.output.response.append(Response(**item))                                                    
                runtimeStatus = RuntimeStatus.completed  
                current_op_model.lastUpdatedTime = datetime.now(timezone.utc)

        except Exception as e:
            self.logger.error(f"{log_mes_base} OP Model generation failed: {e}")
            runtimeStatus = RuntimeStatus.failed
            errorMessage = str(e)              
       
        if not current_op_model:
            # this is the default workplan if no data is retrieved
            # from redis
            self.logger.error(f"{log_mes_base} OP Model not found")
            op_model = OPModelOutput(
                name="ProjectOPModelGenerator",
                instanceId=self.input_.instanceId,
                runtimeStatus=RuntimeStatus.failed,
                input=self.input_,
                output=Output(),
                lastUpdatedTime=datetime.now(timezone.utc),
                createdTime=datetime.now(timezone.utc),
            )
            self.redis.update(instance_id, op_model)
            return             
        
        current_op_model.runtimeStatus = runtimeStatus
        current_op_model.errorMessage = errorMessage        
        self.redis.update(instance_id, current_op_model)
        self.logger.info(f"{log_mes_base} OP Model finished with runtimeStatus: {runtimeStatus}")




        


        
