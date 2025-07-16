from app.core.program_office_api import ProgramOfficeResponse
from app.core.prompt_manager import create_prompt_manager
from app.pmo_workplan.services.workflow.events.eyip_templates_event import (
    EYIPTemplatesEvent,
)
from app.pmo_workplan.services.workflow.events.search_docs_event import SearchDocsEvent
from app.pmo_workplan.services.workflow.events.wp_teamtasks_event import (
    WPTeamTasksEvent,
)
from app.pmo_workplan.services.workflow.events.wp_teamtasksom_event import (
    WPTeamTasksOMEvent,
) 
from app.pmo_workplan.services.workflow.events.good_sql_pmo_event import GoodSQLPMOEvent

from llama_index.core import PromptTemplate


prompt_manager = create_prompt_manager()


async def generate_tasks(
    workflow, event: SearchDocsEvent | EYIPTemplatesEvent, context, llm, logger
) ->  WPTeamTasksEvent | WPTeamTasksOMEvent  | None:
    """
    Generate team tasks based on input data. This input data is:
        - If no documents are provided, only EY IP data is used.
        - If documents are provided, the project outline and team
          data is extracted from them and used in addition to the EY IP data.
    """
    data = context.collect_events(event, [SearchDocsEvent, EYIPTemplatesEvent])

    if data is None:
        return None
    
    project_outline = await context.get("project_outline")
    target_team = await context.get("team")
    logger.info(f"Initialized step generate_tasks for {target_team}")

    search_docs, sql_result = data

    team_documents = [doc["chunk_text"] for doc in search_docs.search_results]

    interd_prompt = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                   key="interdependencies_query")

    response_pmo: ProgramOfficeResponse = await workflow.program_office.run_sql(
        sql_query=interd_prompt
    )
    interdepencies_list = [element["ProjectTeam"] for element in response_pmo.data]
    op_model_data = context.data.get('op_model_data')
    project_details = context.data.get(
        "project_details", {}
    )  # Use a default empty dict if None
    team_documents = [doc["chunk_text"] for doc in search_docs.search_results]
    if op_model_data:
        # generation with op model data
        generation_task = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                        key="task_generation_with_omodel")
        generation_task_template = PromptTemplate(generation_task)

        example_structure = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                       key="example_task_structure_op_model")

        response = await llm.acomplete(
            generation_task_template.format(
                target_team=context.data.get("team"),
                goals=project_outline.get("goal_summary", None),
                objectives=project_outline.get("objective_summary", None),
                description=project_outline.get("project_outline_summary", None),
                op_model_data= op_model_data,
                success_factors=project_outline.get("success_factors_summary", None),
                milestones=project_outline.get("milestones_summary", None),
                eyip_adapted_data=sql_result.result,
                timeline=f"{context.data.get('duration_in_month', None)} months",
                sector=project_details.sector,
                subSector=project_details.subSector,
                example_structure=example_structure,
                search_results=team_documents,
                transaction_type=project_details.transactionType,
                interdepencies_list=interdepencies_list,
            ),
        )
        logger.info(f"Finalized step generate_tasks for {target_team}")        
        return WPTeamTasksOMEvent(result=response.text)

    example_structure = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                       key="example_task_structure")
    if project_outline:
        # generation with project outline and without
        # Is it necessary to reread task_generation.yaml here?
        generation_task = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                        key="task_generation")
        generation_task_template = PromptTemplate(generation_task)

        response = await llm.acomplete(
            generation_task_template.format(
                target_team=context.data.get("team"),
                goals=project_outline.get("goal_summary", None),
                objectives=project_outline.get("objective_summary", None),
                description=project_outline.get("project_outline_summary", None),
                success_factors=project_outline.get("success_factors_summary", None),
                milestones=project_outline.get("milestones_summary", None),
                eyip_adapted_data=sql_result.result,
                timeline=f"{context.data.get('duration_in_month', None)} months",
                sector=project_details.sector,
                subSector=project_details.subSector,
                example_structure=example_structure,
                search_results=team_documents,
                transaction_type=project_details.transactionType,
                interdepencies_list=interdepencies_list,
            ),
        )
    else:
        # generation without project outline
        generation_task = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                        key="task_generation_without_project_outlines")
        generation_task_template = PromptTemplate(generation_task)
        response = await llm.acomplete(
            generation_task_template.format(
                target_team=context.data.get("team"),
                eyip_adapted_data=sql_result.result,
                timeline=f"{context.data.get('duration_in_month', None)} months",
                sector=project_details.sector,
                subSector=project_details.subSector,
                example_structure=example_structure,
                transaction_type=project_details.transactionType,
            ),
        )
    logger.info(f"Finalized step generate_tasks for {target_team}")
    return WPTeamTasksEvent(result=response.text)
