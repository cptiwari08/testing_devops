from llama_index.core.workflow import Event


class NextStepsRetrieveSQLQueries(Event):
    project_team: str
    start_date: str
    end_date: str
