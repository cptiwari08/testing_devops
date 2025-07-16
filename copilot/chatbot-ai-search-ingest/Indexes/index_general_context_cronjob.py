import os
import sys

import pandas as pd
from llama_index.core import Document

from index_core_functions import DBConnection, AISearchCollection, log, FieldHandler

log.info("Starting general context index update")

try:
    engine = DBConnection.create_ey_ip_db_connection()
    with engine.begin() as conn:
        data = pd.read_sql("SELECT * FROM _generalContext;", conn)

    # Loading documents from SQL
    doc = [
        Document(
            text=row.text,
            metadata_template="{key}: {value}",
            metadata_seperator="\n",
            metadata={"tableName": row.table, "tag": row.tag},
        )
        for _, row in data.iterrows()
    ]

    index_name = os.getenv("INDEX_NAME_EYIP")

    metadata_fields = {"tableName": "tableName", "tag": "tag"}

    field_handler = FieldHandler()
    field_handler.add_str_field({"name": "tableName", "retrievable": True, "filterable": True, "searchable": True})
    field_handler.add_str_field({"name": "tag", "retrievable": True, "filterable": True})

    ai_search_collection = AISearchCollection()
    ai_search_collection.process_all_instances(
        metadata=metadata_fields,
        index_name=index_name,
        doc=doc,
        fields=field_handler.fields
    )
except Exception as e:
    log.error(e)
    sys.exit()
