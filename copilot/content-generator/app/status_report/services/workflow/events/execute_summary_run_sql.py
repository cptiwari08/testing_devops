from llama_index.core.workflow import Event


class ExecutiveSummaryRunSQL(Event):
    query_and_rules: dict
    project_team: str
    table: str
    start_date: str
    end_date: str
