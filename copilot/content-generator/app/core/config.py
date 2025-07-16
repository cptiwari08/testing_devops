import json
import os
from dataclasses import dataclass
from app.core.key_vault import KeyVaultManager
from dotenv import load_dotenv

load_dotenv()


@dataclass
class Config:
    ENABLE_APP_INSIGHTS: str = os.getenv("ENABLE_APP_INSIGHTS", "false").lower()
    LOG_LEVEL: str = os.getenv("LOG_LEVEL", "DEBUG").upper()
    PROJECT_FRIENDLY_ID: str = os.getenv("PROJECT_FRIENDLY_ID", "")
    APPLICATIONINSIGHTS_CONNECTION_STRING: str = os.getenv(
        "APPLICATIONINSIGHTS_CONNECTION_STRING", ""
    )


class AISearchConfig:
    def __init__(self, key_vault: KeyVaultManager) -> None:
        ai_search_instance_name = os.getenv("AI_SEARCH_INSTANCE_NAME", "")
        ai_search = str(key_vault.get_secret(ai_search_instance_name))
        ai_search = json.loads(ai_search)
        self.index_name = os.getenv("PMO_WORKPLAN_INDEX_NAME", "")
        self.endpoint = ai_search["endpoint"]
        self.api_key = ai_search["key"]


class AssetManagerConfig:
    def __init__(self, key_vault: KeyVaultManager) -> None:
        self.tenant_id = key_vault.get_secret("ce-assistant-ey-ip-tenant-id")
        self.client_id = key_vault.get_secret("ce-assistant-ey-ip-client-id-readonly")
        self.client_secret = key_vault.get_secret(
            "ce-assistant-ey-ip-client-secret-readonly"
        )
        self.server_eyip = key_vault.get_secret("ce-assistant-ey-ip-server")
        self.db_eyip = key_vault.get_secret("ce-assistant-ey-ip-db")


class RedisConfig:
    def __init__(self, key_vault: KeyVaultManager) -> None:
        self.localhost: bool = os.getenv("REDIS_LOCAL", "false").lower() == "true"
        self.sentinel_service_name: str = "mymaster"
        self.sentinel_port: int = 26379
        self.redis_port: int = int(str(key_vault.get_secret("RedisPort")))
        self.password: str = str(key_vault.get_secret("RedisKey"))
        self.url: str = str(key_vault.get_secret("RedisName"))


class OpenAIConfig:
    def __init__(self, key_vault: KeyVaultManager) -> None:
        self.api_key = key_vault.get_secret("ce-assistant-apim-key")
        self.azure_endpoint = key_vault.get_secret("ce-assistant-apim-endpoint")
        self.deployment_name = key_vault.get_secret(
            "ce-assistant-open-ai-deployment-4o"
        )
        self.engine = key_vault.get_secret("ce-assistant-open-ai-deployment-4o")
        self.model = "gpt-4o"
        self.version = key_vault.get_secret("ce-assistant-open-ai-version-4o")
        self.num_tokens: int = 256


class EmbeddingsConfig:
    def __init__(self, key_vault: KeyVaultManager) -> None:
        self.api_key = key_vault.get_secret("ce-assistant-apim-key")
        self.azure_endpoint = key_vault.get_secret("ce-assistant-apim-endpoint")
        self.deployment_name = key_vault.get_secret(
            "ce-assistant-open-ai-embedding-deployment"
        )
        self.version = key_vault.get_secret("ce-assistant-open-ai-version")
        self.model = key_vault.get_secret("ce-assistant-open-ai-embedding-model")


class CopilotApiConfig:
    def __init__(self, key_vault: KeyVaultManager) -> None:
        self.host_copilot_api: str = str(os.getenv("HOST_COPILOT_API"))
        self.copilot_api_key = key_vault.get_secret("Ce-API-Key-Po")
        self.timeout: int = int(os.getenv("COPILOT_API_TIMEOUT", 50))


@dataclass
class ResponseOptions:
    # used by backends that use SQL
    return_context: str = os.getenv("RETURN_CONTEXT", "")


key_vault_client = KeyVaultManager()


@dataclass
class PromptManagerConfig:
    """Configuration for the PromptManager."""

    api_url: str = os.getenv("HOST_COPILOT_API") + "/Prompt"
    ce_po_api_key: str = key_vault_client.get_secret("Ce-API-Key-Po")
    # Strategy can be 'api', 'yaml', or 'hybrid' (default)
    strategy: str = os.getenv("PROMPT_MANAGER_STRATEGY", "hybrid")


@dataclass
class GPOConfig:
    """
    Unified configuration for the GPO Core API client.
    Reads from environment variables or KeyVault as needed.
    All field names match those expected by GPOCoreClient and HttpClient.
    """
    base_url: str = key_vault_client.get_secret("satKnowledgeAssistantBaseAddress")
    timeout: int = int(os.getenv("GPO_API_TIMEOUT", 30))
    max_retries: int = int(os.getenv("GPO_API_MAX_RETRIES", 3))
    retry_backoff_factor: float = float(os.getenv("GPO_API_RETRY_BACKOFF_FACTOR", 0.5))
    verify_ssl: bool = os.getenv("GPO_API_VERIFY_SSL", "true").lower() in ("true", "1", "yes")
    debug_pdf_storage: bool = os.getenv("GPO_DEBUG_PDF_STORAGE", "false").lower() in ("true", "1", "yes")
    use_key_vault: bool = os.getenv("GPO_USE_KEY_VAULT", "true").lower() in ("true", "1", "yes")
    api_key: str = ""  # Will be populated from KeyVault if use_key_vault is True

    def __post_init__(self):
        """Initialize API key from KeyVault if configured to do so and if no API key was provided."""
        # If api_key is already set (e.g., provided by the user), don't override it
        if not self.api_key:
            if self.use_key_vault:
                try:
                    self.api_key = key_vault_client.get_secret(self.key_vault_secret_name)
                except Exception as e:
                    import logging
                    logging.getLogger(__name__).error(f"Failed to get API key from KeyVault: {e}")
                    # Fall back to environment variable if KeyVault fails
                    self.api_key = os.getenv("GPO_API_KEY", "")
            else:
                # Use environment variable directly
                self.api_key = os.getenv("GPO_API_KEY", "")

        if not self.api_key:
            import logging
            logging.getLogger(__name__).warning("No GPO API key configured. Functionality may be limited.")