import os
import pandas as pd
from data_access.db_connection import DBConnection
from data_access.ai_search_collection import AISearchCollection, GetEmbeddings
from utils.field_handler import FieldHandler
from business_logic.settings import DynamicSettings
from utils.logger import Logger
from utils.functions import *

class GeneralIndexProcessor:
    def __init__(self):
        self.settings = DynamicSettings(directory='business_logic')
        self.config_id = 4
        self.log = Logger()

    def load_all_data(self):
        try:
            queries = [
                self.settings.get_variable(self.config_id, f'query{i}')
                for i in range(1, 8)
            ]
            data_list = []
            for query in queries:
                data = DBConnection.execute_query({"SqlQuery": query, "tables": []})
                data_list.append(data)
                self.log.info(f"Successfully loaded data for query")
            return data_list
        except Exception as e:
            self.log.error(f"Failed to load data - {str(e)}")
            raise e

    def process_data(self, data_list):
        try:
            # Process first index data
            data_false = processing_data_index(data_list[0])
            data_suggestions = DBConnection.suggestions()
            suggestions = processing_data_suggestions(data_suggestions)
            final_second_index = processing_data_index_1(data_list[1], suggestions, data_false)
            metadata = """{"prjdata":{"tableName" : str(row["TableName"]), "isMainTable" : row["isMainTable"]}}"""
            data_final_index = processing_general_index1(final_second_index, [], "prjdata")

            # Process second index data
            glossary = DBConnection.get_glossary()
            glossary = processing_data_index_2(glossary)
            lookup_values_query = processing_lookup_query(data_list[2])
            lookup_values = DBConnection.execute_query({"SqlQuery": lookup_values_query, "tables": []})
            lkup_dictionary = {item['lkup']: item['main_table'] for item in data_list[2]}
            glossary = processing_lookup_values(lookup_values, lkup_dictionary, glossary)
            glossary = processing_lkup_query2(data_list[3], glossary)
            glossary = processing_lkup_query3(data_list[4], glossary)
            glossary = processing_lkup_query4(data_list[5], glossary)

            metadata = """{"prjgloss": {"tableName" : row["tableName"],"tag" : row["tag"],"isSchema":row["isSchema"],"dynamicFields":row["dynamicFields"] if "dynamicFields" in row.keys() else None}}"""
            data_final_index = processing_general_index2(glossary, data_final_index, "prjgloss")

            # Process third index data
            suggestions = DBConnection.suggestions()
            metadata = """{"prjsuggestions":{"source" : row["source"], "appAffinity" : row["appAffinity"], "sqlQuery":row["answerSQL"], "idSuggestion":row["id"],"visibleToAssistant":row["visibleToAssistant"],"isIncluded":row["isIncluded"]}}"""
            data_final_index = processing_general_index3(suggestions, data_final_index, "prjsuggestions")

            # Process fourth index data
            new_index = pd.DataFrame.from_records(data_list[6])
            metadata = """{"prjdatacitingsources":{"AppId" : str(row["AppId"]),"AppName" : row["AppName"],"AppKey" : row["AppKey"],"PageHeader": row["PageHeader"],"HREF": row["HREF"],"PageKey": row["PageKey"],"TeamType": row["TeamType"]}}"""
            data_final_index = processing_general_index4(new_index, data_final_index, "prjdatacitingsources")

            self.log.info(f"{len(data_final_index)} documents to upload")
            ai_search = GetEmbeddings()
            data_final_index = ai_search.get_embeddings(data_final_index)
            for i in data_final_index:
                i.pop('embedding_text', None)

            return data_final_index
        except Exception as e:
            self.log.error(f"Failed to process data - {str(e)}")
            raise e

    def index_documents(self, documents):
        try:
            ai_search_collection = AISearchCollection()
            field_handler = FieldHandler()
            field_handler.add_complex_field({"name": "metadata"})
            field_handler.add_str_field({"name": "indexType", "retrievable": True, "filterable": True})
            
            index_name = os.getenv("PRJDATA_GENERAL_INDEX")
            ai_search_collection.process_all_instances(
                index_name=index_name,
                doc=documents,
                fields=field_handler.fields
            )
            self.log.info("Documents successfully indexed")
        except Exception as e:
            self.log.error(f"Failed to index documents - {str(e)}")
            raise e

    def run(self):
        try:
            self.log.info("Starting general index processing")
            data_list = self.load_all_data()
            documents = self.process_data(data_list)
            self.index_documents(documents)
            return True
        except Exception as e:
            self.log.error(f"Error in general index process: {str(e)}")
            raise e
