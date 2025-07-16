import os
import sys
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
import pandas as pd
import pickle
import luigi
import json
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
config_id = 1
log = Logger()

log.info("Starting ingest process")

class LoadDataFromEndpoint(luigi.Task):
    query1 = luigi.Parameter(default=settings.get_variable(config_id, 'query1'))

    po_host = os.getenv('HOST_ASSET_MANAGER')
    po_json_url = f"{po_host}/data/select/AssetDocuments"
    po_sql_url = f"{po_host}/copilot/execute-query"

    def output(self):
        return [luigi.LocalTarget('temp/data1_comparison.pkl'),
                luigi.LocalTarget('temp/data2_comparison.pkl')]

    def run(self):
        try:
            with open("./utils/payload_asset_manager.json") as f:
                payload_asset_manager = json.load(f)

            
            response_json = DBConnection.connect_db_api(
                payload_asset_manager,
                self.po_json_url,
            )
            data_api = response_json
            with open(self.output()[0].path, 'wb') as f:
                pickle.dump(data_api, f)
            try: 
                engine = DBConnection.create_ey_ip_db_connection()
                with engine.begin() as conn:
                    try:
                        old_info_ingest = pd.read_sql(self.query1, conn)
                        old_info_ingest = json.loads(old_info_ingest['content'][0])
                        with open(self.output()[1].path, 'wb') as t:
                            pickle.dump(old_info_ingest, t)
                    except Exception as e:
                        if "42S02" in str(e):
                            log.error("No data into database")
                            old_info_ingest = []
                            with open(self.output()[1].path, 'wb') as t:
                                pickle.dump(old_info_ingest, t)
                        else:
                            e.error("An error occured while trying the request")
                            e.error(e)
                            sys.exit()
            except Exception as e:
                log.error("No data into database")

            log.info("LoadDataFromSQLTask: Data successfully loaded from SQL")
        except Exception as e:
            log.error(f"Error in LoadDataFromSQLTask: Failed to load data from SQL - {str(e)}")
            raise e

class ProcessingData(luigi.Task):
    similar_data_nodes=  {"process name": "title","process group 1": "function","process group 2": "subfunction",}
    similar_data_tsa= {}
    similar_data_vc= {}
    similar_data_workplan= {}
    po_host = os.getenv('HOST_ASSET_MANAGER')
    po_json_url = f"{po_host}/data/select/AssetDocuments"
    po_sql_url = f"{po_host}/copilot/execute-query"

    def requires(self):
        return LoadDataFromEndpoint()
    
    def output(self):
        return [luigi.LocalTarget('temp/data3_comparison.pkl'),
                luigi.LocalTarget('temp/data4_comparison.pkl'),
                luigi.LocalTarget('temp/data5_comparison.pkl'),
                luigi.LocalTarget('temp/data6_comparison.pkl'),
                luigi.LocalTarget('temp/data7_comparison.pkl')
                ]

    def run(self):
        engine = DBConnection.create_ey_ip_db_connection()
        token_blob_storage = DBConnection.get_token_blob_storage()
        
        with engine.begin() as conn:
            try:
                with open(self.input()[0].path, 'rb') as f:
                    data_api = pickle.load(f)
                
                with open(self.input()[1].path, 'rb') as f:
                    old_info_ingest = pickle.load(f)
                conn.close()
                
                folders,archives, folders_after, archives_after, tables, query, table_name_id_dict_after = process_data_endpoint(data_api, old_info_ingest)
                with engine.begin() as conn:
                    data_json = DBConnection.connect_db_api({"SqlQuery": query, "tables": []}, self.po_sql_url)
                    conn.close()
                archive_md_sum_id, archive_md_sum_id_after, df_metadata = process_data_db(data_json,folders,archives, folders_after, archives_after)
                arc_check1,arc_check2 = check_data(archive_md_sum_id, archive_md_sum_id_after)
                new_data,repeated_data,to_delete_data = general_process(arc_check1,arc_check2,folders,archives, folders_after, archives_after,archive_md_sum_id, archive_md_sum_id_after)
                with open(self.output()[0].path, 'wb') as t:
                    pickle.dump(new_data, t)
                with open(self.output()[1].path, 'wb') as t:
                    pickle.dump(repeated_data, t)
                with open(self.output()[2].path, 'wb') as t:
                    pickle.dump(to_delete_data, t)
                with open(self.output()[3].path, 'wb') as t:
                    pickle.dump(table_name_id_dict_after, t)
                with open(self.output()[4].path, 'wb') as t:
                    pickle.dump(df_metadata, t)

                process_data_final(to_delete_data, new_data, token_blob_storage, self.similar_data_nodes, 
                                        self.similar_data_tsa, self.similar_data_vc, self.similar_data_workplan,
                                        df_metadata, table_name_id_dict_after,engine, data_api,tables)
                log.info("Ingestion process: correct")                    
            except ValueError as e:
                log.error("Ingestion process: failed")
                log.error(e)                
                conn.close()
                

        
if __name__ == "__main__":
    try:
        luigi.build([ProcessingData()], local_scheduler=True)
        clear_temp_files()
    except Exception as e:
        log.error(f"Main: Failed to run Luigi pipeline - {str(e)}")
        clear_temp_files()
        raise e