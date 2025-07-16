# Security Best Practices

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document provides guidance on implementing and maintaining security when working with the GPO Core API Python Client. Following these best practices will help protect sensitive information and ensure secure communication with the GPO API.

## Authentication Security

### Token Handling

The GPO Core API Python Client uses JWT tokens for authentication. Always follow these best practices when handling authentication tokens:

1. **Never Store Tokens in Code**:
   ```python
   # DON'T do this
   token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."  # Hardcoded token
   
   # DO this instead
   token = os.environ.get("GPO_API_TOKEN")  # From environment variable
   ```

2. **Minimize Token Exposure**:
   - Avoid printing tokens in logs
   - Don't include tokens in error reports
   - Clear tokens from memory when no longer needed

3. **Secure Token Storage**:
   - Use a secure credential store or vault service
   - For development, use `.env` files that are excluded from version control

4. **Token Lifetime Management**:
   ```python
   # Check token expiration before use
   from jose import jwt
   import time
   
   def is_token_valid(token, buffer_seconds=60):
       """Check if a token is valid with a buffer time."""
       try:
           claims = jwt.get_unverified_claims(token)
           expiration = claims.get("exp", 0)
           
           # Check if token is expired or about to expire
           return time.time() < (expiration - buffer_seconds)
       except Exception:
           return False
   ```

### Secure Configuration

1. **Environment Variables**:
   - Use environment variables for sensitive configuration
   - Consider using a tool like `python-dotenv` to manage environment variables

   ```python
   # .env file (do not commit to version control)
   GPO_API_BASE_URL=https://gpo-api.example.com
   GPO_API_TIMEOUT=30
   ```

   ```python
   # Load environment variables
   from dotenv import load_dotenv
   
   load_dotenv()  # Load variables from .env file
   ```

2. **Sensitive Data Handling**:
   - Mask sensitive data in logs and error reports
   - Use dedicated models for handling sensitive information

   ```python
   import logging

   def log_request(request):
       """Log request with sensitive data masked."""
       masked_request = request.copy()
       
       if "token" in masked_request:
           masked_request["token"] = "****"
           
       logging.info(f"Request: {masked_request}")
   ```

## Network Security

### TLS/SSL Verification

Always verify TLS/SSL certificates in production:

```python
# Default: SSL verification enabled
client = GPOCoreClient()

# ONLY for development/testing environments
from app.core.gpo_client.config import GPOConfig
dev_config = GPOConfig(verify_ssl=False)
dev_client = GPOCoreClient(config=dev_config)
```

### Request and Response Security

1. **Request Sanitization**:
   - Validate all inputs before sending them to the API
   - Sanitize potentially dangerous inputs

   ```python
   def sanitize_query(query):
       """Basic sanitization for query text."""
       # Remove control characters
       import re
       sanitized = re.sub(r'[\x00-\x1F\x7F]', '', query)
       return sanitized
   ```

2. **Response Validation**:
   - Validate responses from the API
   - Never trust raw responses without validation

   ```python
   async def validate_response(response):
       """Validate a response from the API."""
       # Check that required fields exist
       if not hasattr(response, "message_id"):
           raise ValueError("Missing required field: message_id")
           
       # Validate data formats
       if hasattr(response, "created_at"):
           # Ensure it's a valid datetime
           from datetime import datetime
           try:
               datetime.fromisoformat(response.created_at.replace("Z", "+00:00"))
           except (ValueError, AttributeError):
               raise ValueError("Invalid datetime format in created_at")
       
       return response
   ```

## Secure Coding Practices

### Input Validation

Always validate all inputs, especially those coming from users:

```python
def validate_chat_request(request):
    """Validate a chat request."""
    if not request.query or not isinstance(request.query, str):
        raise ValueError("Query must be a non-empty string")
        
    if len(request.query) > 2000:
        raise ValueError("Query exceeds maximum length (2000 characters)")
        
    # Validate other fields...
    return True
```

### Error Handling

Handle errors securely:

