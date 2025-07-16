import string

from app.core.pydantic_models import QueryPipelineContext
from app.project_data.services.program_office_api import ProgramOffice
from pypika import Query, Table

program_office = ProgramOffice()
translator = str.maketrans("", "", string.ascii_lowercase)


def clean_table(payload: dict):
    """limpia el resultado de la API"""
    for content in payload:
        content.pop("_CanRead", None)
        content.pop("_CanUpdate", None)
        content.pop("_CanDelete", None)
        content.pop("ID", None)


async def get_table_relationships(table: str, token: str):
    fk = Table("foreign_keys", alias="fk", schema="sys")
    tp = Table("tables", alias="tp", schema="sys")
    tr = Table("tables", alias="tr", schema="sys")
    fkc = Table("foreign_key_columns", alias="fkc", schema="sys")
    cp = Table("columns", alias="cp", schema="sys")
    cr = Table("columns", alias="cr", schema="sys")

    query = (
        Query.from_(fk)
        .select(
            tp.name.as_("table_name"),
            fk.name.as_("fk_name"),
            cp.name.as_("main_column"),
            tr.name.as_("ref_table"),
            cr.name.as_("ref_column"),
        )
        .join(tp)
        .on(fk.parent_object_id == tp.object_id)
        .join(tr)
        .on(fk.referenced_object_id == tr.object_id)
        .join(fkc)
        .on(fkc.constraint_object_id == fk.object_id)
        .join(cp)
        .on(
            (fkc.parent_column_id == cp.column_id)
            & (fkc.parent_object_id == cp.object_id)
        )
        .join(cr)
        .on(
            (fkc.referenced_column_id == cr.column_id)
            & (fkc.referenced_object_id == cr.object_id)
        )
        .where(tp.name.isin([table]))
        .orderby(tp.name, cp.column_id)
    )

    final_query = str(query).replace("JOIN", "INNER JOIN")

    query_payload = {
        "SqlQuery": final_query,
        "tables": [],
    }

    relationships_json = {}
    rows = await program_office.run_query(payload=query_payload, token=token)
    for row in rows:
        relationships_json[row.pop("main_column")] = row

    return relationships_json


async def get_metadata_information(main_table: str, token: str):
    _column_metadata = Table("_ColumnMetadata")

    query = (
        Query.from_(_column_metadata)
        .select(
            _column_metadata.ColumnName,
            _column_metadata.LookupField,
            _column_metadata.MultiLookup,
            _column_metadata.MultiLookupTitle,
            _column_metadata.MultiLookupLookupField,
            _column_metadata.MultiLookupDescription,
        )
        .where(_column_metadata.TableName == main_table)
    )

    column_metadata_payload = {"SqlQuery": str(query), "tables": []}
    columns = await program_office.run_query(
        payload=column_metadata_payload, token=token
    )
    clean_table(columns)

    relationships = await get_table_relationships(main_table, token)

    return columns, relationships


def get_main_column_from_columns(columns, relationships):
    main_columns = []
    lookup_columns = []
    for column in columns:
        if column["LookupField"] and relationships.get(column["ColumnName"]):
            json_field = {
                "main_column": column["ColumnName"],
                "lookup_field": column["LookupField"],
            }
            json_field.update(relationships[column["ColumnName"]])
            lookup_columns.append(json_field)
        else:
            main_columns.append(column["ColumnName"])

    return main_columns, lookup_columns


