from llama_index.core.workflow import Event


class GetQueries(Event):
    sections: dict
    tables: dict
    project_team: str
    start_date: str
    end_date: str
