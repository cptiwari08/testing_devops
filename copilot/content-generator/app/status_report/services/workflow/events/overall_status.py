from llama_index.core.workflow import Event


class OverallStatus(Event):
    project_team: str
    sql_results: list
    citing_sources: list
