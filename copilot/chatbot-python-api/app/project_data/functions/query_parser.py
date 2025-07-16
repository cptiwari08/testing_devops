import re

from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import load_prompt_template, raw_chat_completion
from llama_index.core.base.llms.types import ChatResponse


async def query_parser(
    query_output: str | ChatResponse, context: QueryPipelineContext
) -> str:
    """
    Parses the output from a query based on its type.

    This function takes an input expected to be the output from a query. If the
    input is a string, it returns the input unchanged. If the input is an
    instance of ChatResponse, it retrieves the nested message content from the
    ChatResponse object.

    Parameters:
    - query_output (str | ChatResponse): The output of a query, which can
      either be a string or an instance of ChatResponse that contains a nested
      `message.content` structure.

    Returns:
    - str: The original string if `query_output` is a string, or the
      `message.content` from a ChatResponse object if `query_output` is not a
      string.

    Raises:
    - AttributeError: If `query_output` is not a string and does not have a
      `message.content` attribute.
    """
    if handlers := context.callback_handlers:
        context.llm_models.llm.callback_manager.handlers = handlers

    async def clean_sql_string(sql_string):
        # Define a regex pattern to match the SQL block between '```sql' and '```'
        pattern = r'```sql\s*(.*?)\s*```'

        # Search for the pattern and extract the SQL string
        match = re.search(pattern, sql_string, re.DOTALL | re.IGNORECASE)

        if match:
            # Extract the matched SQL content
            cleaned_string = match.group(1).strip()
        else:
            # If no match is found, return the original string or handle the error
            cleaned_string = sql_string

        return cleaned_string

    context.logger.info("I then executed the context parser output function component to finalize the context parsing")
    if isinstance(query_output, str):
        final_string = await clean_sql_string(query_output)
    else:
        final_string = await clean_sql_string(query_output.message.content)  # type: ignore
    return final_string
