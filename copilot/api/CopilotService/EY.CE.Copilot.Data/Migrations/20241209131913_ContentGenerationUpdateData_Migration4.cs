using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContentGenerationUpdateData_Migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 1,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        WP.Title, \r\n        WP.TaskDueDate, \r\n        WP.ReportingLevelId, \r\n        WP.Priority, \r\n        WP.WorkPlanTaskType, \r\n        CASE \r\n            WHEN WP.Priority = '(1) High' THEN 1\r\n            WHEN WP.Priority = '(2) Normal' THEN 2\r\n            WHEN WP.Priority = '(3) Low' THEN 3\r\n            WHEN WP.Priority IS NULL OR WP.Priority= '' THEN 4\r\n        END AS PriorityRank\r\n    FROM Workplan WP\r\n    JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id\r\n    WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task')\r\n	AND S.[Key] NOT IN ('CANCELLED')\r\n    AND WP.HierarchyLevel <= 3\r\n    AND WP.TaskDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND WP.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.TaskDueDate, \r\n    PT.ReportingLevelId, \r\n    PT.Priority, \r\n    PT.WorkPlanTaskType\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 2,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        RAI.IssueRiskCategory, \r\n        RAI.ItemStatusId , \r\n        RAI.ItemDescription, \r\n        RAI.ItemDueDate, \r\n        RAI.ItemPriority, \r\n        RAI.Title, \r\n        RAI.ReportingLevelId, \r\n        CASE \r\n            WHEN RAI.ItemPriority = 'Critical' THEN 1\r\n            WHEN RAI.ItemPriority = 'High' THEN 2\r\n            WHEN RAI.ItemPriority = 'Medium' THEN 3\r\n            WHEN RAI.ItemPriority = 'Low' THEN 4\r\n            WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM RisksAndIssues RAI\r\n    JOIN Statuses S ON RAI.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND RAI.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND RAI.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.IssueRiskCategory, \r\n    PT.ItemStatusId , \r\n    PT.ItemDescription, \r\n    PT.ItemDueDate, \r\n    PT.ItemPriority, \r\n    PT.Title, \r\n    PT.ReportingLevelId\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 3,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        ACT.Title, \r\n        ACT.ItemDescription, \r\n        ACT.ProjectTeamId, \r\n        ACT.ItemPriority, \r\n        ACT.ItemDueDate, \r\n        ACT.ReportingLevelId, \r\n        CASE \r\n            WHEN ACT.ItemPriority = 'Critical' THEN 1\r\n            WHEN ACT.ItemPriority = 'High' THEN 2\r\n            WHEN ACT.ItemPriority = 'Medium' THEN 3\r\n            WHEN ACT.ItemPriority = 'Low' THEN 4\r\n            WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM Actions ACT\r\n    JOIN Statuses S ON ACT.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND ACT.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND ACT.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 4,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        DCS.Title, \r\n        DCS.ItemDescription, \r\n        DCS.ProjectTeamId, \r\n        DCS.ItemPriority, \r\n        DCS.ItemDueDate, \r\n        DCS.ReportingLevelId, \r\n        CASE \r\n            WHEN DCS.ItemPriority = 'Critical' THEN 1\r\n            WHEN DCS.ItemPriority = 'High' THEN 2\r\n            WHEN DCS.ItemPriority = 'Medium' THEN 3\r\n            WHEN DCS.ItemPriority = 'Low' THEN 4\r\n            WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM Decisions DCS\r\n    JOIN Statuses S ON DCS.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND DCS.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND DCS.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 5,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS (\r\n    SELECT \r\n        ID.Title, \r\n        ID.ProviderProjectTeamId, \r\n        ID.ReceiverProjectTeamId, \r\n        ID.ItemDueDate, \r\n        ID.ReportingLevelId\r\n    FROM Interdependencies ID\r\n    JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id \r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND ID.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})\r\n)\r\nSELECT \r\n    Title, \r\n    ProviderProjectTeamId, \r\n    ReceiverProjectTeamId, \r\n    ItemDueDate\r\nFROM FilteredInterdependencies;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 6,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        WP.Title, \r\n        WP.TaskDueDate, \r\n        WP.ReportingLevelId, \r\n        WP.Priority, \r\n        WP.WorkPlanTaskType, \r\n        CASE \r\n            WHEN WP.Priority = '(1) High' THEN 1\r\n            WHEN WP.Priority = '(2) Normal' THEN 2\r\n            WHEN WP.Priority = '(3) Low' THEN 3\r\n            WHEN WP.Priority IS NULL OR  WP.Priority = '' THEN 4\r\n        END AS PriorityRank\r\n    FROM Workplan WP\r\n    JOIN Statuses S ON WP.WorkPlanTaskStatusId= S.Id\r\n    WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task')\r\n    AND S.[Key]  IN ('CLOSED', 'COMPLETED')\r\n    AND WP.TaskDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND WP.HierarchyLevel <= 3\r\n    AND WP.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.TaskDueDate, \r\n    PT.ReportingLevelId, \r\n    PT.Priority, \r\n    PT.WorkPlanTaskType\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 7,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        RAI.IssueRiskCategory, \r\n        RAI.TimeBasedCalculatedStatusId, \r\n        RAI.ItemDescription, \r\n        RAI.ItemDueDate, \r\n        RAI.ItemPriority, \r\n        RAI.Title, \r\n        RAI.ReportingLevelId, \r\n        CASE \r\n            WHEN RAI.ItemPriority = 'Critical' THEN 1\r\n            WHEN RAI.ItemPriority = 'High' THEN 2\r\n            WHEN RAI.ItemPriority = 'Medium' THEN 3\r\n            WHEN RAI.ItemPriority = 'Low' THEN 4\r\n            WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM RisksAndIssues RAI\r\n    JOIN Statuses S ON RAI.ItemStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND RAI.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n   AND RAI.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.IssueRiskCategory, \r\n    PT.ItemDescription, \r\n    PT.ItemDueDate, \r\n    PT.ItemPriority, \r\n    PT.Title, \r\n    PT.ReportingLevelId\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 8,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        ACT.Title, \r\n        ACT.ItemDescription, \r\n        ACT.ProjectTeamId, \r\n        ACT.ItemPriority, \r\n        ACT.ItemDueDate, \r\n        CASE \r\n            WHEN ACT.ItemPriority = 'Critical' THEN 1\r\n            WHEN ACT.ItemPriority = 'High' THEN 2\r\n            WHEN ACT.ItemPriority = 'Medium' THEN 3\r\n            WHEN ACT.ItemPriority = 'Low' THEN 4\r\n            WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM Actions ACT\r\n    JOIN Statuses S ON ACT.ItemStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND ACT.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND ACT.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 9,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        DCS.Title, \r\n        DCS.ItemDescription, \r\n        DCS.ProjectTeamId, \r\n        DCS.ItemPriority, \r\n        DCS.ItemDueDate, \r\n        CASE \r\n            WHEN DCS.ItemPriority = 'Critical' THEN 1\r\n            WHEN DCS.ItemPriority = 'High' THEN 2\r\n            WHEN DCS.ItemPriority = 'Medium' THEN 3\r\n            WHEN DCS.ItemPriority = 'Low' THEN 4\r\n            WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM Decisions DCS\r\n    JOIN Statuses S ON DCS.ItemStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND DCS.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND DCS.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 10,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS (\r\n    SELECT \r\n        ID.Title, \r\n        ID.ProviderProjectTeamId, \r\n        ID.ReceiverProjectTeamId, \r\n        ID.ItemDueDate, \r\n        ID.ReportingLevelId\r\n    FROM Interdependencies ID\r\n    JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND ID.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})\r\n)\r\nSELECT \r\n    Title, \r\n    ProviderProjectTeamId, \r\n    ReceiverProjectTeamId, \r\n    ItemDueDate\r\nFROM FilteredInterdependencies;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 11,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        WP.Title, \r\n        WP.TaskDueDate, \r\n        WP.ReportingLevelId, \r\n        WP.Priority, \r\n        WP.WorkPlanTaskType,\r\n		S.Title as WorkplanTakStatus,\r\n        CASE \r\n            WHEN WP.Priority = '(1) High' THEN 1\r\n            WHEN WP.Priority = '(2) Normal' THEN 2\r\n            WHEN WP.Priority = '(3) Low' THEN 3\r\n            WHEN WP.Priority IS NULL OR WP.Priority = '' THEN 4\r\n        END AS PriorityRank\r\n    FROM Workplan WP\r\n    JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id\r\n    WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task')\r\n    AND S.[Key] NOT IN ('CLOSED', 'CANCELLED', 'COMPLETED')\r\n	AND WP.HierarchyLevel <= 3\r\n    AND WP.TaskDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND WP.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.TaskDueDate, \r\n    PT.ReportingLevelId, \r\n    PT.Priority, \r\n    PT.WorkPlanTaskType,\r\n	PT.WorkplanTakStatus\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 12,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        RAI.IssueRiskCategory, \r\n        S.Title AS ItemStatus,\r\n        RAI.ItemDescription, \r\n        RAI.ItemDueDate, \r\n        RAI.ItemPriority, \r\n        RAI.Title, \r\n        RAI.ReportingLevelId,\r\n        CASE \r\n            WHEN RAI.ItemPriority = 'Critical' THEN 1\r\n            WHEN RAI.ItemPriority = 'High' THEN 2\r\n            WHEN RAI.ItemPriority = 'Medium' THEN 3\r\n            WHEN RAI.ItemPriority = 'Low' THEN 4\r\n            WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM RisksAndIssues RAI\r\n    JOIN Statuses S ON RAI.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n    AND RAI.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n    AND RAI.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.IssueRiskCategory, \r\n    PT.ItemDescription, \r\n    PT.ItemDueDate, \r\n    PT.ItemPriority, \r\n    PT.Title, \r\n    PT.ReportingLevelId,\r\n	PT.ItemStatus\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 13,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        ACT.Title, \r\n        ACT.ItemDescription, \r\n        ACT.ProjectTeamId, \r\n        ACT.ItemPriority, \r\n        ACT.ItemDueDate, \r\n        S.Title AS ItemStatus,\r\n        CASE \r\n            WHEN ACT.ItemPriority = 'Critical' THEN 1\r\n            WHEN ACT.ItemPriority = 'High' THEN 2\r\n            WHEN ACT.ItemPriority = 'Medium' THEN 3\r\n            WHEN ACT.ItemPriority = 'Low' THEN 4\r\n            WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM Actions ACT\r\n    JOIN Statuses S ON ACT.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n    AND ACT.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n    AND ACT.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemStatus\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 14,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        DCS.Title, \r\n        DCS.ItemDescription, \r\n        DCS.ProjectTeamId, \r\n        DCS.ItemPriority, \r\n        DCS.ItemDueDate, \r\n        S.Title AS ItemStatus,\r\n        CASE \r\n            WHEN DCS.ItemPriority = 'Critical' THEN 1\r\n            WHEN DCS.ItemPriority = 'High' THEN 2\r\n            WHEN DCS.ItemPriority = 'Medium' THEN 3\r\n            WHEN DCS.ItemPriority = 'Low' THEN 4\r\n            WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5\r\n        END AS PriorityRank\r\n    FROM Decisions DCS\r\n    JOIN Statuses S ON DCS.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n    AND DCS.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n    AND DCS.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemStatus\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 15,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS (\r\n    SELECT \r\n        ID.Title, \r\n        ID.ProviderProjectTeamId, \r\n        ID.ReceiverProjectTeamId, \r\n        ID.ItemDueDate, \r\n        RL.[Key] AS ReportingLevel,\r\n        S.Title AS ItemStatus\r\n    FROM Interdependencies ID\r\n    JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n	AND ID.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n    AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})\r\n)\r\nSELECT \r\n    Title, \r\n    ProviderProjectTeamId, \r\n    ReceiverProjectTeamId, \r\n    ItemDueDate, \r\n    ReportingLevel,\r\n    ItemStatus\r\nFROM FilteredInterdependencies;\r\n\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 1,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT WP.Title, WP.TaskDueDate, WP.ReportingLevelId, WP.Priority, WP.WorkPlanTaskType, CASE WHEN WP.Priority = '(1) High' THEN 1 WHEN WP.Priority = '(2) Medium' THEN 2 WHEN WP.Priority = '(3) Low' THEN 3 END AS PriorityRank FROM Workplan WP WHERE WP.WorkPlanTaskType IN ('Milestone') AND WP.TimeBasedCalculatedStatusId NOT IN (6, 20, 23, 26, 28, 29, 31) AND WP.Priority IN ('(1) High', '(2) Medium', '(3) Low') AND WP.ReportingLevelId IN (4, 5) AND WP.TaskDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND WP.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.TaskDueDate, PT.ReportingLevelId, PT.Priority, PT.WorkPlanTaskType FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 2,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT RAI.IssueRiskCategory, RAI.TimeBasedCalculatedStatusId, RAI.ItemDescription, RAI.ItemDueDate, RAI.ItemPriority, RAI.Title, RAI.ReportingLevelId, CASE WHEN RAI.ItemPriority = 'Critical' THEN 1 WHEN RAI.ItemPriority = 'High' THEN 2 WHEN RAI.ItemPriority = 'Medium' THEN 3 WHEN RAI.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM RisksAndIssues RAI WHERE RAI.TimeBasedCalculatedStatusId NOT IN (6, 20, 23, 26, 28, 29, 31) AND RAI.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND RAI.ReportingLevelId IN (4, 5) AND RAI.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND RAI.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.IssueRiskCategory, PT.TimeBasedCalculatedStatusId, PT.ItemDescription, PT.ItemDueDate, PT.ItemPriority, PT.Title, PT.ReportingLevelId FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 3,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT ACT.Title, ACT.ItemDescription, ACT.ProjectTeamId, ACT.ItemPriority, ACT.ItemDueDate, CASE WHEN ACT.ItemPriority = 'Critical' THEN 1 WHEN ACT.ItemPriority = 'High' THEN 2 WHEN ACT.ItemPriority = 'Medium' THEN 3 WHEN ACT.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Actions ACT WHERE ACT.ItemStatusId NOT IN (6, 20, 23, 26, 28, 29, 31) AND ACT.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND ACT.ReportingLevelId IN (4, 5) AND ACT.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND ACT.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ItemDescription, PT.ProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 4,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT DCS.Title, DCS.ItemDescription, DCS.ProjectTeamId, DCS.ItemPriority, DCS.ItemDueDate, CASE WHEN DCS.ItemPriority = 'Critical' THEN 1 WHEN DCS.ItemPriority = 'High' THEN 2 WHEN DCS.ItemPriority = 'Medium' THEN 3 WHEN DCS.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Decisions DCS WHERE DCS.ItemStatusId NOT IN (6, 20, 23, 26, 28, 29, 31) AND DCS.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND DCS.ReportingLevelId IN (4, 5) AND DCS.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND DCS.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ItemDescription, PT.ProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 5,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT ID.Title, ID.ProviderProjectTeamId, ID.ReceiverProjectTeamId, ID.ItemPriority, ID.ItemDueDate, CASE WHEN ID.ItemPriority = 'Critical' THEN 1 WHEN ID.ItemPriority = 'High' THEN 2 WHEN ID.ItemPriority = 'Medium' THEN 3 WHEN ID.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Interdependencies ID WHERE ID.InterdependencyStatusId NOT IN (6, 20, 23, 26, 28, 29, 31) AND ID.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND ID.ReportingLevelId IN (4, 5) AND ID.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ProviderProjectTeamId, PT.ReceiverProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 6,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT WP.Title, WP.TaskDueDate, WP.ReportingLevelId, WP.Priority, WP.WorkPlanTaskType, CASE WHEN WP.Priority = '(1) High' THEN 1 WHEN WP.Priority = '(2) Medium' THEN 2 WHEN WP.Priority = '(3) Low' THEN 3 END AS PriorityRank FROM Workplan WP WHERE WP.WorkPlanTaskType IN ('Milestone','Task') AND WP.TimeBasedCalculatedStatusId = 26 AND WP.Priority IN ('(1) High', '(2) Medium', '(3) Low') AND WP.ReportingLevelId IN (4, 5) AND WP.TaskDueDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND WP.TaskDueDate < CAST(GETDATE() AS DATE) AND WP.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.TaskDueDate, PT.ReportingLevelId, PT.Priority, PT.WorkPlanTaskType FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 7,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT RAI.IssueRiskCategory, RAI.TimeBasedCalculatedStatusId, RAI.ItemDescription, RAI.ItemDueDate, RAI.ItemPriority, RAI.Title, RAI.ReportingLevelId, CASE WHEN RAI.ItemPriority = 'Critical' THEN 1 WHEN RAI.ItemPriority = 'High' THEN 2 WHEN RAI.ItemPriority = 'Medium' THEN 3 WHEN RAI.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM RisksAndIssues RAI WHERE RAI.TimeBasedCalculatedStatusId =26 AND RAI.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND RAI.ReportingLevelId IN (4, 5) AND RAI.ItemDueDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND RAI.ItemDueDate < CAST(GETDATE() AS DATE) AND RAI.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.IssueRiskCategory, PT.TimeBasedCalculatedStatusId, PT.ItemDescription, PT.ItemDueDate, PT.ItemPriority, PT.Title, PT.ReportingLevelId FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 8,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT ACT.Title, ACT.ItemDescription, ACT.ProjectTeamId, ACT.ItemPriority, ACT.ItemDueDate, CASE WHEN ACT.ItemPriority = 'Critical' THEN 1 WHEN ACT.ItemPriority = 'High' THEN 2 WHEN ACT.ItemPriority = 'Medium' THEN 3 WHEN ACT.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Actions ACT WHERE ACT.ItemStatusId = 26 AND ACT.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND ACT.ReportingLevelId IN (4, 5) AND ACT.ItemDueDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND ACT.ItemDueDate < CAST(GETDATE() AS DATE) AND ACT.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ItemDescription, PT.ProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 9,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT DCS.Title, DCS.ItemDescription, DCS.ProjectTeamId, DCS.ItemPriority, DCS.ItemDueDate, CASE WHEN DCS.ItemPriority = 'Critical' THEN 1 WHEN DCS.ItemPriority = 'High' THEN 2 WHEN DCS.ItemPriority = 'Medium' THEN 3 WHEN DCS.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Decisions DCS WHERE DCS.ItemStatusId =26 AND DCS.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND DCS.ReportingLevelId IN (4, 5) AND DCS.ItemDueDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND DCS.ItemDueDate < CAST(GETDATE() AS DATE) AND DCS.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ItemDescription, PT.ProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 10,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT ID.Title, ID.ProviderProjectTeamId, ID.ReceiverProjectTeamId, ID.ItemPriority, ID.ItemDueDate, CASE WHEN ID.ItemPriority = 'Critical' THEN 1 WHEN ID.ItemPriority = 'High' THEN 2 WHEN ID.ItemPriority = 'Medium' THEN 3 WHEN ID.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Interdependencies ID WHERE ID.InterdependencyStatusId =26 AND ID.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND ID.ReportingLevelId IN (4, 5) AND ID.ItemDueDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND ID.ItemDueDate < CAST(GETDATE() AS DATE) AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ProviderProjectTeamId, PT.ReceiverProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 11,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT WP.Title, WP.TaskDueDate, WP.ReportingLevelId, WP.Priority, WP.WorkPlanTaskType, CASE WHEN WP.Priority = '(1) High' THEN 1 WHEN WP.Priority = '(2) Medium' THEN 2 WHEN WP.Priority = '(3) Low' THEN 3 END AS PriorityRank FROM Workplan WP WHERE WP.WorkPlanTaskType IN ('Milestone','Task') AND WP.TimeBasedCalculatedStatusId NOT IN (26, 6, 20, 23, 28, 29, 31) AND WP.Priority IN ('(1) High', '(2) Medium', '(3) Low') AND WP.ReportingLevelId IN (4, 5) AND WP.TaskDueDate >= GETDATE() AND WP.TaskDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND WP.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.TaskDueDate, PT.ReportingLevelId, PT.Priority, PT.WorkPlanTaskType FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 12,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT RAI.IssueRiskCategory, RAI.TimeBasedCalculatedStatusId, RAI.ItemDescription, RAI.ItemDueDate, RAI.ItemPriority, RAI.Title, RAI.ReportingLevelId, CASE WHEN RAI.ItemPriority = 'Critical' THEN 1 WHEN RAI.ItemPriority = 'High' THEN 2 WHEN RAI.ItemPriority = 'Medium' THEN 3 WHEN RAI.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM RisksAndIssues RAI WHERE RAI.TimeBasedCalculatedStatusId NOT IN (26, 6, 20, 23, 28, 29, 31) AND RAI.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND RAI.ReportingLevelId IN (4, 5) AND RAI.ItemDueDate >= GETDATE() AND RAI.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND RAI.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.IssueRiskCategory, PT.TimeBasedCalculatedStatusId, PT.ItemDescription, PT.ItemDueDate, PT.ItemPriority, PT.Title, PT.ReportingLevelId FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 13,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT ACT.Title, ACT.ItemDescription, ACT.ProjectTeamId, ACT.ItemPriority, ACT.ItemDueDate, CASE WHEN ACT.ItemPriority = 'Critical' THEN 1 WHEN ACT.ItemPriority = 'High' THEN 2 WHEN ACT.ItemPriority = 'Medium' THEN 3 WHEN ACT.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Actions ACT WHERE ACT.ItemStatusId NOT IN (26, 6, 20, 23, 28, 29, 31) AND ACT.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND ACT.ReportingLevelId IN (4, 5) AND ACT.ItemDueDate >= GETDATE() AND ACT.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND ACT.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ItemDescription, PT.ProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 14,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT DCS.Title, DCS.ItemDescription, DCS.ProjectTeamId, DCS.ItemPriority, DCS.ItemDueDate, CASE WHEN DCS.ItemPriority = 'Critical' THEN 1 WHEN DCS.ItemPriority = 'High' THEN 2 WHEN DCS.ItemPriority = 'Medium' THEN 3 WHEN DCS.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Decisions DCS WHERE DCS.ItemStatusId NOT IN (26, 6, 20, 23, 28, 29, 31) AND DCS.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND DCS.ReportingLevelId IN (4, 5) AND DCS.ItemDueDate >= GETDATE() AND DCS.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND DCS.ProjectTeamId = {{ProjectTeam}}), HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ItemDescription, PT.ProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 15,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (SELECT ID.Title, ID.ProviderProjectTeamId, ID.ReceiverProjectTeamId, ID.ItemPriority, ID.ItemDueDate, CASE WHEN ID.ItemPriority = 'Critical' THEN 1 WHEN ID.ItemPriority = 'High' THEN 2 WHEN ID.ItemPriority = 'Medium' THEN 3 WHEN ID.ItemPriority = 'Low' THEN 4 END AS PriorityRank FROM Interdependencies ID WHERE ID.InterdependencyStatusId NOT IN (6, 20, 23, 26, 28, 29, 31) AND ID.ItemPriority IN ('Critical', 'High', 'Medium', 'Low') AND ID.ReportingLevelId IN (4, 5) AND ID.ItemDueDate <= DATEADD(day, 7, CURRENT_TIMESTAMP) AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})),HighestPriority AS (SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks) SELECT PT.Title, PT.ProviderProjectTeamId, PT.ReceiverProjectTeamId, PT.ItemPriority, PT.ItemDueDate FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank");
        }
    }
}
