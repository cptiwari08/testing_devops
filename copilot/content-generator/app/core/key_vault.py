import os
from typing import Any

from app.core.errors import KeyVaultHTTPConnectionError, SecretNotFoundError
from azure.core.exceptions import HttpResponseError, ResourceNotFoundError
from azure.identity import DefaultAzureCredential
from azure.identity.aio import DefaultAzureCredential as AsyncDefaultAzureCredential
from azure.keyvault.secrets import SecretClient
from azure.keyvault.secrets._models import KeyVaultSecret
from azure.keyvault.secrets.aio import SecretClient as AsyncSecretClient
from dotenv import load_dotenv

load_dotenv()


class KeyVaultManager:
    def __init__(self) -> None:
        self._cache = {}

    def get_secret(self, secret_name) -> str | None:
        if secret_name in self._cache:
            return self._cache[secret_name]

        credential = DefaultAzureCredential()

        # Do not move the url to app.core.config
        # that is going to raise a circular import
        url = str(os.getenv("KEY_VAULT_URL"))

        client = SecretClient(vault_url=url, credential=credential)

        try:
            secret: KeyVaultSecret = client.get_secret(secret_name)
            self._cache[secret_name] = secret.value
        except ResourceNotFoundError:
            raise SecretNotFoundError(secret_name)
        except HttpResponseError as e:
            raise KeyVaultHTTPConnectionError(secret_name, e)
        except Exception as e:
            raise e

        return secret.value

    async def async_get_secret(self, secret_name) -> str | None:
        if secret_name in self._cache:
            return self._cache[secret_name]

        url = str(os.getenv("KEY_VAULT_URL"))
        credentials = AsyncDefaultAzureCredential()
        client = AsyncSecretClient(url, credentials)
        try:
            secret: KeyVaultSecret = await client.get_secret(secret_name)
            self._cache[secret_name] = secret.value
            return secret.value
        except ResourceNotFoundError:
            raise SecretNotFoundError(secret_name)
        except HttpResponseError as e:
            raise KeyVaultHTTPConnectionError(secret_name, e)
        finally:
            await credentials.close()
            await client.close()

    def get_cache_secrets(self) -> dict[Any, Any]:
        return self._cache
