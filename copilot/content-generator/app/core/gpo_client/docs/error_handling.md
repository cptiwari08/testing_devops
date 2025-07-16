# Error Handling

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document provides comprehensive guidance on handling errors when working with the GPO Core API Python Client. Proper error handling is crucial for building robust applications that can gracefully handle exceptional situations.

## Table of Contents

- [Error Hierarchy](#error-hierarchy)
- [Common Errors](#common-errors)
- [Error Handling Patterns](#error-handling-patterns)
- [Retry Mechanisms](#retry-mechanisms)
- [Logging Errors](#logging-errors)
- [Error Recovery Strategies](#error-recovery-strategies)
- [Best Practices](#best-practices)

## Error Hierarchy

The GPO Core API Python Client uses a structured hierarchy of exceptions to make error handling more manageable:

```
Exception
└── GPOClientException
    ├── AuthenticationError
    ├── InvalidRequestError
    │   └── ValidationError
    ├── ConnectionError
    ├── TimeoutError
    ├── ServerError
    │   ├── RateLimitError
    │   └── ServiceUnavailableError
    └── ClientError
```

### Base Exception

```python
class GPOClientException(Exception):
    """Base exception for all GPO client errors."""
    
    def __init__(self, message, code=None):
        super().__init__(message)
        self.message = message
        self.code = code
```

All other exceptions inherit from this base exception, allowing you to catch all client-related errors in a single except block when needed.

## Common Errors

### Authentication Errors

```python
class AuthenticationError(GPOClientException):
    """Raised when authentication fails."""
    pass
```

This error occurs when there are issues with authentication tokens. Common causes include:
- Invalid tokens
- Expired tokens
- Missing tokens
- Insufficient permissions

Example handling:

```python
from app.core.gpo_client.exceptions import AuthenticationError

try:
    response = await client.chat(context=context, request=request)
    # Process response
except AuthenticationError as e:
    # Handle authentication issues
    print(f"Authentication failed: {e}")
    # Attempt to refresh the token
    new_session = await client.session(session_context)
    context.token = new_session.token
    # Retry the request
    response = await client.chat(context=context, request=request)
```

### Invalid Request Errors

```python
class InvalidRequestError(GPOClientException):
    """Raised when the request is invalid."""
    
    def __init__(self, message, code=None, field=None):
        super().__init__(message, code)
        self.field = field  # Indicates which field caused the error
```

```python
class ValidationError(InvalidRequestError):
    """Raised when input validation fails."""
    pass
```

These errors occur when the request structure is invalid or contains validation errors. Common causes include:
- Missing required fields
- Invalid field values
- Malformed data
- Query too long

Example handling:

```python
from app.core.gpo_client.exceptions import InvalidRequestError, ValidationError

try:
    response = await client.chat(context=context, request=request)
except ValidationError as e:
    print(f"Validation error in field '{e.field}': {e.message}")
    # Fix the specific field and retry
    if e.field == "query":
        # Truncate or fix the query
        request.query = request.query[:2000]  # Truncate to allowed length
        response = await client.chat(context=context, request=request)
except InvalidRequestError as e:
    print(f"Invalid request: {e}")
    # Handle general request errors
```

### Connection Errors

```python
class ConnectionError(GPOClientException):
    """Raised when a connection error occurs."""
    
    def __init__(self, message, code=None, attempts=None):
        super().__init__(message, code)
        self.attempts = attempts  # Number of connection attempts made
```

These errors occur when there are network issues. Common causes include:
- Network unavailability
- DNS resolution problems
- TLS/SSL errors
- Proxy issues

Example handling:

```python
from app.core.gpo_client.exceptions import ConnectionError

try:
    response = await client.chat(context=context, request=request)
except ConnectionError as e:
    print(f"Connection error: {e} (Attempts: {e.attempts})")
    # Implement a backoff strategy
    await asyncio.sleep(2)  # Wait before retrying
    try:
        response = await client.chat(context=context, request=request)
    except ConnectionError:
        # If it still fails, use a fallback
        response = {"content": "Sorry, I'm having trouble connecting to the service."}
```

### Timeout Errors

```python
class TimeoutError(GPOClientException):
    """Raised when a request times out."""
    pass
```

These errors occur when a request takes too long. Common causes include:
- Server processing delays
- Network latency
- Complex queries that exceed timeout limits

Example handling:

```python
from app.core.gpo_client.exceptions import TimeoutError

try:
    response = await client.chat(context=context, request=request)
except TimeoutError as e:
    print(f"Request timed out: {e}")
    # Handle timeout specifically
    # Option 1: Retry with a longer timeout
    config = client.config.copy()
    config.timeout *= 2  # Double the timeout
    temp_client = GPOCoreClient(config=config)
    try:
        response = await temp_client.chat(context=context, request=request)
    except TimeoutError:
        # Option 2: Simplify the request
        request.query = "Please provide a brief answer to: " + request.query
        response = await client.chat(context=context, request=request)
```

### Server Errors

```python
class ServerError(GPOClientException):
    """Base class for server-side errors."""
    pass

class RateLimitError(ServerError):
    """Raised when rate limits are exceeded."""
    
    def __init__(self, message, code=None, retry_after=None):
        super().__init__(message, code)
        self.retry_after = retry_after  # Time in seconds to wait before retry
        
class ServiceUnavailableError(ServerError):
    """Raised when the service is unavailable."""
    pass
```

These errors occur when there are server-side issues. Common causes include:
- Too many requests (rate limiting)
- Server overload
- Maintenance or downtime
- Internal server errors

Example handling:

```python
from app.core.gpo_client.exceptions import RateLimitError, ServiceUnavailableError

try:
    response = await client.chat(context=context, request=request)
except RateLimitError as e:
    retry_seconds = e.retry_after or 60  # Default to 60 seconds if not specified
    print(f"Rate limit exceeded. Retrying in {retry_seconds} seconds.")
    await asyncio.sleep(retry_seconds)
    # Retry the request after waiting
    response = await client.chat(context=context, request=request)
except ServiceUnavailableError as e:
    print(f"Service unavailable: {e}")
    # Use a fallback or cached response
    response = get_cached_response(request.query) or {"content": "Service is currently unavailable."}
```

## Error Handling Patterns

### Try-Except-Else-Finally Pattern

For comprehensive error handling:

```python
async def get_chat_response(client, context, query):
    """Get a chat response with comprehensive error handling."""
    request = ChatRequest(query=query, use_openai=True)
    
    try:
        # Attempt the primary action
        response = await client.chat(context=context, request=request)
    except AuthenticationError as e:
        # Handle authentication errors
        logger.error(f"Authentication error: {e}")
        # Refresh token and retry
        session = await client.session(SessionContext(
            instance_id=context.instance_id,
            user_id=context.user_id,
            project_id=context.project_id
        ))
        context.token = session.token
        response = await client.chat(context=context, request=request)
    except (ConnectionError, TimeoutError) as e:
        # Handle network-related errors
        logger.warning(f"Network error: {type(e).__name__}: {e}")
        # Use a fallback or simplified approach
        response = await client.chat(
            context=context,
            request=ChatRequest(query=query, use_openai=False)  # Try without OpenAI
        )
    except ServerError as e:
        # Handle server-side errors
        logger.error(f"Server error: {e}")
        # Return a generic response
        return {"error": "The service is experiencing issues. Please try again later."}
    except GPOClientException as e:
        # Handle any other client errors
        logger.error(f"Client error: {e}")
        return {"error": "An error occurred processing your request."}
    except Exception as e:
        # Handle unexpected errors
        logger.critical(f"Unexpected error: {e}", exc_info=True)
        return {"error": "An unexpected error occurred."}
    else:
        # This block runs if no exception was raised
        logger.info(f"Successfully received response for query: {query[:30]}...")
    finally:
        # This block always runs
        # Clean up resources, close connections, etc.
        await client.close_session()
    
    return response
```

### Context Manager Pattern

For automatic resource management:

```python
class GPOClientContextManager:
    """Context manager for GPO client to ensure proper cleanup."""
    
    def __init__(self, config=None):
        self.client = None
        self.config = config
    
    async def __aenter__(self):
        """Create and return a new client."""
        self.client = GPOCoreClient(config=self.config)
        return self.client
    
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        """Clean up client resources."""
        if self.client:
            await self.client.close_session()

# Usage:
async def main():
    async with GPOClientContextManager() as client:
        try:
            response = await client.chat(context=context, request=request)
            # Process response
        except GPOClientException as e:
            # Handle client errors
            print(f"Error: {e}")
```

### Error Bubbling vs. Handling

Choose the appropriate level to handle errors:

```python
# Low-level function: Bubble up errors for the caller to handle
async def get_tax_information(client, context, country):
    """Get tax information for a specific country."""
    request = ChatRequest(
        query=f"What are the corporate tax rates in {country}?",
        use_openai=True
    )
    
    # Let errors bubble up to the caller
    return await client.chat(context=context, request=request)

# High-level function: Handle errors at this level
async def display_tax_information(client, context, country):
    """Display tax information with error handling."""
    try:
        response = await get_tax_information(client, context, country)
        return {
            "success": True,
            "content": response.message_content,
            "sources": response.sources
        }
    except AuthenticationError:
        # Handle authentication issues
        return {"success": False, "error": "Authentication failed"}
    except GPOClientException as e:
        # Handle any client errors
        return {"success": False, "error": str(e)}
    except Exception as e:
        # Handle unexpected errors
        logger.error(f"Unexpected error in display_tax_information: {e}")
        return {"success": False, "error": "An unexpected error occurred"}
```

## Retry Mechanisms

### Basic Retry

Simple retry with a delay:

```python
async def chat_with_retry(client, context, request, max_retries=3, base_delay=1):
    """Send a chat request with retries on certain errors."""
    retries = 0
    last_exception = None
    
    while retries < max_retries:
        try:
            return await client.chat(context=context, request=request)
        except (ConnectionError, TimeoutError, ServiceUnavailableError) as e:
            retries += 1
            last_exception = e
            
            if retries >= max_retries:
                break
                
            # Exponential backoff with jitter
            delay = base_delay * (2 ** (retries - 1)) * (0.5 + random.random())
            logger.warning(f"Retry {retries}/{max_retries} after {delay:.2f}s due to {type(e).__name__}")
            await asyncio.sleep(delay)
    
    # If we've exhausted retries, re-raise the last exception
    logger.error(f"Failed after {retries} retries: {last_exception}")
    raise last_exception
```

### Using Tenacity

For more advanced retry handling, use the `tenacity` library:

```python
import tenacity
from tenacity import retry, stop_after_attempt, wait_exponential, retry_if_exception_type

# Define retryable exceptions
retryable_errors = (ConnectionError, TimeoutError, ServiceUnavailableError)

@retry(
    retry=retry_if_exception_type(retryable_errors),
    stop=stop_after_attempt(3),
    wait=wait_exponential(multiplier=1, min=1, max=10),
    before_sleep=tenacity.before_sleep_log(logger, logging.WARNING)
)
async def chat_with_tenacity(client, context, request):
    """Send a chat request with automatic retries using tenacity."""
    return await client.chat(context=context, request=request)
```

## Logging Errors

### Structured Error Logging

```python
import logging
import json
from datetime import datetime

def log_error(error, context=None):
    """Log an error with structured information."""
    error_data = {
        "timestamp": datetime.utcnow().isoformat(),
        "error_type": type(error).__name__,
        "message": str(error),
        "code": getattr(error, "code", None)
    }
    
    # Add context if provided
    if context:
        # Sanitize sensitive information
        safe_context = context.copy()
        if "token" in safe_context:
            safe_context["token"] = "****"
        error_data["context"] = safe_context
    
    # Add additional metadata from the error
    for attr in ["field", "attempts", "retry_after"]:
        if hasattr(error, attr):
            error_data[attr] = getattr(error, attr)
    
    logger.error(f"Error occurred: {json.dumps(error_data)}")
```

### Tagged Logging

```python
def tag_log(message, tags, level=logging.INFO):
    """Log a message with tags for better filtering."""
    if not isinstance(tags, dict):
        tags = {"tag": tags}
    
    # Format tags for structured logging
    tags_str = " ".join([f"[{k}:{v}]" for k, v in tags.items()])
    logger.log(level, f"{tags_str} {message}")

# Usage
try:
    response = await client.chat(context=context, request=request)
except AuthenticationError as e:
    tag_log(f"Authentication failed: {e}", 
            {"operation": "chat", "error": "auth"}, 
            level=logging.ERROR)
except ConnectionError as e:
    tag_log(f"Connection error: {e}", 
            {"operation": "chat", "error": "connection", "attempts": e.attempts}, 
            level=logging.WARNING)
```

## Error Recovery Strategies

### Use Fallbacks

```python
async def chat_with_fallbacks(client, context, request):
    """Chat with fallback strategies."""
    try:
        # Try primary approach
        return await client.chat(context=context, request=request)
    except (ConnectionError, TimeoutError, ServerError):
        # Fallback 1: Try without OpenAI
        try:
            request.use_openai = False
            return await client.chat(context=context, request=request)
        except GPOClientException:
            # Fallback 2: Use cached response if available
            cached_response = get_from_cache(request.query)
            if cached_response:
                return cached_response
            
            # Fallback 3: Return a generated response
            return {
                "message_content": "I apologize, but I'm unable to process your request at the moment. Please try again later.",
                "message_id": "fallback",
                "sources": []
            }
```

### Circuit Breaker Pattern

```python
class CircuitBreaker:
    """
    Circuit breaker pattern implementation for GPO client.
    
    States:
    - CLOSED: Normal operation, requests pass through
    - OPEN: Failure threshold exceeded, requests fail fast
    - HALF_OPEN: Testing if service is healthy again
    """
    
    CLOSED = "closed"
    OPEN = "open"
    HALF_OPEN = "half_open"
    
    def __init__(self, failure_threshold=5, recovery_timeout=30):
        self.state = self.CLOSED
        self.failure_count = 0
        self.failure_threshold = failure_threshold
        self.recovery_timeout = recovery_timeout
        self.last_failure_time = None
        self.lock = asyncio.Lock()
    
    async def execute(self, command, fallback=None):
        """
        Execute a command through the circuit breaker.
        
        Args:
            command: Async function to execute
            fallback: Fallback function if circuit is open or command fails
            
        Returns:
            Result of command or fallback
        """
        async with self.lock:
            if self.state == self.OPEN:
                # Check if recovery timeout has elapsed
                if self.last_failure_time and \
                   (datetime.now() - self.last_failure_time).total_seconds() > self.recovery_timeout:
                    self.state = self.HALF_OPEN
                    logger.info("Circuit switched from OPEN to HALF_OPEN")
                else:
                    # Circuit is open, fail fast
                    logger.warning("Circuit OPEN - failing fast")
                    return await fallback() if fallback else None
        
        try:
            result = await command()
            
            # If the command was successful and we were in HALF_OPEN,
            # reset the circuit breaker
            if self.state == self.HALF_OPEN:
                async with self.lock:
                    self.state = self.CLOSED
                    self.failure_count = 0
                    logger.info("Circuit switched from HALF_OPEN to CLOSED")
            
            return result
            
        except Exception as e:
            # Command failed
            async with self.lock:
                if self.state == self.CLOSED:
                    self.failure_count += 1
                    
                    if self.failure_count >= self.failure_threshold:
                        self.state = self.OPEN
                        self.last_failure_time = datetime.now()
                        logger.warning(f"Circuit switched from CLOSED to OPEN after {self.failure_count} failures")
                
                elif self.state == self.HALF_OPEN:
                    self.state = self.OPEN
                    self.last_failure_time = datetime.now()
                    logger.warning("Circuit switched from HALF_OPEN to OPEN due to failure")
            
            # Re-raise if no fallback
            if not fallback:
                raise
            
            return await fallback()

# Usage
breaker = CircuitBreaker(failure_threshold=3, recovery_timeout=60)

async def get_chat_response(client, context, request):
    async def command():
        return await client.chat(context=context, request=request)
    
    async def fallback():
        return {"message_content": "Service temporarily unavailable. Please try again later."}
    
    return await breaker.execute(command, fallback)
```

## Best Practices

### 1. Be Specific in Error Handling

Catch specific exceptions rather than general ones:

```python
# GOOD: Specific error handling
try:
    response = await client.chat(context=context, request=request)
except AuthenticationError as e:
    # Handle authentication errors specifically
    refresh_token()
except ConnectionError as e:
    # Handle connection errors specifically
    retry_with_backoff()
    
# AVOID: Overly broad exception handling
try:
    response = await client.chat(context=context, request=request)
except Exception as e:  # Too broad
    # Generic error handling loses valuable context
    print(f"Error occurred: {e}")
```

### 2. Use Contextual Information

Include relevant context when handling errors:

```python
try:
    response = await client.chat(context=context, request=request)
except GPOClientException as e:
    # Log with context for better debugging
    logger.error(
        f"Error during chat request: {e}", 
        extra={
            "query": request.query[:100] + "..." if len(request.query) > 100 else request.query,
            "session_id": context.session_id,
            "conversation_id": context.conversation_id,
            "error_code": getattr(e, "code", "unknown")
        }
    )
```

### 3. Fail Fast for Programming Errors

Don't catch errors that indicate programming mistakes:

```python
# Let TypeError, ValueError for programming errors propagate
# These should be fixed in the code, not handled at runtime
```

### 4. Graceful Degradation

Implement graceful degradation when services are impaired:

```python
async def get_answer(client, context, query):
    request = ChatRequest(query=query, use_openai=True)
    
    try:
        # Try with full capabilities
        return await client.chat(context=context, request=request)
    except (ServiceUnavailableError, TimeoutError):
        # Degrade gracefully: try without OpenAI integration
        logger.warning("Degrading service - disabling OpenAI")
        request.use_openai = False
        return await client.chat(context=context, request=request)
    except GPOClientException:
        # Further degradation: use basic response
        logger.error("Service severely degraded - using static response")
        return {"message_content": "I'm sorry, I cannot answer that right now."}
```

### 5. Monitor Error Rates

Track error rates to detect systemic issues:

```python
class ErrorMonitor:
    """Monitor error rates for circuit breaking."""
    
    def __init__(self, window_size=60, threshold=0.5):
        self.errors = deque(maxlen=window_size)
        self.requests = deque(maxlen=window_size)
        self.threshold = threshold
    
    def record_request(self, error=None):
        """Record a request and optional error."""
        now = time.time()
        self.requests.append(now)
        
        if error is not None:
            self.errors.append((now, error))
    
    def error_rate(self):
        """Calculate current error rate."""
        if not self.requests:
            return 0.0
        
        return len(self.errors) / len(self.requests)
    
    def should_circuit_break(self):
        """Check if error rate exceeds threshold."""
        return self.error_rate() >= self.threshold
```

### 6. User-Friendly Error Messages

Translate technical errors into user-friendly messages:

```python
def user_friendly_error_message(error):
    """Convert technical errors to user-friendly messages."""
    if isinstance(error, AuthenticationError):
        return "Your session has expired. Please log in again."
    elif isinstance(error, ConnectionError):
        return "Unable to connect to the service. Please check your internet connection."
    elif isinstance(error, TimeoutError):
        return "The request took too long to process. Please try a simpler question."
    elif isinstance(error, RateLimitError):
        return "You've made too many requests. Please wait a moment before trying again."
    elif isinstance(error, ServiceUnavailableError):
        return "The service is currently unavailable. Please try again later."
    else:
        return "An error occurred. Please try again later."
```

## Conclusion

Effective error handling is a critical aspect of working with the GPO Core API Python Client. By implementing the strategies described in this document, you can create robust applications that gracefully handle exceptional situations, provide clear feedback to users, and maintain operational reliability even in the face of errors.

For more information on related topics, see:
- [Security Best Practices](security.md) for information on secure error handling
- [Streaming Responses](streaming.md) for error handling in streaming contexts
- [API Reference](api_reference.md) for detailed information on all exceptions
- [Integration Guide](integration.md) for tips on integrating error handling with existing services