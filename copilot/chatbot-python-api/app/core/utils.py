import asyncio
import base64
import io
import os
import tokenize
from typing import List, Union

import re
import yaml
from app.core.config import TenacityConfig, TokenValidationConfig
from app.core.errors import MissingTokenClaimError, PromptValidationError
from app.core.logger import Logger
from app.core.pydantic_models import LLMModels, MessageRequest, PromptTemplate, Score
from cryptography.hazmat.primitives import serialization
from jose import jwt
from llama_index.core import Document, VectorStoreIndex
from llama_index.core.indices.postprocessor import SimilarityPostprocessor
from llama_index.core.node_parser import SentenceSplitter
from llama_index.core.query_engine import RetrieverQueryEngine
from llama_index.core.retrievers import VectorIndexRetriever
from pydantic import ValidationError
from sql_metadata import Parser
from tenacity import (
    RetryCallState,
    retry,
    retry_if_exception_type,
    stop_after_attempt,
    stop_after_delay,
    wait_fixed,
)

tenacity_config = TenacityConfig()
logger = Logger()


def remove_file(path_: str) -> None:
    """
    Removes a file at a given path.
    Args:
        path_: The path to the file to remove.
    """
    try:
        os.remove(path_)
    except Exception as e:
        logger.error(f"Error removing file {path_}: {e}")


def get_tenacity_exception(retry_state: RetryCallState) -> None:
    """
    Logs an exception that caused a retry to fail.
    Args:
        retry_state: The state of the retry.
    """
    logger = Logger()
    exception = retry_state.outcome.exception()
    logger.error(f"Retry failed with exception: {exception}")


def retry_decorator(exception_type: Exception) -> callable:
    """
    Returns a decorator that performs a retry for a specific exception type.
    Args:
        exception_type: The type of exception to retry on.

    Returns:
        A decorator that performs a retry for the given exception type.
    """

    def wrapper(func):
        return retry(
            retry=retry_if_exception_type(exception_type),
            stop=(
                stop_after_attempt(tenacity_config.max_attempts)
                | stop_after_delay(tenacity_config.timeout)
            ),
            wait=wait_fixed(tenacity_config.delay),
            before_sleep=get_tenacity_exception,
        )(func)

    return wrapper


def count_tokens(string: str) -> int:
    """
    Counts the number of tokens in a given string. Tokens are identified using Python's tokenize module.
    Comments and newlines are not considered as tokens.
    Args:
        string (str): The string to tokenize and count.
    Returns:
        int: The number of tokens in the string.
    """
    tokens = tokenize.tokenize(io.BytesIO(string.encode("utf-8")).readline)
    count = 0
    for token in tokens:
        if token.type != tokenize.COMMENT and token.type != tokenize.NL:
            count += 1

    return count


def get_cost_per_tokens(string: str) -> float:
    """
    Calculates the cost per token for a given string. The cost is defined as the number of tokens in the string divided by 1,000,000.
    Args:
        string (str): The string for which to calculate the cost per token.
    Returns:
        float: The cost per token for the string.
    """
    count = count_tokens(string)
    return count / 1000000


def load_prompt_template(yaml_path: str, **kwargs) -> str:
    """
    Load and process a prompt template from a YAML file.

    This function reads a YAML file specified by `yaml_path`, constructs a
    PromptTemplate object from the data, and then generates a text prompt.
    If the template includes placeholders, they can be filled using `kwargs`.

    Parameters:
    - yaml_path (str): The file path to the YAML file containing the prompt template.
    - **kwargs: Arbitrary keyword arguments that will be used to format the prompt text.

    Returns:
    - str: The formatted prompt text. If `few_shots` is present in the template,
           they are appended to the prompt text, each followed by a newline.
           If `kwargs` are provided, they are used to replace placeholders in the template.

    Raises:
    - PromptValidationError: If there is a validation error when creating the PromptTemplate object.

    Example of use:
    - load_prompt_template("template.yaml", name="ChatGPT", language="Python")

    This will load the prompt template from "template.yaml", and if the template contains placeholders
    like "{name}" or "{language}", they will be replaced with "ChatGPT" and "Python", respectively.
    """
    with open(yaml_path, "r") as file:
        data = yaml.safe_load(file)

    try:
        prompt_template = PromptTemplate(**data)
    except ValidationError as e:
        raise PromptValidationError(f"Error loading prompt template: {e}")

    result = prompt_template.text + "\n"
    if prompt_template.few_shots:
        for shot in prompt_template.few_shots:
            result += shot + "\n"

    if kwargs:
        return result.format(**kwargs)
    return result


