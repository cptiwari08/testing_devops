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
from app.tsa_generator.services.workflow.events.ait_data_event import AITDataEvent
from app.tsa_generator.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.tsa_generator.services.workflow.events.op_models_data_event import (
    OPModelsDataEvent,
)
from app.tsa_generator.services.workflow.events.generated_sql_event import (
    GeneratedSQLEvent,
)
from app.tsa_generator.services.workflow.events.good_sql_event import GoodSQLEvent
from app.tsa_generator.services.workflow.events.search_docs_event import (
    SearchDocsEvent,
)
from app.tsa_generator.services.workflow.events.search_team_event import (
    SearchTeamEvent,
)
from app.tsa_generator.services.workflow.events.tsa_generation_event import (
    TSAGenerationEvent,
)
from app.tsa_generator.services.workflow.steps.build_filters_for_eyip import (
    build_filters_for_eyip,
)
from app.tsa_generator.services.workflow.steps.validate_sql import (
    validate_sql,
)
from app.tsa_generator.services.workflow.steps.op_model_data_retrieve import (
    op_model_data_retrieve,
)
from app.tsa_generator.services.workflow.steps.generate_tsa import (
    generate_tsa,
)
from app.tsa_generator.services.workflow.steps.tsa_formating import (
    tsa_formating,
)
from app.tsa_generator.services.workflow.steps.process_eyip_templates import (
    process_eyip_templates,
)
from app.tsa_generator.services.workflow.steps.process_input import process_input
from app.tsa_generator.services.workflow.steps.search_docs import search_docs
from app.tsa_generator.services.workflow.steps.op_model_data_retrieve import op_model_data_retrieve
from app.tsa_generator.services.workflow.steps.process_app_inventorytracker import process_app_inventorytracker
from dependency_injector.wiring import Provide, inject
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step
from app.core.logging_config import LoggerConfig

