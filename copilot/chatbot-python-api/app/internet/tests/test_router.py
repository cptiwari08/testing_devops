from unittest.mock import AsyncMock, Mock, patch
from uuid import uuid4


def test_simple_greeting_without_memory(
    client, simple_greeting_response, jwt_decode_mock
):
    with patch("llama_index.core.chat_engine.SimpleChatEngine.from_defaults") as mock:
        mock.return_value.achat = AsyncMock(return_value=simple_greeting_response)
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
            "/internet", json=payload, headers={"Authorization": "Bearer 123"}
        )
        assert response.json()["response"] == simple_greeting_response.response
        assert response.status_code == 200


def test_simple_greeting_memory(client, simple_greeting_response, jwt_decode_mock):
    with patch("llama_index.core.chat_engine.SimpleChatEngine.from_defaults") as mock:
        mock.return_value.achat = AsyncMock(return_value=simple_greeting_response)
        payload = {
            "chatId": str(uuid4()),
            "instanceId": str(uuid4()),
            "question": "Hi",
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
            "/internet", json=payload, headers={"Authorization": "Bearer 123"}
        )
        assert response.status_code == 200


def test_context_with_project_description(
    client, simple_greeting_response, jwt_decode_mock
):
    with patch("llama_index.core.chat_engine.SimpleChatEngine.from_defaults") as mock:
        with patch(
            "llama_index.core.node_parser.SentenceSplitter.get_nodes_from_documents"
        ) as splitter_mock:
            splitter_mock.return_value = Mock()
            mock.return_value.achat = AsyncMock(return_value=simple_greeting_response)
            payload = {
                "chatId": str(uuid4()),
                "instanceId": str(uuid4()),
                "question": "Hi",
                "chatHistory": [
                    {
                        "messageId": 1,
                        "role": "system",
                        "content": "Hi nice to meet you",
                    },
                    {"messageId": 2, "role": "user", "content": "Hello there"},
                ],
                "context": {
                    "projectDescription": "This is a project about video games",
                    "user": {
                        "email": "test@test.com",
                    },
                },
            }

            response = client.post(
                "/internet", json=payload, headers={"Authorization": "Bearer 123"}
            )
            assert response.status_code == 200
