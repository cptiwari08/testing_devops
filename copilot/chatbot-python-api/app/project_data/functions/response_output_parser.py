from app.core.pydantic_models import QueryPipelineContext


def response_output_parser(query_output, context: QueryPipelineContext):
    context.logger.info("Executing response output parser function component")
    if query_output == "Query result is empty":
        return "{}"
    else:
        return query_output
