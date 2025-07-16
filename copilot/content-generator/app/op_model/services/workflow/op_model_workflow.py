import pandas as pd
from app.core.asset_manager_engine import AssetManagerEngine
from app.op_model.services.asset_manager import AssetManager
from app.core.azure_llm_models import AzureSyncLLM, AzureEmbeddings
from app.core.redis_service import RedisService
from app.core.program_office_api import ProgramOffice
from app.dependencies import Container
from dependency_injector.wiring import Provide, inject
from app.core.ai_search import AISearch
from app.op_model.schemas.op_model_output import OPModelOutput
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step
from app.op_model.services.workflow.events.eyip_data_event import EYIPDataEvent
from app.op_model.services.workflow.events.no_data_event import NoDataEvent
from app.op_model.services.workflow.events.eyip_event import EyIpEvent
from app.op_model.services.workflow.events.project_docs_event import ProjectDocsEvent
from app.op_model.services.workflow.events.search_docs_event import SearchDocsEvent
from app.op_model.services.workflow.events.postprocess_project_docs import (
    PostprocessProjectDocsEvent)
from app.op_model.services.workflow.events.om_process_event import OMProcessEvent
from app.op_model.services.workflow.steps.process_input import process_input
from app.op_model.services.workflow.steps.search_docs import search_project_docs
from app.op_model.services.workflow.steps.fetch_eyip_data import fetch_eyip_data
from app.op_model.services.workflow.steps.postprocess_project_docs import (
    postprocess_project_docs,
)
from app.op_model.services.workflow.steps.generate_op_model import generate_op_model
from app.op_model.services.workflow.steps.op_model_formating import op_model_formating
from app.core.logging_config import LoggerConfig
import json
import os
from datetime import datetime


class OpModelWorkflow(Workflow):
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
    ) -> NoDataEvent | ProjectDocsEvent | EyIpEvent | StopEvent:
        instance_id = event.instance_id
        await context.set("instanceId", instance_id)
        await context.set("sql_okay", True)
        if self.redis.is_workflow_aborted(instance_id, OPModelOutput):
            return StopEvent(result="Op model aborted")
        try:
            response = await process_input(self, event, context)

            for event in response:
                context.send_event(event)
            if not response:
                raise ValueError("No events to send")
            
        except Exception as e:
            response = NoDataEvent()
            await context.set("sql_okay", False)
        return response
    

 
    @step
    async def fetch_eyip_data(
        self, event: EyIpEvent, context: Context
    ) -> EYIPDataEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, OPModelOutput):
            return StopEvent(result="Op model aborted")
        try:
            response = await fetch_eyip_data(self, event, context)
        except Exception as e:
            response = EYIPDataEvent(sql_output=pd.DataFrame([]))
            await context.set("sql_okay", False)
        return response
 
 
    # -----------
    # Branch 2
    @step
    async def step_search_docs(
        # Concurrency here: This step does the search, and then for each search result
        # it triggers a SearchDocsEvent to the next step. 
        self, event: ProjectDocsEvent, context: Context
    ) -> SearchDocsEvent | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, OPModelOutput):
            return StopEvent(result="Op model aborted")
        if await context.get("project_doc") == []:
            return SearchDocsEvent(search_results="")
        search_results = await search_project_docs(self, event, context)

        # If there are relevant results from Project Docs
        if search_results:
            # Update datasource event list that gets awaited in generate_op_model step
            initial_events_to_collect = await context.get("event_to_collect", [])
            updated_events = []
            for event in initial_events_to_collect:
                if event == SearchDocsEvent:
                    updated_events.append(PostprocessProjectDocsEvent)
                else:
                    updated_events.append(event)
            await context.set("event_to_collect", updated_events)
            context.send_event(SearchDocsEvent(search_result=search_results))
        else:
            initial_events_to_collect = await context.get("event_to_collect", [])
            updated_events = []
            for event in initial_events_to_collect:
                if event != SearchDocsEvent:
                    updated_events.append(event)
            await context.set("event_to_collect", updated_events)

    
    @step
    async def step_postprocess_project_docs(
        self, event:  SearchDocsEvent, context: Context
    ) -> PostprocessProjectDocsEvent | None | StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, OPModelOutput):
            return StopEvent(result="Op model aborted")
        resultEvent = await postprocess_project_docs(self, event, context)

        return resultEvent
    
 
    # -----------
    @step
    async def step_generate_op_model(
        self, event: PostprocessProjectDocsEvent | EYIPDataEvent | NoDataEvent, 
        context: Context
    ) -> OMProcessEvent | None | StopEvent:

        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, OPModelOutput):
            return StopEvent(result="Op model aborted")
        response = await generate_op_model(self, event, context)
        
        return response
 
    @step
    async def step_format_op_model(
        self, event: OMProcessEvent, context: Context
    ) -> StopEvent:
        instanceId = await context.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, OPModelOutput):
            return StopEvent(result="Op model aborted")
        response = await op_model_formating(event, context)
        
        return response


