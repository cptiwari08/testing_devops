from app.tsa_generator.services.workflow.events.search_team_event import SearchTeamEvent
from llama_index.core.workflow import Context, StartEvent


async def process_input(
    workflow, event: StartEvent, context: Context, llm
) -> SearchTeamEvent:
    
    await context.set("retry", False)
    await context.set("team", event.team)
    await context.set("project_details", event.project_details)
    await context.set("project_doc", event.project_doc)
    await context.set("ait_bool", event.ait_bool)
    await context.set("project_documentation", event.project_documentation)
    if await context.get("project_doc") == []:
        await context.set("original_doc_splits", None)     

    #Include EY IP ids to include in SQL
    eyip_projectteam = [d for d in event.eyip_ids if d.projectTeam == event.team.title]
    await context.set("eyip_ids", eyip_projectteam)
    if await context.get("eyip_ids") == []:
        await context.set("ey_ip_templates", None)

    await context.set("op_models", event.op_models)
    if await context.get("op_models") == []:
        await context.set("op_models_data", None)
    
    await context.set("op_models_ids", event.op_models_ids)   

    # Include store procedure name to call
    sp_name = "procGetHierarchyNodesandEnablersbyParentNodeID"
    await context.set("sp_name", sp_name)

    return SearchTeamEvent(project_team={"id":event.team.id,"title":event.team.title})
