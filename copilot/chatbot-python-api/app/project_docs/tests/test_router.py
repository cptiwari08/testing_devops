from unittest.mock import AsyncMock, Mock, patch
from uuid import uuid4

from fastapi import HTTPException
from llama_index.core.chat_engine import ContextChatEngine, SimpleChatEngine


def test_project_docs_with_user_data_no_sources(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, project_docs_response_empty_source_nodes
):
    # First, directly patch the __init__ method of ServiceAgent to avoid SentenceTransformer initialization
    with patch("app.project_docs.services.project_docs_agentic.ServiceAgent.__init__", return_value=None) as mock_init:
        # Then patch the execute method to return our desired response
        with patch("app.project_docs.services.project_docs_agentic.ServiceAgent.execute") as mock_execute:
            # Set up the mock to return a response that simulates the 204 status
            mock_execute.return_value = {"status_code": 204}
            
            # Mock the LLMSingleSelector to avoid the comparison error
            with patch("llama_index.core.selectors.LLMSingleSelector.from_defaults") as mock_selector:
                # Create a mock for the selector result
                mock_selection = Mock()
                mock_selection.index = 0  # This will make it use ServiceAgent
                mock_selection_result = Mock()
                mock_selection_result.selections = [mock_selection]
                
                # Configure the selector mock to return our predefined selection result
                mock_selector.return_value.select = Mock(return_value=mock_selection_result)
            
                payload = {
                    "chatId": str(uuid4()),
                    "instanceId": str(uuid4()),
                    "projectFriendlyId": "project-123",
                    "question": "what is TPMO?",
                    "context": {
                        "user": {"email": "test@test.com"},
                        "documents": [
                            "8189cf55-ca78-4b40-9a45-694c501b91b8",
                        ],
                    },
                }

                response = client.post(
                    "/project-docs", json=payload, headers={"Authorization": "Bearer 123"}
                )

                # Assert response code
                assert response.status_code == 204
                
                # Verify ServiceAgent's execute was called with correct parameters
                mock_execute.assert_called_once()
                # Check the args from the call
                args, kwargs = mock_execute.call_args
                assert "message_request" in kwargs
                assert "authorization" in kwargs
                assert kwargs["authorization"] == "Bearer 123"
                assert "llm_models" in kwargs


def test_project_docs_with_none_documents_list(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, project_docs_response_empty_source_nodes
):
    # Mock the LLMSingleSelector to make the code path use Service instead of ServiceAgent
    with patch("llama_index.core.selectors.LLMSingleSelector.from_defaults") as mock_selector:
        # Create a mock for the selector result that will use Service (not ServiceAgent)
        mock_selection = Mock()
        mock_selection.index = 1  # This will make it use Service, not ServiceAgent
        mock_selection_result = Mock()
        mock_selection_result.selections = [mock_selection]
        
        # Configure the selector mock to return our predefined selection result
        mock_selector.return_value.select = Mock(return_value=mock_selection_result)

        payload = {
            "chatId": str(uuid4()),
            "instanceId": str(uuid4()),
            "projectFriendlyId": "project-123",
            "question": "what is TPMO?",
            "context": {
                "user": {"email": "test@test.com"},
                "documents": None,
            },
        }

        response = client.post(
            "/project-docs", json=payload, headers={"Authorization": "Bearer 123"}
        )

        assert response.status_code == 422


def test_project_docs_with_empty_documents_list(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, project_docs_response_empty_source_nodes
):
    # Mock the LLMSingleSelector to make the code path use Service instead of ServiceAgent
    with patch("llama_index.core.selectors.LLMSingleSelector.from_defaults") as mock_selector:
        # Create a mock for the selector result that will use Service (not ServiceAgent)
        mock_selection = Mock()
        mock_selection.index = 1  # This will make it use Service, not ServiceAgent
        mock_selection_result = Mock()
        mock_selection_result.selections = [mock_selection]
        
        # Configure the selector mock to return our predefined selection result
        mock_selector.return_value.select = Mock(return_value=mock_selection_result)
        
        with patch("app.project_docs.services.project_docs.Service.execute") as mock_execute:
            # Set up the mock to return a response with 422 status
            mock_execute.side_effect = HTTPException(
                status_code=422, detail="No documents IDs in request payload"
            )

            payload = {
                "chatId": str(uuid4()),
                "instanceId": str(uuid4()),
                "projectFriendlyId": "project-123",
                "question": "what is TPMO?",
                "context": {
                    "user": {"email": "test@test.com"},
                    "documents": [],
                },
            }

            response = client.post(
                "/project-docs", json=payload, headers={"Authorization": "Bearer 123"}
            )
            
            assert response.status_code == 422
            mock_execute.assert_called_once()


def test_project_docs_with_user_data(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
        client, project_docs_response):
    # First, directly patch the __init__ method of ServiceAgent to avoid SentenceTransformer initialization
    with patch("app.project_docs.services.project_docs_agentic.ServiceAgent.__init__", return_value=None) as mock_init:
        # Then patch the execute method to return our desired successful response
        with patch("app.project_docs.services.project_docs_agentic.ServiceAgent.execute") as mock_execute:
            # Set up the mock to return a response that simulates a successful response
            mock_execute.return_value = {
                "status_code": 200,
                "response": "This is a test response",
                "citingSources": [{"sourceName": "test-source", "sourceType": "documents", "sourceValue": []}],
                "rawResponse": "Raw response",
                "score": 1,
                "followUpSuggestions": ["Question 1?", "Question 2?"],
                "chainOfThoughts": "DEBUG - Logger initialized with console level: DEBUG, INFO - I received a project data request asking, [question], INFO - Initializing token counter and LLM models, INFO - [instanceId:aaaaa] [chatId:aaa] [projectFriendlyId:None] Now, I am executing the Project Data service., INFO - [instanceId:aaaaa] [chatId:aaa] [projectFriendlyId:None] I am currently processing the project data request: '[question]', INFO - [instanceId:aaaaa] [chatId:aaa] [projectFriendlyId:None] Answer generated"
            }
            
            # Mock the LLMSingleSelector to avoid the comparison error
            with patch("llama_index.core.selectors.LLMSingleSelector.from_defaults") as mock_selector:
                # Create a mock for the selector result
                mock_selection = Mock()
                mock_selection.index = 0  # This will make it use ServiceAgent
                mock_selection_result = Mock()
                mock_selection_result.selections = [mock_selection]
                
                # Configure the selector mock to return our predefined selection result
                mock_selector.return_value.select = Mock(return_value=mock_selection_result)
            
                payload = {
                    "chatId": str(uuid4()),
                    "instanceId": str(uuid4()),
                    "projectFriendlyId": "project-123",
                    "question": "what is TPMO?",
                    "context": {
                        "user": {"email": "test@test.com"},
                        "documents": [
                            "8189cf55-ca78-4b40-9a45-694c501b91b8",
                        ],
                    },
                }

                response = client.post(
                    "/project-docs", json=payload, headers={"Authorization": "Bearer 123"}
                )

                # Assert response code
                assert response.status_code == 200
                assert response.json().get("response") == "This is a test response"
                
                # Verify ServiceAgent's execute was called with correct parameters
                mock_execute.assert_called_once()