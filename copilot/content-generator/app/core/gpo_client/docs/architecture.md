# Architecture Overview

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document provides a comprehensive overview of the GPO Core Python Client's architecture, explaining how all components work together to provide a clean, efficient interface to the GPO Core API.

## High-Level Architecture

The GPO Core Python Client follows a layered architecture designed to provide clean separation of concerns:

```
┌─────────────────────────────────────────┐
│              Client Layer               │
│          (GPOCoreClient class)          │
└───────────────────┬─────────────────────┘
                    │
┌───────────────────▼─────────────────────┐
│               Service Layer             │
│ (Session, Conversation, Chat services)  │
└───────────────────┬─────────────────────┘
                    │
┌───────────────────▼─────────────────────┐
│              HTTP Layer                 │
│   (Request handling, retries, etc.)     │
└───────────────────┬─────────────────────┘
                    │
┌───────────────────▼─────────────────────┐
│             Authentication              │
│   (Token handling, validation, etc.)    │
└───────────────────┬─────────────────────┘
                    │
┌───────────────────▼─────────────────────┐
│                Models                   │
│  (Request/Response data structures)     │
└─────────────────────────────────────────┘
```

## GPO API Integration

The GPO Core Python Client is specifically designed to communicate with the GPO Core API, which provides advanced knowledge retrieval and AI-powered responses for business inquiries. The GPO Core API offers the following capabilities:

1. **Knowledge Base Access**: Access to an extensive corpus of business regulations, guidelines, and precedents.
2. **Natural Language Processing**: Interpretation of complex business-related questions in natural language.
3. **AI-Powered Responses**: Generation of accurate, contextually relevant responses.
4. **Source Attribution**: Citations and references to original source documents.
5. **Session Management**: Long-running conversation sessions with context retention.

### GPO API Architecture

The GPO Core API implements a RESTful API with the following key endpoints:

| Endpoint | Purpose |
|----------|---------|
| `/session` | Create and manage authentication sessions |
| `/conversation` | Create and manage conversation contexts |
| `/chat` | Process user queries and generate responses |
| `/streaming` | Stream responses for real-time display |
| `/sensitive` | Handle queries containing sensitive data with enhanced security |

### Client-Server Communication Model

The GPO Core Python Client communicates with the server through a well-defined protocol:

```
┌──────────────┐                           ┌──────────────┐
│              │     1. HTTP Request       │              │
│  GPO Core    │ ─────────────────────────>│   GPO Core   │
│   Client     │                           │     API      │
│              │     2. JSON Response      │              │
│              │ <─────────────────────────│              │
└──────────────┘                           └──────────────┘
```

The client and server exchange structured JSON payloads with specific schemas that ensure data validation and type safety.

### Integration Benefits

The GPO Core Python Client integrates seamlessly with the GPO Core API due to the following design decisions:

1. **Protocol Alignment**: The client implements all protocols and message formats expected by the GPO Core API, ensuring reliable request-response handling.

2. **Authentication Flow**: The client's authentication mechanism is designed to work with the GPO Core API's JWT-based authentication system, handling token acquisition, validation, and renewal automatically.

3. **Structured Requests/Responses**: All data models in the client match the server's expected request and response formats, leveraging Pydantic for validation and serialization.

4. **Error Handling**: The client's error handling system is designed to interpret and handle the specific error codes and messages returned by the GPO Core API, with appropriate retries for transient failures.

5. **Performance Optimizations**: The client implements optimizations like connection pooling and retry logic that work well with the GPO Core API's behavior under load, ensuring robustness during peak usage.

6. **Streaming Support**: The client provides support for the server's streaming response capabilities, allowing for real-time content display in user interfaces.

7. **Security Model**: The client adheres to the same security principles and data handling policies as the server, ensuring end-to-end protection of sensitive information.

8. **Asynchronous Processing**: The client's async/await design allows for non-blocking operation that complements the GPO Core API's concurrent request processing model, making it ideal for high-throughput applications.

9. **Flexible Configuration**: The configuration system mirrors the server's environment-based setup, allowing for consistent deployment across different environments (development, testing, production).

10. **API Version Compatibility**: The client is designed to handle API versioning gracefully, with forward and backward compatibility considerations built into the interface design.

### System Requirements

To effectively communicate with the GPO Core API, the GPO Core Python Client requires:

- Python 3.8 or higher
- Network access to the GPO Core API endpoints
- Proper configuration (base URL, etc.) as detailed in the [Configuration documentation](configuration.md)
- Authentication credentials appropriate for the GPO Core API environment

