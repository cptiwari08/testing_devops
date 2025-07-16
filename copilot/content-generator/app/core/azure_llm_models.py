from app.core.config import EmbeddingsConfig, OpenAIConfig
from app.core.interfaces import IAzureModel
from app.core.llama_handlers import LoggerHandler
from llama_index.core.callbacks import CallbackManager
from llama_index.embeddings.azure_openai import AzureOpenAIEmbedding
from llama_index.llms.azure_openai import AzureOpenAI


class AzureSyncLLM(IAzureModel):
    """
    This class represents a synchronous Azure Language Model.
    It inherits from the IAzureModel interface.
    """

    def __init__(
        self,
        openai_config: OpenAIConfig,
        logger_handler: LoggerHandler,
    ) -> None:
        super().__init__()
        self.openai_config = openai_config

        self._model = AzureOpenAI(
            callback_manager=CallbackManager([logger_handler]),
            model=self.openai_config.model,  # type: ignore
            engine=self.openai_config.engine,
            deployment_name=self.openai_config.deployment_name,
            api_key=self.openai_config.api_key,
            azure_endpoint=self.openai_config.azure_endpoint,
            num_tokens=self.openai_config.num_tokens,
            temperature=0.000001,
            api_version=self.openai_config.version,
            max_tokens=16384,
            default_headers={
                "Ocp-Apim-Subscription-Key": self.openai_config.api_key,
            },
        )

    def get_model(self) -> AzureOpenAI:
        """
        Returns the AzureOpenAI model instance.
        """
        return self._model


class AzureEmbeddings(IAzureModel):
    """
    This class represents an Azure Embeddings model.
    It inherits from the IAzureModel interface.
    """

    def __init__(self, embeddings_config: EmbeddingsConfig) -> None:
        super().__init__()
        self.embeddings_config = embeddings_config

        self._model = AzureOpenAIEmbedding(
            model=self.embeddings_config.model,  # type: ignore
            deployment_name=self.embeddings_config.deployment_name,
            api_key=self.embeddings_config.api_key,
            azure_endpoint=self.embeddings_config.azure_endpoint,
            api_version=self.embeddings_config.version,
            default_headers={
                "Ocp-Apim-Subscription-Key": self.embeddings_config.api_key,
            },
        )

    def get_model(self) -> AzureOpenAIEmbedding:
        """
        Returns the AzureOpenAIEmbedding model instance.
        """
        return self._model
