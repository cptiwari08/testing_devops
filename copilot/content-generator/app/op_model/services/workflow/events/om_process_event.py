from llama_index.core.workflow import Event


class OMProcessEvent(Event):
    result: list
