from app.core.pydantic_models import LLMModels
from llama_index.core.selectors import LLMSingleSelector


async def calculation_check(question: str, llm_models: LLMModels) -> bool:

    # TODO: This would be a multi-class classification model
    choices = [
        "questions_with_calculations: This tool answers questions that require calculations, run rates, top-dow targets or ratios. For example, 'What is the estimated run rate?', 'Tell me the total number of Tasks and get the ratio by projects'",
        "questions_without_calculations: This tool answers general questions about project data, including data extraction, counts, and general information. For example, 'How many tasks are assigned to me?'",
    ]
    selector = LLMSingleSelector.from_defaults(llm=llm_models.llm)
    selector_result = selector.select(choices, query=question)

    if selector_result.selections[0].index == 0:
        return True
    else:
        return False
