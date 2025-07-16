from app.core.pydantic_models import QueryPipelineContext


def send_teamtype_output(team_name, context: QueryPipelineContext):
    context.logger.info("Executing team type output function component")
    return team_name
