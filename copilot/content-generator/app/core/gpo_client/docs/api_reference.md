# API Reference

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document provides a comprehensive reference of all available classes, methods, and models in the GPO Core Python Client.

## Client Classes

### GPOCoreClient

The main client class for interacting with the GPO Core API.

```python
class GPOCoreClient:
    def __init__(self, config: Optional[GPOConfig] = None):
        """
        Initialize the GPO Core client.
        
        Args:
            config: Optional configuration. If not provided, loads from environment.
        """
        
    @classmethod
    def from_config_file(cls, file_path: str) -> "GPOCoreClient":
        """
        Create a client from a configuration file.
        
        Args:
            file_path: Path to YAML or JSON configuration file.
            
        Returns:
            Configured GPOCoreClient instance.
        """
        
    async def session(self, context: SessionContext) -> SessionResponse:
        """
        Create a new session with the GPO Core API.
        
        Args:
            context: Session context containing user and project information.
            
        Returns:
            Session response with token and session ID.
            
        Raises:
            AuthenticationError: If authentication fails.
            ConnectionError: If unable to connect to the API.
            ServiceUnavailableError: If the service is unavailable.
        """
        
    async def start_conversation(self, 
                                context: ChatContext, 
                                request: Union[dict, ConversationRequest]) -> ConversationResponse:
        """
        Start a new conversation.
        
        Args:
            context: Chat context with session information.
            request: Conversation request with title and optional metadata.
            
        Returns:
            Conversation response with conversation ID and metadata.
            
        Raises:
            AuthenticationError: If authentication fails.
            InvalidRequestError: If request is invalid.
            ConnectionError: If unable to connect to the API.
        """
        
    async def chat(self, 
                  context: ChatContext, 
                  request: Union[dict, ChatRequest]) -> ChatResponse:
        """
        Send a chat message and get a response.
        
        Args:
            context: Chat context with session and conversation information.
            request: Chat request with query and options.
            
        Returns:
            Chat response with generated content and metadata.
            
        Raises:
            AuthenticationError: If authentication fails.
            InvalidRequestError: If request is invalid.
            ConnectionError: If unable to connect to the API.
        """
        
    async def chat_stream(self, 
                         context: ChatContext, 
                         request: Union[dict, StreamingChatRequest]) -> AsyncIterator[StreamingChatResponse]:
        """
        Send a chat message and get a streaming response.
        
        Args:
            context: Chat context with session and conversation information.
            request: Streaming chat request with query and options.
            
        Returns:
            Async iterator of streaming response chunks.
            
        Raises:
            AuthenticationError: If authentication fails.
            InvalidRequestError: If request is invalid.
            ConnectionError: If unable to connect to the API.
        """
        
    async def chat_sensitive_data(self, 
                                 context: ChatContext, 
                                 request: Union[dict, SensitiveChatRequest]) -> ChatResponse:
        """
        Send a chat message containing sensitive data with enhanced privacy.
        
        Args:
            context: Chat context with session and conversation information.
            request: Sensitive chat request with query and sensitive data markers.
            
        Returns:
            Chat response with generated content and metadata.
            
        Raises:
            AuthenticationError: If authentication fails.
            InvalidRequestError: If request is invalid.
            ConnectionError: If unable to connect to the API.
        """
        
    async def get_conversation_history(self, 
                                      context: ChatContext) -> List[ConversationHistoryItem]:
        """
        Retrieve the history of a conversation.
        
        Args:
            context: Chat context with session and conversation information.
            
        Returns:
            List of conversation history items.
            
        Raises:
            AuthenticationError: If authentication fails.
            InvalidRequestError: If request is invalid.
            ConnectionError: If unable to connect to the API.
        """
```

## Context Models

### SessionContext

```python
class SessionContext(BaseModel):
    instance_id: str
    user_id: str
    project_id: str
    environment: str = "production"
    
    class Config:
        extra = "forbid"
```

### ChatContext

```python
class ChatContext(BaseModel):
    session_id: str
    token: str
    conversation_id: Optional[str] = None
    
    class Config:
        extra = "forbid"
```

## Request Models

### ConversationRequest

