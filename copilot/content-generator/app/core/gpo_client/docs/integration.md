# Integration Guide

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This integration guide provides a comprehensive approach to incorporating the GPO Core API Client into your Capital Edge backend services. By following these patterns and best practices, you can create robust, maintainable integrations that leverage the full capabilities of the GPO Core API.

## Table of Contents

- [Overview](#overview)
- [Integration Architecture](#integration-architecture)
- [Basic Integration Patterns](#basic-integration-patterns)
- [Document Management Integration](#document-management-integration)
- [Chat Flow Integration](#chat-flow-integration)
- [Advanced Integration Techniques](#advanced-integration-techniques)
- [Performance Considerations](#performance-considerations)
- [Security Considerations](#security-considerations)
- [Testing Your Integration](#testing-your-integration)
- [Troubleshooting](#troubleshooting)

## Overview

The GPO Core API Client provides a Pythonic interface to the GPO Core API, which offers a range of capabilities including:

- Session management
- Conversation handling
- Chat processing
- Document retrieval and management
- User feedback and ratings
- Analytics and metrics

This guide focuses on how to integrate this client into your existing or new Python services within the Capital Edge ecosystem.

## Integration Architecture

When integrating with the GPO Core API, your application typically follows this high-level architecture:

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│                 │       │                 │       │                 │
│  Your Service   │━━━━━━▶│ GPO Core Client │━━━━━━▶│   GPO Core API  │
│                 │       │                 │       │                 │
└─────────────────┘       └─────────────────┘       └─────────────────┘
       ▲                                                    │
       │                                                    │
       └────────────────────────────────────────────────────┘
                           Response Flow
```

### Integration Layers

A well-designed integration typically consists of these layers:

1. **Service Layer**: Your business logic that needs GPO Core API capabilities
2. **Client Adapter Layer**: A thin wrapper around the GPO Core Client to adapt it to your needs
3. **GPO Core Client**: The library that handles communication with the API
4. **Configuration Layer**: Manages environment-specific settings for the client

## Basic Integration Patterns

### Direct Integration

The simplest approach is to directly instantiate and use the client:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatRequest

async def handle_user_query(user_id, project_id, query):
    # Create client
    client = GPOCoreClient()
    
    # Create session
    session_context = SessionContext(
        instance_id="capital-edge-instance",
        user_id=user_id,
        project_id=project_id
    )
    session_response = await client.session(session_context)
    
    # Start conversation
    chat_context = ChatContext(
        session_id=session_response.session_id,
        token=session_response.token
    )
    conversation = await client.start_conversation(
        chat_context, 
        ConversationRequest(title="User Query")
    )
    
    # Send query and get response
    chat_request = ChatRequest(
        query=query,
        use_openai=True
    )
    response = await client.chat(
        ChatContext(
            session_id=session_response.session_id,
            conversation_id=conversation.conversation_id,
            token=session_response.token
        ),
        chat_request
    )
    
    return response.message_content
```

### Dependency Injection

For more maintainable code, use dependency injection:

```python
from fastapi import Depends, FastAPI
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.config import GPOConfig

app = FastAPI()

# Client factory
def get_gpo_client():
    config = GPOConfig.from_env()
    return GPOCoreClient(config=config)

# FastAPI dependency
@app.post("/api/ask")
async def ask_question(
    question: str,
    client: GPOCoreClient = Depends(get_gpo_client)
):
    # Use client to process question
    # ...
    return {"answer": response}
```

### Repository Pattern

For better testability, use the repository pattern:

```python
class GPORepository:
    def __init__(self, client: GPOCoreClient):
        self.client = client
        
    async def get_answer(self, user_id, project_id, query):
        # Implementation using client
        # ...
        return response

# In your service:
repository = GPORepository(GPOCoreClient())
answer = await repository.get_answer(user_id, project_id, query)
```

## Document Management Integration

One of the key features of GPO Core API is document management. Here are the specialized methods for document retrieval:

### Get Documents Based on Query

```python
async def get_relevant_documents(query: str) -> List[Document]:
    """
    Retrieves a list of relevant documents based on a natural language query.
    
    Args:
        query: The natural language query to search for relevant documents
        
    Returns:
        A list of Document objects containing document metadata
    """
    client = GPOCoreClient()
    
    # Create session first
    session_context = SessionContext(
        instance_id="capital-edge-instance",
        user_id="document-service",
        project_id="retrieval-system"
    )
    session_response = await client.session(session_context)
    
    # Search for documents using the Messages endpoint
    search_result = await client.search_messages(
        chat_context=ChatContext(
            session_id=session_response.session_id,
            token=session_response.token
        ),
        query=query
    )
    
    # Extract and return document references
    return search_result.documents
```

### Get Document by ID

```python
async def get_document_by_id(document_guid: str) -> DocumentContent:
    """
    Retrieves a specific document by its GUID.
    
    Args:
        document_guid: The unique identifier of the document
        
    Returns:
        DocumentContent object containing the document content
    """
    client = GPOCoreClient()
    
    # Create session first
    session_context = SessionContext(
        instance_id="capital-edge-instance",
        user_id="document-service",
        project_id="retrieval-system"
    )
    session_response = await client.session(session_context)
    
    # Get document details using the Documents endpoint
    document_content = await client.get_document(
        chat_context=ChatContext(
            session_id=session_response.session_id,
            token=session_response.token
        ),
        document_guid=document_guid
    )
    
    return document_content
```

### Unified Document Retrieval Method

```python
async def retrieve_documents_for_query(query: str) -> DocumentQueryResponse:
    """
    Comprehensive method that finds relevant documents for a query and 
    retrieves their full content.
    
    Args:
        query: The natural language query to search for documents
        
    Returns:
        DocumentQueryResponse with relevant documents and their content
    """
    client = GPOCoreClient()
    
    # Create session
    session_context = SessionContext(
        instance_id="capital-edge-instance",
        user_id="document-service",
        project_id="retrieval-system"
    )
    session_response = await client.session(session_context)
    
    # Get chat context
    chat_context = ChatContext(
        session_id=session_response.session_id,
        token=session_response.token
    )
    
    # 1. Search for relevant documents
    document_refs = await client.search_relevant_documents(
        chat_context=chat_context,
        query=query
    )
    
    # 2. Retrieve full content for each document
    documents_with_content = []
    for doc_ref in document_refs:
        try:
            content = await client.get_document(
                chat_context=chat_context,
                document_guid=doc_ref.document_guid
            )
            documents_with_content.append({
                "metadata": doc_ref,
                "content": content
            })
        except Exception as e:
            logger.error(f"Failed to retrieve document {doc_ref.document_guid}: {e}")
    
    return DocumentQueryResponse(
        query=query,
        documents=documents_with_content
    )
```

## Chat Flow Integration

For chat applications, you'll want to implement a flow that maintains conversation context:

```python
class ChatService:
    def __init__(self):
        self.client = GPOCoreClient()
        self.active_sessions = {}  # In production, use a proper cache or database
        
    async def start_user_session(self, user_id, project_id):
        # Create and store session
        session_context = SessionContext(
            instance_id="capital-edge-instance",
            user_id=user_id,
            project_id=project_id
        )
        session_response = await self.client.session(session_context)
        
        # Store session info
        self.active_sessions[user_id] = {
            "session_id": session_response.session_id,
            "token": session_response.token,
            "conversations": {}
        }
        
        return session_response.session_id
    
    async def start_conversation(self, user_id, title):
        # Get existing session
        user_session = self.active_sessions.get(user_id)
        if not user_session:
            raise ValueError("User session not found")
        
        # Create conversation
        chat_context = ChatContext(
            session_id=user_session["session_id"],
            token=user_session["token"]
        )
        
        conversation = await self.client.start_conversation(
            chat_context,
            ConversationRequest(title=title)
        )
        
        # Store conversation
        user_session["conversations"][conversation.conversation_id] = {
            "id": conversation.conversation_id,
            "title": title,
            "messages": []
        }
        
        return conversation.conversation_id
    
    async def send_message(self, user_id, conversation_id, message):
        # Get existing session and conversation
        user_session = self.active_sessions.get(user_id)
        if not user_session:
            raise ValueError("User session not found")
            
        conversation = user_session["conversations"].get(conversation_id)
        if not conversation:
            raise ValueError("Conversation not found")
        
        # Send message
        chat_context = ChatContext(
            session_id=user_session["session_id"],
            conversation_id=conversation_id,
            token=user_session["token"]
        )
        
        chat_request = ChatRequest(
            query=message,
            use_openai=True
        )
        
        response = await self.client.chat(chat_context, chat_request)
        
        # Store message history
        conversation["messages"].append({
            "user_message": message,
            "system_response": response.message_content,
            "timestamp": datetime.now().isoformat()
        })
        
        return response
```

## Advanced Integration Techniques

### Streaming Response Integration

For real-time response streaming:

```python
async def stream_response(websocket, user_id, conversation_id, query):
    client = GPOCoreClient()
    session = get_user_session(user_id)  # Your function to get user session
    
    chat_context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation_id,
        token=session.token
    )
    
    request = StreamingChatRequest(
        query=query,
        use_openai=True,
        stream=True
    )
    
    # Stream chunks to the websocket
    async for chunk in client.chat_stream(chat_context, request):
        await websocket.send_json({
            "type": "chunk",
            "content": chunk.content,
            "is_complete": chunk.is_complete
        })
        
        if chunk.is_complete:
            # Send any final metadata
            await websocket.send_json({
                "type": "metadata",
                "sources": [s.dict() for s in chunk.sources] if chunk.sources else []
            })
```

### Retry and Circuit Breaker Pattern

For robust integration with automatic retry and circuit breaking:

```python
from tenacity import retry, stop_after_attempt, wait_exponential
from circuitbreaker import circuit

class RobustGPOClient:
    def __init__(self):
        self.client = GPOCoreClient()
    
    @retry(stop=stop_after_attempt(3), wait=wait_exponential(multiplier=1, min=2, max=10))
    @circuit(failure_threshold=5, recovery_timeout=30)
    async def get_answer(self, session_context, chat_context, query):
        try:
            # Get session
            session_response = await self.client.session(session_context)
            
            # Update chat context with session info
            chat_context.session_id = session_response.session_id
            chat_context.token = session_response.token
            
            # Start conversation if needed
            if not chat_context.conversation_id:
                conv = await self.client.start_conversation(
                    chat_context,
                    ConversationRequest(title="Auto Conversation")
                )
                chat_context.conversation_id = conv.conversation_id
            
            # Get chat response
            response = await self.client.chat(
                chat_context,
                ChatRequest(query=query, use_openai=True)
            )
            
            return response
            
        except Exception as e:
            logger.error(f"GPO API error: {str(e)}")
            raise
```

## Error Handling

When integrating the GPO Core API Client, it's important to handle errors appropriately. The client provides specific exception classes that help identify and handle different types of errors:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest
from app.core.gpo_client.exceptions import (
    GPOClientException,  # Base exception class
    AuthenticationError, # Authentication issues
    ConnectionError,     # Network connection problems
    TimeoutError,        # Request timeouts
    InvalidRequestError, # Invalid parameters
    RateLimitError,      # Too many requests
    ServiceUnavailableError, # API service unavailable
    DocumentNotFoundError,    # Document not found
    DocumentAccessError       # Document access denied
)

async def robust_document_retrieval(query, document_id=None):
    client = GPOCoreClient()
    
    try:
        # Standard authentication flow
        session_context = SessionContext(
            instance_id="capital-edge-instance-001",
            user_id="document-service",
            project_id="retrieval-system"
        )
        session_response = await client.session(session_context)
        
        chat_context = ChatContext(
            session_id=session_response.session_id,
            token=session_response.token
        )
        
        # Attempt document retrieval
        if document_id:
            # Get specific document
            try:
                document = await client.get_document(
                    chat_context=chat_context,
                    document_guid=document_id
                )
                return {"status": "success", "document": document}
            except DocumentNotFoundError:
                # Handle document not found
                return {"status": "error", "error": "document_not_found", "message": "The requested document does not exist"}
            except DocumentAccessError:
                # Handle access denied
                return {"status": "error", "error": "access_denied", "message": "Access to this document is restricted"}
        else:
            # Search for documents
            try:
                documents = await client.search_relevant_documents(
                    chat_context=chat_context,
                    query=query
                )
                return {"status": "success", "documents": documents}
            except Exception as e:
                return {"status": "error", "error": "search_failed", "message": str(e)}
                
    except AuthenticationError as e:
        # Handle authentication issues
        logger.error(f"Authentication failed: {e}")
        return {"status": "error", "error": "authentication", "message": "Authentication failed"}
    
    except ConnectionError as e:
        # Handle connection issues
        logger.error(f"Connection error: {e}")
        return {"status": "error", "error": "connection", "message": "Could not connect to GPO API"}
        
    except TimeoutError as e:
        # Handle timeouts
        logger.error(f"Request timed out: {e}")
        return {"status": "error", "error": "timeout", "message": "Request timed out"}
        
    except RateLimitError as e:
        # Handle rate limiting
        logger.error(f"Rate limit exceeded: {e}")
        return {"status": "error", "error": "rate_limit", "message": "Too many requests, please try again later"}
        
    except ServiceUnavailableError as e:
        # Handle service unavailability
        logger.error(f"Service unavailable: {e}")
        return {"status": "error", "error": "service_unavailable", "message": "Service temporarily unavailable"}
        
    except InvalidRequestError as e:
        # Handle invalid requests
        logger.error(f"Invalid request: {e}")
        return {"status": "error", "error": "invalid_request", "message": str(e)}
        
    except GPOClientException as e:
        # Handle any other client exceptions
        logger.error(f"GPO client error: {e}")
        return {"status": "error", "error": "client_error", "message": str(e)}
        
    except Exception as e:
        # Handle unexpected exceptions
        logger.critical(f"Unexpected error: {e}", exc_info=True)
        return {"status": "error", "error": "unexpected", "message": "An unexpected error occurred"}
```

## Performance Considerations

To optimize performance in your integration:

1. **Connection Pooling**: The client uses connection pooling internally. Reuse client instances when possible.

2. **Caching**: Implement a caching layer for session tokens and frequently requested documents:

```python
from functools import lru_cache

@lru_cache(maxsize=100)
async def get_document_cached(document_guid):
    client = GPOCoreClient()
    # ... authentication code ...
    return await client.get_document(chat_context, document_guid)
```

3. **Asynchronous Processing**: Use asynchronous processing for batch operations:

```python
async def process_document_batch(queries):
    tasks = []
    for query in queries:
        task = retrieve_documents_for_query(query)
        tasks.append(task)
    
    return await asyncio.gather(*tasks)
```

4. **Request Batching**: When performing multiple related operations, batch them when possible:

```python
# Instead of multiple separate document requests
documents = await client.get_documents_batch(
    chat_context,
    document_guids=[guid1, guid2, guid3]
)
```

## Security Considerations

When integrating with the GPO Core API:

1. **Token Handling**: Store session tokens securely and never expose them to clients
2. **Sensitive Data**: Use the sensitive data endpoints for handling PII or confidential information
3. **Audit Logging**: Implement audit logging for all API interactions:

```python
async def log_api_interaction(operation, user_id, success, error=None):
    await audit_logger.log({
        "operation": operation,
        "user_id": user_id,
        "timestamp": datetime.now().isoformat(),
        "success": success,
        "error": str(error) if error else None
    })
```

## Testing Your Integration

### Unit Testing with Mocks

```python
from unittest.mock import AsyncMock, patch

@patch("your_module.GPOCoreClient")
async def test_document_retrieval(mock_client):
    # Setup mock returns
    mock_client.return_value.session = AsyncMock()
    mock_client.return_value.session.return_value = MockSessionResponse(
        session_id="test_session_123",
        token="mock_token"
    )
    
    mock_client.return_value.search_documents = AsyncMock()
    mock_client.return_value.search_documents.return_value = [
        MockDocument(document_guid="doc1", title="Test Doc")
    ]
    
    # Call your function
    result = await get_relevant_documents("test query")
    
    # Assert results
    assert len(result) == 1
    assert result[0].document_guid == "doc1"
    assert mock_client.return_value.search_documents.called
```

### Integration Testing

For actual API integration testing:

```python
@pytest.mark.integration
async def test_live_document_retrieval():
    # Use test credentials
    os.environ["GPO_API_URL"] = "https://gpo-api-test.example.com" 
    
    # Test with actual API
    result = await get_relevant_documents("tax implications merger")
    
    # Verify response format
    assert isinstance(result, list)
    if len(result) > 0:
        assert hasattr(result[0], "document_guid")
        assert hasattr(result[0], "title")
```

## Troubleshooting

For troubleshooting GPO Core API integration issues, see the [Troubleshooting](./troubleshooting.md) guide, which covers:

- Authentication errors
- Rate limiting issues
- Network connectivity problems
- Common error patterns and solutions

## Conclusion

This integration guide provides a comprehensive approach to incorporating the GPO Core API Client into your Capital Edge backend services. By following these patterns and best practices, you can create robust, maintainable integrations that leverage the full capabilities of the GPO Core API.

For specific implementation questions or advanced use cases, refer to the [API Reference](./api_reference.md) or contact the Capital Edge development team.