# Troubleshooting

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document provides guidance for diagnosing and resolving common issues when using the GPO Core API Python Client.

## Table of Contents

- [Common Issues](#common-issues)
  - [Connection Problems](#connection-problems)
  - [Authentication Errors](#authentication-errors)
  - [Request Errors](#request-errors)
  - [Response Parsing Errors](#response-parsing-errors)
  - [Performance Issues](#performance-issues)
- [Logging and Debugging](#logging-and-debugging)
- [Error Messages](#error-messages)
- [Environment Issues](#environment-issues)
- [Support Resources](#support-resources)

## Common Issues

### Connection Problems

#### Issue: Unable to Connect to GPO Core API

**Symptoms:**
- `ConnectionError` exceptions
- Timeout errors
- "Cannot connect to host" messages

**Possible Causes:**
1. Incorrect API base URL
2. Network connectivity issues
3. Firewall or proxy blocking requests
4. API service is down

**Solutions:**
1. Verify the API base URL:
   ```python
   print(f"Using API URL: {client.config.base_url}")
   ```

2. Test basic connectivity:
   ```python
   import aiohttp
   import asyncio

   async def test_connectivity(url):
       try:
           async with aiohttp.ClientSession() as session:
               async with session.get(url, timeout=5) as response:
                   print(f"Connection successful: {response.status}")
       except Exception as e:
           print(f"Connection failed: {e}")

   asyncio.run(test_connectivity("https://gpo-core-api.example.com/health"))
   ```

3. Check if your network requires a proxy:
   ```python
   from app.core.gpo_client.client import GPOCoreClient
   from app.core.gpo_client.config import GPOConfig

   config = GPOConfig(
       base_url="https://gpo-core-api.example.com",
       proxy="http://your-proxy:8080"
   )
   client = GPOCoreClient(config=config)
   ```

4. Increase timeout values:
   ```python
   config = GPOConfig(
       base_url="https://gpo-core-api.example.com",
       timeout=60  # Increase from default
   )
   client = GPOCoreClient(config=config)
   ```

#### Issue: SSL Certificate Verification Errors

**Symptoms:**
- `SSLError` exceptions
- "SSL: CERTIFICATE_VERIFY_FAILED" messages

**Solutions:**
1. Ensure your CA certificates are up to date
2. If needed (for development only), disable SSL verification:
   ```python
   config = GPOConfig(
       base_url="https://gpo-core-api.example.com",
       verify_ssl=False  # Not recommended for production
   )
   client = GPOCoreClient(config=config)
   ```

### Authentication Errors

#### Issue: Session Creation Fails

**Symptoms:**
- `AuthenticationError` exceptions
- "Invalid credentials" or "Unauthorized" error messages

**Possible Causes:**
1. Invalid instance ID, user ID, or project ID
2. Authentication service is unavailable
3. Permission issues

**Solutions:**
1. Verify your session context parameters:
   ```python
   session_context = SessionContext(
       instance_id="capital-edge-instance-001",  # Verify this is correct
       user_id="user-123",  # Verify this is correct
       project_id="project-456"  # Verify this is correct
   )
   ```

2. Check if you're using the correct environment:
   ```python
   session_context = SessionContext(
       instance_id="capital-edge-instance-001",
       user_id="user-123",
       project_id="project-456",
       environment="development"  # Change as needed
   )
   ```

#### Issue: Token Expiration

**Symptoms:**
- Operations work initially but fail after some time
- `TokenExpiredError` exceptions

**Solutions:**
1. Enable automatic token renewal:
   ```python
   config = GPOConfig(
       base_url="https://gpo-core-api.example.com",
       auto_renew_token=True
   )
   client = GPOCoreClient(config=config)
   ```

2. Manually check token expiration before operations:
   ```python
   from jose import jwt
   import time

   def is_token_expired(token):
       try:
           claims = jwt.get_unverified_claims(token)
           exp_time = claims.get("exp", 0)
           return time.time() >= exp_time
       except:
           return True
   
   if is_token_expired(session.token):
       # Create a new session
       session = await client.session(session_context)
   ```

### Request Errors

#### Issue: Invalid Request Parameters

**Symptoms:**
- `InvalidRequestError` exceptions
- Specific validation error messages

**Solutions:**
1. Check model validation requirements:
   ```python
   from app.core.gpo_client.models import ChatRequest
   
   # This will raise validation errors if parameters are invalid
   request = ChatRequest(
       query="What are the tax implications?",
       use_openai=True
   )
   print("Request is valid")
   ```

2. Use proper model instantiation for all requests:
   ```python
   # Incorrect - using dict directly
   conversation = await client.start_conversation(
       context=chat_context,
       request={"title": "Tax Inquiry"}  # May cause issues
   )
   
   # Correct - using the proper model
   from app.core.gpo_client.models import ConversationRequest
   conversation_request = ConversationRequest(
       title="Tax Inquiry"
   )
   conversation = await client.start_conversation(
       context=chat_context, 
       request=conversation_request
   )
   ```

#### Issue: Request Rate Limiting

**Symptoms:**
- `RateLimitError` exceptions
- "Too many requests" error messages

**Solutions:**
1. Implement retry with backoff:
   ```python
   import asyncio
   from app.core.gpo_client.exceptions import RateLimitError
   
   async def send_with_rate_limit_handling(client, context, request):
       max_retries = 3
       retry_count = 0
       
       while retry_count < max_retries:
           try:
               return await client.chat(context=context, request=request)
           except RateLimitError as e:
               retry_count += 1
               wait_time = getattr(e, "retry_after", 30)
               print(f"Rate limited. Waiting {wait_time}s before retry {retry_count}")
               await asyncio.sleep(wait_time)
       
       raise Exception(f"Failed after {max_retries} retries due to rate limiting")
   ```

### Response Parsing Errors

#### Issue: Unexpected Response Format

**Symptoms:**
- `ValueError` or `TypeError` when processing responses
- "Missing required fields" errors

**Solutions:**
1. Add error handling around response processing:
   ```python
   try:
       response = await client.chat(context=chat_context, request=request)
       message = response.message_content
       
       # Safely access optional attributes
       sources = getattr(response, "sources", [])
       response_time = getattr(response, "response_time_ms", None)
   except Exception as e:
       print(f"Error processing response: {e}")
       # Handle the error appropriately
   ```

2. Validate response objects:
   ```python
   response = await client.chat(context=chat_context, request=request)
   
   # Check if the response has the expected structure
   if not hasattr(response, "message_content") or not response.message_content:
       print("Warning: Response is missing expected content")
   ```

### Performance Issues

#### Issue: Slow Response Times

**Symptoms:**
- Operations take longer than expected
- Occasional timeouts

**Solutions:**
1. Optimize your configuration:
   ```python
   config = GPOConfig(
       base_url="https://gpo-core-api.example.com",
       timeout=45,  # Adjust based on expected response times
       max_retries=2  # Fewer retries for faster failure
   )
   client = GPOCoreClient(config=config)
   ```

2. Use streaming for long responses:
   ```python
   from app.core.gpo_client.models import StreamingChatRequest
   
   request = StreamingChatRequest(
       query="Provide a comprehensive analysis...",
       use_openai=True,
       stream=True  # Enable streaming for better user experience
   )
   
   async for chunk in client.chat_stream(context=chat_context, request=request):
       # Process chunks as they arrive
       pass
   ```

## Logging and Debugging

### Enabling Verbose Logging

The GPO Core API Python Client uses Python's standard logging system. To enable more detailed logs:

```python
import logging

# Configure logging
logging.basicConfig(
    level=logging.DEBUG,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)

# Enable specific loggers
logging.getLogger('app.core.gpo_client').setLevel(logging.DEBUG)
logging.getLogger('app.core.gpo_client.http').setLevel(logging.DEBUG)
```

### Enabling Request Tracing

To log detailed information about requests and responses:

```python
config = GPOConfig(
    base_url="https://gpo-core-api.example.com",
    trace_requests=True  # Enable request tracing
)
client = GPOCoreClient(config=config)
```

### Creating a Log File

To save logs to a file for later analysis:

```python
import logging

# Configure file logging
logging.basicConfig(
    level=logging.DEBUG,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
    filename='gpo_core_client.log',  # Save logs to this file
    filemode='a'  # Append mode
)
```

## Error Messages

Here are explanations for common error messages and how to resolve them:

### "Invalid session context provided"

**Cause**: The session context is missing required fields or contains invalid values.

**Solution**: Ensure all required fields are provided in the session context:
```python
session_context = SessionContext(
    instance_id="capital-edge-instance-001",  # Required
    user_id="user-123",  # Required
    project_id="project-456"  # Required
)
```

### "Authentication failed: Invalid token"

**Cause**: The token provided in the context is invalid or has been tampered with.

**Solution**: Create a new session to get a fresh token:
```python
session = await client.session(session_context)
token = session.token  # This is a valid token
```

### "Authentication failed: Token expired"

**Cause**: The token has expired.

**Solution**: Enable automatic token renewal or manually renew the token:
```python
config = GPOConfig(
    base_url="https://gpo-core-api.example.com",
    auto_renew_token=True
)
client = GPOCoreClient(config=config)
```

### "Connection error: Failed to establish connection"

**Cause**: Network connectivity issues or incorrect base URL.

**Solution**: Verify the base URL and your network connectivity:
```python
config = GPOConfig(
    base_url="https://correct-gpo-core-api-url.example.com"  # Verify this URL
)
client = GPOCoreClient(config=config)
```

### "Request timeout after 30 seconds"

**Cause**: The API took longer than the configured timeout to respond.

**Solution**: Increase the timeout value:
```python
config = GPOConfig(
    base_url="https://gpo-core-api.example.com",
    timeout=60  # Increase timeout to 60 seconds
)
client = GPOCoreClient(config=config)
```

### "Rate limit exceeded. Try again in X seconds"

**Cause**: You've sent too many requests in a short period.

**Solution**: Implement rate limit handling and backoff:
```python
except RateLimitError as e:
    retry_after = getattr(e, "retry_after", 30)
    print(f"Rate limited. Waiting {retry_after} seconds")
    await asyncio.sleep(retry_after)
    # Retry the request
```

## Environment Issues

### Working Behind a Corporate Proxy

If you're working in a corporate environment with a proxy:

```python
config = GPOConfig(
    base_url="https://gpo-core-api.example.com",
    proxy="http://your-corporate-proxy:8080"
)
client = GPOCoreClient(config=config)
```

### Environment Variable Configuration

Ensure environment variables are set correctly:

```bash
# Windows
set GPO_API_BASE_URL=https://gpo-core-api.example.com
set GPO_API_TIMEOUT=30

# Linux/macOS
export GPO_API_BASE_URL=https://gpo-core-api.example.com
export GPO_API_TIMEOUT=30
```

Then in your code:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.config import GPOConfig

# Config will be loaded from environment variables
config = GPOConfig.from_env()
client = GPOCoreClient(config=config)
```

### Dependency Issues

If you experience dependency-related errors:

1. Ensure all dependencies are installed:
   ```bash
   pip install -r requirements.txt
   ```

2. If you encounter conflicts, try creating a virtual environment:
   ```bash
   python -m venv gpo_venv
   source gpo_venv/bin/activate  # On Windows: gpo_venv\Scripts\activate
   pip install gpo-core-client
   ```

## Support Resources

If you continue to experience issues:

1. Check the [API Reference](api_reference.md) for detailed information about methods and parameters
2. Review the [Examples](examples.md) for complete usage examples
3. For internal users, contact the Capital Edge support team at `capital-edge-support@example.com`

## Diagnostic Scripts

### Basic Connectivity Test

```python
import aiohttp
import asyncio
import sys

async def test_gpo_connectivity(base_url):
    print(f"Testing connectivity to {base_url}...")
    try:
        async with aiohttp.ClientSession() as session:
            async with session.get(f"{base_url}/health", timeout=10) as response:
                if response.status == 200:
                    print(f"✅ Connection successful! Status: {response.status}")
                    return True
                else:
                    print(f"❌ Connection failed! Status: {response.status}")
                    return False
    except Exception as e:
        print(f"❌ Connection error: {e}")
        return False

if __name__ == "__main__":
    url = sys.argv[1] if len(sys.argv) > 1 else "https://gpo-core-api.example.com"
    asyncio.run(test_gpo_connectivity(url))
```

### Full Client Diagnostics

```python
import asyncio
import logging
import sys
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest
from app.core.gpo_client.config import GPOConfig

# Setup logging
logging.basicConfig(
    level=logging.DEBUG,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)

async def run_diagnostics(base_url):
    print(f"Running diagnostics for GPO Core API Client using {base_url}")
    
    try:
        # Create client with verbose logging
        config = GPOConfig(
            base_url=base_url,
            timeout=30,
            trace_requests=True
        )
        
        client = GPOCoreClient(config=config)
        print("✅ Client initialized successfully")
        
        # Test session creation
        print("\nTesting session creation...")
        session_context = SessionContext(
            instance_id="capital-edge-instance-001",
            user_id="diagnostic-user",
            project_id="diagnostic-project"
        )
        
        session = await client.session(session_context)
        print(f"✅ Session created successfully: {session.session_id}")
        
        # Test conversation creation
        print("\nTesting conversation creation...")
        chat_context = ChatContext(
            session_id=session.session_id,
            token=session.token
        )
        
        conversation = await client.start_conversation(
            context=chat_context,
            request={"title": "Diagnostic Test"}
        )
        print(f"✅ Conversation created successfully: {conversation.conversation_id}")
        
        # Test chat
        print("\nTesting chat functionality...")
        chat_context = ChatContext(
            session_id=session.session_id,
            conversation_id=conversation.conversation_id,
            token=session.token
        )
        
        request = ChatRequest(
            query="Send a brief test response",
            use_openai=True
        )
        
        response = await client.chat(context=chat_context, request=request)
        print(f"✅ Chat response received successfully")
        print(f"Response: {response.message_content[:100]}...")
        
        print("\nAll diagnostics passed successfully!")
        return True
        
    except Exception as e:
        print(f"❌ Diagnostic failed: {e}")
        return False

if __name__ == "__main__":
    url = sys.argv[1] if len(sys.argv) > 1 else "https://gpo-core-api.example.com"
    asyncio.run(run_diagnostics(url))
```