from llama_index.core.workflow import Event


class EYIPTemplatesEvent(Event):
    result: str
