import pytest
from llama_index.core.chat_engine.types import AgentChatResponse
from pydantic import BaseModel
from llama_index.core.tools import QueryEngineTool, ToolOutput
from sqlalchemy.engine import Engine
from unittest.mock import Mock, patch, AsyncMock, MagicMock
from fastapi.testclient import TestClient


# Patch the Logger globally before any test runs
@pytest.fixture(scope="session", autouse=True)
def mock_logger():
    with patch("app.core.logger.Logger") as mock:
        mock.return_value.set_unique_identifiers = Mock()
        mock.return_value.error = Mock()
        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_public_key_deserialization():
    with patch("cryptography.hazmat.primitives.serialization") as mock:
        mock.return_value.load_der_public_key = Mock()
        yield mock


# Patch the MS SQL server globally
@pytest.fixture(scope="session", autouse=True)
def mock_ms_server():
    with patch("app.core.ms_server_manager.MSSQLServerManager") as mock:
        mock_engine = MagicMock(spec=Engine)
        mock.return_value.get_engine = MagicMock(return_value=mock_engine)

        mock_connection = MagicMock()

        mock_engine.connect.return_value.__enter__.return_value = mock_connection

        # Mock the execute method to return a list of tuples
        mock_connection.execute.return_value = [
            (0, 0),
            (1, 1),
        ]

        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_mssql_server_manager():
    with patch("app.core.ms_server_manager.MSSQLServerManager") as mock:
        # Configure the mock inside the context manager
        instance = mock.return_value
        instance.execute_text_query = Mock(
            return_value=[
                ("column1", "value1"),
                ("column2", "value2"),
            ]
        )
        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_azure_key_credencials():
    with patch("azure.core.credentials.AzureKeyCredential") as mock:
        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_key_vault_secret_client():
    with patch("azure.keyvault.secrets.SecretClient") as mock:

        class Data(BaseModel):
            value: str = '{"endpoint": "fake_secret", "key": "fake_key"}'

        data = Data()
        mock.return_value.get_secret.return_value = data
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_deepeval_geval_metric():
    with patch("deepeval.metrics.GEval") as mock:
        geval_instance = Mock()
        geval_instance.a_measure = AsyncMock()
        geval_instance.score = 0.9
        geval_instance.reason = "Mock reason"
        mock.return_value = geval_instance
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_async_key_vault_secret_client():
    with patch("azure.keyvault.secrets.aio.SecretClient.get_secret", new_callable=AsyncMock) as mock:

        class Data(BaseModel):
            value: str = '{"endpoint": "fake_secret", "key": "fake_key"}'

        data = Data()
        mock.return_value = data
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_azure_identity_default_credentials():
    with patch("azure.identity.DefaultAzureCredential") as mock:
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_azure_async_identity_default_credentials():
    with patch("azure.identity.aio.DefaultAzureCredential") as mock:
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_azure_async_search_client():
    with patch("azure.search.documents.aio.SearchClient") as mock:
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_azure_search_client():
    with patch("azure.search.documents.SearchClient") as mock:
        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_azure_ai_search_vector_store():
    with patch(
        "llama_index.vector_stores.azureaisearch.AzureAISearchVectorStore"
    ) as mock:
        yield mock


# Llama-index mock


@pytest.fixture(scope="session", autouse=True)
def mock_llama_agent():
    with patch(
        "llama_index.agent.openai_legacy.ContextRetrieverOpenAIAgent.from_tools_and_retriever"
    ) as mock:
        # Create a mock object that can have both sync and async methods
        mock_agent_instance = Mock()
        mock_agent_instance.reset = Mock()
        mock_agent_instance.achat = AsyncMock(return_value="default async response")

        mock.return_value = mock_agent_instance
        yield mock_agent_instance


@pytest.fixture(scope="session", autouse=True)
def mock_chat_engine():
    with patch("llama_index.core.chat_engine.SimpleChatEngine") as mock:
        mock_engine_instance = Mock()
        mock_engine_instance.achat = AsyncMock(return_value="default async response")

        mock.return_value = mock_engine_instance
        yield mock_engine_instance


