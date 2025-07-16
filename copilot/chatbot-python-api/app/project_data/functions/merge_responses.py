from app.core.pydantic_models import LLMModels
from app.core.utils import raw_chat_completion
from app.core.prompt_manager import create_prompt_manager


async def merge_responses(
    rewritten_question: str, results, llm_models: LLMModels
) -> str:
    """
    Merges multiple query results into a cohesive final answer.
    
    Args:
        rewritten_question: The rewritten form of the original question
        results: List of tuples containing questions and their corresponding results
        llm_models: LLM models for generating responses
        
    Returns:
        str: Final merged response
    """
    # Create prompt manager
    prompt_manager = create_prompt_manager()
    
    answers = []
    for question, result in results:
        answer = f"""\
        sub_question: {question},
        sql_query: {result['query_parser_output']['output']},
        query_result: {result['query_response_parser']['output']},
        """
        answers.append(answer)

    # Get prompt using prompt manager
    prompt = await prompt_manager.get_prompt(
        agent="project_data",
        key="final_answer",
        prompt_parameters={
            "rewritten_question": rewritten_question,
            "answers": "\n".join(answers)
        }
    )
    
    response = await raw_chat_completion(prompt=prompt, llm_models=llm_models)
    return response  # type: ignore
