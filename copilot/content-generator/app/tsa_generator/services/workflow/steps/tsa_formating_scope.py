from app.tsa_generator.services.workflow.events.scope_generation_event import (
    ScopeGenerationEvent,
)
from app.core.prompt_manager import create_prompt_manager
from llama_index.core.workflow import StopEvent

prompt_manager = create_prompt_manager()

async def tsa_formating(event: ScopeGenerationEvent, context) -> StopEvent:
    result = event.result
    team = await context.get("team")

    result = {
            "sourceName": "ServiceInScopeDescription",
            "content": result,
            "status": "200"
        }
       

    return StopEvent(result=result)
