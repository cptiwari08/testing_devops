import json
import os
import sys

import pandas as pd
from llama_index.core import Document
from sql_metadata import Parser

from index_core_functions import DBConnection, AISearchCollection, log, FieldHandler

def row_processor(row):
    answer = "TableName: " + row.TableName + " and Description: " + row.Description
    if row.question:
        if len(row.question) >= 5:
            answer += " and questions that can be answered: " + ', '.join(row.question[0:5])
        if len(row.question) < 5 and isinstance(row.question[0], str):
            answer += " and questions that can be answered: " + ', '.join(row.question[0:len(row.question)])
    return answer


try:
    ai_search_collection = AISearchCollection()

    # Index 1

    query = """SELECT DISTINCT T1.TableName,t1.Description, 
        --t2.COLUMN_NAME,t2.DATA_TYPE, 
        t1.isMainTable 
        FROM (SELECT TableName, 
        Description, 
        CASE 
        WHEN lower(Description) LIKE '%#coretable%' then 'True' 
        ELSE 'False' END AS isMainTable 
        FROM  _tablemetadata WHERE DESCRIPTION IS NOT NULL) t1 LEFT JOIN (SELECT COLUMN_NAME,DATA_TYPE,TABLE_NAME 
        FROM INFORMATION_SCHEMA.COLUMNS) t2 ON t1.TableName = t2.TABLE_NAME
        WHERE t2.COLUMN_NAME IS NOT NULL
        UNION ALL
        SELECT 'vwUnPivotEstimates' AS TableName, 'Table to capture the amount of (Estimate/Actual/Forecast) data into single column by unpivoting data from multiple columns like Y1M1,Y1M2,Y1M3 etc based on the years configuration and cadence selected. Helps to calculare run rate of initiatives.' AS Description, 'True' AS isMainTable
        UNION ALL
        SELECT 'vwValueCaptureTransactionMonths' AS TableName, 'This View is created to extract Month, Quarter and Year values from the tracking period table data.' AS Description, 'True' AS isMainTable
        UNION ALL
        SELECT 'vwUnpivotActuals' AS TableName, 'Table to capture the amount of (Estimate/Actual/Forecast) data into single column by unpivoting data from multiple columns like Y1M1,Y1M2,Y1M3 etc based on the years configuration and cadence selected. Helps to calculare run rate of initiatives.' AS Description, 'True' AS isMainTable
        UNION ALL
        SELECT 'vwUnpivotForecasts' AS TableName, 'Table to capture the amount of (Estimate/Actual/Forecast) data into single column by unpivoting data from multiple columns like Y1M1,Y1M2,Y1M3 etc based on the years configuration and cadence selected. Helps to calculare run rate of initiatives.' AS Description, 'True' AS isMainTable
        ;
    """

    data_json = DBConnection.execute_query({"SqlQuery": query, "tables": []})
    data_false = pd.DataFrame.from_records(data_json)

    data_false = data_false[data_false['isMainTable'] == 'False']
    data_false['question'] = None

    data_suggestions = DBConnection.suggestions()

    suggestions = []
    for i in data_suggestions:
        if i['answerSQL'] is not None and i['answerSQL'].find("WITH") == -1 and i['answerSQL'].find("DELETE") == -1 and i['suggestionText'] != '':
            suggestions.append({'question': i['suggestionText'],
                                'source': i['source'],
                                'appAffinity': i['appAffinity'],
                                'sqlQuery': i['answerSQL'],
                                'mainTable': Parser(i['answerSQL']).tables[0]})

    query = """SELECT TableName, 
            Description, 
            CASE 
            WHEN lower(Description) LIKE '%#coretable%' then 'True' 
            ELSE 'False' END AS isMainTable 
            FROM  _tablemetadata WHERE DESCRIPTION IS NOT NULL
            and CASE WHEN lower(Description) LIKE '%#coretable%' then 'True' 
            ELSE 'False' END = 'True' 
            UNION ALL
            SELECT 'vwUnPivotEstimates' AS TableName, 'Table to capture the amount of (Estimate/Actual/Forecast) data into single column by unpivoting data from multiple columns like Y1M1,Y1M2,Y1M3 etc based on the years configuration and cadence selected. Helps to calculare run rate of initiatives.' AS Description, 'True' AS isMainTable
            UNION ALL
            SELECT 'vwValueCaptureTransactionMonths' AS TableName, 'This View is created to extract Month, Quarter and Year values from the tracking period table data.' AS Description, 'True' AS isMainTable
            UNION ALL
            SELECT 'vwUnpivotActuals' AS TableName, 'Table to capture the amount of (Estimate/Actual/Forecast) data into single column by unpivoting data from multiple columns like Y1M1,Y1M2,Y1M3 etc based on the years configuration and cadence selected. Helps to calculare run rate of initiatives.' AS Description, 'True' AS isMainTable
            UNION ALL
            SELECT 'vwUnpivotForecasts' AS TableName, 'Table to capture the amount of (Estimate/Actual/Forecast) data into single column by unpivoting data from multiple columns like Y1M1,Y1M2,Y1M3 etc based on the years configuration and cadence selected. Helps to calculare run rate of initiatives.' AS Description, 'True' AS isMainTable
            ;
    """

    data_json = DBConnection.execute_query({"SqlQuery": query, "tables": []})
    data_desc = pd.DataFrame.from_records(data_json)
    final_data = pd.DataFrame(suggestions)
    final_data['mainTable'] = final_data['mainTable'].str.lower().replace("[^A-Za-z]", "", regex=True)
    data_desc['TableName_join'] = data_desc['TableName'].str.lower()
    data_true = data_desc.merge(final_data, left_on='TableName_join', right_on='mainTable', how='left', validate='one_to_many')
    agg_df = data_true.groupby(['TableName', 'Description', 'isMainTable'])['question'].agg(lambda x: list(x)).reset_index()

    final_second_index = pd.concat([data_false, agg_df])

    doc1 = [Document(text=row_processor(row),
                     metadata_template='{key}: {value}', metadata_seperator='\n',
                     metadata={"table_name": row.TableName, "isMainTable": row.isMainTable}
                     ) for _, row in final_second_index.iterrows()]

    project_specific_index1 = os.getenv("PRJDATA_INDEX")

    # Define the fields of the index
    field_handler_1 = FieldHandler()
    field_handler_1.add_str_field({"name": "table_name", "retrievable": True, "filterable": True, "searchable": True})
    field_handler_1.add_str_field({"name": "isMainTable", "retrievable": True, "filterable": True})

    metadata_fields_project_specific_index1 = {"table_name": "table_name", "isMainTable": "isMainTable"}

    ai_search_collection.process_all_instances(
        metadata=metadata_fields_project_specific_index1,
        index_name=project_specific_index1,
        doc=doc1,
        fields=field_handler_1.fields
    )

    # Index 2

    glossary = []

    data_glossary = DBConnection.get_glossary()

    for entry in data_glossary:
        record = {
            'context': entry['context'],
            'tableName': entry['tableName'],
            'tag': entry['tag'],
            'isSchema': str(entry['isSchema']).capitalize()
        }
        glossary.append(record)

    lookup_query = """SELECT STRING_AGG(CONCAT('|',CAST(t.TableName AS NVARCHAR(MAX)),'|'), ',') as main_table, cm.LookupField, tr.name 'ref_table', 
    CONCAT(tr.name,'_',cm.LookupField) as lkup
    FROM _TableMetadata t
    LEFT JOIN _ColumnMetadata cm ON t.TableName = cm.TableName
    INNER JOIN sys.tables tp ON tp.name = t.TableName
    INNER JOIN sys.foreign_keys fk ON fk.parent_object_id = tp.object_id
    INNER JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
    INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
    INNER JOIN sys.columns cp ON fkc.parent_column_id = cp.column_id 
        AND fkc.parent_object_id = cp.object_id 
        AND cp.name = cm.ColumnName 
    INNER JOIN sys.columns cr ON fkc.referenced_column_id = cr.column_id 
        AND fkc.referenced_object_id = cr.object_id
    WHERE t.DESCRIPTION IS NOT NULL 
        AND LOWER(t.Description) LIKE '%#coretable%' 
        AND cm.LookupField is not null
		AND t.TableName != 'NodesToProjectTeamsForProjectTeam'
    group by cm.LookupField, tr.name;
    """

    lookup_info = DBConnection.execute_query({"SqlQuery": lookup_query, "tables": []})

    lookup_values_query = "SELECT " + ",".join(
        [
            f"(SELECT CASE WHEN COUNT(1) < 50 THEN STRING_AGG(CAST({x['LookupField']} AS VARCHAR(MAX)),', ') ELSE NULL END from (SELECT distinct {x['LookupField']} from {x['ref_table']}) t) AS {x['ref_table']}_{x['LookupField']}"
            for x in lookup_info
        ]
    )

    lookup_values = DBConnection.execute_query({"SqlQuery": lookup_values_query, "tables": []})[0]

    lkup_dictionary = {item['lkup']: item['main_table'] for item in lookup_info}

    for x in list(lookup_values.keys()):
        if lookup_values[x] is not None and len(lkup_dictionary[x]) < 1024:
            #if x.split("_")[0]=="AccountStatuses" and x.split('_')[1] == "Title":
            #    text = {
            #        "context": f"When asked about for something related to {lookup_values[x]} use the Key column in table "
            #        + x.split("_")[0],
            #        "tableName": lkup_dictionary[x],
            #        "tag": "dynamic",
            #        "isSchema": "False"
            #    }
            #    glossary.append(text)
            #else:
                text = {
                    "context": f"When asked about for something related to {lookup_values[x]} use the {x.split('_')[1]} column in table "
                    + x.split("_")[0],
                    "tableName": lkup_dictionary[x],
                    "tag": "dynamic",
                    "isSchema": "False"
                }
                glossary.append(text)

    query_info = """select STRING_AGG(CONCAT('|',CAST(TableName as nvarchar(MAX)),'|'), ',') as tableName, context
    from (
        select 
            TableName, 
            concat(
                'Information about ',
                CASE WHEN COUNT(ColumnName) > 1 THEN 'the columns ' ELSE 'the column ' END,
                STRING_AGG(CAST(ColumnName as nvarchar(MAX)), ','), '. ',
                CASE WHEN Description is not null THEN CONCAT('Description: ', Description, '. ') ELSE '' END,
                CASE WHEN Choices is not null THEN CONCAT('Possible values: ', Choices) ELSE '' END
                ) as context
        from _ColumnMetadata 
        where Choices is not null and (Description is not null or Description is null)
		and TableName != 'NodesToProjectTeamsForProjectTeam'
        group by TableName, Choices, Description
    ) t
    group by context"""

    data_dynamic = DBConnection.execute_query({"SqlQuery": query_info, "tables": []})

    for x in data_dynamic:
        if len(x["tableName"]) <= 1024:
            text = {"context": x["context"], "tableName": x["tableName"],
                     "tag": "dynamic",
                     "isSchema": "False"}
            glossary.append(text)
    
    lookup_query2 = """SELECT  T1.TableName,
            CONCAT('TableName: ',T1.TableName,' and schema: ',STRING_AGG(COLUMN_NAME,', ')) tableSchema
            FROM 
                (SELECT TableName
                FROM  _tablemetadata 
                WHERE Tablename != 'NodesToProjectTeamsForProjectTeam'
                ) t1 
            INNER JOIN
            (SELECT COLUMN_NAME,DATA_TYPE,TABLE_NAME 
            FROM INFORMATION_SCHEMA.COLUMNS) t2 
            ON t1.TableName = t2.TABLE_NAME
            GROUP BY T1.TableName
            UNION ALL 
            SELECT
            TABLE_NAME AS TableName,
            CONCAT('TableName: ',TABLE_NAME,' and schema: ',STRING_AGG(COLUMN_NAME,', ')) tableSchema
                FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME 
            IN ('vwUnPivotEstimates','vwValueCaptureTransactionMonths','vwUnpivotActuals','vwUnpivotForecasts')
            GROUP BY TABLE_NAME;
    """

    lookup_info2 = DBConnection.execute_query({"SqlQuery": lookup_query2, "tables": []})

    for x in lookup_info2:
        #if len(x['tableSchema'])<=1024:
        text = {"context": x['tableSchema'], 
                "tableName": "|"+x['TableName']+"|", 
                "tag": "dynamic", 
                "isSchema": "True"}
        glossary.append(text)

    glossary = [i for i in glossary if not (i['context'] == "When asked about for something related to Account Disabled, Project Disabled, Project Enabled use the Title column in table AccountStatuses")]

    doc2 = [
        Document(
            text=row["context"],
            metadata_template="{key}: {value}",
            metadata_seperator="\n",
            metadata={"tableName": row["tableName"], 
                      "tag": row["tag"],
                      "isSchema":row['isSchema']},
        )
        for row in glossary
    ]

    project_specific_index2 = os.getenv("PRJGLOSS_INDEX")

    # Define the fields of the index
    field_handler_2 = FieldHandler()
    field_handler_2.add_str_field({"name": "tableName", "retrievable": True, "filterable": True, "searchable": True})
    field_handler_2.add_str_field({"name": "tag", "retrievable": True, "filterable": True})
    field_handler_2.add_str_field({"name": "isSchema", "retrievable": True, "filterable": True})

    metadata_fields_project_specific_index2 = {"tableName": "tableName", "tag": "tag","isSchema":"isSchema"}

    ai_search_collection.process_all_instances(
        metadata=metadata_fields_project_specific_index2,
        index_name=project_specific_index2,
        doc=doc2,
        fields=field_handler_2.fields
    )


    # Third Index
    suggestions = []
    for i in data_suggestions:
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

    doc3 = [
        Document(
            text=row["question"],
            metadata_template="{key}: {value}",
            metadata_seperator="\n",
            metadata={
                "sqlQuery": row["sqlQuery"],
                "source": row["source"],
                "appAffinity": row["appAffinity"],
                "idSuggestion": row["id"],
                "visibleToAssistant": row["visibleToAssistant"]
            },
            excluded_embed_metadata_keys=['sqlQuery','source','appAffinity','idSuggestion','visibleToAssistant']

        )
        for row in suggestions
    ]

    project_specific_index3 = os.getenv("PRJDATA_SUGGESTION_INDEX")

    metadata_fields_project_specific_index3 = {
        "sqlQuery": "sqlQuery",
        "source": "source",
        "appAffinity": "appAffinity",
        "idSuggestion" : "idSuggestion",
        "visibleToAssistant": "visibleToAssistant"
    }

    field_handler_3 = FieldHandler()
    field_handler_3.add_str_field({"name": "sqlQuery", "retrievable": True, "filterable": True})
    field_handler_3.add_str_field({"name": "source", "retrievable": True, "filterable": True})
    field_handler_3.add_str_field({"name": "appAffinity", "retrievable": True, "filterable": True})
    field_handler_3.add_int_field({"name": "idSuggestion", "retrievable": True, "filterable": True})
    field_handler_3.add_bool_field({"name": "visibleToAssistant", "retrievable": True, "filterable": True})
    
    ai_search_collection.process_all_instances(
        metadata=metadata_fields_project_specific_index3,
        index_name=project_specific_index3,
        doc=doc3,
        fields=field_handler_3.fields
    )
except Exception as e:
    log.error(e)
    sys.exit()
