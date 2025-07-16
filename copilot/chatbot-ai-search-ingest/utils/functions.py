import json
import os
import sys
from sql_metadata import Parser
import pandas as pd
import numpy as np
import uuid
import ast
import time
from datetime import datetime
from data_access.db_connection import DBConnection
from utils.logger import Logger
from sqlalchemy import insert, Table, MetaData
from sqlalchemy.sql import text

log = Logger()

metadata_obj = MetaData()


def row_processor(row):
    answer = "TableName: " + row.TableName + " and Description: " + row.Description
    if row.question:
        if len(row.question) >= 5:
            answer += " and questions that can be answered: " + ', '.join(row.question[0:5])
        if len(row.question) < 5 and isinstance(row.question[0], str):
            answer += " and questions that can be answered: " + ', '.join(row.question[0:len(row.question)])
    return answer

def processing_data_index(data):
    data_false = pd.DataFrame.from_records(data)
    data_false = data_false[data_false['isMainTable'] == 'False']
    data_false['question'] = None
    return data_false

def processing_data_suggestions(data):
    suggestions = []
    for i in data:
        if i['answerSQL'] is not None and i['answerSQL'].find("WITH") == -1 and i['answerSQL'].find("DELETE") == -1 and i['suggestionText'] != '':
            suggestions.append({'question': i['suggestionText'],
                                'source': i['source'],
                                'appAffinity': i['appAffinity'],
                                'sqlQuery': i['answerSQL'],
                                'mainTable': Parser(i['answerSQL']).tables[0]})
    return suggestions

def processing_data_index_1(data, suggestions,data_false):
    data_desc = pd.DataFrame.from_records(data)
    final_data = pd.DataFrame(suggestions)
    final_data['mainTable'] = final_data['mainTable'].str.lower().replace("[^A-Za-z]", "", regex=True)
    data_desc['TableName_join'] = data_desc['TableName'].str.lower()
    data_true = data_desc.merge(final_data, left_on='TableName_join', right_on='mainTable', how='left', validate='one_to_many')
    agg_df = data_true.groupby(['TableName', 'Description', 'isMainTable'])['question'].agg(lambda x: list(x)).reset_index()
    #data_false = processing_data_index(data)
    final_second_index = pd.concat([data_false, agg_df])
    return final_second_index

def processing_data_index_2(data):
    glossary = []
    for entry in data:
        record = {
            'context': entry['context'],
            'tableName': entry['tableName'],
            'tag': entry['tag'],
            'isSchema': str(entry['isSchema']).capitalize()
        }
        glossary.append(record)
    return glossary

def processing_lookup_query(data):
    return "SELECT " + ",".join(
        [
            f"(SELECT CASE WHEN COUNT(1) < 50 THEN STRING_AGG(CAST({x['LookupField']} AS VARCHAR(MAX)),', ') ELSE NULL END from (SELECT distinct {x['LookupField']} from {x['ref_table']}) t) AS {x['ref_table']}_{x['LookupField']}"
            for x in data
        ]
    )

def processing_lookup_dict(data):
    return {item['lkup']: item['main_table'] for item in data}

def processing_lookup_values(data,lkup_dictionary, glossary):
    data = data[0]
    for x in list(data.keys()):
        if data[x] is not None and len(lkup_dictionary[x]) < 1024:
        
            text = {
                "context": f"When asked about for something related to {data[x]} use the {x.split('_')[1]} column in table "
                + x.split("_")[0],
                "tableName": lkup_dictionary[x],
                "tag": "dynamic",
                "isSchema": "False"
            }
            glossary.append(text)
    return glossary

def processing_lkup_query2(data, glossary):
    for x in data:
        if len(x["tableName"]) <= 1024:
            text = {"context": x["context"], "tableName": x["tableName"],
                        "tag": "dynamic",
                        "isSchema": "False"}
            glossary.append(text)
    return glossary

def processing_lkup_query3(data, glossary):
    for x in data:
        #if len(x['tableSchema'])<=1024:
        text = {"context": x['tableSchema'], 
                "tableName": "|"+x['TableName']+"|", 
                "tag": "dynamic", 
                "isSchema": "True"}
        glossary.append(text)

    glossary = [i for i in glossary if not (i['context'] == "When asked about for something related to Account Disabled, Project Disabled, Project Enabled use the Title column in table AccountStatuses")]

    return glossary

def processing_suggestions(data):
    suggestions = []
    for i in data:
        if i['suggestionText'] != '':
            suggestions.append(
                {
                    "question": i["suggestionText"],
                    "source": i["source"],
                    "appAffinity": i["appAffinity"],
                    "sqlQuery": i["answerSQL"],
                    "id": i["id"],
                    "visibleToAssistant": i["visibleToAssistant"]
                }
            )
    return suggestions

