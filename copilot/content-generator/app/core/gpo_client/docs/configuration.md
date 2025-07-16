# Configuration Guide

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document explains how to configure the GPO Core Python Client for different environments and use cases. Proper configuration ensures the client operates efficiently and securely when communicating with the GPO Core API.

## Configuration Options

The GPO Core Client offers the following configuration options:

| Option | Type | Description | Default |
|--------|------|-------------|---------|
| `base_url` | string | API base URL | `https://gpo-api.example.com` |
| `timeout` | integer | Request timeout in seconds | 30 |
| `max_retries` | integer | Maximum number of retries | 3 |
| `retry_backoff_factor` | float | Backoff factor for retries | 0.5 |
| `verify_ssl` | boolean | Whether to verify SSL certificates | True |
| `auto_renew_token` | boolean | Auto-renew authentication tokens | False |
| `token_renewal_buffer` | integer | Seconds before expiry to renew | 60 |
| `user_agent` | string | User agent string for API requests | `gpo-python-client/0.1.0` |
| `log_level` | string | Logging level (DEBUG, INFO, etc.) | INFO |
| `trace_requests` | boolean | Log detailed request/response info | False |

## Configuration Methods

The GPO Core Client supports several methods for configuration:

### 1. Environment Variables

Configuration via environment variables is the recommended approach for containerized deployments:

```bash
# GPO Core API Configuration
export GPO_API_BASE_URL="https://gpo-api.example.com"
export GPO_API_TIMEOUT="30"
export GPO_API_MAX_RETRIES="3"
export GPO_API_RETRY_BACKOFF_FACTOR="0.5"
export GPO_API_VERIFY_SSL="true"
export GPO_API_AUTO_RENEW_TOKEN="false"
export GPO_API_TOKEN_RENEWAL_BUFFER="60"
export GPO_API_USER_AGENT="capital-edge-service/1.0.0"
export GPO_API_LOG_LEVEL="INFO"
export GPO_API_TRACE_REQUESTS="false"
```

Then initialize the client without parameters:

```python
from app.core.gpo_client.client import GPOCoreClient

# Configuration loaded automatically from environment variables
client = GPOCoreClient()
```

You can customize the environment variable prefix:

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

# Use custom environment variable prefix
config = GPOConfig.from_env(prefix="MY_APP_GPO_")
client = GPOCoreClient(config)
```

### 2. Configuration File

For local development or testing, you can use YAML or JSON configuration files:

#### YAML Example:

```yaml
# gpo_config.yaml
base_url: "https://gpo-api.example.com"
timeout: 30
max_retries: 3
retry_backoff_factor: 0.5
verify_ssl: true
auto_renew_token: false
token_renewal_buffer: 60
user_agent: "capital-edge-service/1.0.0"
log_level: "INFO"
trace_requests: false
```

#### JSON Example:

```json
{
  "base_url": "https://gpo-api.example.com",
  "timeout": 30,
  "max_retries": 3,
  "retry_backoff_factor": 0.5,
  "verify_ssl": true,
  "auto_renew_token": false,
  "token_renewal_buffer": 60,
  "user_agent": "capital-edge-service/1.0.0",
  "log_level": "INFO",
  "trace_requests": false
}
```

Loading from a configuration file:

```python
from app.core.gpo_client.client import GPOCoreClient

# Load from YAML file
client = GPOCoreClient.from_config_file("gpo_config.yaml")

# Or using config object
from app.core.gpo_client.config import GPOConfig
config = GPOConfig.from_file("gpo_config.json")
client = GPOCoreClient(config)
```

### 3. Direct Parameter Passing

For programmatic configuration:

```python
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.config import GPOConfig

# Create config object with custom parameters
config = GPOConfig(
    base_url="https://gpo-api.example.com",
    timeout=30,
    max_retries=3,
    retry_backoff_factor=0.5,
    verify_ssl=True,
    auto_renew_token=False,
    token_renewal_buffer=60,
    user_agent="capital-edge-service/1.0.0",
    log_level="INFO",
    trace_requests=False
)

# Initialize client with config
client = GPOCoreClient(config)
```

## Environment-Specific Configuration

For different environments (development, staging, production), consider the following patterns:

### Development

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

config = GPOConfig(
    base_url="https://dev-gpo-api.example.com",
    timeout=60,  # Longer timeout for debugging
    max_retries=2,
    verify_ssl=False,  # Often useful for local development
    trace_requests=True,  # Log request/response details
    log_level="DEBUG"
)

client = GPOCoreClient(config)
```

### Staging

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

config = GPOConfig(
    base_url="https://staging-gpo-api.example.com",
    timeout=30,
    max_retries=3,
    verify_ssl=True,
    log_level="INFO"
)

