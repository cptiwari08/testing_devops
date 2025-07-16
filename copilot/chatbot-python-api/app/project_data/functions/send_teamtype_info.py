from app.core.pydantic_models import QueryPipelineContext

def send_teamtype(context: QueryPipelineContext):
    """Function to send team type name to final answer prompt, to ensure this value is always displayed

    Args:
        context (QueryPipelineContext): Context that contains the input request

    Returns:
        str: Team type name
    """
    team_name = context.message_request.context.appInfo.name
    return team_name