@pytest.fixture(scope="session", autouse=True)
def mock_llama_vector_store_index():
    with patch("llama_index.core.VectorStoreIndex") as mock_vector_store_index:
        # Set up the `as_retriever` method on the `VectorStoreIndex` mock
        mock_as_retriever = mock_vector_store_index.return_value.as_retriever
        mock_as_retriever.return_value = Mock()

        # Set up `free_req_inputs_keys` on the object returned by `as_retriever()`
        mock_as_retriever.return_value.free_req_inputs_keys = Mock()
        # You can customize the return value as needed
        mock_as_retriever.return_value.free_req_inputs_keys.return_value = {
            "key": "value"
        }

        yield mock_vector_store_index

@pytest.fixture(scope="session", autouse=True)
def mock_vector_index_retriever():
    with patch("llama_index.core.retrievers.VectorIndexRetriever") as mock:
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_retriever_query_engine():
    with patch("llama_index.core.query_engine.RetrieverQueryEngine") as mock:
        mock.return_value.retrieve = Mock(return_value=[])
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_sentence_splitter():
    with patch("llama_index.core.node_parser.SentenceSplitter") as mock:
        mock.return_value.get_nodes_from_documents = Mock()
        yield mock

@pytest.fixture(scope="session", autouse=True)
def mock_llama_query_pipeline():
    with patch("llama_index.core.query_pipeline.QueryPipeline") as mock:

        def mocked_run(*args, **kwargs):
            class Message(BaseModel):
                content: str

            class MockedResponse(BaseModel):
                message: Message

            content = Message(content="default response")
            mocked_response = MockedResponse(message=content)
            return {
                "final_answer_llm": {"output": mocked_response},
                "query_parser_output": {
                    "output": "",
                },
                "response_output_parser": {
                    "output": "",
                },
                "context_parser_output": {
                    "output": "",
                },
                "question_rewritting_parser_output": {
                    "output": "",
                },
                "send_teamtype_output": {
                    "output": "",
                },
            }

        mock.return_value.arun_multi = AsyncMock(side_effect=mocked_run)
        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_llama_sqldatabase():
    with patch("llama_index.core.SQLDatabase") as mock:
        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_nl_sql_table_query_engine():
    with patch("llama_index.core.query_engine.NLSQLTableQueryEngine") as mock:
        yield mock


@pytest.fixture(scope="session", autouse=True)
def mock_query_engine_tool():
    with patch("llama_index.core.tools.QueryEngineTool") as mock:
        mock.return_value.from_defaults = Mock(spec=QueryEngineTool)
        yield mock


@pytest.fixture(scope="session", autouse=True)
def azure_llm_model():
    with patch("llama_index.llms.azure_openai.AzureOpenAI") as mock:
        yield mock


@pytest.fixture(scope="session", autouse=True)
def azure_async_llm_model():
    with patch("llama_index.llms.azure_openai.AsyncAzureOpenAI") as mock:
        yield mock


@pytest.fixture(scope="session", autouse=True)
def tiktoken_encoding():
    with patch("tiktoken.encoding_for_model") as mock:
        yield mock



@pytest.fixture(scope="session", autouse=True)
def azure_embeddings_model():
    with patch("llama_index.embeddings.azure_openai.AzureOpenAIEmbedding") as mock:
        yield mock


@pytest.fixture(scope="session")
def jwt_decode_mock():
    mock_public_key = MagicMock()
    with patch("base64.b64decode", return_value=MagicMock()) as mock_b64decode, patch(
        "cryptography.hazmat.primitives.serialization.load_der_public_key",
        return_value=mock_public_key,
    ) as mock_load_der, patch(
        "jose.jwt.decode",
        return_value={
            "expire": "000000",
            "docs_index_name": "index_name",
            "ai_search_instance_name": "mocked_instance_name",
        },
    ) as mock_jwt_decode:
        yield mock_b64decode, mock_load_der, mock_jwt_decode


@pytest.fixture(scope="module")
def client():
    # Import the app after the Logger has been patched
    # this is necessary to avoid the initialization
    # in middlewares
    from app.main import app

    with TestClient(app) as client:
        yield client


# response models
# Define nested Pydantic models to match the expected structure
class Node(BaseModel):
    id_: str
    metadata: dict
    text: str


