using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSQLForContentGenerator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 1,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        WP.Title, \r\n        WP.TaskDueDate, \r\n        WP.ReportingLevelId, \r\n        WP.Priority, \r\n        WP.WorkPlanTaskType, \r\n        CASE \r\n            WHEN WP.Priority = '(1) High' THEN 1\r\n            WHEN WP.Priority = '(2) Normal' THEN 2\r\n            WHEN WP.Priority = '(3) Low' THEN 3\r\n            WHEN WP.Priority IS NULL OR WP.Priority= '' THEN 4\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN WP.TaskDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN WP.TaskDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title as StatusTitle\r\n    FROM Workplan WP\r\n    JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id\r\n    WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task')\r\n	AND S.[Key] NOT IN ('CANCELLED')\r\n    AND WP.HierarchyLevel <= 3\r\n    AND WP.TaskDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND WP.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.TaskDueDate, \r\n    PT.ReportingLevelId, \r\n    PT.Priority, \r\n    PT.WorkPlanTaskType,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 2,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        RAI.IssueRiskCategory, \r\n        RAI.ItemStatusId , \r\n        RAI.ItemDescription, \r\n        RAI.ItemDueDate, \r\n        RAI.ItemPriority, \r\n        RAI.Title, \r\n        RAI.ReportingLevelId, \r\n        CASE \r\n            WHEN RAI.ItemPriority = 'Critical' THEN 1\r\n            WHEN RAI.ItemPriority = 'High' THEN 2\r\n            WHEN RAI.ItemPriority = 'Medium' THEN 3\r\n            WHEN RAI.ItemPriority = 'Low' THEN 4\r\n            WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN RAI.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN RAI.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM RisksAndIssues RAI\r\n    JOIN Statuses S ON RAI.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND RAI.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND RAI.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.IssueRiskCategory, \r\n    PT.ItemStatusId , \r\n    PT.ItemDescription, \r\n    PT.ItemDueDate, \r\n    PT.ItemPriority, \r\n    PT.Title, \r\n    PT.ReportingLevelId,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 3,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        ACT.Title, \r\n        ACT.ItemDescription, \r\n        ACT.ProjectTeamId, \r\n        ACT.ItemPriority, \r\n        ACT.ItemDueDate, \r\n        ACT.ReportingLevelId, \r\n        CASE \r\n            WHEN ACT.ItemPriority = 'Critical' THEN 1\r\n            WHEN ACT.ItemPriority = 'High' THEN 2\r\n            WHEN ACT.ItemPriority = 'Medium' THEN 3\r\n            WHEN ACT.ItemPriority = 'Low' THEN 4\r\n            WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN ACT.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN ACT.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Actions ACT\r\n    JOIN Statuses S ON ACT.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND ACT.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND ACT.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 4,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        DCS.Title, \r\n        DCS.ItemDescription, \r\n        DCS.ProjectTeamId, \r\n        DCS.ItemPriority, \r\n        DCS.ItemDueDate, \r\n        DCS.ReportingLevelId, \r\n        CASE \r\n            WHEN DCS.ItemPriority = 'Critical' THEN 1\r\n            WHEN DCS.ItemPriority = 'High' THEN 2\r\n            WHEN DCS.ItemPriority = 'Medium' THEN 3\r\n            WHEN DCS.ItemPriority = 'Low' THEN 4\r\n            WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN DCS.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN DCS.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Decisions DCS\r\n    JOIN Statuses S ON DCS.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND DCS.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND DCS.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 5,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS (\r\n    SELECT \r\n        ID.Title, \r\n        ID.ProviderProjectTeamId, \r\n        ID.ReceiverProjectTeamId, \r\n        ID.ItemDueDate, \r\n        ID.ReportingLevelId,\r\n        CASE \r\n            WHEN ID.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN ID.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Interdependencies ID\r\n    JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id \r\n    WHERE S.[Key] NOT IN ('CANCELLED')\r\n    AND ID.ItemDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})\r\n)\r\nSELECT \r\n    Title, \r\n    ProviderProjectTeamId, \r\n    ReceiverProjectTeamId, \r\n    ItemDueDate,\r\n    ItemCategory,\r\n    StatusTitle\r\nFROM FilteredInterdependencies;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 6,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        WP.Title, \r\n        WP.TaskDueDate, \r\n        WP.ReportingLevelId, \r\n        WP.Priority, \r\n        WP.WorkPlanTaskType, \r\n        CASE \r\n            WHEN WP.Priority = '(1) High' THEN 1\r\n            WHEN WP.Priority = '(2) Normal' THEN 2\r\n            WHEN WP.Priority = '(3) Low' THEN 3\r\n            WHEN WP.Priority IS NULL OR  WP.Priority = '' THEN 4\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN WP.TaskDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN WP.TaskDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Workplan WP\r\n    JOIN Statuses S ON WP.WorkPlanTaskStatusId= S.Id\r\n    WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task')\r\n    AND S.[Key]  IN ('CLOSED', 'COMPLETED')\r\n    AND WP.TaskDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND WP.HierarchyLevel <= 3\r\n    AND WP.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.TaskDueDate, \r\n    PT.ReportingLevelId, \r\n    PT.Priority, \r\n    PT.WorkPlanTaskType,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 7,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        RAI.IssueRiskCategory, \r\n        RAI.TimeBasedCalculatedStatusId, \r\n        RAI.ItemDescription, \r\n        RAI.ItemDueDate, \r\n        RAI.ItemPriority, \r\n        RAI.Title, \r\n        RAI.ReportingLevelId, \r\n        CASE \r\n            WHEN RAI.ItemPriority = 'Critical' THEN 1\r\n            WHEN RAI.ItemPriority = 'High' THEN 2\r\n            WHEN RAI.ItemPriority = 'Medium' THEN 3\r\n            WHEN RAI.ItemPriority = 'Low' THEN 4\r\n            WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN RAI.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN RAI.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM RisksAndIssues RAI\r\n    JOIN Statuses S ON RAI.ItemStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND RAI.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n   AND RAI.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.IssueRiskCategory, \r\n    PT.ItemDescription, \r\n    PT.ItemDueDate, \r\n    PT.ItemPriority, \r\n    PT.Title, \r\n    PT.ReportingLevelId,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 8,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        ACT.Title, \r\n        ACT.ItemDescription, \r\n        ACT.ProjectTeamId, \r\n        ACT.ItemPriority, \r\n        ACT.ItemDueDate, \r\n        CASE \r\n            WHEN ACT.ItemPriority = 'Critical' THEN 1\r\n            WHEN ACT.ItemPriority = 'High' THEN 2\r\n            WHEN ACT.ItemPriority = 'Medium' THEN 3\r\n            WHEN ACT.ItemPriority = 'Low' THEN 4\r\n            WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN ACT.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN ACT.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Actions ACT\r\n    JOIN Statuses S ON ACT.ItemStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND ACT.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND ACT.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 9,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        DCS.Title, \r\n        DCS.ItemDescription, \r\n        DCS.ProjectTeamId, \r\n        DCS.ItemPriority, \r\n        DCS.ItemDueDate, \r\n        CASE \r\n            WHEN DCS.ItemPriority = 'Critical' THEN 1\r\n            WHEN DCS.ItemPriority = 'High' THEN 2\r\n            WHEN DCS.ItemPriority = 'Medium' THEN 3\r\n            WHEN DCS.ItemPriority = 'Low' THEN 4\r\n            WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN DCS.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN DCS.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Decisions DCS\r\n    JOIN Statuses S ON DCS.ItemStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND DCS.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND DCS.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 10,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS (\r\n    SELECT \r\n        ID.Title, \r\n        ID.ProviderProjectTeamId, \r\n        ID.ReceiverProjectTeamId, \r\n        ID.ItemDueDate, \r\n        ID.ReportingLevelId,\r\n        CASE \r\n            WHEN ID.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN ID.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Interdependencies ID\r\n    JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id\r\n    WHERE S.[Key] IN ('CLOSED', 'COMPLETED')\r\n    AND ID.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE()\r\n    AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})\r\n)\r\nSELECT \r\n    Title, \r\n    ProviderProjectTeamId, \r\n    ReceiverProjectTeamId, \r\n    ItemDueDate,\r\n    ItemCategory,\r\n    StatusTitle\r\nFROM FilteredInterdependencies;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 11,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        WP.Title, \r\n        WP.TaskDueDate, \r\n        WP.ReportingLevelId, \r\n        WP.Priority, \r\n        WP.WorkPlanTaskType,\r\n        CASE \r\n            WHEN WP.Priority = '(1) High' THEN 1\r\n            WHEN WP.Priority = '(2) Normal' THEN 2\r\n            WHEN WP.Priority = '(3) Low' THEN 3\r\n            WHEN WP.Priority IS NULL OR WP.Priority = '' THEN 4\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN WP.TaskDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN WP.TaskDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Workplan WP\r\n    JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id\r\n    WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task')\r\n    AND S.[Key] NOT IN ('CLOSED', 'CANCELLED', 'COMPLETED')\r\n	AND WP.HierarchyLevel <= 3\r\n    AND WP.TaskDueDate BETWEEN DATEADD(day, -7, CURRENT_TIMESTAMP) AND DATEADD(day, 7, CURRENT_TIMESTAMP)\r\n    AND WP.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.TaskDueDate, \r\n    PT.ReportingLevelId, \r\n    PT.Priority, \r\n    PT.WorkPlanTaskType,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 12,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        RAI.IssueRiskCategory, \r\n        S.Title AS ItemStatus,\r\n        RAI.ItemDescription, \r\n        RAI.ItemDueDate, \r\n        RAI.ItemPriority, \r\n        RAI.Title, \r\n        RAI.ReportingLevelId,\r\n        CASE \r\n            WHEN RAI.ItemPriority = 'Critical' THEN 1\r\n            WHEN RAI.ItemPriority = 'High' THEN 2\r\n            WHEN RAI.ItemPriority = 'Medium' THEN 3\r\n            WHEN RAI.ItemPriority = 'Low' THEN 4\r\n            WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN RAI.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN RAI.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM RisksAndIssues RAI\r\n    JOIN Statuses S ON RAI.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n    AND RAI.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n    AND RAI.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.IssueRiskCategory, \r\n    PT.ItemDescription, \r\n    PT.ItemDueDate, \r\n    PT.ItemPriority, \r\n    PT.Title, \r\n    PT.ReportingLevelId,\r\n    PT.ItemStatus,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 13,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        ACT.Title, \r\n        ACT.ItemDescription, \r\n        ACT.ProjectTeamId, \r\n        ACT.ItemPriority, \r\n        ACT.ItemDueDate, \r\n        S.Title AS ItemStatus,\r\n        CASE \r\n            WHEN ACT.ItemPriority = 'Critical' THEN 1\r\n            WHEN ACT.ItemPriority = 'High' THEN 2\r\n            WHEN ACT.ItemPriority = 'Medium' THEN 3\r\n            WHEN ACT.ItemPriority = 'Low' THEN 4\r\n            WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN ACT.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN ACT.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Actions ACT\r\n    JOIN Statuses S ON ACT.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n    AND ACT.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n    AND ACT.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemStatus,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 14,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS (\r\n    SELECT \r\n        DCS.Title, \r\n        DCS.ItemDescription, \r\n        DCS.ProjectTeamId, \r\n        DCS.ItemPriority, \r\n        DCS.ItemDueDate, \r\n        S.Title AS ItemStatus,\r\n        CASE \r\n            WHEN DCS.ItemPriority = 'Critical' THEN 1\r\n            WHEN DCS.ItemPriority = 'High' THEN 2\r\n            WHEN DCS.ItemPriority = 'Medium' THEN 3\r\n            WHEN DCS.ItemPriority = 'Low' THEN 4\r\n            WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5\r\n        END AS PriorityRank,\r\n        CASE \r\n            WHEN DCS.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN DCS.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Decisions DCS\r\n    JOIN Statuses S ON DCS.ItemStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n    AND DCS.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n    AND DCS.ProjectTeamId = {{ProjectTeam}}\r\n),\r\nHighestPriority AS (\r\n    SELECT MIN(PriorityRank) AS MinPriorityRank\r\n    FROM PriorityTasks\r\n)\r\nSELECT \r\n    PT.Title, \r\n    PT.ItemDescription, \r\n    PT.ProjectTeamId, \r\n    PT.ItemPriority, \r\n    PT.ItemDueDate,\r\n    PT.ItemStatus,\r\n    PT.ItemCategory,\r\n    PT.StatusTitle\r\nFROM PriorityTasks PT\r\nJOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;\r\n\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 15,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS (\r\n    SELECT \r\n        ID.Title, \r\n        ID.ProviderProjectTeamId, \r\n        ID.ReceiverProjectTeamId, \r\n        ID.ItemDueDate,\r\n        S.Title AS ItemStatus,\r\n        CASE \r\n            WHEN ID.ItemDueDate <= GETDATE() THEN 'Was past due current week' \r\n            WHEN ID.ItemDueDate > GETDATE() THEN 'Will be past due next week' \r\n        END AS ItemCategory,\r\n        S.Title AS StatusTitle\r\n    FROM Interdependencies ID\r\n    JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id\r\n    WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')\r\n  	AND ID.ItemDueDate BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, 7, GETDATE())\r\n	AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}})\r\n)\r\nSELECT \r\n    Title, \r\n    ProviderProjectTeamId, \r\n    ReceiverProjectTeamId, \r\n    ItemDueDate,\r\n    ItemStatus,\r\n    ItemCategory,\r\n    StatusTitle\r\nFROM FilteredInterdependencies;\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 16,
                column: "SQLQuery",
                value: "WITH StatusCheck AS (\r\n    SELECT\r\n        s.[Key],\r\n        s.ID\r\n    FROM\r\n        WorkPlan wp\r\n    JOIN\r\n        Statuses s ON wp.WorkPlanTaskStatusId = s.ID\r\n    WHERE\r\n        wp.ProjectTeamId = {{ProjectTeam}}\r\n        AND wp.TaskDueDate < GETDATE()\r\n        AND wp.TaskDueDate >= DATEADD(DAY, -7, GETDATE())\r\n),\r\nFinalStatus AS (\r\n    SELECT\r\n        wss.ID,\r\n		wss.[Key],\r\n        wss.Title\r\n    FROM\r\n        StatusCheck sc\r\n    JOIN\r\n        WeeklyStatusStatuses wss ON sc.[Key] = wss.[Key]\r\n)\r\nSELECT\r\n    CASE\r\n        WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE')\r\n        WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'AT_RISK') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'AT_RISK')\r\n        WHEN EXISTS (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'ON_TRACK') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'ON_TRACK')\r\n        ELSE NULL\r\n    END AS StatusId,\r\n    CASE\r\n        WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE')\r\n        WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'AT_RISK') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'AT_RISK')\r\n        WHEN EXISTS (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'ON_TRACK') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'ON_TRACK')\r\n        ELSE NULL\r\n    END AS StatusTitle;\r\n\r\n\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 16,
                column: "SQLQuery",
                value: "SELECT CASE WHEN COUNT(CASE WHEN WorkPlanTaskStatusId = 1 THEN 1 END) > 0 THEN 'Behind Schedule' WHEN COUNT(CASE WHEN WorkPlanTaskStatusId = 2 THEN 1 END) > 0 THEN 'At Risk' ELSE 'On Track' END AS GeneralStatus FROM WorkPlan WHERE ProjectTeamId = {{ProjectTeam}} AND TaskDueDate < GETDATE() AND TaskDueDate >= DATEADD(DAY, -7, GETDATE())");
        }
    }
}
