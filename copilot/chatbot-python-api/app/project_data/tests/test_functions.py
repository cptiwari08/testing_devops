from unittest.mock import AsyncMock, patch, Mock

import pytest


@pytest.mark.asyncio
async def test_denormalized_query_generator():
    import httpx
    from app.project_data.functions.denormalized_query_generator import (
        denormalized_query_generator,
    )

    class MockResponse:
        def __init__(self, status_code, json_data):
            self.status_code = status_code
            self._json_data = json_data

        def json(self):
            return self._json_data

        def raise_for_status(self):
            if self.status_code != 200:
                raise httpx.HTTPStatusError("Error", request=None, response=None)

    mock_response = MockResponse(status_code=200, json_data=[])

    async def mock_post(*args, **kwargs):
        return mock_response

    with patch("httpx.AsyncClient.post", new=AsyncMock(side_effect=mock_post)):
        token_details = {
            "unique_name": "fake value",
            "email": "fake value",
            "family_name": "fake value",
            "given_name": "fake value",
            "user_type": "fake value",
            "oid": "fake value",
            "ce_oid": "fake value",
            "upn": "fake value",
            "sp_url": "https://fake.com",
            "po_app_url": "https://fake.com",
            "po_api_url": "https://fake.com",
            "copilot_app_url": "https://fake.com",
            "copilot_api_url": "https://fake.com",
            "project_id": "fake value",
            "project_friendly_id": "fake value",
            "scope": ["ey-guidance", "internet", "project-docs", "project-data"],
            "nbf": 1713448733,
            "exp": 1713452333,
            "iat": 1713448733,
            "iss": "fake value",
            "aud": "fake value",
        }
        from typing import Callable

        from app.core.pydantic_models import (
            QueryPipelineContext,
            MessageRequest,
            LLMModels,
        )
        from pydantic import BaseModel

        class Logger(BaseModel):
            info: Callable = lambda message: message

        context_data = {
                    "chatId": "",
                    "instanceId": "",
                    "question": "How many open status risks the HR team have?",
                    "context": {
                        "user": {
                            "email": "{{user_email}}"
                        }
                    }
                }
        llm_models = LLMModels(llm=Mock(), embed_model=Mock())
        class Context(QueryPipelineContext):
            token: str = " "
            logger: Logger = Logger()
            message_request: MessageRequest = MessageRequest(**context_data)

        context = Context(llm_models=llm_models)
        with patch("jose.jwt.decode", return_value=token_details):
            with patch("httpx.URL", new_callable=lambda: "https://fake.com"):
                query = await denormalized_query_generator(
                    tables_list=["Workplan"], context=context
                )
                assert query == "SELECT  FROM Workplan W "


def test_question_rewritting_parser():
    from typing import Callable
    from app.core.pydantic_models import LLMModels

    from app.project_data.functions.question_rewritting_parser import (
        question_rewritting_parser,
    )
    from pydantic import BaseModel

    class Logger(BaseModel):
        info: Callable = lambda message: message

    class Context(BaseModel):
        token: str = "fake bearer"
        logger: Logger = Logger()

    class MessageContent(BaseModel):
        content: str

    class QuestionOutput(BaseModel):
        message: MessageContent

    llm_models = LLMModels(llm=Mock(), embed_model=Mock())
    question_output = QuestionOutput(message=MessageContent(content="content"))
    result = question_rewritting_parser(question_output, Context(llm_models=llm_models))
    assert result == question_output.message.content


def test_context_parser_with_multiple_nodes():
    from typing import Callable

    from pydantic import BaseModel

    class Logger(BaseModel):
        info: Callable = lambda message: message

    from app.core.pydantic_models import QueryPipelineContext, MessageRequest, LLMModels

    context_data = {
                    "chatId": "",
                    "instanceId": "",
                    "question": "How many open status risks the HR team have?",
                    "context": {
                        "user": {
                            "email": "{{user_email}}"
                        }
                    }
                }

    llm_models = LLMModels(llm=Mock(), embed_model=Mock())
    class Context(QueryPipelineContext):
        token: str = "fake bearer"
        logger: Logger = Logger()
        message_request: MessageRequest = MessageRequest(**context_data)

    class NodeWithScore(BaseModel):
        text: str
        score: float = 1.0

    from app.project_data.functions.context_parser import context_parser

    # Arrange
    nodes = ([NodeWithScore(text="Hello"), NodeWithScore(text="World")],"")
    expected_output = "Hello\n\nWorld"+"""
Tables schemas: 
"""

    # Act
    result = context_parser(nodes, Context(llm_models=llm_models))

    # Assert
    assert (
        result == expected_output
    ), "Should correctly parse multiple nodes and join texts with double newlines"


