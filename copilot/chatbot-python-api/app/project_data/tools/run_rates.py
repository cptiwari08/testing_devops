import asyncio
import os
import sys

current_dir = os.path.dirname((__file__))
parent_dir = os.path.dirname(os.path.dirname(os.path.dirname(current_dir)))
sys.path.append(parent_dir)
from app.project_data.services.program_office_api import ProgramOffice

# montly run rate calculation
# Key Points:
# Dynamic SQL Construction:
# The script constructs a dynamic SQL query inside the @SqlQuery variable.
# The column name (@ColumnName) is based on the project timeframe (@Y), and it looks for monthly estimates (M12Estimate).
# TeamTypeID as a Parameter:
# The team_type_id is passed as a parameter to the SQL query, ensuring that the TeamTypeID condition is respected in the subquery.
# SQL Execution:
# The sp_executesql command is used to execute the dynamic SQL query within the database.
# The final run rate is calculated and returned as ValuetoAchieve in millions.


async def calculate_monthly_runrate(team_type_id: int = 1, token: str = None) -> str:

    program_office = ProgramOffice()

    sql_query = """
    DECLARE @Y INT;
    DECLARE @ColumnName NVARCHAR(50);
    DECLARE @SqlQuery NVARCHAR(MAX);

    -- Retrieve the selected project timeframe ID
    SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT)
    FROM MetastoreGeneralSettings
    WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';

    -- Construct the column name based on the timeframe
    SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';

    -- Construct the dynamic SQL query
    SET @SqlQuery = '
        SELECT (SUM(ISNULL(' + QUOTENAME(@ColumnName) + ', 0)) * 12) / 1000000 AS ValuetoAchieve
        FROM ValueCaptureEstimates VCE
        JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID
        WHERE ISNULL(VCE.Recurring, 0) = 1
        AND VCI.IsItemActive = 1
        AND VCI.ProjectTeamID IN (
            SELECT ID FROM ProjectTeams
            WHERE ManageValueCapture = 1 AND TeamTypeID = {team_type_id}
        )
        AND VCE.ValueCaptureValueTypeId = 1';

    -- Execute the dynamic SQL query
    EXEC sp_executesql @SqlQuery;
    """.format(team_type_id=team_type_id)
    tables = ["ValueCaptureEstimates", "ValueCaptureInitiatives", "ProjectTeams"]

    payload = {
        "SqlQuery": sql_query,
        "tables": tables,
    }

    output = await program_office.run_query(payload, token)
    # Create the json result only with first 100 rowa to avoid exceed token for LLM
    if not output:
        return "Query result is empty"
    return output


async def calculate_quarterly_runrate(team_type_id: int = 1, token: str = None) -> str:

    program_office = ProgramOffice()

    # SQL Script to calculate the quarterly run rate
    sql_query = """
    DECLARE @Y INT;
    DECLARE @ColumnName NVARCHAR(50);
    DECLARE @SqlQuery NVARCHAR(MAX);

    -- Retrieve the selected project timeframe ID
    SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT)
    FROM MetastoreGeneralSettings
    WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';

    -- Construct the column name based on the timeframe
    SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'Q4Estimate';

    -- Construct the dynamic SQL query
    SET @SqlQuery = '
        SELECT (SUM(ISNULL(' + QUOTENAME(@ColumnName) + ', 0)) * 4) / 1000000 AS ValuetoAchieve
        FROM ValueCaptureEstimates VCE
        JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID
        WHERE ISNULL(VCE.Recurring, 0) = 1
        AND VCI.IsItemActive = 1
        AND VCI.ProjectTeamID IN (
            SELECT ID FROM ProjectTeams
            WHERE ManageValueCapture = 1 AND TeamTypeID = {team_type_id}
        )
        AND VCE.ValueCaptureValueTypeId = 1';

    -- Execute the dynamic SQL query
    EXEC sp_executesql @SqlQuery;
    """.format(team_type_id=team_type_id)

    tables = ["ValueCaptureEstimates", "ValueCaptureInitiatives", "ProjectTeams"]

    payload = {
        "SqlQuery": sql_query,
        "tables": tables,
    }

    output = await program_office.run_query(payload, token)
    # Create the json result only with first 100 rowa to avoid exceed token for LLM
    if not output:
        return "Query result is empty"
    return output


async def calculate_anual_runrate(team_type_id: int = 1, token: str = None) -> str:

    program_office = ProgramOffice()

    # SQL Script to calculate the annual run rate
    sql_query = """
    DECLARE @Y INT;
    DECLARE @ColumnName NVARCHAR(50);
    DECLARE @SqlQuery NVARCHAR(MAX);

    -- Retrieve the selected project timeframe ID
    SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT)
    FROM MetastoreGeneralSettings
    WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';

    -- Construct the column name based on the timeframe
    SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'Estimate';

    -- Construct the dynamic SQL query
    SET @SqlQuery = '
        SELECT (SUM(ISNULL(' + QUOTENAME(@ColumnName) + ', 0)) / 1000000) AS ValuetoAchieve
        FROM ValueCaptureEstimates VCE
        JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID
        WHERE ISNULL(VCE.Recurring, 0) = 1
        AND VCI.IsItemActive = 1
        AND VCI.ProjectTeamID IN (
            SELECT ID FROM ProjectTeams
            WHERE ManageValueCapture = 1 AND TeamTypeID = {team_type_id}
        )
        AND VCE.ValueCaptureValueTypeId = 1';

    -- Execute the dynamic SQL query
    EXEC sp_executesql @SqlQuery;
    """.format(
        team_type_id=team_type_id
    )

    tables = ["ValueCaptureEstimates", "ValueCaptureInitiatives", "ProjectTeams"]

    payload = {
        "SqlQuery": sql_query,
        "tables": tables,
    }

    output = await program_office.run_query(payload, token)
    # Create the json result only with first 100 rowa to avoid exceed token for LLM
    if not output:
        return "Query result is empty"
    return output

async def main():
    # Example usage of the calculate_monthly_runrate function
    monthly_runrate = await calculate_monthly_runrate(team_type_id=1)


# This function should evolve to be a tool
#monthly_runrate_tool = FunctionTool.from_defaults(calculate_monthly_runrate)


if __name__ == "__main__":
    response = asyncio.run(main())
