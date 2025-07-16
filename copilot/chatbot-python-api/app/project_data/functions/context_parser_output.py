from app.core.pydantic_models import QueryPipelineContext


def context_parser_output(context_output, context: QueryPipelineContext):
    context.logger.info("I then executed the context parser output function component to finalize the context parsing")
    if context_output == "Query schema is empty":
        return [""]
    else:
        return [context_output]
