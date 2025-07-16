# GPO Core Python Client

<div align="center">
  <h3>GPO Core API Python Client Library</h3>
  <p>A clean, Pythonic interface to the GPO Core API for Capital Edge backend services</p>
</div>

---

## 📖 Table of Contents

1. [Overview](#overview)
2. [Key Features](#key-features)
3. [Installation](#installation)
4. [Quick Start](#quick-start)
5. [Documentation](#documentation)
6. [Development](#development)
7. [Contributing](#contributing)
8. [License](#license)
9. [Support](#support)

---

## Overview

The GPO Core Python Client is a lightweight, asynchronous library for interacting with the GPO Core API. It provides a simple, intuitive interface for accessing the capabilities of the GPO Core platform within Python applications.

## Key Features

- **Asynchronous API**: Built with `asyncio` for efficient non-blocking operations
- **Streaming Support**: Stream responses in real-time for better user experience
- **Comprehensive Models**: Type-safe request and response models
- **Error Handling**: Robust error handling with detailed exceptions
- **Connection Management**: Automatic retry, backoff, and connection pooling
- **Flexible Configuration**: Multiple configuration methods to suit various deployment scenarios
- **Token Management**: Automatic token refresh and management

## Installation

This library is designed to be embedded within Capital Edge applications:

```bash
# If using pip directly
pip install -e /path/to/gpo-core-client

# If using requirements.txt
echo "app.core.gpo_client @ file:///path/to/gpo-core-client" >> requirements.txt
pip install -r requirements.txt
```

## Quick Start

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

async def main():
    # Initialize client
    client = GPOCoreClient()
    
    # Create a session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    session = await client.session(session_context)
    
    # Start a conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Technical Assistance"}
    )
    
    # Update context with conversation ID
    chat_context.conversation_id = conversation.conversation_id
    
    # Send a chat message
    request = ChatRequest(
        query="What are the key provisions of the OECD Pillar Two agreement?",
        use_openai=True
    )
    
    response = await client.chat(context=chat_context, request=request)
    
    # Process response
    print(f"Response: {response.message_content}")
    
    # Display sources
    if response.sources:
        print("\nSources:")
        for source in response.sources:
            print(f"- {source.title}")

if __name__ == "__main__":
    asyncio.run(main())
```

## Documentation

Comprehensive documentation is available in the [docs](./docs) directory:

- [Getting Started](./docs/getting_started.md)
- [API Reference](./docs/api_reference.md)
- [Configuration](./docs/configuration.md)
- [Authentication](./docs/authentication.md)
- [Error Handling](./docs/error_handling.md)
- [Streaming](./docs/streaming.md)
- [Examples](./docs/examples.md)

## Basic Usage Patterns

### Simple Chat

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

async def simple_chat():
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
        request={"title": "Quick Question"}
    )
    
    # Update context with conversation ID
    chat_context.conversation_id = conversation.conversation_id
    
    # Send a chat message
    request = ChatRequest(
        query="What are the implications of BEPS 2.0 for multinational enterprises?",
        use_openai=True
    )
    
    response = await client.chat(context=chat_context, request=request)
    print(f"Response: {response.message_content}")

asyncio.run(simple_chat())
```

### Streaming Chat

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, StreamingChatRequest

async def streaming_chat():
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
        request={"title": "Streaming Demo"}
    )
    
    # Update context with conversation ID
    chat_context.conversation_id = conversation.conversation_id
    
    # Create streaming request
    request = StreamingChatRequest(
        query="Explain the concept of a substance-based income exclusion in global minimum tax.",
        stream=True,
        use_openai=True
    )
    
    # Process streaming response
    print("Streaming response:")
    async for chunk in client.chat_stream(context=chat_context, request=request):
        if chunk.content:
            print(chunk.content, end="", flush=True)
            
        if chunk.is_complete:
            print("\n\nResponse complete")

asyncio.run(streaming_chat())
```

### Handling Sensitive Data

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, SensitiveChatRequest, SensitivityMarker

async def sensitive_data_chat():
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
        request={"title": "Sensitive Data Conversation"}
    )
    
    # Update context with conversation ID
    chat_context.conversation_id = conversation.conversation_id
    
    # Query containing sensitive data
    query = "My company ABC Corp (EIN: 12-3456789) needs advice on tax provisions."
    
    # Mark sensitive portions
    markers = [
        SensitivityMarker(type="ein", start=23, end=33)
    ]
    
    # Create request with sensitivity markers
    request = SensitiveChatRequest(
        query=query,
        is_sensitive=True,
        sensitivity_markers=markers,
        data_handling_policy="confidential"
    )
    
    # Send the query with sensitive data handling
    response = await client.chat_sensitive_data(context=chat_context, request=request)
    print(f"Response: {response.message_content}")

asyncio.run(sensitive_data_chat())
```

## Error Handling

The client provides detailed exception classes for proper error handling:

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext
from app.core.gpo_client.exceptions import (
    AuthenticationError,
    ConnectionError,
    RateLimitError,
    InvalidRequestError,
    GPOCoreClientException
)

async def error_handling_example():
    client = GPOCoreClient()
    
    try:
        # Attempt to create a session
        session_context = SessionContext(
            instance_id="capital-edge-instance-001",
            user_id="user-123",
            project_id="invalid-project"  # This might cause an error
        )
        
        session = await client.session(session_context)
        print(f"Session created successfully: {session.session_id}")
        
    except AuthenticationError as e:
        print(f"Authentication failed: {e}")
        # Implement authentication recovery
        
    except ConnectionError as e:
        print(f"Connection error: {e} (attempts: {e.attempts})")
        # Implement retry or fallback strategy
        
    except RateLimitError as e:
        print(f"Rate limit exceeded: {e} (retry after: {e.retry_after} seconds)")
        # Implement backoff strategy
        
    except InvalidRequestError as e:
        print(f"Invalid request: {e} (field: {e.field})")
        # Fix request parameters
        
    except GPOCoreClientException as e:
        print(f"Client error: {e}")
        # Handle other client errors
        
    except Exception as e:
        print(f"Unexpected error: {e}")
        # Handle unexpected errors

asyncio.run(error_handling_example())
```

## Development

### Setup Development Environment

```bash
# Install development dependencies
pip install -e ".[dev]"

# Run tests
pytest

# Run with coverage
pytest --cov=app.core.gpo_client
```

### Linting and Formatting

```bash
# Run linters
flake8 app/core/gpo_client
mypy app/core.gpo_client

# Format code
black app/core/gpo_client
isort app/core/gpo_client
```

## Contributing

For internal contributors, please follow these guidelines:

1. Follow the standard workflow for Capital Edge internal projects
2. Run tests before submitting changes
3. Update documentation for any API changes
4. Add examples for new features
5. Follow the coding style of the project

## License

For internal use within Capital Edge applications only.

## Support

For issues, questions, or feature requests, please contact the GPO Core team.
