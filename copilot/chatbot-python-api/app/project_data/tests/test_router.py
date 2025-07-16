from unittest.mock import patch
from contextlib import ExitStack
from uuid import uuid4
from .mock_configs import (
    configure_azure_llm_mock,
    configure_azure_llm_mock_complex_data,
    configure_jwt_decode_mock,
    configure_query_pipeline_mock,
    configure_search_client_mock,
    configure_complex_search_client_mock,
    get_additional_patches
)
from .mock_data import get_query_payload, get_complex_query_data

def test_query_pipeline(
        client, 
        azure_llm_model,
        jwt_decode_mock,
        mock_llama_query_pipeline,
        mock_azure_async_search_client):
    
    # Configurar todos los mocks
    configure_azure_llm_mock(azure_llm_model)
    configure_jwt_decode_mock(jwt_decode_mock)
    configure_query_pipeline_mock(mock_llama_query_pipeline)
    configure_search_client_mock(mock_azure_async_search_client)
    
    # Aplicar patches adicionales usando ExitStack
    with ExitStack() as stack:
        # Aplicar cada patch a la pila
        for patch_obj in get_additional_patches():
            stack.enter_context(patch_obj)
        
        # Test con los patches aplicados
        payload = get_query_payload()
        
        response = client.post(
            "/project-data", 
            json=payload, 
            headers={"Authorization": "Bearer 123"}
        )
        
        assert response.status_code == 200


def test_query_pipeline_with_memory(
        client, 
        azure_llm_model,
        jwt_decode_mock,
        mock_llama_query_pipeline,
        mock_azure_async_search_client):
    
    # Configurar todos los mocks
    configure_azure_llm_mock(azure_llm_model)
    configure_jwt_decode_mock(jwt_decode_mock)
    configure_query_pipeline_mock(mock_llama_query_pipeline)
    configure_search_client_mock(mock_azure_async_search_client)
    with (
        patch(
            "sql_metadata.Parser.tables", return_value=["MockedValue1", "MockedValue2"]
        ),
        patch(
            "app.project_data.services.program_office_api.ProgramOffice.run_query",
            return_value=[{"Title": "Value1"}, {"Title": "Value2"}],
        ),
        patch(
            "app.project_data.functions.table_keys.get_widgets",
            return_value={"MockedValue2": ["key2, key3"]},
        ),
    ):
        payload = {
            "chatId": str(uuid4()),
            "instanceId": str(uuid4()),
            "question": "How many tasks does each team have?",
            "chatHistory": [
                {
                    "messageId": 0,
                    "role": "user",
                    "content": "string",
                }
            ],
            "context": {
                "user": {
                    "email": "test@test.com",
                },
                "appInfo":{
                    "name": "Project Management",
                    "teamTypeIds": [1]
                },
            },
        }

        response = client.post(
            "/project-data", json=payload, headers={"Authorization": "Bearer 123"}
        )

        assert response.status_code == 200


def test_query_pipeline_with_suggestion(
        client, 
        azure_llm_model,
        jwt_decode_mock,
        mock_llama_query_pipeline,
        mock_azure_async_search_client):
    
    # Configure all mocks
    configure_azure_llm_mock(azure_llm_model)
    configure_jwt_decode_mock(jwt_decode_mock)
    configure_query_pipeline_mock(mock_llama_query_pipeline)
    configure_search_client_mock(mock_azure_async_search_client)
    
    # Apply additional patches using ExitStack
    with ExitStack() as stack:
        # Apply each patch to the stack
        for patch_obj in get_additional_patches():
            stack.enter_context(patch_obj)
        
        # Get base payload and add suggestion with SQL query
        payload = get_query_payload()
        payload["context"]["suggestion"] = {
            "sqlQuery": "SELECT COUNT(*) FROM Projects WHERE ManagerEmail = '{Username}' AND Status = 'Active'"
        }
        
        # Make request with suggestion in context
        response = client.post(
            "/project-data", 
            json=payload, 
            headers={"Authorization": "Bearer 123"}
        )
        
        # Verify response
        assert response.status_code == 200
        # The response should have been processed through the suggestion pipeline
        assert response.json().get("backend") == "project-data"

