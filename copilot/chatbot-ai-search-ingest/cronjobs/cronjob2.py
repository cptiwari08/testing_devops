import os
import sys
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
import pandas as pd
import pickle
import luigi
from llama_index.core import Document
from data_access.db_connection import DBConnection
from data_access.ai_search_collection import AISearchCollection
from utils.field_handler import FieldHandler
from business_logic.settings import DynamicSettings
from utils.logger import Logger
from utils.cleanup_temp import clear_temp_files

# Settings
settings = DynamicSettings(directory='business_logic')
config_id = 2
log = Logger()

class LoadDataFromSQLTask(luigi.Task):
    query = luigi.Parameter(default=settings.get_variable(config_id, 'query'))
    
    def output(self):
        return luigi.LocalTarget('temp/data.pkl')

    def run(self):
        try:
            engine = DBConnection.create_ey_ip_db_connection()
            with engine.begin() as conn:
                data = pd.read_sql(self.query, conn)
                with open(self.output().path, 'wb') as f:
                    pickle.dump(data, f)
            log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e

class DocumentProcessingTask(luigi.Task):
    def requires(self):
        return LoadDataFromSQLTask()

    def output(self):
        return luigi.LocalTarget('temp/processed_documents.pkl')

    def run(self):
        try:
            with open(self.input().path, 'rb') as f:
                data = pickle.load(f)
            
            documents = [
                Document(
                    text=row.text,
                    metadata_template="{key}: {value}",
                    metadata_separator="\n",
                    metadata={"tableName": row.table, "tag": row.tag}
                )
                for _, row in data.iterrows()
            ]
            
            with open(self.output().path, 'wb') as f:
                pickle.dump(documents, f)
            log.info("DocumentProcessingTask: Documents successfully processed")
        except Exception as e:
            log.error(f"Error in DocumentProcessingTask: Failed to process documents - {str(e)}")
            raise e

class IndexMetadataTask(luigi.Task):
    def requires(self):
        return DocumentProcessingTask()

    def run(self):
        try:
            with open(self.input().path, 'rb') as f:
                documents = pickle.load(f)
            
            metadata_fields = {"tableName": "tableName", "tag": "tag"}
            
            field_handler = FieldHandler()
            for field in settings.get_variable(config_id, 'field_handler'):
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
            
        except Exception as e:
            log.error(f"Error in IndexMetadataTask: Failed to index metadata - {str(e)}")
            
            raise e

if __name__ == "__main__":
    try:
        luigi.build([IndexMetadataTask()], local_scheduler=True)
        clear_temp_files()
    except Exception as e:
        log.error(f"Main: Failed to run Luigi pipeline - {str(e)}")
        clear_temp_files()
        raise e