from typing import Any

from app.project_data.services.program_office_api import ProgramOffice
from pypika import Query, Table


async def title_table_name(tables: list, token: str) -> str | Any:
    program_office = ProgramOffice()

    table_metadata = Table("_TableMetadata")

    query = (
        Query.from_(table_metadata)
        .select(table_metadata.Title)
        .where(table_metadata.TableName.isin(tables))
    )

    payload = {
        "SqlQuery": str(query),
        "tables": tables,
    }

    response = await program_office.run_query(payload, token)

    if not response:
        return []
    return response
