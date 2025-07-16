from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import get_tables_from_sql_query
from app.project_data.services.program_office_api import ProgramOffice


async def query_response_parser(
    query_output: str, context: QueryPipelineContext
) -> str:
    context.logger.info("I executed the query parser function component")
    program_office = ProgramOffice()

    query_output = query_output.replace("as Function", "as FunctionName").replace("AS Function", "as FunctionName")
    tables = get_tables_from_sql_query(query_output)
    payload = {
        "SqlQuery": query_output,
        "tables": tables,
    }
    context.logger.info("I made a request to the program office")
    response = await program_office.run_query(payload, context.token)
    # Create the json result only with first 100 rowa to avoid exceed token for LLM
    output = " \n".join(str(response_item) for response_item in response[:100])
    if not output:
        return "Query result is empty"
    return output
