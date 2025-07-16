import hashlib
import json
import math
import os
import re

import time
from datetime import datetime

import pandas as pd
import pyodbc
import requests
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient
from msal import ConfidentialClientApplication
import sys

from logger import Logger

logger = Logger()
logger.info("Starting ingest process")

key_vault_url = os.getenv("KEY_VAULT_URL")

# Create a DefaultAzureCredential instance
credential_secret = DefaultAzureCredential()

# Create a SecretClient instance
client = SecretClient(vault_url=key_vault_url, credential=credential_secret)

ce_po_api_key = client.get_secret("Ce-API-Key-Po").value


def connect_db_own_res(server, db):
    # Azure AD application (client) ID, Secret, and Tenant ID
    try:
        tenant_id = client.get_secret("ce-assistant-ey-ip-tenant-id").value
        client_id = client.get_secret("ce-assistant-ey-ip-client-id").value
        client_secret = client.get_secret("ce-assistant-ey-ip-client-secret").value
    except Exception as e:
        logger.error(f"An error occurred while fetching information from secret: {e}")
        sys.exit()

    # Authority URL
    authority = f'https://login.microsoftonline.com/{tenant_id}'


    # Scope for Azure SQL Database
    scope = ["https://database.windows.net//.default"]

    # Initialize the MSAL confidential client
    app = ConfidentialClientApplication(client_id, authority=authority,
                                        client_credential=client_secret)

    # Acquire token
    result = app.acquire_token_for_client(scopes=scope)

    # Check if the token was acquired
    if 'access_token' not in result:
        logger.error(
            f'Error acquiring token: {result.get("error")}. correlation_id: {result.get("correlation_id")}. {result.get("error_description")}')
        sys.exit()
    access_token = result['access_token']

    # Establish connection using the ODBC Driver
    try:
        _conn = pyodbc.connect(
            f"DRIVER={{ODBC Driver 18 for SQL Server}};SERVER={server};DATABASE={db};Authentication"
            f"=ActiveDirectoryServicePrincipal;AccessToken={access_token};Uid={client_id};Pwd={client_secret}",
            autocommit=False,
        )
        _cursor = _conn.cursor()
        return _conn, _cursor
    except Exception as e:
        logger.error("Connection Error")
        logger.error(e)
        sys.exit()


po_host = os.getenv('HOST_PROGRAM_OFFICE')
po_json_url = f"{po_host}/data/select/AssetDocuments"
po_sql_url = f"{po_host}/copilot/execute-query"


def connect_db_api(payload: dict, po_url: str):
    headers = {
        "Accept": "application/json, text/plain, */*",
        "Accept-Language": "en-US,en;q=0.9",
        "ce-api-key": ce_po_api_key,
        "Connection": "keep-alive",
        "Content-Type": "application/json",
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0",
    }

    try:
        response = requests.post(po_url, headers=headers, data=json.dumps(payload))
    except Exception as e:
        logger.error("An error occured while trying the request")
        logger.error(e)
        sys.exit()
    return response.json()


# Connecting to server and database
conn, cursor = connect_db_own_res(
    client.get_secret("ce-assistant-ey-ip-server").value,
    client.get_secret("ce-assistant-ey-ip-db").value,
)

########################Ingesting process from API ASSET MANAGER########################

# Four (4) variables are defined to rename some columns from _columnMetadata if it necessary (this process is manual).
# In this case, just one column needs to be renamed: column ‘’process name’’ of Nodes for ‘’title’’.
# It means in the file obtained from API for Nodes, the column was named as process name but it will
# be renamed as title.
# For similar_data_tsa, similar_data_vc and similar_data_workplan there are no renames

similar_data_nodes = {
    "process name": "title",
    "process group 1": "function",
    "process group 2": "subfunction",
}
similar_data_tsa = {}
similar_data_vc = {}
similar_data_workplan = {}


# Function to create table
def create_table(df, table_name_filter):
    column_defs = [f"[{col}] VARCHAR(5000)" for col in df.columns.values]
    query = f"CREATE TABLE {table_name_filter} (" + ", ".join(column_defs) + ");"
    try:
        cursor.execute(query)
    except Exception as e:
        logger.error("Error Creating Table")
        logger.error(e)
        sys.exit()
    return None


# Function to alter table in the case
def alter_table(table_name_filter, alter_table):
    alter = "ALTER TABLE {} ADD ".format(table_name_filter) + ', '.join(f"{[x]} VARCHAR(5000)" for x in list(alter_table)).replace("'","")
    try:
        cursor.execute(alter)
    except Exception as e:
        logger.error("Error Altering Table")
        logger.error(e)
        sys.exit()
    return None


