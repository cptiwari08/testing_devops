from typing import List

from app.core.pydantic_models import QueryPipelineContext
from llama_index.core.schema import NodeWithScore


def context_parser(nodes: tuple[List[NodeWithScore],str,str], context: QueryPipelineContext) -> str:
    """
    Parses a list of NodeWithScore objects to extract text from each node and
    concatenate them into a single string.

    This function iterates through the provided list of NodeWithScore objects,
    extracts the 'text' attribute from each node, and then concatenates these
    texts into a single string, with each piece of text separated by two
    newlines.

    Parameters:
        nodes (List[NodeWithScore]): A list of NodeWithScore objects from which text is to be extracted.

    Returns:
        str: A string containing all extracted texts from the nodes, separated by two newlines.

    Example:
        >>> nodes = [NodeWithScore(text="Hello"), NodeWithScore(text="World")]
        >>> context_parser(nodes)
        'Hello\\n\\nWorld'
    """
    context.logger.info("I continued by executing the context parser function component")
    context_str = []
    for i in nodes[0]:
        selected_text = i.text
        context_str.append(selected_text)
    schema = """
Tables schemas: 
"""+str(nodes[1])
    final_context = "\n\n".join(context_str)+ schema
    return final_context
