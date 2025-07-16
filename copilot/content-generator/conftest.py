import pytest
from unittest.mock import patch

@pytest.fixture(scope="session", autouse=True)
def client():
    with patch("azure.monitor.opentelemetry.configure_azure_monitor"):
        with patch("opentelemetry.instrumentation.fastapi.FastAPIInstrumentor"):
            with patch("msal.ConfidentialClientApplication.acquire_token_for_client"):
                from app.main import app
                from fastapi.testclient import TestClient
                with TestClient(app) as client:
                    yield client
