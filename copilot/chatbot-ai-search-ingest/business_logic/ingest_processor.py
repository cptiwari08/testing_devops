import os
import pandas as pd
import pickle
import json
from data_access.db_connection import DBConnection
from utils.logger import Logger
from utils.functions import *
from business_logic.settings import DynamicSettings

class IngestProcessor:
    def __init__(self):
        self.settings = DynamicSettings(directory='business_logic')
        self.config_id = 1
        self.log = Logger()
        self.po_host = os.getenv('HOST_PROGRAM_OFFICE')
        self.po_json_url = f"{self.po_host}/data/select/AssetDocuments"
        self.po_sql_url = f"{self.po_host}/copilot/execute-query"
        self.similar_data_nodes = {"process name": "title", "process group 1": "function", "process group 2": "subfunction"}
        self.similar_data_tsa = {}
        self.similar_data_vc = {}
        self.similar_data_workplan = {}

    def load_data(self):
        query1 = self.settings.get_variable(self.config_id, 'query1')
        
        with open("./utils/payload_asset_manager.json") as f:
            payload_asset_manager = json.load(f)
        
        response_json = DBConnection.connect_db_api(
            payload_asset_manager,
            self.po_json_url,
        )
        data_api = response_json
        
        engine = DBConnection.create_ey_ip_db_connection()
        try:
            with engine.begin() as conn:
                old_info_ingest = pd.read_sql(query1, conn)
                old_info_ingest = json.loads(old_info_ingest['content'][0])
        except Exception as e:
            if "42S02" in str(e):
                self.log.error("No data into database")
                old_info_ingest = []
            else:
                raise e
        
        return data_api, old_info_ingest

    def process_data(self, data_api, old_info_ingest):
        engine = DBConnection.create_ey_ip_db_connection()
        token_blob_storage = DBConnection.get_token_blob_storage()

        folders, archives, folders_after, archives_after, tables, query, table_name_id_dict_after = process_data_endpoint(data_api, old_info_ingest)
        
        data_json = DBConnection.connect_db_api({"SqlQuery": query, "tables": []}, self.po_sql_url)
        archive_md_sum_id, archive_md_sum_id_after, df_metadata = process_data_db(data_json, folders, archives, folders_after, archives_after)
        
        arc_check1, arc_check2 = check_data(archive_md_sum_id, archive_md_sum_id_after)
        new_data, repeated_data, to_delete_data = general_process(arc_check1, arc_check2, folders, archives, folders_after, archives_after, archive_md_sum_id, archive_md_sum_id_after)

        process_data_final(
            to_delete_data, new_data, token_blob_storage,
            self.similar_data_nodes, self.similar_data_tsa,
            self.similar_data_vc, self.similar_data_workplan,
            df_metadata, table_name_id_dict_after, engine,
            data_api, tables
        )

    def run(self):
        try:
            self.log.info("Starting ingestion process")
            data_api, old_info_ingest = self.load_data()
            self.process_data(data_api, old_info_ingest)
            self.log.info("Ingestion process: correct")
            return True
        except Exception as e:
            self.log.error(f"Error in ingestion process: {str(e)}")
            raise e
