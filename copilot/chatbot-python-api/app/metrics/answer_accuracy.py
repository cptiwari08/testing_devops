from app.core.azure_llm_models import CustomDeepEvalOpenAIModel
from app.core.pydantic_models import MetricInput, Score
from app.metrics.base_evaluation import Evaluation
from app.core.prompt_manager import create_prompt_manager
from deepeval.metrics import GEval
from deepeval.test_case import LLMTestCase, LLMTestCaseParams


class AnswerAccuracy(Evaluation):
    def __init__(self, logger=None):
        """Initialize with the prompt manager"""
        super().__init__()
        self._logger = logger
        self._prompt_manager = create_prompt_manager(logger)
    
    async def run(self, metric_input: MetricInput) -> Score | None:
        name = "answer_accuracy"
        
        # Get evaluation steps from prompt manager
        steps = await self._prompt_manager.get_prompt(
            agent="metrics",
            key="answer_accuracy_steps",
            raw_data=True
        )
        
        if metric_input.project_description:
            steps[0] = (
                f"Evaluate if the response is related to the following project description: {metric_input.project_description}."
            )

        test_case = LLMTestCase(
            input=metric_input.user_input,
            actual_output=metric_input.llm_response,
        )
        metric = GEval(
            name=name,
            threshold=metric_input.threshold,
            model=CustomDeepEvalOpenAIModel(),
            evaluation_steps=steps,
            evaluation_params=[LLMTestCaseParams.INPUT, LLMTestCaseParams.ACTUAL_OUTPUT],
        )
        await metric.a_measure(test_case)
        return Score(metric=name, value=round(metric.score, 2), reason=metric.reason)  # type: ignore
