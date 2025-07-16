from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import get_tables_from_sql_query
from app.project_data.services.program_office_api_retry import ProgramOffice


async def query_response_parser_retry(
    query_output: str, context: QueryPipelineContext
) -> dict:
    context.logger.info("I then executed the query response parser function component")
    program_office = ProgramOffice()

    query_output = query_output.replace("as Function", "as FunctionName").replace("as Group", "as GroupName").replace(
        "AS Function", "as FunctionName"
    ).replace(
        "AS Group", "as GroupName"
    )
    tables = get_tables_from_sql_query(query_output)
    payload = {
        "SqlQuery": query_output,
        "tables": tables,
    }
    context.logger.info("I made a request to the program office")
    response = await program_office.run_query(
        payload, context.token, return_raw_response=False
    )
    if response.get("error"):
        return {
            "intention": "retry",
            "output": response.get("response"),
            "url": response.get("url"),
            "message_error": response.get("message_error"),
        }
    else:
        # Create the json result only with first 100 rowa to avoid exceed token for LLM
        # response = response.json()
        output = " \n".join(
            str(response_item) for response_item in response.get("response")[:100]
        )
        if not output:
            return {
                "intention": "normal",
                "output": "Query result is empty",
                "url": None,
            }
        return {"output": output, "intention": "normal", "url": None}
