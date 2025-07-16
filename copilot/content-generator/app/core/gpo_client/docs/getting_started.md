# Getting Started

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This guide will help you get started with the GPO Core Python Client. It covers installation, basic usage, and common patterns to help you integrate the client into your applications quickly and efficiently.

## Installation

The GPO Core Python Client is designed to be embedded within Capital Edge applications. To use the client:

1. Add the client as a dependency in your project:

   ```bash
   # If using pip directly
   pip install -e /path/to/gpo-core-client
   
   # If using requirements.txt
   echo "app.core.gpo_client @ file:///path/to/gpo-core-client" >> requirements.txt
   pip install -r requirements.txt
   ```

2. Import the client in your code:

   ```python
   from app.core.gpo_client.client import GPOCoreClient
   ```

## Basic Configuration

The client needs to be configured to connect to the GPO Core API. There are several ways to configure the client:

### 1. Using Environment Variables

The simplest way to configure the client is through environment variables:

```bash
# Set environment variables
export GPO_API_BASE_URL="https://gpo-api.example.com"
export GPO_API_TIMEOUT="30"
export GPO_API_MAX_RETRIES="3"
```

Then initialize the client without parameters:

```python
from app.core.gpo_client.client import GPOCoreClient

client = GPOCoreClient()  # Will use environment variables
```

### 2. Using a Configuration File

You can create a YAML or JSON configuration file:

```yaml
# gpo_config.yaml
base_url: "https://gpo-api.example.com"
timeout: 30
max_retries: 3
verify_ssl: true
```

Then load the configuration from the file:

```python
from app.core.gpo_client.client import GPOCoreClient

client = GPOCoreClient.from_config_file("gpo_config.yaml")
```

### 3. Using Direct Parameter Passing

You can also configure the client by passing parameters directly:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.config import GPOConfig

config = GPOConfig(
    base_url="https://gpo-api.example.com",
    timeout=30,
    max_retries=3
)

client = GPOCoreClient(config=config)
```

## Quick Start Example

Here's a complete example that demonstrates the basic workflow:

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

async def main():
    # 1. Initialize the client
    client = GPOCoreClient()
    
    # 2. Create a session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    
    session = await client.session(session_context)
    print(f"Session created: {session.session_id}")
    
    # 3. Start a conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Tax Consultation"}
    )
    print(f"Conversation started: {conversation.conversation_id}")
    
    # 4. Send a chat message
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
    
    # 5. Process the response
    print(f"Response: {response.message_content}")
    
    # 6. Display sources
    if response.sources:
        print("\nSources:")
        for source in response.sources:
            print(f"- {source.title} (Relevance: {source.relevance_score})")

if __name__ == "__main__":
    asyncio.run(main())
```

## Error Handling

The GPO Core Python Client provides a comprehensive error handling mechanism. Here's a basic error handling example:

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest
from app.core.gpo_client.exceptions import (
    GPOCoreClientException,
    AuthenticationError,
    ConnectionError,
    RateLimitError,
    InvalidRequestError
)

async def main():
    client = GPOCoreClient()
    
    try:
        # Create a session
        session_context = SessionContext(
            instance_id="capital-edge-instance-001",
            user_id="user-123",
            project_id="project-456"
        )
        
        session = await client.session(session_context)
        
        # Prepare chat context
        chat_context = ChatContext(
            session_id=session.session_id,
            token=session.token
        )
        
        # Start a conversation
        conversation = await client.start_conversation(
            context=chat_context,
            request={"title": "Tax Consultation"}
        )
        
        # Update context with conversation ID
        chat_context.conversation_id = conversation.conversation_id
        
        # Send a chat message
        request = ChatRequest(
            query="What are the tax implications of a merger between companies in Mexico?",
            use_openai=True
        )
        
        response = await client.chat(context=chat_context, request=request)
        print(f"Response: {response.message_content}")
        
    except AuthenticationError as e:
        print(f"Authentication error: {e}")
        # Handle authentication issues, potentially by refreshing the session
        
    except ConnectionError as e:
        print(f"Connection error: {e} (attempts: {e.attempts})")
        # Handle connection issues, potentially by retrying after a delay
        
    except RateLimitError as e:
        print(f"Rate limit exceeded: {e} (retry after: {e.retry_after} seconds)")
        # Handle rate limiting by waiting the suggested time before retrying
        
    except InvalidRequestError as e:
        print(f"Invalid request: {e} (field: {e.field})")
        # Handle validation errors in the request
        
    except GPOCoreClientException as e:
        print(f"Client error: {e}")
        # Handle other client errors
        
    except Exception as e:
        print(f"Unexpected error: {e}")
        # Handle unexpected errors

if __name__ == "__main__":
    asyncio.run(main())
```

## Working with Streaming Responses

The GPO Core Python Client supports streaming responses for real-time display:

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, StreamingChatRequest

async def stream_example():
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
        request={"title": "Streaming Example"}
    )
    
    # Update context with conversation ID
    chat_context.conversation_id = conversation.conversation_id
    
    # Create streaming request
    request = StreamingChatRequest(
        query="Explain the global minimum tax in detail with a step-by-step breakdown.",
        stream=True,
        use_openai=True
    )
    
    # Process streaming response
    full_response = ""
    sources = []
    
    print("Streaming response:")
    async for chunk in client.chat_stream(context=chat_context, request=request):
        if chunk.content:
            print(chunk.content, end="", flush=True)
            full_response += chunk.content
        
        if chunk.sources and not sources:
            sources = chunk.sources
            
        if chunk.is_complete:
            print("\n\nResponse complete")
            print(f"Message ID: {chunk.message_id}")
    
    print("\n\nFull response:", full_response)
    
    # Display sources
    if sources:
        print("\nSources:")
        for source in sources:
            print(f"- {source.title}")

if __name__ == "__main__":
    asyncio.run(stream_example())
```

## Next Steps

Now that you're familiar with the basics of the GPO Core Python Client, you might want to explore:

- [API Reference](api_reference.md) for detailed documentation of all classes and methods
- [Configuration](configuration.md) for advanced configuration options
- [Examples](examples.md) for more usage examples
- [Authentication](authentication.md) for details on authentication and token handling
- [Error Handling](error_handling.md) for more information on error handling strategies
- [Streaming](streaming.md) for advanced streaming techniques