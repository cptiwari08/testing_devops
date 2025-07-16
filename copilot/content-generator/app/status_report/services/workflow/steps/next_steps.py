import json
import math

from app.core.prompt_manager import create_prompt_manager
from app.status_report.services.workflow.events.general_results import GeneralResults
from app.status_report.services.workflow.events.next_steps import NextSteps
from llama_index.core.workflow import Context


prompt_manger = create_prompt_manager()


async def next_steps(workflow, ctx: Context, ev: NextSteps) -> GeneralResults:
    project_context = await workflow.copilot_api.get_project_context()

    char_limit_query = await workflow.program_office.run_sql(
        sql_query=prompt_manger.get_prompt_sync(
            agent="status_report",
            key="acc_and_ne_char_limit_query",
        )
    )

    default_char_limit = 255
    try:
        char_limit_init = char_limit_query.data[0]["MaxCharLimit"]
        if isinstance(char_limit_init, str) and char_limit_init.isdigit():
            char_limit = int(char_limit_init)
        elif isinstance(char_limit_init, int) and char_limit_init >= 20:
            char_limit = char_limit_init
        else:
            char_limit = default_char_limit
    except:
        char_limit = default_char_limit

    assumed_char_per_word = 10
    words_limit = math.floor(char_limit/assumed_char_per_word)

    next_steps_prompt = str(await ctx.get("next_steps_prompt")).replace(
        "$queries_next_steps$", ev.es_queries_result
    ).replace(
        "$project_context$", project_context["value"]
    ).replace(
        "$target_team$", str(ev.project_team.title)
    ).replace("$num_words$",str(words_limit))


    llm_response = await workflow.llm.acomplete(next_steps_prompt)

    try:
        ne_content = json.loads(llm_response.text)
    except:
        retry_prompt = (
        str(await ctx.get("retry_prompt"))
        .replace("$output", llm_response.text)
        )
        retry_llm_response = await workflow.llm.acomplete(retry_prompt)
        ne_content = json.loads(retry_llm_response.text)

    source= {
        "sourceName": ev.citing_sources[0]["sourceName"],
        "content": ne_content,
        "status": "200",
        "citingSources": ev.citing_sources
    }

    return GeneralResults(
        source=source
    )
