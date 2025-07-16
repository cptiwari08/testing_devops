from app.tsa_generator.services.workflow.events.ait_data_event import AITDataEvent


# prompt_manager = create_prompt_manager()


async def process_app_inventorytracker(workflow, context) -> AITDataEvent:
    """
    Executes the app inventory tracker SQL query and returns the result.
    This function is called by the workflow to process the app inventory tracker data.
    
    Args:
        workflow: The workflow instance.
        context: The context containing the SQL query parameters.
    retuns:
        AITDataEvent: The event containing the results of the SQL query.
    """
    try:
        team_id = context.data.get("team").id
        sql_query = f"select APPINV.ID, APPINV.Title, D.Title Disposition, s.Title Status from AppInventory APPINV LEFT JOIN Dispositions D on APPINV.DayOneDispositionId = D.ID LEFT JOIN Statuses s on APPINV.ApplicationStatusId = s.ID where D.Title in ('TSA', 'rTSA') and S.[Title] = 'ACTIVE' and ProjectTeamId = {team_id};"
                
        result = await workflow.program_office.run_sql(sql_query)
        ait_data = {
            'ait_data': result.data,
            'sql_query': sql_query
            }
        await context.set("ait_data",ait_data)
        return AITDataEvent(ait_data_results=result.data)
    except Exception as e:
        workflow.logger.error(f"Failed to execute the AIT query: {e}")
        raise RuntimeError("An error occurred while retrieving OP Model data.") from e