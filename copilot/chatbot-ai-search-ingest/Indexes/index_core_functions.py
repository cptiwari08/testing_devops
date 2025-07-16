from inspect import signature
import json
import os
import sys
import requests
import time
from azure.core.credentials import AzureKeyCredential
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient
from azure.search.documents.indexes import SearchIndexClient
from azure.search.documents.indexes.models import (
    ExhaustiveKnnAlgorithmConfiguration,
    ExhaustiveKnnParameters,
    HnswAlgorithmConfiguration,
    HnswParameters,
    SearchIndex,
    SemanticConfiguration,
    SemanticField,
    SemanticPrioritizedFields,
    SemanticSearch,
    SearchField,
    SearchableField,
    SearchFieldDataType,
    SimpleField,
    VectorSearch,
    VectorSearchAlgorithmKind,
    VectorSearchAlgorithmMetric,
    VectorSearchProfile,
)
from llama_index.core import StorageContext, VectorStoreIndex
from llama_index.core.settings import Settings
from llama_index.embeddings.azure_openai import AzureOpenAIEmbedding
from llama_index.llms.azure_openai import AzureOpenAI
from llama_index.vector_stores.azureaisearch import (
    AzureAISearchVectorStore,
    IndexManagement,
)
from logger import Logger
from msal import ConfidentialClientApplication

log = Logger()


def measure_index_execution_time(func):
    """
    Creates a retriever from filters.
    This method creates a retriever from the given metadata fields and filters.

    Args:
        metadata_fields (dict): Metadata fields.
        filters (MetadataFilters): Metadata filters.
        similarity_top_k (int, optional): Defaults to 1.

    Returns:
        Retriever: Return a vector retriver
    """
    def wrapper(*args, **kwargs):
        start_time = time.time()
        result = func(*args, **kwargs)
        end_time = time.time()
        execution_time = end_time - start_time
        arguments = signature(func).bind(*args, **kwargs)
        arguments.apply_defaults()
        index_name = arguments.arguments.get("index_name")
        log.info(f"Index {index_name} successfully loaded after {execution_time:.0f} seconds")
        return result
    return wrapper


# Getting data from Azure
key_vault_url = os.getenv("KEY_VAULT_URL")

# Create a DefaultAzureCredential instance
credential_secret = DefaultAzureCredential()

# Create a SecretClient instance
try:
    secret_client = SecretClient(vault_url=key_vault_url, credential=credential_secret)
    # Open AI variables
    azure_openai_embedding_deployment = secret_client.get_secret("ce-assistant-open-ai-embedding-deployment").value
    azure_openai_embedding_model = secret_client.get_secret("ce-assistant-open-ai-embedding-model").value
    azure_openai_deployment = secret_client.get_secret("ce-assistant-open-ai-deployment").value
    azure_openai_model = secret_client.get_secret("ce-assistant-open-ai-model").value
    api_key = secret_client.get_secret("ce-assistant-apim-key").value
    azure_endpoint = secret_client.get_secret("ce-assistant-apim-endpoint").value
    api_version = secret_client.get_secret("ce-assistant-open-ai-version").value

    # LLM and Embedding Config
    llm = AzureOpenAI(
        model=azure_openai_model,
        deployment_name=azure_openai_deployment,
        api_key=api_key,
        azure_endpoint=azure_endpoint,
        api_version=api_version,
    )

    # You need to deploy your own embedding model as well as your own chat completion model
    embed_model = AzureOpenAIEmbedding(
        model=azure_openai_embedding_model,
        deployment_name=azure_openai_embedding_deployment,
        api_key=api_key,
        azure_endpoint=azure_endpoint,
        api_version=api_version,
    )

    Settings.llm = llm
    Settings.embed_model = embed_model
except Exception as ex:
    ex.add_note("CONTEXT: Error occurred when getting information from secret")
    raise


