from app.core.azure_llm_models import CustomDeepEvalOpenAIModel
from app.core.pydantic_models import MetricInput, Score
from app.metrics.base_evaluation import Evaluation
from app.core.prompt_manager import create_prompt_manager
from deepeval.metrics import GEval
from deepeval.test_case import LLMTestCase, LLMTestCaseParams


class SQLEvaluator(Evaluation):
    def __init__(self, logger=None):
        """Initialize with the prompt manager"""
        super().__init__()
        self._logger = logger
        self._prompt_manager = create_prompt_manager(logger)
        
    async def run(self, metric_input: MetricInput) -> Score | None:
        name = "sql_evaluator"
        
        # Get evaluation steps from prompt manager
        steps = await self._prompt_manager.get_prompt(
            agent="metrics",
            key="sql_evaluator_noinput_steps",
            raw_data=True
        )
        
        test_case = LLMTestCase(
            input=metric_input.user_input,
            actual_output=metric_input.llm_response,
        )
        metric = GEval(
            name=name,
            evaluation_steps=steps,
            evaluation_params=[
                LLMTestCaseParams.ACTUAL_OUTPUT,
                LLMTestCaseParams.INPUT,
            ],
            model=CustomDeepEvalOpenAIModel(),
        )
        await metric.a_measure(test_case)
        return Score(metric=name, value=round(metric.score, 2), reason=metric.reason)  # type: ignore
