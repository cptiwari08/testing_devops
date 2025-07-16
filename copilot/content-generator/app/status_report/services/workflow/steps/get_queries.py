import re

import pandas as pd
from app.status_report.services.workflow.events.create_report import CreateReport
from app.status_report.services.workflow.events.get_queries import GetQueries
from llama_index.core.workflow import Context, StopEvent


def format_queries(full_data, sections, tables):
    # Filtering data to have only the queries for this generator that are active
    df = pd.DataFrame.from_dict(full_data)
    df_filtered = df.loc[
        (df["generatorType"] == "ProjectStatus") & (df["isActive"] == True),
        ["key", "description", "sqlQuery"],
    ]
    # List of sections and tables names
    sections_list = list(sections.keys())
    tables_list = list(tables.keys())
    # Tagging each row with section and table name
    df_filtered["section"] = df["key"].apply(
        lambda x: sections[re.findall(r"|".join(sections_list), x, re.IGNORECASE)[0]]
    )
    df_filtered["table"] = df["key"].apply(
        lambda x: tables[re.findall(r"|".join(tables_list), x, re.IGNORECASE)[0]]
    )
    df_filtered["sqlQuery"] = df_filtered["sqlQuery"].apply(
        lambda x: x.replace('"',"")
    )

    # Transforming into the required dictionary
    grouped_df = df_filtered.groupby("section")
    output_dict = {}
    for section in sections.values():
        output_dict[section] = (
            grouped_df.get_group(section)[["sqlQuery", "description", "table"]]
            .set_index(["table"])
            .to_dict("index")
        )
    return output_dict


async def get_queries_run_sql(
    workflow,
    ctx: Context,
    ev: GetQueries,
) -> CreateReport | StopEvent:
    workflow.logger.info("Retrieving SQL queries")
    full_data = await workflow.copilot_api.get_content_generator_sqls()

    if full_data is None or len(full_data) == 0:
        workflow.logger.error("No sql statements found")
        return StopEvent(result="No sql statements found")

    output = format_queries(full_data, ev.sections, ev.tables)
    workflow.logger.info("SQL queries retrieved")
    return CreateReport(data=output, project_team=ev.project_team, start_date=ev.start_date, end_date=ev.end_date)
