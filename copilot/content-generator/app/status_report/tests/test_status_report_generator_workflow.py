from unittest.mock import AsyncMock, MagicMock
from uuid import UUID

import pytest


@pytest.fixture(scope="function", autouse=False)
def mock_dependencies():
    logger = MagicMock()
    program_office = MagicMock()
    copilot_api = MagicMock()
    llm = MagicMock()
    redis = MagicMock()
    return logger, program_office, copilot_api, llm, redis


@pytest.fixture(scope="function", autouse=False)
def workflow(mock_dependencies):
    from app.status_report.services.workflow.status_report_generator_workflow import (
        StatusReportGeneratorWorkflow,
    )

    logger, program_office, copilot_api, llm, redis = mock_dependencies
    return StatusReportGeneratorWorkflow(
        timeout=60,
        verbose=True,
        logger=logger,
        program_office=program_office,
        copilot_api=copilot_api,
        llm=llm,
        redis=redis,
    )


@pytest.mark.asyncio
async def test_start_report_creation(workflow) -> None:
    # Arrange
    from app.status_report.schemas.status_report_input import (
        ProjectDetails,
        ProjectTeam,
        ReportingPeriod,
        StatusReportInput,
    )
    from app.status_report.services.workflow.events.get_queries import GetQueries
    from llama_index.core.workflow import Context, StartEvent

    ctx = Context(workflow=workflow)
    input_data = StatusReportInput(
        instanceId=UUID("6bd23cc3-9f08-42d6-910b-32975a232d9a"),
        projectTeam=ProjectTeam(title="HR", id="59"),
        reportingPeriod=ReportingPeriod(
            id="1111", 
            title="2024/10/22 - 2024/10/28",
            periodStartDate="2024-10-22",
            periodEndDate="2024-10-28"
        ),
        projectDetails=ProjectDetails(
            sector="Information Technology",
            subSector="Software",
            transactionType="Buy&Integrate",
        ),
    )

    ev = StartEvent()
    ev.value = input_data
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    # Act
    result = await workflow.start_report_creation(ctx, ev)

    # Assert
    assert isinstance(result, GetQueries)
    assert result.sections == workflow.sections
    assert result.tables == workflow.tables_names


