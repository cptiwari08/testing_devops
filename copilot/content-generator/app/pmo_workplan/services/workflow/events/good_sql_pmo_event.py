from llama_index.core.workflow import Event


class GoodSQLPMOEvent(Event):
    sql_output: dict
