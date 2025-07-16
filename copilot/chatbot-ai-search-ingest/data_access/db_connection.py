import os
import json
import requests
import sys
from msal import ConfidentialClientApplication
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient
from sqlalchemy.engine import URL
from sqlalchemy import create_engine
from dotenv import load_dotenv

load_dotenv(override=True)

# Getting data from Azure
key_vault_url = os.getenv("KEY_VAULT_URL")

# Create a DefaultAzureCredential instance
credential_secret = DefaultAzureCredential()

# Create a SecretClient instance
secret_client = SecretClient(vault_url=key_vault_url, credential=credential_secret)

po_host = os.getenv('HOST_PROGRAM_OFFICE')
po_json_url = f"{po_host}/data/select/AssetDocuments"
po_sql_url = f"{po_host}/copilot/execute-query"


class DBConnection:
    ce_po_api_key = secret_client.get_secret("Ce-API-Key-Po").value
    token_blob_storage =  secret_client.get_secret("ce-am-blob-storage-token").value


    headers = {
        "Accept": "application/json, text/plain, */*",
        "Accept-Language": "en-US,en;q=0.9",
        "ce-api-key": ce_po_api_key,
        "Connection": "keep-alive",
        "Content-Type": "application/json",
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) "
                      "Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0",
    }
    @classmethod
    def execute_query(cls, payload: dict):
        try:
            response = requests.post(
                f"{os.getenv('HOST_PROGRAM_OFFICE')}/copilot/execute-query",
                headers=cls.headers,
                data=json.dumps(payload),
            )
            response.raise_for_status()
            return response.json()
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when making the SQL request")
            raise

    @classmethod
    def suggestions(cls):
        try:
            response = requests.get(f"{os.getenv('HOST_COPILOT_API')}/suggestions", headers=cls.headers)
            response.raise_for_status()
            return response.json()
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when requesting the suggestions")
            raise

    @classmethod
    def get_glossary(cls):
        try:
            response = requests.get(f"{os.getenv('HOST_COPILOT_API')}/glossary", headers=cls.headers)
            response.raise_for_status()
            return response.json()
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when requesting the glossary")
            raise

    @classmethod
    def connect_db_api(cls,payload: dict, po_url: str):
        try:
            response = requests.post(po_url, headers=cls.headers, data=json.dumps(payload))
            print(response)
        except Exception as e:
            e.error("An error occured while trying the request")
            e.error(e)
            sys.exit()
        return response.json()    

    @classmethod
    def create_ey_ip_db_connection(cls):
        try:
            tenant_id = secret_client.get_secret("ce-assistant-ey-ip-tenant-id").value
            client_id = secret_client.get_secret("ce-assistant-ey-ip-client-id").value
            client_secret = secret_client.get_secret("ce-assistant-ey-ip-client-secret").value
            server_eyip = secret_client.get_secret("ce-assistant-ey-ip-server").value
            db_eyip = secret_client.get_secret("ce-assistant-ey-ip-db").value
        except Exception as e:
            e.add_note("Error occurred when fetching information from secret")
            raise

        authority = f'https://login.microsoftonline.com/{tenant_id}'
        scope = ["https://database.windows.net//.default"]

        app = ConfidentialClientApplication(client_id, authority=authority, client_credential=client_secret)
        result = app.acquire_token_for_client(scopes=scope)

        if "access_token" not in result:
            sys.exit()
        access_token = result['access_token']

        try:
            connection_string = (f"DRIVER={{ODBC Driver 18 for SQL Server}};SERVER={server_eyip};DATABASE={db_eyip};"
                                 f"Authentication=ActiveDirectoryServicePrincipal;AccessToken={access_token};"
                                 f"Uid={client_id};Pwd={client_secret}")

            connection_url = URL.create("mssql+pyodbc", query={"odbc_connect": connection_string})
            return create_engine(connection_url)
        except Exception as e:
            e.add_note("CONTEXT: Error occurred when retrieving the information from the EY IP SQL Server")
            raise
    
    @classmethod
    def get_token_blob_storage(cls):
        return cls.token_blob_storage