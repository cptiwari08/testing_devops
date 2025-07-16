from llama_index.core.workflow import Event


class NextSteps(Event):
    project_team: str
    es_queries_result: str
    citing_sources: list
