# Authentication

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

The GPO Core Python Client implements secure authentication to interact with the GPO Core API. This document describes the authentication mechanisms and best practices for securely using the client.

## Authentication Flow

The client uses JWT (JSON Web Tokens) for authentication with the following flow:

1. **Session Creation**: Initial authentication occurs during session creation with the GPO Core API.
2. **Token Acquisition**: A JWT token is received from the API as part of the session response.
3. **Token Storage**: The token is stored in the context objects for subsequent requests.
4. **Token Validation**: The token is validated before each request to ensure it hasn't expired.
5. **Token Renewal**: If the token is expired, automatic renewal can be configured.

## Token Structure

The JWT tokens used by GPO Core typically follow this structure:

```
header.payload.signature
```

Where:
- **Header**: Contains the token type and signing algorithm.
- **Payload**: Contains claims about the entity (typically the user) and other metadata.
- **Signature**: Ensures the token hasn't been altered.

## Authentication Handlers

The GPO Core Python Client provides several classes for handling authentication:

### AuthHandler

The main class for authentication-related operations:

```python
class AuthHandler:
    def __init__(self, config):
        self.config = config
        self.key_manager = RSAKeyManager(config)
        
    def validate_token(self, token, input_data):
        # Validates a JWT token
        pass
        
    def extract_token_claims(self, token):
        # Extracts and validates claims from a token
        pass
        
    def prepare_auth_header(self, token):
        # Creates authorization headers from a token
        return {"Authorization": f"Bearer {token}"}
```

### TokenValidator

Handles validating JWT tokens and their claims:

```python
class TokenValidator:
    def __init__(self, rsa_key_manager):
        self.rsa_key_manager = rsa_key_manager
        
    def validate(self, token, options=None):
        # Validates a token with the given options
        pass
        
    def decode_token(self, token):
        # Decodes a token without validating
        pass
```

### RSAKeyManager

Manages the RSA public keys used for token verification:

```python
class RSAKeyManager:
    def __init__(self, config):
        self.config = config
        self._public_keys = {}
        
    async def get_public_key(self, kid):
        # Gets the public key for a given key ID
        pass
        
    async def refresh_public_keys(self):
        # Refreshes the cached public keys
        pass
```

## Using Authentication in Requests

The client handles authentication automatically when provided with a valid token. Here's how it works:

1. **Session Creation**: First, create a session to get a valid token:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext

async def get_authenticated_session():
    client = GPOCoreClient()
    
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    
    session_response = await client.session(session_context)
    
    # The token is in the session_response
    token = session_response.token
    
    return session_response
```

2. **Using the Token for Subsequent Requests**:

```python
from app.core.gpo_client.models import ChatContext, ChatRequest

async def send_authenticated_chat(session_response, conversation_id):
    client = GPOCoreClient()
    
    # Create context with session ID, conversation ID, and token
    context = ChatContext(
        session_id=session_response.session_id,
        conversation_id=conversation_id,
        token=session_response.token
    )
    
    request = ChatRequest(
        query="What are the tax implications of a merger?",
        use_openai=True
    )
    
    # The token will be used for authentication
    response = await client.chat(context=context, request=request)
    
    return response
```

## Token Expiration and Renewal

The GPO Core API tokens have an expiration time. The client can be configured to handle token expiration in several ways:

### 1. Manual Token Checking

You can check token expiration manually:

```python
from jose import jwt
import time

def is_token_expired(token):
    try:
        claims = jwt.get_unverified_claims(token)
        expiration = claims.get('exp', 0)
        return time.time() >= expiration
    except:
        return True
        
if is_token_expired(token):
    # Create a new session to get a fresh token
    session_response = await client.session(session_context)
    token = session_response.token
```

### 2. Automatic Token Renewal

The client can be configured to automatically renew expired tokens:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.config import GPOConfig

config = GPOConfig(
    base_url="https://gpo-api.example.com",
    auto_renew_token=True  # Enable automatic token renewal
)

client = GPOCoreClient(config=config)
```

## Security Best Practices

When using authentication with the GPO Core Python Client:

1. **Don't Hardcode Tokens**: Never hardcode tokens in your application code.

2. **Secure Token Storage**: Store tokens securely, avoiding plain text files.

3. **Environment Separation**: Use different credentials for development, testing, and production.

4. **Least Privilege**: Always use the minimal permissions required.

5. **Token Hygiene**: Regularly rotate keys and tokens.

6. **Error Handling**: Properly handle authentication errors to avoid exposing sensitive information.

```python
try:
    response = await client.chat(context=context, request=request)
except AuthenticationError as e:
    # Handle authentication errors properly
    logger.warning("Authentication failed: %s", e)
    # Don't expose the actual error in user-facing responses
    return {"error": "Authentication error occurred"}
```

7. **Logging**: Be careful not to log sensitive authentication information.

```python
# BAD:
logger.info(f"Using token: {token}")

# GOOD:
logger.info(f"Authentication performed for user: {context.user_id}")
```

## Authentication for Integration

When integrating the client with other Capital Edge services, consider implementing a centralized authentication service that can:

1. Manage tokens securely
2. Handle token renewal
3. Apply appropriate security policies

See the [Integration Guide](integration.md) for more details on integrating authentication with existing services.