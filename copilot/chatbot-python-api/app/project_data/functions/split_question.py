import json

from app.core.pydantic_models import LLMModels
from app.core.utils import raw_chat_completion
from app.core.prompt_manager import create_prompt_manager


async def split_question(
    question: str, chat_history: str, llm_models: LLMModels
) -> dict:
    """
    Splits a complex question into multiple sub-questions.
    
    Args:
        question: The original user question to split
        chat_history: The conversation history for context
        llm_models: LLM models for generating responses
        
    Returns:
        dict: JSON response containing the sub-questions
    """
    # Create prompt manager
    prompt_manager = create_prompt_manager()
    
    # Get prompt using prompt manager
    prompt = await prompt_manager.get_prompt(
        agent="project_data",
        key="sub_questions_prompt",
        prompt_parameters={
            "query_str": question,
            "chat_history": chat_history
        }
    )
    
    response = await raw_chat_completion(prompt=prompt, llm_models=llm_models)
    return json.loads(response)  # type: ignore
