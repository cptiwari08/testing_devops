using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSQLQueryWithNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 4,
                column: "AnswerSQL",
                value: "SELECT wp.Title Workplan, pt.Title ProjectTeam, s.Title [Status] ,Wp.TaskDueDate, wp.StartDate, wp.IsCritical, UP.Title TaskOwner from WorkPlan wp   LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  left join ProjectTeams pt on pt.ID = wp.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId LEFT JOIN UserProfiles UP ON wp.TaskOwnerId = UP.ID where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone' and TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 5,
                column: "AnswerSQL",
                value: "SELECT RI.Title, RI.ItemDescription, RI.ItemDueDate, RI.RiskImpact, RI.RiskMitigation, RI.RiskProbability, PT.Title ProjectTeam, UP.Title ItemOwner  from RisksAndIssues RI    LEFT JOIN Statuses s on RI.ItemStatusId = s.ID    LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId  LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId  LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk' and TT.[Key] = 'PROJECT_MANAGEMENT' ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 6,
                column: "AnswerSQL",
                value: "SELECT I.Title,  R.Title ReceiverTeam, P.Title ProviderTeam, UR.Title ReceiverOwner, UP.Title ProviderOwner, I.ItemDueDate  from Interdependencies I   LEFT JOIN InterdependencyStatuses s on I.InterdependencyStatusId = s.ID   LEFT JOIN ProjectTeams R ON I.ReceiverProjectTeamID = R.ID LEFT JOIN ProjectTeams P ON I.ProviderProjectTeamId = P.ID LEFT JOIN UserProfiles UR ON I.ReceiverOwnerId = UR.ID LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID where s.[Key] = 'BEHIND_SCHEDULE'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT wp.Title Workplan, pt.Title ProjectTeam, s.Title [Status] ,Wp.TaskDueDate, wp.StartDate, wp.IsCritical, UP.Title TaskOwner  from WorkPlan wp    LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID   left join ProjectTeams pt on pt.ID = wp.ProjectTeamId  LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId  LEFT JOIN UserProfiles UP ON wp.TaskOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, {'Username'}) AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                column: "AnswerSQL",
                value: "SELECT TOP 1      CAST(RP.PeriodEndDate AS DATE) AS WeeklyStatusDueDate,     RP.Title AS PeriodTitle,     RP.PeriodStartDate AS StartDate,     RP.PeriodEndDate AS End Date,     RP.Created AS CreatedDate,     RP.Modified AS LastModifiedDate FROM      ReportingPeriods RP  WHERE      CAST(GETDATE() AS DATE) BETWEEN CAST(RP.PeriodStartDate AS DATE) AND CAST(RP.PeriodEndDate AS DATE)  ORDER BY      RP.Modified DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 18,
                column: "AnswerSQL",
                value: "SELECT PT.Title as ProjectTeam, UP.Title AS FunctionalOwner FROM ProjectTeams PT JOIN Userprofiles UP ON PT.ItemOwnerID = UP.ID WHERE PT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 19,
                column: "AnswerSQL",
                value: "SELECT      WP.Title,  WP.TaskDueDate,  WP.ActualStartDate,  WP.WorkPlanTaskType,  WP.TaskDescription,  WP.[Priority],  WP.IsCritical,  UP.Title TaskOwner,  PT.Title AS ProjectTeam,  S.Title AS [Status] FROM     Workplan WP JOIN      ProjectTeams PT ON WP.ProjectTeamId = PT.ID JOIN      Statuses S ON WP.WorkPlanTaskStatusId = S.ID JOIN      TeamTypes TT ON PT.TeamTypeID = TT.ID LEFT JOIN   UserProfiles UP ON wp.TaskOwnerId = UP.ID  WHERE      S.[Key] = 'BEHIND_SCHEDULE'     AND TT.[Key] = 'PROJECT_MANAGEMENT'     AND PT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS WorkPlanItem,  W.TaskDueDate,  W.ActualStartDate,  W.WorkPlanTaskType,  W.TaskDescription,  W.[Priority],  W.IsCritical,  UP.Title TaskOwner,  PT.Title AS ProjectTeam,  S.Title AS [Status] FROM      Workplan W LEFT JOIN      ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN      TeamTypes TT ON PT.TeamTypeId = TT.ID LEFT JOIN   UserProfiles UP ON W.TaskOwnerId = UP.ID LEFT JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID WHERE      TT.[Key] = 'PROJECT_MANAGEMENT'     AND DATEPART(ww, W.TaskDueDate) = DATEPART(ww, GETDATE()) + 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 21,
                column: "AnswerSQL",
                value: "SELECT  SF.Title , F.Title AS [Function], SF.[Key] FROM  Subfunctions SF  JOIN  Functions F ON SF.FunctionID = F.ID  WHERE  F.Title = {ProjectTeam}");

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
                keyValue: 24,
                column: "AnswerSQL",
                value: "SELECT I.Title, R.Title ReceiverTeam, P.Title ProviderTeam, UR.Title ReceiverOwner, UP.Title ProviderOwner, I.ItemDueDate FROM Interdependencies I  LEFT JOIN InterdependencyStatuses S on I.InterdependencyStatusId = S.ID   LEFT JOIN ProjectTeams R ON I.ReceiverProjectTeamID = R.ID LEFT JOIN ProjectTeams P ON I.ProviderProjectTeamId = P.ID LEFT JOIN UserProfiles UR ON I.ReceiverOwnerId = UR.ID LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE CAST(I.ItemDueDate AS DATE) < CAST(GETDATE() AS DATE)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 26,
                column: "AnswerSQL",
                value: "SELECT      RI.Title AS RisksWithNoMitigation,  RI.IsCritical,  RI.IssueRiskCategory,  RI.ItemDescription,  RI.ItemDueDate,  RI.ItemPriority,  RI.RiskImpact,  RI.RiskProbability,  TT.TITLE AS TeamType,  UP.Title TaskOwner,  PT.Title AS ProjectTeam,  S.Title AS [Status]  FROM      RisksandIssues RI LEFT JOIN      ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN      TeamTypes TT ON PT.TeamTypeId = TT.ID LEFT JOIN  Statuses S ON S.ID = RI.ItemStatusId LEFT JOIN  UserProfiles UP ON UP.ID = RI.ItemOwnerId WHERE      RI.RiskMitigation IS NULL     AND RI.IssueRiskCategory = 'Risk'     AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 27,
                column: "AnswerSQL",
                value: "SELECT      RI.Title AS RisksWithNoOwner, RI.IsCritical, RI.IssueRiskCategory, RI.ItemDescription, RI.ItemDueDate, RI.ItemPriority, RI.RiskImpact, RI.RiskProbability, TT.TITLE AS TeamType, UP.Title TaskOwner, PT.Title AS ProjectTeam, S.Title AS [Status], RI.RiskMitigation FROM      RisksandIssues RI LEFT JOIN      ProjectTeams PT     ON RI.ProjectTeamId = PT.ID LEFT JOIN      TeamTypes TT     ON PT.TeamTypeId = TT.ID LEFT JOIN Statuses S ON S.ID = RI.ItemStatusId LEFT JOIN UserProfiles UP ON UP.ID = RI.ItemOwnerId WHERE      RI.ItemOwnerID IS NULL     AND RI.IssueRiskCategory = 'Risk'     AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 28,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS Milestones, W.TaskDueDate, W.ActualStartDate, W.WorkPlanTaskType, W.TaskDescription, W.[Priority], W.IsCritical, UP.Title TaskOwner, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      Workplan W LEFT JOIN      ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN      TeamTypes TT ON PT.TeamTypeId = TT.ID LEFT JOIN  UserProfiles UP ON W.TaskOwnerId = UP.ID LEFT JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID WHERE      W.WorkplanTaskType = 'Milestone'     AND CAST(W.StartDate AS DATE) > CAST(GETDATE() AS DATE)     AND TT.[Key] = 'PROJECT_MANAGEMENT'     AND EXISTS (         SELECT 1          FROM WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WRIARI          WHERE WRIARI.WorkPlanId = W.ID     )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 29,
                column: "AnswerSQL",
                value: "SELECT  PT.Title AS Team , UP.Title AS [Owner], TT.Title AS TeamType, PT.[Key] FROM  ProjectTeams PT  JOIN  TeamTypes TT ON PT.TeamTypeID = TT.ID  LEFT JOIN  UserProfiles UP ON PT.ItemOwnerId = UP.ID WHERE  TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 30,
                column: "AnswerSQL",
                value: "SELECT  Child.Title AS Teams, UP.Title AS [Owner], TT.Title AS TeamType, Child.[Key] FROM  ProjectTeams Parent  JOIN  ProjectTeams Child ON Child.ParentProjectTeamId = Parent.ID  JOIN  TeamTypes TT ON Child.TeamTypeID = TT.ID  LEFT JOIN  UserProfiles UP ON Child.ItemOwnerId = UP.ID WHERE  Parent.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 31,
                column: "AnswerSQL",
                value: "SELECT  Parent.Title AS ParentTeam, UP.Title AS [Owner], TT.Title AS TeamType, Parent.[Key] FROM  ProjectTeams Parent  JOIN  ProjectTeams Child ON Child.ParentProjectTeamId = Parent.ID  JOIN  TeamTypes TT ON Parent.TeamTypeId = TT.ID  LEFT JOIN  UserProfiles UP ON Parent.ItemOwnerId = UP.ID WHERE  TT.[Key] = 'PROJECT_MANAGEMENT'  AND Child.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "AnswerSQL",
                value: "SELECT      AN.Title AS Accomplishment, AN.AccomplishmentNextstepCategory, WS.Title, PT.Title AS ProjectTeams, RP.PeriodStartDate, RP.PeriodEndDate, RP.Title ReportingPeriod, PSE.ID AS ProjectStatusEntriesID FROM      ProjectStatusEntries PSE JOIN      ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN      UserProfiles UP ON PT.ItemOwnerID = UP.ID JOIN      AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN      ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID JOIN      WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID WHERE      UP.EMail = '{Username}'     AND RP.ID = (         SELECT RPID.ID - 1         FROM ReportingPeriods RPID         WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate     )     AND AN.AccomplishmentNextstepCategory = 'Accomplishment'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 33,
                column: "AnswerSQL",
                value: "SELECT      AN.Title AS NextSteps,     AN.AccomplishmentNextstepCategory, WS.Title, PT.Title AS ProjectTeams, RP.PeriodStartDate, RP.PeriodEndDate, RP.Title ReportingPeriod, PSE.ID AS ProjectStatusEntriesID FROM      ProjectStatusEntries PSE JOIN      ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN      AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN      ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID JOIN      WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID WHERE      PT.Title = {ProjectTeam}     AND RP.ID = (         SELECT RPID.ID         FROM ReportingPeriods RPID         WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate     )     AND AN.AccomplishmentNextstepCategory = 'Next Step'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                column: "AnswerSQL",
                value: "SELECT I.Title, R.Title ReceiverTeam, P.Title ProviderTeam, UR.Title ReceiverOwner, UP.Title ProviderOwner, I.ItemDueDate FROM Interdependencies I LEFT JOIN InterdependencyStatuses S on I.InterdependencyStatusId = S.ID   LEFT JOIN ProjectTeams R ON I.ReceiverProjectTeamID = R.ID LEFT JOIN ProjectTeams P ON I.ProviderProjectTeamId = P.ID LEFT JOIN UserProfiles UR ON I.ReceiverOwnerId = UR.ID LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.EMail = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 36,
                column: "AnswerSQL",
                value: "SELECT      WP.UniqueItemIdentifier,     WP.Title, WP.TaskDueDate, WP.ActualStartDate, WP.WorkPlanTaskType, WP.TaskDescription, WP.[Priority], WP.IsCritical, UP.Title TaskOwner, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      WORKPLAN WP JOIN      ProjectTeams PT ON WP.ProjectTeamId = PT.ID JOIN      Statuses S ON WP.WorkPlanTaskStatusId = S.ID JOIN      TeamTypes TT ON PT.TeamTypeID = TT.ID LEFT JOIN  UserProfiles UP ON WP.TaskOwnerId = UP.ID  WHERE      TaskOwnerId IS NULL     AND WorkPlanTaskType = 'Task'     AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "AnswerSQL",
                value: "SELECT      W.Title,     'Workplan' AS [Category],     UP.Title AS UserName, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      Workplan W LEFT JOIN      UserProfiles UP ON W.TaskOwnerID = UP.ID JOIN      ProjectTeams PT ON W.ProjectTeamId = PT.ID JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID  WHERE      UP.Title = (SELECT TOP 1 Title FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))  UNION ALL  SELECT      RI.Title,     IssueRiskCategory AS [Category],     UP.Title AS UserName, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      RisksAndIssues RI LEFT JOIN      UserProfiles UP ON RI.ItemOwnerId = UP.ID JOIN      ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN      Statuses S ON RI.ItemStatusId = S.ID WHERE      UP.Title = (SELECT TOP 1 Title FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))  UNION ALL  SELECT      A.Title,     'Action' AS [Category],     UP.Title AS UserName, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      Actions A LEFT JOIN      UserProfiles UP ON A.ItemOwnerId = UP.ID JOIN      ProjectTeams PT ON A.ProjectTeamId = PT.ID JOIN      Statuses S ON A.ItemStatusId = S.ID WHERE      UP.Title = (SELECT TOP 1 Title FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))  UNION ALL  SELECT      D.Title,     'Decision' AS [Category],     UP.Title AS UserName, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      Decisions D LEFT JOIN      UserProfiles UP ON D.ItemOwnerId = UP.ID JOIN      ProjectTeams PT ON D.ProjectTeamId = PT.ID JOIN      Statuses S ON D.ItemStatusId = S.ID WHERE      UP.Title = (SELECT TOP 1 Title FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))  UNION ALL  SELECT      I.Title,     'Interdependecy as Provider' AS [Category],     UP.Title AS UserName, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      Interdependencies I LEFT JOIN      UserProfiles UP ON I.ProviderOwnerId = UP.ID JOIN      ProjectTeams PT ON I.ProviderProjectTeamId = PT.ID JOIN      Statuses S ON I.InterdependencyStatusId = S.ID WHERE      UP.Title = (SELECT TOP 1 Title FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))  UNION ALL  SELECT      I.Title,     'Interdependecy as Receiver' AS [Category],     UP.Title AS UserName, PT.Title AS ProjectTeam, S.Title AS [Status] FROM      Interdependencies I LEFT JOIN      UserProfiles UP ON I.ReceiverOwnerId = UP.ID JOIN      ProjectTeams PT ON I.ReceiverProjectTeamId = PT.ID JOIN      Statuses S ON I.InterdependencyStatusId = S.ID WHERE      UP.Title = (SELECT TOP 1 Title FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team] , WS.Title WeeklyStatus,  PSE.ExecutiveSummary FROM ProjectStatusEntries PSE   LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID   LEFT JOIN WeeklyStatusStatuses WS ON WS.ID = PSE.WeeklyStatusStatusId   WHERE  RP.ID = (SELECT RPID.ID       FROM ReportingPeriods RPID      WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate)      AND WS.[KEY] IN ('AT_RISK','BEHIND_SCHEDULE')      AND TT.[Key]='PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                column: "AnswerSQL",
                value: "SELECT RI.UniqueItemIdentifier, RI.Title,    'Risks' as Category , PT.Title ProjectTeam, U.Title ItemOwner FROM    RisksAndIssues RI    LEFT JOIN Statuses S ON RI.ItemStatusId = S.ID   LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID  LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  LEFT JOIN UserProfiles U ON RI.ItemOwnerId = U.ID WHERE    S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD' )      AND RI.IssueRiskCategory = 'Risk'     AND TT.[Key] = 'PROJECT_MANAGEMENT'   UNION   SELECT     RI.UniqueItemIdentifier,     RI.Title,    'Issues' as Category  , PT.Title ProjectTeam, U.Title ItemOwner FROM    RisksAndIssues RI   LEFT JOIN Statuses S ON RI.ItemStatusId = S.ID  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  LEFT JOIN UserProfiles U ON RI.ItemOwnerId = U.ID WHERE    S.[KEY] NOT IN (   'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD'   )     AND RI.IssueRiskCategory = 'Issue'     AND TT.[Key] = 'PROJECT_MANAGEMENT'  UNION   SELECT     A.UniqueItemIdentifier,     A.Title,    'Actions' as Category , PT.Title ProjectTeam, U.Title ItemOwner FROM    Actions A    JOIN Statuses S ON A.ItemStatusId = S.ID    JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  LEFT JOIN UserProfiles U ON A.ItemOwnerId = U.ID WHERE    S.[KEY] NOT IN (  'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD' )     AND TT.[Key] = 'PROJECT_MANAGEMENT'  UNION   SELECT     D.UniqueItemIdentifier,     D.Title,    'Decisions' as Category  ,PT.Title ProjectTeam, U.Title ItemOwner FROM    Decisions D    LEFT JOIN Statuses S ON D.ItemStatusId = S.ID      JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  LEFT JOIN UserProfiles U ON D.ItemOwnerId = U.ID WHERE    S.[KEY] NOT IN (   'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD'  )    AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as OpenInterdependencies, S.Title [Status], R.Title ReceiverTeam, P.Title ProviderTeam, UR.Title ReceiverOwner, UP.Title ProviderOwner, I.ItemDueDate FROM Interdependencies I  LEFT JOIN InterdependencyStatuses S on I.InterdependencyStatusId = S.ID LEFT JOIN ProjectTeams R ON I.ReceiverProjectTeamID = R.ID LEFT JOIN ProjectTeams P ON I.ProviderProjectTeamId = P.ID LEFT JOIN UserProfiles UR ON I.ReceiverOwnerId = UR.ID LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "AnswerSQL",
                value: "SELECT W.UniqueItemIdentifier, W.Title CriticalTasks, PT.Title WorkStream, S.Title [Status] , W.TaskDueDate, W.StartDate, W.IsCritical, UP.Title TaskOwner  FROM WorkPlan W   LEFT JOIN Statuses S on wp.WorkPlanTaskStatusId = S.ID   LEFT JOIN ProjectTeams PT on PT.ID = W.WorkstreamId LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId  LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE W.IsCritical = 1  AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as ITIntProvider, S.Title [Status], RT.Title ReceiverTeam, PT.Title ProviderTeam, UR.Title ReceiverOwner, UP.Title ProviderOwner, I.ItemDueDate FROM Interdependencies I  LEFT JOIN InterdependencyStatuses S on I.InterdependencyStatusId = S.ID LEFT JOIN ProjectTeams RT ON I.ReceiverProjectTeamID = RT.ID LEFT JOIN ProjectTeams PT ON I.ProviderProjectTeamId = PT.ID LEFT JOIN UserProfiles UR ON I.ReceiverOwnerId = UR.ID LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID  WHERE PT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as as ITIntReceiver, S.Title [Status], RT.Title ReceiverTeam, PT.Title ProviderTeam, UR.Title ReceiverOwner, UP.Title ProviderOwner, I.ItemDueDate FROM Interdependencies I  LEFT JOIN InterdependencyStatuses S on I.InterdependencyStatusId = S.ID LEFT JOIN ProjectTeams RT ON I.ReceiverProjectTeamID = RT.ID LEFT JOIN ProjectTeams PT ON I.ProviderProjectTeamId = PT.ID LEFT JOIN UserProfiles UR ON I.ReceiverOwnerId = UR.ID LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID  WHERE RT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                column: "AnswerSQL",
                value: "SELECT W.UniqueItemIdentifier, W.Title TasksWithoutOwner, PT.Title WorkStream, S.Title [Status] , W.TaskDueDate, W.StartDate, W.IsCritical FROM WorkPlan W   LEFT JOIN Statuses S on wp.WorkPlanTaskStatusId = S.ID   LEFT JOIN ProjectTeams PT on PT.ID = W.WorkstreamId LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId  WHERE W.TaskOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 54,
                column: "AnswerSQL",
                value: "SELECT  RI.UniqueItemIdentifier,RI.Title ItemsWithoutOwner, IssueRiskCategory AS Category , PT.Title ProjectTeam, ItemPriority FROM  RisksAndIssues RI  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE  RI.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'   UNION    SELECT  A.UniqueItemIdentifier,A.Title  ItemsWithoutOwner, 'Actions' AS Category , PT.Title ProjectTeam, ItemPriority FROM  Actions A LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  A.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'    UNION    SELECT  D.UniqueItemIdentifier,D.Title  ItemsWithoutOwner, 'Decisions' AS Category , PT.Title ProjectTeam, ItemPriority FROM  Decisions D LEFT JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  D.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 55,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks , P.Title ProviderTeam, R.Title ReceiverTeam FROM Interdependencies I  LEFT JOIN WorkPlansToInterdependenciesForProviderTasks WTP ON WTP.InterdependencyId = I.ID LEFT JOIN ProjectTeams P ON I.ProviderProjectTeamID = P.ID LEFT JOIN ProjectTeams R ON I.ReceiverProjectTeamId = R.ID WHERE WTP.ID IS NULL    UNION    SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks , P.Title ProviderTeam, R.Title ReceiverTeam FROM Interdependencies I  LEFT JOIN WorkPlansToInterdependenciesForReceiverTasks WTR ON WTR.InterdependencyId = I.ID  LEFT JOIN ProjectTeams P ON I.ProviderProjectTeamID = P.ID LEFT JOIN ProjectTeams R ON I.ReceiverProjectTeamId = R.ID WHERE WTR.ID IS NULL;");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 56,
                column: "AnswerSQL",
                value: "SELECT DISTINCT UP.Title AS UserName, UP.Email   FROM WorkPlan W  JOIN UserProfiles UP ON UP.ID = W.TaskOwnerId  JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId  JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE S.[Key] IN ('AT_RISK', 'BEHIND_SCHEDULE')      AND UP.Title IS NOT NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 57,
                column: "AnswerSQL",
                value: "SELECT RI.Title RisksAndIssues, IssueRiskCategory , PT.Title ProjectTeam, IsCritical, ItemPriority, ItemDescription, RiskMitigation, RiskImpact   FROM RisksAndIssues RI  LEFT JOIN Statuses S ON S.ID = RI.ItemStatusId   LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  WHERE IsCritical IN (1) AND S.[Key] NOT IN ('CLOSED') AND TT.[Key]='PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS [List of Critical Milestones due in next 15 days with no owners Assigned yet],      PT.Title ProjectTeam,      W.TaskDueDate DueDate,      W.StartDate StartDate,      UP.Title AS TaskOwner,      W.IsCritical IsCritical FROM      WorkPlan W JOIN      ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN      TeamTypes TT ON PT.TeamTypeId = TT.ID LEFT JOIN      UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE      W.WorkPlanTaskType = 'Milestone'     AND CAST(W.TaskDueDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 15, CAST(GETDATE() AS DATE))     AND W.IsCritical = 1     AND W.TaskOwnerId IS NULL     AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 60,
                column: "AnswerSQL",
                value: "SELECT W.Title as [List down all tasks associated with interdependencies] , W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title TaskStatus, W.IsCritical from Workplan W  LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID where W.ID in  ( SELECT WTIR.WorkPlanId  FROM WorkPlansToInterdependenciesForReceiverTasks WTIR  UNION  select WTIP.WorkPlanId  FROM WorkPlansToInterdependenciesForProviderTasks WTIP) AND W.WorkPlanTaskType='Task'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 61,
                column: "AnswerSQL",
                value: "SELECT W.Title as WorkplanAssociatedWithInterdependencies , W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title TaskStatus, W.IsCritical from Workplan W  INNER JOIN WorkPlansToInterdependenciesForReceiverTasks WPRT ON W.id = WPRT.WorkPlanId  LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN Interdependencies IR ON WPRT.InterdependencyId = IR.ID  WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task'   UNION   SELECT W.Title  , W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title TaskStatus, W.IsCritical FROM WorkPlan W  INNER JOIN WorkPlansToInterdependenciesForProviderTasks WPPT ON W.id = WPPT.WorkPlanId  LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN Interdependencies IR ON WPPT.InterdependencyId = IR.ID  WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                column: "AnswerSQL",
                value: "SELECT  W.UniqueItemIdentifier,     W.Title WorkPlanItem, W.TaskDueDate, W.ActualStartDate, W.WorkPlanTaskType, W.TaskDescription, W.[Priority], W.IsCritical, UP.Title TaskOwner, PT.Title AS ProjectTeam, S.Title AS [Status] FROM  Workplan W  LEFT JOIN  ProjectTeams PT ON W.ProjectTeamId = PT.ID  LEFT JOIN  TeamTypes TT ON PT.TeamTypeId = TT.ID  JOIN      Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN  UserProfiles UP ON W.TaskOwnerId = UP.ID  WHERE  TT.[Key] = 'PROJECT_MANAGEMENT'  AND DATEPART(ww,W.TaskDueDate) = DATEPART(ww,GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                column: "AnswerSQL",
                value: "SELECT  W.Title,  W.WorkPlanTaskType,  W.StartDate,  W.TaskDueDate,  W.UniqueItemIdentifier, W.ActualStartDate, W.TaskDescription, W.[Priority], W.IsCritical, PT.Title ProjectTeam,  S.Title [Status],  UP.Title TaskOwner , TT.Title AS TeamType FROM  Workplan W  LEFT JOIN  Statuses S ON W.WorkPlanTaskStatusId = S.ID  LEFT JOIN  ProjectTeams PT ON W.ProjectTeamId = PT.ID  LEFT JOIN  UserProfiles UP ON W.TaskOwnerId = UP.ID  LEFT JOIN  TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE  W.UniqueItemIdentifier = 'HR.2.2.6'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                column: "AnswerSQL",
                value: "SELECT  RI.UniqueItemIdentifier, RI.Title, RI.IssueRiskCategory, PT.Title ProjectTeam, RI.ItemDueDate ,S.Title ItemStatus   FROM RisksAndIssues RI  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN Statuses S ON RI.ItemStatusId  = S.ID WHERE   RI.ItemOwnerId IS NULL     UNION ALL    SELECT   D.UniqueItemIdentifier, D.Title  ,'Decision'  , PT.Title ProjectTeam, D.ItemDueDate ,S.Title ItemStatus   FROM Decisions D LEFT JOIN ProjectTeams PT ON D.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN Statuses S ON D.ItemStatusId  = S.ID WHERE   D.ItemOwnerId IS NULL     UNION ALL     SELECT A.UniqueItemIdentifier, A.Title ,'Action',  PT.Title ProjectTeam, A.ItemDueDate ,S.Title ItemStatus   FROM Actions A LEFT JOIN ProjectTeams PT ON A.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN Statuses S ON A.ItemStatusId  = S.ID WHERE  A.ItemOwnerId IS NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                column: "AnswerSQL",
                value: "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  , W.TaskDueDate, W.StartDate, PT.Title ProjectTeam, W.IsCritical, S.Title TaskStatus FROM    Workplan W     LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId    LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId    LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId  WHERE S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')    AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)     AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                column: "AnswerSQL",
                value: "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  , W.TaskDueDate, W.StartDate, PT.Title ProjectTeam, W.IsCritical, S.Title TaskStatus FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId    LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId   LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)    AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                column: "AnswerSQL",
                value: "SELECT DISTINCT PT.Title as ProjectTeams_With_Items_At_Risk, U.Title TeamOwner FROM ProjectTeams PT     JOIN Workplan W ON W.ProjectTeamID = PT.ID     JOIN Statuses S ON S.ID = W.WorkplanTaskStatusID     JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  LEFT JOIN UserProfiles U ON PT.ItemOwnerId = U.ID WHERE S.[Key] = 'AT_RISK'   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                column: "AnswerSQL",
                value: "SELECT W.Title Workplan , W.TaskDueDate, W.StartDate, S.Title TaskStatus, PT.Title ProjectTeam , U.Title TaskOwner FROM Workplan W  JOIN ( SELECT   DISTINCT W.Title, WorkPlanLinksTargetId AS TaskID         FROM WorkPlanLinks WL           LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksTargetId    LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID     LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId where TT.[Key]='PROJECT_MANAGEMENT'        UNION            SELECT    DISTINCT W.Title, WorkPlanLinksSourceId AS TaskID        FROM WorkPlanLinks WL            LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksSourceId    LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID     LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId where TT.[Key]='PROJECT_MANAGEMENT' )  SUB  ON W.ID = TaskID LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN UserProfiles U ON W.TaskOwnerId = U.ID ORDER BY TaskID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                column: "AnswerSQL",
                value: "SELECT W.Title AS [List of Milestones due in next 7 days and Not Updated Since Last 2 weeks] , W.TaskDueDate, W.StartDate, S.Title TaskStatus, PT.Title ProjectTeam , U.Title TaskOwner FROM      WorkPlan W  JOIN Statuses S on s.ID = W.WorkPlanTaskStatusId  JOIN ProjectTeams PT  ON PT.ID = W.ProjectTeamId  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  JOIN Userprofiles U ON W.TaskOwnerId = U.ID WHERE      W.WorkPlanTaskType IN ('Milestone')     AND W.IsCritical IN (1)      AND CAST(W.Modified AS DATE) < DATEADD(day, -14, CAST(GETDATE() AS DATE))  AND S.[Key] not in ('COMPLETED', 'CANCELLED') AND TT.[Key] =  'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                column: "AnswerSQL",
                value: "SELECT RI.TITLE AS [Risk items that does not have Risk Mitigation plan], , RI.ItemDescription, RI.ItemDueDate, RI.RiskImpact, RI.RiskMitigation, RI.RiskProbability, PT.Title ProjectTeam, UP.Title ItemOwner  FROM RisksAndIssues RI  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID WHERE RI.IssueRiskCategory = 'Risk' AND RI.RiskMitigation IS NULL AND TT.[Key]='PROJECT_MANAGEMENT'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 4,
                column: "AnswerSQL",
                value: "SELECT wp.Title  from WorkPlan wp   LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  left join ProjectTeams pt on pt.ID = wp.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone' and TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 5,
                column: "AnswerSQL",
                value: "SELECT RI.Title  from RisksAndIssues RI   LEFT JOIN Statuses s on RI.ItemStatusId = s.ID   LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk' and TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 6,
                column: "AnswerSQL",
                value: "SELECT I.Title  from Interdependencies I  LEFT JOIN InterdependencyStatuses s on I.InterdependencyStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT W.Title, UP.Title as UserName AS WorkPlanItem FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, '{Username}') AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                column: "AnswerSQL",
                value: "SELECT TOP 1 CAST(RP.PeriodEndDate AS DATE) AS WeeklyStatusDueDate  FROM ReportingPeriods RP WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(RP.PeriodStartDate AS DATE) AND CAST(RP.PeriodEndDate AS DATE) ORDER BY RP.Modified DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 18,
                column: "AnswerSQL",
                value: "SELECT UP.Title AS FunctionalOwner FROM ProjectTeams PT JOIN Userprofiles UP ON PT.ItemOwnerID = UP.ID WHERE PT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 19,
                column: "AnswerSQL",
                value: "SELECT WP.Title FROM Workplan WP JOIN ProjectTeams PT ON WP.ProjectTeamId = PT.ID JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.ID JOIN TeamTypes TT ON PT.TeamTypeID = TT.ID WHERE S.[Key] = 'BEHIND_SCHEDULE' AND TT.[Key] = 'PROJECT_MANAGEMENT' AND PT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "AnswerSQL",
                value: "SELECT W.Title AS WorkPlanItem FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT' AND DATEPART(ww,W.TaskDueDate) = DATEPART(ww,GETDATE()) + 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 21,
                column: "AnswerSQL",
                value: "SELECT SF.Title FROM Subfunctions SF JOIN Functions F ON SF.FunctionID = F.ID WHERE F.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 22,
                column: "AnswerSQL",
                value: "SELECT WS.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT RPID.ID -1 FROM ReportingPeriods RPID WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 23,
                column: "AnswerSQL",
                value: "SELECT AN.Title, AN.AccomplishmentNextstepCategory FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT RPID.ID -1 FROM ReportingPeriods RPID WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate) ORDER BY AN.AccomplishmentNextstepCategory");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 24,
                column: "AnswerSQL",
                value: "SELECT I.Title FROM Interdependencies I WHERE CAST(I.ItemDueDate AS DATE) < CAST(GETDATE() AS DATE)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 26,
                column: "AnswerSQL",
                value: "SELECT RI.Title AS RisksWithNoMitigation FROM RisksandIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE RI.RiskMitigation IS NULL AND RI.IssueRiskCategory = 'Risk' AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 27,
                column: "AnswerSQL",
                value: "SELECT RI.Title AS RisksWithNoMitigation FROM RisksandIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE RI.ItemOwnerID IS NULL AND RI.IssueRiskCategory = 'Risk' AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 28,
                column: "AnswerSQL",
                value: "SELECT W.Title As Milestones FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE W.WorkplanTaskType = 'Milestone'  AND CAST(W.StartDate AS DATE) > CAST(GETDATE() AS DATE) AND TT.[Key] = 'PROJECT_MANAGEMENT' AND EXISTS (SELECT 1 FROM WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WRIARI WHERE WRIARI.WorkPlanId = W.ID)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 29,
                column: "AnswerSQL",
                value: "SELECT PT.Title AS Team FROM ProjectTeams PT JOIN TeamTypes TT ON PT.TeamTypeID = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 30,
                column: "AnswerSQL",
                value: "SELECT Child.Title AS Teams FROM ProjectTeams Parent JOIN ProjectTeams Child ON Child.ParentProjectTeamId = Parent.ID WHERE Parent.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 31,
                column: "AnswerSQL",
                value: "SELECT Parent.Title AS ParentTeam FROM ProjectTeams Parent JOIN ProjectTeams Child ON Child.ParentProjectTeamId = Parent.ID JOIN TeamTypes TT ON Parent.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT' AND Child.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "AnswerSQL",
                value: "SELECT AN.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN UserProfiles UP ON PT.ItemOwnerID = UP.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE UP.EMail = '{Username}' AND RP.ID = (SELECT RPID.ID -1 FROM ReportingPeriods RPID WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Accomplishment'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 33,
                column: "AnswerSQL",
                value: "SELECT AN.Title AS NextSteps FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT RPID.ID FROM ReportingPeriods RPID WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Next Step'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                column: "AnswerSQL",
                value: "SELECT I.Title FROM Interdependencies I JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.EMail = '{Username}' ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 36,
                column: "AnswerSQL",
                value: "SELECT  WP.UniqueItemIdentifier ,WP.Title FROM WORKPLAN WP JOIN ProjectTeams PT ON WP.ProjectTeamId = PT.ID JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.ID JOIN TeamTypes TT ON PT.TeamTypeID = TT.ID WHERE  TaskOwnerId IS NULL AND WorkPlanTaskType = 'Task' AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "AnswerSQL",
                value: "SELECT  W.Title,'Workplan' [Category],UP.Title as UserName FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID WHERE UP.Title = = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))  UNION ALL   SELECT RI.Title,IssueRiskCategory [Category],UP.Title as UserName FROM RisksAndIssues RI LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, '{Username}')) UNION ALL SELECT A.Title,'Action' [Category] FROM Actions A LEFT JOIN UserProfiles UP ON A.ItemOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title   FROM UserProfiles WHERE FREETEXT(Title, '{Username}')) UNION ALL  SELECT D.Title,'Decision' [Category],UP.Title as UserName FROM Decisions D LEFT JOIN UserProfiles UP ON D.ItemOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, '{Username}')) UNION ALL   SELECT I.Title,'Interdependecy as Provider' [Category],UP.Title as UserName FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, '{Username}')) UNION ALL  SELECT I.Title,'Interdependecy as Receiver' [Category],UP.Title as UserName FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ReceiverOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title   FROM UserProfiles WHERE FREETEXT(Title, '{Username}'))");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team]  FROM ProjectStatusEntries PSE  LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID  LEFT JOIN WeeklyStatusStatuses WS ON WS.ID = PSE.WeeklyStatusStatusId  WHERE  RP.ID = (SELECT RPID.ID  FROM ReportingPeriods RPID WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate) AND WS.[KEY] IN ('AT_RISK','BEHIND_SCHEDULE') AND TT.[Key]='PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                column: "AnswerSQL",
                value: "SELECT     RI.UniqueItemIdentifier,     RI.Title,    'Risks' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD' )     AND RI.IssueRiskCategory = 'Risk'     AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT     RI.UniqueItemIdentifier,     RI.Title,    'Issues' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (   'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD'   )     AND RI.IssueRiskCategory = 'Issue'     AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT     A.UniqueItemIdentifier,     A.Title,    'Actions' as Category  FROM    Actions A    JOIN Statuses S ON A.ItemStatusId = S.ID    JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (  'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD' )     AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT     D.UniqueItemIdentifier,     D.Title,    'Decisions' as Category  FROM    Decisions D    LEFT JOIN Statuses S ON D.ItemStatusId = S.ID      JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (   'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD'  )    AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as OpenInterdependencies, S.Title as Status FROM Interdependencies I LEFT JOIN InterdependencyStatuses S ON I.InterdependencyStatusId = S.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "AnswerSQL",
                value: "SELECT    W.UniqueItemIdentifier,    W.TITLE as CriticalTasks  FROM    Workplan W      JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    W.IsCritical = 1   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as ITIntProvider FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ProviderProjectTeamID WHERE PT.Title = {ProjectTeam} ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as ITIntReceiver FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ReceiverProjectTeamId WHERE PT.Title = {ProjectTeam} ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                column: "AnswerSQL",
                value: "SELECT  W.UniqueItemIdentifier,W.Title TasksWithoutOwner FROM  WorkPlan W JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  W.TaskOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 54,
                column: "AnswerSQL",
                value: "SELECT  RI.UniqueItemIdentifier,RI.Title ItemsWithoutOwner, IssueRiskCategory AS Category FROM  RisksAndIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE  RI.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT  A.UniqueItemIdentifier,A.Title  ItemsWithoutOwner, 'Actions' AS Category FROM  Actions A LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  A.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'  UNION  SELECT  D.UniqueItemIdentifier,D.Title  ItemsWithoutOwner, 'Decisions' AS Category FROM  Decisions D LEFT JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  D.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 55,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForProviderTasks WTP ON WTP.InterdependencyId = I.ID WHERE WTP.ID IS NULL  UNION  SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForReceiverTasks WTR ON WTR.InterdependencyId = I.ID WHERE WTR.ID IS NULL;");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 56,
                column: "AnswerSQL",
                value: "SELECT DISTINCT      UP.Title AS UserName  FROM      WorkPlan W JOIN UserProfiles UP ON UP.ID = W.TaskOwnerId JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      S.[Key] IN ('AT_RISK', 'BEHIND_SCHEDULE')     AND UP.Title IS NOT NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 57,
                column: "AnswerSQL",
                value: "SELECT RI.Title RisksAndIssues, IssueRiskCategory  FROM RisksAndIssues RI  LEFT JOIN Statuses S ON S.ID = RI.ItemStatusId  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE IsCritical IN (1) AND S.[Key] NOT IN ('CLOSED') AND TT.[Key]='PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS [List of Critical Milestones due in next 15 days with no owners Assigned yet] FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      W.WorkPlanTaskType IN ('Milestone')     AND CAST(W.TaskDueDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 15, CAST(GETDATE() AS DATE))     AND W.IsCritical IN (1)     AND W.TaskOwnerId IS NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 60,
                column: "AnswerSQL",
                value: "SELECT W.Title as [List down all tasks associated with interdependencies]  from Workplan W where W.ID in  ( SELECT WTIR.WorkPlanId  FROM WorkPlansToInterdependenciesForReceiverTasks WTIR  UNION  select WTIP.WorkPlanId  FROM WorkPlansToInterdependenciesForProviderTasks WTIP) AND W.WorkPlanTaskType='Task'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 61,
                column: "AnswerSQL",
                value: "SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForReceiverTasks WPRT ON W.id = WPRT.WorkPlanId LEFT JOIN Interdependencies IR ON WPRT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task' UNION SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForProviderTasks WPPT ON W.id = WPPT.WorkPlanId LEFT JOIN Interdependencies IR ON WPPT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                column: "AnswerSQL",
                value: "SELECT W.Title AS WorkPlanItem FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT'  AND DATEPART(ww,W.TaskDueDate) = DATEPART(ww,GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                column: "AnswerSQL",
                value: "SELECT W.Title, W.WorkPlanTaskType, W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title [Status], UP.Title TaskOwner FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE W.UniqueItemIdentifier = 'HR.2.2.6'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                column: "AnswerSQL",
                value: "SELECT  RI.UniqueItemIdentifier, RI.Title, RI.IssueRiskCategory  FROM RisksAndIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE   RI.ItemOwnerId IS NULL   UNION ALL  SELECT   D.UniqueItemIdentifier, D.Title  ,'Decision'  FROM Decisions D LEFT JOIN ProjectTeams PT ON D.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE   D.ItemOwnerId IS NULL   UNION ALL   SELECT A.UniqueItemIdentifier, A.Title ,'Action'  FROM Actions A LEFT JOIN ProjectTeams PT ON A.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE  A.ItemOwnerId IS NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                column: "AnswerSQL",
                value: "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId   LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId   LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)    AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                column: "AnswerSQL",
                value: "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId    LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId   LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)    AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                column: "AnswerSQL",
                value: "SELECT DISTINCT PT.Title as ProjectTeams_With_Items_At_Risk  FROM ProjectTeams PT    JOIN Workplan W ON W.ProjectTeamID = PT.ID    JOIN Statuses S ON S.ID = W.WorkplanTaskStatusID    JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[Key] = 'AT_RISK'   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                column: "AnswerSQL",
                value: "SELECT Title Workplan  FROM ( SELECT   DISTINCT W.Title, WorkPlanLinksTargetId AS TaskID      FROM WorkPlanLinks WL        LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksTargetId LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId where TT.[Key]='PROJECT_MANAGEMENT'  UNION      SELECT    DISTINCT W.Title, WorkPlanLinksSourceId AS TaskID     FROM          WorkPlanLinks WL         LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksSourceId LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId where TT.[Key]='PROJECT_MANAGEMENT' ) AS SubQuery ORDER BY TaskID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS [List of Milestones due in next 7 days and Not Updated Since Last 2 weeks] FROM      WorkPlan W JOIN Statuses S on s.ID = W.WorkPlanTaskStatusId JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      W.WorkPlanTaskType IN ('Milestone')     AND W.IsCritical IN (1)     AND CAST(W.Modified AS DATE) < DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND S.[Key] not in ('COMPLETED', 'CANCELLED') AND TT.[Key] =  'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                column: "AnswerSQL",
                value: "SELECT     RI.TITLE AS [Risk items that does not have Risk Mitigation plan] FROM     RisksAndIssues RI  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE     RI.IssueRiskCategory = 'Risk'     AND RI.RiskMitigation IS  NULL AND TT.[Key]='PROJECT_MANAGEMENT'");
        }
    }
}
