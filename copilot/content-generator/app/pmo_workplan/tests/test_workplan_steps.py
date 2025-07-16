from unittest.mock import AsyncMock, MagicMock, patch

import pytest
from llama_index.core.workflow import Context, StartEvent


@pytest.fixture(scope="function", autouse=False)
def dependencies():
    dependencies = {
        "llm": MagicMock(),
        "embed_model": MagicMock(),
        "ai_search": MagicMock(),
        "redis": MagicMock(),
        "asset_manager_engine": MagicMock(),
    }
    return dependencies


@pytest.mark.asyncio
async def test_step_process_input_return_search_team_event(dependencies):
    from app.pmo_workplan.services.workflow.events.search_team_event import (
        SearchTeamEvent,
    )
    from app.pmo_workplan.services.workflow.workplan_generator_workflow import (
        WorkplanGeneratorWorkflow,
    )

    # Mock Azure key vault client to prevent the real call
    with patch("azure.keyvault.secrets._client.SecretClient.get_secret") as mock_get_secret:
        # Configure the mock to return a fake secret
        mock_secret = MagicMock()
        mock_secret.value = "fake-secret-value"
        mock_get_secret.return_value = mock_secret
        
        # Now initialize the workflow with key vault mocked
        workflow = WorkplanGeneratorWorkflow(**dependencies)

    workflow.llm.acomplete = AsyncMock()
    workflow.llm.acomplete.return_value = MagicMock(
        text='["Information Technology", "IT"]'
    )
    ctx = Context(workflow=workflow)
    input_ = {
        "duration_in_month": 12,
        "instance_id": "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf",
        "project_details": {
            "subSector": "Healthcare Technology",
            "sector": "Healthcare",
            "transactionType": "Sell & Separate",
        },
        "document_ids": [
            "b3cad6d7-aa82-45d6-97af-17f22484d7ac",
            "ca2ef4ae-778e-487e-a4c0-4e7a45612494",
            "40f0c817-9259-4ce4-b3c7-4ca821006774",
            "4497bc30-6e73-4729-a153-fba9bdde9d95",
        ],
        "team": "IT",
        "project_outline": "This is a test project",
    }
    ev = StartEvent(value=input_)
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)

    # Act
    response = await workflow.step_process_input(ev, ctx)

    # Assert
    assert isinstance(response, SearchTeamEvent)


@pytest.mark.asyncio
async def test_step_generate_sql_for_teamtasks_builds_the_sql(dependencies):
    from app.core.asset_manager import AssetManager
    from app.pmo_workplan.services.workflow.events.search_team_event import (
        SearchTeamEvent,
    )
    from app.pmo_workplan.services.workflow.workplan_generator_workflow import (
        WorkplanGeneratorWorkflow,
    )

    # Mock Azure key vault client to prevent the real call
    with patch("azure.keyvault.secrets._client.SecretClient.get_secret") as mock_get_secret:
        # Configure the mock to return a fake secret
        mock_secret = MagicMock()
        mock_secret.value = "fake-secret-value"
        mock_get_secret.return_value = mock_secret
        
        # Now initialize the workflow with key vault mocked
        workflow = WorkplanGeneratorWorkflow(**dependencies)
        
    workflow.asset_manager = MagicMock()
    workflow.asset_manager.__class__ = AssetManager
    # Updated to return 3 values as expected by the function
    build_query_mock = MagicMock(side_effect=[("select * from table", {}, "replaced_query")])
    workflow.asset_manager.build_query = build_query_mock
    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf")
    await ctx.set("workplan_params", {})
    ev = SearchTeamEvent(project_team=["IT"])
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    # Act
    response = await workflow.step_generate_sql_for_teamtasks(ev, ctx)
    # Assert
    assert response.sql_generated == "select * from table"
    assert response.query_params == {}


@pytest.mark.asyncio
async def test_step_validate_sql_has_no_errors(dependencies):
    from app.core.asset_manager import AssetManager
    from app.pmo_workplan.services.workflow.events.generated_sql_event import (
        GeneratedSQLEvent,
    )
    from app.pmo_workplan.services.workflow.events.good_sql_event import GoodSQLEvent
    from app.pmo_workplan.services.workflow.workplan_generator_workflow import (
        WorkplanGeneratorWorkflow,
    )

    # Arrange
    workflow = WorkplanGeneratorWorkflow(**dependencies)
    workflow.asset_manager = MagicMock()
    workflow.asset_manager.__class__ = AssetManager
    execute_query_mock = AsyncMock(
        side_effect=[
            (
                {
                    "title": None,
                    "parenttask": None,
                    "projectteam": None,
                    "workplantasktype": None,
                }
            )
        ]
    )
    workflow.asset_manager.execute_query = execute_query_mock
    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf")
    event_data = {
        "sql_generated": "select * from table",
        "query_params": {},
        "sql_object": workflow.asset_manager,
    }
    ev = GeneratedSQLEvent(**event_data)
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    # Act
    response = await workflow.step_validate_sql(ev, ctx)
    # Assert
    assert isinstance(response, GoodSQLEvent)


@pytest.mark.asyncio
async def test_step_proccess_eyip_template_flatten_the_template(dependencies):
    from app.pmo_workplan.services.workflow.events.eyip_templates_event import (
        EYIPTemplatesEvent,
    )
    from app.pmo_workplan.services.workflow.workplan_generator_workflow import (
        WorkplanGeneratorWorkflow,
    )

    # Arrange
    workflow = WorkplanGeneratorWorkflow(**dependencies)
    ctx = Context(workflow=workflow)
    await ctx.set("instanceId", "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf")
    await ctx.set("project_outline", "This is a test project")
    await ctx.set("project_team", "IT")
    workflow.llm.acomplete = AsyncMock()
    workflow.llm.acomplete.return_value = MagicMock(
        text="""
          [
                {
                    "summary_task": "Detailed summary task header goes here",
                    "tasks": [
                        "Task description 1",
                        "Task description 2",
                        "Task description 3",
                        "Task description 4",
                        "Task description 5",
                        "Task description 6",
                        "Task description 7",
                        "Task description 8",
                        "Task description 9",
                        "Task description 10",
                        "Task description 11",
                        "Task description 12",
                        "Task description 13",
                        "Task description 14",
                        "Task description 15",
                    ],
                    "milestone": "Milestone description header goes here"
                },
          ]
        """
    )
    sql_output_to_dict_mock = MagicMock(to_dict={})
    ev = MagicMock()
    ev.sql_output.return_value = sql_output_to_dict_mock
    workflow.redis.is_workflow_aborted = MagicMock(return_value=False)
    # Act
    response = await workflow.step_process_eyip_templates(ev, ctx)
    # Assert
    assert isinstance(response, EYIPTemplatesEvent)
