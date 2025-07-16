import os
from dataclasses import dataclass

import yaml
from app.core.key_vault import KeyVaultManager
from dotenv import load_dotenv

load_dotenv()

key_vault_client = KeyVaultManager()


@dataclass
class Logging:
    app_insights_instrumentation_key: str = os.getenv(
        "APP_INSIGHTS_INSTRUMENTATION_KEY",
    )  # type: ignore
    level: str = os.getenv("LOGGING_LEVEL", "NONE")


@dataclass
class OpenAIConfig:
    api_key = key_vault_client.get_secret("ce-assistant-apim-key")
    azure_endpoint = key_vault_client.get_secret(
        "ce-assistant-apim-endpoint"
    )
    deployment_name = key_vault_client.get_secret(
        "ce-assistant-open-ai-deployment-4o"
    )
    engine = key_vault_client.get_secret(
        "ce-assistant-open-ai-deployment-4o"
    )
    model = "gpt-4o"
    version = key_vault_client.get_secret("ce-assistant-open-ai-version-4o")
    num_tokens: int = 256


@dataclass
class EmbeddingsConfig:
    api_key = key_vault_client.get_secret("ce-assistant-apim-key")
    azure_endpoint = key_vault_client.get_secret(
        "ce-assistant-apim-endpoint"
    )
    deployment_name = key_vault_client.get_secret(
        "ce-assistant-open-ai-embedding-deployment"
    )
    version = key_vault_client.get_secret("ce-assistant-open-ai-version")
    model = key_vault_client.get_secret(
        "ce-assistant-open-ai-embedding-model"
    )


class TenacityConfig:
    def __init__(self):
        with open("tenacity.yaml") as file:
            config = yaml.full_load(file)
            self.max_attempts = config["retry"]["max_attempts"]
            self.delay = config["retry"]["delay"]
            self.timeout = config["retry"]["stop_after_delay"]


@dataclass
class AISearchConfig:
    api_key: str = os.getenv("AI_SEARCH_API_KEY")
    endpoint: str = os.getenv("AI_SEARCH_ENDPOINT")
    version: str = os.getenv("AI_SEARCH_API_VERSION")
    ey_ip_project_context_index: str = os.getenv("EY_IP_PROJECT_CONTEXT_INDEX")


@dataclass
class TokenValidationConfig:
    public_key = key_vault_client.get_secret("ceassistant-publickey")
    issuer = key_vault_client.get_secret("JwtCe--Issuer")


@dataclass
class MemorySize:
    eyip = os.getenv("EYIP_MEMORY_SIZE", -6)
    internet = os.getenv("INTERNET_MEMORY_SIZE", -6)
    project_data = os.getenv("PROJECT_DATA_MEMORY_SIZE", -6)
    project_docs = os.getenv("PROJECT_DOCS", -6)


@dataclass
class ResponseOptions:
    # used by backends that use SQL
    return_sql: str = os.getenv("RETURN_SQL", "")
    # used by project docs
    return_chunks: str = os.getenv("PROJECT_DOCS_RETURN_CHUNKS", "")
    calculate_scores: str = os.getenv("ENABLE_CONFIDENCE_SCORES", "false")


@dataclass
class MMRConfig:
    mmr_threshold = os.getenv("MMR_THRESHOLD", 0.6)
    mmr_num_questions = os.getenv("MMR_NUM_QUESTIONS", 3)


@dataclass
class PromptManagerConfig:
    """Configuration for the PromptManager."""
    api_url: str = os.getenv("PROMPT_API_URL", None)
    ce_po_api_key: str = key_vault_client.get_secret("Ce-API-Key-Po")
    # Strategy can be 'api', 'yaml', or 'hybrid' (default)
    strategy: str = os.getenv("PROMPT_MANAGER_STRATEGY", "hybrid") if api_url else "yaml"