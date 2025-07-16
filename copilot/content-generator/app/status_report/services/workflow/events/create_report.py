from llama_index.core.workflow import Event


class CreateReport(Event):
    data: dict
    project_team: str
    start_date: str
    end_date: str