```python
class ConversationRequest(BaseModel):
    title: str
    metadata: Optional[Dict[str, Any]] = None
    
    class Config:
        extra = "forbid"
```

### ChatRequest

```python
class ChatRequest(BaseModel):
    query: str
    use_openai: bool = True
    client_metadata: Optional[Dict[str, Any]] = None
    
    class Config:
        extra = "forbid"
```

### StreamingChatRequest

```python
class StreamingChatRequest(ChatRequest):
    stream: bool = True
    
    class Config:
        extra = "forbid"
```

### SensitiveChatRequest

```python
class SensitivityMarker(BaseModel):
    type: str
    start: int
    end: int
    
class ChatHistory(BaseModel):
    role: str
    content: str
    
class SensitiveChatRequest(ChatRequest):
    is_sensitive: bool = True
    sensitivity_markers: Optional[List[SensitivityMarker]] = None
    data_handling_policy: Optional[str] = None
    chat_history: Optional[List[ChatHistory]] = None
    
    class Config:
        extra = "forbid"
```

## Response Models

### SessionResponse

```python
class SessionResponse(BaseModel):
    session_id: str
    token: str
    expires_at: datetime
    user_context: Optional[Dict[str, Any]] = None
    
    class Config:
        extra = "allow"
```

### ConversationResponse

```python
class ConversationResponse(BaseModel):
    conversation_id: str
    title: str
    created_at: datetime
    status: str
    metadata: Optional[Dict[str, Any]] = None
    
    class Config:
        extra = "allow"
```

### Source

```python
class Source(BaseModel):
    id: str
    title: str
    relevance_score: float
    url: Optional[str] = None
    metadata: Optional[Dict[str, Any]] = None
    
    class Config:
        extra = "allow"
```

### ChatResponse

```python
class ChatResponse(BaseModel):
    message_id: str
    message_content: str
    response_time_ms: int
    model_used: str
    sources: List[Source] = []
    compliance_warnings: Optional[List[Dict[str, Any]]] = None
    
    class Config:
        extra = "allow"
```

### StreamingChatResponse

```python
class StreamingChatResponse(BaseModel):
    content: Optional[str] = None
    sources: Optional[List[Source]] = None
    is_complete: bool = False
    message_id: Optional[str] = None
    
    class Config:
        extra = "allow"
```

### ConversationHistoryItem

```python
class ConversationHistoryItem(BaseModel):
    message_id: str
    role: str
    content: str
    timestamp: datetime
    metadata: Optional[Dict[str, Any]] = None
    
    class Config:
        extra = "allow"
```

## Configuration Classes

### GPOConfig

```python
class GPOConfig:
    def __init__(self, 
                base_url: str = None,
                timeout: int = None,
                max_retries: int = None,
                retry_backoff_factor: float = None,
                verify_ssl: bool = None,
                auto_renew_token: bool = None,
                token_renewal_buffer: int = None,
                user_agent: str = None,
                log_level: str = None,
                trace_requests: bool = None):
        """
        Initialize configuration with optional parameters.
        
        Args:
            base_url: Base URL for the GPO API.
            timeout: Request timeout in seconds.
            max_retries: Maximum number of request retries.
            retry_backoff_factor: Backoff factor for retries.
            verify_ssl: Whether to verify SSL certificates.
            auto_renew_token: Whether to automatically renew tokens before expiry.
            token_renewal_buffer: Seconds before expiry to renew tokens.
            user_agent: User agent string for requests.
            log_level: Log level for client logging.
            trace_requests: Whether to log request and response details.
        """
        
    @classmethod
    def from_env(cls, prefix: str = "GPO_API_") -> "GPOConfig":
        """
        Create configuration from environment variables.
        
        Args:
            prefix: Environment variable prefix.
            
        Returns:
            Configuration instance.
        """
        
    @classmethod
    def from_file(cls, file_path: str) -> "GPOConfig":
        """
        Create configuration from file.
        
        Args:
            file_path: Path to YAML or JSON file.
            
        Returns:
            Configuration instance.
        """
        
    def to_dict(self) -> Dict[str, Any]:
        """
        Convert configuration to dictionary.
        
        Returns:
            Dictionary of configuration values.
        """
```

## Exception Classes

### Exception Hierarchy