class SourceNode(BaseModel):
    node: Node
    score: float | None
    metadata: dict = {"mocked_metadata": "mocked_value"}
    text: str = "mocked chunk text"


class RawOutput(BaseModel):
    response: str
    source_nodes: list[SourceNode]
    metadata: dict


class Source(BaseModel):
    content: str
    tool_name: str
    raw_input: dict
    raw_output: RawOutput


class MockAgentChatResponse(BaseModel):
    response: str
    sources: list[Source]
    source_nodes: list[SourceNode] = []


@pytest.fixture
def full_agent_chat_response():
    data = {
        "response": "There are 3822 normative operating models in the app.",
        "sources": [
            {
                "content": "There are 3822 nodes in the database.",
                "tool_name": "om_tool",
                "raw_input": {"input": "SELECT COUNT(*) FROM nodes"},
                "raw_output": {
                    "response": "There are 3822 nodes in the database.",
                    "source_nodes": [
                        {
                            "node": {
                                "id_": "265fb738-9650-4766-b886-09dbe0a38e3a",
                                "embedding": None,
                                "metadata": {},
                                "excluded_embed_metadata_keys": [],
                                "excluded_llm_metadata_keys": [],
                                "relationships": {},
                                "text": "[(3822,)]",
                                "start_char_idx": None,
                                "end_char_idx": None,
                                "text_template": "{metadata_str}\n\n{content}",
                                "metadata_template": "{key}: {value}",
                                "metadata_seperator": "\n",
                            },
                            "score": None,
                        }
                    ],
                    "metadata": {
                        "265fb738-9650-4766-b886-09dbe0a38e3a": {},
                        "sql_query": "SELECT COUNT(*) FROM Nodes",
                        "result": [[3822]],
                        "col_keys": [""],
                    },
                },
            }
        ],
        "source_nodes": [
            {
                "node": {
                    "id_": "265fb738-9650-4766-b886-09dbe0a38e3a",
                    "embedding": None,
                    "metadata": {},
                    "excluded_embed_metadata_keys": [],
                    "excluded_llm_metadata_keys": [],
                    "relationships": {},
                    "text": "[(3822,)]",
                    "start_char_idx": None,
                    "end_char_idx": None,
                    "text_template": "{metadata_str}\n\n{content}",
                    "metadata_template": "{key}: {value}",
                    "metadata_seperator": "\n",
                },
                "score": None,
            }
        ],
    }
    return MockAgentChatResponse(**data)


@pytest.fixture
def full_agent_chat_response_pd_flag():
    data = {
        "response": "#PD",
        "sources": [],
        "source_nodes": [],
    }
    return MockAgentChatResponse(**data)


@pytest.fixture
def full_agent_chat_response_source_node_error():
    data = {
        "response": "",
        "sources": [
            {
                "content": "",
                "tool_name": "",
                "raw_input": {},
                "raw_output": {
                    "response": "",
                    "source_nodes": [
                        {
                            "node": {
                                "id_": "",
                                "metadata": {},
                                "text": "error"
                            },
                            "score": None,
                        }
                    ],
                    "metadata": {},
                },
            }
        ],
        "source_nodes": [],
    }
    return MockAgentChatResponse(**data)


@pytest.fixture
def full_agent_chat_response_no_content():
    data = {
        "response": "There are 3822 normative operating models in the app.",
        "sources": [
            {
                "content": "There are 3822 nodes in the database.",
                "tool_name": "fallback_function",
                "raw_input": {"input": "SELECT COUNT(*) FROM nodes"},
                "raw_output": {
                    "response": "There are 3822 nodes in the database.",
                    "source_nodes": [
                        {
                            "node": {
                                "id_": "265fb738-9650-4766-b886-09dbe0a38e3a",
                                "embedding": None,
                                "metadata": {},
                                "excluded_embed_metadata_keys": [],
                                "excluded_llm_metadata_keys": [],
                                "relationships": {},
                                "text": "[(3822,)]",
                                "start_char_idx": None,
                                "end_char_idx": None,
                                "text_template": "{metadata_str}\n\n{content}",
                                "metadata_template": "{key}: {value}",
                                "metadata_seperator": "\n",
                            },
                            "score": None,
                        }
                    ],
                    "metadata": {
                        "265fb738-9650-4766-b886-09dbe0a38e3a": {},
                        "sql_query": "SELECT COUNT(*) FROM Nodes",
                        "result": [[3822]],
                        "col_keys": [""],
                    },
                },
            }
        ],
        "source_nodes": [
            {
                "node": {
                    "id_": "265fb738-9650-4766-b886-09dbe0a38e3a",
                    "embedding": None,
                    "metadata": {},
                    "excluded_embed_metadata_keys": [],
                    "excluded_llm_metadata_keys": [],
                    "relationships": {},
                    "text": "[(3822,)]",
                    "start_char_idx": None,
                    "end_char_idx": None,
                    "text_template": "{metadata_str}\n\n{content}",
                    "metadata_template": "{key}: {value}",
                    "metadata_seperator": "\n",
                },
                "score": None,
            }
        ],
    }
    return MockAgentChatResponse(**data)


