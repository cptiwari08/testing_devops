from unittest.mock import AsyncMock, MagicMock, Mock, patch
from uuid import uuid4


def test_user_prompt_and_memory(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent, full_agent_chat_response
):
    async_achat_mock = AsyncMock(return_value=full_agent_chat_response)
    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "How many normative operating models are in the app?",
        "chatHistory": [
            {"messageId": 1, "role": "system", "content": "Hi nice to meet you"},
            {"messageId": 2, "role": "user", "content": "Hello there"},
        ],
        "context": {
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.status_code == 200


def test_retry_error_exception_handler(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent
):
    from tenacity import RetryError

    async def async_achat_mock(*args, **kwargs):
        last_attend = Mock()
        raise RetryError(last_attend)

    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "Hi",
        "context": {
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.json()["detail"] == "RetryError"
    assert response.status_code == 503


def test_openai_api_error_exception_handler(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent
):
    import openai

    async def async_achat_mock(*args, **kwargs):
        raise openai.APIError(message="", request=Mock(), body=None)

    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "Hi",
        "context": {
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.json()["detail"] == "APIError"
    assert response.status_code == 400


def test_openai_connection_error_exception_handler(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent
):
    import openai

    async def async_achat_mock(*args, **kwargs):
        raise openai.APIConnectionError(request=Mock())

    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "Hi",
        "context": {
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.json()["detail"] == "APIConnectionError"
    assert response.status_code == 503


def test_openai_authentication_error_exception_handler(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent
):
    import openai

    async def async_achat_mock(*args, **kwargs):
        raise openai.AuthenticationError(response=Mock(), message="", body=Mock())

    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "Hi",
        "context": {
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.json()["detail"] == "AuthenticationError"
    assert response.status_code == 401


def test_openai_bad_request_error_exception_handler(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent
):
    import openai

    async def async_achat_mock(*args, **kwargs):
        raise openai.BadRequestError(response=Mock(), message="", body=Mock())

    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "Hi",
        "context": {
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.json()["detail"] == "InvalidRequestError"
    assert response.status_code == 400


def test_prompt_validation_error_handler(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent
):
    from app.core.errors import PromptValidationError

    async def async_achat_mock(*args, **kwargs):
        raise PromptValidationError()

    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "Hi",
        "context": {
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.json()["detail"] == "PromptValidationError"
    assert response.status_code == 500


def test_context_with_project_description(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent, full_agent_chat_response
):
    async_achat_mock = AsyncMock(return_value=full_agent_chat_response)
    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "How many normative operating models are in the app?",
        "chatHistory": [
            {"messageId": 1, "role": "system", "content": "Hi nice to meet you"},
            {"messageId": 2, "role": "user", "content": "Hello there"},
        ],
        "context": {
            "projectDescription": "This app have many normative operating models",
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.status_code == 200


def test_invalid_auth_token(client, azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client):
    from jose import JWTError

    mock_public_key = MagicMock()
    with patch("base64.b64decode", return_value=MagicMock()), patch(
        "cryptography.hazmat.primitives.serialization.load_der_public_key",
        return_value=mock_public_key,
    ), patch("jose.jwt.decode", side_effect=JWTError):
        payload = {
            "chatId": str(uuid4()),
            "instanceId": str(uuid4()),
            "question": "How many normative operating models are in the app?",
            "context": {
                "user": {
                    "email": "test@test.com",
                },
            },
        }

        response = client.post(
            "/ey-ip", json=payload, headers={"Authorization": "Bearer 1234"}
        )
        assert response.status_code == 401


def test_agent_request_with_suggested_question(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent, full_agent_chat_response
):
    async_achat_mock = AsyncMock(return_value=full_agent_chat_response)
    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "How many normative operating models are in the app?",
        "context": {
            "suggestion": {
                "id": "10001",
                "sqlQuery": "select * from table",
                "source": "project-data",
            },
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.status_code == 200


def test_agent_request_response_pd_flag(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent, full_agent_chat_response_pd_flag
):
    async_achat_mock = AsyncMock(return_value=full_agent_chat_response_pd_flag)
    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "How many normative operating models are in the app?",
        "context": {
            "suggestion": {
                "id": "10001",
                "sqlQuery": "select * from table",
                "source": "project-data",
            },
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.status_code == 200


def test_agent_request_response_source_node_error(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent, full_agent_chat_response_source_node_error
):
    async_achat_mock = AsyncMock(return_value=full_agent_chat_response_source_node_error)
    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "How many normative operating models are in the app?",
        "context": {
            "suggestion": {
                "id": "10001",
                "sqlQuery": "select * from table",
                "source": "project-data",
            },
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )
    assert response.status_code == 500


def test_agent_request_response_content(azure_llm_model, jwt_decode_mock, mock_llama_query_pipeline, mock_azure_async_search_client,
    client, monkeypatch, mock_llama_agent, full_agent_chat_response_no_content, mock_mssql_server_manager
):
    async_achat_mock = AsyncMock(return_value=full_agent_chat_response_no_content)
    monkeypatch.setattr(mock_llama_agent, "achat", async_achat_mock)
    
    payload = {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "How many normative operating models are in the app?",
        "context": {
            "suggestion": {
                "id": "10001",
                "sqlQuery": "select * from table",
                "source": "project-data",
            },
            "user": {
                "email": "test@test.com",
            },
        },
    }

    response = client.post(
        "/ey-ip", json=payload, headers={"Authorization": "Bearer 123"}
    )

    assert response.status_code == 200
