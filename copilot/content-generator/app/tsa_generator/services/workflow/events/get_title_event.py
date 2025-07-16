from llama_index.core.workflow import Event


class GetTitleEvent(Event):
    title: str
