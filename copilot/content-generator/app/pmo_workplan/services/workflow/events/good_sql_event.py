import pandas as pd
from llama_index.core.workflow import Event


class GoodSQLEvent(Event):
    sql_output: pd.DataFrame
