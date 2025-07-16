from app.tsa_generator.services.workflow.events.tsa_generation_event import (
    TSAGenerationEvent,
)
from app.core.prompt_manager import create_prompt_manager
from app.tsa_generator.utils import (
    string_to_json, 
    create_citing_sources, 
    create_citing_sources_templates,
    create_citing_sources_op_models,
    create_citing_sources_ait
)
from llama_index.core.workflow import StopEvent
from llama_index.core import PromptTemplate

prompt_manager = create_prompt_manager()

async def tsa_formating(workflow, event: TSAGenerationEvent, context, llm) -> StopEvent:
    result = event.result
    result_json = string_to_json(result.replace("\n","")) 
    team = await context.get("team")

    ## datasources
    project_docs = await context.get("project_doc")
    project_docs_data = await context.get("original_doc_splits")
    ey_ip_templates = await context.get("ey_ip_templates")
    if ey_ip_templates:
        ey_ip_data = ey_ip_templates["eyip"]
    else:
        ey_ip_data =[]
    op_models_data = await context.get("op_model_data")
    ait = await context.get("ait_bool", False)
    ait_data = await context.get("ait_data")

    
    project_docs_citation = create_citing_sources(project_docs_data, team.title)              
    ey_ip_templates_citation = create_citing_sources_templates(ey_ip_templates, team.title)
    op_model_citation = create_citing_sources_op_models(op_models_data,team.title)
    ait_citation = create_citing_sources_ait(ait_data,team.title)

    query_origin = "select Id, Title, ItemDescription, [Key] from AssistantGeneratorOrigins"
    origin_data = await workflow.program_office.run_sql(query_origin)
    citing_source_per_item = prompt_manager.get_prompt_sync(
        agent="tsa_generator",
        key="citing_source_per_item")
    citing_source_per_item_template = PromptTemplate(citing_source_per_item)
    
    if op_models_data or project_docs or ey_ip_data or ait:
        tsa_citation_per_item_llm = await llm.acomplete(
            citing_source_per_item_template.format(
                tsa_generated = result_json,
                op_model = op_models_data,
                ey_ip = ey_ip_data,
                documents = project_docs_data,
                ait_data = ait_data,
                origin_data = origin_data.data
            ),
        )
        tsa_citation_per_item = string_to_json(tsa_citation_per_item_llm.text)
    else:
        tsa_citation_per_item = []
        for item in result_json:
            item_content = {
                    "title":item["title"], 
                    "serviceInScopeDescription":item["serviceInScopeDescription"],
                    "tSADay1Disposition": {"key":"TSA"},
                    "assistantGeneratorOrigins": [], 
                    "associatedProcesses": [], 
                    "associatedSystems": [], 
                    "associatedFacilities": [], 
                    "associatedAssets": [], 
                    "associatedTPAs": [], 
                    "associatedAITs": []
                }
            tsa_citation_per_item.append(item_content)

    result = {
            "projectTeam": {"id":team.id,"title":team.title},
            "content": tsa_citation_per_item,
            "status": "200",
            "citingSources": [
                project_docs_citation,ey_ip_templates_citation,op_model_citation,ait_citation
            ]
        }
       

    return StopEvent(result=result)
