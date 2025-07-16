from app.tsa_generator.services.workflow.events.op_models_data_event import OPModelsDataEvent


async def op_model_data_retrieve(workflow, context) -> OPModelsDataEvent:
    """
    Execute the Store Procedure procGetHierarchyNodesandEnablersbyParentNodeID
    to retrieve OP Model data from the database using the provided node IDs.
    
    Args:
        workflow: The workflow instance.
        context: The context containing the node IDs and other parameters.
    Returns:
        OPModelsDataEvent: The event containing the results of the stored procedure.
    """
    #TODO: Call SP
    try:       
        
        team = await context.get("team")
        team_id = team.id
        node_maps = await context.get("op_models_ids", None)        
        mapped_node_ids = []
        op_models = await context.get("op_models")
        for node in node_maps:
            if node['projectTeamId'] == team_id:
                mapped_node_ids = node['mappedNodeId']
                break
        node_ids = ', '.join(map(str, mapped_node_ids))
        result = await workflow.program_office.run_stored_procedure(context.data.get("sp_name",None), {"nodeparentids": node_ids})
        filtered_result = [x for x in result.data if str(x["Category"]).upper() in op_models]
        await context.set("op_model_data",filtered_result,None)
        
        return OPModelsDataEvent(result=filtered_result)
    
    except Exception as e:
        workflow.logger.error(f"Failed to execute stored procedure: {e}")
        raise RuntimeError("An error occurred while retrieving OP Model data.") from e
