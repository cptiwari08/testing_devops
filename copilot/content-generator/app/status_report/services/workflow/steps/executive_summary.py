import math

from app.core.prompt_manager import create_prompt_manager
from app.status_report.services.workflow.events.executive_summary import (
    ExecutiveSummary,
)
from app.status_report.services.workflow.events.executive_summary_tone_mimic import (
    ExecuteSummaryToneMimic,
)
from llama_index.core.workflow import Context

import os


prompt_manager = create_prompt_manager()


async def executive_summary(
    workflow, ctx: Context, ev: ExecutiveSummary
) -> ExecuteSummaryToneMimic:
    project_context = await workflow.copilot_api.get_project_context()

    if project_context is None:
        project_context = {
            "value": f"The name of this project is: {ev.project_team.title}"
        }

    char_limit_query = await workflow.program_office.run_sql(
        sql_query=prompt_manager.get_prompt_sync(
            agent="status_report",
            key="executive_summary_char_limit_query"
        )
    )

    default_char_limit = 4000
    try:
        char_limit_init = char_limit_query.data[0]["MaxCharLimit"]
        if isinstance(char_limit_init, str) and char_limit_init.isdigit():
            char_limit = int(char_limit_init)
        elif isinstance(char_limit_init, int) and char_limit_init >= 500:
            char_limit = char_limit_init
        else:
            char_limit = default_char_limit
    except:
        char_limit = default_char_limit

    assumed_char_per_word = 10
    words_limit = math.floor(char_limit/assumed_char_per_word)

    executive_summary_prompt = (
        str(await ctx.get("executive_summary_prompt"))
        .replace("$queries_summary$", ev.es_queries_result)
        .replace("$project_context$", project_context["value"])
        .replace("$target_team$", str(ev.project_team.title))
        .replace("$num_words$",str(words_limit))
    )

    llm_response = await workflow.llm.acomplete(executive_summary_prompt)

    promp_text = str(llm_response.text)
    new_line = os.linesep
    final_response = str(llm_response.text).replace('\n',str(new_line))
    
    num_characters_in_prompt = len(promp_text)
    num_words_in_prompt = len(promp_text.split(" "))

    num_words = words_limit

    if num_characters_in_prompt > char_limit and num_words_in_prompt <= words_limit:
        num_words = math.floor(words_limit*0.9)

    return ExecuteSummaryToneMimic(
        project_team=ev.project_team,
        source="executiveSummary",
        response=final_response,
        citing_sources=ev.citing_sources,
        num_words=num_words
    )
