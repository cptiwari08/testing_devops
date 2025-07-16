from llama_index.core.workflow import Event


class ScopeGenerationEvent(Event):
    result: str