@pytest.fixture
def simple_greeting_response():
    data = {
        "response": "Hello! How can I assist you today?",
        "sources": [],
        "source_nodes": [],
    }
    return MockAgentChatResponse(**data)


@pytest.fixture
def project_docs_response_empty_source_nodes():
    return AgentChatResponse(
        response="TPMO stands for Tax PMO, as mentioned in the context information.",
        sources=[
            ToolOutput(
                content="TPMO stands for Tax PMO, as mentioned in the context information.",
                tool_name="tool_name",
                raw_input={},
                raw_output=RawOutput(
                    response="TPMO stands for Tax PMO, as mentioned in the context information.",
                    source_nodes=[],
                    metadata={},
                ),
            )
        ],
        source_nodes=[],
        followUpSuggestions=[],
    )


@pytest.fixture
def project_docs_response():
    data = {
        "response": "There are 3822 normative operating models in the app.",
        "sources": [
            {
                "content": "There are 3822 nodes in the database.",
                "tool_name": "om_tool",
                "raw_input": {"input": "SELECT COUNT(*) FROM nodes"},
                "raw_output": {
                    "response": "There are 3822 nodes in the database.",
                    "source_nodes": [
                        {
                            "node": {
                                "id_": "265fb738-9650-4766-b886-09dbe0a38e3a",
                                "embedding": None,
                                "metadata": {},
                                "excluded_embed_metadata_keys": [],
                                "excluded_llm_metadata_keys": [],
                                "relationships": {},
                                "text": "[(3822,)]",
                                "start_char_idx": None,
                                "end_char_idx": None,
                                "text_template": "{metadata_str}\n\n{content}",
                                "metadata_template": "{key}: {value}",
                                "metadata_seperator": "\n",
                            },
                            "score": None,
                            "metadata": {},  # Add metadata field here
                        }
                    ],
                    "metadata": {
                        "265fb738-9650-4766-b886-09dbe0a38e3a": {},
                        "sql_query": "SELECT COUNT(*) FROM Nodes",
                        "result": [[3822]],
                        "col_keys": [""],
                    },
                },
            }
        ],
        "source_nodes": [
            {
                "node": {
                    "id_": "265fb738-9650-4766-b886-09dbe0a38e3a",
                    "embedding": None,
                    "metadata": {},
                    "excluded_embed_metadata_keys": [],
                    "excluded_llm_metadata_keys": [],
                    "relationships": {},
                    "text": "[(3822,)]",
                    "start_char_idx": None,
                    "end_char_idx": None,
                    "text_template": "{metadata_str}\n\n{content}",
                    "metadata_template": "{key}: {value}",
                    "metadata_seperator": "\n",
                },
                "score": None,
                "metadata": {"chunk_id": "00", "chunk_text": "fake text"},
            }
        ],
        "followUpSuggestions": [
            {
                "id": 0,
                "suggestionText": "What impact has Capital Edge had on EY's engagements?",
            },
            {
                "id": 0,
                "suggestionText": "How does Capital Edge contribute to effective program management?",
            },
            {
                "id": 0,
                "suggestionText": "Could you describe the flexibility offered by Capital Edge?",
            },
        ],
    }
    return MockAgentChatResponse(**data)