```
GPOCoreClientException
  ├── AuthenticationError
  ├── ConnectionError
  │     └── TimeoutError
  ├── RateLimitError
  ├── InvalidRequestError
  ├── ServiceUnavailableError
  └── ServerError
```

### GPOCoreClientException

```python
class GPOCoreClientException(Exception):
    """Base exception for all client exceptions."""
    
    def __init__(self, message: str, code: Optional[str] = None):
        """
        Initialize with message and optional error code.
        
        Args:
            message: Error message.
            code: Optional error code.
        """
        super().__init__(message)
        self.code = code
```

### AuthenticationError

```python
class AuthenticationError(GPOCoreClientException):
    """Exception raised for authentication failures."""
    
    def __init__(self, message: str, code: Optional[str] = None):
        """
        Initialize with message and optional error code.
        
        Args:
            message: Error message.
            code: Optional error code.
        """
        super().__init__(message, code)
```

### ConnectionError

```python
class ConnectionError(GPOCoreClientException):
    """Exception raised for connection problems."""
    
    def __init__(self, message: str, attempts: int = 0, code: Optional[str] = None):
        """
        Initialize with message and optional parameters.
        
        Args:
            message: Error message.
            attempts: Number of failed attempts.
            code: Optional error code.
        """
        super().__init__(message, code)
        self.attempts = attempts
```

### TimeoutError

```python
class TimeoutError(ConnectionError):
    """Exception raised when a request times out."""
    
    def __init__(self, message: str, attempts: int = 0, code: Optional[str] = None):
        """
        Initialize with message and optional parameters.
        
        Args:
            message: Error message.
            attempts: Number of failed attempts.
            code: Optional error code.
        """
        super().__init__(message, attempts, code)
```

### RateLimitError

```python
class RateLimitError(GPOCoreClientException):
    """Exception raised when rate limits are exceeded."""
    
    def __init__(self, message: str, retry_after: Optional[int] = None, code: Optional[str] = None):
        """
        Initialize with message and optional parameters.
        
        Args:
            message: Error message.
            retry_after: Seconds to wait before retrying.
            code: Optional error code.
        """
        super().__init__(message, code)
        self.retry_after = retry_after
```

### InvalidRequestError

```python
class InvalidRequestError(GPOCoreClientException):
    """Exception raised for invalid requests."""
    
    def __init__(self, message: str, field: Optional[str] = None, code: Optional[str] = None):
        """
        Initialize with message and optional parameters.
        
        Args:
            message: Error message.
            field: Field that caused the error.
            code: Optional error code.
        """
        super().__init__(message, code)
        self.field = field
```

### ServiceUnavailableError

```python
class ServiceUnavailableError(GPOCoreClientException):
    """Exception raised when the service is unavailable."""
    
    def __init__(self, message: str, code: Optional[str] = None):
        """
        Initialize with message and optional error code.
        
        Args:
            message: Error message.
            code: Optional error code.
        """
        super().__init__(message, code)
```

### ServerError

```python
class ServerError(GPOCoreClientException):
    """Exception raised for server-side errors."""
    
    def __init__(self, message: str, status_code: Optional[int] = None, code: Optional[str] = None):
        """
        Initialize with message and optional parameters.
        
        Args:
            message: Error message.
            status_code: HTTP status code.
            code: Optional error code.
        """
        super().__init__(message, code)
        self.status_code = status_code
```

## HTTP Utilities

### HttpClient

