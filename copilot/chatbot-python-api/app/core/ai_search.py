import json

from app.core.key_vault import KeyVaultManager
from app.core.pydantic_models import LLMModels
from azure.core.credentials import AzureKeyCredential
from azure.search.documents import SearchClient
from azure.search.documents.aio import SearchClient as AsyncSearchClient
from llama_index.core import StorageContext, VectorStoreIndex
from llama_index.vector_stores.azureaisearch import (
    AzureAISearchVectorStore,
    IndexManagement,
)


class BaseAISearch:

    def __init__(self, llm_models: LLMModels) -> None:
        self.client = None
        self.async_client = None
        self.llm_models = llm_models

    def set_client(self, index_name: str, instance_name: str):
        key_vault = KeyVaultManager()

        ai_search_param = json.loads(key_vault.get_secret(instance_name))
        endpoint = ai_search_param["endpoint"]
        api_key = ai_search_param["key"]
        credential = AzureKeyCredential(api_key)

        self.client = SearchClient(
            endpoint=endpoint,
            index_name=index_name,
            credential=credential,
        )

    async def async_set_client(self, index_name: str, instance_name: str):

        key_vault = KeyVaultManager()
        secret = await key_vault.async_get_secret(instance_name)
        ai_search_param = json.loads(secret)
        endpoint = ai_search_param["endpoint"]
        api_key = ai_search_param["key"]
        credential = AzureKeyCredential(api_key)

        self.async_client = AsyncSearchClient(
            endpoint=endpoint,
            index_name=index_name,
            credential=credential,
        )

    def get_index(self, metadata_fields: dict | None = None):
        """
        Get an existing index
        Args:
            metadata_fields (dict): Metadata fields.
        Returns:
            Index: Return a vector retriver
        """
        vector_store = AzureAISearchVectorStore(
            # self.client must be defined in the set_client first
            search_or_index_client=self.client,
            filterable_metadata_field_keys=metadata_fields,
            index_management=IndexManagement.VALIDATE_INDEX,
            id_field_key="id",
            chunk_field_key="chunk",
            embedding_field_key="embedding",
            embedding_dimensionality=1536,
            metadata_string_field_key="metadata",
            doc_id_field_key="doc_id",
        )
        storage_context = StorageContext.from_defaults(vector_store=vector_store)
        index = VectorStoreIndex.from_documents(
            [],
            storage_context=storage_context,
        )
        return index
