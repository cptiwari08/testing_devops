from app.core.pydantic_models import QueryPipelineContext


def question_rewritting_parser_output(question_output, context: QueryPipelineContext):
    context.logger.info(
        "I executed the question rewriting parser output function component"
    )
    return question_output