class DBConnection:
    """
        Creates a retriever from filters.
        This method creates a retriever from the given metadata fields and filters.

        Args:
            metadata_fields (dict): Metadata fields.
            filters (MetadataFilters): Metadata filters.
            similarity_top_k (int, optional): Defaults to 1.

        Returns:
            Retriever: Return a vector retriver
        """
    ce_po_api_key = secret_client.get_secret("Ce-API-Key-Po").value

    headers = {
        "Accept": "application/json, text/plain, */*",
        "Accept-Language": "en-US,en;q=0.9",
        "ce-api-key": ce_po_api_key,
        "Connection": "keep-alive",
        "Content-Type": "application/json",
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) "
                      "Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0",
    }

    @classmethod
    def execute_query(cls, payload: dict):
        """
        SQL endpoint retriever.
        This method reads the data from a SQL endpoint of a CE4 Program Office database in JSON format.

        Returns:
            Retriever: Return a JSON with the data of the suggestions.
        """
        try:
            response = requests.post(
                f"{os.getenv('HOST_PROGRAM_OFFICE')}/copilot/execute-query",
                headers=cls.headers,
                data=json.dumps(payload),
            )
            response.raise_for_status()
            return response.json()
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when making the SQL request")
            raise

    @classmethod
    def suggestions(cls):
        """
        Suggestions retriever.
        This method reads the data from an endpoint of a CE5 database in JSON format.

        Returns:
            Retriever: Return a JSON with the data of the suggestions.
        """
        try:
            response = requests.get(f"{os.getenv('HOST_COPILOT_API')}/suggestions", headers=cls.headers)
            response.raise_for_status()
            return response.json()
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when requesting the suggestions")
            raise

    @classmethod
    def get_glossary(cls):
        try:
            response = requests.get(f"{os.getenv('HOST_COPILOT_API')}/glossary", headers=cls.headers)
            response.raise_for_status()
            return response.json()
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when requesting the glossary")
            raise

    @classmethod
    def create_ey_ip_db_connection(cls):
        """
        EY IP connection retriever.
        This method reads the related secrets and creates the ODBC Engine using SQL Alchemy.

        Returns:
            Retriever: Return a SQLAlchemy ODBC engine.
        """
        from sqlalchemy.engine import URL
        from sqlalchemy import create_engine

        # Azure AD application (client) ID, Secret, and Tenant ID
        try:
            tenant_id = secret_client.get_secret("ce-assistant-ey-ip-tenant-id").value
            client_id = secret_client.get_secret("ce-assistant-ey-ip-client-id").value
            client_secret = secret_client.get_secret("ce-assistant-ey-ip-client-secret").value
            server_eyip = secret_client.get_secret("ce-assistant-ey-ip-server").value
            db_eyip = secret_client.get_secret("ce-assistant-ey-ip-db").value
        except Exception as e:
            e.add_note("Error occurred when fetching information from secret")
            raise

        # Authority URL
        authority = f'https://login.microsoftonline.com/{tenant_id}'
        # Scope for Azure SQL Database
        scope = ["https://database.windows.net//.default"]

        # Initialize the MSAL confidential client
        app = ConfidentialClientApplication(client_id, authority=authority, client_credential=client_secret)
        # Acquire token
        result = app.acquire_token_for_client(scopes=scope)

        # Check if the token was acquired
        if "access_token" not in result:
            log.error(
                f"Error acquiring token: {result.get('error')}. correlation_id: {result.get('correlation_id')}. "
                f"{result.get('error_description')}"
            )
            sys.exit()
        access_token = result['access_token']

        # Establish connection using the ODBC Driver
        try:
            connection_string = (f"DRIVER={{ODBC Driver 18 for SQL Server}};SERVER={server_eyip};DATABASE={db_eyip};"
                                 f"Authentication=ActiveDirectoryServicePrincipal;AccessToken={access_token};"
                                 f"Uid={client_id};Pwd={client_secret}")

            connection_url = URL.create("mssql+pyodbc", query={"odbc_connect": connection_string})
            return create_engine(connection_url)
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when retrieving the information from the EY IP SQL Server")
            raise


class AISearchCollection:
    """
    Class to handle multiple AI Search instances.
    This class initialize one or multiple instances from the env variables and process the data accordingly.
    """

    def __init__(self):
        self.__instances = [AISearch(ia.strip()) for ia in os.getenv("AIS_INSTANCE").split(",")]

    @property
    def instances(self):
        """
        AI Search instances retriever.
        This method is the getter method for the list of AI Search instances.

        Returns:
            Retriever: Return a list of AISearch instances.
        """
        return self.__instances

    def process_all_instances(self, metadata, index_name, doc, fields):
        """
        Execute the process of all the stored AI Search instances.
        This method creates a retriever from the given metadata fields and filters.

        Args:
            metadata (dict): Metadata fields.
            index_name (str): Name of the index to be processed.
            doc (list): List of documents to be loaded.
            fields (list): List of fields to be used as a schema to create the Index.
        """
        for ai_search in self.__instances:
            try:
                ai_search.process_all_in_one(metadata, index_name, doc, fields)
            except Exception as e:
                log.error(f"An error occurred when loading the data to the {index_name} index in the {ai_search.instance_name} instance")
                log.error(e)
                continue


