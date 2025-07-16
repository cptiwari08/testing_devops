from llama_index.core.workflow import Event


class SampleTasksEvent(Event):
    result: str
