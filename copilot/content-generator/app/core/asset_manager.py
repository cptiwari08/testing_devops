import asyncio
from typing import Sequence

from app.dependencies import Container
from dependency_injector.wiring import Provide
from sqlalchemy import text


class AssetManager:
    def __init__(self, engine: Provide[Container.asset_manager_engine]):
        self.engine = engine.get_engine()

    def build_query(self, params):
        """
        Build the SQL query based on the provided parameters.
        Returns:
            A tuple of (query_string, query_params) where query_string is a SQL string
            and query_params is a dictionary of parameters for use in a parameterized query.
        """
        # Extract mandatory parameters directly from params dictionary and project_details object
        teams = params.get("team")

        # Check mandatory parameter
        if not teams:
            raise ValueError("'team' is a mandatory parameter.")

        # Extract optional parameters (if they exist, otherwise None)
        project_details = params.get("project_details", {})
        transaction_type = getattr(project_details, "transactionType", None)
        sector = getattr(project_details, "sector", None)
        sub_sector = getattr(project_details, "subSector", None)

        # Start the query without extra indentation inside the triple-quoted string
        query_string = "SELECT title, parenttask, projectteam, workplantasktype FROM Workplan WHERE 1=1"

        query_params = {}

        # option 1
        query_string += (
            " AND (" + " OR ".join([f"projectteam = '{team}'" for team in teams]) + ")"
        )
        query_string_replaced = query_string

        # Optional conditions
        if transaction_type:
            query_string += " AND (_TransactionType LIKE :transaction_type OR _TransactionType IS NULL)"
            query_string_replaced += f" AND (_TransactionType LIKE '%{transaction_type}%' OR _TransactionType IS NULL)"
            query_params["transaction_type"] = f"%{transaction_type}%"

        if sector:
            query_string += " AND (_Sector LIKE :sector OR _Sector IS NULL)"
            query_string_replaced += (
                f" AND (_Sector LIKE '%{sector}%' OR _Sector IS NULL)"
            )
            query_params["sector"] = f"%{sector}%"

        if sub_sector:
            query_string += " AND (_Industry LIKE :subSector OR _Industry IS NULL)"
            query_string_replaced += (
                f" AND (_Industry LIKE '%{sub_sector}%' OR _Industry IS NULL)"
            )
            query_params["subSector"] = f"%{sub_sector}%"

        return query_string, query_params, query_string_replaced

    async def execute_query(
        self, statement: str, parameters: dict | None = None
    ) -> Sequence:
        def awaitable_query_execution(statement, parameters):
            with self.engine.connect() as connection:
                result = connection.execute(text(statement), parameters)
                data = result.fetchall()
            return data

        data = await asyncio.to_thread(awaitable_query_execution, statement, parameters)
        return data
