from llama_index.core.workflow import Event


class NextStepsResults(Event):
    project_team: str
    source: str
    response: str
    rules: str
    source_value: dict
    table: str
