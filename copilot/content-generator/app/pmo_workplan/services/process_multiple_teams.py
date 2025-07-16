from app.core.utils import clean_and_load_json
from app.core.prompt_manager import create_prompt_manager
from llama_index.core import PromptTemplate


prompt_manager = create_prompt_manager()


async def process_multiple_teams(
    llm,
    teams,
    asset_manager
) -> str:
    process_input_multi = prompt_manager.get_prompt_sync(
        agent="pmo_workplan",
        key="process_input_multi"
    )
    query = "SELECT DISTINCT projectteam FROM Workplan"
    result = await asset_manager.execute_query(query)
    targets = [row[0] for row in result]
    teams_list = [
            project_team.title for project_team in teams
        ]
    process_input_multi_template = PromptTemplate(process_input_multi)
    response = await llm.acomplete(
        process_input_multi_template.format(
            inputs=str(teams_list),
            targets=str(targets),
        )
    )
    result = clean_and_load_json(response.text)
    # check if any of the key is having empty array then update the empty array with key itself
    for key, value in result.items():
        if isinstance(value, list) and not value:
            result[key] = [key]
    return result
