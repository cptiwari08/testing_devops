import json
import re

import yaml
from app.core.errors import PromptValidationError
from app.core.schemas import PromptTemplate
from pydantic import ValidationError


def load_template(yaml_path: str, **kwargs) -> str:
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
    - load_template("template.yaml", name="ChatGPT", language="Python")

    This will load the prompt template from "template.yaml", and if the template contains placeholders
    like "{name}" or "{language}", they will be replaced with "ChatGPT" and "Python", respectively.
    """
    with open(yaml_path, "r") as file:
        data = yaml.safe_load(file)

    try:
        prompt_template = PromptTemplate(**data)
    except ValidationError as e:
        raise PromptValidationError(f"Error loading prompt template: {e}")

    result = prompt_template.text

    if kwargs:
        return result.format(**kwargs)
    return result


def clean_and_load_json(response_text):
    """
    Cleans the GPT output by removing quotation marks and code fences,
    then parses it into a JSON object.

    Parameters:
        response_text (str): The raw text output from GPT.

    Returns:
        dict or list: The parsed JSON object.

    Raises:
        ValueError: If the text cannot be parsed into JSON after cleaning.
    """
    # Remove code fences and language identifiers
    response_text = re.sub(r'^```(?:json)?\s*', '', response_text.strip(), flags=re.IGNORECASE)
    response_text = re.sub(r'```$', '', response_text.strip())

    # Remove leading and trailing quotation marks
    if response_text.startswith('"') and response_text.endswith('"'):
        response_text = response_text[1:-1]

    # Unescape any escaped quotation marks within the JSON
    response_text = response_text.replace('\\"', '"')
    response_text = response_text.replace('{{', '{').replace('}}', '}')
    try:
        return json.loads(response_text)
    except json.JSONDecodeError as e:
        raise ValueError(f"Failed to parse JSON: {e.msg}") from e
