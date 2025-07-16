from llama_index.core.workflow import Event


class PostprocessProjectDocsEvent(Event):
    result: list