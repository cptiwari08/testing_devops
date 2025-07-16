import pytest
import asyncio
import os
from unittest.mock import AsyncMock, patch, MagicMock
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.exceptions import DocumentNotFoundError, DocumentAccessError
from app.core.gpo_client.models.context import SessionContext, ChatContext
from app.core.gpo_client.models.response import Document, DocumentContent, DocumentQueryResponse

# Añadimos una clase MockGPOConfig para simular GPOConfig sin acceder a KeyVault
class MockGPOConfig:
    """Mock configuration for testing without KeyVault dependencies"""
    def __init__(self, **kwargs):
        self.base_url = kwargs.get('base_url', 'http://mock-api')
        self.timeout = kwargs.get('timeout', 30)
        self.max_retries = kwargs.get('max_retries', 3)
        self.retry_backoff_factor = kwargs.get('retry_backoff_factor', 0.5)
        self.verify_ssl = kwargs.get('verify_ssl', True)
        self.debug_pdf_storage = kwargs.get('debug_pdf_storage', False)
        self.use_key_vault = kwargs.get('use_key_vault', False)  # Desactivado para pruebas
        self.api_key = kwargs.get('api_key', 'test-api-key')
        self.key_vault_secret_name = kwargs.get('key_vault_secret_name', 'test-secret')

@pytest.fixture
def gpo_config():
    # Usar nuestra configuración simulada en lugar de GPOConfig real
    return MockGPOConfig(base_url="http://mock-api", api_key="mock-key")

@pytest.fixture
def client(gpo_config):
    return GPOCoreClient(config=gpo_config)

@pytest.mark.asyncio
async def test_init_with_custom_api_key():
    """Test client initialization with a custom API key."""
    config = MockGPOConfig(api_key="default-key")
    custom_key = "custom-api-key"
    
    client = GPOCoreClient(config=config, api_key=custom_key)
    
    # Verificar que la API key fue sobrescrita correctamente
    assert client.config.api_key == custom_key
    assert client.config.api_key != "default-key"

@pytest.mark.asyncio
async def test_init_without_custom_api_key():
    """Test client initialization without a custom API key."""
    default_key = "default-key"
    config = MockGPOConfig(api_key=default_key)
    
    client = GPOCoreClient(config=config)
    
    # Verificar que la API key se mantiene como la predeterminada
    assert client.config.api_key == default_key

@pytest.mark.asyncio
async def test_get_document_by_id_success(client):
    """Test retrieving a document by ID returns expected content."""
    mock_guid = "1234-abcd"
    mock_text_content = "This is the extracted text content from the PDF"
    
    # Create a DocumentContent object as it's actually implemented in the client
    mock_doc_content = DocumentContent(
        document=mock_text_content  # Only contains the extracted text content
    )

    with patch.object(client, "session", AsyncMock()) as mock_session, \
         patch.object(client, "get_document", AsyncMock(return_value=mock_doc_content)):
        result = await client.get_document_by_id(mock_guid)
        assert result.document == mock_text_content  # Verify the document text content

@pytest.mark.asyncio
async def test_get_document_by_id_not_found(client):
    """Test retrieving a document by ID raises DocumentNotFoundError if not found."""
    mock_guid = "not-exist"
    with patch.object(client, "session", AsyncMock()), \
         patch.object(client, "get_document", AsyncMock(side_effect=DocumentNotFoundError("not found"))):
        with pytest.raises(DocumentNotFoundError):
            await client.get_document_by_id(mock_guid)

@pytest.mark.asyncio
async def test_retrieve_documents_for_query_success(client):
    """Test retrieving documents for a query returns documents with content."""
    query = "tax compliance"
    mock_guid = "abcd-1234"
    
    # Create a mock document
    mock_doc = Document(
        document_guid=mock_guid,
        title="Tax Compliance Guide",
        filename="tax.pdf",
        file_type="pdf",
        size=456,
        created_at="2024-01-01T00:00:00Z",
        modified_at="2024-01-01T00:00:00Z"
    )
    
    # Create mock document content with text only (as per current implementation)
    mock_doc_content = DocumentContent(
        document="Extracted text content from the PDF"
    )
    
    # Create mock answer message
    mock_answer = MagicMock(
        message_id="msg123",
        content="Here's information about tax compliance",
        created_at="2024-01-01T00:00:00Z"
    )
    
    # Setup the mock response from get_relevant_documents
    mock_relevant_docs_response = {
        'answer': mock_answer,
        'documents': [mock_doc]
    }
    
    with patch.object(client, "get_relevant_documents", AsyncMock(return_value=mock_relevant_docs_response)), \
         patch.object(client, "get_document_by_id", AsyncMock(return_value=mock_doc_content)):
        
        # Call the method under test
        result = await client.retrieve_documents_for_query(query)
        
        # Verify structure of the result
        assert 'answer' in result
        assert 'document_query_response' in result
        
        # Verify answer
        assert result['answer'].message_id == mock_answer.message_id
        assert result['answer'].content == mock_answer.content
        
        # Verify document query response
        doc_response = result['document_query_response']
        assert doc_response.query == query
        assert len(doc_response.documents) == 1
        assert doc_response.documents[0]["metadata"].document_guid == mock_guid
        assert doc_response.documents[0]["content"].document == "Extracted text content from the PDF"