```python
try:
    result = await client.chat(context=context, request=request)
    return result
except AuthenticationError as e:
    # Log with limited info (no tokens or sensitive data)
    logger.error(f"Authentication error: {type(e).__name__}")
    raise
except Exception as e:
    # Avoid exposing internal details
    logger.error(f"Error in chat request: {type(e).__name__}")
    # Return a generic error to the caller
    return {"error": "An error occurred processing your request"}
```

### Rate Limiting

Implement rate limiting to prevent abuse:

```python
import time
import asyncio
from collections import deque

class RateLimiter:
    """Simple rate limiter."""
    
    def __init__(self, max_calls=10, time_window=60):
        """
        Initialize the rate limiter.
        
        Args:
            max_calls: Maximum calls in the time window
            time_window: Time window in seconds
        """
        self.max_calls = max_calls
        self.time_window = time_window
        self.calls = deque()
        self.lock = asyncio.Lock()
    
    async def acquire(self):
        """
        Acquire permission to make a call.
        
        Returns:
            True if the call is allowed, False if rate limited
        """
        now = time.time()
        
        async with self.lock:
            # Remove expired timestamps
            while self.calls and self.calls[0] <= now - self.time_window:
                self.calls.popleft()
            
            # Check if we're under the limit
            if len(self.calls) < self.max_calls:
                self.calls.append(now)
                return True
            
            return False
```

## Sensitive Data Handling

### Chat with Sensitive Data

Use the dedicated methods for handling sensitive data:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, SensitiveChatRequest

# Initialize client
client = GPOCoreClient()

# Create a session
session_context = SessionContext(
    instance_id="capital-edge-instance-001",
    user_id="user-123",
    project_id="project-456"
)
session = await client.session(session_context)

# Set up context for sensitive data
context = ChatContext(
    session_id=session.session_id,
    conversation_id="conversation-789",
    token=session.token
)

# Create a request with sensitive data
request = SensitiveChatRequest(
    query="Our company's Q1 revenue was $5.3M. What are the tax implications?",
    is_sensitive=True,
    sensitivity_markers=[
        {"type": "financial_data", "start": 24, "end": 29}
    ],
    data_handling_policy="secure_processing"
)

# Use the dedicated method for sensitive data
response = await client.chat_sensitive_data(context=context, request=request)
```

### Compliance Checks

Implement compliance checks for sensitive data:

```python
def check_for_sensitive_data(query):
    """Check if a query might contain sensitive data."""
    import re
    
    # Check for patterns that might indicate sensitive data
    patterns = {
        "credit_card": r'\b(?:\d{4}[-\s]?){3}\d{4}\b',  # Credit card pattern
        "ssn": r'\b\d{3}-\d{2}-\d{4}\b',  # US SSN pattern
        "email": r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b',  # Email pattern
        "financial": r'\$\d+(?:\.\d{1,2})?[MBK]?',  # Financial amounts
        "percentage": r'\d{1,3}%'  # Percentages
    }
    
    matches = {}
    for key, pattern in patterns.items():
        found = re.findall(pattern, query)
        if found:
            matches[key] = found
    
    return matches
```

### Data Minimization

Apply data minimization principles:

```python
def minimize_query_data(query, context):
    """Apply data minimization to a query."""
    
    # Only include necessary context
    minimized_context = {
        # Only include fields necessary for the query
        "topic": context.get("topic"),
        "region": context.get("region")
    }
    
    # Remove unnecessary details from query if possible
    # (In a real implementation, this would be more sophisticated)
    
    return query, minimized_context
```

## Security in Production Environments

### Logging Best Practices

1. **Structured Logging**:
   ```python
   import structlog
   
   logger = structlog.get_logger()
   
   # Log with structured data
   logger.info(
       "API request completed",
       request_id="req-123",
       method="chat",
       duration_ms=156,
       status="success"
   )
   ```

2. **Sensitive Data Redaction**:
   ```python
   import structlog
   from structlog.processors import JSONRenderer
   
   def redact_sensitive_data(_, __, event_dict):
       """Redact sensitive fields."""
       if "token" in event_dict:
           event_dict["token"] = "****"
       return event_dict
   
   structlog.configure(
       processors=[
           redact_sensitive_data,
           JSONRenderer()
       ]
   )
   ```

### Monitoring and Alerting

Set up monitoring and alerting for security events:

1. **Monitor for authentication failures**
2. **Set alerts for unusual usage patterns**
3. **Track API response times**
4. **Monitor for excessive rate limit hits**

Example integration with a monitoring service:

```python
import sentry_sdk

