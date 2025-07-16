from llama_index.core.workflow import Event


class OverallStatusRetrieveSQLQueries(Event):
    project_team: str
    start_date: str
    end_date: str
