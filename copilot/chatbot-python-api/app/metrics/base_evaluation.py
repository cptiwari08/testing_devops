from abc import abstractmethod

from app.core.pydantic_models import MetricInput, Score


class Evaluation:
    @abstractmethod
    async def run(self, metric_input: MetricInput) -> Score | None:
        pass
