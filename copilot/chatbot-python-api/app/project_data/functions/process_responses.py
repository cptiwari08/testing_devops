from app.core.pydantic_models import LLMModels
from app.core.prompt_manager import create_prompt_manager
from app.project_data.tools.base import *
from llama_index.core.agent import AgentRunner, FunctionCallingAgentWorker
from llama_index.core.llms import ChatMessage


async def walker_agent(
    question: str = None,
    evidence: str | zip = None,
    llm_models: LLMModels | None = None,
) -> str:
    """
    Creates an agent that processes a question and evidence to generate a response.
    
    Args:
        question: The user's question
        evidence: Either a string or a zip of questions and their results
        llm_models: LLM models for generating responses
        
    Returns:
        str: Agent's final response
    """
    # Create prompt manager
    prompt_manager = create_prompt_manager()

    if isinstance(evidence, zip):
        answers = [
                f"User Question: {question},\n"
                f"Query Result: {result.get('query_response_parser', {}).get('output', 'N/A')},"
                for question, result in evidence
            ]

        answers = "\n".join(answers)
    else:
        answers = evidence

    tools = [
        add_tool,
        multiply_tool,
        divide_tool,
        subtract_tool,
        average_tool,
        max_tool,
        min_tool,
        ratio_tool,
        percent_tool,
    ]

    # Get prompt using prompt manager
    agent_template = await prompt_manager.get_prompt(
        agent="project_data",
        key="agent",
        prompt_parameters={}
    )

    prefix_messages = [
        ChatMessage(
            role="system",
            content=agent_template.format(
                user="",
                user_info=answers
            ),
        )
    ]

    worker = FunctionCallingAgentWorker(
        tools=tools,
        llm=llm_models.llm,
        prefix_messages=prefix_messages,
        max_function_calls=10,
        allow_parallel_tool_calls=False,
    )

    agent = AgentRunner(worker)
    task = agent.create_task(question)
    step_output = await agent.arun_step(task.task_id)

    max_steps = 20
    while not step_output.is_last and max_steps > 0:
        step_output = await agent.arun_step(task.task_id)
        max_steps -= 1

    response = agent.finalize_response(task.task_id)
    return str(response)
