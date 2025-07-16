# Testing Guide

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This guide provides detailed information on how to test applications that use the GPO Core Python Client. Thorough testing is essential to ensure your integration works correctly across various scenarios.

## Table of Contents

- [Introduction](#introduction)
- [Test Prerequisites](#test-prerequisites)
- [Unit Testing](#unit-testing)
- [Integration Testing](#integration-testing)
- [Mock Testing](#mock-testing)
- [End-to-End Testing](#end-to-end-testing)
- [Test Fixtures](#test-fixtures)
- [Testing Async Code](#testing-async-code)
- [Testing Edge Cases](#testing-edge-cases)
- [Security Testing](#security-testing)
- [Continuous Integration](#continuous-integration)
- [Performance Testing](#performance-testing)

## Introduction

Testing applications that use the GPO Core Python Client involves verifying several aspects:

1. **Authentication and session handling** - Test the client's ability to authenticate and maintain sessions.
2. **Request formation and validation** - Verify that requests to the GPO Core API are correctly formed and validated.
3. **Response processing** - Test that responses from the GPO Core API are correctly parsed and processed.
4. **Error handling** - Verify that errors are properly caught, handled, and reported.
5. **Performance and reliability** - Test the client under various load conditions to ensure stability.

This guide covers approaches for comprehensively testing these aspects.

## Test Prerequisites

Before beginning testing, make sure you have:

- A test environment configured with the GPO Core Python Client installed
- Test API credentials for GPO Core services
- Appropriate testing frameworks (pytest is recommended)
- Mock libraries (pytest-mock or unittest.mock)
- An understanding of asynchronous testing concepts (for testing async functions)

### Setting Up Test Environment

```python
# requirements-test.txt
pytest==7.3.1
pytest-asyncio==0.21.0
pytest-mock==3.10.0
aioresponses==0.7.4
coverage==7.2.7
```

Install the test dependencies:

```bash
pip install -r requirements-test.txt
```

## Unit Testing

Unit tests focus on testing individual components in isolation.

### Example Unit Test for ChatRequest Validation

```python
import pytest
from app.core.gpo_client.models import ChatRequest
from app.core.gpo_client.exceptions import ValidationError

def test_chat_request_validation():
    """Test that ChatRequest validates correctly."""
    # Valid request
    valid_request = ChatRequest(query="What is the capital of France?")
    assert valid_request.query == "What is the capital of France?"
    
    # Invalid request - empty query
    with pytest.raises(ValidationError) as excinfo:
        ChatRequest(query="")
    assert "query" in str(excinfo.value)
    
    # Invalid request - query too long
    with pytest.raises(ValidationError) as excinfo:
        ChatRequest(query="x" * 10001)  # Assuming 10,000 is the max length
    assert "query" in str(excinfo.value)
    assert "length" in str(excinfo.value)
```

### Testing Client Configuration

```python
def test_client_configuration():
    """Test that client configuration works correctly."""
    from app.core.gpo_client.client import GPOCoreClient
    from app.core.gpo_client.config import ClientConfig
    
    # Test default configuration
    client = GPOCoreClient()
    assert client.config.base_url == "https://api.gpo.example"
    assert client.config.timeout == 30
    
    # Test custom configuration
    custom_config = ClientConfig(
        base_url="https://custom-api.gpo.example",
        timeout=60,
        max_retries=5
    )
    client = GPOCoreClient(config=custom_config)
    assert client.config.base_url == "https://custom-api.gpo.example"
    assert client.config.timeout == 60
    assert client.config.max_retries == 5
```

## Integration Testing

Integration tests verify that the GPO Core Python Client correctly interacts with the GPO Core API.

### Setting Up Test Fixtures

```python
import pytest
import os
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

@pytest.fixture
def client():
    """Create a GPO Core Client for testing."""
    return GPOCoreClient()

@pytest.fixture
async def authenticated_context(client):
    """Create an authenticated context for testing."""
    # Get credentials from environment variables
    session_context = SessionContext(
        instance_id=os.environ.get("TEST_INSTANCE_ID"),
        user_id=os.environ.get("TEST_USER_ID"),
        project_id=os.environ.get("TEST_PROJECT_ID")
    )
    
    # Get an authenticated session
    session = await client.session(session_context)
    
    # Return a chat context with the session token
    chat_context = ChatContext(
        instance_id=session_context.instance_id,
        user_id=session_context.user_id,
        project_id=session_context.project_id,
        token=session.token
    )
    
    return chat_context
```

### Example Integration Test

```python
import pytest

@pytest.mark.asyncio
async def test_chat_integration(client, authenticated_context):
    """Test that the chat endpoint works correctly."""
    request = ChatRequest(query="What is the GPO Core Client?")
    
    response = await client.chat(
        context=authenticated_context,
        request=request
    )
    
    assert response is not None
    assert response.message_id is not None
    assert response.message_content is not None
    assert "GPO Core Client" in response.message_content
```

## Mock Testing

Mock tests use fake implementations to simulate the GPO Core API, allowing testing without making actual API calls.

### Using aioresponses for Mocking HTTP Responses

```python
import pytest
import json
from aioresponses import aioresponses
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest

@pytest.mark.asyncio
async def test_chat_with_mock():
    """Test chat functionality with mocked API responses."""
    client = GPOCoreClient()
    
    # Create a mock session context and chat context
    session_context = SessionContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project"
    )
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="mock-token"
    )
    
    # Create a chat request
    request = ChatRequest(query="What is the GPO Core Client?")
    
    # Mock the API responses
    with aioresponses() as m:
        # Mock the session endpoint
        m.post(
            f"{client.config.base_url}/api/session",
            status=200,
            payload={
                "token": "mock-token",
                "expires_at": "2023-12-31T23:59:59Z"
            }
        )
        
        # Mock the chat endpoint
        m.post(
            f"{client.config.base_url}/api/chat",
            status=200,
            payload={
                "message_id": "mock-message-id",
                "message_content": "The GPO Core Client is a Python library for interacting with the GPO Core API.",
                "sources": [
                    {
                        "source_id": "source-1",
                        "title": "GPO Core Client Documentation",
                        "url": "https://docs.gpo.example"
                    }
                ]
            }
        )
        
        # Test session creation
        session = await client.session(session_context)
        assert session.token == "mock-token"
        
        # Test chat functionality
        response = await client.chat(context=chat_context, request=request)
        assert response.message_id == "mock-message-id"
        assert "GPO Core Client is a Python library" in response.message_content
        assert len(response.sources) == 1
        assert response.sources[0].title == "GPO Core Client Documentation"
```

### Mocking Error Responses

```python
@pytest.mark.asyncio
async def test_chat_with_error_responses():
    """Test handling of error responses."""
    client = GPOCoreClient()
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="mock-token"
    )
    
    request = ChatRequest(query="What is the GPO Core Client?")
    
    with aioresponses() as m:
        # Mock a 401 Unauthorized response
        m.post(
            f"{client.config.base_url}/api/chat",
            status=401,
            payload={
                "error": "Unauthorized",
                "message": "Invalid or expired token"
            }
        )
        
        # Test that the client correctly raises an AuthenticationError
        from app.core.gpo_client.exceptions import AuthenticationError
        with pytest.raises(AuthenticationError) as excinfo:
            await client.chat(context=chat_context, request=request)
        
        assert "Invalid or expired token" in str(excinfo.value)
        
        # Mock a 429 Too Many Requests response
        m.post(
            f"{client.config.base_url}/api/chat",
            status=429,
            payload={
                "error": "RateLimitExceeded",
                "message": "Rate limit exceeded",
                "retry_after": 30
            },
            headers={"Retry-After": "30"}
        )
        
        # Test that the client correctly raises a RateLimitError
        from app.core.gpo_client.exceptions import RateLimitError
        with pytest.raises(RateLimitError) as excinfo:
            await client.chat(context=chat_context, request=request)
        
        assert "Rate limit exceeded" in str(excinfo.value)
        assert excinfo.value.retry_after == 30
```

## End-to-End Testing

End-to-end tests verify that the entire system works correctly from the client to the GPO Core API and back.

### Example End-to-End Test

```python
import pytest
import os

@pytest.mark.asyncio
@pytest.mark.e2e  # Mark as end-to-end test so it can be skipped in CI
async def test_complete_workflow():
    """Test a complete workflow from session creation to chat."""
    from app.core.gpo_client.client import GPOCoreClient
    from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest
    
    # Create a client with real API credentials
    client = GPOCoreClient()
    
    # Step 1: Create a session
    session_context = SessionContext(
        instance_id=os.environ.get("PROD_INSTANCE_ID"),
        user_id=os.environ.get("PROD_USER_ID"),
        project_id=os.environ.get("PROD_PROJECT_ID")
    )
    
    session = await client.session(session_context)
    assert session.token is not None
    
    # Step 2: Create a chat context
    chat_context = ChatContext(
        instance_id=session_context.instance_id,
        user_id=session_context.user_id,
        project_id=session_context.project_id,
        token=session.token,
        conversation_id="test-conversation"
    )
    
    # Step 3: Send a chat message
    request = ChatRequest(
        query="What services does GPO Core provide?",
        use_openai=True
    )
    
    response = await client.chat(context=chat_context, request=request)
    assert response.message_id is not None
    assert len(response.message_content) > 0
    
    # Step 4: Send a follow-up message in the same conversation
    follow_up_request = ChatRequest(
        query="Can you provide more details?",
        use_openai=True
    )
    
    follow_up_response = await client.chat(context=chat_context, request=follow_up_request)
    assert follow_up_response.message_id is not None
    assert follow_up_response.message_id != response.message_id
    assert len(follow_up_response.message_content) > 0
```

## Test Fixtures

Here's a more comprehensive set of fixtures for testing:

```python
# conftest.py
import pytest
import os
import json
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext, ChatContext, ChatRequest
from app.core.gpo_client.config import ClientConfig

@pytest.fixture
def client_config():
    """Create a test client configuration."""
    return ClientConfig(
        base_url=os.environ.get("GPO_API_URL", "https://api.gpo.example"),
        timeout=10,
        max_retries=2
    )

@pytest.fixture
def client(client_config):
    """Create a GPO Core Client with the test configuration."""
    return GPOCoreClient(config=client_config)

@pytest.fixture
def session_context():
    """Create a session context for testing."""
    return SessionContext(
        instance_id=os.environ.get("TEST_INSTANCE_ID", "test-instance"),
        user_id=os.environ.get("TEST_USER_ID", "test-user"),
        project_id=os.environ.get("TEST_PROJECT_ID", "test-project")
    )

@pytest.fixture
def mock_session():
    """Create a mock session response."""
    return {
        "token": "mock-token-value",
        "expires_at": "2099-12-31T23:59:59Z"
    }

@pytest.fixture
def mock_chat_response():
    """Create a mock chat response."""
    return {
        "message_id": "mock-message-id",
        "message_content": "This is a mock response to your query.",
        "sources": [
            {
                "source_id": "source-1",
                "title": "Mock Source",
                "url": "https://example.com/source"
            }
        ]
    }

@pytest.fixture
def sample_test_data():
    """Load sample test data from JSON file."""
    with open("tests/data/sample_test_data.json", "r") as f:
        return json.load(f)
```

## Testing Async Code

Testing asynchronous code requires special handling.

### Using pytest-asyncio

```python
# test_async.py
import pytest
import asyncio

@pytest.mark.asyncio
async def test_async_session(client, session_context, mocker):
    """Test the async session creation."""
    # Mock the client's _make_request method
    mocker.patch.object(
        client, 
        "_make_request",
        return_value={"token": "test-token", "expires_at": "2099-12-31T23:59:59Z"}
    )
    
    # Call the session method
    session = await client.session(session_context)
    
    # Verify the result
    assert session.token == "test-token"
    
    # Verify the _make_request call
    client._make_request.assert_called_once()
    args, kwargs = client._make_request.call_args
    assert args[0] == "POST"
    assert args[1] == "/api/session"
    assert kwargs["json"]["instance_id"] == session_context.instance_id
```

### Testing Timeouts

```python
@pytest.mark.asyncio
async def test_timeout_handling(client, mocker):
    """Test handling of timeouts."""
    # Mock a request that times out
    async def mock_timeout(*args, **kwargs):
        await asyncio.sleep(0.1)
        raise asyncio.TimeoutError("Request timed out")
        
    mocker.patch.object(client._session, "request", side_effect=mock_timeout)
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="test-token"
    )
    
    request = ChatRequest(query="What is the GPO Core Client?")
    
    # Test that the client correctly raises a TimeoutError
    from app.core.gpo_client.exceptions import TimeoutError
    with pytest.raises(TimeoutError) as excinfo:
        await client.chat(context=chat_context, request=request)
    
    assert "Request timed out" in str(excinfo.value)
```

## Testing Edge Cases

It's important to test edge cases to ensure the client behaves correctly in all situations.

### Empty and Boundary Values

```python
@pytest.mark.asyncio
async def test_empty_and_boundary_values(client, mocker):
    """Test handling of empty and boundary values."""
    # Mock _make_request to return a successful response
    mocker.patch.object(
        client, 
        "_make_request",
        return_value={"message_id": "test", "message_content": "test", "sources": []}
    )
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="test-token"
    )
    
    # Test with minimum length query
    min_request = ChatRequest(query="A")
    await client.chat(context=chat_context, request=min_request)
    
    # Test with maximum length query (assuming 10,000 is max)
    max_request = ChatRequest(query="A" * 10000)
    await client.chat(context=chat_context, request=max_request)
    
    # Test with empty sources in response
    empty_sources_response = {"message_id": "test", "message_content": "test", "sources": []}
    mocker.patch.object(client, "_make_request", return_value=empty_sources_response)
    response = await client.chat(context=chat_context, request=min_request)
    assert len(response.sources) == 0
```

### Testing Error Handling for Various HTTP Status Codes

```python
@pytest.mark.asyncio
async def test_http_status_codes(client, mocker):
    """Test handling of various HTTP status codes."""
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="test-token"
    )
    
    request = ChatRequest(query="Test query")
    
    # Test 400 Bad Request
    class MockResponse400:
        status = 400
        
        async def json(self):
            return {
                "error": "BadRequest",
                "message": "Invalid request format",
                "field": "query"
            }
            
    async def mock_request_400(*args, **kwargs):
        return MockResponse400()
        
    mocker.patch.object(client._session, "request", side_effect=mock_request_400)
    
    from app.core.gpo_client.exceptions import InvalidRequestError
    with pytest.raises(InvalidRequestError) as excinfo:
        await client.chat(context=chat_context, request=request)
    
    assert "Invalid request format" in str(excinfo.value)
    assert excinfo.value.field == "query"
    
    # Test 404 Not Found
    class MockResponse404:
        status = 404
        
        async def json(self):
            return {"error": "NotFound", "message": "Endpoint not found"}
            
    async def mock_request_404(*args, **kwargs):
        return MockResponse404()
        
    mocker.patch.object(client._session, "request", side_effect=mock_request_404)
    
    from app.core.gpo_client.exceptions import ClientError
    with pytest.raises(ClientError) as excinfo:
        await client.chat(context=chat_context, request=request)
    
    assert "Endpoint not found" in str(excinfo.value)
    
    # Test 500 Internal Server Error
    class MockResponse500:
        status = 500
        
        async def json(self):
            return {"error": "InternalServerError", "message": "Internal server error occurred"}
            
    async def mock_request_500(*args, **kwargs):
        return MockResponse500()
        
    mocker.patch.object(client._session, "request", side_effect=mock_request_500)
    
    from app.core.gpo_client.exceptions import ServerError
    with pytest.raises(ServerError) as excinfo:
        await client.chat(context=chat_context, request=request)
    
    assert "Internal server error occurred" in str(excinfo.value)
```

## Security Testing

Security testing is essential for applications that interact with APIs handling sensitive information. This section covers how to test security aspects of the GPO Core Python Client implementation.

### Authentication Testing

Test authentication mechanisms to ensure they work correctly and securely:

```python
import pytest
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import SessionContext
from app.core.gpo_client.exceptions import AuthenticationError

@pytest.mark.asyncio
async def test_authentication_failure_handling():
    """Test that authentication failures are handled correctly."""
    client = GPOCoreClient()
    
    # Test with invalid credentials
    invalid_context = SessionContext(
        instance_id="invalid-instance",
        user_id="invalid-user",
        project_id="invalid-project"
    )
    
    # Should raise AuthenticationError
    with pytest.raises(AuthenticationError) as excinfo:
        await client.session(invalid_context)
    
    assert "Authentication failed" in str(excinfo.value)
```

### Token Security Testing

Test token handling and validation to ensure tokens are used securely:

```python
@pytest.mark.asyncio
async def test_token_validation(mocker):
    """Test that tokens are validated correctly."""
    from app.core.gpo_client.auth import AuthHandler
    
    auth_handler = AuthHandler()
    
    # Mock token validation
    mocker.patch.object(auth_handler, "validate_token")
    
    # Valid token
    auth_handler.validate_token("valid-token")
    assert auth_handler.validate_token.called
    
    # Test token expiration check
    expired_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiZXhwIjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
    assert auth_handler.is_token_expiring_soon(expired_token) is True

    # Test token extraction
    mocker.patch.object(auth_handler, "extract_token_claims", return_value={"exp": 9999999999})
    assert auth_handler.is_token_expiring_soon("valid-token", 300) is False
```

### SSL/TLS Security Testing

Test SSL verification settings and proper handling of certificates:

```python
def test_ssl_verification_settings():
    """Test SSL verification settings in the client."""
    from app.core.gpo_client.client import GPOCoreClient
    from app.core.gpo_client.config import GPOConfig
    
    # Default should have SSL verification enabled
    default_config = GPOConfig()
    assert default_config.verify_ssl is True
    
    # Test with SSL verification disabled
    insecure_config = GPOConfig(verify_ssl=False)
    insecure_client = GPOCoreClient(config=insecure_config)
    
    # Check that warning is logged when SSL verification is disabled
    # This requires capturing logs and checking them
    import logging
    with caplog.at_level(logging.WARNING):
        # Perform an action that would trigger the warning
        insecure_client.http_client
        
    assert "SSL certificate verification is disabled" in caplog.text
```

### Input Validation Testing

Test that user inputs are properly validated to prevent injection attacks:

```python
@pytest.mark.asyncio
async def test_input_validation(client, mocker):
    """Test that inputs are properly validated."""
    # Mock _make_request to avoid actual API calls
    mocker.patch.object(client, "_make_request", return_value={})
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="test-token"
    )
    
    # Test with potential SQL injection
    sql_injection = "SELECT * FROM users; --"
    request = ChatRequest(query=sql_injection)
    
    # Request should be allowed but sanitized
    # The important part is the client doesn't crash and handles it properly
    await client.chat(context=chat_context, request=request)
    
    # Verify the request was made with the sanitized query
    args, kwargs = client._make_request.call_args
    assert kwargs["json"]["query"] == sql_injection
    
    # Test with potential XSS attack
    xss_attack = "<script>alert('XSS')</script>"
    request = ChatRequest(query=xss_attack)
    await client.chat(context=chat_context, request=request)
    
    # Verify the request was made with the sanitized query
    args, kwargs = client._make_request.call_args
    assert kwargs["json"]["query"] == xss_attack
```

### Sensitive Data Handling Testing

Test that sensitive data is handled securely:

```python
@pytest.mark.asyncio
async def test_sensitive_data_handling(client, mocker):
    """Test that sensitive data is handled securely."""
    # Mock _make_request to avoid actual API calls
    mocker.patch.object(client, "_make_request", return_value={})
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="test-token"
    )
    
    # Test with sensitive data
    sensitive_query = "My SSN is 123-45-6789 and my credit card is 4111-1111-1111-1111"
    sensitive_request = ChatSensitiveRequest(
        query=sensitive_query,
        is_sensitive=True,
        sensitivity_markers=[
            {"type": "ssn", "start": 11, "end": 21},
            {"type": "credit_card", "start": 42, "end": 61}
        ]
    )
    
    # Should use the sensitive chat endpoint
    await client.sensitive_chat(context=chat_context, request=sensitive_request)
    
    # Verify the request was made to the correct endpoint
    args, kwargs = client._make_request.call_args
    assert "sensitive" in args[1]
    assert kwargs["json"]["is_sensitive"] is True
    assert len(kwargs["json"]["sensitivity_markers"]) == 2
```

### Error Handling Security Testing

Test that errors are handled securely without leaking sensitive information:

```python
@pytest.mark.asyncio
async def test_secure_error_handling(client, mocker):
    """Test that errors are handled securely without information leakage."""
    # Create a mock error response that might contain sensitive details
    class MockErrorResponse:
        status = 500
        
        async def json(self):
            return {
                "error": "InternalServerError",
                "message": "Error processing request",
                "details": "Database query failed: SELECT * FROM users WHERE user_id='test-user'",
                "stack_trace": "at line 42 in database.py..."
            }
            
    async def mock_error_request(*args, **kwargs):
        return MockErrorResponse()
        
    mocker.patch.object(client._session, "request", side_effect=mock_error_request)
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="test-token"
    )
    
    request = ChatRequest(query="Test query")
    
    # Should raise ServerError but without exposing internal details
    from app.core.gpo_client.exceptions import ServerError
    with pytest.raises(ServerError) as excinfo:
        await client.chat(context=chat_context, request=request)
    
    # Error message should not contain sensitive details
    assert "stack_trace" not in str(excinfo.value)
    assert "SELECT * FROM users" not in str(excinfo.value)
```

### Penetration Testing

For more comprehensive security testing, consider these additional test cases:

1. **Rate Limiting Tests**: Test that rate limiting mechanisms work properly to prevent DoS attacks
2. **Authorization Scope Tests**: Test that users can only access resources they're authorized for
3. **Session Fixation Tests**: Test that session IDs change after authentication
4. **Token Refresh Tests**: Test that token refresh mechanisms work securely
5. **Error Message Tests**: Test that error messages don't reveal sensitive information
6. **Parameter Tampering Tests**: Test that parameter tampering is detected and prevented
7. **Cookie Security Tests**: Test that cookies are used securely (if applicable)

### Automated Security Scanning

Integrate automated security scanning into your testing pipeline:

```bash
# Example: Run Bandit for static security analysis
bandit -r app/core/gpo_client -f json -o security-report.json

# Example: Check dependencies for known vulnerabilities
safety check -r requirements.txt

# Example: Run custom security checks
python security_checks.py
```

### External Security Testing Tools

Consider using these external tools for more comprehensive security testing:

1. **OWASP ZAP**: For API security testing
2. **PyT**: For detecting security vulnerabilities in Python code
3. **Bandit**: For finding common security issues in Python code
4. **Safety**: For checking installed dependencies against security advisories

## Continuous Integration

Set up continuous integration to run tests automatically.

### Example pytest.ini Configuration

```ini
[pytest]
asyncio_mode = auto
testpaths = tests
python_files = test_*.py
python_classes = Test*
python_functions = test_*
markers =
    unit: Unit tests (fast, no external dependencies)
    integration: Integration tests (require API access)
    e2e: End-to-end tests (require full system access)
```

### Example GitHub Actions Workflow

```yaml
# .github/workflows/test.yml
name: Run Tests

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        python-version: [3.8, 3.9, '3.10', '3.11']

    steps:
    - uses: actions/checkout@v3
    
    - name: Set up Python ${{ matrix.python-version }}
      uses: actions/setup-python@v4
      with:
        python-version: ${{ matrix.python-version }}
        
    - name: Install dependencies
      run: |
        python -m pip install --upgrade pip
        pip install -r requirements.txt
        pip install -r requirements-test.txt
        
    - name: Test with pytest (unit tests only)
      run: |
        pytest -v -m "unit"
        
    - name: Test with pytest (integration tests)
      if: github.event_name != 'pull_request'
      env:
        TEST_INSTANCE_ID: ${{ secrets.TEST_INSTANCE_ID }}
        TEST_USER_ID: ${{ secrets.TEST_USER_ID }}
        TEST_PROJECT_ID: ${{ secrets.TEST_PROJECT_ID }}
        GPO_API_URL: ${{ secrets.GPO_API_URL }}
      run: |
        pytest -v -m "integration"
        
    - name: Generate coverage report
      run: |
        coverage run -m pytest
        coverage xml
        
    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v3
```

## Performance Testing

Test how the client performs under load.

### Example Performance Test

```python
import pytest
import asyncio
import time

@pytest.mark.asyncio
async def test_chat_performance(client, mocker):
    """Test the performance of the chat method."""
    # Mock _make_request to return quickly
    mocker.patch.object(
        client, 
        "_make_request",
        return_value={"message_id": "test", "message_content": "test", "sources": []}
    )
    
    chat_context = ChatContext(
        instance_id="test-instance",
        user_id="test-user",
        project_id="test-project",
        token="test-token"
    )
    
    request = ChatRequest(query="Performance test")
    
    # Measure the time for a single request
    start_time = time.time()
    await client.chat(context=chat_context, request=request)
    elapsed = time.time() - start_time
    
    assert elapsed < 0.5, f"Single request took {elapsed:.2f}s, expected < 0.5s"
    
    # Measure throughput with multiple concurrent requests
    num_requests = 10
    start_time = time.time()
    tasks = [client.chat(context=chat_context, request=request) for _ in range(num_requests)]
    await asyncio.gather(*tasks)
    total_elapsed = time.time() - start_time
    
    requests_per_second = num_requests / total_elapsed
    print(f"Throughput: {requests_per_second:.2f} requests per second")
    
    assert requests_per_second > 5, f"Throughput {requests_per_second:.2f} req/s, expected > 5 req/s"
```

## Conclusion

Comprehensive testing is essential to ensure that applications using the GPO Core Python Client work correctly and reliably. By implementing the testing strategies described in this document, you can gain confidence in your code's ability to handle various scenarios, including error conditions, edge cases, and performance requirements.

When contributing to the GPO Core Python Client itself, thorough testing is required to maintain the library's quality and ensure that new features don't break existing functionality.

For more information on related topics, see:
- [Error Handling](error_handling.md) for details on the client's error handling features
- [Security Best Practices](security.md) for testing security aspects
- [Integration Guide](integration.md) for tips on integrating with existing systems
- [Contributing Guide](contributing.md) for contribution requirements, including testing standards