async def sql_query_builder_for_bridge_table(
    main_table_translated, columns, relationships, token
):
    query_result = ""
    from_query_result = ""
    multilookup_columns = []
    main_tables_result = []
    EXCLUDED_TABLE = 'NodesToProjectTeamsForProjectTeam'

    for column in columns:
        if ((column["LookupField"] is None and relationships.get(column["ColumnName"], None) is not None) or column["MultiLookup"]):
            json_field = {"main_column": column["ColumnName"]}
            json_field.update(relationships[column["ColumnName"]])
            multilookup_columns.append(json_field)

    for count, multicolumn in enumerate(multilookup_columns):
        if count > 0:
            query_result += ","

        multicolumn_tablename = multicolumn["ref_table"]
        main_tables_result.append(multicolumn_tablename)

        multicolumn_tablename_translated = multicolumn_tablename.translate(translator)
        from_query_result += f"LEFT JOIN {multicolumn_tablename} {multicolumn_tablename_translated} ON {multicolumn_tablename_translated}.{multicolumn['ref_column']} = {main_table_translated}.{multicolumn['main_column']} "

        lookup_columns_json, lookup_relationships = await get_metadata_information(
            multicolumn_tablename, token
        )

        main_columns, lookup_columns = get_main_column_from_columns(lookup_columns_json, lookup_relationships)

        # Main columns
        query_result += ",".join(
            [
                f'{multicolumn_tablename_translated}.{col} "{multicolumn_tablename_translated}_{col}"'
                for col in main_columns
            ]
        )
        # Lookup columns
        for idx, column in enumerate(lookup_columns):
            if multicolumn["ref_table"] == EXCLUDED_TABLE:
                continue
            query_result += f",{multicolumn_tablename_translated}_{column['ref_table'].translate(translator)}{idx}.{column['lookup_field']} \"{column['main_column'][:-2] if column['main_column'].endswith('Id')  else column['main_column']}\""
            from_query_result += f"LEFT JOIN {column['ref_table']} {multicolumn_tablename_translated}_{column['ref_table'].translate(translator)}{idx} ON {multicolumn_tablename_translated}.{column['main_column']} = {multicolumn_tablename_translated}_{column['ref_table'].translate(translator)}{idx}.{column['ref_column']} "
    return query_result, from_query_result


def sql_query_builder_for_non_bridge_table(main_table_translated, columns, relationships):
    query_result = ""
    from_query_result = ""
    EXCLUDED_TABLE = 'NodesToProjectTeamsForProjectTeam'

    main_columns, lookup_columns = get_main_column_from_columns(columns, relationships)

    # Main columns
    query_result += f"{','.join([f'{main_table_translated}.{col}' for col in main_columns])}"
    # Lookup columns
    for idx, column in enumerate(lookup_columns):
        if column['ref_table'] == EXCLUDED_TABLE:
            continue
        query_result += f",{column['ref_table'].translate(translator)}{idx}.{column['lookup_field']} \"{column['main_column'][:-2] if column['main_column'].endswith('Id') else column['main_column']}\""
        from_query_result += f"LEFT JOIN {column['ref_table']} {column['ref_table'].translate(translator)}{idx} ON {main_table_translated}.{column['main_column']} = {column['ref_table'].translate(translator)}{idx}.{column['ref_column']} "
    return query_result, from_query_result


def json_query_builder_for_non_bridge_table(main_table, columns, relationships):
    main_columns = []
    lookup_columns = []
    for column in columns:
        if column["LookupField"]:
            json_field = {
                "main_column": column["ColumnName"],
                "lookup_field": column["LookupField"],
            }
            json_field.update(relationships[column["ColumnName"]])
            lookup_columns.append(json_field)
        else:
            main_columns.append(column["ColumnName"])

    json_result = {
        "tableName": main_table,
        "columnNames": main_columns,
        "joins": [],
    }

    for column in lookup_columns:
        json_result["joins"].append(
            {
                "relationshipName": column["fk_name"].split("$")[-1],
                "table": {"tableName": column["ref_table"]},
                "columnNames": [column["lookup_field"]],
            }
        )

    return json_result


async def multilookup_query_builder(main_table: str, json_format: bool, token: str):
    main_table_translated = main_table.translate(translator)
    query_result = "SELECT "
    from_query_result = f" FROM {main_table} {main_table_translated} "

    columns, relationships = await get_metadata_information(main_table, token)

    # Bridge-tables
    if any([col["MultiLookup"] for col in columns]):  # is_multilookup
        # JSON Query for bridge-tables
        if json_format:
            raise NotImplementedError()
        else:
            query_result_suffix, from_query_result_suffix = (
                await sql_query_builder_for_bridge_table(
                    main_table_translated, columns, relationships, token
                )
            )
            return (
                query_result
                + query_result_suffix
                + from_query_result
                + from_query_result_suffix
            )

    else:  # Non-bridge-tables
        if json_format:
            return json_query_builder_for_non_bridge_table(main_table, columns, relationships)
        else:
            query_result_suffix, from_query_result_suffix = sql_query_builder_for_non_bridge_table(main_table_translated, columns, relationships)
            return query_result + query_result_suffix + from_query_result + from_query_result_suffix


async def denormalized_query_generator(
    tables_list: list, context: QueryPipelineContext
) -> str:
    context.logger.info("I executed the denormalized query generator function component")
    # Even if there are multiple tables,
    # we are only interested in the first table
    table = tables_list[0]

    sqlquery_result = await multilookup_query_builder(table, False, context.token)
    return sqlquery_result
