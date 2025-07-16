from llama_index.core.workflow import Event


class WPTeamTasksEvent(Event):
    result: str
