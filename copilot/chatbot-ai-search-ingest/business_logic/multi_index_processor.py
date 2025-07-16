import os
from llama_index.core import Document
from data_access.db_connection import DBConnection
from data_access.ai_search_collection import AISearchCollection
from utils.field_handler import FieldHandler
from business_logic.settings import DynamicSettings
from utils.logger import Logger
from utils.functions import *

class MultiIndexProcessor:
    def __init__(self):
        self.settings = DynamicSettings(directory='business_logic')
        self.config_id = 3
        self.log = Logger()

    def load_data(self):
        try:
            data1 = DBConnection.execute_query({"SqlQuery": self.settings.get_variable(self.config_id, 'query1'), "tables": []})
            data2 = DBConnection.execute_query({"SqlQuery": self.settings.get_variable(self.config_id, 'query2'), "tables": []})
            data3 = DBConnection.execute_query({"SqlQuery": self.settings.get_variable(self.config_id, 'query3'), "tables": []})
            data4 = DBConnection.execute_query({"SqlQuery": self.settings.get_variable(self.config_id, 'query4'), "tables": []})
            data5 = DBConnection.execute_query({"SqlQuery": self.settings.get_variable(self.config_id, 'query5'), "tables": []})
            return data1, data2, data3, data4, data5
        except Exception as e:
            self.log.error(f"Failed to load data - {str(e)}")
            raise e

    def process_index1(self, data1, data2):
        try:
            data_false = processing_data_index(data1)
            data_suggestions = DBConnection.suggestions()
            suggestions = processing_data_suggestions(data_suggestions)
            final_second_index = processing_data_index_1(data2, suggestions, data_false)
            
            documents = [
                Document(
                    text=row_processor(row),
                    metadata_template="{key}: {value}",
                    metadata_separator="\n",
                    metadata={"table_name": row.TableName, "isMainTable": row.isMainTable},
                )
                for _, row in final_second_index.iterrows()
            ]
            return documents
        except Exception as e:
            self.log.error(f"Failed to process index 1 - {str(e)}")
            raise e

    def process_index2(self, data3, data4, data5):
        try:
            glossary = DBConnection.get_glossary()
            glossary = processing_data_index_2(glossary)
            
            lookup_info = data3
            lookup_values_query = processing_lookup_query(lookup_info)
            lookup_values = DBConnection.execute_query({"SqlQuery": lookup_values_query, "tables": []})
            lkup_dictionary = {item['lkup']: item['main_table'] for item in lookup_info}
            glossary = processing_lookup_values(lookup_values, lkup_dictionary, glossary)
            
            glossary = processing_lkup_query2(data4, glossary)
            glossary = processing_lkup_query3(data5, glossary)

            documents = [
                Document(
                    text=row["context"],
                    metadata_template="{key}: {value}",
                    metadata_seperator="\n",
                    metadata={"tableName": row["tableName"], 
                            "tag": row["tag"],
                            "isSchema":row['isSchema']},
                )
                for row in glossary
            ]
            return documents
        except Exception as e:
            self.log.error(f"Failed to process index 2 - {str(e)}")
            raise e

    def process_index3(self):
        try:
            suggestions = processing_suggestions(DBConnection.suggestions())
            documents = [
                Document(
                    text=row["question"],
                    metadata_template="{key}: {value}",
                    metadata_seperator="\n",
                    metadata={
                        "sqlQuery": row["sqlQuery"],
                        "source": row["source"],
                        "appAffinity": row["appAffinity"],
                        "idSuggestion": row["id"],
                        "visibleToAssistant": row["visibleToAssistant"]
                    },
                    excluded_embed_metadata_keys=['sqlQuery','source','appAffinity','idSuggestion','visibleToAssistant']
                )
                for row in suggestions
            ]
            return documents
        except Exception as e:
            self.log.error(f"Failed to process index 3 - {str(e)}")
            raise e

    def index_documents(self, documents, index_type):
        try:
            if index_type == 1:
                metadata_fields = {"table_name": "table_name", "isMainTable": "isMainTable"}
                field_handler = FieldHandler()
                field_handler.add_str_field({"name": "table_name", "retrievable": True, "filterable": True, "searchable": True})
                field_handler.add_str_field({"name": "isMainTable", "retrievable": True, "filterable": True})
                index_name = os.getenv("PRJDATA_INDEX")
            
            elif index_type == 2:
                metadata_fields = {"tableName": "tableName", "tag": "tag", "isSchema":"isSchema"}
                field_handler = FieldHandler()
                field_handler.add_str_field({"name": "tableName", "retrievable": True, "filterable": True, "searchable": True})
                field_handler.add_str_field({"name": "tag", "retrievable": True, "filterable": True})
                field_handler.add_str_field({"name": "isSchema", "retrievable": True, "filterable": True})
                index_name = os.getenv("PRJGLOSS_INDEX")
            
            elif index_type == 3:
                metadata_fields = {
                    "sqlQuery": "sqlQuery",
                    "source": "source",
                    "appAffinity": "appAffinity",
                    "idSuggestion": "idSuggestion",
                    "visibleToAssistant": "visibleToAssistant"
                }
                field_handler = FieldHandler()
                field_handler.add_str_field({"name": "sqlQuery", "retrievable": True, "filterable": True})
                field_handler.add_str_field({"name": "source", "retrievable": True, "filterable": True})
                field_handler.add_str_field({"name": "appAffinity", "retrievable": True, "filterable": True})
                field_handler.add_int_field({"name": "idSuggestion", "retrievable": True, "filterable": True})
                field_handler.add_bool_field({"name": "visibleToAssistant", "retrievable": True, "filterable": True})
                index_name = os.getenv("PRJDATA_SUGGESTION_INDEX")

            field_handler.add_str_field({"name": "metadata", "retrievable": True})
            field_handler.add_str_field({"name": "doc_id", "retrievable": True, "filterable": True})
            
            ai_search_collection = AISearchCollection()
            ai_search_collection.process_all_instances(
                index_name=index_name,
                doc=documents,
                fields=field_handler.fields,
                metadata=metadata_fields
            )
        except Exception as e:
            self.log.error(f"Failed to index documents for index {index_type} - {str(e)}")
            raise e

    def run(self):
        try:
            self.log.info("Starting project data processing")
            data1, data2, data3, data4, data5 = self.load_data()
            
            # Process and index the first index
            documents1 = self.process_index1(data1, data2)
            self.index_documents(documents1, 1)
            
            # Process and index the second index
            documents2 = self.process_index2(data3, data4, data5)
            self.index_documents(documents2, 2)
            
            # Process and index the third index
            documents3 = self.process_index3()
            self.index_documents(documents3, 3)
            
            return True
        except Exception as e:
            self.log.error(f"Error in project data process: {str(e)}")
            raise e
