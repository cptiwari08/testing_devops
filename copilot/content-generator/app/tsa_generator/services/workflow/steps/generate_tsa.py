from app.core.prompt_manager import create_prompt_manager
from app.tsa_generator.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.tsa_generator.services.workflow.events.ait_data_event import AITDataEvent
from app.tsa_generator.services.workflow.events.op_models_data_event import OPModelsDataEvent
from app.tsa_generator.services.workflow.events.search_docs_event import SearchDocsEvent
from app.tsa_generator.services.workflow.events.tsa_generation_event import (
    TSAGenerationEvent,
)
from llama_index.core import PromptTemplate


prompt_manager = create_prompt_manager()


async def generate_tsa(
    workflow, event: SearchDocsEvent | EYIPTemplatesEvent | OPModelsDataEvent | AITDataEvent, context, llm
) -> TSAGenerationEvent | None:
    """
    Generate team tasks based on input data. This input data is:
        - If no documents are provided, only EY IP data is used.
        - If documents are provided, the project outline and team
          data is extracted from them and used in addition to the EY IP data.
    """

    data = context.collect_events(event, [SearchDocsEvent, EYIPTemplatesEvent, OPModelsDataEvent, AITDataEvent]) 

    if data is None:
        return None

    search_docs, sql_result, op_model_result, ait_result = data
    ait_data_full = await context.get("ait_data")
    ait_data = ait_data_full["ait_data"]
    len_ait_data = len(ait_data)

    op_model = await context.get("op_model_data")
    len_op_model = len(op_model)

    ey_ip_data = await context.get("ey_ip_templates")
    ey_ip = ey_ip_data["eyip"]
    len_ey_ip = len(ey_ip)
    
    project_docs = await context.get("chunk_text")
    len_project_docs = len(project_docs)

    len_data = len_project_docs+len_ey_ip+len_ait_data+len_op_model

    min_tsa = max(10,len_data)
    example_structure = prompt_manager.get_prompt_sync(
        agent="tsa_generator",
        key="example_tsa_structure")
    generate_tsa = prompt_manager.get_prompt_sync(
        agent="tsa_generator",
        key="tsa_generation")

    project_details = context.data.get(
        "project_details", {}
    )  # Use a default empty dict if None

    generate_tsa_template = PromptTemplate(generate_tsa)
    response = await llm.acomplete(
        generate_tsa_template.format(
            team=context.data.get("team").title,
            eyip_adapted_data=sql_result,
            project_documentation=context.data.get("project_documentation"),
            search_results=search_docs,
            sector=project_details.sector,
            sub_sector=project_details.subSector,
            example_structure=example_structure,
            transaction_type=project_details.transactionType,
            operating_model=op_model_result,
            app_inventory=ait_data,
            min_tsa=min_tsa
        ),
    )      
    return TSAGenerationEvent(result=response.text)