from unittest.mock import MagicMock, patch, AsyncMock
from llama_index.core.selectors.types import SingleSelection

def test_query_pipeline_with_complex_question(
        client, 
        azure_llm_model,
        jwt_decode_mock,
        mock_llama_query_pipeline,
        mock_azure_async_search_client):
    
    # Configure all mocks
    configure_azure_llm_mock_complex_data(azure_llm_model)
    configure_jwt_decode_mock(jwt_decode_mock)
    configure_query_pipeline_mock(mock_llama_query_pipeline)
    configure_complex_search_client_mock(mock_azure_async_search_client)
    
    # Apply additional patches using ExitStack
    with ExitStack() as stack:
        # Mock the LLMSingleSelector to avoid calculation_check issues
        mock_selector = MagicMock()
        mock_selection = SingleSelection(index=1, reason="This is a mocked selection")
        mock_selector.select.return_value = MagicMock(selections=[mock_selection])
        stack.enter_context(patch(
            "app.project_data.functions.calculation_check.LLMSingleSelector.from_defaults",
            return_value=mock_selector
        ))
        
        # Mock raw_chat_completion in split_question function
        stack.enter_context(patch(
            "app.project_data.functions.split_question.raw_chat_completion",
            return_value='{"sub_questions": ["How many tasks are assigned to Team A?", "How many tasks are assigned to Team B?"], "rewritten_question": "What is the task distribution across teams?"}'
        ))
        
        # Mock calculation_check to return False (no calculation needed)
        stack.enter_context(patch(
            "app.project_data.functions.calculation_check.calculation_check",
            return_value=False
        ))
        
        # Mock the pipeline execution results
        mock_pipeline_result = {
            "query_parser_output": {"output": "SELECT * FROM Tasks GROUP BY TeamId"},
            "query_response_parser": {"output": "Team A: 10 tasks, Team B: 15 tasks"}
        }
        stack.enter_context(patch(
            "app.project_data.services.splited_query_pipeline_standard.SplitedPipelineStandard.execute_pipelines",
            return_value=mock_pipeline_result
        ))
        
        # Mock asyncio.gather
        mock_gather = AsyncMock()
        mock_gather.return_value = [mock_pipeline_result, mock_pipeline_result]  # Return two results
        stack.enter_context(patch(
            "asyncio.gather", 
            side_effect=mock_gather
        ))
        
        # Mock merge_responses
        stack.enter_context(patch(
            "app.project_data.functions.merge_responses.merge_responses",
            return_value="Team A has 10 tasks and Team B has 15 tasks."
        ))
        
        # Mock response_post_processing
        mock_post_processing = {
            "citingSources": [
                {"sourceType": "page-key", "sourceValue": ["page1"]},
                {"sourceType": "table", "sourceValue": ["table1"]}
            ],
            "sql": {"query": "SELECT * FROM Tasks", "result": "[{\"Team\": \"A\", \"Tasks\": 10}, {\"Team\": \"B\", \"Tasks\": 15}]"}
        }
        stack.enter_context(patch(
            "app.project_data.services.response_post_processing.response_post_processing",
            return_value=mock_post_processing
        ))
        
        # Apply additional standard patches
        for patch_obj in get_additional_patches():
            stack.enter_context(patch_obj)
        
        # Use the complex query data
        payload = get_complex_query_data()
        
        # Make request with the complex question
        response = client.post(
            "/project-data", 
            json=payload, 
            headers={"Authorization": "Bearer 123"}
        )
        
        # Verify response
        assert response.status_code == 200
        response_data = response.json()
        assert response_data["backend"] == "project-data"
        assert response_data["response"] == "No match"
        assert "citingSources" in response_data
        assert "sql" in response_data