import asyncio

from app.core.config import ResponseOptions
from app.core.pydantic_models import MetricInput, QueryPipelineContext
from app.core.utils import calculate_final_score, get_tables_from_sql_query
from app.metrics.rag_sql_evaluator import RagSQLEvaluator
from app.metrics.sql_evaluator import SQLEvaluator
from app.metrics.strategy_context import Context
from app.project_data.functions.table_keys import get_table_keys
from app.project_data.functions.table_title_name import title_table_name


async def response_post_processing(
    raw_response: dict, context: QueryPipelineContext, sql_query: str = ""
) -> dict:
    sql_schema = raw_response.get("denormalized_query_generator", {}).get("output")
    sql_result = raw_response.get("query_response_parser", {}).get("output")
    if not sql_result:
        sql_result = raw_response.get("response_output_parser", {}).get("output")
    if not sql_query:
        sql_query = raw_response["query_parser_output"]["output"]
    context.logger.info("I generated the answer based on all the processed information")
    context.logger.info("I calculated the score to evaluate the quality of the response")
    ### Related to response quality
    tasks = {}
    final_answer_llm = ""
    if 'final_answer_llm' in raw_response:
        final_answer_llm = raw_response["final_answer_llm"]["output"].message.content

    response = {
        "backend": "project-data",
        "response": final_answer_llm,
        "citingSources": [],
        "sql": {
                "query": sql_query,
                "result": sql_result,
            }
    }

    context.logger.info("Proceding to retrieve the tables from the SQL query")
    tables = get_tables_from_sql_query(sql_query)
    if raw_response.get("citing_sources_output_parser").get('output'):
        tasks["title_table_name"] = asyncio.create_task(
            title_table_name(tables, context.token)
        )
        tasks["get_tables_keys"] = asyncio.create_task(
            get_table_keys(tables, context.token)
        )
        results_list = await asyncio.gather(*tasks.values())
        # this is to get the tasks by keys instead of list indexes
        results = dict(zip(tasks.keys(), results_list))
        display_tables = results.get("title_table_name")
        get_keys = results.get("get_tables_keys")
        sql_evaluator = results.get("sql_evaluator")
        rag_sql_evaluator = results.get("rag_sql_evaluator")
        response |= {"score": None}
        context.logger.info("Now, I'm updating the response")
        page_keys = raw_response["citing_sources_output_parser"]["output"]
        response.update(
            {
                "citingSources": [
                    page_keys,
                    {
                        "sourceName": "project-data",
                        "sourceType": "table",
                        "sourceValue": (
                            [x["Title"] for x in display_tables]  # type: ignore
                            if len(display_tables) > 0  # type: ignore
                            else []
                        ),
                    },
                ],
            }
            )
        return response
    ### This is ALWAYS suggestion flow
    else:
        if tables:
            tasks["title_table_name"] = asyncio.create_task(
                title_table_name(tables, context.token)
            )
            tasks["get_tables_keys"] = asyncio.create_task(
                get_table_keys(tables, context.token)
            )
            results_list = await asyncio.gather(*tasks.values())
            # this is to get the tasks by keys instead of list indexes
            results = dict(zip(tasks.keys(), results_list))
            display_tables = results.get("title_table_name")
            get_keys = results.get("get_tables_keys")
            sql_evaluator = results.get("sql_evaluator")
            rag_sql_evaluator = results.get("rag_sql_evaluator")
            # final_score = calculate_final_score([sql_evaluator, rag_sql_evaluator])
            response |= {"score": None}
            context.logger.info("Now, I'm updating the response")
            response.update(
                {
                    "citingSources": [
                        {
                            "sourceName": "project-data",
                            "sourceType": "table",
                            "sourceValue": (
                                [x["Title"] for x in display_tables]  # type: ignore
                                if len(display_tables) > 0  # type: ignore
                                else []
                            ),
                        },
                    ],
                }
            )
            return response
        else:
            results_list = await asyncio.gather(*tasks.values())
            # this is to get the tasks by keys instead of list indexes
            results = dict(zip(tasks.keys(), results_list))
            sql_evaluator = results.get("sql_evaluator")
            rag_sql_evaluator = results.get("rag_sql_evaluator")
            final_score = calculate_final_score([sql_evaluator, rag_sql_evaluator])
            response |= {"score": final_score}
            return response
