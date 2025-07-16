from app.core.config import EmbeddingsConfig, OpenAIConfig
from app.core.interfaces import IAzureModel
from deepeval.models.base_model import DeepEvalBaseLLM
from llama_index.embeddings.azure_openai import AzureOpenAIEmbedding
from llama_index.llms.azure_openai import AsyncAzureOpenAI, AzureOpenAI


class AzureSyncLLM(IAzureModel):
    """
    This class represents a synchronous Azure Language Model.
    It inherits from the IAzureModel interface.
    """

    def __init__(self, callback_manager) -> None:
        super().__init__()

        self._model = AzureOpenAI(
            callback_manager=callback_manager,
            model=OpenAIConfig.model,  # type: ignore
            engine=OpenAIConfig.engine,
            deployment_name=OpenAIConfig.deployment_name,
            api_key=OpenAIConfig.api_key,
            azure_endpoint=OpenAIConfig.azure_endpoint,
            num_tokens=OpenAIConfig.num_tokens,
            temperature=0.000001,
            api_version=OpenAIConfig.version,
            default_headers={
                "Ocp-Apim-Subscription-Key": OpenAIConfig.api_key,
            },
        )

    def get_model(self) -> AzureOpenAI:
        """
        Returns the AzureOpenAI model instance.
        """
        return self._model


class AzureAsyncLLM(IAzureModel):
    """
    This class represents a synchronous Azure Language Model.
    It inherits from the IAzureModel interface.
    """

    def __init__(self) -> None:
        super().__init__()

        self._model = AsyncAzureOpenAI(
            api_key=OpenAIConfig.api_key,
            azure_endpoint=OpenAIConfig.azure_endpoint,
            api_version=OpenAIConfig.version,
            default_headers={
                "Ocp-Apim-Subscription-Key": OpenAIConfig.api_key,
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

    def __init__(self) -> None:
        super().__init__()

        self._model = AzureOpenAIEmbedding(
            model=EmbeddingsConfig.model,  # type: ignore
            deployment_name=EmbeddingsConfig.deployment_name,
            api_key=EmbeddingsConfig.api_key,
            azure_endpoint=EmbeddingsConfig.azure_endpoint,
            api_version=EmbeddingsConfig.version,
            default_headers={
                "Ocp-Apim-Subscription-Key": OpenAIConfig.api_key,
            },
        )

    def get_model(self) -> AzureOpenAIEmbedding:
        """
        Returns the AzureOpenAIEmbedding model instance.
        """
        return self._model


class CustomDeepEvalOpenAIModel(DeepEvalBaseLLM):
    def __init__(self):
        self.model = AzureOpenAI(
            model=OpenAIConfig.model,  # type: ignore
            engine=OpenAIConfig.engine,
            deployment_name=OpenAIConfig.deployment_name,
            api_key=OpenAIConfig.api_key,
            azure_endpoint=OpenAIConfig.azure_endpoint,
            num_tokens=OpenAIConfig.num_tokens,
            temperature=0.000001,
            api_version=OpenAIConfig.version,
            default_headers={
                "Ocp-Apim-Subscription-Key": OpenAIConfig.api_key,
            },
        )

    def load_model(self):
        return self.model

    def generate(self, prompt: str) -> str:
        completion = self.model.chat.completions.create(  # type: ignore
            model=OpenAIConfig.deployment_name,
            messages=[
                {
                    "role": "user",
                    "content": prompt,
                },
            ],
        )
        return completion.choices[0].message.content

    async def a_generate(self, prompt: str) -> str:
        llm = AsyncAzureOpenAI(
            api_version=OpenAIConfig.version,
            api_key=OpenAIConfig.api_key,
            azure_endpoint=OpenAIConfig.azure_endpoint,  # type: ignore
        )
        completion = await llm.chat.completions.create(
            model=OpenAIConfig.deployment_name,  # type: ignore
            messages=[
                {
                    "role": "user",
                    "content": prompt,
                },
            ],
        )
        return completion.choices[0].message.content  # type: ignore

    def get_model_name(self):
        return "Custom Azure OpenAI Model (Llamaindex)"
