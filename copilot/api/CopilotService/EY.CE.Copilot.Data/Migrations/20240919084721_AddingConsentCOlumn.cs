using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingConsentCOlumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Consent",
                table: "AssistantMessageFeedbacks",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 7,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID  LEFT JOIN ProjectTeams pt on pt.ID = wp.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID  WHERE wp.WorkPlanTaskType = 'Milestone'    AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())    AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED'))  AND TT.[Key] = 'PROJECT_MANAGEMENT' AND up.EMail = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 8,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek  FROM RisksAndIssues RI  LEFT JOIN statuses S ON RI.ItemStatusId = S.ID  LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID  LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE RI.IssueRiskCategory = 'Risk'    AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED'))  AND TT.[Key] = 'PROJECT_MANAGEMENT' AND up.EMail = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 9,
                column: "AnswerSQL",
                value: "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I  LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID LEFT JOIn ProjectTeams PT on PT.ID = I.ReceiverProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE   YEAR(I.ItemDueDate) = YEAR(GETDATE())      AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())      AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED')) AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS WorkPlanItem,  W.TaskDueDate,  W.ActualStartDate,  W.WorkPlanTaskType,  W.TaskDescription,  W.[Priority],  W.IsCritical,  UP.Title TaskOwner,  PT.Title AS ProjectTeam,  S.Title AS [Status]  FROM      Workplan W  LEFT JOIN      ProjectTeams PT ON W.ProjectTeamId = PT.ID  LEFT JOIN      TeamTypes TT ON PT.TeamTypeId = TT.ID  LEFT JOIN   UserProfiles UP ON W.TaskOwnerId = UP.ID  LEFT JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID  WHERE      TT.[Key] = 'PROJECT_MANAGEMENT'      AND DATEPART(ww, W.TaskDueDate) = DATEPART(ww, GETDATE()) + 1 AND YEAR(W.TaskDueDate) = YEAR(GETDATE()) AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED'))");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 22,
                column: "AnswerSQL",
                value: "SELECT      WS.Title, PT.Title AS ProjectTeams, RP.PeriodStartDate, RP.PeriodEndDate, RP.Title ReportingPeriod, PSE.ID AS ProjectStatusEntriesID FROM      ProjectStatusEntries PSE JOIN      ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN      WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID  JOIN      ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE      PT.Title = {ProjectTeam}      AND  RP.ID = (  SELECT RPID.ID - 1         FROM ReportingPeriods RPID         WHERE CAST (GETDATE() as DATE) BETWEEN CAST (RPID.PeriodStartDate as DATE) AND CAST (RPID.PeriodEndDate as DATE))");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 23,
                column: "AnswerSQL",
                value: "SELECT AN.Title, AN.AccomplishmentNextstepCategory, WS.Title, PT.Title AS ProjectTeams, RP.PeriodStartDate, RP.PeriodEndDate, RP.Title ReportingPeriod, PSE.ID AS ProjectStatusEntriesID  FROM      ProjectStatusEntries PSE JOIN      ProjectTeams PT ON PSE.ProjectTeamID = PT.ID  JOIN      AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN      ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID  JOIN      WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID    WHERE      PT.Title = {ProjectTeam}      AND RP.ID = (SELECT RPID.ID - 1 FROM ReportingPeriods RPID WHERE CAST( GETDATE() as DATE) BETWEEN CAST(RPID.PeriodStartDate as DATE) AND CAST(RPID.PeriodEndDate as DATE)) ORDER BY      AN.AccomplishmentNextstepCategory");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 40,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedLastWeek   FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID     JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID    JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  YEAR(W.ActualEndDate) = YEAR(GETDATE()) AND DATEPART(ww, W.ActualEndDate) = DATEPART(ww, GETDATE()) -1 AND S.[KEY] IN ('COMPLETED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 41,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedThisWeek   FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID     JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID    JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  YEAR(W.ActualEndDate) = YEAR(GETDATE()) AND DATEPART(ww, W.ActualEndDate) = DATEPART(ww, GETDATE()) AND S.[KEY] IN ('COMPLETED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 42,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksDueThisWeek  FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID    JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID    JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE  YEAR(W.ActualEndDate) = YEAR(GETDATE()) AND DATEPART(ww, W.ActualEndDate) = DATEPART(ww, GETDATE()) AND S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 43,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksDueNextWeek   FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID     JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID   JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    YEAR(W.ActualEndDate) = YEAR(GETDATE()) AND DATEPART(ww, W.ActualEndDate) = DATEPART(ww, GETDATE()) + 1   AND S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "AnswerSQL",
                value: "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount,  DENSE_RANK() OVER (ORDER BY COUNT(PT.ID)) ROW_NUM FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE      YEAR(W.Modified) = YEAR(GETDATE()) AND DATEPART(ww, W.Modified) = DATEPART(ww, GETDATE()) - 1   AND PT.ItemIsActive=1 AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY PT.Title) A");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                column: "AnswerSQL",
                value: "SELECT  W.UniqueItemIdentifier,     W.Title WorkPlanItem, W.TaskDueDate, W.ActualStartDate, W.WorkPlanTaskType, W.TaskDescription, W.[Priority], W.IsCritical, UP.Title TaskOwner, PT.Title AS ProjectTeam, S.Title AS [Status]  FROM  Workplan W  LEFT JOIN  ProjectTeams PT ON W.ProjectTeamId = PT.ID  LEFT JOIN  TeamTypes TT ON PT.TeamTypeId = TT.ID  JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN  UserProfiles UP ON W.TaskOwnerId = UP.ID   WHERE  TT.[Key] = 'PROJECT_MANAGEMENT'  AND DATEPART(ww,W.TaskDueDate) = DATEPART(ww,GETDATE()) AND YEAR(W.TaskDueDate) = YEAR(GETDATE()) AND S.[KEY] NOT IN ('COMPLETED', 'CANCELLED')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "AnswerSQL",
                value: "select PSE.ExecutiveSummary from ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID  JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID  WHERE PT.Title = 'Finance' AND RP.ID = (SELECT RPID.ID  FROM ReportingPeriods RPID WHERE CAST(GETDATE() as DATE) BETWEEN CAST(RPID.PeriodStartDate as DATE) AND CAST (RPID.PeriodEndDate as DATE))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consent",
                table: "AssistantMessageFeedbacks");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 7,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID  LEFT JOIN ProjectTeams pt on pt.ID = wp.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID  WHERE wp.WorkPlanTaskType = 'Milestone'    AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())    AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED')) AND TT.[Key] = 'PROJECT_MANAGEMENT' AND up.EMail = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 8,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek  FROM RisksAndIssues RI  LEFT JOIN statuses S ON RI.ItemStatusId = S.ID  LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID  LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE RI.IssueRiskCategory = 'Risk'    AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())    AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND TT.[Key] = 'PROJECT_MANAGEMENT' AND up.EMail = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 9,
                column: "AnswerSQL",
                value: "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      YEAR(I.ItemDueDate) = YEAR(GETDATE())     AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS WorkPlanItem,  W.TaskDueDate,  W.ActualStartDate,  W.WorkPlanTaskType,  W.TaskDescription,  W.[Priority],  W.IsCritical,  UP.Title TaskOwner,  PT.Title AS ProjectTeam,  S.Title AS [Status] FROM      Workplan W LEFT JOIN      ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN      TeamTypes TT ON PT.TeamTypeId = TT.ID LEFT JOIN   UserProfiles UP ON W.TaskOwnerId = UP.ID LEFT JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID WHERE      TT.[Key] = 'PROJECT_MANAGEMENT'     AND DATEPART(ww, W.TaskDueDate) = DATEPART(ww, GETDATE()) + 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 22,
                column: "AnswerSQL",
                value: "SELECT      WS.Title, PT.Title AS ProjectTeams, RP.PeriodStartDate, RP.PeriodEndDate, RP.Title ReportingPeriod, PSE.ID AS ProjectStatusEntriesID FROM      ProjectStatusEntries PSE JOIN      ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN      WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID JOIN      ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE      PT.Title = {ProjectTeam}     AND  RP.ID = (         SELECT RPID.ID - 1         FROM ReportingPeriods RPID         WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate     )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 23,
                column: "AnswerSQL",
                value: "SELECT      AN.Title,      AN.AccomplishmentNextstepCategory, WS.Title, PT.Title AS ProjectTeams, RP.PeriodStartDate, RP.PeriodEndDate, RP.Title ReportingPeriod, PSE.ID AS ProjectStatusEntriesID FROM      ProjectStatusEntries PSE JOIN      ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN      AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN      ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID JOIN      WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID   WHERE      PT.Title = {ProjectTeam}     AND RP.ID = (         SELECT RPID.ID - 1         FROM ReportingPeriods RPID         WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate     ) ORDER BY      AN.AccomplishmentNextstepCategory");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 40,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedLastWeek  FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID    JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID   JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    CAST(ActualEndDate AS DATE) >= DATEADD(week,DATEDIFF(week, 0, GETDATE()) -1,0)    AND CAST(ActualEndDate AS DATE) < DATEADD(week, DATEDIFF(week, 0, GETDATE()), 0)    AND S.[KEY] IN ('COMPLETED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 41,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedLastWeek  FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID    JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID   JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  CAST(ActualEndDate AS DATE) >= CAST(DATEADD(week,DATEDIFF(week,0,GETDATE()), 0) AS DATE)   AND CAST(ActualEndDate AS DATE) < CAST( DATEADD(week,DATEDIFF(week,0, GETDATE()) + 1,0) AS DATE)   AND S.[KEY] IN ('COMPLETED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 42,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedLastWeek  FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID    JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID   JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  CAST(W.ActualEndDate AS DATE) >= CAST(DATEADD(week,DATEDIFF(week,0,GETDATE()), 0) AS DATE)   AND CAST(W.ActualEndDate AS DATE) < CAST( DATEADD(week,DATEDIFF(week,0, GETDATE()) + 1,0) AS DATE)   AND S.[KEY] NOT IN ('COMPLETED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 43,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedLastWeek  FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID    JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID   JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE   CAST(TaskDueDate AS DATE) >=  CAST(DATEADD(week, DATEDIFF(week,0, GETDATE()) + 1,0) AS DATE)   AND CAST(TaskDueDate AS DATE) < CAST(DATEADD(week,DATEDIFF(week,0,GETDATE()) + 2,0) AS DATE)   AND S.[KEY] NOT IN ('COMPLETED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "AnswerSQL",
                value: "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID)) ROW_NUM FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      CAST(W.Modified AS DATE) BETWEEN DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND PT.ItemIsActive=1 AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY PT.Title) A");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                column: "AnswerSQL",
                value: "SELECT  W.UniqueItemIdentifier,     W.Title WorkPlanItem, W.TaskDueDate, W.ActualStartDate, W.WorkPlanTaskType, W.TaskDescription, W.[Priority], W.IsCritical, UP.Title TaskOwner, PT.Title AS ProjectTeam, S.Title AS [Status] FROM  Workplan W  LEFT JOIN  ProjectTeams PT ON W.ProjectTeamId = PT.ID  LEFT JOIN  TeamTypes TT ON PT.TeamTypeId = TT.ID  JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN  UserProfiles UP ON W.TaskOwnerId = UP.ID  WHERE  TT.[Key] = 'PROJECT_MANAGEMENT'  AND DATEPART(ww,W.TaskDueDate) = DATEPART(ww,GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "AnswerSQL",
                value: "select PSE.ExecutiveSummary from ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID  JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = 'Finance' AND RP.ID = (SELECT RPID.ID  FROM ReportingPeriods RPID WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate)");
        }
    }
}