sentry_sdk.init(
    dsn="https://your-sentry-dsn",
    environment="production",
    traces_sample_rate=0.1
)

try:
    result = await client.chat(context=context, request=request)
    return result
except Exception as e:
    # Capture exception (automatically strips sensitive data)
    sentry_sdk.capture_exception(e)
    raise
```

### Audit Logging

Implement audit logging for sensitive operations:

```python
import logging
import json
from datetime import datetime

audit_logger = logging.getLogger("audit")
file_handler = logging.FileHandler("audit.log")
file_handler.setFormatter(logging.Formatter('%(message)s'))
audit_logger.addHandler(file_handler)
audit_logger.setLevel(logging.INFO)

def log_audit_event(event_type, user_id, action, resource_id=None, metadata=None):
    """Log an audit event."""
    audit_event = {
        "timestamp": datetime.utcnow().isoformat(),
        "event_type": event_type,
        "user_id": user_id,
        "action": action,
        "resource_id": resource_id,
        "metadata": metadata or {}
    }
    
    audit_logger.info(json.dumps(audit_event))
```

## Security Testing

### Regular Security Checks

Implement security checks in your automated testing:

```python
import unittest
from jose import jwt

class SecurityTests(unittest.TestCase):
    
    def test_token_validation(self):
        """Test that token validation works correctly."""
        from app.core.gpo_client.utils.token import validate_token
        
        # Test with valid token
        valid_token = create_test_token(expiry=3600)  # 1 hour expiry
        self.assertTrue(validate_token(valid_token))
        
        # Test with expired token
        expired_token = create_test_token(expiry=-3600)  # Expired 1 hour ago
        self.assertFalse(validate_token(expired_token))
    
    def test_ssl_verification(self):
        """Test that SSL verification is enforced."""
        from app.core.gpo_client.client import GPOCoreClient
        from app.core.gpo_client.config import GPOConfig
        
        # Default config should have SSL verification enabled
        config = GPOConfig()
        self.assertTrue(config.verify_ssl)
        
        # Test that invalid certs are rejected
        client = GPOCoreClient(config=config)
        with self.assertRaises(Exception):
            # This should fail if verify_ssl is working
            asyncio.run(client.session(SessionContext(
                instance_id="test",
                user_id="test",
                project_id="test"
            )))
```

For comprehensive security testing guidance including authentication testing, token security, SSL/TLS verification, input validation, sensitive data handling, and more, see the [Security Testing section](testing_guide.md#security-testing) in the Testing Guide.

### Dependency Security

1. **Regular dependency updates**:
   ```bash
   # Update dependencies
   pip install --upgrade -r requirements.txt
   
   # Check for security vulnerabilities
   safety check
   ```

2. **Dependency scanning in CI/CD**:
   ```yaml
   # Example GitHub workflow for dependency scanning
   name: "Security Scan"
   on:
     push:
       branches: [ main ]
     pull_request:
       branches: [ main ]
     
   jobs:
     security:
       runs-on: ubuntu-latest
       steps:
       - uses: actions/checkout@v2
       - name: Set up Python
         uses: actions/setup-python@v2
         with:
           python-version: '3.9'
       - name: Install dependencies
         run: |
           python -m pip install --upgrade pip
           pip install safety
       - name: Run security scan
         run: safety check
   ```

## Conclusion

Security is a critical aspect of working with any client library that handles sensitive information or interacts with APIs. By following these best practices, you can ensure that your use of the GPO Core API Python Client remains secure and protects sensitive data.

Remember that security is not a one-time effort but a continuous process. Regularly review and update your security practices as new versions of the client are released and as best practices evolve.

For more information, refer to:
- [Configuration Guide](configuration.md) for details on secure configuration
- [Error Handling Guide](error_handling.md) for more information on handling errors securely
- [API Reference](api_reference.md) for details on the sensitive data handling methods
- [Integration Guide](integration.md) for secure integration with existing services