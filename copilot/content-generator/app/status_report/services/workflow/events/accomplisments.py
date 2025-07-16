from llama_index.core.workflow import Event


class Accomplishments(Event):
    project_team: str
    es_queries_result: str
    citing_sources: list