def inject_project_description(
    message_request: MessageRequest, yaml_path: str
) -> MessageRequest:

    project_description = (
        message_request.context.projectDescription if message_request.context else None
    )

    if project_description:
        message_request.question = load_prompt_template(
            yaml_path=yaml_path,
            project_description=project_description,
            user_prompt=message_request.question,
        )

    return message_request


def decode_jwt_token(token) -> dict:
    """
    Decode a JWT token
    :param token: The JWT token as a string.
    """
    token = token.replace("Bearer ", "")
    config = TokenValidationConfig()
    public_key_der = base64.b64decode(config.public_key)  # type: ignore
    public_key = serialization.load_der_public_key(public_key_der)

    decoded_token = jwt.decode(
        token,
        public_key,  # type: ignore
        algorithms=["RS256"],
        issuer=config.issuer,
        options={
            "verify_aud": False
        },  # Currently not possible due to EY implementation
    )
    return decoded_token


def extract_claim(jwt_token, claim_name, decode_func=decode_jwt_token):
    """
    Extract a specific claim from a JWT token.

    :param jwt_token: The JWT token as a string.
    :param claim_name: The name of the claim to extract.
    :return: The value of the specified claim or None if the claim does not exist.
    """
    decoded_token = decode_func(jwt_token)
    try:
        # Extract the claim
        claim_value = decoded_token[claim_name]
    except KeyError:
        raise MissingTokenClaimError(claim_name)

    return claim_value


def get_tables_from_sql_query(sql_query: str) -> List:
    """
    Try to extract the table names from a SQL query string

    :param sql_query: string with the sql query
    :return: A list of tables or empty list if the parser fails
    """
    try:
        tables = Parser(sql_query).tables
    except ValueError:
        tables = []
    return tables


def extract_relevant_content(
    input_text: Union[List[str], str],
    user_question: str,
    llm_models: LLMModels,
    chunk_size=100,
    chunk_overlap=10,
    similarity_top_k=3,
    threshold=0.7,
) -> str:
    """
    Return the most significant parts of a document according to a threshold
    Args:
        input_text (Union[List[str], str]): A list of documents or a single document with project descriptions
        user_question (str): User needs
        chunk_size (int): Size of the chunks to split the document
        chunk_overlap (int): Overlap between chunks
        similarity_top_k (int): Number of top similar chunks to retrieve
        threshold (float): Similarity cutoff threshold
    Returns:
        str: A string of the most relevant parts
    """

    if isinstance(input_text, str):
        input_text = [input_text]

    documents = [Document(text=t) for t in input_text]

    splitter = SentenceSplitter(
        chunk_size=chunk_size,
        chunk_overlap=chunk_overlap,
    )

    nodes = splitter.get_nodes_from_documents(documents)

    index = VectorStoreIndex(nodes, embed_model=llm_models.embed_model)

    retriever = VectorIndexRetriever(
        index=index, similarity_top_k=similarity_top_k, llm=llm_models.llm
    )

    # If metadata grows, we can add a response synthesizer to summarize the response
    # configure response synthesizer
    # response_synthesizer = get_response_synthesizer(
    #     response_mode="tree_summarize",
    # )

    # For some reason this method is overriding the callback
    # just keeping the reference an reasigning later
    # this is a llama-index bug
    callback_manager = llm_models.llm.callback_manager
    query_engine = RetrieverQueryEngine.from_args(
        callback_manager=callback_manager,
        retriever=retriever,
        response_synthesizer=None,
        node_postprocessors=[
            SimilarityPostprocessor(similarity_cutoff=threshold),
        ],
        llm=llm_models.llm,
    )
    llm_models.llm.callback_manager = callback_manager

    nodes = query_engine.retrieve(user_question)

    if not nodes:
        return ""

    relevant_content = ""

    for node in nodes:
        relevant_content += node.text
        relevant_content += "\n"

    return relevant_content


def calculate_final_score(scores: List[Score | None] | Score) -> Score | None:
    """
    Calculate the final scores based on a list of scores
    """
    if isinstance(scores, Score):
        return scores

    if any(scores):
        scores = [score for score in scores if score]
        return min(scores, key=lambda score: score.value)


async def raw_chat_completion(prompt: str, llm_models: LLMModels) -> str:
    completion = await asyncio.to_thread(llm_models.llm.complete, prompt)
    return completion.text


def clean_title(title):
    cleaned_title = re.sub(r'(_[0-9a-fA-F-]{36})', '', title)
    cleaned_title = cleaned_title.strip('_ ')
    return cleaned_title


def clear_special_characters(input_string):
    """
    Remove special characters from the input string.
    
    Parameters:
    input_string (str): The string from which to remove special characters.
    
    Returns:
    str: The cleaned string with special characters removed.
    """
    # Use regex to replace special characters with an empty string
    cleaned_string = re.sub(r'[^a-zA-Z0-9\s_]', '', input_string)
    return cleaned_string