class AISearch:
    """
    Creates a handler for each AI Search Instance.
    This class hold all the methods to delete/create/load data in the indexes of the given AI Search instance.
    It grabs the information for the connection from the Azure Key Vault.

    Args:
        instance_name (str): Name of the AI Search instance.
    """

    vector_search = VectorSearch(
        algorithms=[
            HnswAlgorithmConfiguration(
                name="myHnsw",
                kind=VectorSearchAlgorithmKind.HNSW,
                parameters=HnswParameters(
                    m=4,
                    ef_construction=400,
                    ef_search=500,
                    metric=VectorSearchAlgorithmMetric.COSINE,
                ),
            ),
            ExhaustiveKnnAlgorithmConfiguration(
                name="myExhaustiveKnn",
                kind=VectorSearchAlgorithmKind.EXHAUSTIVE_KNN,
                parameters=ExhaustiveKnnParameters(
                    metric=VectorSearchAlgorithmMetric.COSINE,
                ),
            ),
        ],
        profiles=[
            VectorSearchProfile(
                name="myHnswProfile",
                algorithm_configuration_name="myHnsw",
            ),
            # Add more profiles if needed
            VectorSearchProfile(
                name="myExhaustiveKnnProfile",
                algorithm_configuration_name="myExhaustiveKnn",
            ),
            # Add more profiles if needed
        ],
    )

    semantic_config = SemanticConfiguration(
        name="mySemanticConfig",
        prioritized_fields=SemanticPrioritizedFields(
            content_fields=[SemanticField(field_name="chunk")]
        )
    )

    semantic_search = SemanticSearch(configurations=[semantic_config])

    def __init__(self, instance_name):
        # AI Search variables
        self.__instance_name = instance_name
        self.__ai_search_param = json.loads(secret_client.get_secret(instance_name).value)
        self.__search_service_api_key = self.__ai_search_param["key"]
        self.__search_service_endpoint = self.__ai_search_param["endpoint"]
        self.__search_service_api_version = self.__ai_search_param["version"]
        self.__credential = AzureKeyCredential(self.__search_service_api_key) if len(
            self.__search_service_api_key) > 0 else credential_secret
        self.__client = SearchIndexClient(endpoint=self.__search_service_endpoint, credential=self.__credential)

    @property
    def instance_name(self):
        return self.__instance_name

    def delete_index(self, index_rm):
        """
        AI Search Index deletion.
        This method deletes the given index using an AISearch API.
        Status code 204 and 404 mean a successful operation.

        Args:
            index_rm (str): Index to be deleted
        """
        try:
            # Target URL
            url = f'{self.__search_service_endpoint}/indexes/{index_rm}?api-version={self.__search_service_api_version}'

            headers = {'Content-Type': 'application/json', 'api-key': self.__search_service_api_key}

            # Sending a GET request to the URL
            response = requests.delete(url, headers=headers)
            if response.status_code == 204:
                log.info(f"Index {index_rm} found and deleted correctly in the {self.__instance_name} instance")
            elif response.status_code == 404:
                log.info(f"Index {index_rm} not found when attempting to delete it in the {self.__instance_name} instance")
            else:
                response.raise_for_status()
        except Exception as e:
            e.add_note("CONTEXT: Error happened when deleting the index")
            raise

    def create_index(self, index_name, fields):
        """
        AI Search Index creation.
        This method creates the given index with the specified fields using the AISearch library.
        Both, semantic and vector search configurations are set up.

        Args:
            index_name (str): Index to be deleted
            fields (list): List of fields to be used as a schema to create the Index.
        """
        try:
            index = SearchIndex(
                name=index_name,
                fields=fields,
                vector_search=self.vector_search,
                semantic_search=self.semantic_search
            )

            self.__client.create_or_update_index(index)
        except Exception as e:
            e.add_note("CONTEXT: Error happened when creating the index")
            raise

    def load_index(self, metadata, index_name, doc):
        """
        AI Search Index loading.
        This method loads the given documents and metadata in the specified index using the AISearch library.

        Args:
            index_name (str): Index to be deleted
            metadata (dict): Metadata fields.
            doc (list): List of documents to be loaded.
        """
        try:
            vector_store = AzureAISearchVectorStore(
                search_or_index_client=self.__client,
                filterable_metadata_field_keys=metadata,
                index_name=index_name,
                index_management=IndexManagement.CREATE_IF_NOT_EXISTS,
                id_field_key="id",
                chunk_field_key="chunk",
                embedding_field_key="embedding",
                embedding_dimensionality=1536,
                metadata_string_field_key="metadata",
                doc_id_field_key="doc_id",
            )
            storage_context = StorageContext.from_defaults(vector_store=vector_store)
            VectorStoreIndex.from_documents(doc, storage_context=storage_context)
        except Exception as e:
            e.add_note("CONTEXT: Error happened when loading the data into the index")
            raise

    @measure_index_execution_time
    def process_all_in_one(self, metadata, index_name, doc, fields):
        """
        Execute the process of deletion/creation/loading all in one.
        This method executes all the steps with the given parameters.

        Args:
            metadata (dict): Metadata fields.
            index_name (str): Name of the index to be processed.
            doc (list): List of documents to be loaded.
            fields (list): List of fields to be used as a schema to create the Index.
        """
        self.delete_index(index_rm=index_name)
        self.create_index(index_name=index_name, fields=fields)
        self.load_index(metadata=metadata, index_name=index_name, doc=doc)


