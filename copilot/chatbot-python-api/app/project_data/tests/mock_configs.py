from unittest.mock import AsyncMock, patch
from pydantic import BaseModel
from .mock_data import AsyncIterator, SAMPLE_SEARCH_RESULTS, COMPLEX_QUESTION_SEARCH_RESULTS

def configure_azure_llm_mock(mock):
    """Configura el mock de Azure LLM para test_query_pipeline"""
    mock.return_value.complete.return_value.text = "sugestion_1"
    return mock

def configure_azure_llm_mock_complex_data(mock):
    """Configura el mock de Azure LLM para test_query_pipeline"""
    mock.return_value.complete.return_value.text = "No match"
    return mock

def configure_jwt_decode_mock(jwt_decode_mock):
    """Configura el mock de JWT para test_query_pipeline"""
    mock_b64decode, mock_load_der, mock_jwt_decode = jwt_decode_mock
    mock_jwt_decode.return_value = {
        "expire": "000000",
        "docs_index_name": "index_name",
        "ai_search_instance_name": "mocked_instance_name",
        "metadata_index_name": "test"
    }
    return jwt_decode_mock

def configure_query_pipeline_mock(mock_llama_query_pipeline):
    """Configura el mock de QueryPipeline para test_query_pipeline"""
    def mocked_run(*args, **kwargs):
        class Message(BaseModel):
            content: str
        class MockedResponse(BaseModel):
            message: Message
        
        content = Message(content="project_data specific response")
        mocked_response = MockedResponse(message=content)
        return {
            "final_answer_llm": {"output": mocked_response},
            "query_parser_output": {"output": "SELECT * FROM table"},
            "response_output_parser": {"output": "Response output"},
            "context_parser_output": {"output": ""},
            "question_rewritting_parser_output": {"output": ""},
            "send_teamtype_output": {"output": ""},
            "citing_sources_output_parser": {"output": None},
        }
    
    mock_llama_query_pipeline.return_value.arun_multi = AsyncMock(side_effect=mocked_run)
    return mock_llama_query_pipeline

def configure_search_client_mock(mock_azure_async_search_client):
    """Configura el mock de SearchClient para test_query_pipeline"""
    mock_instance = mock_azure_async_search_client.return_value
    mock_instance.__aenter__ = AsyncMock(return_value=mock_instance)
    mock_instance.__aexit__ = AsyncMock(return_value=None)
    
    # Configurar la respuesta de búsqueda
    mock_instance.search = AsyncMock(return_value=AsyncIterator(SAMPLE_SEARCH_RESULTS))
    return mock_azure_async_search_client

def configure_complex_search_client_mock(mock_azure_async_search_client):
    """Configures the search client mock specifically for complex question test"""
    mock_instance = mock_azure_async_search_client.return_value
    mock_instance.__aenter__ = AsyncMock(return_value=mock_instance)
    mock_instance.__aexit__ = AsyncMock(return_value=None)
    
    # Configure with complex question search results that don't have suggestions
    mock_instance.search = AsyncMock(return_value=AsyncIterator(COMPLEX_QUESTION_SEARCH_RESULTS))
    return mock_azure_async_search_client

# Función auxiliar para aplicar todos los patches adicionales
def apply_additional_patches():
    """Devuelve un context manager con los patches adicionales"""
    return patch.multiple(
        target=None,  # No target needed for multiple
        sql_tables=patch(
            "sql_metadata.Parser.tables", 
            return_value=["MockedValue1", "MockedValue2"]
        ),
        program_office=patch(
            "app.project_data.services.program_office_api.ProgramOffice.run_query",
            return_value=[{"Title": "Value1"}, {"Title": "Value2"}]
        ),
        table_keys=patch(
            "app.project_data.functions.table_keys.get_widgets",
            return_value={"MockedValue2": ["key2, key3"]}
        )
    )


def get_additional_patches():
    """Devuelve los patches adicionales como una lista"""
    
    async def mock_calculation_check(*args, **kwargs):
        return False
    
    return [
        patch(
            "sql_metadata.Parser.tables", 
            return_value=["MockedValue1", "MockedValue2"]
        ),
        patch(
            "app.project_data.services.program_office_api.ProgramOffice.run_query",
            return_value=[{"Title": "Value1"}, {"Title": "Value2"}]
        ),
        patch(
            "app.project_data.functions.table_keys.get_widgets",
            return_value={"MockedValue2": ["key2, key3"]}
        ),
        patch("app.project_data.functions.split_question.raw_chat_completion", 
              return_value='{"sub_questions": ["question1", "question2"], "rewritten_question": "Rewritten complex question"}'),
        patch("app.project_data.functions.calculation_check.calculation_check", 
              side_effect=mock_calculation_check),
        patch("app.project_data.functions.merge_responses.merge_responses",
              return_value="Final merged answer for complex query")
    ]
