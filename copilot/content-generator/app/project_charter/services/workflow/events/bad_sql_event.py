from llama_index.core.workflow import Event


class BadSQLEvent(Event):
    sql_generated: str
    feedback: str
