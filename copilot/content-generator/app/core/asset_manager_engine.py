from app.core.config import AssetManagerConfig
from fastapi import HTTPException, status
from msal import ConfidentialClientApplication
from sqlalchemy import create_engine
from sqlalchemy.engine import URL


class AssetManagerEngine:
    def __init__(self, asset_manager_config: AssetManagerConfig):
        self.asset_manager_config = asset_manager_config

    def get_engine(self):
        authority = (
            f"https://login.microsoftonline.com/{self.asset_manager_config.tenant_id}"
        )
        scope = ["https://database.windows.net//.default"]

        try:
            app = ConfidentialClientApplication(
                self.asset_manager_config.client_id,
                authority=authority,
                client_credential=self.asset_manager_config.client_secret,
            )
            auth_details = app.acquire_token_for_client(scopes=scope)
        except Exception:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Asset manager authentication error",
            )
        access_token = auth_details["access_token"]  # type: ignore

        connection_string = (
            f"DRIVER={{ODBC Driver 17 for SQL Server}};"
            f"SERVER={self.asset_manager_config.server_eyip};DATABASE={self.asset_manager_config.db_eyip};"
            f"Authentication=ActiveDirectoryServicePrincipal;"
            f"AccessToken={access_token};"
            f"Uid={self.asset_manager_config.client_id};Pwd={self.asset_manager_config.client_secret}"
        )

        connection_url = URL.create(
            "mssql+pyodbc", query={"odbc_connect": connection_string}
        )
        return create_engine(connection_url)