def processing_general_index1(data, last_data, indexType):
    
    if last_data == []:
        last_data = []
    for _, row in data.iterrows():
        emb = "TableName: " +  row['TableName']+ " isMainTable: "+ row['isMainTable'] + " "+ row_processor(row) 
        line = {"id": str(uuid.uuid4()), #hash unico por chunk
        "embedding_text": emb,
        "chunk": row_processor(row),
        "metadata":{"prjdata":{"tableName" : str(row['TableName']), 
                        "isMainTable" : row['isMainTable'], 
                        }},
        "indexType": indexType}
        last_data.append(line)
    return last_data

def processing_general_index2(data, last_data, indexType):
    
    if last_data == []:
        last_data = []
    for row in data:
        emb = "tableName: " + row['tableName'] + " tag: "+ row['tag'] + " isSchema:" + row['isSchema'] + (" dynamicFields: " + row['dynamicFields'] if 'dynamicFields' in row.keys() else "") + " " + row['context'] 
        line = {"id": str(uuid.uuid4()), #hash unico por chunk
        "embedding_text": emb,
        "chunk": row['context'],
        "metadata":{"prjgloss":
                    {"tableName" : row['tableName'], 
                        "tag" : row['tag'], 
                        "isSchema":row['isSchema'],
                        "dynamicFields":row['dynamicFields'] if 'dynamicFields' in row.keys() else None
                        }},
        "indexType": indexType}
        last_data.append(line)
    return last_data

def processing_general_index3(data, last_data, indexType):
    
    if last_data == []:
        last_data = []
    for row in data:
        if row['suggestionText'] != '':
            emb = row['suggestionText']
            line = {"id": str(uuid.uuid4()), #hash unico por chunk
            "embedding_text": emb,
            "chunk": row['suggestionText'],
            "metadata":{"prjsuggestions":
                        {"source" : row['source'], 
                            "appAffinity" : row['appAffinity'], 
                            "sqlQuery":row['answerSQL'],
                            "idSuggestion":row['id'],
                            "visibleToAssistant":row['visibleToAssistant'],
                            "isIncluded":row['isIncluded']
                            }},
            "indexType": indexType}
        last_data.append(line)
    return last_data

def processing_general_index4(data, last_data, indexType):
    
    if last_data == []:
        last_data = []
    for _, row in data.iterrows():
        emb = row['description']
        line = {"id": str(uuid.uuid4()), #hash unico por chunk
            "embedding_text": emb,
            "chunk": row['description'],
            "metadata":{"prjdatacitingsources":
                        {"AppId" : str(row['AppId']), 
                            "AppName" : row['AppName'], 
                            "AppKey" : row['AppKey'],
                            "PageHeader": row["PageHeader"],
                            "HREF": row["HREF"],
                            "PageKey": row["PageKey"],
                            "TeamType": row["TeamType"]}},
            "indexType": indexType}
        last_data.append(line)
    return last_data

def processing_lkup_query4(data, glossary):
    for x in data:
        text = {"context": x['context'], "tableName": x['TableName'], "tag": "dynamic_fields", "isSchema": "False", #"dynamicFields": "True"
                }
        glossary.append(text)
    return glossary

def processing_citing_sources(data):
    new_index = pd.DataFrame.from_records(data)
    return new_index

#####comparison

def process_data_endpoint(data_api,old_info_ingest):

    folders = []
    archives = []
    for data in old_info_ingest:
        if data['IsFolder'] is True:
            folders.append(data)
        if data['Parent'] is not None and data['IsFolder'] is False:
            archives.append(data)

    folders_after = []
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

    tables = []
    for i in folders_after:
        if i['TableName'] is not None:
            tables.append(i['TableName'])
    generator_expr = (str(valor) for valor in tables)
    separator = r"','"
    result_string = separator.join(generator_expr)
    query = "SELECT distinct tableName, columnName, Title, Description FROM _ColumnMetadata WHERE TableName in ('{}');".format(
        result_string
    )
    return folders,archives, folders_after, archives_after, tables, query, table_name_id_dict_after

def process_data_db(data_json,folders,archives, folders_after, archives_after):
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

    return archive_md_sum_id, archive_md_sum_id_after, df_metadata

def check_data(archive_md_sum_id, archive_md_sum_id_after):
    arc_check1 = set(archive_md_sum_id.keys()) - set(archive_md_sum_id_after.keys())

    # Checking new data - old data
    arc_check2 = set(archive_md_sum_id_after.keys()) - set(archive_md_sum_id.keys())
    return arc_check1,arc_check2

def general_process(arc_check1,arc_check2,folders,archives, folders_after, archives_after,archive_md_sum_id, archive_md_sum_id_after):
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
    return new_data,repeated_data,to_delete_data


def create_table(df, table_name_filter, conn):
    column_defs = [f"[{col}] VARCHAR(5000)" for col in df.columns.values]
    query = f"CREATE TABLE {table_name_filter} (" + ", ".join(column_defs) + ");"
    try:
        conn.execute(text(query))
    except Exception as e:
        log.error("Error Creating Table")
        log.error(e)
        sys.exit()
    return None


