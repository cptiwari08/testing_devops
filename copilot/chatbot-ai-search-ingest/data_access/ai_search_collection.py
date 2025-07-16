import os
import json
import requests
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
from llama_index.embeddings.azure_openai import AzureOpenAIEmbedding,AzureOpenAI
from llama_index.core.settings import Settings
from llama_index.vector_stores.azureaisearch import (
    AzureAISearchVectorStore,
    IndexManagement,
)
from utils.logger import Logger
#from utils import measure_index_execution_time
from dotenv import load_dotenv

load_dotenv(override=True)

log = Logger()

key_vault_url = os.getenv("KEY_VAULT_URL")
credential_secret = DefaultAzureCredential()
secret_client = SecretClient(vault_url=key_vault_url, credential=credential_secret)

class AISearchCollection:
    def __init__(self):
        self.__instances = [AISearch(ia.strip()) for ia in os.getenv("AIS_INSTANCE").split(",")]

    @property
    def instances(self):
        return self.__instances

    def process_all_instances(self, index_name, doc, fields, metadata=None):
        for ai_search in self.__instances:
            try:
                ai_search.process_all_in_one(index_name, doc, fields,metadata)
            except Exception as e:
                log.error(f"An error occurred when loading the data to the {index_name} index in the {ai_search.instance_name} instance")
                log.error(e)
                continue

class AISearch:
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
            VectorSearchProfile(
                name="myExhaustiveKnnProfile",
                algorithm_configuration_name="myExhaustiveKnn",
            ),
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
        try:
            url = f'{self.__search_service_endpoint}/indexes/{index_rm}?api-version={self.__search_service_api_version}'
            headers = {'Content-Type': 'application/json', 'api-key': self.__search_service_api_key}
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
        try:
            azure_openai_embedding_deployment = secret_client.get_secret("ce-assistant-open-ai-embedding-deployment").value
            azure_openai_embedding_model = secret_client.get_secret("ce-assistant-open-ai-embedding-model").value
            api_key = secret_client.get_secret("ce-assistant-apim-key").value
            azure_endpoint = secret_client.get_secret("ce-assistant-apim-endpoint").value
            api_version = secret_client.get_secret("ce-assistant-open-ai-version").value

            embed_model = AzureOpenAIEmbedding(
            model=azure_openai_embedding_model,
            deployment_name=azure_openai_embedding_deployment,
            api_key=api_key,
            azure_endpoint=azure_endpoint,
            api_version=api_version,
            )   
            Settings.embed_model = embed_model

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
            log.info(f"Index {index_name} successfully loaded")
        except Exception as e:
            e.add_note("CONTEXT: Error happened when loading the data into the index")
            raise

    def load_index_general(self, index_name, doc):
        """
        AI Search Index loading.
        This method loads the given documents and metadata in the specified index using the AISearch library.

        Args:
            index_name (str): Index to be deleted
            metadata (dict): Metadata fields.
            doc (list): List of documents to be loaded.
        """
        try:

            #for i in range(0,len(doc),700):
            #    self.__search_client.upload_documents(documents=doc[i:i+700])
            #    print(len(doc[i:i+700])," documents loaded")

            url = f'{self.__search_service_endpoint}/indexes/{index_name}/docs/index?api-version={self.__search_service_api_version}'
            headers = {'Content-Type': 'application/json', 'api-key': self.__search_service_api_key}
            # Sending a GET request to the URL
            response = requests.post(url, headers=headers,data=doc)
            if response.status_code == 200:
                log.info(f"Data correctly loaded into {index_name}. Instance: {self.__instance_name} instance")
            elif response.status_code == 404:
                log.info(f"Index {index_name} not found when attempting to load data in the {self.__instance_name} instance")
            else:
                response.raise_for_status()
        except Exception as e:
            e.add_note("CONTEXT: Error happened when loading the data into the index")
            raise
    
    #@measure_index_execution_time
    def process_all_in_one(self, index_name, doc, fields, metadata):
        self.delete_index(index_rm=index_name)
        self.create_index(index_name=index_name, fields=fields)
        if metadata is None:
            for i in range(0,len(doc),700):
                load_batch = {"value": doc[i:i+700]}
                json_load_batch = json.dumps(load_batch)
                self.load_index_general(index_name=index_name,doc=json_load_batch)
                print(len(doc[i:i+700])," documents loaded")
        else:
            self.load_index(metadata=metadata, index_name=index_name, doc=doc)

class GetEmbeddings:

    def get_embeddings(self,list_dict):
        azure_openai_embedding_deployment = secret_client.get_secret("ce-assistant-open-ai-embedding-deployment").value
        azure_openai_embedding_model = secret_client.get_secret("ce-assistant-open-ai-embedding-model").value
        api_key = secret_client.get_secret("ce-assistant-apim-key").value
        azure_endpoint = secret_client.get_secret("ce-assistant-apim-endpoint").value
        api_version = secret_client.get_secret("ce-assistant-open-ai-version").value

        llm = AzureOpenAI(
        azure_deployment=azure_openai_embedding_deployment,
        api_version=api_version,
        azure_endpoint=azure_endpoint,
        api_key=api_key,
        ) 
        for i in range(0, len(list_dict), 500):
            if i + 500 <= len(list_dict):
                batch = list_dict[i:i+500]
            else:
                batch = list_dict[i:]

            content = [item['embedding_text'] for item in batch]
            content_response = llm.embeddings.create(input=content, model=azure_openai_embedding_model)
            content_embeddings = [item.embedding for item in content_response.data]
            for j, item in enumerate(batch):
                item['embedding'] = content_embeddings[j]
                
        return list_dict