```python
class HttpClient:
    def __init__(self, config: GPOConfig):
        """
        Initialize HTTP client with configuration.
        
        Args:
            config: Client configuration.
        """
        
    async def request(self, 
                     method: str, 
                     path: str, 
                     headers: Optional[Dict[str, str]] = None,
                     params: Optional[Dict[str, Any]] = None,
                     json_data: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
        """
        Send HTTP request to the API.
        
        Args:
            method: HTTP method (GET, POST, etc.)
            path: API path.
            headers: Request headers.
            params: URL parameters.
            json_data: JSON request body.
            
        Returns:
            API response.
            
        Raises:
            ConnectionError: If unable to connect to the API.
            TimeoutError: If the request times out.
            AuthenticationError: If authentication fails.
            InvalidRequestError: If request is invalid.
            RateLimitError: If rate limits are exceeded.
            ServiceUnavailableError: If the service is unavailable.
            ServerError: For other server errors.
        """
        
    async def stream(self, 
                    method: str, 
                    path: str, 
                    headers: Optional[Dict[str, str]] = None,
                    params: Optional[Dict[str, Any]] = None,
                    json_data: Optional[Dict[str, Any]] = None) -> AsyncIterator[Dict[str, Any]]:
        """
        Send HTTP request and stream the response.
        
        Args:
            method: HTTP method (GET, POST, etc.)
            path: API path.
            headers: Request headers.
            params: URL parameters.
            json_data: JSON request body.
            
        Returns:
            Async iterator of response chunks.
            
        Raises:
            ConnectionError: If unable to connect to the API.
            TimeoutError: If the request times out.
            AuthenticationError: If authentication fails.
            InvalidRequestError: If request is invalid.
            RateLimitError: If rate limits are exceeded.
            ServiceUnavailableError: If the service is unavailable.
            ServerError: For other server errors.
        """
```

## Authentication Utilities

### AuthHandler

```python
class AuthHandler:
    def __init__(self, config: GPOConfig):
        """
        Initialize authentication handler with configuration.
        
        Args:
            config: Client configuration.
        """
        
    def validate_token(self, token: str, input_data: Dict[str, Any]) -> bool:
        """
        Validate JWT token.
        
        Args:
            token: JWT token.
            input_data: Data to validate against token claims.
            
        Returns:
            True if valid, False otherwise.
        """
        
    def extract_token_claims(self, token: str) -> Dict[str, Any]:
        """
        Extract and validate claims from token.
        
        Args:
            token: JWT token.
            
        Returns:
            Token claims.
            
        Raises:
            AuthenticationError: If token is invalid.
        """
        
    def prepare_auth_header(self, token: str) -> Dict[str, str]:
        """
        Create authorization headers from token.
        
        Args:
            token: JWT token.
            
        Returns:
            Headers dictionary with Authorization.
        """
```

## Enumerations

### EnvironmentType

```python
class EnvironmentType(str, Enum):
    PRODUCTION = "production"
    STAGING = "staging"
    DEVELOPMENT = "development"
    TEST = "test"
```

### ConversationStatus

```python
class ConversationStatus(str, Enum):
    ACTIVE = "active"
    ARCHIVED = "archived"
    DELETED = "deleted"
```

### MessageRole

```python
class MessageRole(str, Enum):
    USER = "user"
    ASSISTANT = "assistant"
    SYSTEM = "system"
```

## Constants

```python
# Default configuration values
DEFAULT_BASE_URL = "https://gpo-api.example.com"
DEFAULT_TIMEOUT = 30
DEFAULT_MAX_RETRIES = 3
DEFAULT_RETRY_BACKOFF_FACTOR = 0.5
DEFAULT_VERIFY_SSL = True
DEFAULT_AUTO_RENEW_TOKEN = False
DEFAULT_TOKEN_RENEWAL_BUFFER = 60
DEFAULT_USER_AGENT = "gpo-python-client/0.1.0"
DEFAULT_LOG_LEVEL = "INFO"
DEFAULT_TRACE_REQUESTS = False

# API endpoints
ENDPOINT_SESSION = "/api/User/session"
ENDPOINT_CONVERSATION = "/api/Chat/start-conversation"
ENDPOINT_CHAT = "/api/Chat"
ENDPOINT_CHAT_SENSITIVE = "/api/Chat/sensitive"
ENDPOINT_CONVERSATION_HISTORY = "/api/Chat/history"

# Header names
HEADER_CONTENT_TYPE = "Content-Type"
HEADER_AUTHORIZATION = "Authorization"
HEADER_USER_AGENT = "User-Agent"

# Status codes
HTTP_TOO_MANY_REQUESTS = 429
HTTP_UNAUTHORIZED = 401
HTTP_BAD_REQUEST = 400
HTTP_SERVICE_UNAVAILABLE = 503
HTTP_INTERNAL_SERVER_ERROR = 500
```

## Usage Examples

For detailed usage examples, see the [Examples](examples.md) document.