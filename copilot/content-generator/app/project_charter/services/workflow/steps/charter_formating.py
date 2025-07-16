from app.project_charter.services.workflow.events.wp_teamtasks_event import (
    WPTeamTasksEvent,
)
from app.project_charter.utils import (
    string_to_json, 
    create_citing_sources, 
    create_citing_sources_templates
)
from llama_index.core.workflow import StopEvent


async def charter_formating(event: WPTeamTasksEvent, context) -> StopEvent:
    result = event.result
    team = await context.get("team", None)
    result_json = string_to_json(result)    
    
    # datasources
    project_docs = await context.get("original_doc_splits", None)
    ey_ip_templates = await context.get("ey_ip_templates", None)
    
    project_docs = create_citing_sources(project_docs, team)              
    ey_ip_templates = create_citing_sources_templates(ey_ip_templates, team)
        
    result = [
        {
            "sourceName": item['section'],
            "content": item['items'],
            "status": "200",
            "citingSources": [
                project_docs,
                ey_ip_templates
            ]
        }
        for item in result_json
    ]    

    return StopEvent(result=result)
