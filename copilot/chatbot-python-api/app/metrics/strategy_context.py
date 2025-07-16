from app.core.config import ResponseOptions
from app.core.pydantic_models import MetricInput, Score
from app.metrics.base_evaluation import Evaluation


class Context:

    def __init__(self, strategy: Evaluation) -> None:
        self._strategy = strategy

    @property
    def strategy(self) -> Evaluation:
        return self._strategy

    @strategy.setter
    def strategy(self, strategy: Evaluation) -> None:
        self._strategy = strategy

    async def run(self, metric_input: MetricInput, avoid_bypass=False) -> Score | None:
        if (ResponseOptions.calculate_scores.lower() in {"true"}) or avoid_bypass:
            result = await self._strategy.run(metric_input)
            return result
