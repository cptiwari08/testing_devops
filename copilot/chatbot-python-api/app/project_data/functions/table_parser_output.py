from app.core.pydantic_models import QueryPipelineContext


def table_parser_output(table_output, context: QueryPipelineContext):
    context.logger.info("Executing table parser output function component")
    return table_output
