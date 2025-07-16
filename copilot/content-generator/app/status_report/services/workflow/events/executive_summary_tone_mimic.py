from llama_index.core.workflow import Event


class ExecuteSummaryToneMimic(Event):
    project_team: str
    source: str
    response: str
    citing_sources: list
    num_words: int
