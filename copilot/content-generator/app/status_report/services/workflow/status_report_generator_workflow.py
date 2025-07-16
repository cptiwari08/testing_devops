import json

from app.core.azure_llm_models import AzureSyncLLM
from app.core.copilot_api import CopilotApi
from app.core.logging_config import LoggerConfig
from app.core.program_office_api import ProgramOffice
from app.core.redis_service import RedisService
from app.core.prompt_manager import create_prompt_manager
from app.core.config import ResponseOptions
from app.core.utils import load_template
from app.status_report.schemas.status_report_input import StatusReportInput
from app.status_report.schemas.status_report_output import StatusReportOutput
from app.status_report.services.workflow.events.accomplishments_retrieve_sql_queries import (
    AccomplishmentsRetrieveSQLQueries,
)
from app.status_report.services.workflow.events.accomplishments_run_sql import (
    AccomplishmentsRunSQL,
)
from app.status_report.services.workflow.events.accomplisments import Accomplishments
from app.status_report.services.workflow.events.accomplisments_results import (
    AccomplishmentsResults,
)
from app.status_report.services.workflow.events.create_report import CreateReport
from app.status_report.services.workflow.events.execute_summary_run_sql import (
    ExecutiveSummaryRunSQL,
)
from app.status_report.services.workflow.events.executive_summary import (
    ExecutiveSummary,
)
from app.status_report.services.workflow.events.executive_summary_results import (
    ExecutiveSummaryResults,
)
from app.status_report.services.workflow.events.executive_summary_retrieve_sql_queries import (
    ExecutiveSummaryRetrieveSQLQueries,
)
from app.status_report.services.workflow.events.executive_summary_tone_mimic import (
    ExecuteSummaryToneMimic,
)
from app.status_report.services.workflow.events.general_results import GeneralResults
from app.status_report.services.workflow.events.get_queries import GetQueries
from app.status_report.services.workflow.events.next_steps import NextSteps
from app.status_report.services.workflow.events.next_steps_results import (
    NextStepsResults,
)
from app.status_report.services.workflow.events.next_steps_retrieve_sql_queries import (
    NextStepsRetrieveSQLQueries,
)
from app.status_report.services.workflow.events.next_steps_run_sql import (
    NextStepsRunSQL,
)
from app.status_report.services.workflow.events.overall_status import OverallStatus
from app.status_report.services.workflow.events.overall_status_results import (
    OverallStatusResults,
)
from app.status_report.services.workflow.events.overall_status_retrieve_sql_queries import (
    OverallStatusRetrieveSQLQueries,
)
from app.status_report.services.workflow.events.overall_status_run_sql import (
    OverallStatusRunSQL,
)
from app.status_report.services.workflow.steps.accomplishments import accomplishments
from app.status_report.services.workflow.steps.accomplishments_run_sql import (
    accomplishments_run_sql,
)
from app.status_report.services.workflow.steps.execute_summary_run_sql import (
    execute_summary_run_sql,
)
from app.status_report.services.workflow.steps.executive_summary import (
    executive_summary,
)
from app.status_report.services.workflow.steps.executive_summary_tone_mimic import (
    executive_summary_tone_mimic,
)
from app.status_report.services.workflow.steps.get_queries import get_queries_run_sql
from app.status_report.services.workflow.steps.join_reports import join_reports
from app.status_report.services.workflow.steps.next_steps import next_steps
from app.status_report.services.workflow.steps.next_steps_run_sql import (
    next_steps_run_sql,
)
from app.status_report.services.workflow.steps.overall_status import overall_status
from app.status_report.services.workflow.steps.overall_status_run_sql import (
    overall_status_run_sql,
)
from llama_index.core.workflow import Context, StartEvent, StopEvent, Workflow, step


