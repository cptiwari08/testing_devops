from app.ey_ip.tools.base import EYIPBaseTool
from llama_index.core.tools import FunctionTool


class DummyTool(EYIPBaseTool):
    """
    This is a Dummy tool so questions that do not need a tool are caught here.
    It inherits from the EYIPBaseTool base class.
    """

    def __init__(self, logger, llm_models) -> None:
        super().__init__(logger, llm_models)
        self._logger.info("Initializing DummyTool")

    def generate(self) -> FunctionTool:
        def dummy_fallback_function():
            return True

        tool = FunctionTool.from_defaults(dummy_fallback_function)
        self._logger.info("DummyTool generated")
        return tool
