import os
import sys
import pytest
from unittest.mock import patch, MagicMock

# Define our secret cache first, before any imports
SECRET_CACHE = {
    "RedisPort": "6379",
    "RedisKey": "fake_password",
    "RedisName": "localhost",
    "ce-assistant-apim-key": "fake_api_key",
    "ce-assistant-apim-endpoint": "https://fake-endpoint",
    "ce-assistant-open-ai-deployment-4o": "fake_deployment",
    "ce-assistant-open-ai-version-4o": "2023-05-15",
    "ce-assistant-open-ai-version": "2023-05-15",
    "ce-assistant-open-ai-embedding-deployment": "fake_embedding_deployment",
    "ce-assistant-open-ai-embedding-model": "text-embedding-ada-002",
    "Ce-API-Key-Po": "fake_po_api_key"
}

# Set required environment variables before any imports
os.environ["KEY_VAULT_URL"] = "https://fake-keyvault.vault.azure.net/"

# Create a mock KeyVaultManager class before importing anything from the app
class MockKeyVaultManager:
    """Mock implementation of KeyVaultManager for testing."""
    
    def __init__(self):
        """Initialize with pre-populated cache to avoid real KeyVault calls."""
        self._cache = SECRET_CACHE.copy()
    
    def get_secret(self, secret_name):
        """Return pre-defined secret values without real authentication."""
        if secret_name in self._cache:
            return self._cache[secret_name]
        
        # Return a fake value for unknown secrets
        self._cache[secret_name] = f"fake_{secret_name}_value"
        return self._cache[secret_name]

# Patch the module before it's imported
sys.modules["app.core.key_vault"] = MagicMock()
sys.modules["app.core.key_vault"].KeyVaultManager = MockKeyVaultManager

# Set up additional Azure mocks
mock_credential = MagicMock()
mock_token = MagicMock()
mock_token.token = "fake-token"
mock_token.expires_on = 12345678

sys.modules["azure.identity"] = MagicMock()
sys.modules["azure.identity"].DefaultAzureCredential = MagicMock(return_value=mock_credential)

# Now we can import and set up client fixture with other mocks
@pytest.fixture(scope="session", autouse=True)
def client():
    """Create test client with all Azure components mocked."""
    with patch("azure.monitor.opentelemetry.configure_azure_monitor"):
        with patch("opentelemetry.instrumentation.fastapi.FastAPIInstrumentor"):
            with patch("msal.ConfidentialClientApplication.acquire_token_for_client"):
                from app.main import app
                from fastapi.testclient import TestClient
                return TestClient(app)