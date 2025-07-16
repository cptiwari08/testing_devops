from app.op_model.services.workflow.events.om_process_event import (
    OMProcessEvent,
)
from app.op_model.utils import (
    string_to_json,
    create_citing_sources,
    create_citing_sources_templates,
    transform_operating_model
)
from llama_index.core.workflow import StopEvent, Context
 
async def op_model_formating(event: OMProcessEvent, context:Context) -> StopEvent:
    result = event.result
    if isinstance(result, list):
        transformer_data = []
        for item, business_function in result:
            if isinstance(item, str) and "```json" in item:
                result_json = string_to_json(item)
            if isinstance(item, dict):
                result_json = item
            if item:
                _transformer_data = transform_operating_model(result_json, business_function)
            else:
                _transformer_data
            transformer_data.append(_transformer_data)
    else:
        result_json = string_to_json(result)   
        transformer_data = transform_operating_model(result_json) 
    
    # datasources
    try:
        project_docs = await context.get("original_doc_splits", None)
    except ValueError:
        project_docs = None
        
    try:
        ey_ip_data = await context.get("ey_ip_data")
    except ValueError:
        ey_ip_data = None
    
    project_docs = create_citing_sources(project_docs)              
    ey_ip_data = create_citing_sources_templates(ey_ip_data)
        
    result = [
        {
            "sourceName": None,
            "content": item,
            "status": "200",
            "citingSources": [
                project_docs,
                ey_ip_data
            ]
        }
        for item in transformer_data
    ]    
 
    return StopEvent(result=result)
