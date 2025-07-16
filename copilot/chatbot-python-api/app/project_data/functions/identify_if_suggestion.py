from app.core.pydantic_models import LLMModels
from app.core.utils import raw_chat_completion
from app.core.prompt_manager import create_prompt_manager


async def identify_if_suggestion(
    question: str, question_bank: str, llm_models: LLMModels
) -> dict:
    """
    Identifies if the given question is a suggestion based on a question bank.
    
    Args:
        question: The user question to evaluate
        question_bank: Bank of questions to compare against
        llm_models: LLM models for generating responses
        
    Returns:
        dict: Response from the LLM about whether the question is a suggestion
    """
    # Create prompt manager without logger since this is a function
    prompt_manager = create_prompt_manager()
    
    # Get prompt using prompt manager
    prompt = await prompt_manager.get_prompt(
        agent="project_data",
        key="compare_suggestion_free_text",
        prompt_parameters={
            "user_question": question,
            "question_bank": question_bank
        }
    )
    
    response = await raw_chat_completion(prompt=prompt, llm_models=llm_models)
    return response  # type: ignore
