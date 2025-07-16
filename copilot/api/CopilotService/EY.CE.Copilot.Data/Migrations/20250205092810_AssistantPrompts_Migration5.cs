using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AssistantPrompts_Migration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssistantPrompts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Backend = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    ChangeDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantPrompts", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 1,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      WP.Title,      WP.TaskDueDate,      WP.ReportingLevelId,      WP.Priority,      WP.WorkPlanTaskType,      CASE          WHEN WP.Priority = '(1) High' THEN 1         WHEN WP.Priority = '(2) Normal' THEN 2         WHEN WP.Priority = '(3) Low' THEN 3         WHEN WP.Priority IS NULL OR WP.Priority= '' THEN 4     END AS PriorityRank,     CASE          WHEN WP.TaskDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN WP.TaskDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title as StatusTitle FROM Workplan WP JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task') AND S.[Key] NOT IN ('CANCELLED') AND WP.HierarchyLevel <= 3 AND WP.TaskDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}}) AND WP.ProjectTeamId = {{ProjectTeam}}),HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.TaskDueDate,  PT.ReportingLevelId,  PT.Priority,  PT.WorkPlanTaskType, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 2,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      RAI.IssueRiskCategory,      RAI.ItemStatusId ,      RAI.ItemDescription,      RAI.ItemDueDate,      RAI.ItemPriority,      RAI.Title,      RAI.ReportingLevelId,      CASE          WHEN RAI.ItemPriority = 'Critical' THEN 1         WHEN RAI.ItemPriority = 'High' THEN 2         WHEN RAI.ItemPriority = 'Medium' THEN 3         WHEN RAI.ItemPriority = 'Low' THEN 4         WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  RAI.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN RAI.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM RisksAndIssues RAI JOIN Statuses S ON RAI.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED') AND RAI.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND RAI.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.IssueRiskCategory,  PT.ItemStatusId ,  PT.ItemDescription,  PT.ItemDueDate,  PT.ItemPriority,  PT.Title,  PT.ReportingLevelId, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 3,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      ACT.Title,      ACT.ItemDescription,      ACT.ProjectTeamId,      ACT.ItemPriority,      ACT.ItemDueDate,      ACT.ReportingLevelId,      CASE          WHEN ACT.ItemPriority = 'Critical' THEN 1         WHEN ACT.ItemPriority = 'High' THEN 2         WHEN ACT.ItemPriority = 'Medium' THEN 3         WHEN ACT.ItemPriority = 'Low' THEN 4         WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  ACT.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ACT.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Actions ACT JOIN Statuses S ON ACT.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED') AND ACT.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND ACT.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 4,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      DCS.Title,      DCS.ItemDescription,      DCS.ProjectTeamId,      DCS.ItemPriority,      DCS.ItemDueDate,      DCS.ReportingLevelId,      CASE          WHEN DCS.ItemPriority = 'Critical' THEN 1         WHEN DCS.ItemPriority = 'High' THEN 2         WHEN DCS.ItemPriority = 'Medium' THEN 3         WHEN DCS.ItemPriority = 'Low' THEN 4         WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  DCS.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN DCS.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Decisions DCS JOIN Statuses S ON DCS.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED') AND DCS.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND DCS.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 5,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS ( SELECT      ID.Title,      ID.ProviderProjectTeamId,      ID.ReceiverProjectTeamId,      ID.ItemDueDate,      ID.ReportingLevelId,     CASE          WHEN  ID.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ID.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Interdependencies ID JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id  WHERE S.[Key] NOT IN ('CANCELLED') AND ID.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}}) ) SELECT  Title,  ProviderProjectTeamId,  ReceiverProjectTeamId,  ItemDueDate, ItemCategory, StatusTitle FROM FilteredInterdependencies;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 6,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      WP.Title,      WP.TaskDueDate,      WP.ReportingLevelId,      WP.Priority,      WP.WorkPlanTaskType,      CASE          WHEN WP.Priority = '(1) High' THEN 1         WHEN WP.Priority = '(2) Normal' THEN 2         WHEN WP.Priority = '(3) Low' THEN 3         WHEN WP.Priority IS NULL OR  WP.Priority = '' THEN 4     END AS PriorityRank,     CASE          WHEN  WP.TaskDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN WP.TaskDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Workplan WP JOIN Statuses S ON WP.WorkPlanTaskStatusId= S.Id WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task') AND S.[Key]  IN ('CLOSED', 'COMPLETED') AND WP.TaskDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND WP.HierarchyLevel <= 3 AND WP.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.TaskDueDate,  PT.ReportingLevelId,  PT.Priority,  PT.WorkPlanTaskType, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 7,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      RAI.IssueRiskCategory,      RAI.TimeBasedCalculatedStatusId,      RAI.ItemDescription,      RAI.ItemDueDate,      RAI.ItemPriority,      RAI.Title,      RAI.ReportingLevelId,      CASE          WHEN RAI.ItemPriority = 'Critical' THEN 1         WHEN RAI.ItemPriority = 'High' THEN 2         WHEN RAI.ItemPriority = 'Medium' THEN 3         WHEN RAI.ItemPriority = 'Low' THEN 4         WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  RAI.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN RAI.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM RisksAndIssues RAI JOIN Statuses S ON RAI.ItemStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND RAI.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}      AND RAI.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.IssueRiskCategory,  PT.ItemDescription,  PT.ItemDueDate,  PT.ItemPriority,  PT.Title,  PT.ReportingLevelId, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 8,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      ACT.Title,      ACT.ItemDescription,      ACT.ProjectTeamId,      ACT.ItemPriority,      ACT.ItemDueDate,      CASE          WHEN ACT.ItemPriority = 'Critical' THEN 1         WHEN ACT.ItemPriority = 'High' THEN 2         WHEN ACT.ItemPriority = 'Medium' THEN 3         WHEN ACT.ItemPriority = 'Low' THEN 4         WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  ACT.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ACT.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Actions ACT JOIN Statuses S ON ACT.ItemStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND ACT.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}   AND ACT.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;   ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 9,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      DCS.Title,      DCS.ItemDescription,      DCS.ProjectTeamId,      DCS.ItemPriority,      DCS.ItemDueDate,      CASE          WHEN DCS.ItemPriority = 'Critical' THEN 1         WHEN DCS.ItemPriority = 'High' THEN 2         WHEN DCS.ItemPriority = 'Medium' THEN 3         WHEN DCS.ItemPriority = 'Low' THEN 4         WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  DCS.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN DCS.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Decisions DCS JOIN Statuses S ON DCS.ItemStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND DCS.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}   AND DCS.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;   ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 10,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS ( SELECT      ID.Title,      ID.ProviderProjectTeamId,      ID.ReceiverProjectTeamId,      ID.ItemDueDate,      ID.ReportingLevelId,     CASE          WHEN  ID.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ID.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Interdependencies ID JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND ID.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}   AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}}) ) SELECT  Title,  ProviderProjectTeamId,  ReceiverProjectTeamId,  ItemDueDate, ItemCategory, StatusTitle FROM FilteredInterdependencies;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 11,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      WP.Title,      WP.TaskDueDate,      WP.ReportingLevelId,      WP.Priority,      WP.WorkPlanTaskType,     CASE          WHEN WP.Priority = '(1) High' THEN 1         WHEN WP.Priority = '(2) Normal' THEN 2         WHEN WP.Priority = '(3) Low' THEN 3         WHEN WP.Priority IS NULL OR WP.Priority = '' THEN 4     END AS PriorityRank,     CASE          WHEN  WP.TaskDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN WP.TaskDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Workplan WP JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task') AND S.[Key] NOT IN ('CLOSED', 'CANCELLED', 'COMPLETED')  AND WP.HierarchyLevel <= 3 AND WP.TaskDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND WP.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.TaskDueDate,  PT.ReportingLevelId,  PT.Priority,  PT.WorkPlanTaskType, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank; ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 12,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      RAI.IssueRiskCategory,      S.Title AS ItemStatus,     RAI.ItemDescription,      RAI.ItemDueDate,      RAI.ItemPriority,      RAI.Title,      RAI.ReportingLevelId,     CASE          WHEN RAI.ItemPriority = 'Critical' THEN 1         WHEN RAI.ItemPriority = 'High' THEN 2         WHEN RAI.ItemPriority = 'Medium' THEN 3         WHEN RAI.ItemPriority = 'Low' THEN 4         WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  RAI.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN RAI.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM RisksAndIssues RAI JOIN Statuses S ON RAI.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED') AND RAI.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND RAI.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.IssueRiskCategory,  PT.ItemDescription,  PT.ItemDueDate,  PT.ItemPriority,  PT.Title,  PT.ReportingLevelId, PT.ItemStatus, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 13,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      ACT.Title,      ACT.ItemDescription,      ACT.ProjectTeamId,      ACT.ItemPriority,      ACT.ItemDueDate,      S.Title AS ItemStatus,     CASE          WHEN ACT.ItemPriority = 'Critical' THEN 1         WHEN ACT.ItemPriority = 'High' THEN 2         WHEN ACT.ItemPriority = 'Medium' THEN 3         WHEN ACT.ItemPriority = 'Low' THEN 4         WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  ACT.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ACT.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Actions ACT JOIN Statuses S ON ACT.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED') AND ACT.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND ACT.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemStatus, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 14,
                column: "SQLQuery",
                value: "WITH PriorityTasks AS ( SELECT      DCS.Title,      DCS.ItemDescription,      DCS.ProjectTeamId,      DCS.ItemPriority,      DCS.ItemDueDate,      S.Title AS ItemStatus,     CASE          WHEN DCS.ItemPriority = 'Critical' THEN 1         WHEN DCS.ItemPriority = 'High' THEN 2         WHEN DCS.ItemPriority = 'Medium' THEN 3         WHEN DCS.ItemPriority = 'Low' THEN 4         WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  DCS.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN DCS.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Decisions DCS JOIN Statuses S ON DCS.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED') AND DCS.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND DCS.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemStatus, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;   ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 15,
                column: "SQLQuery",
                value: "WITH FilteredInterdependencies AS ( SELECT      ID.Title,      ID.ProviderProjectTeamId,      ID.ReceiverProjectTeamId,      ID.ItemDueDate,     S.Title AS ItemStatus,     CASE          WHEN  ID.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ID.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Interdependencies ID JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')    AND ID.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})    AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}}) ) SELECT  Title,  ProviderProjectTeamId,  ReceiverProjectTeamId,  ItemDueDate, ItemStatus, ItemCategory, StatusTitle FROM FilteredInterdependencies;  ");

            migrationBuilder.UpdateData(
                table: "AssistantContentGeneratorQueries",
                keyColumn: "ID",
                keyValue: 16,
                column: "SQLQuery",
                value: "WITH StatusCheck AS ( SELECT     s.[Key],     s.ID FROM     WorkPlan wp JOIN     Statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE     wp.ProjectTeamId = {{ProjectTeam}}     AND wp.TaskDueDate < {{periodEndDate}}    AND wp.TaskDueDate >= DATEADD(DAY, -7, {{periodEndDate}}) ), FinalStatus AS ( SELECT     wss.ID,   wss.[Key],     wss.Title FROM     StatusCheck sc JOIN     WeeklyStatusStatuses wss ON sc.[Key] = wss.[Key] ) SELECT CASE     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE')     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'AT_RISK') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'AT_RISK')     WHEN EXISTS (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'ON_TRACK') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'ON_TRACK')     ELSE NULL END AS StatusId, CASE     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE')     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'AT_RISK') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'AT_RISK')     WHEN EXISTS (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'ON_TRACK') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'ON_TRACK')     ELSE NULL END AS StatusTitle;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssistantPrompts");

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
    }
}
