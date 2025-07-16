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
from utils.functions import *
from utils.cleanup_temp import clear_temp_files

# Settings
settings = DynamicSettings(directory='business_logic')
config_id = 3
log = Logger()

class LoadDataFromSQLTask1(luigi.Task):
    query1 = luigi.Parameter(default=settings.get_variable(config_id, 'query1'))
    def output(self):
        return luigi.LocalTarget('temp/data1.pkl')

    def run(self):

        try:
            data = DBConnection.execute_query({"SqlQuery": self.query1, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e

class LoadDataFromSQLTask2(luigi.Task):
    query2 = luigi.Parameter(default=settings.get_variable(config_id, 'query2'))
    def output(self):
        return luigi.LocalTarget('temp/data2.pkl')

    def run(self):
        
        try:
            data = DBConnection.execute_query({"SqlQuery": self.query2, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e
        
class LoadDataFromSQLTask3(luigi.Task):
    query3 = luigi.Parameter(default=settings.get_variable(config_id, 'query3'))

    def output(self):
        return luigi.LocalTarget('temp/data3.pkl')

    def run(self):
        
        try:
            data = DBConnection.execute_query({"SqlQuery": self.query3, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e
        
class LoadDataFromSQLTask4(luigi.Task):
    query4 = luigi.Parameter(default=settings.get_variable(config_id, 'query4'))

    def output(self):
        return luigi.LocalTarget('temp/data4.pkl')

    def run(self):
        
        try:
            data = DBConnection.execute_query({"SqlQuery": self.query4, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e
        
class LoadDataFromSQLTask5(luigi.Task):
    query5 = luigi.Parameter(default=settings.get_variable(config_id, 'query5'))

    def output(self):
        return luigi.LocalTarget('temp/data5.pkl')

    def run(self):
        
        try:
            data = DBConnection.execute_query({"SqlQuery": self.query5, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e


class DocumentProcessingTaskIndex1(luigi.Task):
    def requires(self):
        return [LoadDataFromSQLTask1(), LoadDataFromSQLTask2()]

    def output(self):
        return luigi.LocalTarget('temp/processed_documents1.pkl')

    def run(self):
        input_files = self.input()
        try:
            with open(self.input()[0].path, 'rb') as in_file:
                data = pickle.load(in_file)
            data_false = processing_data_index(data)
            data_suggestions = DBConnection.suggestions()
            suggestions = processing_data_suggestions(data_suggestions) 

            with open(self.input()[1].path, 'rb')  as in_file:
                data = pickle.load(in_file)
            final_second_index =processing_data_index_1(data, suggestions,data_false)

            documents = [
                Document(
                    text=row_processor(row),
                    metadata_template="{key}: {value}",
                    metadata_separator="\n",
                    metadata={"table_name": row.TableName, "isMainTable": row.isMainTable},
                )
                for _, row in final_second_index.iterrows()
            ]

            with open(self.output().path, 'wb') as f:
                pickle.dump(documents, f)
            log.info("DocumentProcessingTask: Documents successfully processed")
        except Exception as e:
            log.error(f"Error in DocumentProcessingTask: Failed to process documents - {str(e)}")
            raise e

class DocumentProcessingTaskIndex2(luigi.Task):
    def requires(self):
        return [LoadDataFromSQLTask3(), LoadDataFromSQLTask4(), LoadDataFromSQLTask5()]

    def output(self):
        return luigi.LocalTarget('temp/processed_documents2.pkl')

    def run(self):
        input_files = self.input()
        try:
            with open(self.input()[0].path, 'rb') as in_file:
                data = pickle.load(in_file)
                #print("la data es",data)
                #print(type(data))

            glossary = DBConnection.get_glossary()
            glossary = processing_data_index_2(glossary)
            lookup_info = data
            lookup_values_query = processing_lookup_query(lookup_info)
            lookup_values = DBConnection.execute_query({"SqlQuery": lookup_values_query, "tables": []})
            print(lookup_values)
            lkup_dictionary = {item['lkup']: item['main_table'] for item in lookup_info}
            glossary = processing_lookup_values(lookup_values,lkup_dictionary,glossary)
            print(glossary)
            with open(self.input()[1].path, 'rb') as in_file:
                data = pickle.load(in_file)
            glossary = processing_lkup_query2(data, glossary)
            with open(self.input()[2].path, 'rb') as in_file:
                data = pickle.load(in_file)
            glossary = processing_lkup_query3(data, glossary)

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

            with open(self.output().path, 'wb') as f:
                pickle.dump(documents, f)
            log.info("DocumentProcessingTask: Documents successfully processed")
        except Exception as e:
            log.error(f"Error in DocumentProcessingTask: Failed to process documents - {str(e)}")
            raise e
#
class DocumentProcessingTaskIndex3(luigi.Task):
    def output(self):
        return luigi.LocalTarget('temp/processed_documents3.pkl')

    def run(self):
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

            with open(self.output().path, 'wb') as f:
                pickle.dump(documents, f)
            log.info("DocumentProcessingTask: Documents successfully processed")
        except Exception as e:
            log.error(f"Error in DocumentProcessingTask: Failed to process documents - {str(e)}")
            raise e
#
#
#
class IndexMetadataTask1(luigi.Task):
    def requires(self):
        return DocumentProcessingTaskIndex1()

    def run(self):
        try:
            with open(self.input().path, 'rb') as f:
                documents = pickle.load(f)
            
            metadata_fields_project_specific_index1 = {"table_name": "table_name", "isMainTable": "isMainTable"}
            field_handler_1 = FieldHandler()
            field_handler_1.add_str_field({"name": "table_name", "retrievable": True, "filterable": True, "searchable": True})
            field_handler_1.add_str_field({"name": "isMainTable", "retrievable": True, "filterable": True})
            field_handler_1.add_str_field({"name": "metadata", "retrievable": True})
            field_handler_1.add_str_field({"name": "doc_id", "retrievable": True, "filterable": True})
            project_specific_index1 = os.getenv("PRJDATA_INDEX")
            ai_search_collection = AISearchCollection()
            ai_search_collection.process_all_instances(
                metadata=metadata_fields_project_specific_index1,
                index_name=project_specific_index1,
                doc=documents,
                fields=field_handler_1.fields
            )
        except Exception as e:
            log.error(f"Error in IndexMetadataTask: Failed to index metadata - {str(e)}")
            raise e
#
class IndexMetadataTask2(luigi.Task):
    def requires(self):
        return DocumentProcessingTaskIndex2()

    def run(self):
        try:
            with open(self.input().path, 'rb') as f:
                documents = pickle.load(f)
            
            metadata_fields_project_specific_index2 = {"tableName": "tableName", "tag": "tag","isSchema":"isSchema"}
            field_handler_2 = FieldHandler()
            field_handler_2.add_str_field({"name": "tableName", "retrievable": True, "filterable": True, "searchable": True})
            field_handler_2.add_str_field({"name": "tag", "retrievable": True, "filterable": True})
            field_handler_2.add_str_field({"name": "isSchema", "retrievable": True, "filterable": True})
            field_handler_2.add_str_field({"name": "metadata", "retrievable": True})
            field_handler_2.add_str_field({"name": "doc_id", "retrievable": True, "filterable": True})
            project_specific_index2 = os.getenv("PRJGLOSS_INDEX")
            ai_search_collection = AISearchCollection()
            ai_search_collection.process_all_instances(
                index_name=project_specific_index2,
                doc=documents,
                fields=field_handler_2.fields,
                metadata=metadata_fields_project_specific_index2
                
            )
            
        except Exception as e:
            log.error(f"Error in IndexMetadataTask: Failed to index metadata - {str(e)}")
            raise e
#        
class IndexMetadataTask3(luigi.Task):
    def requires(self):
        return DocumentProcessingTaskIndex3()

    def run(self):
        try:
            with open(self.input().path, 'rb') as f:
                documents = pickle.load(f)
            
            metadata_fields_project_specific_index3 = {
                    "sqlQuery": "sqlQuery",
                    "source": "source",
                    "appAffinity": "appAffinity",
                    "idSuggestion" : "idSuggestion",
                    "visibleToAssistant": "visibleToAssistant"
                }
            field_handler_3 = FieldHandler()
            field_handler_3.add_str_field({"name": "sqlQuery", "retrievable": True, "filterable": True})
            field_handler_3.add_str_field({"name": "source", "retrievable": True, "filterable": True})
            field_handler_3.add_str_field({"name": "appAffinity", "retrievable": True, "filterable": True})
            field_handler_3.add_int_field({"name": "idSuggestion", "retrievable": True, "filterable": True})
            field_handler_3.add_bool_field({"name": "visibleToAssistant", "retrievable": True, "filterable": True})
            field_handler_3.add_str_field({"name": "metadata", "retrievable": True})
            field_handler_3.add_str_field({"name": "doc_id", "retrievable": True, "filterable": True})
            
            project_specific_index3 = os.getenv("PRJDATA_SUGGESTION_INDEX")
            ai_search_collection = AISearchCollection()
            ai_search_collection.process_all_instances(
                index_name=project_specific_index3,
                doc=documents,
                fields=field_handler_3.fields,
                metadata=metadata_fields_project_specific_index3
            )
            
        except Exception as e:
            log.error(f"Error in IndexMetadataTask: Failed to index metadata - {str(e)}")
            raise e

if __name__ == "__main__":
    try:
        luigi.build([IndexMetadataTask3(),IndexMetadataTask2(),IndexMetadataTask1()], local_scheduler=True)
        #luigi.build([DocumentProcessingTaskIndex1(),DocumentProcessingTaskIndex2()], local_scheduler=True)
        clear_temp_files()

    except Exception as e:
        log.error(f"Main: Failed to run Luigi pipeline - {str(e)}")
        clear_temp_files()
        raise e