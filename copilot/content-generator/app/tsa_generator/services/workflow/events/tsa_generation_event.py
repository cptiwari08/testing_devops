from llama_index.core.workflow import Event


class TSAGenerationEvent(Event):
    result: str
