from app.core.key_vault import KeyVaultManager
from app.core.pydantic_models import MSServerConfig

key_vault_client = KeyVaultManager()


creds = {
    "tenant_id": key_vault_client.get_secret(
        "ce-assistant-ey-ip-tenant-id"
    ),
    "client_id": key_vault_client.get_secret(
        "ce-assistant-ey-ip-client-id-readonly"
    ),
    "client_secret": key_vault_client.get_secret(
        "ce-assistant-ey-ip-client-secret-readonly"
    ),
    "server": key_vault_client.get_secret("ce-assistant-ey-ip-server"),
    "database": key_vault_client.get_secret("ce-assistant-ey-ip-db"),
}

config = MSServerConfig(**creds)
