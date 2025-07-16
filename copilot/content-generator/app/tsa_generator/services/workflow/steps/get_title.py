from app.tsa_generator.services.workflow.events.get_title_event import GetTitleEvent
from tenacity import retry, stop_after_attempt, wait_fixed
from llama_index.core.workflow import StartEvent


#@retry(stop=stop_after_attempt(3), wait=wait_fixed(3))
async def get_title(workflow, event: StartEvent, context) -> GetTitleEvent:
    """Validate the SQL generated over program office"""
    query = "select TSA.Title, D.Title as DispositionType from TSAItems TSA LEFT JOIN TSADay1Dispositions D on D.ID = TSA.TSADay1DispositionId where TSA.ID = "+str(event.item_data)
    result = await workflow.program_office.run_sql(query)
    await context.set("tsa_title", result)

    return GetTitleEvent(title=str(result.data))
