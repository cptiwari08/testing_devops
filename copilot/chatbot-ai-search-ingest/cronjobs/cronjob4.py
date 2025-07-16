import os
import sys
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
import pandas as pd
import pickle
import luigi
from llama_index.core import Document
from data_access.db_connection import DBConnection
from data_access.ai_search_collection import AISearchCollection
from data_access.ai_search_collection import GetEmbeddings
from utils.field_handler import FieldHandler
from business_logic.settings import DynamicSettings
from utils.logger import Logger
from utils.functions import *
from utils.cleanup_temp import clear_temp_files

# Settings
settings = DynamicSettings(directory='business_logic')
config_id = 4
log = Logger()

class LoadDataFromSQLTask1(luigi.Task):
    query1 = luigi.Parameter(default=settings.get_variable(config_id, 'query1'))

    def output(self):
        return luigi.LocalTarget('temp/data1_general.pkl')

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
        return luigi.LocalTarget('temp/data2_general.pkl')

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
        return luigi.LocalTarget('temp/data3_general.pkl')

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
        return luigi.LocalTarget('temp/data4_general.pkl')

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
        return luigi.LocalTarget('temp/data5_general.pkl')

    def run(self):
        
        try:
            data = DBConnection.execute_query({"SqlQuery": self.query5, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e

class LoadDataFromSQLTask6(luigi.Task):
    query6 = luigi.Parameter(default=settings.get_variable(config_id, 'query6'))
    def output(self):
        return luigi.LocalTarget('temp/data6_general.pkl')

    def run(self):
        
        try:
            data = DBConnection.execute_query({"SqlQuery": self.query6, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e
        
class LoadDataFromSQLTask7(luigi.Task):
    query7 = luigi.Parameter(default=settings.get_variable(config_id, 'query7'))

    def output(self):
        return luigi.LocalTarget('temp/data7_general.pkl')

    def run(self):
        
        try:
            data = DBConnection.execute_query({"SqlQuery": self.query7, "tables": []})
            with open(self.output().path, 'wb') as f:
                pickle.dump(data, f)
                log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e

class DocumentProcessingTask(luigi.Task):
    def requires(self):
        return [LoadDataFromSQLTask1(), 
                LoadDataFromSQLTask2(),
                LoadDataFromSQLTask3(), 
                LoadDataFromSQLTask4(),
                LoadDataFromSQLTask5(), 
                LoadDataFromSQLTask6(),
                LoadDataFromSQLTask7()]

    def output(self):
        return luigi.LocalTarget('temp/processed_documents.pkl')

    def run(self):
        input_files = self.input()
        try:
            with open(self.input()[0].path, 'rb') as in_file:
                data = pickle.load(in_file)
            data_false = processing_data_index(data)
            data_suggestions = DBConnection.suggestions()
            suggestions = processing_data_suggestions(data_suggestions) 

            with open(self.input()[1].path, 'rb') as in_file:
                data = pickle.load(in_file)
            final_second_index =processing_data_index_1(data, suggestions,data_false)
            metadata = """{"prjdata":{"tableName" : str(row["TableName"]), "isMainTable" : row["isMainTable"]}}"""
            data_final_index = processing_general_index1(final_second_index, [], "prjdata")
            
            #index 2
            with open(self.input()[2].path, 'rb') as in_file:
                data = pickle.load(in_file)
            glossary = DBConnection.get_glossary()
            glossary = processing_data_index_2(glossary)
            lookup_values_query = processing_lookup_query(data)
            lookup_values = DBConnection.execute_query({"SqlQuery": lookup_values_query, "tables": []})
            lkup_dictionary = {item['lkup']: item['main_table'] for item in data}
            glossary = processing_lookup_values(lookup_values,lkup_dictionary,glossary)
            with open(self.input()[3].path, 'rb') as in_file:
                data = pickle.load(in_file)
            glossary = processing_lkup_query2(data, glossary)
            with open(self.input()[4].path, 'rb') as in_file:
                data = pickle.load(in_file)
            glossary = processing_lkup_query3(data, glossary)
            with open(self.input()[5].path, 'rb') as in_file:
                data = pickle.load(in_file)
            glossary = processing_lkup_query4(data, glossary)

            metadata ="""{"prjgloss": {"tableName" : row["tableName"],"tag" : row["tag"],"isSchema":row["isSchema"],"dynamicFields":row["dynamicFields"] if "dynamicFields" in row.keys() else None}}"""
            data_final_index = processing_general_index2(glossary, data_final_index, "prjgloss")
            
            #Index 3
            suggestions = DBConnection.suggestions()
            metadata = """{"prjsuggestions":{"source" : row["source"], "appAffinity" : row["appAffinity"], "sqlQuery":row["answerSQL"], "idSuggestion":row["id"],"visibleToAssistant":row["visibleToAssistant"],"isIncluded":row["isIncluded"]}}"""
            data_final_index = processing_general_index3(suggestions, data_final_index, "prjsuggestions")

            with open(self.input()[6].path, 'rb') as in_file:
                data = pickle.load(in_file)
            new_index = pd.DataFrame.from_records(data)
            
            metadata = """{"prjdatacitingsources":{"AppId" : str(row["AppId"]),"AppName" : row["AppName"],"AppKey" : row["AppKey"],"PageHeader": row["PageHeader"],"HREF": row["HREF"],"PageKey": row["PageKey"],"TeamType": row["TeamType"]}}"""
            data_final_index = processing_general_index4(new_index, data_final_index, "prjdatacitingsources")
            print(len(data_final_index)," documents to upload")
            ai_search = GetEmbeddings()
            data_final_index = ai_search.get_embeddings(data_final_index)
            for i in data_final_index:
                i.pop('embedding_text',None)

            with open(self.output().path, 'wb') as f:
                pickle.dump(data_final_index, f)
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
            
            ai_search_collection = AISearchCollection()
            field_handler_1 = FieldHandler()
            field_handler_1.add_complex_field({"name": "metadata"})
            field_handler_1.add_str_field({"name": "indexType", "retrievable": True, "filterable": True})
            project_specific_general_index = os.getenv("PRJDATA_GENERAL_INDEX")

            ai_search_collection.process_all_instances(
                index_name=project_specific_general_index,
                doc=documents,
                fields=field_handler_1.fields
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