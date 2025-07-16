from abc import ABC, abstractmethod

from llama_index.embeddings.azure_openai import AzureOpenAIEmbedding
from llama_index.llms.azure_openai import AzureOpenAI


class IBaseLogger:
    @abstractmethod
    def debug(cls, message: str):
        pass

    @abstractmethod
    def info(cls, message: str):
        pass

    @abstractmethod
    def warning(cls, message: str):
        pass

    @abstractmethod
    def error(cls, message: str):
        pass

    @abstractmethod
    def critical(cls, message: str):
        pass

    @abstractmethod
    def set_unique_identifiers(
        self, instance_id: str, chat_id: str, project_friendly_id: str
    ):
        pass

class IAzureModel(ABC):

    @abstractmethod
    def get_model(self) -> AzureOpenAIEmbedding | AzureOpenAI:
        pass