# Function to insert into table
def insert_into(df, table_name_filter):
    columns = ', '.join([f"[{x}]" for x in df.columns.values])

    # if table to insert is _generalcontex, there is a special treatment before inserting data
    batch= 1000
    for i in range(0,len(df),batch):
        final_data=r""
        for _, row in df.iloc[i: i+batch].iterrows():
            final_data = final_data + "," + str(tuple([None if pd.isna(value) else str(value).replace("'", "").replace("\n", " ").replace('"', "") for value in row]))

        final_data = final_data[1:].replace('None', 'NULL')
        final_data = final_data.replace(r'"', r"'")
        insert_query = f"INSERT INTO {table_name_filter} ({columns}) VALUES {final_data}"
        try:
            cursor.execute(insert_query)
        except Exception as e:
            logger.error("Error on Insertion")
            logger.error(e)
            sys.exit()    
    return None

def insert_into_general_context(df):
    columns = ', '.join([f"[{x}]" for x in df.columns.values])
    final_data = ""
    for _, row in df.iterrows():
        final_data = (
            final_data
            + ","
            + str(tuple([None if pd.isna(value) else value for value in row]))
        )
    final_data = final_data[1:].replace('"',"'")
    insert_query = f"INSERT INTO _generalContext ({columns}) VALUES {final_data}"
    try:
        cursor.execute(insert_query)
    except Exception as e:
        logger.error("Error on Insertion")
        logger.error(e)
        sys.exit()
    return None


