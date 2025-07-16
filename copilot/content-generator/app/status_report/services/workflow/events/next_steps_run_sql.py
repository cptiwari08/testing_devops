from llama_index.core.workflow import Event


class NextStepsRunSQL(Event):
    query_and_rules: dict
    table: str
    project_team: str
    start_date: str
    end_date: str
