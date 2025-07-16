from llama_index.core.workflow import Event


class SearchDocsEvent(Event):
    search_results: list
