from llama_index.core.workflow import Event


class SearchTeamEvent(Event):
    project_team: list[str]
