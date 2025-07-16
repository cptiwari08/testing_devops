import os

from app.core.errors import KeyVaultHTTPConnectionError, SecretNotFoundError
from azure.core.exceptions import HttpResponseError, ResourceNotFoundError
from azure.identity import DefaultAzureCredential
from azure.identity.aio import DefaultAzureCredential as AsyncDefaultAzureCredential
from azure.keyvault.secrets import SecretClient
from azure.keyvault.secrets.aio import SecretClient as AsyncSecretClient
from dotenv import load_dotenv

load_dotenv()


class KeyVaultManager:

    def get_secret(self, secret_name) -> str | None:

        # Use DefaultAzureCredential to handle authentication
        credential = DefaultAzureCredential()

        # Do not move the url to app.core.config
        # that is going to raise a circular import
        url = str(os.getenv("KEY_VAULT_URL"))

        # Create a SecretClient to interact with the Key Vault
        client = SecretClient(vault_url=url, credential=credential)
        # Retrieve the secret from the Key Vault
        try:
            secret = client.get_secret(secret_name)
        except ResourceNotFoundError:
            raise SecretNotFoundError(secret_name)
        except HttpResponseError as e:
            raise KeyVaultHTTPConnectionError(secret_name, e)

        return secret.value


    async def async_get_secret(self, secret_name) -> str | None:
        url = str(os.getenv("KEY_VAULT_URL"))
        async with AsyncDefaultAzureCredential() as credentials:
            async with AsyncSecretClient(url, credentials) as client:
                try:
                    secret = await client.get_secret(secret_name)
                except ResourceNotFoundError:
                    raise SecretNotFoundError(secret_name)
                except HttpResponseError as e:
                    raise KeyVaultHTTPConnectionError(secret_name, e)

        return secret.value
