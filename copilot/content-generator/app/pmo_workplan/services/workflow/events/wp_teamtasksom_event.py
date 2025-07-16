from llama_index.core.workflow import Event


class WPTeamTasksOMEvent(Event):
    result: str
