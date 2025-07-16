import time

from app.core.interfaces import IBaseLogger
from app.core.pydantic_models import MSServerConfig
from sqlalchemy import create_engine
from sqlalchemy.engine import Engine
from sqlalchemy.exc import ProgrammingError
from sqlalchemy.sql import text


class MSSQLServerManager:
    """
    This class only creates the engine;
    all the database logic is handled by llama.
    """

    def __init__(self, config: MSServerConfig, logger: IBaseLogger) -> None:
        self._logger = logger
        self._config = config
        self.engine = self.get_engine()

    def get_engine(self) -> Engine:
        connection_string = (
            f"Driver={{ODBC Driver 17 for SQL Server}};"
            f"Server={self._config.server};"
            f"Database={self._config.database};"
            f"Authentication=ActiveDirectoryServicePrincipal;"
            f"UID={self._config.client_id};"
            f"Scopes=openid;"
            f"PWD={self._config.client_secret};"
            f"TenantID={self._config.tenant_id};"
        )
        self._logger.info("Creating SQL Server engine")
        start = time.time()
        self.engine = create_engine(
            "mssql+pyodbc:///?odbc_connect=" + connection_string
        )
        end = time.time()
        self._logger.info(f"SQL Server engine created in {end - start} seconds")

        return self.engine

    def execute_text_query(self, query):
        engine = self.engine
        connection = None
        try:
            connection = engine.connect()
            result = connection.execute(text(query))
            return result.fetchall()
        except ProgrammingError as e:
            self._logger.error(
                "Error fetching data from the database on EY IP database"
            )
            raise ProgrammingError(query, {}, e)
        finally:
            # Manually close to avoid issues when called async
            if connection:
                connection.close()
                self._logger.info("SQL Server connection closed")