class StatusReportGeneratorWorkflow(Workflow):

    def __init__(
        self,
        timeout,
        verbose,
        logger: LoggerConfig,
        program_office: ProgramOffice,
        copilot_api: CopilotApi,
        llm: AzureSyncLLM,
        redis: RedisService,
    ) -> None:
        super().__init__(timeout=timeout, verbose=verbose)
        self.logger: LoggerConfig = logger
        self.program_office: ProgramOffice = program_office
        self.copilot_api: CopilotApi = copilot_api
        self.llm = llm.get_model()
        self.redis: RedisService = redis
        self.prompt_manager = create_prompt_manager(logger=self.logger)

    sections = {
        "EXECUTIVE_SUMMARY": "executive-summary",
        "OVERALL_STATUS": "overall-status",
        "ACCOMPLISHMENTS": "accomplishments",
        "NEXT_STEPS": "next-steps",
    }

    tables_names = {
        "WORKPLAN": "Workplan",
        "RISKS_ISSUES": "Risks and Issues",
        "ACTIONS": "Actions",
        "DECISIONS": "Decisions",
        "INTERDEPENDENCIES": "Interdependencies",
        "OVERALL_STATUS": "Workplan",
    }

    @step
    async def start_report_creation(
        self, ctx: Context, ev: StartEvent
    ) -> GetQueries | StopEvent:
        input: StatusReportInput = ev.value
        instance_id = str(input.instanceId)
        await ctx.set("instanceId", instance_id)
        await ctx.set("input", input)

        if self.redis.is_workflow_aborted(instance_id, StatusReportOutput):
            return StopEvent(result="Workflow aborted")
        
        start_date = input.reportingPeriod.periodStartDate
        end_date = input.reportingPeriod.periodEndDate

        return GetQueries(
            sections=self.sections,
            tables=self.tables_names,
            project_team=input.projectTeam.id,
            start_date=start_date,
            end_date=end_date
        )

    @step
    async def fetch_data(
        self, ctx: Context, ev: GetQueries
    ) -> CreateReport | StopEvent:
        """DB connection and retrieve queries for all sections.

        Returns:
            _type_: _description_
        """
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        return await get_queries_run_sql(self, ctx, ev)

    @step
    async def create_report(
        self, ctx: Context, ev: CreateReport
    ) -> (
        OverallStatusRetrieveSQLQueries
        | ExecutiveSummaryRetrieveSQLQueries
        | AccomplishmentsRetrieveSQLQueries
        | NextStepsRetrieveSQLQueries
        | StopEvent
        | None
    ):

        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        await ctx.set("data", ev.data)
        await ctx.set(
            "retry_prompt",
            load_template(
                "app/status_report/services/workflow/templates/retry_formatting.yaml"
            )
        )

        # Trigger report sections
        ctx.send_event(OverallStatusRetrieveSQLQueries(project_team=ev.project_team, start_date=ev.start_date, end_date=ev.end_date))
        ctx.send_event(ExecutiveSummaryRetrieveSQLQueries(project_team=ev.project_team, start_date=ev.start_date, end_date=ev.end_date))
        ctx.send_event(AccomplishmentsRetrieveSQLQueries(project_team=ev.project_team, start_date=ev.start_date, end_date=ev.end_date))
        ctx.send_event(NextStepsRetrieveSQLQueries(project_team=ev.project_team, start_date=ev.start_date, end_date=ev.end_date))

    def get_prompt_data_by_source(self, ready, no_data_found="No data found") -> str:
        text = ""
        for value in ready:
            if value.response:
                text = (
                    text
                    + "Table name: "
                    + value.table
                    + " Rules: "
                    + str(value.rules)
                    + " SQL result: "
                    + str(value.response)
                    + " "
                )
        return str(text)

    def get_citing_sources_by_source(self, ready, context = "") -> dict:
        source_value_list = []
        for value in ready:
            source_value_list.append(value.source_value)
        citing_sources = [{
            "sourceName": f"project-status-{value.source}",
            "sourceType": "table",
            "sourceValue": source_value_list,
            "context": ""
        }]
        if ResponseOptions.return_context == "true":
            citing_sources[0]["context"]= context
        return citing_sources

    def get_joined_query_results(self, ready) -> list:
        results = []
        for value in ready:
            if not value.response:
                value.response = []
                results = results + value.response
            else:
                transformed_response_previous = value.response.replace("'", '"').replace(" \n","*|??|*").replace("None",'"None"').replace("`","")
                transformed_response = transformed_response_previous.replace('"s',"'s")
                list_response = [json.loads(x) for x in transformed_response.split("*|??|*")]
                results = results + list_response
        return results

    @step
    async def overall_status_retrieve_sql_queries(
        self, ctx: Context, event: OverallStatusRetrieveSQLQueries
    ) -> OverallStatusRunSQL | None | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        data = await ctx.get("data")
        source_queries = data[self.sections["EXECUTIVE_SUMMARY"]]
        self.logger.info("Starting overall status")
        for subcategory in source_queries:
            ctx.send_event(
                OverallStatusRunSQL(
                    query_and_rules=source_queries[subcategory],
                    table=subcategory,
                    project_team=event.project_team,
                    start_date=event.start_date, 
                    end_date=event.end_date
                )
            )
        await ctx.set("length_overall_status", len(source_queries))

    @step
    async def overall_status_run_sql(
        self, ctx: Context, event: OverallStatusRunSQL
    ) -> OverallStatusResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        return await overall_status_run_sql(self, ctx, event)

    @step
    async def overall_status_join_queries_results(
        self, ctx: Context, ev: OverallStatusResults
    ) -> OverallStatus | None | GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        amount_of_queries = await ctx.get("length_overall_status")
        ready_os = ctx.collect_events(ev, [OverallStatusResults] * amount_of_queries)

        # Just when all queries are ready we can join them
        if ready_os is None:
            return None

        os_joined_results = self.get_joined_query_results(ready_os)

        if all([not value.response for value in ready_os]):
            self.logger.info("Not information for overall status")
            source_os = {
                "sourceName": f"project-status-{self.sections['OVERALL_STATUS']}",
                "content": {
                    "id": None,
                    "title": None,
                },
                "status": "204",
                "citingSources": self.get_citing_sources_by_source(ready_os, "No data found"),
            }
            return GeneralResults(source=source_os)
        
        self.logger.info("Overall status information joined")

        citing_sources = self.get_citing_sources_by_source(ready_os,str(os_joined_results))

        ctx.send_event(
            OverallStatus(
                project_team=ev.project_team,
                sql_results=os_joined_results,
                citing_sources=citing_sources
            )
        )

    @step
    async def overall_status(
        self, ctx: Context, event: OverallStatus
    ) -> GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        return await overall_status(self, ctx, event)

    @step
    async def executive_summary_retrieve_sql_queries(
        self, ctx: Context, event: ExecutiveSummaryRetrieveSQLQueries
    ) -> ExecutiveSummaryRunSQL | None | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        data = await ctx.get("data")
        source_queries = data[self.sections["EXECUTIVE_SUMMARY"]]
        for subcategory in source_queries:
            ctx.send_event(
                ExecutiveSummaryRunSQL(
                    query_and_rules=source_queries[subcategory],
                    table=subcategory,
                    project_team=event.project_team,
                    start_date=event.start_date, 
                    end_date=event.end_date
                )
            )
        await ctx.set("length_executive_summary", len(source_queries))

    @step
    async def executive_summary_run_sql(
        self, ctx: Context, event: ExecutiveSummaryRunSQL
    ) -> ExecutiveSummaryResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        return await execute_summary_run_sql(self, ctx, event)

    @step
    async def executive_summary_join_queries_results(
        self, ctx: Context, ev: ExecutiveSummaryResults
    ) -> ExecutiveSummary | None | GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        amount_of_queries = await ctx.get("length_executive_summary")
        ready_es = ctx.collect_events(ev, [ExecutiveSummaryResults] * amount_of_queries)

        # Just when all queries are ready we can join them
        if ready_es is None:
            return None

        if all([not value.response for value in ready_es]):
            self.logger.info("Not information for executive summary")
            source = {
                "sourceName": f"project-status-{self.sections['EXECUTIVE_SUMMARY']}",
                "content": "I am sorry, I could not find any data for the executive summary.",
                "status": "204",
                "citingSources": self.get_citing_sources_by_source(ready_es, "No data found"),
            }
            return GeneralResults(source=source)
        
        self.logger.info("Executive summary information joined")

        text = self.get_prompt_data_by_source(ready_es)
        citing_sources = self.get_citing_sources_by_source(ready_es, text)

        ctx.send_event(
            ExecutiveSummary(
                project_team=ev.project_team,
                es_queries_result=text,
                citing_sources=citing_sources,
            )
        )

    @step
    async def executive_summary(
        self, ctx: Context, event: ExecutiveSummary
    ) -> ExecuteSummaryToneMimic | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        await ctx.set(
            "executive_summary_prompt",
            self.prompt_manager.get_prompt_sync(
                agent="status_report",
                key="executive_summary",
            )
        )
        return await executive_summary(self, ctx, event)

    @step
    async def executive_summary_tone_mimic(
        self, ctx: Context, event: ExecuteSummaryToneMimic
    ) -> GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        await ctx.set(
            "execute_summary_mimic_tone",
            self.prompt_manager.get_prompt_sync(
                agent="status_report",
                key="mimic_tone",
            )
        )
        return await executive_summary_tone_mimic(self, ctx, event)

    @step
    async def accomplishments_retrieve_sql_queries(
        self, ctx: Context, event: AccomplishmentsRetrieveSQLQueries
    ) -> AccomplishmentsRunSQL | None | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        data = await ctx.get("data")
        source_queries = data[self.sections["ACCOMPLISHMENTS"]]
        for subcategory in source_queries:
            ctx.send_event(
                AccomplishmentsRunSQL(
                    query_and_rules=source_queries[subcategory],
                    table=subcategory,
                    project_team=event.project_team,
                    start_date=event.start_date, 
                    end_date=event.end_date
                )
            )
        await ctx.set("length_accomplishments", len(source_queries))

    @step
    async def accomplishments_run_sql(
        self, ctx: Context, event: AccomplishmentsRunSQL
    ) -> AccomplishmentsResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        return await accomplishments_run_sql(self, ctx, event)

    @step
    async def accomplishments_join_queries_results(
        self, ctx: Context, ev: AccomplishmentsResults
    ) -> Accomplishments | None | GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        amount_of_queries = await ctx.get("length_accomplishments")
        ready_acc = ctx.collect_events(ev, [AccomplishmentsResults] * amount_of_queries)

        # Just when all queries are ready we can join them
        if ready_acc is None:
            return None

        if all([not value.response for value in ready_acc]):
            self.logger.info("Not information for accomplishments")
            source = {
                "sourceName": f"project-status-{self.sections['ACCOMPLISHMENTS']}",
                "content": [
                    "I am sorry, I could not find any data for the accomplishments."
                ],
                "status": "204",
                "citingSources": self.get_citing_sources_by_source(ready_acc, "No data found"),
            }
            return GeneralResults(source=source)
        
        self.logger.info("Accomplishments information joined")

        text = self.get_prompt_data_by_source(ready_acc)
        citing_sources = self.get_citing_sources_by_source(ready_acc, text)

        ctx.send_event(
            Accomplishments(
                project_team=ev.project_team,
                es_queries_result=text,
                citing_sources=citing_sources
            )
        )

    @step
    async def accomplishments(
        self, ctx: Context, event: Accomplishments
    ) -> GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        await ctx.set(
            "accomplishments_prompt",
            self.prompt_manager.get_prompt_sync(
                agent="status_report",
                key="accomplishments",
            )
        )
        return await accomplishments(self, ctx, event)

    @step
    async def next_steps_retrieve_sql_queries(
        self, ctx: Context, event: NextStepsRetrieveSQLQueries
    ) -> NextStepsRunSQL | None | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        data = await ctx.get("data")
        source_queries = data[self.sections["NEXT_STEPS"]]
        for subcategory in source_queries:
            ctx.send_event(
                NextStepsRunSQL(
                    query_and_rules=source_queries[subcategory],
                    table=subcategory,
                    project_team=event.project_team,
                    start_date=event.start_date, 
                    end_date=event.end_date
                )
            )
        await ctx.set("length_next_steps", len(source_queries))

    @step
    async def next_steps_run_sql(
        self, ctx: Context, event: NextStepsRunSQL
    ) -> NextStepsResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        return await next_steps_run_sql(self, ctx, event)

    @step
    async def next_steps_join_queries_results(
        self, ctx: Context, ev: NextStepsResults
    ) -> NextSteps | None | GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        amount_of_queries = await ctx.get("length_next_steps")
        ready_ne = ctx.collect_events(ev, [NextStepsResults] * amount_of_queries)

        # Just when all queries are ready we can join them
        if ready_ne is None:
            return None

        if all([not value.response for value in ready_ne]):
            self.logger.info("Not information for next steps")
            source = {
                "sourceName": f"project-status-{self.sections['NEXT_STEPS']}",
                "content": [
                    "I am sorry, I could not find any data for the next steps."
                ],
                "status": "204",
                "citingSources": self.get_citing_sources_by_source(ready_ne, "No data found"),
            }
            return GeneralResults(
                source=source,
            )
        
        self.logger.info("Next steps information joined")

        text = self.get_prompt_data_by_source(ready_ne)
        citing_sources = self.get_citing_sources_by_source(ready_ne, text)

        ctx.send_event(
            NextSteps(
                project_team=ev.project_team,
                es_queries_result=text,
                citing_sources=citing_sources
            )
        )

    @step
    async def next_steps(
        self, ctx: Context, event: NextSteps
    ) -> GeneralResults | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        await ctx.set(
            "next_steps_prompt",
            self.prompt_manager.get_prompt_sync(
                agent="status_report",
                key="next_steps",
            )
        )
        return await next_steps(self, ctx, event)


    @step
    async def join_reports(
        self, ctx: Context, event: GeneralResults
    ) -> StopEvent | None | StopEvent:
        instanceId = await ctx.get("instanceId")
        if self.redis.is_workflow_aborted(instanceId, StatusReportOutput):
            return StopEvent(result="Workflow aborted")

        return await join_reports(self, ctx, event)
