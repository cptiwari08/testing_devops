import os
import pandas as pd
import pickle
from llama_index.core import Document
from data_access.db_connection import DBConnection
from data_access.ai_search_collection import AISearchCollection
from utils.field_handler import FieldHandler
from business_logic.settings import DynamicSettings
from utils.logger import Logger

class MetadataProcessor:
    def __init__(self):
        self.settings = DynamicSettings(directory='business_logic')
        self.config_id = 2
        self.log = Logger()

    def load_data(self):
        query = self.settings.get_variable(self.config_id, 'query')
        try:
            engine = DBConnection.create_ey_ip_db_connection()
            with engine.begin() as conn:
                data = pd.read_sql(query, conn)
            self.log.info("Data successfully loaded from SQL")
            return data
        except Exception as e:
            self.log.error(f"Failed to load data from SQL - {str(e)}")
            raise e

    def process_documents(self, data):
        try:
            documents = [
                Document(
                    text=row.text,
                    metadata_template="{key}: {value}",
                    metadata_separator="\n",
                    metadata={"tableName": row.table, "tag": row.tag}
                )
                for _, row in data.iterrows()
            ]
            self.log.info("Documents successfully processed")
            return documents
        except Exception as e:
            self.log.error(f"Failed to process documents - {str(e)}")
            raise e

    def index_metadata(self, documents):
        try:
            metadata_fields = {"tableName": "tableName", "tag": "tag"}
            
            field_handler = FieldHandler()
            for field in self.settings.get_variable(self.config_id, 'field_handler'):
                field_handler.add_str_field(field)
            field_handler.add_str_field({"name": "metadata", "retrievable": True})
            field_handler.add_str_field({"name": "doc_id", "retrievable": True, "filterable": True})
            
            index_name = os.getenv("INDEX_NAME_EYIP")
            ai_search_collection = AISearchCollection()
            ai_search_collection.process_all_instances(
                index_name=index_name,
                doc=documents,
                fields=field_handler.fields,
                metadata=metadata_fields
            )
            self.log.info("Metadata successfully indexed")
        except Exception as e:
            self.log.error(f"Failed to index metadata - {str(e)}")
            raise e

    def run(self):
        try:
            self.log.info("Starting metadata processing")
            data = self.load_data()
            documents = self.process_documents(data)
            self.index_metadata(documents)
            return True
        except Exception as e:
            self.log.error(f"Error in metadata process: {str(e)}")
            raise e
