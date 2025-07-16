from app.core.pydantic_models import QueryPipelineContext


def question_rewritting_parser(question_output, context: QueryPipelineContext):
    """Transforms the query into a str"""
    context.logger.info("I executed the question rewriting parser function component")
    question = question_output.message.content
    return question
