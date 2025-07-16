import math

from app.core.prompt_manager import create_prompt_manager
from app.core.program_office_api import ProgramOfficeResponse
from app.status_report.services.workflow.events.executive_summary_tone_mimic import (
    ExecuteSummaryToneMimic,
)
from app.status_report.services.workflow.events.general_results import GeneralResults
from llama_index.core.workflow import Context


prompt_manager = create_prompt_manager()


async def executive_summary_tone_mimic(
    workflow, ctx: Context, ev: ExecuteSummaryToneMimic
) -> GeneralResults:

    examples_response: ProgramOfficeResponse = await workflow.program_office.run_sql(
        sql_query=prompt_manager.get_prompt_sync(
            agent="status_report",
            key="executive_summary_sql_examples",
        )
    )

    executive_summary_mimic_tone_prompt = (
        str(await ctx.get("execute_summary_mimic_tone"))
        .replace("$executive_summary$", ev.response)
        .replace("$executive_summary_examples$", str(examples_response.data).strip('[]'))
        .replace("$num_words$",str(ev.num_words))
    )

    llm_response = await workflow.llm.acomplete(executive_summary_mimic_tone_prompt)

    source= {
        "sourceName": ev.citing_sources[0]["sourceName"],
        "content": str(llm_response.text),
        "status": "200",
        "citingSources": ev.citing_sources
    }

    return GeneralResults(
        source=source
    )
