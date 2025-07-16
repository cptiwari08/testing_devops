import json

from app.core.pydantic_models import QueryPipelineContext


def table_parser(reranker_result_str, context: QueryPipelineContext):
    context.logger.info("I proceeded to execute the table parser function component to analyze the tables involved in the process")
    reranker_list = json.loads(reranker_result_str.message.content)

    return reranker_list