# Function to alter table in the case
def alter_table(table_name_filter, alter_table, conn):
    alter = "ALTER TABLE {} ADD ".format(table_name_filter) + ', '.join(f"{[x]} VARCHAR(5000)" for x in list(alter_table)).replace("'","")
    try:
        conn.execute(text(alter))
    except Exception as e:
        log.error("Error Altering Table")
        log.error(e)
        sys.exit()
    return None


# Function to insert into table
def insert_into(df, table_name_filter, conn):
    table = Table(table_name_filter, metadata_obj, autoload_with=conn)

    # if table to insert is _generalcontex, there is a special treatment before inserting data
    batch = 1000
    stmt = insert(table)
    for i in range(0, len(df), batch):

        final_data = (df.iloc[i: i+batch]
                      .fillna(np.nan)
                      .replace(r'\n', ' ', regex=True)
                      .replace([np.nan], [None])
                      .astype(str)
                      .replace('None', None)
                      .to_dict(orient="records"))

        try:
            conn.execute(stmt, final_data)
        except Exception as e:
            log.error("Error on Insertion")
            log.error(e)
            sys.exit()    
    return None

def insert_into_general_context(df, conn):
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
        conn.execute(text(insert_query))
    except Exception as e:
        log.error("Error on Insertion")
        log.error(e)
        sys.exit()
    return None

def process_data_final(to_delete_data,new_data, token_blob_storage,similar_data_nodes,similar_data_tsa,similar_data_vc, similar_data_workplan, df_metadata, table_name_id_dict_after, engine,data_api, tables):
    
    engine = DBConnection.create_ey_ip_db_connection()
    with engine.begin() as conn:
        for value in to_delete_data:
            query = "DELETE FROM {} WHERE _ID ='{}'".format(list(value.values())[0], list(value.keys())[0])
            try:
                conn.execute(text(query))
            except Exception as e:
                log.error("Error while Deleting")
                log.error(e)
                sys.exit()

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
                
                final = conn.execute(text("SELECT * from {};".format(table_name_filter)))
                final.fetchall()
                time.sleep(3)
                # Getting top 1 of table in order to save name of the existing columns
                query_columns = "SELECT top 1 * FROM  {};".format(table_name_filter)
                df_columns = pd.read_sql(query_columns, conn)
                alter_table_var = set(df_archive.columns) - set(df_columns.columns)
                # If there is a difference between columns in the table of the database and the columns of the dataframe
                # we are going to insert then we have to run alter table

                if len(alter_table_var) == 0:

                    final= conn.execute(text("SELECT * from {};".format(table_name_filter)))
                    final.fetchall()
                    time.sleep(3)
                    if len(df_archive) != 0:
                        insert_into(df_archive, table_name_filter,conn)
                else:
                    alter_table(table_name_filter, alter_table_var,conn)
                    if len(df_archive) != 0:
                        insert_into(df_archive, table_name_filter,conn)

            except Exception as e:
                # If table doesn't exists this block will execute and first create the table and the insert into
                create_table(df_archive, table_name_filter,conn)
                if len(df_archive) != 0:
                    insert_into(df_archive, table_name_filter,conn)

        now = datetime.now()
        jsondata = json.dumps(data_api).replace("'", "''")

        try:
            # If table exists this block will execute
            r = conn.execute(text("SELECT * from _infoIngest"))
            r.fetchall()
            insert_query = f"INSERT INTO _infoIngest (content, _dateInserted) VALUES ('{jsondata}', '{now}')"
            conn.execute(text(insert_query))
        except Exception as e:
            log.error(e)
            # If table doesn't exists this block will execute and first create the table and the insert into
            try:
                conn.execute(text("CREATE TABLE _infoIngest ([content] [varchar](max) NULL,[_dateInserted] [varchar](max) NULL)"))
                insert_query = f"INSERT INTO _infoIngest (content, _dateInserted) VALUES ('{jsondata}', '{now}')"
                conn.execute(text(insert_query))
            except Exception as e:
                log.error("Error on insertion")
                log.error(e)
                sys.exit()

        ######################## CREATING TABLE _generalContext THAT NEED TO BE INDEXED########################

        # Create dynamic and static context
        # Table to create: _generalContext
        with open("./utils/static_data_gc.json") as f:
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

            query2 = "SELECT {} from {}".format(header[0:-1], table)
            df_depu = pd.read_sql(query2, conn)
            for col in df_depu.columns:
                lista = df_depu[col].unique()
                text_list.append(
                    {
                        "text": f"When asked for something in {', '.join(f'{x}' for x in lista if x is not None)} filter column {col}, from {table}",
                        "tag": "dynamic",
                        "table": table,
                    }
                )

        text_list = pd.DataFrame(text_list)
        final_df = pd.DataFrame()
        final_df = final_df.from_records(static_data)
        final_df = pd.concat([final_df, text_list], ignore_index=True)

        try:
            conn.execute(text("DROP TABLE  IF EXISTS _generalContext"))
        except Exception as e:
            log.error("Error while Dropping Table")
            log.error(e)
            sys.exit()

        create_table(final_df, table_name_filter='_generalContext',conn=conn)
        insert_into_general_context(final_df,conn)

        conn.commit()