class TSAGeneratorWorkflow(Workflow):
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
        self.logger.info("TSAGeneratorWorkflow: Workflow instance initialized")

    @step
    async def step_process_input(
        self, event: StartEvent, context: Context
    ) -> SearchTeamEvent | StopEvent:
        instance_id = event.instance_id
        self.logger.info(f"TSAGeneratorWorkflow: Starting workflow for instanceId: {instance_id}")
        self.logger.info(f"TSAGeneratorWorkflow: Processing input data, setting up context")
        
        await context.set("instanceId", instance_id)
        if self.redis.is_workflow_aborted(instance_id, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted for instanceId: {instance_id}")
            return StopEvent(result="TSA aborted")
            
        self.logger.info(f"TSAGeneratorWorkflow: Calling process_input step for instanceId: {instance_id}")
        response = await process_input(self, event, context, self.llm)
        
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Input processed for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")

        project_details = await context.get("project_details")
        if project_details:
            self.logger.info(
                f"TSAGeneratorWorkflow: Project Details - Sector: {getattr(project_details, 'sector', 'N/A')}, "
                f"SubSector: {getattr(project_details, 'subSector', 'N/A')}, "
                f"TransactionType: {getattr(project_details, 'transactionType', 'N/A')}"
            )
        else:
            self.logger.info("TSAGeneratorWorkflow: Project Details not found in context after input processing.")

        project_doc_list = await context.get("project_doc", [])
        if project_doc_list:
            self.logger.info(f"TSAGeneratorWorkflow: Project documents to process: {len(project_doc_list)} IDs. Preview: {str(project_doc_list[:3])}{'...' if len(project_doc_list) > 3 else ''}")
        else:
            self.logger.info("TSAGeneratorWorkflow: No project documents found in context.")

        ait_bool_status = await context.get("ait_bool", False)
        self.logger.info(f"TSAGeneratorWorkflow: AIT data inclusion specified: {ait_bool_status}")

        eyip_ids_list = await context.get("eyip_ids", [])
        if eyip_ids_list:
            eyip_ids_summary = [str(item) for item in eyip_ids_list[:2]]
            self.logger.info(f"TSAGeneratorWorkflow: EYIP IDs to process: {len(eyip_ids_list)} items. Preview: {eyip_ids_summary}{'...' if len(eyip_ids_list) > 2 else ''}")
        else:
            self.logger.info("TSAGeneratorWorkflow: No EYIP IDs found in context.")

        op_models_list = await context.get("op_models", [])
        if op_models_list:
            self.logger.info(f"TSAGeneratorWorkflow: Operating Models to process: {len(op_models_list)} items. Preview: {str(op_models_list[:3])}{'...' if len(op_models_list) > 3 else ''}")
        else:
            self.logger.info("TSAGeneratorWorkflow: No Operating Models found in context.")
        
        self.logger.info("TSAGeneratorWorkflow: Returning SearchTeamEvent, workflow will branch into multiple steps")
        
        return response
    
    ### Branch 1
    @step
    async def step_build_filters_for_eyip(
        self, event: SearchTeamEvent, context: Context
    ) -> GeneratedSQLEvent | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Branch 1 - Starting build_filters_for_eyip for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during build_filters_for_eyip for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
            
        eyip_ids = await context.get("eyip_ids")
        if eyip_ids == []:
            self.logger.info(f"TSAGeneratorWorkflow: No EYIP IDs provided, skipping SQL generation")
            return GeneratedSQLEvent(sql_generated="", query_params={}, sql_object=None)
            
        self.logger.info(f"TSAGeneratorWorkflow: Building filters for {len(eyip_ids)} EYIP IDs")
        response = await build_filters_for_eyip(self, event, context)
        self.logger.info(f"TSAGeneratorWorkflow: Filters built successfully, returning GeneratedSQLEvent")
        
        return response

    @step
    async def step_validate_sql(
        self, event: GeneratedSQLEvent, context: Context
    ) -> GoodSQLEvent | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Validating SQL for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during SQL validation for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
            
        eyip_ids = await context.get("eyip_ids")
        if eyip_ids == []:
            self.logger.info("TSAGeneratorWorkflow: No EYIP IDs, skipping SQL validation")
            return GoodSQLEvent(sql_output=pd.DataFrame())
            
        try:
            self.logger.info(f"TSAGeneratorWorkflow: Executing SQL validation with query: {event.sql_generated[:100]}...")
            response = await validate_sql(event, context)
            sql_output_shape = response.sql_output.shape if hasattr(response.sql_output, 'shape') else "No DataFrame"
            self.logger.info(f"TSAGeneratorWorkflow: SQL validation successful, output rows: {sql_output_shape}")
            return response
        except Exception as e:
            self.logger.error(f"TSAGeneratorWorkflow: SQL validation failed: {str(e)}")
            response = GoodSQLEvent(sql_output=pd.DataFrame([]))
            await context.set("sql_okay", False)
            return response


    @step
    async def step_process_eyip_templates(
        self, event: GoodSQLEvent, context: Context
    ) -> EYIPTemplatesEvent | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Processing EYIP templates for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during EYIP templates processing for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
            
        eyip_ids = await context.get("eyip_ids")
        if eyip_ids == []:
            self.logger.info("TSAGeneratorWorkflow: No EYIP IDs, skipping EYIP templates processing")
            ey_ip_templates = {
                'eyip': [],
                'sql_query': ""
            }
            await context.set("ey_ip_templates", ey_ip_templates)
            return EYIPTemplatesEvent(result="")
        sql_output_shape = event.sql_output.shape if hasattr(event.sql_output, 'shape') else "No DataFrame"
        self.logger.info(f"TSAGeneratorWorkflow: Processing EYIP templates with SQL output shape: {sql_output_shape}")
        response = await process_eyip_templates(self, event, context, self.llm)
        self.logger.info(f"TSAGeneratorWorkflow: EYIP templates processed, result length: {len(str(response.result))}")
        
        return response
    
    # -----------
    # AIT Data
    @step
    async def step_ait_data(
        self, event: SearchTeamEvent, context: Context
    ) -> AITDataEvent | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Processing Application Inventory Tracker data for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during AIT data processing for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
            
        ait_bool = await context.get("ait_bool")
        if ait_bool == False:
            self.logger.info("TSAGeneratorWorkflow: AIT not included, skipping AIT data processing")
            await context.set("ait_data", {
            'ait_data': [],
            'sql_query': ""
            })
            return AITDataEvent(ait_data_results=[])
            
        self.logger.info("TSAGeneratorWorkflow: Processing AIT data")
        response_data = await process_app_inventorytracker(self, context) # Renamed to avoid conflict
        self.logger.info(f"TSAGeneratorWorkflow: AIT data processed, results count: {len(response_data)}")
        
        return AITDataEvent(ait_data_results=response_data)
    
    # -----------
    # Branch 2
    @step
    async def step_search_docs(
        self, event: SearchTeamEvent, context: Context
    ) -> SearchDocsEvent | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Branch 2 - Searching documents for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during document search for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
            
        project_doc = await context.get("project_doc")
        if project_doc == []:
            self.logger.info("TSAGeneratorWorkflow: No project documents, skipping document search")
            await context.set("chunk_text", [])
            return SearchDocsEvent(search_results=[])
            
        self.logger.info(f"TSAGeneratorWorkflow: Searching {len(project_doc)} project documents")
        response = await search_docs(self, context, self.embed_model)
        self.logger.info(f"TSAGeneratorWorkflow: Document search completed, results count: {len(response.search_results)}")
        
        return response
    
    # -----------
    # Branch 3      
    @step
    async def step_process_op_model(
        self, event: SearchTeamEvent, context: Context
    ) -> OPModelsDataEvent | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Branch 3 - Processing Operating Model data for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during OP model processing for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
            
        op_models = await context.get("op_models")
        if op_models == []:
            self.logger.info("TSAGeneratorWorkflow: No Operating Models included, skipping")
            await context.set("op_model_data", [])
            return OPModelsDataEvent(result=[])
            
        self.logger.info(f"TSAGeneratorWorkflow: Processing Operating Model data with {len(op_models)} components")
        response = await op_model_data_retrieve(self, context)
        self.logger.info(f"TSAGeneratorWorkflow: Operating Model data processed, result count: {len(response.result)}")
        
        return response

    # -----------
    @step
    async def step_generate_tsa(
        self, event: SearchDocsEvent | EYIPTemplatesEvent | OPModelsDataEvent | AITDataEvent, context: Context
    ) -> TSAGenerationEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Consolidation step - Generating TSA content for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        self.logger.info(f"TSAGeneratorWorkflow: Generating TSA with event type: {type(event).__name__}")
        
        # Log a summary of the context data we have
        self.logger.info("TSAGeneratorWorkflow: Context data summary for TSA generation:")
        self.logger.info(f"TSAGeneratorWorkflow: - Project Team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        try:
            project_details = await context.get("project_details")
            self.logger.info(
                f"TSAGeneratorWorkflow: - Project Details: Sector={getattr(project_details, 'sector', 'N/A')}, "
                f"SubSector={getattr(project_details, 'subSector', 'N/A')}, "
                f"Type={getattr(project_details, 'transactionType', 'N/A')}"
            )
        except ValueError:
            self.logger.info("TSAGeneratorWorkflow: - Project Details: Not available")
        
        try:
            project_doc = await context.get("project_doc")
            doc_status = f"Available ({len(project_doc)} documents)" if project_doc else "Empty list"
            self.logger.info(f"TSAGeneratorWorkflow: - Project Documents: {doc_status}")
        except ValueError:
            self.logger.info("TSAGeneratorWorkflow: - Project Documents: Not available")
        
        try:
            eyip_ids = await context.get("eyip_ids")
            eyip_status = f"Available ({len(eyip_ids)} items)" if eyip_ids else "Empty list"
            self.logger.info(f"TSAGeneratorWorkflow: - EYIP IDs: {eyip_status}")
        except ValueError:
            self.logger.info("TSAGeneratorWorkflow: - EYIP IDs: Not available")
        
        try:
            op_models = await context.get("op_models")
            op_models_status = f"Available ({len(op_models)} models: {', '.join(op_models[:3])}{'...' if len(op_models) > 3 else ''})" if op_models else "Empty list"
            self.logger.info(f"TSAGeneratorWorkflow: - Operating Models: {op_models_status}")
        except ValueError:
            self.logger.info("TSAGeneratorWorkflow: - Operating Models: Not available")
        
        try:
            ait_data = await context.get("ait_data")
            if ait_data and isinstance(ait_data, dict) and 'ait_data' in ait_data:
                ait_list = ait_data['ait_data']
                ait_status = f"Available ({len(ait_list)} items)"
            else:
                ait_status = "Available (empty or invalid format)"
            self.logger.info(f"TSAGeneratorWorkflow: - AIT Data: {ait_status}")
        except ValueError:
            self.logger.info("TSAGeneratorWorkflow: - AIT Data: Not available")
        
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during TSA generation for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
        self.logger.info("TSAGeneratorWorkflow: Calling generate_tsa to consolidate all data sources")
        response = await generate_tsa(self, event, context, self.llm)
        
        if response:
            result_content = getattr(response, 'result', '')
            result_length = len(result_content) if result_content else 0
            self.logger.info(f"TSAGeneratorWorkflow: TSA content generated successfully, content length: {result_length}")
        else:
            self.logger.warning("TSAGeneratorWorkflow: No TSA content was generated")
            
        return response

    @step
    async def step_format_tsa(
        self, event: TSAGenerationEvent, context: Context
    ) -> StopEvent:
        instanceId = await context.get("instanceId")
        team = await context.get("team")
        self.logger.info(f"TSAGeneratorWorkflow: Final step - Formatting TSA for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        if self.redis.is_workflow_aborted(instanceId, TSAGeneratorOutput):
            self.logger.warning(f"TSAGeneratorWorkflow: Workflow aborted during TSA formatting for instanceId: {instanceId}")
            return StopEvent(result="TSA aborted")
        
        result_content = getattr(event, 'result', '')
        result_length = len(result_content) if result_content else 0
        self.logger.info(f"TSAGeneratorWorkflow: Formatting TSA content of length: {result_length}")
        
        response = await tsa_formating(self, event, context, self.llm)
        self.logger.info(f"TSAGeneratorWorkflow: TSA formatting completed, result: {type(response).__name__}")
        
        # Log completion of the entire workflow
        self.logger.info(f"TSAGeneratorWorkflow: Workflow completed successfully for team: {getattr(team, 'title', 'UNKNOWN')} (ID: {getattr(team, 'id', 'UNKNOWN')})")
        
        return response