def test_context_parser_with_single_node():
    from typing import Callable

    from pydantic import BaseModel

    class Logger(BaseModel):
        info: Callable = lambda message: message

    from app.core.pydantic_models import QueryPipelineContext, MessageRequest, LLMModels

    context_data = {
                    "chatId": "",
                    "instanceId": "",
                    "question": "How many open status risks the HR team have?",
                    "context": {
                        "user": {
                            "email": "{{user_email}}"
                        }
                    }
                }

    llm_models = LLMModels(llm=Mock(), embed_model=Mock())
    class Context(QueryPipelineContext):
        token: str = "fake bearer"
        logger: Logger = Logger()
        message_request: MessageRequest = MessageRequest(**context_data)

    class NodeWithScore(BaseModel):
        text: str
        score: float = 1.0

    from app.project_data.functions.context_parser import context_parser

    # Arrange
    nodes = ([NodeWithScore(text="Single Node Text")],"")
    expected_output = "Single Node Text"+"""
Tables schemas: 
"""

    # Act
    result = context_parser(nodes, Context(llm_models=llm_models))

    # Assert
    assert result == expected_output, "Should handle a single node correctly"


def test_context_parser_with_empty_list():
    from typing import Callable

    from pydantic import BaseModel

    class Logger(BaseModel):
        info: Callable = lambda message: message

    from app.core.pydantic_models import QueryPipelineContext, MessageRequest, LLMModels

    context_data = {
                    "chatId": "",
                    "instanceId": "",
                    "question": "How many open status risks the HR team have?",
                    "context": {
                        "user": {
                            "email": "{{user_email}}"
                        }
                    }
                }

    llm_models = LLMModels(llm=Mock(), embed_model=Mock())
    class Context(QueryPipelineContext):
        token: str = "fake bearer"
        logger: Logger = Logger()
        message_request: MessageRequest = MessageRequest(**context_data)

    from app.project_data.functions.context_parser import context_parser

    # Arrange
    nodes = ([],"")
    expected_output = ""+"""
Tables schemas: 
"""

    # Act
    result = context_parser(nodes, Context(llm_models=llm_models))

    # Assert
    assert (
        result == expected_output
    ), "Should return an empty string for an empty list of nodes"


@pytest.mark.asyncio
async def test_title_table_name_with_response():
    tables = ["table1", "table2"]
    token = "token"

    expected_output = [{"Title": "value1"}, {"Title": "value2"}]

    # Mock the run_query method
    with patch(
        "app.project_data.services.program_office_api.ProgramOffice.run_query",
        return_value=expected_output,
    ):
        from app.project_data.functions.table_title_name import title_table_name

        result = await title_table_name(tables, token)
        assert result == expected_output


@pytest.mark.asyncio
async def test_title_table_name_with_no_response():
    tables = ["table1", "table2"]
    token = "token"

    # Mock the run_query method
    with patch(
        "app.project_data.services.program_office_api.ProgramOffice.run_query",
        return_value=[],
    ):
        from app.project_data.functions.table_title_name import title_table_name

        result = await title_table_name(tables, token)
        assert result == []


@pytest.mark.asyncio
async def test_get_table_keys():
    widget_output = {"Mocked1": ["key1", "key2"], "Mocked2": ["key3", "key4"]}
    token = "token"

    with patch(
        "app.project_data.functions.table_keys.get_widgets",
        return_value=widget_output,
    ):
        from app.project_data.functions.table_keys import get_table_keys

        result = await get_table_keys(token, ["Mocked2"])
        assert result == []


def test_context_retriever_raise_exception():
    query_str = "sample query"
    tables_list = ["table1", "table2"]
    token = "Bearer your_token"
    with (
        patch("app.core.utils.extract_claim"),
        patch(
            "jose.jwt.decode",
            return_value={"gloss_index_name": "index", "ai_search_instance_name": None},
        ),
    ):
        from app.project_data.functions.context_retriever import context_retriever
        from fastapi import HTTPException
        from starlette import status

        try:
            context_retriever(query_str, tables_list, token)
        except HTTPException as exc:
            assert exc.status_code == status.HTTP_422_UNPROCESSABLE_ENTITY
            assert exc.detail == "Invalid AI Search instance name"


def test_table_retriever_raise_exception():
    query_str = "sample query"
    token = "Bearer your_token"
    with (
        patch("app.core.utils.extract_claim"),
        patch(
            "jose.jwt.decode",
            return_value={"data_index_name": "index", "ai_search_instance_name": None},
        ),
    ):
        from app.project_data.functions.table_retriever import table_retriever
        from fastapi import HTTPException
        from starlette import status

        try:
            table_retriever(query_str, token)
        except HTTPException as exc:
            assert exc.status_code == status.HTTP_422_UNPROCESSABLE_ENTITY
            assert exc.detail == "Invalid AI Search instance name"


def test_few_shots_raise_exception():
    query_str = "sample query"
    token = "Bearer your_token"
    with (
        patch("app.core.utils.extract_claim"),
        patch(
            "jose.jwt.decode",
            return_value={
                "suggestions_index_name": "index",
                "ai_search_instance_name": None,
            },
        ),
    ):
        from app.project_data.functions.few_shots_retriever import few_shots_retriever
        from fastapi import HTTPException
        from starlette import status

        try:
            few_shots_retriever(query_str, token)
        except HTTPException as exc:
            assert exc.status_code == status.HTTP_422_UNPROCESSABLE_ENTITY
            assert exc.detail == "Invalid AI Search instance name"
