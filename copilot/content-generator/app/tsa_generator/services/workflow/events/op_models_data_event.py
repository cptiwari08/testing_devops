from llama_index.core.workflow import Event


class OPModelsDataEvent(Event):
    result: list
