from typing import List

from app.core.pydantic_models import QueryPipelineContext
from llama_index.core.schema import NodeWithScore


def question_rewritting_team_type(nodes: tuple[List[NodeWithScore],str,str], context: QueryPipelineContext):
    context.logger.info(
        "I executed the question rewriting team type output function component to rephrase the questions appropriately"
    )
    user_question = nodes[2]
    return user_question