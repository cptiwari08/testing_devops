import asyncer
import sqlparse.sql
from app.core.interfaces import IBaseLogger
from app.core.ms_server_manager import MSSQLServerManager
from app.ey_ip.config import config
from pypika import Field, Query, Table
from sql_metadata import Parser


def get_document_query(table, where_):

    table_ = Table(table)
    query = Query.from_(table_).select(table_._ID, table_._TemplateFile).distinct()

    document_query = f"{query.get_sql().rstrip(';')} {where_}"
    return document_query


def process_document_data(results, tables_list):
    for row in results:
        document_data = {
            "documentId": row[0],
            "documentName": row[1],
        }
        tables_list.append(document_data)


def get_table_schema(table):
    # Define the table and fields using PyPika
    information_schema_columns = Table("INFORMATION_SCHEMA.COLUMNS")
    column_name = Field("COLUMN_NAME")
    table_name = Field("TABLE_NAME")
    # Build the query
    query = (
        Query.from_(information_schema_columns)
        .select(column_name)
        .where(table_name == table)
    )
    return query.get_sql(quote_char=None)


async def build_citing_sources(
    raw_response, logger: IBaseLogger, doc_query_func=get_document_query
) -> dict:
    citing_sources = {
        "sourceName": "ey-ip",
        "sourceType": "documents",
        "sourceValue": [],
    }
    tables_list = []
    # do this to keep sql outside of the for scope and being able to wrap
    # the latest retrieved sql
    sql = ""
    schema = ""
    ms_sql_server = MSSQLServerManager(config, logger)

    logger.info("Fetching data from the database for documents name")
    source_errors = []
    for source in raw_response.sources:
        # This checks if  source is None or logically false (empty in some way)
        #
        # it also checks if the raw response is bool because the dummy tool
        # fallback function returns True
        if not source or isinstance(source.raw_output, bool):
            continue

        if source.raw_output.source_nodes[0].node.text[:5].lower() == "error":
            source_errors.append(True)
            continue
        else:
            source_errors.append(False)

        sql = source.raw_output.metadata["sql_query"]
        tables = Parser(sql).tables
        statements = sqlparse.parse(sql)[0]
        text_query = get_table_schema(tables[0])
        columns_base = ms_sql_server.execute_text_query(text_query)
        formated_columns = [value[0] for value in columns_base]
        columns = ",".join(formated_columns)
        schema = f"Table called '{tables[0]}' contains the following columns: {columns}"
        # default value if no where clause in SQL statement
        where_ = "where 1 = 1"
        for token in statements.tokens:
            if isinstance(token, sqlparse.sql.Where):
                where_ = token.value

        for table in tables:
            query = doc_query_func(table, where_)
            results = await asyncer.asyncify(ms_sql_server.execute_text_query)(
                query=query
            )
            process_document_data(results, tables_list)

    # Response logic
    if tables_list:
        citing_sources["sourceValue"] = list(tables_list)

    return {
        "source_errors": source_errors,
        "tables_list": tables_list,
        "citing_sources": citing_sources,
        "sql": sql,
        "schema": schema,
    }
