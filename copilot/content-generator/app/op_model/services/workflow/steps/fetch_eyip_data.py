import pandas as pd
from app.op_model.services.workflow.events.eyip_event import EyIpEvent
from app.op_model.services.workflow.events.eyip_data_event import (
    EYIPDataEvent,
)
import os
from llama_index.core.workflow import Workflow, Context
from tenacity import retry, stop_after_attempt, wait_fixed
log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "


@retry(stop=stop_after_attempt(3), wait=wait_fixed(3))
async def fetch_eyip_data(
    workflow: Workflow, event: EyIpEvent, context:Context
    ) -> EYIPDataEvent:
    """Validate the SQL generated over program office"""
    # query = event.sql_generated
    asset_manager = workflow.asset_manager
    logger = workflow.logger

    eyip_ids = await context.get("eyip_ids")
    project_details = await context.get("project_details")

    query, query_params = asset_manager.build_query({
        "project_details": project_details,
        "eyip_ids": eyip_ids  # Include ey ip ids in the search
    })

    df_results = await asset_manager.execute_query(query, query_params)
    
    df = pd.DataFrame(df_results)
    target_columns = ["function", "subfunction", "title"]
    for col in target_columns:
        if col not in df.columns:
            df[col] = None          
    result = df[target_columns]

    ey_ip_dict = build_process_dict(result)
    logger.warning(f"{log_mes_base} EYIP data fetched for: {[eyip['process_lvl_1'] for eyip in ey_ip_dict]}")
    
    
    ey_ip_data = {
        'eyip': ey_ip_dict,
        'sql_query': query
    }
    await context.set("ey_ip_data", ey_ip_data)

    return EYIPDataEvent(result=ey_ip_dict)


def build_process_dict(df: pd.DataFrame) -> list[dict]:
    """
    Build a nested dictionary of process data from a DataFrame.
    
    Args:
        df: DataFrame containing function, subfunction, and title columns
        
    Returns:
        List of dictionaries representing the process hierarchy
    """
    result = []
    for function, df_func in df.groupby('function'):
        sub_processes = []

        for subfunction, df_sub in df_func.groupby('subfunction'):
            sub_processes.append({
                "process_lvl_2": subfunction,
                "processes": df_sub["title"].tolist()
            })

        
        result.append({
                "process_lvl_1": function,
                "sub_processess": sub_processes
        })
    return result