try:
    with open("./payload_asset_manager.json") as f:
        payload_asset_manager = json.load(f)

    response_json = connect_db_api(
        payload_asset_manager,
        po_json_url,
    )
    data_api = response_json

    ########################Ingesting process from API ASSET MANAGER########################

    # Get the previous run of the process and save into old_info_ingest to compare with data_api
    # obtained from the API ASSET MANAGER at the beginning
    try:
        query = "SELECT content, _dateInserted FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY _dateInserted DESC) AS ROW_NUM FROM _infoIngest) F WHERE ROW_NUM=1;"
        old_info_ingest = pd.read_sql(query, conn)
        old_info_ingest = json.loads(old_info_ingest['content'][0])
    except Exception as e:
        logger.error("No data into database")
        old_info_ingest = []

    ########################Ingesting process from API ASSET MANAGER########################

    # Saving folder from old API data
    folders = []

    # Saving archives from old API data
    archives = []

    # Going through each dictionary found in old API data and saving into folders if it is a folder or archive if it is a file
    for data in old_info_ingest:
        if data['IsFolder'] is True:
            folders.append(data)
        if data['Parent'] is not None and data['IsFolder'] is False:
            archives.append(data)

    # Saving folder from API data
    folders_after = []

    # Saving archives from API DATA
    archives_after = []

    # Going through each dictionary found in API data and saving into folders if it is a folder or archive if it is a file
    for data in data_api:
        if data['IsFolder'] is True:
            folders_after.append(data)
        if data['Parent'] is not None and data['IsFolder'] is False:
            archives_after.append(data)

    # Going through each dictionary found in old API data and getting the name of the Tables (Nodes, TsaItems, ValueCaptureInitiatives and WorkPlan)
    table_name_id_dict = {folder["ID"]: folder["TableName"] for folder in folders}
    table_name_id_dict_after = {
        folder["ID"]: folder["TableName"] for folder in folders_after
    }

    new_archives = []
    for archive in archives_after:
        # print(archive)
        tableNameFilter = table_name_id_dict_after[archive['Parent']['ID']]
        if tableNameFilter is not None:
            new_archives.append(archive)

    archives_after = new_archives

    old_archives = []
    for archive in archives:
        # print(archive)
        tableNameFilter = table_name_id_dict[archive['Parent']['ID']]
        if tableNameFilter is not None:
            old_archives.append(archive)

    archives = old_archives

    # Going through each dictionary found in API data and getting the name of the Tables (Nodes, TsaItems, ValueCaptureInitiatives and WorkPlan)
    tables = []
    for i in folders_after:
        if i['TableName'] is not None:
            tables.append(i['TableName'])

    # Geting data from euwdsatbotsql03.database.windows.net and database EUWDSATBOTSDB01
    generator_expr = (str(valor) for valor in tables)
    separator = r"','"
    result_string = separator.join(generator_expr)
    query = "SELECT distinct tableName, columnName, Title, Description FROM _ColumnMetadata WHERE TableName in ('{}');".format(
        result_string
    )
    data_json = connect_db_api({"SqlQuery": query, "tables": []}, po_sql_url)
    df_metadata = pd.DataFrame.from_records(data_json)

    # Comparision between data_api and old_info_ingest to see if there is any general change

    # Get checksum for each old folder
    folder_md_sum_id = {folder['ID']:[str(folder), folder['TableName']] for folder in folders}

    # Get checksum for each new folder
    folder_md_sum_id_after = {folder['ID']:[str(folder),folder['TableName']] for folder in folders_after}

    # Get checksum for each old archive
    lookup = {item['ID']: item['TableName'] for item in folders}
    archive_md_sum_id = {archive['ID']:[str(archive),lookup[archive['Parent']['ID']]] for archive in archives}

    # Get checksum for each new archive
    lookup_after = {item['ID']: item['TableName'] for item in folders_after}
    archive_md_sum_id_after = {archive['ID']:[str(archive),lookup_after[archive['Parent']['ID']]] for archive in archives_after}

    ########################Ingesting process from API ASSET MANAGER########################

    # Check old data  - new data
    arc_check1 = set(archive_md_sum_id.keys()) - set(archive_md_sum_id_after.keys())

    # Checking new data - old data
    arc_check2 = set(archive_md_sum_id_after.keys()) - set(archive_md_sum_id.keys())

    new_data = []
    repeated_data = []
    to_delete_data = []

    # Comparison is done for each ID of archive

    # case 1
    # {old} - {new} = {}
    # {new} - {old} = {data}
    if arc_check1 == set() and arc_check2 != set():
        print("case1")
        for i in archives_after:
            if i['ID'] in archive_md_sum_id.keys():
                    # Check at level of checksum to see if there is any change. If there is no change, it will add
                    # archive ID to repeated_data variable
                    if archive_md_sum_id_after[i['ID']][0] == archive_md_sum_id[i['ID']][0]:
                        repeated_data.append(i['ID'])
                    else:
                        # Check at level of checksum to see if there is any change. If there is change, it will add
                        # archive ID to to_delete_data and archive to new_data
                        to_delete_data.append({i['ID']: archive_md_sum_id[i['ID']][-1]})
                        new_data.append(i)
            else:
                # if new ID is not in older data, it will add archive new_data
                new_data.append(i)

    # case 2
    # {old} - {new} = {data}
    # {new} - {old} = {data}

    # case 3
    # {old} - {new} = {data}
    # {new} - {old} = {}
    if (arc_check1 != set() and arc_check2 != set()) or (arc_check1 != set() and arc_check2 == set()):
        print("case2-case3")
        for i in archives_after:
            if i['ID'] in archive_md_sum_id.keys():
                    # Check at level of checksum to see if there is any change. If there is no change, it will add
                    # archive ID to repeated_data variable
                    if archive_md_sum_id_after[i['ID']][0] == archive_md_sum_id[i['ID']][0]:
                        repeated_data.append(i['ID'])
                    else:
                        # if there is any change at level of checksum then it will add archive ID to to_delete_data variable
                        # and archive to new:data
                        to_delete_data.append({i['ID']: archive_md_sum_id[i['ID']][-1]})
                        new_data.append(i)
            else:
                # if new ID is not in older data, it will add archive new_data
                new_data.append(i)

        # check old data in new data
        for i in archives:
            if i['ID'] not in archive_md_sum_id_after.keys():
                to_delete_data.append({i['ID']: archive_md_sum_id[i['ID']][-1]})

    # case 4
    # {old} - {new} = {}
    # {new} - {old} = {}
    if arc_check1 == set() and arc_check2 == set():
        print('case4')
        for i in archives_after:
            if i['ID'] in archive_md_sum_id.keys():
                if archive_md_sum_id_after[i['ID']][0] == archive_md_sum_id[i['ID']][0]:
                    repeated_data.append(i['ID'])
                else:
                    to_delete_data.append({i['ID']: archive_md_sum_id[i['ID']][-1]})
                    new_data.append(i)

    for value in to_delete_data:
        query = "DELETE FROM {} WHERE _ID ='{}'".format(list(value.values())[0], list(value.keys())[0])
        try:
            cursor.execute(query)
        except Exception as e:
            logger.error("Error while Deleting")
            logger.error(e)
            sys.exit()

    # Necessary token to get data from Blob storage
    token_blob_storage = client.get_secret("ce-am-blob-storage-token").value

    # For each archive in new_data, it runs a script to read file from blob sotarge, adding some metadata, alter table
    # in the case the readed file has more columns than the table in the database and INSERT INTO final table
    for archive in new_data:
        now = datetime.now()
        table_name_filter = table_name_id_dict_after[archive['Parent']['ID']]

        new_column = df_metadata[df_metadata['tableName'] == table_name_filter]
        new_column = dict(zip(new_column['Title'].str.lower(), new_column['columnName']))

        # Get data from blob storage and read through pandas
        url = archive['BlobUrl']
        url = url + "?" + token_blob_storage
        # Check if the file has one or more sheets and process the correct sheet that has the information
        tabs = pd.ExcelFile(url).sheet_names

        # Reading file: check if TableNameFilter is Nodes because of the sheets, in some cases for Nodes there are more
        # than 1 sheet and the correct sheet is called "Data"

        if len(tabs) > 1:
            if table_name_filter == 'Nodes':
                df_archive = pd.read_excel(url, sheet_name="Data")  # , sheet_name="Data", header=1)
            else:
                df_archive = pd.read_excel(url, sheet_name="Data", header=1)
        else:
            df_archive = pd.read_excel(url)
        header = df_archive.columns.str.lower()

        # Verifying dictionaries to check the hard-coded columns that are not present in _columnMetadata. Example, for
        # table Nodes process name --> title. and get the new header

        new_header1 = []
        if table_name_filter == 'Nodes':
            get_table = similar_data_nodes
        elif table_name_filter == 'TSAItems':
            get_table = similar_data_tsa
        elif table_name_filter == 'ValueCaptureInitiatives':
            get_table = similar_data_vc
        else:
            get_table = similar_data_workplan

        for item in header:
            if item in get_table:
                new_header1.append(get_table[item])
            else:
                new_header1.append(item)
        # Setting new name of columns to the header
        df_archive.columns = new_header1

        # New header to compare each item with Title column in header to get ColumnName from _ColumnMetadata
        new_header = []
        for item in new_header1:
            if item in new_column:
                new_header.append(new_column[item].lower())
            else:
                new_header.append(item.lower())

        # Deleting -id- string at the end of the name of columns.
        new_header = [i[:-2] if i.endswith('id') and len(i) > 2 else i for i in new_header]
        df_archive.columns = new_header

        # Creating dictionary with metadata fields
        add_columns = {
            "_Area": (
                archive.get("Area")
                if archive.get("Area") is None
                else archive.get("Area").get("Title")
            ),
            "_Region": (
                archive.get("Region")
                if archive.get("Region") is None
                else archive.get("Region").get("Title")
            ),
            "_TransactionType": (
                archive.get("TransactionType")
                if archive.get("TransactionType") is None
                else archive.get("TransactionType").get("Title")
            ),
            "_Sector": (
                archive.get("Sector")
                if archive.get("Sector") is None
                else archive.get("Sector").get("Title")
            ),
            "_BlobUrl": archive.get("BlobUrl"),
            "_Industry": (
                archive.get("Industry")
                if archive.get("Industry") is None
                else archive.get("Industry").get("Title")
            ),
            "_Modified": archive.get("Modified"),
            "_Created": archive.get("Created"),
            "_TemplateFile": archive.get("Title"),
            "_Author": (
                archive.get("Author")
                if archive.get("Author") is None
                else archive.get("Author").get("ID")
            ),
            "_Editor": (
                archive.get("Editor")
                if archive.get("Editor") is None
                else archive.get("Editor").get("ID")
            ),
            "_validFrom": archive.get("ValidFrom"),
            "_validTo": archive.get("ValidTo"),
            "_ID": archive.get("ID"),
            "_dateInserted": str(now),
        }

        # Inserting metadata in the final dataframe we are building from the beginning.
        for col_name, col_data in add_columns.items():
            df_archive[col_name] = col_data

        # Deleting duplicate columns. In the case of workplan we have duplicate columns (parentaskID)

        df_archive = df_archive.loc[:, ~df_archive.columns.duplicated()]

        try:
            # If table exists this block will execute
            cursor.execute("SELECT * from {};".format(table_name_filter))
            cursor.fetchall()
            time.sleep(3)
            # Getting top 1 of table in order to save name of the existing columns
            query_columns = "SELECT top 1 * FROM  {};".format(table_name_filter)
            df_columns = pd.read_sql(query_columns, conn)
            alter_table_var = set(df_archive.columns) - set(df_columns.columns)
            # If there is a difference between columns in the table of the database and the columns of the dataframe
            # we are going to insert then we have to run alter table

            if len(alter_table_var) == 0:

                cursor.execute("SELECT * from {};".format(table_name_filter))
                cursor.fetchall()
                time.sleep(3)
                if len(df_archive) != 0:
                    insert_into(df_archive, table_name_filter)
            else:
                alter_table(table_name_filter, alter_table_var)
                if len(df_archive) != 0:
                    insert_into(df_archive, table_name_filter)

        except Exception as e:
            # If table doesn't exists this block will execute and first create the table and the insert into
            create_table(df_archive, table_name_filter)
            if len(df_archive) != 0:
                insert_into(df_archive, table_name_filter)


    ######################## CREATING TABLE _infoIngest TO SAVE INFORMATION ABOUT THE PROCESS########################

    # Saving json from API into SQL
    # Table to create: _infoIngest

    now = datetime.now()
    jsondata = json.dumps(data_api).replace("'", "''")

    try:
        # If table exists this block will execute
        cursor.execute("SELECT * from _infoIngest")
        insert_query = f"INSERT INTO _infoIngest (content, _dateInserted) VALUES ('{jsondata}', '{now}')"
        cursor.execute(insert_query)
    except:
        # If table doesn't exists this block will execute and first create the table and the insert into
        try:
            cursor.execute("CREATE TABLE _infoIngest ([content] [varchar](max) NULL,[_dateInserted] [varchar](max) NULL)")
            insert_query = f"INSERT INTO _infoIngest (content, _dateInserted) VALUES ('{jsondata}', '{now}')"
            cursor.execute(insert_query)
        except Exception as e:
            logger.error("Error on insertion")
            logger.error(e)
            sys.exit()

    ######################## CREATING TABLE _generalContext THAT NEED TO BE INDEXED########################

    # Create dynamic and static context
    # Table to create: _generalContext
    with open("./static_data_gc.json") as f:
        static_data = json.load(f)

    # For dynamic context: Check each table of Nodes, TsaItems,
    # ValueCaptureInitiatives and WorkPlan and get just those columns with less than 21 different values

    for i in static_data:
        i['text'] = i['text'].replace("'","''")
        
    generator_expr = (str(valor) for valor in tables)
    separator = r"','"
    result_string = separator.join(generator_expr)
    query = "SELECT COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME in ('{}');".format(
        result_string
    )
    df_columns = pd.read_sql(query, conn)

    delete_columns = {
        "_Modified",
        "_Created",
        "_validFrom",
        "_validTo",
        "_dateInserted",
        "_ID",
        "_BlobUrl",
        "isdirty",
        "_ArchiveTitle",
        "id",
        "isitemactive",
        "isfolder",
    }
    text_list = []
    for table in tables:
        string_query = ""
        filtered_columns_dict = []
        for row in (
            set(df_columns[df_columns["TABLE_NAME"] == table]["COLUMN_NAME"])
            - delete_columns
        ):
            string_query += "COUNT(DISTINCT[{}]) AS [{}],".format(row, row)
        query = "SELECT {} FROM {}".format(string_query[0:-1], table)
        df_conteo = pd.read_sql(query, conn)

        header = ""
        for i in df_conteo.columns[
            (df_conteo.max() < 21) & (df_conteo.max() > 0)
        ].to_list():
            header = header + "[" + i.replace("'", "") + "] AS [" + i + "],"

        if header:  # Only proceed if header is not empty
            query2 = "SELECT {} from {}".format(header[0:-1], table)
            df_depu = pd.read_sql(query2, conn)
            for col in df_depu.columns:
                lista = df_depu[col].unique()
                text_list.append({
                    "text": f"When asked for something in {', '.join(f'{x}' for x in lista if x is not None)} filter column {col}, from {table}",
                    "tag": "dynamic",
                    "table": table,
                })

    text_list = pd.DataFrame(text_list)
    final_df = pd.DataFrame()
    final_df = final_df.from_records(static_data)
    final_df = pd.concat([final_df, text_list], ignore_index=True)

    try:
        cursor.execute("DROP TABLE  IF EXISTS _generalContext")
    except Exception as e:
        logger.error("Error while Dropping Table")
        logger.error(e)
        sys.exit()

    create_table(final_df, table_name_filter='_generalContext')
    insert_into_general_context(final_df)

    conn.commit()
    logger.info("Ingestion process: correct")
except ValueError as e:
    logger.error("Ingestion process: failed")
    logger.error(e)
    conn.close()
