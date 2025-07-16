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
        ### Include the eyip ids in the list of params
        eyip_ids_list = params.get("eyip_ids",None)

        # Start the query without extra indentation inside the triple-quoted string
        query_string = "SELECT title,serviceinscopedescription  FROM TSAItems WHERE 1=1"
        query_params = {}

        ## Add ids
        if eyip_ids_list:
            teams = eyip_ids_list[0].mappedIPProjectTeams
            eyip_ids = [str(x) for x in eyip_ids_list[0].templates]
            query_string += " AND (" + " OR ".join([f"projectteam = '{team}'" for team in teams]) + ")"
            query_string += " AND (_ID IN (" + ",".join(eyip_ids) + "))"
            query_params['eyip_ids'] = eyip_ids

        return query_string, query_params



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