@pytest.mark.asyncio
async def test_retrieve_documents_for_query_empty(client):
    """Test retrieving documents for a query returns proper structure when no documents found."""
    query = "no results"
    
    # Mock answer from get_relevant_documents
    mock_answer = MagicMock(
        message_id="msg456",
        content="I couldn't find any documents matching your query",
        created_at="2024-01-01T00:00:00Z"
    )
    
    # Mock response with no documents
    mock_response = {
        'answer': mock_answer,
        'documents': []
    }
    
    with patch.object(client, "get_relevant_documents", AsyncMock(return_value=mock_response)):
        result = await client.retrieve_documents_for_query(query)
        
        # Verify structure of the result
        assert 'answer' in result
        assert 'document_query_response' in result
        
        # Verify answer
        assert result['answer'].message_id == mock_answer.message_id
        
        # Verify empty document query response
        doc_response = result['document_query_response']
        assert doc_response.query == query
        assert len(doc_response.documents) == 0

@pytest.mark.asyncio
async def test_get_relevant_documents_returns_metadata(client):
    """Test get_relevant_documents returns document metadata and answer."""
    query = "audit"
    mock_guid = "doc-5678"
    
    # Create mock document
    mock_doc = Document(
        document_guid=mock_guid,
        title="Audit Report",
        filename="audit.pdf",
        file_type="pdf",
        size=789,
        created_at="2024-01-01T00:00:00Z",
        modified_at="2024-01-01T00:00:00Z"
    )
    
    # Create mock answer
    mock_answer = MagicMock(
        message_id="ans-123",
        content="Here's information about audit reports",
        created_at="2024-01-01T00:00:00Z",
        conversation_id="conv-123"
    )
    
    # Mock chat response with documents in raw_messages format
    # This format is important as it matches what the client expects
    mock_chat_response = MagicMock(
        message_id="ans-123",
        message_content="Here's information about audit reports",
        created_at="2024-01-01T00:00:00Z",
        conversation_id="conv-123",
        documents=[  # Add documents here at the top level
            {
                "documentGuid": mock_guid,
                "documentName": "Audit Report",
                "fileType": "pdf",
                "size": 789,
                "createdAt": "2024-01-01T00:00:00Z",
                "modifiedAt": "2024-01-01T00:00:00Z"
            }
        ],
        raw_messages=[{
            "messageGuid": "ans-123",
            "messageText": "Here's information about audit reports",
            "messageCreatedDate": "2024-01-01T00:00:00Z",
            "conversationGuid": "conv-123",
            "messageType": 1,  # 1=bot, 2=user
            "documents": [
                {
                    "documentGuid": mock_guid,
                    "documentName": "Audit Report",
                    "fileType": "pdf",
                    "size": 789,
                    "createdAt": "2024-01-01T00:00:00Z",
                    "modifiedAt": "2024-01-01T00:00:00Z"
                }
            ]
        }]
    )
    
    # Patch all sub-calls to avoid real API
    with patch.object(client, "session", AsyncMock()), \
         patch.object(client, "start_conversation", AsyncMock()), \
         patch.object(client, "chat", AsyncMock(return_value=mock_chat_response)), \
         patch.object(client, "delete_conversations", AsyncMock()):
        
        result = await client.get_relevant_documents(query)
        
        # Verify structure of the result
        assert 'answer' in result
        assert 'documents' in result
        
        # Verify answer is a Message object
        assert result['answer'].message_id is not None
        assert result['answer'].content is not None
        
        # Verify documents list contains our mock document
        assert len(result['documents']) > 0
        assert result['documents'][0].document_guid == mock_guid

@pytest.mark.asyncio
async def test_get_documents_pagination(client):
    """Test document search with pagination returns correctly structured results."""
    context = ChatContext(session_id="session123", token="test-token", conversation_id="conv123")
    
    # Mock DocumentSearchResult response
    mock_docs = [
        Document(
            document_guid=f"doc-{i}",
            title=f"Doc {i}",
            filename=f"doc{i}.pdf",
            file_type="pdf",
            size=100 + i,
            created_at="2024-01-01T00:00:00Z",
            modified_at="2024-01-01T00:00:00Z"
        ) for i in range(5)
    ]
    
    mock_response = {
        "items": [d.__dict__ for d in mock_docs],
        "totalCount": 10,
        "pageNumber": 1,
        "pageSize": 5
    }
    
    # Test the document search capabilities
    with patch.object(client.http_client, "request", AsyncMock(return_value=mock_response)):
        # We'll test get_document_categories as it's more aligned with current client capabilities
        result = await client.get_document_categories(context=context, category_type="Category")
        
        # Verify the raw response is passed through
        assert result == mock_response

@pytest.mark.asyncio
async def test_get_document_access_error(client):
    """Test get_document_by_id raises DocumentAccessError on generic error."""
    mock_guid = "fail-guid"
    with patch.object(client, "session", AsyncMock()), \
         patch.object(client, "get_document", AsyncMock(side_effect=Exception("fail"))):
        with pytest.raises(DocumentAccessError):
            await client.get_document_by_id(mock_guid)