## Core Components

### 1. Client Layer (`client.py`)

The `GPOCoreClient` class is the main entry point of the library. It provides a high-level interface to all GPO Core API operations. This class is responsible for:

- Initializing the configuration
- Managing the HTTP client lifecycle
- Exposing the public API methods
- Coordinating between different layers

```python
# Example of the GPOCoreClient class structure
class GPOCoreClient:
    def __init__(self, config=None):
        self.config = config or GPOConfig.from_env()
        self.http_client = HttpClient(self.config)
        self.auth_handler = AuthHandler(self.config)
    
    async def session(self, context):
        # Implementation for session endpoint
        pass
        
    async def start_conversation(self, context, request):
        # Implementation for conversation endpoint
        pass
        
    async def chat(self, context, request):
        # Implementation for chat endpoint
        pass
        
    async def chat_sensitive_data(self, context, request):
        # Implementation for sensitive data chat endpoint
        pass
        
    async def chat_stream(self, context, request):
        # Implementation for streaming chat endpoint
        pass
```

### 2. Authentication Layer (`auth.py`)

The authentication layer handles JWT token management, validation, and token renewal if needed. It consists of:

- `AuthHandler`: Main class for authentication operations
- `TokenValidator`: Validates JWT tokens and their claims
- `RSAKeyManager`: Manages RSA public keys for token verification

This layer implements the security mechanisms required to interact with the GPO Core API securely.

### 3. Configuration (`config.py`)

The configuration module manages all settings for the client. It provides:

- Default configurations
- Environment variable loading
- Configuration file parsing
- Configuration validation

The `GPOConfig` class holds all configuration values and provides helper methods to load them from different sources.

### 4. HTTP Layer (`utils/http.py`)

This layer handles all HTTP communication with the GPO Core API:

- Making HTTP requests to the API endpoints
- Implementing retry logic with exponential backoff
- Handling timeouts
- Processing responses and errors
- Supporting streaming responses

### 5. Models (`models/`)

The models package contains Pydantic models for:

- Request objects
- Response objects
- Context objects
- Data validation

These models provide type safety and validation for all data exchanged with the API.

## Data Flow

### 1. Request Flow

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│  User Code  │ -> │GPOCoreClient│ -> │HTTP Client  │ -> │  GPO Core   │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
       │                 │                  │                  │
       │  Call method    │                  │                  │
       │ with context    │                  │                  │
       │---------------->│                  │                  │
       │                 │  Prepare request │                  │
       │                 │  Add auth headers│                  │
       │                 │----------------->│                  │
       │                 │                  │  Send HTTP req   │
       │                 │                  │----------------->│
       │                 │                  │                  │  Process
       │                 │                  │                  │  request
```

### 2. Response Flow

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│  User Code  │ <- │GPOCoreClient│ <- │HTTP Client  │ <- │  GPO Core   │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
       │                 │                  │                  │
       │                 │                  │                  │  Send HTTP 
       │                 │                  │                  │  response
       │                 │                  │<-----------------| 
       │                 │                  │  Process resp    │
       │                 │                  │  Handle errors   │
       │                 │<-----------------|                  │
       │                 │  Convert to      │                  │
       │                 │  model objects   │                  │
       │<----------------|                  │                  │
       │  Use response   │                  │                  │
       │  objects        │                  │                  │
```

## Error Handling

The client implements a comprehensive error handling strategy:

1. HTTP-level errors are captured by the HTTP client
2. Application errors returned by the API are parsed and converted to appropriate exceptions
3. Exceptions are categorized into different types (auth errors, validation errors, etc.)
4. All exceptions inherit from a base `GPOClientException` class

## Retry Mechanism

The client includes a configurable retry mechanism for handling transient errors:

1. Retries are implemented using the `backoff` library
2. Configurable backoff factor and maximum number of retries
3. Exponential backoff strategy with jitter
4. Selective retry based on error types (retries for network errors but not for validation errors)

## Thread Safety and Concurrency

The client is designed to be safe for concurrent use:

1. Uses `aiohttp` for asynchronous HTTP requests
2. Properly manages connection pooling
3. Maintains proper resource cleanup

## Integration Points

The GPO Core Python Client is designed to integrate with:

1. `chatbot-python-api` backend service
2. `content-generator` service
3. Other potential Capital Edge services

Each integration point may have specific requirements for authentication, error handling, and response processing, which are documented in the [Integration Guide](integration.md).