@pytest.mark.asyncio
async def test_fetch_data(workflow) -> None:
    # Arrange
    from app.status_report.schemas.status_report_input import (
        ProjectDetails,
        ProjectTeam,
        ReportingPeriod,
        StatusReportInput,
    )
    from app.status_report.services.workflow.events.create_report import CreateReport
    from app.status_report.services.workflow.events.get_queries import GetQueries
    from llama_index.core.workflow import Context
    from unittest.mock import patch

    workflow.copilot_api.get_content_generator_sqls = AsyncMock()
    workflow.copilot_api.get_content_generator_sqls.return_value = [
        {
            "id": 1,
            "app": "PMO",
            "title": "Project Status Executive Summary Workplan",
            "description": 'The workplan section focuses on milestones and tasks that need immediate attention. The system filters them based on criteria such as a timeframe of seven days before or after today\'s date, a status that is not cancelled, the project team selected in the UI, all priority levels with emphasis on critical, high, medium, low, and "null", a hierarchy level of three or less, and types including milestones and tasks.',
            "generatorType": "ProjectStatus",
            "key": "EXECUTIVE_SUMMARY_WORKPLAN",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 2,
            "app": "PMO",
            "title": "Project Status Executive Summary Risks and Issues",
            "description": "The system identifies high-priority risks and issues that need prompt attention. It filters based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with emphasis on critical, high, medium, low, and null",
            "generatorType": "ProjectStatus",
            "key": "EXECUTIVE_SUMMARY_RISKS_ISSUES",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 3,
            "app": "PMO",
            "title": "Project Status Executive Summary Actions",
            "description": "ctions are tasks or steps required to advance the project. The system searches for actions based on parameters such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            "generatorType": "ProjectStatus",
            "key": "EXECUTIVE_SUMMARY_ACTIONS",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 4,
            "app": "PMO",
            "title": "Project Status Executive Summary Decisions",
            "description": "Decisions are crucial choices that need to be made about the project's direction. The system searches for pending decisions based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            "generatorType": "ProjectStatus",
            "key": "EXECUTIVE_SUMMARY_DECISIONS",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 5,
            "app": "PMO",
            "title": "Project Status Executive Summary Interdependencies",
            "description": "The system identifies high-priority interdependencies that need prompt attention. It searches based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, and the receiver or provider project team selected in the UI.",
            "generatorType": "ProjectStatus",
            "key": "EXECUTIVE_SUMMARY_INTERDEPENDENCIES",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 6,
            "app": "PMO",
            "title": "Project Status Accomplishments Workplan",
            "description": "The Workplan section showcases milestones or tasks completed in the past week. The system filters these accomplishments by looking for a closed status, the project team selected in the UI, high, medium, low, and null priorities, completion within the last seven days, a hierarchy level of three or less, and types including tasks and milestones.",
            "generatorType": "ProjectStatus",
            "key": "ACCOMPLISHMENTS_WORKPLAN",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 7,
            "app": "PMO",
            "title": "Project Status Accomplishments Risks and Issues",
            "description": "This subsection identifies risks and issues resolved in the past week. The search criteria include a closed status, the project team selected in the UI, high, medium, and low priorities, and resolution within the last seven days.",
            "generatorType": "ProjectStatus",
            "key": "ACCOMPLISHMENTS_RISKS_ISSUES",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 8,
            "app": "PMO",
            "title": "Project Status Accomplishments Actions",
            "description": "The system searches for actions completed in the past week using parameters such as a closed status, the project team selected in the UI, high, medium, and low priorities, and completion within the last seven days.",
            "generatorType": "ProjectStatus",
            "key": "ACCOMPLISHMENTS_ACTIONS",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 9,
            "app": "PMO",
            "title": "Project Status Accomplishments Decisions",
            "description": "Decisions made and closed in the past week are identified using criteria such as a closed status, the project team selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            "generatorType": "ProjectStatus",
            "key": "ACCOMPLISHMENTS_DECISIONS",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 10,
            "app": "PMO",
            "title": "Project Status Accomplishments Interdependencies",
            "description": "The system identifies high-priority interdependencies that need prompt attention. The search criteria include a closed status, the receiver and provider project teams selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            "generatorType": "ProjectStatus",
            "key": "ACCOMPLISHMENTS_INTERDEPENDENCIES",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 11,
            "app": "PMO",
            "title": "Project Status Next Steps Workplan",
            "description": "The Workplan section outlines the upcoming milestones or tasks for the next week. The system searches for these items using filters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, due dates within the last seven days and the next seven days, and types including tasks and milestones.",
            "generatorType": "ProjectStatus",
            "key": "NEXT_STEPS_WORKPLAN",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 12,
            "app": "PMO",
            "title": "Project Status Next Steps Risks and Issues",
            "description": "Upcoming risks and issues expected to be addressed in the next week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            "generatorType": "ProjectStatus",
            "key": "NEXT_STEPS_RISKS_ISSUES",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 13,
            "app": "PMO",
            "title": "Project Status Next Steps Actions",
            "description": "The system searches for actions on track to be completed in the next week using parameters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            "generatorType": "ProjectStatus",
            "key": "NEXT_STEPS_ACTIONS",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 14,
            "app": "PMO",
            "title": "Project Status Next Steps Decisions",
            "description": "Decisions expected to be made in the upcoming week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            "generatorType": "ProjectStatus",
            "key": "NEXT_STEPS_DECISIONS",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 15,
            "app": "PMO",
            "title": "Project Status Next Steps Interdependencies",
            "description": "The system identifies high-priority interdependencies that need prompt attention using criteria such as a status that is not completed, closed, or cancelled, critical, high, medium, and low priorities, the receiver and provider project teams selected in the UI, and due dates within the last seven days and the next seven days.",
            "generatorType": "ProjectStatus",
            "key": "NEXT_STEPS_INTERDEPENDENCIES",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
        {
            "id": 16,
            "app": "PMO",
            "title": "Project Status Overall Status",
            "description": 'The Project Status section is crucial for understanding the overall health of the project. The system evaluates the project\'s history to determine the overall status as: - At Risk: If any item is marked as \\" At Risk \\ ". - Behind Schedule: If any item is marked as \\" Behind Schedule \\ ". - On Track: If no items are marked as \\" At Risk \\ " or \\" Behind Schedule \\ ".',
            "generatorType": "ProjectStatus",
            "key": "PROJECT_STATUS_OVERALL_STATUS",
            "sqlQuery": "select * from table",
            "isActive": "true",
            "createdAt": "null",
            "updatedAt": "null",
            "createdBy": "System",
            "updatedBy": "System",
        },
    ]
    input_data = StatusReportInput(
        instanceId=UUID("6bd23cc3-9f08-42d6-910b-32975a232d9a"),
        projectTeam=ProjectTeam(title="HR", id="59"),
        reportingPeriod=ReportingPeriod(
            id="1111", 
            title="2024/10/22 - 2024/10/28",
            periodStartDate="2024-10-22",
            periodEndDate="2024-10-28"
        ),
        projectDetails=ProjectDetails(
            sector="Information Technology",
            subSector="Software",
            transactionType="Buy&Integrate",
        ),
    )
    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    await ctx.set("input", input_data)
    ev = GetQueries(
        sections={
            "EXECUTIVE_SUMMARY": "executive-summary",
            "OVERALL_STATUS": "overall-status",
            "ACCOMPLISHMENTS": "accomplishments",
            "NEXT_STEPS": "next-steps",
        },
        tables={
            "WORKPLAN": "Workplan",
            "RISKS_ISSUES": "Risks and Issues",
            "ACTIONS": "Actions",
            "DECISIONS": "Decisions",
            "INTERDEPENDENCIES": "Interdependencies",
            "OVERALL_STATUS": "Workplan",
        },
        project_team="59",
        start_date="2024-10-22",
        end_date="2024-10-28"
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    
    # Mock format_queries to avoid NaN error
    with patch("app.status_report.services.workflow.steps.get_queries.format_queries") as mock_format:
        mock_format.return_value = {
            "executive-summary": {
                "Workplan": {"sqlQuery": "select * from table", "description": "Description"},
                "Risks and Issues": {"sqlQuery": "select * from table", "description": "Description"},
            },
            "overall-status": {"Workplan": {"sqlQuery": "select * from table", "description": "Description"}},
            "accomplishments": {"Workplan": {"sqlQuery": "select * from table", "description": "Description"}},
            "next-steps": {"Workplan": {"sqlQuery": "select * from table", "description": "Description"}}
        }
        
        result = await workflow.fetch_data(ctx, ev)

        assert isinstance(result, CreateReport)


@pytest.mark.asyncio
async def test_create_report(workflow) -> None:
    from app.status_report.services.workflow.events.create_report import CreateReport
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = CreateReport(
        data={}, 
        project_team="team",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.create_report(ctx, ev)

    # Verify that events are sent
    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("OverallStatus" in item for item in broker_log)
    assert any("ExecutiveSummaryRetrieveSQLQueries" in item for item in broker_log)
    assert any("AccomplishmentsRetrieveSQLQueries" in item for item in broker_log)
    assert any("NextStepsRetrieveSQLQueries" in item for item in broker_log)


@pytest.mark.asyncio
async def test_overall_status_retrieve_sql_queries(workflow):
    from app.status_report.services.workflow.events.overall_status_retrieve_sql_queries import (
        OverallStatusRetrieveSQLQueries,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    data = {
        "executive-summary": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The workplan section focuses on milestones and tasks that need immediate attention. The system filters them based on criteria such as a timeframe of seven days before or after today's date, a status that is not cancelled, the project team selected in the UI, all priority levels with emphasis on critical, high, medium, low, and null, a hierarchy level of three or less, and types including milestones and tasks.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority risks and issues that need prompt attention. It filters based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with emphasis on critical, high, medium, low, and null",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "ctions are tasks or steps required to advance the project. The system searches for actions based on parameters such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions are crucial choices that need to be made about the project's direction. The system searches for pending decisions based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. It searches based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, and the receiver or provider project team selected in the UI.",
            },
        },
        "overall-status": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": 'The Project Status section is crucial for understanding the overall health of the project. The system evaluates the project\'s history to determine the overall status as: - At Risk: If any item is marked as \\" At Risk \\ ". - Behind Schedule: If any item is marked as \\" Behind Schedule \\ ". - On Track: If no items are marked as \\" At Risk \\ " or \\" Behind Schedule \\ ".',
            }
        },
        "accomplishments": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section showcases milestones or tasks completed in the past week. The system filters these accomplishments by looking for a closed status, the project team selected in the UI, high, medium, low, and null priorities, completion within the last seven days, a hierarchy level of three or less, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "This subsection identifies risks and issues resolved in the past week. The search criteria include a closed status, the project team selected in the UI, high, medium, and low priorities, and resolution within the last seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions completed in the past week using parameters such as a closed status, the project team selected in the UI, high, medium, and low priorities, and completion within the last seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions made and closed in the past week are identified using criteria such as a closed status, the project team selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. The search criteria include a closed status, the receiver and provider project teams selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
        },
        "next-steps": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section outlines the upcoming milestones or tasks for the next week. The system searches for these items using filters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, due dates within the last seven days and the next seven days, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "Upcoming risks and issues expected to be addressed in the next week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions on track to be completed in the next week using parameters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions expected to be made in the upcoming week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention using criteria such as a status that is not completed, closed, or cancelled, critical, high, medium, and low priorities, the receiver and provider project teams selected in the UI, and due dates within the last seven days and the next seven days.",
            },
        },
    }
    await ctx.set("data", data)
    ev = OverallStatusRetrieveSQLQueries(
        project_team="59",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.overall_status_retrieve_sql_queries(ctx, ev)

    # Verify that events are sent
    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("OverallStatusRunSQL" in item for item in broker_log)


@pytest.mark.asyncio
async def test_overall_status_run_sql(workflow):
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.overall_status_results import (
        OverallStatusResults,
    )
    from app.status_report.services.workflow.events.overall_status_run_sql import (
        OverallStatusRunSQL,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = OverallStatusRunSQL(
        query_and_rules={
            "sqlQuery": "select * from table",
            "description": "Test description",
        },
        project_team="59",
        table="Workplan",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.return_value = ProgramOfficeResponse(
        status_code="200",
        data=[
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "RisksTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Issue",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
        ],
   )

    result = await workflow.overall_status_run_sql(ctx, ev)

    assert isinstance(result, OverallStatusResults)


@pytest.mark.asyncio
async def test_overall_status_join_queries_results(workflow):
    from app.status_report.services.workflow.events.overall_status_results import (
        OverallStatusResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = OverallStatusResults(
        project_team="59",
        source="overall-status",
        response="{'IssueRiskCategory': 'Risk', 'ItemStatusId': 21, 'ItemDescription': None, 'ItemDueDate': '2024-12-26T12:00:00+00:00', 'ItemPriority': 'Medium', 'Title': 'test157', 'ReportingLevelId': None, 'ItemCategory': 'Was past due current week', 'StatusTitle': 'On Track'}",
        rules="rules",
        source_value={"key": "value"},
        table="Workplan",
    )
    await ctx.set("length_overall_status", 1)

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    result = await workflow.overall_status_join_queries_results(ctx, ev)

    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("OverallStatus" in item for item in broker_log)


@pytest.mark.asyncio
async def test_overall_status(workflow):
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.general_results import (
        GeneralResults,
    )
    from app.status_report.services.workflow.events.overall_status import OverallStatus
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = OverallStatus(
        project_team="59",
        sql_results=[
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 21,
                "ItemDescription": "None",
                "ItemDueDate": "2024-12-26T12:00:00+00:00",
                "ItemPriority": "Medium",
                "Title": "test157",
                "ReportingLevelId": "None",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "On Track",
            },
            {
                "Title": "Test Actiona 112",
                "ItemDescription": "None",
                "ProjectTeamId": 59,
                "ItemPriority": "Critical",
                "ItemDueDate": "2024-12-26T12:00:00+00:00",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "On Track",
            },
            {
                "Title": "Test Actiona 116",
                "ItemDescription": "None",
                "ProjectTeamId": 59,
                "ItemPriority": "Critical",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "On Track",
            },
        ],
        citing_sources=[],  # Changed from {} to []
    )
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.return_value = ProgramOfficeResponse(
        data=[{"Id": "1011"}], status_code="200"
    )

    result = await workflow.overall_status(ctx, ev)

    assert isinstance(result, GeneralResults)


@pytest.mark.asyncio
async def test_executive_summary_retrieve_sql_queries(workflow) -> None:
    from app.status_report.services.workflow.events.executive_summary_retrieve_sql_queries import (
        ExecutiveSummaryRetrieveSQLQueries,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    data = {
        "executive-summary": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The workplan section focuses on milestones and tasks that need immediate attention. The system filters them based on criteria such as a timeframe of seven days before or after today's date, a status that is not cancelled, the project team selected in the UI, all priority levels with emphasis on critical, high, medium, low, and null, a hierarchy level of three or less, and types including milestones and tasks.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority risks and issues that need prompt attention. It filters based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with emphasis on critical, high, medium, low, and null",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "ctions are tasks or steps required to advance the project. The system searches for actions based on parameters such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions are crucial choices that need to be made about the project's direction. The system searches for pending decisions based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. It searches based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, and the receiver or provider project team selected in the UI.",
            },
        },
        "overall-status": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": 'The Project Status section is crucial for understanding the overall health of the project. The system evaluates the project\'s history to determine the overall status as: - At Risk: If any item is marked as \\" At Risk \\ ". - Behind Schedule: If any item is marked as \\" Behind Schedule \\ ". - On Track: If no items are marked as \\" At Risk \\ " or \\" Behind Schedule \\ ".',
            }
        },
        "accomplishments": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section showcases milestones or tasks completed in the past week. The system filters these accomplishments by looking for a closed status, the project team selected in the UI, high, medium, low, and null priorities, completion within the last seven days, a hierarchy level of three or less, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "This subsection identifies risks and issues resolved in the past week. The search criteria include a closed status, the project team selected in the UI, high, medium, and low priorities, and resolution within the last seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions completed in the past week using parameters such as a closed status, the project team selected in the UI, high, medium, and low priorities, and completion within the last seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions made and closed in the past week are identified using criteria such as a closed status, the project team selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. The search criteria include a closed status, the receiver and provider project teams selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
        },
        "next-steps": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section outlines the upcoming milestones or tasks for the next week. The system searches for these items using filters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, due dates within the last seven days and the next seven days, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "Upcoming risks and issues expected to be addressed in the next week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions on track to be completed in the next week using parameters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions expected to be made in the upcoming week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention using criteria such as a status that is not completed, closed, or cancelled, critical, high, medium, and low priorities, the receiver and provider project teams selected in the UI, and due dates within the last seven days and the next seven days.",
            },
        },
    }
    await ctx.set("data", data)
    ev = ExecutiveSummaryRetrieveSQLQueries(
        project_team="59",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.executive_summary_retrieve_sql_queries(ctx, ev)

    # Verify that events are sent
    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("ExecutiveSummaryRunSQL" in item for item in broker_log)


@pytest.mark.asyncio
async def test_executive_summary_run_sql(workflow) -> None:
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.execute_summary_run_sql import (
        ExecutiveSummaryRunSQL,
    )
    from app.status_report.services.workflow.events.executive_summary_results import (
        ExecutiveSummaryResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = ExecutiveSummaryRunSQL(
        query_and_rules={
            "sqlQuery": "select * from table",
            "description": "Test description",
        },
        project_team="59",
        table="Workplan",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.return_value = ProgramOfficeResponse(
        status_code="200",
        data=[
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "RisksTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Issue",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
        ],
    )

    result = await workflow.executive_summary_run_sql(ctx, ev)

    assert isinstance(result, ExecutiveSummaryResults)


@pytest.mark.asyncio
async def test_executive_summary_join_queries_results(workflow):
    from app.status_report.services.workflow.events.executive_summary_results import (
        ExecutiveSummaryResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = ExecutiveSummaryResults(
        project_team="59",
        source="source",
        response="response",
        rules="rules",
        source_value={"key": "value"},
        table="Workplan",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )
    await ctx.set("length_executive_summary", 1)

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.executive_summary_join_queries_results(ctx, ev)

    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("ExecutiveSummary" in item for item in broker_log)


@pytest.mark.asyncio
async def test_executive_summary(workflow):
    from app.core.program_office_api import ProgramOfficeResponse  # Fixed import path
    from app.status_report.services.workflow.events.executive_summary import (
        ExecutiveSummary,
    )
    from app.status_report.services.workflow.events.executive_summary_tone_mimic import (
        ExecuteSummaryToneMimic,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = ExecutiveSummary(
        project_team="59", 
        es_queries_result="result", 
        citing_sources=[]  # Changed from {} to [] based on previous fixes
    )
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.side_effect = [
        ProgramOfficeResponse(
            status_code="200",
            data=[{"MaxCharLimit": 1000}],
        ),
    ]

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.copilot_api.get_project_context = AsyncMock()
    workflow.copilot_api.get_project_context.return_value = {
        "value": "The name of this project is: HR"
    }
    workflow.llm.acomplete = AsyncMock()
    workflow.llm.acomplete.return_value = MagicMock(text="response")

    result = await workflow.executive_summary(ctx, ev)

    assert isinstance(result, ExecuteSummaryToneMimic)


@pytest.mark.asyncio
async def test_executive_summary_tone_mimic(workflow):
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.executive_summary_tone_mimic import (
        ExecuteSummaryToneMimic,
    )
    from app.status_report.services.workflow.events.general_results import (
        GeneralResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = ExecuteSummaryToneMimic(
        project_team="59",
        source="source",
        response="response",
        citing_sources=[{"sourceName": "source"}],  # Changed to a list with a dictionary
        num_words=250,
    )
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.side_effect = [
        ProgramOfficeResponse(
            status_code="200",
            data=["Example1", "Example2", "Example3"],
        ),
        ProgramOfficeResponse(
            status_code="200",
            data=[{"MaxCharLimit": 1000}],
        ),
    ]

    workflow.llm.acomplete = AsyncMock()
    workflow.llm.acomplete.return_value = MagicMock(text="response")

    result = await workflow.executive_summary_tone_mimic(ctx, ev)

    assert isinstance(result, GeneralResults)


@pytest.mark.asyncio
async def test_accomplishments_retrieve_sql_queries(workflow):
    from app.status_report.services.workflow.events.accomplishments_retrieve_sql_queries import (
        AccomplishmentsRetrieveSQLQueries,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    data = {
        "executive-summary": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The workplan section focuses on milestones and tasks that need immediate attention. The system filters them based on criteria such as a timeframe of seven days before or after today's date, a status that is not cancelled, the project team selected in the UI, all priority levels with emphasis on critical, high, medium, low, and null, a hierarchy level of three or less, and types including milestones and tasks.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority risks and issues that need prompt attention. It filters based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with emphasis on critical, high, medium, low, and null",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "ctions are tasks or steps required to advance the project. The system searches for actions based on parameters such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions are crucial choices that need to be made about the project's direction. The system searches for pending decisions based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. It searches based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, and the receiver or provider project team selected in the UI.",
            },
        },
        "overall-status": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": 'The Project Status section is crucial for understanding the overall health of the project. The system evaluates the project\'s history to determine the overall status as: - At Risk: If any item is marked as \\" At Risk \\ ". - Behind Schedule: If any item is marked as \\" Behind Schedule \\ ". - On Track: If no items are marked as \\" At Risk \\ " or \\" Behind Schedule \\ ".',
            }
        },
        "accomplishments": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section showcases milestones or tasks completed in the past week. The system filters these accomplishments by looking for a closed status, the project team selected in the UI, high, medium, low, and null priorities, completion within the last seven days, a hierarchy level of three or less, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "This subsection identifies risks and issues resolved in the past week. The search criteria include a closed status, the project team selected in the UI, high, medium, and low priorities, and resolution within the last seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions completed in the past week using parameters such as a closed status, the project team selected in the UI, high, medium, and low priorities, and completion within the last seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions made and closed in the past week are identified using criteria such as a closed status, the project team selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. The search criteria include a closed status, the receiver and provider project teams selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
        },
        "next-steps": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section outlines the upcoming milestones or tasks for the next week. The system searches for these items using filters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, due dates within the last seven days and the next seven days, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "Upcoming risks and issues expected to be addressed in the next week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions on track to be completed in the next week using parameters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions expected to be made in the upcoming week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention using criteria such as a status that is not completed, closed, or cancelled, critical, high, medium, and low priorities, the receiver and provider project teams selected in the UI, and due dates within the last seven days and the next seven days.",
            },
        },
    }
    await ctx.set("data", data)
    ev = AccomplishmentsRetrieveSQLQueries(
        project_team="59",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.accomplishments_retrieve_sql_queries(ctx, ev)

    # Verify that events are sent
    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("AccomplishmentsRunSQL" in item for item in broker_log)


@pytest.mark.asyncio
async def test_accomplishments_run_sql(workflow):
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.accomplishments_run_sql import (
        AccomplishmentsRunSQL,
    )
    from app.status_report.services.workflow.events.accomplisments_results import (
        AccomplishmentsResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = AccomplishmentsRunSQL(
        query_and_rules={
            "sqlQuery": "select * from table",
            "description": "Test description",
        },
        project_team="59",
        table="Workplan",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.return_value = ProgramOfficeResponse(
        status_code="200",
        data=[
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "RisksTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Issue",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
        ],
    )

    result = await workflow.accomplishments_run_sql(ctx, ev)

    assert isinstance(result, AccomplishmentsResults)


@pytest.mark.asyncio
async def test_accomplishments_join_queries_results(workflow):
    from app.status_report.services.workflow.events.accomplisments_results import (
        AccomplishmentsResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = AccomplishmentsResults(
        project_team="59",
        source="source",
        response="response",
        rules="rules",
        source_value={"key": "value"},
        table="Workplan",
    )
    await ctx.set("length_accomplishments", 1)

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.accomplishments_join_queries_results(ctx, ev)

    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("Accomplishments" in item for item in broker_log)


@pytest.mark.asyncio
async def test_accomplishments(workflow):
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.accomplisments import (
        Accomplishments,
    )
    from app.status_report.services.workflow.events.general_results import (
        GeneralResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = Accomplishments(
        project_team="59",
        es_queries_result="result",
        citing_sources=[{"sourceName": "source"}],  # Changed from dict to list containing a dict
    )
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.copilot_api.get_project_context = AsyncMock()
    workflow.copilot_api.get_project_context.return_value = {
        "value": "The name of this project is: HR"
    }
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.return_value = ProgramOfficeResponse(
        status_code="200",
        data=[{"MaxCharLimit": 1000}],
    )
    workflow.llm.acomplete = AsyncMock()
    workflow.llm.acomplete.return_value = MagicMock(text='{"text": "response"}')

    result = await workflow.accomplishments(ctx, ev)

    assert isinstance(result, GeneralResults)


@pytest.mark.asyncio
async def test_next_steps_retrieve_sql_queries(workflow):
    from app.status_report.services.workflow.events.next_steps_retrieve_sql_queries import (
        NextStepsRetrieveSQLQueries,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    data = {
        "executive-summary": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The workplan section focuses on milestones and tasks that need immediate attention. The system filters them based on criteria such as a timeframe of seven days before or after today's date, a status that is not cancelled, the project team selected in the UI, all priority levels with emphasis on critical, high, medium, low, and null, a hierarchy level of three or less, and types including milestones and tasks.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority risks and issues that need prompt attention. It filters based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with emphasis on critical, high, medium, low, and null",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "ctions are tasks or steps required to advance the project. The system searches for actions based on parameters such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions are crucial choices that need to be made about the project's direction. The system searches for pending decisions based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. It searches based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, and the receiver or provider project team selected in the UI.",
            },
        },
        "overall-status": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": 'The Project Status section is crucial for understanding the overall health of the project. The system evaluates the project\'s history to determine the overall status as: - At Risk: If any item is marked as \\" At Risk \\ ". - Behind Schedule: If any item is marked as \\" Behind Schedule \\ ". - On Track: If no items are marked as \\" At Risk \\ " or \\" Behind Schedule \\ ".',
            }
        },
        "accomplishments": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section showcases milestones or tasks completed in the past week. The system filters these accomplishments by looking for a closed status, the project team selected in the UI, high, medium, low, and null priorities, completion within the last seven days, a hierarchy level of three or less, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "This subsection identifies risks and issues resolved in the past week. The search criteria include a closed status, the project team selected in the UI, high, medium, and low priorities, and resolution within the last seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions completed in the past week using parameters such as a closed status, the project team selected in the UI, high, medium, and low priorities, and completion within the last seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions made and closed in the past week are identified using criteria such as a closed status, the project team selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention. The search criteria include a closed status, the receiver and provider project teams selected in the UI, high, medium, and low priorities, and closure within the last seven days.",
            },
        },
        "next-steps": {
            "Workplan": {
                "sqlQuery": "select * from table",
                "description": "The Workplan section outlines the upcoming milestones or tasks for the next week. The system searches for these items using filters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, due dates within the last seven days and the next seven days, and types including tasks and milestones.",
            },
            "Risks and Issues": {
                "sqlQuery": "select * from table",
                "description": "Upcoming risks and issues expected to be addressed in the next week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Actions": {
                "sqlQuery": "select * from table",
                "description": "The system searches for actions on track to be completed in the next week using parameters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Decisions": {
                "sqlQuery": "select * from table",
                "description": "Decisions expected to be made in the upcoming week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.",
            },
            "Interdependencies": {
                "sqlQuery": "select * from table",
                "description": "The system identifies high-priority interdependencies that need prompt attention using criteria such as a status that is not completed, closed, or cancelled, critical, high, medium, and low priorities, the receiver and provider project teams selected in the UI, and due dates within the last seven days and the next seven days.",
            },
        },
    }
    await ctx.set("data", data)
    ev = NextStepsRetrieveSQLQueries(
        project_team="59",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.next_steps_retrieve_sql_queries(ctx, ev)

    # Verify that events are sent
    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("NextStepsRunSQL" in item for item in broker_log)


@pytest.mark.asyncio
async def test_next_steps_run_sql(workflow):
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.next_steps_results import (
        NextStepsResults,
    )
    from app.status_report.services.workflow.events.next_steps_run_sql import (
        NextStepsRunSQL,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = NextStepsRunSQL(
        query_and_rules={
            "sqlQuery": "select * from table",
            "description": "Test description",
        },
        project_team="59",
        table="Workplan",
        start_date="2024-10-22",  # Added required field
        end_date="2024-10-28"     # Added required field
    )

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.return_value = ProgramOfficeResponse(
        status_code="200",
        data=[
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "RisksTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Risk",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
            {
                "IssueRiskCategory": "Issue",
                "ItemStatusId": 25,
                "ItemDescription": "null",
                "ItemDueDate": "2024-12-30T12:00:00+00:00",
                "ItemPriority": "null",
                "Title": "IssuesTest302",
                "ReportingLevelId": "null",
                "ItemCategory": "Was past due current week",
                "StatusTitle": "Pending",
            },
        ],
    )

    result = await workflow.next_steps_run_sql(ctx, ev)

    assert isinstance(result, NextStepsResults)


@pytest.mark.asyncio
async def test_next_steps_join_queries_results(workflow):
    from app.status_report.services.workflow.events.next_steps_results import (
        NextStepsResults,
    )
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = NextStepsResults(
        project_team="59",
        source="source",
        response="response",
        rules="rules",
        source_value={"key": "value"},
        table="Workplan",
    )
    await ctx.set("length_next_steps", 1)

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    await workflow.next_steps_join_queries_results(ctx, ev)

    broker_log = ctx.to_dict().get("broker_log", [])
    assert any("NextSteps" in item for item in broker_log)


@pytest.mark.asyncio
async def test_next_steps(workflow):
    from app.core.program_office_api import ProgramOfficeResponse
    from app.status_report.services.workflow.events.general_results import (
        GeneralResults,
    )
    from app.status_report.services.workflow.events.next_steps import NextSteps
    from llama_index.core.workflow import Context

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")
    ev = NextSteps(
        project_team="59",
        es_queries_result="result",
        citing_sources=[{"sourceName": "source"}],  # Changed from dict to list containing a dict
        start_date="2024-10-22",
        end_date="2024-10-28"
    )
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    workflow.copilot_api.get_project_context = AsyncMock()
    workflow.copilot_api.get_project_context.return_value = {
        "value": "The name of this project is: HR"
    }
    workflow.program_office.run_sql = AsyncMock()
    workflow.program_office.run_sql.return_value = ProgramOfficeResponse(
        status_code="200",
        data=[{"MaxCharLimit": 1000}],
    )
    workflow.llm.acomplete = AsyncMock()
    workflow.llm.acomplete.return_value = MagicMock(text='{"text": "response"}')

    result = await workflow.next_steps(ctx, ev)

    assert isinstance(result, GeneralResults)


@pytest.mark.asyncio
async def test_join_reports(workflow):
    from app.status_report.services.workflow.events.general_results import (
        GeneralResults,
    )
    from llama_index.core.workflow import Context, StopEvent

    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "6bd23cc3-9f08-42d6-910b-32975a232d9a")

    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    result = await workflow.join_reports(ctx, GeneralResults(source={"key": "1"}))
    result = await workflow.join_reports(ctx, GeneralResults(source={"key": "2"}))
    result = await workflow.join_reports(ctx, GeneralResults(source={"key": "3"}))
    result = await workflow.join_reports(ctx, GeneralResults(source={"key": "4"}))

    assert isinstance(result, StopEvent)