class FieldHandler:
    """
    Class to handle the Field creation.
    This class initialize a basic list of fields and then lets you create new ones with specific configurations.
    """

    def __init__(self):
        # Constants
        self.__DATA_TYPE_STR = "Edm.String"
        self.__DATA_TYPE_INT = "Edm.Int32"
        self.__DATA_TYPE_BOL = "Edm.Boolean"
        self.__ANALYZER_NAME = "en.microsoft"

        self.__fields = [
            SimpleField(name="id", type=self.__DATA_TYPE_STR, key=True, retrievable=True),
            SearchField(
                name="embedding",
                type=SearchFieldDataType.Collection(SearchFieldDataType.Single),
                searchable=True,
                vector_search_dimensions=1536,
                vector_search_profile_name="myHnswProfile",
            ),
        ]
        self.add_str_field({"name": "chunk", "retrievable": True, "searchable": True})
        self.add_str_field({"name": "metadata", "retrievable": True})
        self.add_str_field({"name": "doc_id", "retrievable": True, "filterable": True})

    def add_str_field(self, kwargs):
        """
        Fields addition.
        This method adds to the list a nre field with the given configuration.
        SimpleField and SearchableField are the field types allowed.
        Just string fields can be added.

        Args:
            kwargs (dict): Fields configuration.
        """
        if kwargs.get('searchable'):
            self.__fields.append(SearchableField(type=self.__DATA_TYPE_STR, analyzer_name=self.__ANALYZER_NAME, **kwargs))
        else:
            self.__fields.append(SimpleField(type=self.__DATA_TYPE_STR, **kwargs))
    
    def add_bool_field(self, kwargs):
        """
        Fields addition.
        This method adds to the list a nre field with the given configuration.
        SimpleField and SearchableField are the field types allowed.
        Just int fields can be added.

        Args:
            kwargs (dict): Fields configuration.
        """
        if kwargs.get('searchable'):
            self.__fields.append(SearchableField(type=self.__DATA_TYPE_BOL, analyzer_name=self.__ANALYZER_NAME, **kwargs))
        else:
            self.__fields.append(SimpleField(type=self.__DATA_TYPE_BOL, **kwargs))
    
    def add_int_field(self, kwargs):
        """
        Fields addition.
        This method adds to the list a nre field with the given configuration.
        SimpleField and SearchableField are the field types allowed.
        Just int fields can be added.

        Args:
            kwargs (dict): Fields configuration.
        """
        if kwargs.get('searchable'):
            self.__fields.append(SearchableField(type=self.__DATA_TYPE_INT, analyzer_name=self.__ANALYZER_NAME, **kwargs))
        else:
            self.__fields.append(SimpleField(type=self.__DATA_TYPE_INT, **kwargs))

    @property
    def fields(self):
        """
        Index fields retriever.
        This method is the getter method for the list of Fields for a specific AI Search index.

        Returns:
            Retriever: Return a list of index fields.
        """
        return self.__fields

