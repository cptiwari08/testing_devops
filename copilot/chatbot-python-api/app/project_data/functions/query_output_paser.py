from app.core.pydantic_models import QueryPipelineContext


def query_output_parser(query_output, context: QueryPipelineContext):
    context.logger.info("I executed the query output parser function component")
    return query_output