client = GPOCoreClient(config)
```

### Production

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

config = GPOConfig(
    base_url="https://gpo-api.example.com",
    timeout=20,  # Shorter timeout for production
    max_retries=4,  # More retries in production
    retry_backoff_factor=1.0,  # More cautious backoff
    verify_ssl=True,
    auto_renew_token=True,  # Auto-renew tokens in production
    user_agent="capital-edge-service/1.0.0 (production)",
    log_level="WARNING"  # Less verbose logging
)

client = GPOCoreClient(config)
```

## Configuration Priority

When multiple configuration sources are available, the client follows this priority order:

1. Direct parameters passed to `GPOConfig` constructor
2. Configuration loaded from file
3. Environment variables
4. Default values

This means that any parameters explicitly passed to the constructor will override environment variables or defaults.

## Dynamic Configuration Updates

While most configuration options are set at initialization, some settings can be updated dynamically:

```python
from app.core.gpo_client.client import GPOCoreClient

client = GPOCoreClient()

# Update log level
client.http_client.update_log_level("DEBUG")

# Enable request tracing
client.http_client.set_trace_requests(True)
```

## Proxy Configuration

For environments that require a proxy:

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

config = GPOConfig(
    base_url="https://gpo-api.example.com",
    proxy="http://proxy.example.com:8080",
    proxy_auth="username:password"  # Optional proxy authentication
)

client = GPOCoreClient(config)
```

## SSL Certificate Verification

For SSL certificate handling:

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

# Disable SSL verification (not recommended for production)
config = GPOConfig(verify_ssl=False)

# Use a custom CA certificate
config = GPOConfig(
    verify_ssl="/path/to/custom/ca/cert.pem"
)

# Use a client certificate
config = GPOConfig(
    client_cert="/path/to/client.pem"
)

client = GPOCoreClient(config)
```

## Logging Configuration

Control the client's logging behavior:

```python
import logging
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

# Configure Python's logging module
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)

# Set client's log level through configuration
config = GPOConfig(
    log_level="DEBUG",
    trace_requests=True
)

client = GPOCoreClient(config)
```

## Configuration Validation

The GPO Core Python Client validates the configuration at startup. Invalid configurations will raise an exception:

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.exceptions import InvalidConfigError

try:
    # Invalid configuration - negative timeout
    config = GPOConfig(timeout=-5)
    client = GPOCoreClient(config)
except InvalidConfigError as e:
    print(f"Configuration error: {e}")
```

## Advanced Configuration

### Custom HTTP Adapter

For advanced HTTP configuration:

```python
import aiohttp
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.config import GPOConfig

# Create a custom connector
connector = aiohttp.TCPConnector(
    limit=20,  # Maximum number of connections
    ttl_dns_cache=300,  # DNS cache TTL in seconds
    use_dns_cache=True,
    ssl=False
)

# Create client with custom session params
config = GPOConfig(
    base_url="https://gpo-api.example.com",
    http_session_params={
        "connector": connector,
        "timeout": aiohttp.ClientTimeout(total=30, connect=10),
        "raise_for_status": True
    }
)

client = GPOCoreClient(config)
```

### Configuration for High Volume

For applications with high request volumes:

```python
from app.core.gpo_client.config import GPOConfig
from app.core.gpo_client.client import GPOCoreClient

config = GPOConfig(
    base_url="https://gpo-api.example.com",
    http_session_params={
        "connector": {
            "limit": 100,  # Higher connection limit
            "keepalive_timeout": 120  # Longer keepalive
        }
    },
    timeout=60,
    auto_renew_token=True,
    token_renewal_buffer=120  # Renew token 2 minutes before expiry
)

client = GPOCoreClient(config)
```

## Configuration Best Practices

1. **Use environment variables in production** for flexibility and security
2. **Never hardcode credentials** in your source code
3. **Customize timeouts** based on your application's needs
4. **Enable auto token renewal** for long-running applications
5. **Include a meaningful user agent** to help API administrators identify your service
6. **Use HTTPS** for all production environments
7. **Test with tracing enabled** during development, but disable in production
8. **Validate configuration** before deploying to production
9. **Keep retry counts reasonable** to prevent overwhelming the API during outages
10. **Use different configurations** for different environments

## Troubleshooting Configuration Issues

If you encounter issues with your configuration:

1. **Enable debug logging** to see more detail about client behavior
2. **Enable request tracing** to see details of HTTP requests and responses
3. **Validate your base URL** is correct and accessible
4. **Check environment variables** are set correctly
5. **Ensure SSL certificates** are valid if using HTTPS
6. **Verify proxy settings** if operating in a proxied environment

For more help, see the [Troubleshooting Guide](troubleshooting.md).