from abc import abstractmethod

from app.core.interfaces import IBaseLogger, ILlamaBaseTool
from app.core.pydantic_models import LLMModels
from app.core.singleton_meta import SingletonMeta
from llama_index.core.tools import FunctionTool, QueryEngineTool


class EYIPBaseTool(ILlamaBaseTool, metaclass=SingletonMeta):
    """
    A base class for EY IP tools.
    This class provides a method to generate a
    QueryEngineTool and a method to create a tables index.
    It inherits from the ILlamaBaseTool interface.
    """

    def __init__(self, logger: IBaseLogger, llm_models: LLMModels):
        self._logger = logger
        self.llm_models = llm_models

    @abstractmethod
    def generate(self) -> FunctionTool | QueryEngineTool:
        pass
