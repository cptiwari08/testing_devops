# Usage Examples

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document provides comprehensive examples of how to use the GPO Core Python Client in various scenarios. These examples cover common use cases and demonstrate best practices for working with the client.

## Table of Contents
- [Basic Usage](#basic-usage)
- [Session Management](#session-management)
- [Conversation Handling](#conversation-handling)
- [Chat Interactions](#chat-interactions)
- [Sensitive Data Handling](#sensitive-data-handling)
- [Streaming Responses](#streaming-responses)
- [Error Handling](#error-handling)
- [Advanced Usage](#advanced-usage)

## Basic Usage

### Initializing the Client

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient

# Initialize with defaults (environment variables)
client = GPOCoreClient()

# Initialize with explicit configuration
from app.core.gpo_client.config import GPOConfig
config = GPOConfig(
    base_url="https://gpo-api.example.com",
    timeout=30,
    max_retries=3
)
client = GPOCoreClient(config=config)

# Initialize from a configuration file
client = GPOCoreClient.from_config_file("config/production.yaml")
```

### Complete Workflow Example

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

async def main():
    # Initialize client
    client = GPOCoreClient()
    
    # 1. Create a session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    
    session = await client.session(session_context)
    print(f"Session created: {session.session_id}")
    
    # 2. Start a conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Tax consultation", "metadata": {"source": "example"}}
    )
    print(f"Conversation started: {conversation.conversation_id}")
    
    # 3. Send a chat message
    chat_context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation.conversation_id,
        token=session.token
    )
    
    request = ChatRequest(
        query="What are the tax implications of a merger between companies in Mexico?",
        use_openai=True
    )
    
    response = await client.chat(context=chat_context, request=request)
    
    # 4. Process the response
    print(f"Response: {response.message_content}")
    
    for source in response.sources:
        print(f"Source: {source.title} (Relevance: {source.relevance_score})")

if __name__ == "__main__":
    asyncio.run(main())
```

## Session Management

### Creating a New Session

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext

async def create_session_example():
    client = GPOCoreClient()
    
    context = SessionContext(
        instance_id="capital-edge-instance-001", 
        user_id="user-123",
        project_id="project-456",
        environment="production"  # Optional, defaults to "production"
    )
    
    session = await client.session(context)
    
    print(f"Session ID: {session.session_id}")
    print(f"Token: {session.token}")
    print(f"Expires at: {session.expires_at}")
    print(f"User context: {session.user_context}")
    
    return session
```

### Working with Session Tokens

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext
from datetime import datetime
import time
from jose import jwt

async def session_token_management():
    client = GPOCoreClient()
    
    # Create a session
    context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    
    session = await client.session(context)
    token = session.token
    
    # Decode token to check claims (without validation)
    claims = jwt.get_unverified_claims(token)
    
    # Check expiration
    if "exp" in claims:
        expiration = datetime.fromtimestamp(claims["exp"])
        now = datetime.now()
        
        time_left = expiration - now
        print(f"Token expires in: {time_left}")
        
        # Check if token is about to expire
        if time_left.total_seconds() < 300:  # Less than 5 minutes
            print("Token is about to expire, renewing...")
            session = await client.session(context)  # Create new session
            token = session.token
    
    return session
```

## Conversation Handling

### Starting a New Conversation

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import ChatContext, ConversationRequest

async def start_conversation_example(session):
    client = GPOCoreClient()
    
    context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    
    # Create request with title and metadata
    request = ConversationRequest(
        title="Corporate Tax Inquiry",
        metadata={
            "industry": "Technology",
            "topic": "Corporate Tax",
            "region": "LATAM"
        }
    )
    
    # Start conversation
    conversation = await client.start_conversation(
        context=context,
        request=request
    )
    
    print(f"Conversation ID: {conversation.conversation_id}")
    print(f"Title: {conversation.title}")
    print(f"Created at: {conversation.created_at}")
    print(f"Status: {conversation.status}")
    
    return conversation
```

### Managing Multiple Conversations

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ConversationRequest

async def manage_multiple_conversations():
    client = GPOCoreClient()
    
    # Create a session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    session = await client.session(session_context)
    
    # Start multiple conversations
    conversations = []
    topics = ["Corporate Tax", "Transfer Pricing", "VAT Compliance"]
    
    for topic in topics:
        context = ChatContext(
            session_id=session.session_id,
            token=session.token
        )
        
        request = ConversationRequest(
            title=f"{topic} Inquiry",
            metadata={"topic": topic}
        )
        
        conversation = await client.start_conversation(context=context, request=request)
        conversations.append(conversation)
        
        print(f"Started conversation: {conversation.conversation_id} - {conversation.title}")
    
    # Now you can interact with any of these conversations
    for conversation in conversations:
        chat_context = ChatContext(
            session_id=session.session_id,
            conversation_id=conversation.conversation_id,
            token=session.token
        )
        
        # Send a query relevant to this conversation topic
        # Implementation continues...
    
    return conversations
```

## Chat Interactions

### Basic Chat Query

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import ChatContext, ChatRequest

async def basic_chat_example(session, conversation_id):
    client = GPOCoreClient()
    
    context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation_id,
        token=session.token
    )
    
    request = ChatRequest(
        query="What are the tax implications of a merger between companies in Mexico?",
        use_openai=True,
        client_metadata={
            "client_id": "client-789",
            "application": "tax-advisor"
        }
    )
    
    response = await client.chat(context=context, request=request)
    
    print(f"Message ID: {response.message_id}")
    print(f"Content: {response.message_content}")
    print(f"Response time: {response.response_time_ms}ms")
    print(f"Model used: {response.model_used}")
    
    # Process sources
    print("\nSources:")
    for source in response.sources:
        print(f"- {source.title} (Relevance: {source.relevance_score})")
        if hasattr(source, "url") and source.url:
            print(f"  URL: {source.url}")
    
    return response
```

### Multi-Turn Conversation

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

async def multi_turn_conversation():
    client = GPOCoreClient()
    
    # Create session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    session = await client.session(session_context)
    
    # Start conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Multi-turn conversation"}
    )
    conversation_id = conversation.conversation_id
    
    # Set up chat context for conversation
    context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation_id,
        token=session.token
    )
    
    # First question
    response1 = await client.chat(
        context=context,
        request=ChatRequest(
            query="What are the tax implications of a merger between companies in Mexico?",
            use_openai=True
        )
    )
    print(f"Response 1: {response1.message_content[:100]}...")
    
    # Follow-up question (in same conversation)
    response2 = await client.chat(
        context=context,
        request=ChatRequest(
            query="What documentation is required for this process?",
            use_openai=True
        )
    )
    print(f"Response 2: {response2.message_content[:100]}...")
    
    # Another follow-up
    response3 = await client.chat(
        context=context,
        request=ChatRequest(
            query="Are there any tax incentives available for technology companies in this scenario?",
            use_openai=True
        )
    )
    print(f"Response 3: {response3.message_content[:100]}...")
    
    return [response1, response2, response3]
```

## Sensitive Data Handling

### Processing Sensitive Information

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import (
    SessionContext, 
    ChatContext, 
    SensitiveChatRequest,
    ChatHistory
)

async def sensitive_data_example():
    client = GPOCoreClient()
    
    # Create session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    session = await client.session(session_context)
    
    # Start conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Sensitive data consultation"}
    )
    
    # Create chat context
    context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation.conversation_id,
        token=session.token
    )
    
    # Previous chat history for context
    chat_history = [
        ChatHistory(
            role="user",
            content="I need advice on tax structure for our subsidiary."
        ),
        ChatHistory(
            role="assistant",
            content="I'd be happy to help with your tax structure. What specific aspects are you interested in?"
        )
    ]
    
    # Sensitive data request
    request = SensitiveChatRequest(
        query="Our company reported $4.5M in revenue last year with a profit margin of 12%. How can we optimize our tax structure in Mexico?",
        is_sensitive=True,
        chat_history=chat_history,
        sensitivity_markers=[
            {"type": "financial_data", "start": 15, "end": 20},
            {"type": "percentage", "start": 57, "end": 60}
        ],
        data_handling_policy="secure_processing"
    )
    
    response = await client.chat_sensitive_data(context=context, request=request)
    
    print(f"Response: {response.message_content}")
    
    # Check if there are compliance warnings
    if hasattr(response, "compliance_warnings") and response.compliance_warnings:
        print("\nCompliance Warnings:")
        for warning in response.compliance_warnings:
            print(f"- {warning.code}: {warning.message} (Severity: {warning.severity})")
    
    return response
```

## Streaming Responses

### Processing a Response Stream

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import (
    SessionContext, 
    ChatContext, 
    StreamingChatRequest
)

async def streaming_response_example():
    client = GPOCoreClient()
    
    # Create session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    session = await client.session(session_context)
    
    # Start conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Streaming response example"}
    )
    
    # Set up context for streaming
    context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation.conversation_id,
        token=session.token
    )
    
    # Create streaming request
    request = StreamingChatRequest(
        query="Explain the tax benefits of setting up a holding company in Mexico",
        use_openai=True,
        stream=True
    )
    
    print("Streaming response:\n")
    
    # Process stream
    full_response = ""
    sources = []
    
    async for chunk in client.chat_stream(context=context, request=request):
        # Print chunk content if available
        if chunk.content:
            print(chunk.content, end="", flush=True)
            full_response += chunk.content
        
        # Collect sources (usually arrive at the end)
        if chunk.sources:
            sources.extend(chunk.sources)
        
        # Check if this is the final chunk
        if chunk.is_complete:
            print("\n\nStream complete!")
            
            # Process collected sources
            if sources:
                print("\nSources:")
                for source in sources:
                    print(f"- {source.title} (Relevance: {source.relevance_score})")
    
    return {"content": full_response, "sources": sources}
```

## Error Handling

### Handling Common Errors

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest
from app.core.gpo_client.exceptions import (
    GPOCoreClientException,
    AuthenticationError,
    InvalidRequestError,
    ConnectionError,
    TimeoutError,
    RateLimitError,
    ServiceUnavailableError
)
import logging

logger = logging.getLogger(__name__)

async def error_handling_example():
    client = GPOCoreClient()
    
    try:
        # Create session
        session_context = SessionContext(
            instance_id="capital-edge-instance-001",
            user_id="user-123",
            project_id="project-456"
        )
        
        session = await client.session(session_context)
        
        # Start conversation
        chat_context = ChatContext(
            session_id=session.session_id,
            token=session.token
        )
        
        conversation = await client.start_conversation(
            context=chat_context,
            request={"title": "Error handling example"}
        )
        
        # Chat request
        context = ChatContext(
            session_id=session.session_id,
            conversation_id=conversation.conversation_id,
            token=session.token
        )
        
        request = ChatRequest(
            query="What are the tax implications of a merger?",
            use_openai=True
        )
        
        response = await client.chat(context=context, request=request)
        return response
        
    except AuthenticationError as e:
        logger.error(f"Authentication error: {e}")
        # Handle authentication errors (e.g., refresh credentials)
        return {"error": "authentication", "message": str(e)}
        
    except InvalidRequestError as e:
        logger.error(f"Invalid request: {e} (Field: {e.field})")
        # Handle validation errors (e.g., fix parameters)
        return {"error": "invalid_request", "message": str(e), "field": e.field}
        
    except ConnectionError as e:
        logger.error(f"Connection error: {e} (Attempts: {e.attempts})")
        # Handle connection issues (e.g., retry later)
        return {"error": "connection", "message": str(e)}
        
    except TimeoutError as e:
        logger.error(f"Timeout error: {e}")
        # Handle timeouts (e.g., simplify request or increase timeout)
        return {"error": "timeout", "message": str(e)}
        
    except RateLimitError as e:
        retry_after = getattr(e, "retry_after", 60)
        logger.warning(f"Rate limit exceeded. Retry after {retry_after} seconds.")
        # Handle rate limiting (e.g., implement backoff)
        return {"error": "rate_limit", "message": str(e), "retry_after": retry_after}
        
    except ServiceUnavailableError as e:
        logger.error(f"Service unavailable: {e}")
        # Handle service unavailability (e.g., use fallback)
        return {"error": "service_unavailable", "message": str(e)}
        
    except GPOCoreClientException as e:
        # Handle any other client exceptions
        logger.error(f"GPO client error: {e}")
        return {"error": "client_error", "message": str(e)}
        
    except Exception as e:
        # Handle unexpected errors
        logger.critical(f"Unexpected error: {e}", exc_info=True)
        return {"error": "unexpected", "message": "An unexpected error occurred"}
```

### Using Retry Decorators

```python
import asyncio
import backoff
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import ChatContext, ChatRequest
from app.core.gpo_client.exceptions import ConnectionError, TimeoutError, ServiceUnavailableError

# Define which errors should trigger a retry
retryable_errors = (ConnectionError, TimeoutError, ServiceUnavailableError)

# Create a retry-enabled function using backoff
@backoff.on_exception(
    backoff.expo,
    retryable_errors,
    max_tries=3,
    factor=2,
    jitter=backoff.full_jitter
)
async def chat_with_retry(client, context, request):
    """Send a chat request with automatic retries for certain errors."""
    return await client.chat(context=context, request=request)

async def retry_example(session, conversation_id):
    client = GPOCoreClient()
    
    context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation_id,
        token=session.token
    )
    
    request = ChatRequest(
        query="What are the tax implications of a merger?",
        use_openai=True
    )
    
    # This will automatically retry on connection errors, timeouts, etc.
    try:
        response = await chat_with_retry(client, context, request)
        print(f"Response received: {response.message_content[:50]}...")
        return response
    except retryable_errors as e:
        print(f"Failed after multiple retries: {e}")
        # Handle final failure
        return None
```

## Advanced Usage

### Parallel Processing

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

async def parallel_processing_example():
    client = GPOCoreClient()
    
    # Create session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    session = await client.session(session_context)
    
    # Start conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Parallel queries"}
    )
    
    # Prepare multiple queries
    queries = [
        "What are the tax implications of a merger in Mexico?",
        "How does transfer pricing work for multinational corporations?",
        "Explain VAT requirements for technology services in LATAM."
    ]
    
    # Process queries in parallel
    async def process_query(query):
        context = ChatContext(
            session_id=session.session_id,
            conversation_id=conversation.conversation_id,
            token=session.token
        )
        
        request = ChatRequest(query=query, use_openai=True)
        response = await client.chat(context=context, request=request)
        
        return {
            "query": query,
            "response": response.message_content,
            "sources": [s.title for s in response.sources] if response.sources else []
        }
    
    # Execute all queries in parallel
    tasks = [process_query(query) for query in queries]
    results = await asyncio.gather(*tasks)
    
    # Print results
    for i, result in enumerate(results):
        print(f"\nQuery {i+1}: {result['query']}")
        print(f"Response: {result['response'][:100]}...")
        print(f"Sources: {', '.join(result['sources'][:3])}")
    
    return results
```

### Custom Service Implementation

```python
import logging
from typing import Dict, Any, Optional, List
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import (
    SessionContext, 
    ChatContext, 
    ChatRequest, 
    ConversationRequest
)
from app.core.gpo_client.exceptions import GPOCoreClientException

logger = logging.getLogger(__name__)

class TaxAdvisorService:
    """
    A service that uses GPO Core to provide tax-related advice.
    """
    
    def __init__(self, config=None):
        """Initialize the service with optional configuration."""
        self.client = GPOCoreClient(config=config)
    
    async def get_tax_advice(
        self,
        query: str,
        user_id: str,
        instance_id: str,
        project_id: str,
        region: Optional[str] = None,
        industry: Optional[str] = None
    ) -> Dict[str, Any]:
        """
        Get tax advice for a specific query.
        
        Args:
            query: The user's tax-related question
            user_id: User identifier
            instance_id: Instance identifier
            project_id: Project identifier
            region: Optional geographic region for context
            industry: Optional industry for context
            
        Returns:
            Dictionary with response content and metadata
        """
        try:
            # 1. Create session
            session = await self._create_session(
                user_id=user_id,
                instance_id=instance_id,
                project_id=project_id
            )
            
            # 2. Start a conversation
            conversation = await self._start_conversation(
                session=session,
                title=f"Tax advice: {self._extract_title(query)}",
                metadata={
                    "region": region,
                    "industry": industry,
                    "query_type": "tax_advice"
                }
            )
            
            # 3. Format the query with additional context
            enhanced_query = self._enhance_query(query, region, industry)
            
            # 4. Send the query and get a response
            response = await self._get_response(
                session=session,
                conversation_id=conversation.conversation_id,
                query=enhanced_query
            )
            
            # 5. Process the response
            return {
                "success": True,
                "advice": response.message_content,
                "conversation_id": conversation.conversation_id,
                "sources": [
                    {"title": s.title, "relevance": s.relevance_score}
                    for s in response.sources
                ] if response.sources else [],
                "metadata": {
                    "region": region,
                    "industry": industry,
                    "model_used": response.model_used
                }
            }
            
        except GPOCoreClientException as e:
            logger.error(f"Error getting tax advice: {e}")
            return {
                "success": False,
                "error": str(e),
                "error_code": getattr(e, "code", "unknown")
            }
    
    async def _create_session(self, user_id: str, instance_id: str, project_id: str):
        """Create a session with the GPO Core API."""
        session_context = SessionContext(
            instance_id=instance_id,
            user_id=user_id,
            project_id=project_id
        )
        return await self.client.session(session_context)
    
    async def _start_conversation(self, session, title: str, metadata: Dict[str, Any]):
        """Start a new conversation."""
        context = ChatContext(
            session_id=session.session_id,
            token=session.token
        )
        request = ConversationRequest(title=title, metadata=metadata)
        return await self.client.start_conversation(context=context, request=request)
    
    async def _get_response(self, session, conversation_id: str, query: str):
        """Get a response from the GPO Core API."""
        context = ChatContext(
            session_id=session.session_id,
            conversation_id=conversation_id,
            token=session.token
        )
        request = ChatRequest(query=query, use_openai=True)
        return await self.client.chat(context=context, request=request)
    
    def _extract_title(self, query: str, max_length: int = 50) -> str:
        """Extract a title from the query."""
        if len(query) <= max_length:
            return query
        return f"{query[:max_length].rsplit(' ', 1)[0]}..."
    
    def _enhance_query(self, query: str, region: Optional[str], industry: Optional[str]) -> str:
        """Enhance the query with additional context."""
        enhanced_query = query
        
        if region:
            enhanced_query += f"\n\nPlease focus on the {region} region."
        
        if industry:
            enhanced_query += f"\n\nThe advice should be specifically tailored for the {industry} industry."
        
        return enhanced_query

# Usage example
async def use_tax_advisor_service():
    service = TaxAdvisorService()
    
    result = await service.get_tax_advice(
        query="What are the tax implications of establishing a subsidiary in Mexico?",
        user_id="user-123",
        instance_id="capital-edge-instance-001",
        project_id="project-456",
        region="LATAM",
        industry="Technology"
    )
    
    if result["success"]:
        print(f"Advice: {result['advice'][:200]}...")
        print(f"Sources: {', '.join([s['title'] for s in result['sources']])}")
    else:
        print(f"Error: {result['error']}")
```

### Caching Responses

```python
import asyncio
import hashlib
import json
from datetime import datetime, timedelta
from typing import Dict, Any, Optional
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import ChatContext, ChatRequest

class CachedGPOCoreClient:
    """
    A wrapper around GPOCoreClient that adds caching capabilities.
    """
    
    def __init__(self, client=None, cache_ttl_seconds=300):
        """
        Initialize the cached client.
        
        Args:
            client: Existing GPOCoreClient instance (creates new one if None)
            cache_ttl_seconds: Time-to-live for cache entries in seconds
        """
        self.client = client or GPOCoreClient()
        self.cache = {}  # Simple in-memory cache
        self.cache_ttl_seconds = cache_ttl_seconds
        self.lock = asyncio.Lock()
    
    async def chat(self, context: ChatContext, request: ChatRequest, use_cache: bool = True):
        """
        Send a chat request, using cache if available.
        
        Args:
            context: Chat context
            request: Chat request
            use_cache: Whether to use cache (default: True)
            
        Returns:
            Chat response
        """
        if not use_cache:
            # Bypass cache
            return await self.client.chat(context=context, request=request)
        
        # Generate cache key
        cache_key = self._generate_cache_key(context, request)
        
        # Try to get from cache
        cached_response = await self._get_from_cache(cache_key)
        if cached_response:
            print("Using cached response")
            return cached_response
        
        # Not in cache, call the API
        response = await self.client.chat(context=context, request=request)
        
        # Store in cache
        await self._store_in_cache(cache_key, response)
        
        return response
    
    def _generate_cache_key(self, context: ChatContext, request: ChatRequest) -> str:
        """Generate a unique cache key for a request."""
        # Include relevant parts of context and request in the key
        key_data = {
            "conversation_id": context.conversation_id,
            "query": request.query,
            "use_openai": request.use_openai
        }
        
        # Convert to a stable string representation and hash
        serialized = json.dumps(key_data, sort_keys=True)
        return hashlib.md5(serialized.encode()).hexdigest()
    
    async def _get_from_cache(self, key: str) -> Optional[Any]:
        """Get a value from cache if it exists and is not expired."""
        if key in self.cache:
            entry = self.cache[key]
            if datetime.now() < entry["expires"]:
                return entry["value"]
            else:
                # Remove expired entry
                async with self.lock:
                    if key in self.cache:
                        del self.cache[key]
        return None
    
    async def _store_in_cache(self, key: str, value: Any):
        """Store a value in the cache with expiration time."""
        async with self.lock:
            self.cache[key] = {
                "value": value,
                "expires": datetime.now() + timedelta(seconds=self.cache_ttl_seconds)
            }
    
    async def clear_cache(self):
        """Clear all cached entries."""
        async with self.lock:
            self.cache.clear()

# Usage example
async def cached_client_example(session, conversation_id):
    # Create cached client
    cached_client = CachedGPOCoreClient()
    
    # Create context
    context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation_id,
        token=session.token
    )
    
    # Create request
    request = ChatRequest(
        query="What are the tax implications of a merger in Mexico?",
        use_openai=True
    )
    
    # First call (will hit the API)
    print("First call...")
    start_time = datetime.now()
    response1 = await cached_client.chat(context=context, request=request)
    elapsed1 = (datetime.now() - start_time).total_seconds()
    print(f"Response received in {elapsed1:.2f} seconds")
    
    # Second call with the same request (should use cache)
    print("\nSecond call (should use cache)...")
    start_time = datetime.now()
    response2 = await cached_client.chat(context=context, request=request)
    elapsed2 = (datetime.now() - start_time).total_seconds()
    print(f"Response received in {elapsed2:.2f} seconds")
    
    # Third call with cache bypassed
    print("\nThird call (bypass cache)...")
    start_time = datetime.now()
    response3 = await cached_client.chat(context=context, request=request, use_cache=False)
    elapsed3 = (datetime.now() - start_time).total_seconds()
    print(f"Response received in {elapsed3:.2f} seconds")
    
    return {
        "first_call": elapsed1,
        "second_call": elapsed2,
        "third_call": elapsed3,
        "cached_hit_speedup": f"{elapsed1/elapsed2:.1f}x"
    }
```

## Conclusion

These examples demonstrate the various ways the GPO Core Python Client can be used in your applications. For more advanced scenarios or specific use cases, refer to the [API Reference](api_reference.md) or [Integration Guide](integration.md) documentation.