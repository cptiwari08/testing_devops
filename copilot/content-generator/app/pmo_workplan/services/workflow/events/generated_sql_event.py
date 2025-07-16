from app.core.asset_manager import AssetManager
from llama_index.core.workflow import Event


class GeneratedSQLEvent(Event):
    sql_generated: str
    query_params: dict
    sql_object: AssetManager
