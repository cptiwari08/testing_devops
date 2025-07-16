using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamTypeInSuggestionQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 369);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 1,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE wp.WorkPlanTaskType = 'Milestone' and TT.[Key] = 'PROJECT_MANAGEMENT' GROUP BY pt.Title ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 2,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(*) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE IssueRiskCategory = 'Risk' and TT.[Key] = 'PROJECT_MANAGEMENT' GROUP BY pt.Title");

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
                keyValue: 10,
                column: "AnswerSQL",
                value: "SELECT     pt.Title AS ProjectTeamTitle, COUNT(*) AS OverdueMilestoneCount FROM   WorkPlan wp INNER JOIN  ProjectTeams pt ON wp.ProjectTeamId = pt.ID  LEFT JOIN  statuses s ON wp.WorkPlanTaskStatusId = s.ID  LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE  wp.WorkPlanTaskType = 'Milestone'  AND wp.TaskDueDate < GETDATE()    AND TT.[Key] = 'PROJECT_MANAGEMENT' AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY   pt.Title ORDER BY      OverdueMilestoneCount DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 11,
                column: "AnswerSQL",
                value: "SELECT  pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM  RisksAndIssues RI  LEFT JOIN ProjectTeams pt ON RI.ProjectTeamId = pt.ID  LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID  WHERE      RI.IssueRiskCategory = 'Risk'      AND RI.ItemDueDate < GETDATE()      AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) AND TT.[Key] = 'PROJECT_MANAGEMENT' GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "AnswerSQL",
                value: "SELECT   RiskImpact,  RiskProbability,  COUNT(*) AS RiskCount  FROM  RisksAndIssues RI LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE IssueRiskCategory = 'Risk'  AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))   AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) AND TT.[Key] = 'PROJECT_MANAGEMENT' GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 37,
                column: "AnswerSQL",
                value: "SELECT  COUNT(1) [HighRiskItemCount]  FROM RisksAndIssues R  LEFT JOIN ProjectTeams PT ON PT.ID = R.ProjectTeamId  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE  RiskImpact = 'High'  AND IssueRiskCategory = 'Risk' AND TT.[Key]='PROJECT_MANAGEMENT' AND PT.Title ={ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team]  FROM ProjectStatusEntries PSE  LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID  LEFT JOIN WeeklyStatusStatuses WS ON WS.ID = PSE.WeeklyStatusStatusId  WHERE  RP.ID = (SELECT ID  FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND WS.[KEY] IN ('AT_RISK','BEHIND_SCHEDULE') AND TT.[Key]='PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(WR.ID) Count,'Risks' as Category FROM Workplan W   JOIN WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WR On WR.WorkPlanId = W.ID   LEFT JOIN RisksAndIssues RI ON RI.ID = WR.RisksAndIssueId   LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  WHERE RI.IssueRiskCategory = 'Risk'  and TT.[Key] = 'PROJECT_MANAGEMENT'    UNION ALL SELECT COUNT(WR.ID) Count, 'Issues' as Category   FROM Workplan W   JOIN WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WR On WR.WorkPlanId = W.ID   LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN RisksAndIssues RI ON RI.ID = WR.RisksAndIssueId  WHERE RI.IssueRiskCategory = 'Issue'  and TT.[Key] = 'PROJECT_MANAGEMENT'    UNION ALL SELECT COUNT(WA.ID) Count, 'Actions' as Category    FROM Workplan W   JOIN WorkPlansToActionsForAssociatedActions WA On WA.WorkPlanId = W.ID    LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  where TT.[Key] = 'PROJECT_MANAGEMENT'    UNION ALL SELECT    COUNT(WD.ID) Count,    'Decisions' as Category    FROM    Workplan W      JOIN WorkPlansToDecisionsForAssociatedDecisions WD On WD.WorkPlanId = W.ID  LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  where TT.[Key] = 'PROJECT_MANAGEMENT'", "How many Risks, Issues, Actions and Decisions linked to Workplan Tasks?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT WSS.Title StatusValue ,'CurrentStatus' as Category FROM ProjectStatusEntries PSE LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT TOP 1 ReportingPeriods.ID FROM ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC) AND PT.Title like {ProjectTeam}  UNION SELECT WSS.Title StatusValue ,'PreviousStatus' as Category FROM ProjectStatusEntries PSE  LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT  ReportingPeriods.ID FROM   ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC Offset 1 Rows FETCH NEXT 1 ROWS ONLY) AND  PT.Title like {ProjectTeam}", "What's prior week and current week status for Finance function?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    RI.UniqueItemIdentifier,    RI.Title,    'Risks' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND RI.IssueRiskCategory = 'Risk'    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    RI.UniqueItemIdentifier,    RI.Title,    'Issues' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND RI.IssueRiskCategory = 'Issue'    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    A.UniqueItemIdentifier,    A.Title,    'Actions' as Category  FROM    Actions A    JOIN Statuses S ON A.ItemStatusId = S.ID    JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    D.UniqueItemIdentifier,    D.Title,    'Decisions' as Category  FROM    Decisions D    LEFT JOIN Statuses S ON D.ItemStatusId = S.ID      JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all Open Risks, Issues, Actions and Decisions in PMO app.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as OpenInterdependencies, S.Title as Status FROM Interdependencies I LEFT JOIN InterdependencyStatuses S ON I.InterdependencyStatusId = S.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'Canceled') ", "List all Open Interdependencies." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE as CriticalTasks  FROM    Workplan W      JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    W.IsCritical = 1   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all critical workplan tasks in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as ITIntProvider FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ProviderProjectTeamID WHERE PT.Title = {ProjectTeam} ", "List all Interdependencies where IT is Interdependency Provider.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as ITIntReceiver FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ReceiverProjectTeamId WHERE PT.Title = {ProjectTeam} ", "List all Interdependencies where IT is Interdependency Receiver." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    COUNT(W.ID) as CountofCriticalPath   FROM    Workplan W      JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    W.IsCritical = 1   AND TT.[Key] = 'PROJECT_MANAGEMENT'   and W.WorkPlanTaskType = 'Milestone'", "How many critical milestones in workplan in PMO app?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    COUNT(W.ID) AS TasksDueIn30Days  FROM    Workplan W    LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID        JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    CAST(W.TaskDueDate AS DATE) <= CAST(GETDATE() + 30 AS DATE)    AND CAST(W.TaskDueDate AS DATE) >= CAST(GETDATE() AS DATE)    AND S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "How many tasks are due in next 30 days in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  W.UniqueItemIdentifier,W.Title TasksWithoutOwner FROM  WorkPlan W JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  W.TaskOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List any workplan items that do not have an owner assigned in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  RI.UniqueItemIdentifier,RI.Title ItemsWithoutOwner, IssueRiskCategory AS Category FROM  RisksAndIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE  RI.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT  A.UniqueItemIdentifier,A.Title  ItemsWithoutOwner, 'Actions' AS Category FROM  Actions A LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  A.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'  UNION  SELECT  D.UniqueItemIdentifier,D.Title  ItemsWithoutOwner, 'Decisions' AS Category FROM  Decisions D LEFT JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  D.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List any RAID log items that do not have an owner assigned in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForProviderTasks WTP ON WTP.InterdependencyId = I.ID WHERE WTP.ID IS NULL  UNION  SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForReceiverTasks WTR ON WTR.InterdependencyId = I.ID WHERE WTR.ID IS NULL;", "List interdependencies that do not have a provider or receiver task linked." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT      UP.Title AS UserName  FROM      WorkPlan W JOIN UserProfiles UP ON UP.ID = W.TaskOwnerId JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      S.[Key] IN ('AT_RISK', 'BEHIND_SCHEDULE')     AND UP.Title IS NOT NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'", "Show me a list of all users that have behind schedule or at risk items assigned to them in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title RisksAndIssues, IssueRiskCategory  FROM RisksAndIssues RI  LEFT JOIN Statuses S ON S.ID = RI.ItemStatusId  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE IsCritical IN (1) AND S.[Key] NOT IN ('CLOSED') AND TT.[Key]='PROJECT_MANAGEMENT'", "Show me all Risks and issues that are flagged as Critical and not complete." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID)) ROW_NUM FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      CAST(W.Modified AS DATE) BETWEEN DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND PT.ItemIsActive=1 AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY      pt.title) A", "Which functions has made the least # of updates to their workplan in last week in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      W.Title AS [List of Critical Milestones due in next 15 days with no owners Assigned yet] FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      WorkPlanTaskType IN ('Milestone')     AND CAST(TaskDueDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 15, CAST(GETDATE() AS DATE))     AND IsCritical IN (1)     AND TaskOwnerId IS NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'", "Can you please list down all milestones due in next 15 days which are on critical path and have no owners assigned to them in PMO app??" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title as [List down all tasks associated with interdependencies] from Workplan where ID in ( SELECT WorkPlanId FROM WorkPlansToInterdependenciesForReceiverTasks UNION select WorkPlanId FROM WorkPlansToInterdependenciesForProviderTasks) AND WorkPlanTaskType='Task'", "Can you please list down all tasks associated with interdependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForReceiverTasks WPRT ON W.id = WPRT.WorkPlanId LEFT JOIN Interdependencies IR ON WPRT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task' UNION SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForProviderTasks WPPT ON W.id = WPPT.WorkPlanId LEFT JOIN Interdependencies IR ON WPPT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task'", "Can you please list down all tasks whose due date is less than the due date of the associated interdependency?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT W.Title AS WorkPlanItem FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT'  AND DATEPART(ww,TaskDueDate) = DATEPART(ww,GETDATE())", "List out workplan items due this week in PMO app.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT W.Title, W.WorkPlanTaskType, W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title [Status], UP.Title TaskOwner FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE UniqueItemIdentifier = 'HR.2.2.6'", "Provide details for workplan ID HR.2.2.6." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(RI.ID) IssueCoutnwithPendingStatus FROM RisksandIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN RiskAndIssueStatuses RS ON RI.ItemStatusID = RS.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE IssueRiskCategory = 'Issue' AND TT.[Key] = 'PROJECT_MANAGEMENT' AND RS.Title = 'Pending'", "How many issues are in pending status in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  RI.UniqueItemIdentifier, RI.Title, RI.IssueRiskCategory  FROM RisksAndIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE   RI.ItemOwnerId IS NULL   UNION ALL  SELECT   D.UniqueItemIdentifier, D.Title  ,'Decision'  FROM Decisions D LEFT JOIN ProjectTeams PT ON D.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE   D.ItemOwnerId IS NULL   UNION ALL   SELECT A.UniqueItemIdentifier, A.Title ,'Action'  FROM Actions A LEFT JOIN ProjectTeams PT ON A.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE  A.ItemOwnerId IS NULL", "List RAID items that do not have an owner assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId   LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId   LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)    AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all Workplan tasks that are 'Not Started' and planned start date has passed in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId    LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId   LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)    AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all Workplan tasks which are past due in PMO app.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT PT.Title as ProjectTeams_With_Items_At_Risk  FROM ProjectTeams PT    JOIN Workplan W ON W.ProjectTeamID = PT.ID    JOIN Statuses S ON S.ID = W.WorkplanTaskStatusID    JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[Key] = 'AT_RISK'   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all project teams with workplan items that are 'At Risk' in PMO app.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title FROM ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE ManageProjectStatus = 1 AND  TT.[Key] = 'PROJECT_MANAGEMENT'  EXCEPT   SELECT PT.Title  FROM ProjectStatusEntries PSE  LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodId = RP.ID  LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamId = PT.ID  WHERE RP.ID = (SELECT ID FROM ReportingPeriods WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE)      )   AND PSE.WeeklyStatusStatusId IS NOT NULL", "Which teams have not entered thier weekly status report for this reporting period?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  COUNT(R.ID) AS NewRisksCount FROM  RisksAndIssues R JOIN ProjectTeams PT ON R.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  IssueRiskCategory = 'Risk' AND DATEDIFF(day, R.Created, GETDATE()) <= 5 AND TT.[Key] = 'PROJECT_MANAGEMENT'", "How many new risks have been created in the last 5 days in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title Workplan  FROM ( SELECT   DISTINCT W.Title, WorkPlanLinksTargetId AS TaskID      FROM WorkPlanLinks WL        LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksTargetId LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId where TT.[Key]='PROJECT_MANAGEMENT'  UNION      SELECT    DISTINCT W.Title, WorkPlanLinksSourceId AS TaskID     FROM          WorkPlanLinks WL         LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksSourceId LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId where TT.[Key]='PROJECT_MANAGEMENT' ) AS SubQuery ORDER BY TaskID", "Which tasks have a predecessor or successor linked to them?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID) DESC) ROW_NUM FROM      RisksAndIssues RI JOIN ProjectTeams PT ON PT.ID = RI.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE PT.ItemIsActive=1 and RI.IssueRiskCategory = 'Risk' AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY      pt.title) A", "Which function has the highest number of risk associated with in PMO app?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      W.Title AS [List of Milestones due in next 7 days and Not Updated Since Last 2 weeks] FROM      WorkPlan W JOIN Statuses S on s.ID = W.WorkPlanTaskStatusId JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      W.WorkPlanTaskType IN ('Milestone')     AND W.IsCritical IN (1)     AND CAST(W.Modified AS DATE) < DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND S.[Key] not in ('COMPLETED', 'CANCELLED') AND TT.[Key] =  'PROJECT_MANAGEMENT'", "Can you please list down all milestones due in next 7 days which are on critical path and have no update made to them in last 2 weeks in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     RI.TITLE AS [Risk items that does not have Risk Mitigation plan] FROM     RisksAndIssues RI  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId WHERE     RI.IssueRiskCategory = 'Risk'     AND RI.RiskMitigation IS  NULL AND TT.[Key]='PROJECT_MANAGEMENT'", "Are there any risks items that doesn't have risk mitigation plan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(R.ID) [Reports available in PMO] FROM Reports R JOIN MenuOptions M ON R.ParentMenuItemId = M.ID WHERE M.[Key] = 'PROJECT_ANALYTICS' AND R.ReportIsActive = 1", "How many Analytics reports are available for PMO App?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  A.*,  B.StartOfWeek,  B.EndOfWeek  FROM ( SELECT TOP 1 COUNT(*) AS NumberOfItemsDue,         DATEPART(week, TaskDueDate) AS WeekNumber  FROM WorkPlan  W LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId     WHERE TaskDueDate IS NOT NULL AND S.[key] <> 'COMPLETE'  AND TT.[Key]='PROJECT_MANAGEMENT' GROUP BY DATEPART(week, TaskDueDate)     ORDER BY COUNT(*) DESC ) AS A   INNER JOIN (    SELECT DATEADD(week, DATEDIFF(week, 0, TaskDueDate), 0) AS StartOfWeek,         DATEADD(week, DATEDIFF(week, 0, TaskDueDate) + 1, 0) - 1 AS EndOfWeek,         DATEPART(week, TaskDueDate) AS WeekNumber     FROM WorkPlan W    LEFT JOIN ProjectTeams PT ON W.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId      WHERE TaskDueDate IS NOT NULL AND S.[key] <> 'COMPLETE'    AND TT.[Key]='PROJECT_MANAGEMENT'   GROUP BY DATEPART(week, TaskDueDate), DATEADD(week, DATEDIFF(week, 0, TaskDueDate), 0),       DATEADD(week, DATEDIFF(week, 0, TaskDueDate) + 1, 0) ) AS B ON B.WeekNumber = A.WeekNumber", "What week has the most workplan items due across the project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT      AccomplishmentsAndNextSteps.Title FROM     ProjectStatusEntries     LEFT OUTER JOIN AccomplishmentsAndNextSteps ON ProjectStatusEntries.ID = AccomplishmentsAndNextSteps.ProjectStatusEntryId     LEFT JOIN ProjectTeams PT ON PT.ID = ProjectStatusEntries.ProjectTeamId     LEFT OUTER JOIN ReportingPeriods ON ProjectStatusEntries.ReportingPeriodId = ReportingPeriods.ID  WHERE     AccomplishmentNextStepCategory IN '{AccomplishmentNextStepCategory}' AND ReportingPeriods.PeriodStartDate = (         SELECT TOP 1 PeriodStartDate          FROM ReportingPeriods          WHERE CAST (GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE)      ) ", "What are the key action items scheduled for next week?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH CTE AS (SELECT COUNT(*) AS OverdueItemCount,      PT.Title AS [ProjectTeam With Most Overdue Item]  FROM WorkPlan W  LEFT JOIN [ProjectTeams] PT ON PT.ID = W.ProjectteamId   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId  WHERE  	  S.[Key] <> '{Key}'    AND TT.[Key]='PROJECT_MANAGEMENT'    AND CAST(W.TaskDueDate AS DATE ) < CAST(GETDATE() AS DATE) 	  AND PT.Title IS NOT NULL	 GROUP BY PT.Title )  SELECT Top 1 [ProjectTeam With Most Overdue Item] FROM CTE ORDER BY OverdueItemCount DESC", "Which function has the most overdue items?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "CE4-VC", "What are our targets for this engagement?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '{Username}' GROUP BY S.Title ", "CE4-VC", "How are my initiatives doing?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '{Username}' ", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '{Username}' ", "How  many initiatives are assigned to me?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '{Username}' ", "Give my cost reduction initiatives." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(RevenueGrowthEstimate,0)) [RevenueGrowthTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title = {ProjectTeam}", "What are the revenue growth targets for Sales & Marketing?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(CostToAchieveEstimate,0)) [CostToAchieveTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title ={ProjectTeam}", "What are the cost to achieve targets for R&D?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT S.Title [Value Capture Stage] , COUNT(VI.ID) [No. of Initiatives] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} GROUP BY S.Title", "How many initiatives are there in IT across different stages?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Approved Initiaves] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} AND S.Title = 'Approved'", "List IT initiatives that are in approved stage." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Initiaves with Workplan] FROM ValueCaptureInitiatives VI JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WV ON VI.ID = WV.ValueCaptureInitiativeId", "List initiatives that have workplan item linked to them.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT VI.Title [Initiaves with Risk] FROM ValueCaptureInitiatives VI JOIN RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RV ON VI.ID = RV.ValueCaptureInitiativeId LEFT JOIN RisksAndIssues RI ON RI.ID = RV.RisksAndIssueId WHERE RI.IssueRiskCategory = 'Risk'", "How many initiatives have Risks linked?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) [HeadCountCostReductionTarget] FROM [ValueCaptureTopDownEstimates] VT", "What is the total headcount cost reduction target?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Non Active Initiaves] FROM ValueCaptureInitiatives VI WHERE ISNULL(VI.IsItemActive ,0) = 0", "List initiatives that are not active." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages  VCS ON VCI.ValueCaptureStageId = VCS.ID WHERE VCS.Title = {ValueCaptureStage} ", "How many initiatives have been realized?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(NonHeadcountCostReductionEst) + SUM(RevenueGrowthEstimate) + SUM(HeadcountCostReductionEst) 'Total Top Down Target Value' FROM ValueCaptureTopDownEstimates", "What is the Total Top Down Target Value?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(RevenueGrowthEstimate) RevenueGrowth,SUM(TotalCostReduction) CostReduction FROM ValueCaptureTopDownEstimates", "Show my top down target values by cost reduction and revenue growth.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(ID) FROM ValueCaptureInitiatives WHERE IsItemActive = 1", "How many active initatives are there in my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title [Project Team] ,CAST(SUM(ISNULL(RevenueGrowthEstimate,0))/1000000 AS FLOAT) [Revenue Growth(in Million)] FROM [ValueCaptureTopDownEstimates] VT 	LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId GROUP BY PT.Title", "What is the total of revenue growth by team?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VC.Title AS [ValueCaptureInitiatives with no Owners] FROM ValueCaptureInitiatives VC WHERE ItemOwnerId IS NULL", "List out initiatives with no owners assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiative with Workplan at Risk] FROM WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WTVI LEFT JOIN Workplan W ON W.ID = WTVI.WorkPlanId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId LEFT JOIN ValueCaptureInitiatives VI on VI.ID = WTVI.ValueCaptureInitiativeId WHERE S.[KEY] = 'AT_RISK'", "List out initiatives that have at risk workplan task linked." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VP.Title AS ValueCapturepriority, COUNT(VC.ValueCapturePriorityId) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId where ValueCapturePriorityId is not null GROUP BY ValueCapturePriorityId, VP.Title", "Show me initiatives count by priority." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT UP.Title AS UserProfile, COUNT(VC.ID) AS ValueCaptureOwnerwiseCount FROM ValueCaptureInitiatives VC LEFT JOIN UserProfiles UP ON UP.ID = VC.ItemOwnerId WHERE ItemOwnerId IS NOT NULL GROUP BY ItemOwnerId, UP.Title", "Show me initiatives count by owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiaves] FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Title = '{Username}'", "List initiatives where I'm assigned as the Owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as IdentifiedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Identified'", "How many initiatives are identified?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ApprovedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Approved' ", "How many initiatives have been approved?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ValidatedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Validated' ", "How many initiatives have been validated?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCS.Title AS [Value Capture Stage] ,COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCI.ValueCaptureStageId = VCS.ID GROUP BY VCS.Title", "Can you provide breakdown of the initiatives across various stages?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title AS Initiatives FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}'", "What initatives have I been assigned to?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VS.Title ValueCaptureStage FROM ValueCaptureInitiatives VI JOIN ValueCaptureStages VS ON VI.ValueCaptureStageID = VS.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}' ORDER BY ValueCaptureStage", "List my initatives by stages.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VT.Title ValueCaptureType FROM ValueCaptureInitiatives VI JOIN ValueCaptureTypes VT ON VI.ValueCaptureStageID = VT.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}' ORDER BY ValueCaptureType", "List my initatives by Value Capture Type." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title,SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(RevenueGrowthEstimate,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)) AS TotalTopDownTarget FROM ValueCaptureTopDownEstimates LEFT JOIN ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY PT.Title", "What is the top-down target for this project?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS ValueCaptureStage, COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VC.ValueCaptureStageId GROUP BY ValueCaptureStageId, VCS.Title", "Show me initiatives count by stage.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT F.Title as Functions, COUNT(VC.FunctionId) AS ValueCaptureFunction FROM ValueCaptureInitiatives VC LEFT JOIN Functions f ON f.ID = VC.FunctionId WHERE FunctionId IS NOT NULL GROUP BY FunctionId, F.Title ", "Show me initiatives count by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT     CC.Title AS [Cost/Revenue Centers linked to PnL Line Items], CC.Account FROM CostCenters CC     LEFT JOIN ValueCaptureFinancialLineItems Fin         ON CC.FinancialLineItemId = Fin.ID     LEFT JOIN ValueCaptureFinancialLineItemTypes FinType         ON Fin.FinancialLineItemTypeId = FinType.ID WHERE FinType.Title = 'PnL'", "List all Cost/Revenue Centers linked to PnL Line Items.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(ID) AS [Top Down targets #],        SUM(ISNULL(NonHeadcountCostReductionEst, 0)) AS [Total Non Headcount Cost Reduction Estimate],        SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Revenue Growth Estimate],        SUM(ISNULL(CostToAchieveEstimate, 0)) AS [Total Cost To Achieve Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) AS [Total Headcount Cost Reduction Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates ", "Can you provide Top Down Targets KPIs?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  VCTT.Title, PT.Title, VCTT.CostToAchieveEstimate, VCTT.NonHeadcountCostReductionEst, VCTT.HeadcountCostReductionEst, VCTT.RevenueGrowthEstimate FROM   ValueCaptureTopDownEstimates VCTT  Left Join ProjectTeams PT on PT.ID = VCTT.ProjectTeamId", "How many top down targets do we have for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) AS HeadcountCostReduction FROM ValueCaptureTopDownEstimates", "What is the Total Headcount Cost Reduction Target?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  SUM(ISNULL(NonHeadcountCostReductionEst,0)) AS NonHeadcountCostReduction FROM  ValueCaptureTopDownEstimates", "What is the Total Non Headcount Cost Reduction Target?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  SUM(ISNULL(RevenueGrowthEstimate,0)) AS RevenueGrowth FROM  ValueCaptureTopDownEstimates", "What is the Total Revenue Growth Target?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  SUM(ISNULL(CostToAchieveEstimate,0)) AS CostToAchive FROM  ValueCaptureTopDownEstimates", "What is the Total Cost to Achieve Target?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  PT.Title ProjectTeams, VBM.Title Benchmark FROM  ValueCaptureCostBenchmarks VBM LEFT JOIN  ProjectTeams PT ON PT.ID = VBM.ProjectTeamId", "Are there any benchmarks for this project?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Amount as [Client Baseline PnL Amount], A.FinancialLineItem as FinancialLineItem FROM( SELECT F.ID, F.Ordinal, F.Title FinancialLineItem , SUM(C.Amount)/1000000 Amount FROM ValueCaptureFinancialLineItems F LEFT JOIN CostCenters C ON C.FinancialLineItemId = F.ID  GROUP BY F.ID, F.Title, F.Ordinal ) A WHERE A.Amount IS NOT NULL", "What is my program Client Baseline PnL?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     Title AS [Recommended Initiatives List] FROM     ValueCaptureRecommendedInitiatives", "List all recommended initiatives for my program." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT 	SUM(a.[Total Top Down Target Value]) AS [Top Down Targets],      SUM(b.[Bottom Up Initiatives Value]) AS [Bottom Up Initiatives Value], 	( SUM(b.[Bottom Up Initiatives Value])-SUM(a.[Total Top Down Target Value])) AS Variance FROM ( SELECT   SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates) a, ( select SUM(Amount)*12 as [Bottom Up Initiatives Value]  FROM vwUnpivotEstimates where MYear='Y3M12' AND Recurring =1 ) b ", "Compare Top Down Targets vs Bottom Up Initiatives for my project.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "   SELECT ISNULL(SUM(Act.AMOUNT),0.0) AS [Cumulative One Time Cost Amount] 			   FROM vwUnpivotActuals Act 			   LEFT JOIN ValueCaptureImpactTypes VCIT ON Act.ValueCaptureImpactTypeID=VCIT.ID 			   LEFT JOIN ValueCaptureInitiatives VCI ON Act.ValueCaptureInitiativeID=VCI.ID 			   WHERE VCIT.PositiveOrNegativeValues='Negative' 			   AND Act.Recurring<>1 			   AND VCI.Title = '{ValueCaptureInitiative}'", "What is cumulative one-time cost for all initiatives?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "  SELECT  	   WP.Title AS [Workplan Items Linked to Value Capture Initiatives]  	   FROM WorkPlan WP 	   LEFT JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WPVC  	             ON WP.ID=WPVC.WorkPlanId 	   LEFT JOIN ValueCaptureInitiatives VCI  	             ON VCI.ID=WPVC.ValueCaptureInitiativeId", "List all workplan items linked to VC initiatives." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "WITH EstimatedRunRates AS (     SELECT          E.ValueCaptureInitiativeId,  B.Title AS Initiative,          ISNULL(SUM(E.Amount),0) * 12 AS EstimatedAnnualizedRunRate     FROM          vwUnPivotEstimates E     JOIN          ValueCaptureInitiatives B ON E.ValueCaptureInitiativeId = B.ID     WHERE          E.Recurring = 1     GROUP BY          E.ValueCaptureInitiativeId, B.Title ), ActualRunRates AS (     SELECT          A.ValueCaptureInitiativeId, B.Title AS Initiative,          SUM(ISNULL(A.Amount, 0)) * 12 AS ActualAnnualizedRunRate     FROM          vwUnpivotActuals A     JOIN          ValueCaptureInitiatives B ON A.ValueCaptureInitiativeId = B.ID     WHERE          A.Recurring = 1      GROUP BY          A.ValueCaptureInitiativeId, B.Title )   SELECT Initiative AS [List of Initiatives with 10% differ between Estimate and Actual] FROM ( SELECT      E.ValueCaptureInitiativeId,E.Initiative,     E.EstimatedAnnualizedRunRate,     A.ActualAnnualizedRunRate,    IIF(E.EstimatedAnnualizedRunRate = 0 ,0 , (A.ActualAnnualizedRunRate - E.EstimatedAnnualizedRunRate) / E.EstimatedAnnualizedRunRate * 100) AS PercentageDifference FROM      EstimatedRunRates E JOIN      ActualRunRates A ON E.ValueCaptureInitiativeId = A.ValueCaptureInitiativeId ) A WHERE      PercentageDifference > 10", "List all initiatives that had an estimate annualized run-rate differed by more than 10% from the Actual annualized run-rate.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     CONCAT('Tracking Period of the Program is from ', CAST(MIN(StartDt) AS DATE), ' to ', CAST(MAX(EndDt) AS DATE)) FROM     ValueCaptureTransactionMonths WHERE     IsItemActive = 1", "What's the tracking period for my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = '{ProjectTeam}' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = '{ProjectTeam}'    ) Act         ON 1 = 1", "What's the variance for Legal function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "	SELECT   	    COUNT(VCI.ID) AS [Initiative Count for Selected Evaluator Quad]  		     FROM ValueCaptureInitiatives VCI 		LEFT JOIN VCInitiativeEvaluatorQuadInfo VCQ  		    ON VCI.VCInitiativeEvaluatorQuadInfoId=VCQ.ID 		       WHERE VCQ.Title= {EvaluatorQuad}", "Based on Evaluator comparison, how many initiatives have 'High Benefit, High Complexity'?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = '{ProjectTeam}' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = '{ProjectTeam}'     ) Act         ON 1 = 1", "What is my variance for the IT initiatives?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title Initiatives FROM ValueCaptureInitiatives WHERE ValueCapturePriorityId = 1", "List all initiatives that requires attention." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ISNULL(SUM(Est.Amount), 0.0) AS Estimates,        ISNULL(SUM(Act.Amount), 0.0) AS Actuals,        ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = 'Finance'           and VCI.Title = 'Initiative 1'           and Est.MYear = 'Y1M1' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = 'Finance'               AND VCI.Title = 'Initiative 1'               AND MYear = 'Y1M1'     ) Act         ON 1 = 1", "What is my Y1M1 Estimates, Actuals and Variance for Finance - [Initiative 1]?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "WITH EstimatedRunRates AS (     SELECT          E.ValueCaptureInitiativeId,  B.Title AS Initiative,          ISNULL(SUM(E.Amount),0) * 12 AS EstimatedAnnualizedRunRate     FROM          vwUnPivotEstimates E     JOIN          ValueCaptureInitiatives B ON E.ValueCaptureInitiativeId = B.ID 	LEFT JOIN ValueCaptureTransactionMonths VT ON VT.[Key] = E.MYear      WHERE          E.Recurring = 1 		AND (CAST( DATEADD(MM, 3 , GETDATE()) AS DATE) BETWEEN VT.StartDt AND VT.EndDt)     GROUP BY          E.ValueCaptureInitiativeId, B.Title ), ActualRunRates AS (     SELECT          A.ValueCaptureInitiativeId, B.Title AS Initiative,          SUM(ISNULL(A.Amount, 0)) * 12 AS ActualAnnualizedRunRate     FROM          vwUnpivotActuals A     JOIN          ValueCaptureInitiatives B ON A.ValueCaptureInitiativeId = B.ID  	LEFT JOIN ValueCaptureTransactionMonths VT ON VT.[Key] = A.MYear      WHERE          A.Recurring = 1  		AND (CAST( DATEADD(MM, 3 , GETDATE()) AS DATE) BETWEEN VT.StartDt AND VT.EndDt)     GROUP BY          A.ValueCaptureInitiativeId, B.Title )  SELECT Initiative AS [List of Initiatives with 10% differ between Estimate and Actual] FROM ( SELECT      E.ValueCaptureInitiativeId,E.Initiative,     E.EstimatedAnnualizedRunRate,     A.ActualAnnualizedRunRate,    IIF(E.EstimatedAnnualizedRunRate = 0 ,0 , ABS(A.ActualAnnualizedRunRate - E.EstimatedAnnualizedRunRate) / E.EstimatedAnnualizedRunRate * 100) AS PercentageDifference FROM      EstimatedRunRates E JOIN      ActualRunRates A ON E.ValueCaptureInitiativeId = A.ValueCaptureInitiativeId ) A WHERE      PercentageDifference > 10", "List initiatives that had an Estimate ARR that differed by 10% (higher or lower) from the Actual ARR for previous month.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS TotalNetSales FROM ValueCaptureEstimates VE LEFT JOIN ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId WHERE FLT.Title = 'PnL' -- pnl 	AND FL.Title in ('Net sales') -- net total", "In the PnL, what is my net sales?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS TotalOperatingPerformance FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'PnL'  AND FLG.Title in ('Total Operating Performance') ", "In the PnL, what is my total operating performance?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS GrossProfit FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'PnL'  AND FLG.Title in ('Gross Profit') ", "In the PnL, what is my gross profit?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [EBITA & Net Income] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'PnL' AND FLG.Title in ('EBITDA','Net Income') ", "In the PnL, what is my EBITDA and Net Income?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [Total Assets] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'Balance Sheet'  AND FLG.Title in ('Total Current Assets','Total Non-Current Assets') ", "In the Balance Sheet, what is my Total Assets?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [Total Liabilities] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'Balance Sheet'  AND FLG.Title in ('Total Current Liabilities','Total Non-Current Liabilities') ", "In the Balance Sheet, what is my Total Liabilities?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [Total Equity] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'Balance Sheet'  AND FLG.Title in ('Equity') ", "In the Balance Sheet, what is my Total Equity?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	SUM(NonHeadcountCostReductionEst) NonHeadcountCostReduction, 	SUM(HeadcountCostReductionEst) HeadcountCostReduction FROM ValueCaptureTopDownEstimateS", "What is the total of headcount and non-headcount related cost reduction?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH CTE AS  ( 	SELECT ProjectTeamID  	FROM ValueCaptureEstimates E 		LEFT JOIN ValueCaptureInitiatives I ON I.ID = E.ValueCaptureInitiativeId 	EXCEPT 	SELECT ProjectTeamID FROM [ValueCaptureTopDownEstimates] )  SELECT PT.Title [ProjectTeam]  FROM CTE LEFT JOIN ProjectTeams PT ON PT.ID = CTE.ProjectTeamID", "List out teams that have bottom-up estimates but missing top-down targets.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " DECLARE @CurrentYR NVARCHAR(10), @SQL NVARCHAR(MAX)  SELECT  TOP 1 @CurrentYR = [YEAR] FROM vwValueCaptureTransactionMonths WHERE RelativeYear = YEAR(GETDATE())   SELECT @SQL = ' ;WITH TopDownCTE AS ( 	SELECT  		PT.ID  PROJECTTEAMID 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(RevenueGrowthEstimate,0))  + SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM [ValueCaptureTopDownEstimates] VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 	GROUP BY  		PT.ID  		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(RevenueGrowthEstimate,0))  + SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM [ValueCaptureTopDownEstimates] VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 ) , BottomUPCTE AS ( 	SELECT  		VI.PROJECTTEAMID 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 	GROUP BY  		VI.PROJECTTEAMID 		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 )  SELECT  	COALESCE(T.PROJECTTEAMID,B.PROJECTTEAMID) ProjectTeamID 	,COALESCE(T.ProjectTeam,B.ProjectTeam) ProjectTeam 	,ISNULL(T.[Target],0) [TopDown Target(M)] 	,ISNULL(B.Estimate,0) [BottomUP Estimate(M)] 	,ISNULL(T.[Target],0) - ISNULL(B.Estimate,0) [Variance(M)] FROM TopDownCTE T 	FULL OUTER JOIN BottomUPCTE B ON T.PROJECTTEAMID = B.ProjectTeamId ORDER BY PROJECTTEAMID'  EXEC sp_executesql  @SQL", "What is the variance between Top down and bottom up? Provide overall variance and variance by the team.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "WITH EstimatedRunRates AS (     SELECT          E.ValueCaptureInitiativeId,  B.Title AS Initiative,          ISNULL(SUM(E.Amount),0) * 12 AS EstimatedAnnualizedRunRate     FROM          vwUnPivotEstimates E     JOIN          ValueCaptureInitiatives B ON E.ValueCaptureInitiativeId = B.ID     WHERE          E.Recurring = 1 		AND E.MYear IN ('Y1M12')     GROUP BY          E.ValueCaptureInitiativeId, B.Title ), ActualRunRates AS (     SELECT          A.ValueCaptureInitiativeId, B.Title AS Initiative,          SUM(ISNULL(A.Amount, 0)) * 12 AS ActualAnnualizedRunRate     FROM          vwUnpivotActuals A     JOIN          ValueCaptureInitiatives B ON A.ValueCaptureInitiativeId = B.ID     WHERE          A.Recurring = 1          AND A.MYear IN ('Y1M12')     GROUP BY          A.ValueCaptureInitiativeId, B.Title )   SELECT Initiative AS [List of Initiatives with 10% differ between Estimate and Actual] FROM ( SELECT      E.ValueCaptureInitiativeId,E.Initiative,     E.EstimatedAnnualizedRunRate,     A.ActualAnnualizedRunRate,    IIF(E.EstimatedAnnualizedRunRate = 0 ,0 , ABS(A.ActualAnnualizedRunRate - E.EstimatedAnnualizedRunRate) / E.EstimatedAnnualizedRunRate * 100) AS PercentageDifference FROM      EstimatedRunRates E JOIN      ActualRunRates A ON E.ValueCaptureInitiativeId = A.ValueCaptureInitiativeId ) A WHERE      PercentageDifference > 10", "List out initiatives that have an Estimate Annualized Run Rate that differed by 10% (higher or lower) from the Actual Annualized Run Rate for Y1M12.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      VC.Title AS ValueCaptureInitiativesReadyForApproval FROM      ValueCaptureInitiatives VC WHERE  	ReadyForApproval = 1", "List out validated initiatives that are ready to be approved." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      VI.title AS InitiativeImpactOnPnL FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId WHERE      FL.FinancialLineItemTypeId = 1 GROUP BY      VI.Title", "List out initiatives that are impacting PnL." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TOP 5     Initiatives,Amount FROM( SELECT VI.Title Initiatives ,sum(VE.Amount) Amount FROM      vwUnpivotActuals VE LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN 	ValueCaptureImpactTypes VIT ON VIT.ID = VE.ValueCaptureImpactTypeid GROUP BY      VI.Title,VE.Amount ) A  WHERE A.Amount > 0 ORDER BY  	Amount DESC", "List out top 5 initiatives that are positively impacting PnL." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "	SELECT TOP 5  		VI.Title [top 5 initiatives with best benefit score] 	FROM  		ValueCaptureInitiatives VI 	LEFT JOIN  		ValueCaptureImpacts VIM ON VIM.ID = VI.ValueCaptureImpactid 	WHERE  		VIM.ID in (1) 	ORDER BY  		VI.BenefitScore DESC", "List out top 5 initiatives with best benefit score that have high financial impact." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 148,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "	SELECT TOP 5  		VI.Title [top 5 most complex initiatives] 	FROM  		ValueCaptureInitiatives VI 	LEFT JOIN  		ValueCaptureImpacts VIM ON VIM.ID = VI.ValueCaptureImpactid 	WHERE  		VIM.ID in (3) 	ORDER BY  		VI.ComplexityScore DESC", "List out top 5 most complex initiatives that have low financial impact." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TOP 1 	PT.Title, SUM(ISNULL(HeadcountCostReductionEst,0)) AS MostHeadcountCostReduction FROM  	ValueCaptureTopDownEstimates LEFT JOIN  	ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY  	PT.Title ORDER BY  	SUM(ISNULL(HeadcountCostReductionEst,0)) DESC", "Which team has most amount of headcount related cost reduction?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Iniatives that are Cost Reduction and are Easy for Implementation Ease] FROM ValueCaptureInitiatives VI     LEFT JOIN ValueCaptureTypes VT         ON VT.ID = VI.ValueCaptureTypeID     LEFT JOIN ImplementationEase VD         ON VD.ID = VI.ImplementationEaseID WHERE VT.Title = 'Cost Reduction'       AND VD.Title = 'Easy'", "List all Iniatives that are Cost Reduction and are Easy for Implementation Ease." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VS.Title [Value Capture Stage],        VI.Title [Initiatives that have Talent intiative value drivers] FROM ValueCaptureInitiatives VI     LEFT JOIN ValueCaptureStages VS         ON VS.ID = VI.ValueCaptureStageID     LEFT JOIN ValueCaptureInitiativesToValueCaptureValueDriversForValueCaptureValueDrivers VD         ON VD.ValueCaptureInitiativeId = VI.ID     LEFT JOIN ValueCaptureValueDrivers VV         ON VD.ValueCaptureValueDriverId = VV.ID WHERE VV.Title = 'Talent' ", "Provide me a list of Iniatives by stage that have Talent iniative value drivers." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	Title as [Value Capture Initiatives with No Owners] FROM 	ValueCaptureInitiatives as VC WHERE 	ItemOwnerId IS NULL", "Show me all iniatives that do not have an owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ISNULL(VP.Title, 'No Priority') AS ValueCapturePriority,     COUNT(VC.ID) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId --WHERE VC.ValueCapturePriorityId IS NOT NULL GROUP BY VC.ValueCapturePriorityId, VP.Title  ---Stage---  SELECT ISNULL(VS.Title, 'No Stage Assigned') AS ValueCaptureStage,     COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VS ON VS.ID = VC.ValueCaptureStageId WHERE VC.ValueCaptureStageId IS NOT NULL GROUP BY VC.ValueCaptureStageId, VS.Title", "Provide me a count of initiatives by prioirty and stage." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT FL.Title [Balance sheet line items that do not have a linked account] FROM ValueCaptureFinancialLineItems FL     LEFT JOIN ValueCaptureFinancialLineItemTypes FLT         ON FLT.ID = FL.FinancialLineItemTypeID     LEFT JOIN BalanceSheetAccounts BLA         ON BLA.FinancialLineItemID = FL.ID WHERE FLT.Title = 'Balance Sheet'       AND BLA.ID IS NULL", "Show me all Balance sheet line items that do not have a linked account." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(E.Amount, 0) * 12) [Run Rate for iniatives in Approved, Committed and Realized] FROM ValueCaptureInitiatives VI     LEFT JOIN vwUnpivotEstimates E         ON E.ValueCaptureInitiativeId = VI.ID     LEFT JOIN ValueCaptureStages VS         ON VS.ID = VI.ValueCaptureStageID WHERE VS.Title IN ( 'Approved', 'Committed', 'Realized' )       AND MYear =       (           select [Key]           from ValueCaptureTransactionMonths           where CAST(EndDt AS date) =           (               SELECT MAX(CAST(EndDt AS date)) FROM ValueCaptureTransactionMonths           )       )", "What is the Run Rate for iniatives in Approved, Committed and Realized?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title [Project Team],        SUM(T.HeadCountCostReductionEst) [Total target Headcount cost redutciton] FROM ValueCaptureTopDownEstimates T     LEFT JOIN ProjectTeams PT         ON PT.ID = T.ProjectTeamId GROUP BY PT.Title", "Provide the total Headcount cost redutciton target listed out by project team and provide the number of FTEs." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(T.FTEs) [FTEs part of the top down targets for EMEIA] FROM ValueCaptureTopDownEstimates T     LEFT JOIN Regions R         ON R.ID = T.RegionID WHERE R.Title = 'EMEIA'", "How many FTEs are part of the top down targets for EMEIA?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	VI.Title [Initiatives with Benefit score over 75] FROM 	ValueCaptureInitiatives VI LEFT JOIN  	ProjectTeams PT ON PT.ID = VI.ProjectTeamId WHERE 	CAST(BenefitScore AS INT) > 75 	AND PT.Title IN ('HR')", "Which HR iniatives have a benefit score over 75?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 	VI.Title [Initiatives that have Risk associated with them] FROM 	ValueCaptureInitiatives VI LEFT JOIN  	ValueCaptureStages VS ON VS.ID = VI.ValueCaptureStageId LEFT JOIN  	RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs VIR ON VIR.ValueCaptureInitiativeId = VI.ID LEFT JOIN 	RisksAndIssues RI ON RI.ID = VIR.RisksAndIssueId WHERE  	RI.IssueRiskCategory IN ('Risk') 	AND VS.Title IN ('Committed')", "List out all Committed Iniatives that have a risk associated with them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	VI.Title [top 5 initiatives with best benefit score] FROM  	ValueCaptureInitiatives VI LEFT JOIN 	ValueCaptureImpactTypes VIT ON VIT.ID = VI.ValueCaptureImpactTypeId LEFT JOIN  	ValueCaptureImpacts VIM ON VIM.ID = VI.ValueCaptureImpactid WHERE  	VIM.[Key] in ('High') 	AND VIT.PositiveOrNegativeValues IN ('Positive')", "List all High Benefit iniatives that are in the Identified Stage." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 	VI.Title [Initiative is Approved with no Owner] FROM 	ValueCaptureInitiatives VI LEFT JOIN  	ValueCaptureStages VS ON VS.ID = VI.ValueCaptureStageId WHERE  	VS.Title IN ('Approved') 	AND 	VI.ItemOwnerId IS NULL", "Show me all iniatives that are approved and do not have an owner assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 	COUNT(VI.ID) FROM 	ValueCaptureInitiatives VI LEFT JOIN 	UserProfiles UP ON UP.ID = VI.ItemOwnerId WHERE  	UP.EMail = '{Username}'", "How many iniatives are assigned to me?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	PT.Title ProjectTeam,SUM(ISNULL(NonHeadcountCostReductionEst,0)) NonHeadcountCostReductionTarget FROM  	ValueCaptureTopDownEstimates LEFT JOIN  	ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY  	PT.Title", "Provide the total non-headcount cost reduciton targets listed out by project team." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 164,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 165,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 166,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 167,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "How many assets are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 168,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "Can you provide me number of processes by op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in Day1 state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 172,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(1) FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'  AND N.NodeTypeId = 3  AND N.ID NOT IN (   SELECT DISTINCT NTO.NodeId   FROM Nodes N1   LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null  )", "How many processes don't have an Owner assigned in Current State?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 173,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.ID 	,PR.Title FROM Nodes PR JOIN TransactionStates T ON T.ID = PR.TransactionStateId WHERE T.[Key] = 'DAY_ONE' 	AND PR.NodeTypeId = 3 	AND NOT EXISTS  				(SELECT 1 FROM [NodesToDispositionsForDispositionNew] D WHERE D.NodeId = PR.ID)", "List processes that don't have Disposition assigned in Day 1 state." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT d.Title FROM Dispositions d", "List the Disposition options available." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 175,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [ProcessGroup] ,COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) [Enablers Count] FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 2 GROUP BY N.Title ORDER BY [Enablers Count] DESC", "How many enablers are associated with each Process Group?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 176,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title [Disposition] 	,COUNT(SD.SystemId) [SystemCount] FROM Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID GROUP BY D.Title", "List the total number of systems by disposition." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 177,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT PR.Title [Process] , D.DispositionId FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId  	LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 	AND D.NodeId IS NULL 	AND S.[Key] = 'DAY_ONE'", "Can you please list down all Day 1 processes where no disposition has been tagged?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 178,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT 	N.Title [Process]	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 3 GROUP BY N.Title HAVING COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) = 0", "Can you please list all processes with no Enablers?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.Title as MissingInDay1 FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'CURRENT_STATE' EXCEPT SELECT 	PR.Title [Process] FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'DAY_ONE'", "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 180,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title Enabler, 'Asset' as Category	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID LEFT JOIN [Assets] A ON A.ID = NA.AssetsId WHERE A.Title IS NOT NULL UNION ALL SELECT DISTINCT F.Title Enabler, 'Facility' as Category FROM Nodes N LEFT JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID LEFT JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE F.Title IS NOT NULL  UNION ALL SELECT DISTINCT S.Title Enabler , 'System' as Category FROM Nodes N LEFT JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID LEFT JOIN [Systems] S ON S.ID = NS.SystemsId WHERE S.Title IS NOT NULL  UNION ALL SELECT DISTINCT T.Title Enabler	, 'TPA' as Category FROM Nodes N LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID LEFT JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId WHERE T.Title IS NOT NULL ", "What Enablers are we tracking for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(s.ID) AS [# Systems present in this functional OP model] FROM Systems s", "How many Systems are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(tpa.ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements tpa", "How many TPAs are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 183,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ST.Title AS SystemType, COUNT(S.ID) [# Systems by Type] FROM Systems S LEFT JOIN SystemTypes ST ON ST.ID = S.TypeId WHERE S.TypeId IS NOT NULL GROUP BY ST.Title", "List the number of Systems by Type." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 184,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT O.Title AS Owners, Count(*) AS [# TPAs by Ownership] FROM ThirdPartyAgreements TP LEFT JOIN Owners O ON O.ID = TP.OwnerID WHERE TP.OwnerID IS NOT NULL GROUP BY O.Title", "List the number of TPAs by Ownership." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [ProcessGroups With No Process] FROM Nodes N WHERE N.nodetypeid = 2 AND ID NOT IN (SELECT ns.NodeParentId FROM Nodes ns)", "List down all process groups with no process within them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 186,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(P.[ID]) AS [Number of Process across Op Model] FROM Nodes P     LEFT JOIN NodeTypes NType         ON P.NodeTypeId = NType.ID WHERE NType.Title = 'Process'", "List the number of processes across op models." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        OMA.Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        [OMA.Title] AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.Modified >= DATEADD(hh, -1, GETDATE())", "List history of changes to the op model in the past one hour." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        [OMA.Title] AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.FieldImpacted = 'Owner'       AND CAST(OMA.Modified AS DATE) = CAST(GETDATE() AS DATE)", "List the history of changes to Ownership in the op model today." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        [OMA.Title] AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.Title = 'Deleted'       AND DATEPART(wk, CAST(OMA.Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(OMA.Modified) = YEAR(GETDATE())", "List the history of deletes in the op model this week." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 190,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT T.Title [State], N.Title AS OperatingModel FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId  WHERE N.NodeTypeId = 1", "List the functional operating models across different states." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 191,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title Opmodel, L.Title LiveNote, Note FROM OperatingModelLiveNotes L LEFT JOIN Nodes N ON N.Id = L.NodeId", "Are there any Live Notes in this op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 192,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process FROM OperatingModelActivityLog OL  LEFT JOIN Nodes N ON N.ID = Ol.ObjectID WHERE OL.Title = 'Updated' AND Object = 'Process' AND (CAST(OL.Modified AS DATE) BETWEEN DATEADD(DAY, -7, CAST(GETDATE()AS DATE))  AND CAST(GETDATE() AS DATE))", "List down all processes which were updated since last 1 week." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 193,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process, OldValue, NewValue  FROM OperatingModelActivityLog PL  LEFT JOIN Nodes N ON N.ID = PL.ObjectID  WHERE Object = 'Process' AND PL.Title = 'Updated' AND FieldImpacted = 'Title' AND N.TransactionStateId = 6", "List down all processes which have been renamed in Day 1 op model." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 194,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TOP 5     S.Title AS [Systems],     Count(NSS.NodesID) AS [ProcessCount] FROM Nodes N     LEFT JOIN [dbo].[NodesToSystemsForEnablerSystems] NSS         ON N.ID = NSS.NodesId     LEFT JOIN [dbo].[Systems] S         ON NSS.SystemsId = S.ID WHERE N.NodeTypeId = 3 GROUP BY S.Title ORDER BY Count(NSS.NodesID) DESC", "Please provide top 5 systems that are linked to various processes." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 195,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT N.Title [List of processes for CurrentState where ownership is not present] FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'   AND N.NodeTypeId = 3   AND N.ID NOT IN (     SELECT DISTINCT NTO.NodeId     FROM Nodes N1     LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null   )", "For Current State, list all processes that doesn't have any ownership.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 196,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " SELECT     PR.Title [Process] FROM     Nodes                                    PR     JOIN         TransactionStates                    S             ON S.ID = PR.TransactionStateId     LEFT JOIN         NodesToDispositionsForDispositionNew D             ON D.NodeId = PR.ID WHERE     PR.NodeTypeId = 3     AND D.NodeId IS NULL     AND S.[Key] = 'DAY_ONE'", "For Day 1 Operating Models, list all processes that doesn't have any dispositions.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 197,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,       --SELECT * FROM PG_Hierarchy         ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT TOP 1     Functions AS [Function with highest number of processes] FROM (     SELECT Functions,            COUNT(ID) as [Process Count]     FROM ProcessCTE     GROUP BY Functions ) A order by [Process Count] desc    ", "Which function has the highest number of processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 198,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     N.Title AS [List of Processes with Disposition as TSA] FROM   Nodes   N   LEFT JOIN     NodesToDispositionsForDispositionNew NTD             ON NTD.NodeId = N.ID     LEFT JOIN         Dispositions D  ON NTD.DispositionId = D.ID WHERE     D.Title = 'TSA'     AND N.NodeTypeId = 3 -- To get the list of Processes", "List down all Processes which have their disposition set as TSA.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 199,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT       N.Title as [List of Processes with No Disposition] FROM Nodes N          WHERE N.NodeTypeId=3 AND N.ID NOT IN   (SELECT DN.NodeId FROM NodesToDispositionsForDispositionNew DN)", "List down all processes with no Disposition assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 200,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(1)  [No. of Processes] FROM Nodes PR JOIN NODES P ON PR.NodeParentID = P.ID WHERE PR.NodeTypeId = 3 AND P.Title = '{FunctionName}'", "How many processes are there in the IT function?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 201,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  PR.Title [Process] , COUNT(DISTINCT D.DispositionId) [DispostionCount] FROM Nodes PR JOIN NODES P ON PR.NodeParentID = P.ID LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 AND P.Title = 'COMMERCIAL' GROUP BY PR.Title HAVING COUNT(DISTINCT D.DispositionId) > 1", "List processes that have more than 1 disposition in IT." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 202,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ),       --SELECT * FROM PG_Hierarchy         ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT ISNULL(Geo.Title, 'Not Assigned') AS [Geographic Coverages],        COUNT(P.ProcessID) AS [Total Number of Process in Real Estate] FROM ProcessCTE P     LEFT JOIN ProcessAttributes PA         ON P.ProcessID = PA.NodeId     LEFT JOIN GeographicCoverages Geo         ON PA.GeographicCoverageId = Geo.ID WHERE P.Functions ='{FunctionName}' GROUP BY Geo.Title", "What is total number of processes in Real Estate grouped by geographic coverage?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 203,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      F.Title AS [Functions],     SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID ORDER BY 1,          2", "List all functions and sub-functions for functional operating model [Name].", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 204,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     O.Title     AS [Process Owner],     COUNT(N.ID) AS [Process Distribution] FROM     Nodes      N     LEFT JOIN         Owners O             ON N.OwnerId = O.ID WHERE     N.NodeTypeId = 3 GROUP BY     O.Title ", "Provide process owner distribution for my project.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 205,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID WHERE F.Title='{FunctionName}' ORDER BY 1", "List all sub-functions for Finance.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 206,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,       --SELECT * FROM PG_Hierarchy        ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT P.Processes,        ISNULL(S.Title, 'Not Assigned') AS [Systems],        ISNULL(TPA.Title, 'Not Assigned') AS [Third Party Agreements] FROM ProcessCTE P     LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA         ON P.ProcessID = NTPA.NodesId     LEFT JOIN ThirdPartyAgreements TPA         ON NTPA.ThirdPartyAgreementsId = TPA.ID     LEFT JOIN NodesToSystemsForEnablerSystems NSys         ON NSys.NodesId = P.ProcessID     LEFT JOIN Systems S         ON NSys.SystemsId = S.ID", "List all details for processes and systems, third party agreements linkages.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 207,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT A.Title Assest,        P.Title LinkedProcess FROM Assets A     LEFT JOIN ProcessAssetLinks PA         ON PA.AssetId = A.ID     LEFT JOIN Nodes P         ON P.ID = PA.ProcessId     LEFT JOIN NodeTypes NType         ON P.NodeTypeId = NType.ID WHERE NType.Title = 'Process'", "List the Assets and their linked processes.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 208,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title OpModel, BP.Title  EndToEndProcess FROM BusinessProcesses BP  LEFT JOIN Nodes N ON N.ID = BP.NodeId  WHERE N.NodeTypeId = 1", "Are there any end-to-end business processes in this op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 209,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes  N    WHERE NodeParentID IS NULL           AND TransactionStateID = 8     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ) SELECT N.Title [Function],        NTSS.Title AS [Field Status],        OPS.Title AS [Op Status] FROM NodeTrackers NT     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NT.ID     JOIN     (SELECT * FROM GetSelectedFunction GSF WHERE HierId = 2 and Title = 'HR') N         ON N.ID = NT.NodeId     LEFT JOIN NodeTrackerStatusFields NTSS         ON NTSS.ID = NTS.NodeTrackerStatusFieldId     LEFT JOIN OpStatuses OPS         ON OPS.ID = NTS.NodeTrackerStatusStatusId", "In the Progress Tracker, what is the current status of HR?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 210,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "/*---- What is Automatic Workplan?----*/ SELECT D.Title Disposition,        WP.Title Tasks FROM NodesToWorkPlansForTask NWP     LEFT JOIN Nodes N         ON N.ID = NWP.NodeId     LEFT JOIN WorkPlan WP         ON NWP.WorkPlanId = WP.ID     LEFT JOIN NodesToDispositionsForDispositionNew ND         ON ND.NodeID = N.ID     LEFT JOIN Dispositions D         ON D.ID = ND.DispositionID WHERE N.NodeTypeId = 3", "In the Automatic Workplan, what are the defualt task lists for different dispostions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes ND    WHERE NodeParentID IS NULL           AND TransactionStateID = 6     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM Nodes ROOT     WHERE ROOT.ID in (  select ID from GetSelectedFunction GSF where HierId = 2 and Title = 'Finance'  )     UNION ALL     SELECT CHILD.NodeParentID, CHILD.ID,   CHILD.Title, PARENT.TITLE,   PARENT.ID, CHILD.NodeTypeId     FROM PG_Hierarchy PARENT PGHP,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ) SELECT COUNT(*) AS [Process with multiple dispositions for Finance] FROM PG_Hierarchy WHERE NodeTypeId = 3       AND ID IN (                     SELECT NodeID                     FROM                     (                         SELECT NodeId,                                Count(DispositionID) As [Disposition Count] FROM NodesToDispositionsForDispositionNew NDDN  GROUP BY NodeId HAVING Count(DispositionID) > 1 ) a  )", "In Finance, how many processes have multiple dispositions?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 212,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     N.Title AS [List of Process which doesn''t have ownership] FROM  Nodes   N  JOIN  TransactionStates T  ON T.ID = N.TransactionStateId WHERE     OwnerId IS NULL     AND N.NodeTypeId = 3 -- To select Node Type = Process", "List down all processes with no Owners assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 213,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title AS [List of Processes with more than one Disposition] FROM Nodes N WHERE N.NodeTypeId = 3       AND N.ID IN (                       SELECT NodeID                       FROM                       (                           SELECT NodeId,                                  COUNT(DispositionID) AS [Disposition Count]                           FROM NodesToDispositionsForDispositionNew                           GROUP BY NodeId                           HAVING COUNT(DispositionID) > 1                       ) a                   ) ", "List down all processes with more than one Disposition associated with them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT ID,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes  WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.ID,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, TransactionStateID) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.TransactionStateID     FROM Nodes ROOT     WHERE ROOT.ID IN (                          SELECT ID                          FROM GetSelectedFunction                          WHERE HierId = 2                                AND Title = '{FunctionName}'                                AND TransactionStateId = 6                      )     UNION ALL     SELECT CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.TransactionStateId     FROM PG_Hierarchy PARENT,          Nodes CHILD     WHERE PARENT.id = CHILD.NodeParentID    ) SELECT Title AS [List of Processes Associated with Legal Function] FROM PG_Hierarchy ORDER BY ID,          title DESC ", "Can you please list all processes associated with Legal function?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 215,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     N.Title AS [List of all Processes associated with APAC] FROM     Nodes       N     LEFT JOIN         Regions R             ON N.CountryRegionID = R.ID WHERE     n.NodeTypeId = 3     AND R.Title = '{Region}'", "Can you please list all processes associated with APAC?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT P.Functions, A.Processes  FROM (SELECT DISTINCT [Processes]  		FROM ProcessCTE 		GROUP BY Processes  		HAVING COUNT(DISTINCT [Functions] ) > 1) A  	LEFT JOIN ProcessCTE P ON P.Processes = A.Processes ", "Can you please list all processes with same names across different functions?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 217,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     [List of Processes with same name] FROM     (         SELECT             N.Title  AS [List of Processes with same name],             Count(1) AS [Repeat Count]         FROM             Nodes N         WHERE             N.NodeTypeId = 3         GROUP BY             N.Title         HAVING             COUNT(*) > 1     ) a ORDER BY     1 ", "Can you please list all processes with same name?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 218,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title AS [List of Processes tagged as ParentCo in Current State with Disposition set as TSA ] FROM Nodes N     LEFT JOIN NodesToOwnersForOwnerNew NTO         on N.ID = NTO.NodeId     LEFT JOIN Owners O         ON O.ID = NTO.OwnerId     LEFT JOIN NodeTypes NT         ON N.NodeTypeId = NT.ID     LEFT JOIN TransactionStates TS         ON N.TransactionStateId = TS.ID     LEFT JOIN NodesToDispositionsForDispositionNew NTD         ON N.ID = NTD.NodeId     LEFT JOIN Dispositions D         on NTD.DispositionId = D.ID WHERE NT.Title = 'Process'       AND TS.[Key] = 'CURRENT_STATE'       AND O.Title = 'Parent Co'       AND D.Title = 'TSA'", "Can you please list the processes which are tagged as 'ParentCo' in Current state having disposition set as 'TSA'?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 219,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     Title AS [Systems not tagged in any process] FROM     Systems WHERE     ID NOT IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes                               N                       LEFT JOIN                           NodesToSystemsForEnablerSystems NSS                               ON N.ID = NSS.NodesId  					  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId                    WHERE T.Title= 'Day 1' AND                        N.NodeTypeId = 3               )", "can you please list all systems which are not tagged to any process in Day 1 op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 220,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     ISNULL(ST.Title, 'System Type Not Assigned') AS [SystemsType],     S.Title AS [Systems] FROM     Systems         S     LEFT JOIN         SystemTypes ST             ON S.TypeId = ST.ID", "Can you please provide of all systems based upon their type?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 221,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nod  WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number for Multiple Deposition]  				FROM ProcessCTE 				WHERE ID IN ( 									SELECT NodeID 									FROM 									( 										SELECT NodeId, 											   Count(DispositionID) As [Disposition Count] 										FROM NodesToDispositionsForDispositionNew 										GROUP BY NodeId 										HAVING Count(DispositionID) > 1 									) a 								) 		GROUP BY [Functions] 				ORDER BY  COUNT(*) DESC    ", "Which function has the highest number of multi disposition processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 222,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nod  WHERE NodeParentID IS NULL   UNION ALL         SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   WHERE Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2    UNION ALL         SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  WHERE  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS (  SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Live Without]  FROM ProcessCTE WHERE ID IN (	SELECT NodeId 				FROM NodesToDispositionsForDispositionNew ND  				LEFT JOIN Dispositions D ON D.ID = DispositionID  				WHERE D.Title = 'Live without'			 				) GROUP BY [Functions] ORDER BY  COUNT(*) DESC", "which function has the highest number of 'Live without'?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 223,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nod WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Processes tagged to them] , COUNT(*)  [No of Processes] 				FROM ProcessCTE 		GROUP BY [Functions] 				ORDER BY  COUNT(*) DESC    ", "Which function has the highest number of processes tagged to them? and how many?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 224,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ),       ProcessCTE AS (SELECT DISTINCT         F.Title AS [Functions],         f.id AS FunctionID     FROM     (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF             ON F.ID = SF.NodeParentID    ) SELECT Top 1     Functions as [Function with Highest Number of Enablers],     [Enablers Count] FROM (     SELECT P.Functions,            --,COUNT(DISTINCT TPA.ID) as TPACount            --,COUNT(DISTINCT S.ID) as SystemCount            --,COUNT(DISTINCT Asset.ID) as AssetCount            --,COUNT(DISTINCT Roles.ID) as RolesCount,            COUNT(DISTINCT TPA.ID) + COUNT(DISTINCT S.ID) + COUNT(DISTINCT Asset.ID) + COUNT(DISTINCT Roles.ID) AS [Enablers Count]     FROM ProcessCTE P         LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA --Third Party Aggrements             ON P.FunctionID = NTPA.NodesId         LEFT JOIN ThirdPartyAgreements TPA             ON NTPA.ThirdPartyAgreementsId = TPA.ID         LEFT JOIN NodesToSystemsForEnablerSystems NSys -- Systems             ON NSys.NodesId = P.FunctionID         LEFT JOIN Systems S             ON NSys.SystemsId = S.ID         LEFT JOIN NodesToAssetsForEnablerAssets NAsset --Assets             ON NAsset.NodesId = P.FunctionID         LEFT JOIN Assets Asset             ON Asset.ID = NAsset.AssetsId         LEFT JOIN NodesToRolesForEnablerRoles NRole --Roles             ON NAsset.NodesId = P.FunctionID         LEFT JOIN OrgRolesMaster Roles             ON Roles.ID = NRole.RolesId     GROUP BY P.Functions ) A ORDER BY [Enablers Count] DESC", "Which function has the highest number of enablers tagged to them? and how many?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 225,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID,  HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL    UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,    HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy(NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM Nodes ROOT   WHERE ROOT.ID in (select ID from GetSelectedFunction where HierId=2 and Title='Finance & Accounting')       UNION ALL        SELECT  CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  )   SELECT PG.Title, OldValue, NewValue  FROM PG_Hierarchy PG  JOIN (SELECT PL.ObjectID, OldValue, NewValue 			FROM OperatingModelActivityLog PL 			WHERE Object = 'Process' AND Title = 'Updated' AND PL.Modified = ( 			SELECT  Max(Modified) Modified FROM OperatingModelActivityLog  			WHERE ObjectID IN (SELECT ID FROM PG_Hierarchy PG WHERE NodeTypeId=3 )  			AND Object = 'Process' AND Title = 'Updated')) A ON A.ObjectID = PG.ID", "Which process within Finance has been updated most recently and what was the updates?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 226,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     N.Title AS [List of Process Associated with Workplan] FROM     Nodes                       N     LEFT JOIN         NodesToWorkPlansForTask NWP             ON N.ID = NWP.NodeId     LEFT JOIN         WorkPlan                WP             ON NWP.WorkPlanId = WP.ID WHERE     N.NodeTypeId = 3", "How many processes have been associated with a workplan item?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 227,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(DISTINCT n.ID) AS [No of Process having 2 owners or Dispositions] FROM Nodes n LEFT JOIN (     SELECT NodeId     FROM NodesToDispositionsForDispositionNew  ndd   GROUP BY NodeId     HAVING COUNT(DispositionID) = 2 ) d ON n.ID = d.NodeId LEFT JOIN (     SELECT NodeID     FROM NodesToOwnersForOwnerNew  non   GROUP BY NodeID     HAVING COUNT(OwnerId) = 2 ) o ON n.ID = o.NodeID WHERE n.NodeTypeId = 3 AND (d.NodeId IS NOT NULL OR o.NodeID IS NOT NULL)", "How many processes have 2 owners or dispositions? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 228,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	COUNT(SD.SystemId) [No of Systems are dispositioned rebuild] FROM  Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID WHERE D.Title = 'Rebuild'", "How many systems are dispositoned Rebuild?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 229,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT O.Title [Owners], COUNT(N.ID) [No of Processes]  FROM Nodes N  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID  LEFT JOIN NodesToOwnersForOwnerNew NOS ON NOS.NodeId = N.ID  LEFT JOIN Owners O ON O.ID = NOS.OwnerId  WHERE NT.Title = 'Process'  GROUP BY O.Title", "Provide a count of all processes by owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 230,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	R.Title Regions 	, COUNT(D.DispositionId) [Count of Disposition]  FROM NodesToDispositionsForDispositionNew D     JOIN Nodes N ON D.NodeId = N.ID 	JOIN TransactionStates S ON S.ID = N.TransactionStateId  	LEFT JOIN NodeTypes NT ON NT.ID = N.NodeTypeId  	LEFT JOIN Regions R ON R.ID = N.CountryRegionID  WHERE NT.Title = 'Process'  	AND S.[Key] = 'DAY_ONE' GROUP BY R.Title", "Show me the count of dispositions in Day 1 model by assigned region." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 231,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [Processes with no owner assigned] FROM Nodes N  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID WHERE NT.Title = 'Process'    AND N.ID NOT IN (     SELECT DISTINCT NTO.NodeId     FROM Nodes N1     LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	WHERE NTO.NodeId is not null   )", "List all processes that do not have an owner assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 232,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title AS [List of Teams with Disposition set as Unidentified] FROM Nodes N     LEFT JOIN NodeTypes NT         ON N.NodeTypeId = NT.ID 	LEFT JOIN  NodesToProjectTeamsForProjectTeam NP 	    ON N.ID = NP.NodesId  	LEFT JOIN ProjectTeams PT 	    ON PT.ID = NP.ProjectTeamsId      LEFT JOIN NodesToDispositionsForDispositionNew NTD         ON N.ID = NTD.NodeId     LEFT JOIN Dispositions D         on NTD.DispositionId = D.ID WHERE NT.Title = 'Process'       AND D.Title = 'Unidentified'", "Which teams have processes that are disposition 'Unidentified'?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 233,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [Processes with total headcount greater than 0] FROM Nodes N     LEFT JOIN vwHeadcount H         ON H.NodeId = N.ID     LEFT JOIN Nodetypes NT         ON NT.ID = N.NodeTypeId GROUP BY N.Title HAVING SUM(ISNULL(H.TotalHeadCount, 0)) > 0", "List all processes with total headcount greater than 0." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 234,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT F.Title [Facilities in North America that are leased or owned]   FROM Facilities F  LEFT JOIN FacilityOccupancyTypes FT ON FT.ID = F.FacilityOccupancyTypeID  LEFT JOIN Countries C ON C.ID = F.CountryID  LEFT JOIN Regions R ON R.ID = C.CountryRegionID WHERE FT.Title IN ('Leased', 'Owned') AND R.Title = 'North America'", "List all Facilities in North America that are leased or owned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 235,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(N.ID) [No of Proccesses to which TPA is assigned] FROM NodesToThirdPartyAgreementsForEnablerTpa NTP  INNER JOIN Nodes N ON N.ID = NTP.NodesId  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID WHERE NT.Title = 'Process'", "How many processes have a TPA assigned to them? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 236,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Enabler [Enabler with linked processes] FROM( SELECT DISTINCT A.Title Enabler 	 FROM Nodes N LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID INNER JOIN [Assets] A ON A.ID = NA.AssetsId  WHERE NTT.Title = 'Process'    UNION ALL  SELECT DISTINCT F.Title Enabler FROM Nodes N LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID INNER JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE NTT.Title = 'Process'    UNION ALL  SELECT DISTINCT S.Title Enabler  FROM Nodes N  LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID INNER JOIN [Systems] S ON S.ID = NS.SystemsId WHERE NTT.Title = 'Process'    UNION ALL  SELECT DISTINCT T.Title Enabler	 FROM Nodes N  LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID INNER JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId  WHERE NTT.Title = 'Process'   ) A", "Show me all enablers that have a linked process." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 237,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title AS [List of Processes with Disposition set as TSA ] FROM Nodes N     LEFT JOIN NodeTypes NT         ON N.NodeTypeId = NT.ID     LEFT JOIN NodesToDispositionsForDispositionNew NTD         ON N.ID = NTD.NodeId     LEFT JOIN Dispositions D         on NTD.DispositionId = D.ID WHERE NT.Title = 'Process'       AND D.Title = 'TSA'", "List all processes that have the disposition TSA." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 238,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(N.ID) AS [No of Processes are Live Without]  FROM Nodes N  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID WHERE NT.Title = 'Process' AND N.ID IN (	SELECT NodeId 				FROM NodesToDispositionsForDispositionNew ND  				LEFT JOIN Dispositions D ON D.ID = DispositionID  				WHERE D.Title = 'Live without'			 				) ", "How many processes are Live without? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 239,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     Title AS [Systems being used by Project Team and Process Group] FROM     Systems WHERE  ID IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes N                       LEFT JOIN NodesToSystemsForEnablerSystems NSS                        ON N.ID = NSS.NodesId  					  LEFT JOIN  NodesToProjectTeamsForProjectTeam NP 					  ON N.ID = NP.NodesId  					  LEFT JOIN NodeTypes NT ON NT.ID = N.NodeTypeId                    WHERE  NT.Title IN ('ProcessGroup')               )	", "List out the systems being used by Project Team and Process Group." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 240,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [Process groups are not started on Followup Calls for Current State] FROM Nodes N     LEFT JOIN Nodetypes NT         ON NT.ID = N.NodeTypeId     LEFT JOIN NodeTrackers NTC         ON NTC.NodeID = N.ID     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NTC.ID     LEFT JOIN TransactionStates T         ON T.ID = N.TransactionStateID WHERE N.NodeOnTracker = 1       AND NT.Title = 'Process Group'       AND T.[Key] = 'CURRENT_STATE'       AND N.ID NOT IN (SELECT N.ID ProcessGroupID   FROM Nodes N  LEFT JOIN Nodetypes NT ON NT.ID = N.NodeTypeId LEFT JOIN NodeTrackers NTC  ON NTC.NodeID = N.ID LEFT JOIN NodeTrackerStatuses NTS ON NTS.NodeTrackerID = NTC.ID  LEFT JOIN TransactionStates T  ON T.ID = N.TransactionStateID   WHERE N.NodeOnTracker = 1 AND NT.Title = 'Process Group' AND T.[Key] = 'CURRENT_STATE' AND NTS.Title = 'FollowUp Call')", "Which process groups are not started on Followup Calls for Current State?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 241,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL      UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ) SELECT O.Title [Client Owner for HR] FROM GetSelectedFunction N 	LEFT JOIN NodesToOwnersForOwnerNew NOS ON NOS.NodeId = N.ID  	LEFT JOIN Owners O ON O.ID = NOS.OwnerId  WHERE N.HierId = 2 AND N.Title = 'HR'", "Who is the Client owner for HR?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 242,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 243,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 244,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Provider." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 245,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Receiver." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 246,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "Can you provide me list of TSAs by phases?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 247,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "Show me the breakdown of TSAs by duration." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 248,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "How many TSAs does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 249,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    TST.Title as [Phase],    COUNT(TC.ID) AS [# TSAs across different phases] FROM    TSAItems TC    LEFT JOIN TSAPhases TST ON TC.PhaseID = TST.ID  Group BY    TST.Title ", "List the number of TSAs across different phases." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 250,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    T.Title PendingTSAItems  FROM    TSAItems T    JOIN TSAStatuses TS ON T.TSAItemTSAStatusId = TS.ID  WHERE    T.AuthorId = '{Username}'    AND TS.[key] = 'ACTIVE' ", "How many pending TSA items do I have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 251,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    tb.Title as [Billing Period Name],    tb.DeadlineDate as [Contribution Deadlines]  FROM    TSABillingPeriods tb", "In Cost Tracking, how is my billing periods setup?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 252,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT    tp.Title AS [TSA Phases]  FROM TSAPhases tp", "What are the different TSA phases in my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 253,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ProviderLegalEntityId = L.ID where  L.ServiceLocation is not null UNION  SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ReceiverLegalEntityId = L.ID   where  L.ServiceLocation is not null ", "List the TSA service locations in my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 254,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     TSA.Title AS [TSAs],     s.Title   AS [TSA Statuses] FROM     TSAItems        TSA     LEFT JOIN         TSAStatuses S             ON TSA.TSAItemTSAStatusId = S.ID     LEFT JOIN         Functions   F             ON TSA.FunctionId = F.ID WHERE     F.Title = '{FunctionName}'", "List the TSAs for the Finance fucntion with their status.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 255,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "DECLARE @UserProfiles int; SELECT    @UserProfiles = ID  FROM    UserProfiles up WHERE    EMail = '{Username}'; WITH TEMPCTE AS (   SELECT      tr.FunctionId,      tr.SubFunctionId    FROM      TSAReviewers tr   WHERE      tr.ProviderApproverId = @UserProfiles      OR tr.RecipientApproverId = @UserProfiles )  SELECT    TSA.Title as [List of TSA to be reviewed] FROM    TSAItems TSA    JOIN TEMPCTE ON TSA.FunctionId = TEMPCTE.FunctionId    AND tsa.SubFunctionId = TEMPCTE.SubFunctionId    JOIN TSAPhases TP ON TSA.PhaseId = TP.ID  WHERE    TP.[Key] IN ('ALIGNMENT', 'APPROVAL'); ", "List the TSAs that I need to review." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 256,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT   tsa.Title DefaultCurrency FROM  TSACurrencies tsa WHERE tsa.CurrencyCode ='{CurrencyCode}'", "What is the default currency setting?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 257,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT   tsa.CurrencyExchangeRateToUSD  FROM  TSACurrencies tsa WHERE tsa.CurrencyCode ={CurrencyCode}", "List the exchange rates we setup for the default currency on my project.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 258,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    DISTINCT F.Title as [Functions To Be Configured] FROM    TSAReviewers TR    LEFT JOIN Functions F on F.ID = TR.FunctionId ", "In Reviewer Assignments, what functions require to be configured properly?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 259,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    DISTINCT SF.Title as [SubFunctions To Be Configured] FROM    TSAReviewers TR    LEFT JOIN SubFunctions SF on SF.ID = TR.SubFunctionId ", "In Reviewer Assignments, what sub-functions need to be configured for my project?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 260,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT CASE WHEN SUM(DeadlineReminder) > 0 THEN 'Yes, email reminder before the deadline is setup' ELSE 'No, email reminder before the deadline is setup' END AS [Send an email reminder before the deadline?] FROM( SELECT     tsa.Title,     CAST( tsa.DeadlineReminder AS INT) DeadlineReminder FROM     TSABillingPeriods tsa WHERE     DeadlineReminder = 1) A", "In Billing Periods, have we setup email reminder beofre deadline?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 261,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     tsa.Title        AS [Billing Period Name],     tsa.DeadlineDate AS [Contribution Deadlines] FROM     TSABillingPeriods tsa", "List the billing periods with their Contribution deadlines." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 262,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT CASE WHEN SUM(SendEmailImmediately) > 0 THEN 'Yes, notifying data contributors by email ' ELSE 'No, not notifying data contributors by email ' END AS [Notify data contributors by email ?] FROM( SELECT     tbp.Title                AS [Billing Period Name],     CAST(tbp.SendEmailImmediately AS INT) SendEmailImmediately FROM     TSABillingPeriods tbp WHERE     tbp.SendEmailImmediately = 1) A", "Are we not notifying data contributors by email for any billing periods?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 263,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     f.title   AS [Functions],     (t.TSAItemFTECostForServiceDurationUSD + t.MarkupOnFTECostUSD + t.ExternalMaterialCostUSD) AS [TSA Costs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND t.Duration IS NOT NULL", "What my TSA costs by  function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 264,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "-- Total TSA Costs over Month SELECT month_year AS [By Month],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Month] FROM vwTSACostWaterfall tcw GROUP BY date_,          month_year   -- Total TSA Costs over Quarter SELECT [quarter] AS [By Quarter],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Quarter] FROM vwTSACostWaterfall tcwf GROUP BY Concat(Year(date_), Datepart(quarter, date_)),          [quarter]   -- Total TSA Costs over Year SELECT [year] AS [By Year],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Year] FROM vwTSACostWaterfall vtcwf GROUP BY [year]", "What is my total TSA costs over time?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 265,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     TSA.Title AS [TSAs] FROM     TSAItems        TSA     LEFT JOIN         TSAStatuses S             ON TSA.TSAItemTSAStatusId = S.ID WHERE          S.Title<>'{TSAStatus}'", "How many TSAs left to sign?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 266,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     COUNT(TSA.ID) AS [# TSAs signed by Function],     F.Title       AS Functions FROM     TSAItems        TSA     LEFT JOIN         TSAStatuses S             ON TSA.TSAItemTSAStatusId = S.ID     LEFT JOIN         Functions   F             ON TSA.FunctionId = F.ID WHERE     S.Title = '{TSAStatus}' GROUP BY     F.Title", "List number of signed TSAs by function.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 267,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     [tcwf.year]                               AS [By Year],     ROUND(SUM(tcwf.fte_cost_per_month), 2)    AS [TSA FTE Cost for Selected Year],     ROUND(SUM(tcwf.nonfte_cost_per_month), 2) AS [TSA Non-FTE Cost for Selected Year] FROM     vwTSACostWaterfall tcwf WHERE      [tcwf.year] = '{Year}' GROUP BY     [tcwf.year]", "Break down the total TSA cost for this year by FTE and Non-FTE cost." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 268,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     tp.Title AS [TSA Stages] FROM     TSAPhases tp ORDER BY     Ordinal", "List out stages that are preconfigured on this instance for TSA lifecycle.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 269,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Stg.Title AS [Stage Name],        COUNT(TSA.ID) AS [# TSAs Identified] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID WHERE Stg.[Key] = 'IDENTIFICATION' GROUP BY Stg.Title", "How many TSA have been identified?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 270,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     ti.Title AS [TSAs that have duration for more than 18 months] FROM     TSAItems ti WHERE     ti.Duration > 18", "List out TSAs that have duration for more than 18 months." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 271,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     TSA.Title AS [TSAs that are ready to be approved] FROM     TSAItems      TSA     LEFT JOIN         TSAPhases Stg             ON TSA.PhaseId = Stg.ID WHERE     Stg.[Key] IN ( 'ALIGNMENT','APPROVAL') ", "List out TSAs are that are ready to be approved." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 272,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     f.title AS [Functions],     SUM((t.TSAItemFTECostForServiceDurationUSD + t.MarkupOnFTECostUSD + t.ExternalMaterialCostUSD) )AS [TSA Costs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND t.Duration IS NOT NULL GROUP BY  f.title", "What is the total of TSA cost by function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 273,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSAs with Open Risks] FROM TSAItems TSA     LEFT JOIN RisksAndIssues RI         ON TSA.ID = RI.TSAItemId     LEFT JOIN Statuses S         ON RI.ProgressId = S.ID WHERE S.[Key] NOT IN ( 'COMPLETED', 'CLOSED' )", "List out TSA with Open risks." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 274,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS [Buyer Side Reviewer for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.RecipientApproverID = UP.ID WHERE F.Title ='{FunctionName}'", "Who is the buy side reviewer for finance TSAs?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 275,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS [Buyer Side Reviewer for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.ProviderApproverId = UP.ID WHERE F.Title ='{FunctionName}'", "Who is the sell side reviewer for finance TSAs?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 276,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT UP.Title AS [Approver for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.RecipientApproverID = UP.ID WHERE F.Title = '{FunctionName}'  UNION   SELECT UP.Title AS [Approver for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.ProviderApproverId = UP.ID WHERE F.Title ='{FunctionName}'", "Who is the approver for finance TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 277,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT TSA.Title AS [TSAs with Behind Schedule Tasks Linked] FROM TSAItems TSA     LEFT JOIN Workplan WP         ON TSA.ID = WP.TSAItemId     LEFT JOIN Statuses S         ON WP.ProgressId = S.ID WHERE WP.WorkPlanTaskType = 'Task'       AND S.[Key] = 'BEHIND_SCHEDULE'", "List out TSAs with behind schedule tasks linked.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 278,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT f.title AS [Functions],        COUNT(t.ID) AS [TSAs Count],        (SUM(t.TSAItemFTECostForServiceDurationUSD) + SUM(t.MarkupOnFTECostUSD) + SUM(t.ExternalMaterialCostUSD)) AS [TSAs Costs] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN Functions f         ON t.FunctionId = f.id     LEFT OUTER JOIN TSADay1Dispositions TSADisp         ON t.TSADay1DispositionId = TSADisp.ID WHERE TSADisp.Title = 'rTSA (reverse TSA)'       AND s.title <> 'Canceled'       AND t.Duration IS NOT NULL GROUP BY f.title", "Show me the count and cost of reverse TSAs by function.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 279,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(TSA.ID) AS [TSA Items in Alignment phase for IT] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID WHERE Stg.[Key] = 'ALIGNMENT'       AND F.Title = '{FunctionName}'", "How many items are in the allignment phase for IT?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 280,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(TSA.ID) AS [# TSA items in the repository which have been modified in the last 7 days] FROM TSAItems TSA     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND CAST(TSA.Modified AS DATE)       BETWEEN CAST(GETDATE() - 7 AS DATE) AND CAST(GETDATE() AS DATE) ", "How many items in the repository have been modified in the last 7 days?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 281,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(TSA.ID) AS [# TSA items in the repository which have been created in the last 5 days] FROM TSAItems TSA     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND CAST(TSA.Created AS DATE)       BETWEEN CAST(GETDATE() - 5 AS DATE) AND CAST(GETDATE() AS DATE) ", "How many new items s in the repository have been created in the last 5 days?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 282,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT AVG(t.Duration) AS [Average Duration for IT TSAs],        (SUM(t.TSAItemFTECostForServiceDurationUSD) + SUM(t.MarkupOnFTECostUSD) + SUM(t.ExternalMaterialCostUSD)) AS [Cost for IT TSAs] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN Functions f         ON t.FunctionId = f.id WHERE s.title <> 'Canceled'       AND t.Duration IS NOT NULL       AND f.Title = 'IT' GROUP BY f.title", "What is the average duration and cost for IT TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 283,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSAs with Behind Schedule / At Risk Tasks Linked] FROM TSAItems TSA     LEFT JOIN Workplan WP         ON TSA.ID = WP.TSAItemId     LEFT JOIN Statuses S         ON WP.WorkPlanTaskStatusId = S.ID WHERE WP.WorkPlanTaskType = 'Task'       AND S.[Key] IN ('BEHIND_SCHEDULE','AT_RISK')", "Show me all TSAs that have a linked task that is At Risk or Behind Schedule." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 284,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSA Items that need review] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID WHERE Stg.[Key] IN ( 'ALIGNMENT', 'APPROVAL' )", "List all TSAs that need review." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 285,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title as [TSAs that need to be signed along with Item Owner], t.TSAItemPersonInCharge as [Item Owner] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]='SIGNED'", "List all TSAs that need to be signed along with the item owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 286,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [TSAs and rTSAs that have a markup greater than 5%]    ,TSADisp.Title AS [TSA / rTSA] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     	LEFT OUTER JOIN 	    TSADay1Dispositions TSADisp  		    ON t.TSADay1DispositionId=TSADisp.ID WHERE         s.title <> 'Canceled' 	  AND t.TSAItemMarkup >0.05", "Show me all TSAs and rTSAs that have a markup greater than 5%." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 287,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSAs are in a local currency that is not US dollar] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN TSACurrencies TSACurr         ON t.TSAItemLocalCurrencyId = TSACurr.ID     LEFT OUTER JOIN Currencies Curr         ON TSACurr.CurrencyID = Curr.ID WHERE Curr.Title <> 'USD ($)'       AND s.title <> 'Canceled'", "How many TSAs are in a local currency that is not US dollar?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 288,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title as [Inactive TSAs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]<>'ACTIVE' ", "List out all Inactive TSAs." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 289,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT t.Title as [TSAs that have duration less than 2 months] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]<>'CANCELED' AND t.Duration<2", "List All TSAs where the duration is less than 2 months." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 290,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT t.Title as [TSAs that have duration less than 2 months] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]<>'CANCELED' AND t.Duration>10", "List all TSAs where the duration is more than 10 months." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 291,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSAs due in next 45days] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id WHERE CAST(t.TSAItemEndDate AS DATE)       BETWEEN CAST(GETDATE() AS DATE) AND CAST(GETDATE() + 45 AS DATE)       AND s.title <> 'Canceled'", "List All TSAs due in next 45days." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 292,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [TSAs that have a markup above 1%] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     WHERE         s.title <> 'Canceled' 	  AND t.TSAItemMarkup >0.01", "List all TSAs where the markup is above 1%." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 293,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     t.Title AS [TSAs where Provider is not tagged] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE     s.title <> 'Canceled'     AND t.ProviderLegalEntityId IS NULL", "List all TSAs where provider is not tagged." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 294,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT    t.Title AS [TSAs where Receiver is not tagged] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     WHERE         s.title <> 'Canceled' 	  AND t.ReceiverLegalEntityId IS NULL", "List all TSAs where receiver is not tagged." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 295,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [Mixed Price Type TSAs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id   	LEFT OUTER JOIN 	PriceTypes Pric  	        ON t.TSAItemPriceTypeID=Pric.ID WHERE         s.title <> 'Canceled' 	  AND Pric.Title='Mixed price'", "List all mixed price type TSAs." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 296,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [List all TSAs which are not getting settled in USD]    FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     	LEFT OUTER JOIN 	    TSACurrencies TSACurr  		    ON t.TSAItemLocalCurrencyId=TSACurr.ID 	LEFT OUTER JOIN Currencies Curr 	        ON TSACurr.CurrencyID=Curr.ID WHERE         Curr.Title<>'USD ($)'  	  AND s.title <> 'Canceled'", "List all TSAs which are not getting settled n USD." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 297,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [TSAs where User has provided comments] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     WHERE         s.title <> 'Canceled' 	  AND t.Comments IS NOT NULL", "List all TSAs where User has provided comments." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 298,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     ISNULL(Stg.Title, 'No Phase Assigned') AS [Phase],     (TSA.Title)                          AS [ TSAs title] FROM     TSAItems      TSA     LEFT JOIN         TSAPhases Stg             ON TSA.PhaseId = Stg.ID", "List all TSAs per phase." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 299,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     t.Title AS [TSAs assigned to function due in next 30 days] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND f.Title='{FunctionName}' 	AND CAST(t.TSAItemEndDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND CAST(GETDATE()+30 AS DATE)", "List All TSAs assigned to IT due in next 30 days.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 300,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSAs assigned to IT due in next 30 days] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN Functions f         ON t.FunctionId = f.id WHERE s.title <> 'Canceled'       AND f.Title = 'Finance'       AND CAST(t.TSAItemEndDate AS DATE)       BETWEEN CAST(GETDATE() AS DATE) AND CAST(GETDATE() + 15 AS DATE)       AND t.TSAItemMarkup <= 0.01       AND t.ReceiverLegalEntityId IS NULL", "List All TSAs assigned to finance due in next 15 days where markup is below 1% and has no receiver tagged." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 301,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT t.Title AS [TSAs Items which are tagged as rTSA for Day 1 disposition] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN TSADay1Dispositions TSADisp         ON t.TSADay1DispositionId = TSADisp.ID WHERE s.title <> 'Canceled'       AND TSADisp.Title = 'rTSA (reverse TSA)'", "List all Items which are tagged as rTSA for Day 1 disposition.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 302,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSA Items where total number of FTEs providing service is greater than 5] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN TSADay1Dispositions TSADisp         ON t.TSADay1DispositionId = TSADisp.ID WHERE s.title <> 'Canceled'       AND t.TSAItemNoOfFTEProvidingService > 5", "List all Items where total number of FTEs providing service is greater than 5." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 303,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSA items which have been in Detailing phase for more than 15 days] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID WHERE Stg.[Key] = 'DETAILING'       AND CAST(TSA.Modified AS DATE) < CAST(GETDATE() - 15 AS DATE)", "List all items which have been in Detailing phase for more than 15 days." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 304,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [Finance related TSA items with relevant SLA details],        ISNULL(TSA.TSAItemSLAName, ' ') AS [SLA Details] FROM TSAItems TSA     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND F.Title = 'Finance'", "List all finance related items with relevant SLA details." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 305,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSA items where external cost is greater than FTE cost for entire duration] FROM TSAItems TSA     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND (TSA.TSAItemExternalMaterialCost > TSA.TSAItemFTECostForServiceDuration)", "List all items where external cost is greater than FTE cost for entire duration." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 306,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "What are the project teams that make up the governance structure for this engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 307,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many milestones does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 308,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "How many interdependencies does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 309,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "How many risks does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 310,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 311,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 312,
                column: "SuggestionText",
                value: "What is the difference between Progress and Calculated Status?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 313,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 315,
                column: "SuggestionText",
                value: "How do I add a client user to the PMO app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 316,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 317,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 318,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this {ProjectType} project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 319,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 321,
                column: "SuggestionText",
                value: "What are typical one-time costs that we should be considering?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 322,
                column: "SuggestionText",
                value: "What are the IT cost savings levers that I should be thinking about?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 323,
                column: "SuggestionText",
                value: "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 324,
                column: "SuggestionText",
                value: "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 325,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 326,
                column: "SuggestionText",
                value: "Help understand VC methodology.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 328,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 329,
                column: "SuggestionText",
                value: "What are the steps to setup the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 330,
                column: "SuggestionText",
                value: "What reports are available in the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 331,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 332,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 333,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 334,
                column: "SuggestionText",
                value: "Why are TSAs important?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 335,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 336,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 337,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 338,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 339,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 340,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this {ProjectType} project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 341,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 342,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key risks for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 343,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 344,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 345,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 346,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 347,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 348,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 349,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 350,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 351,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 352,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 353,
                column: "SuggestionText",
                value: "What are typical implications for cross border deals?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 354,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a normative operating model for the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 355,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are key considerations when defining an operating model for a {Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 356,
                column: "SuggestionText",
                value: "What are examples of Day 1 process dispositions?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 357,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 358,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 359,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 360,
                column: "SuggestionText",
                value: "Provides templates of TSA.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 361,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 362,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 363,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 364,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the key risks for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 365,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 366,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 367,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 1,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 2,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(*) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 4,
                column: "AnswerSQL",
                value: "SELECT wp.Title  from WorkPlan wp  LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 5,
                column: "AnswerSQL",
                value: "SELECT RI.Title  from RisksAndIssues RI  LEFT JOIN Statuses s on RI.ItemStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 7,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 8,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 10,
                column: "AnswerSQL",
                value: "SELECT     pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueMilestoneCount FROM      WorkPlan wp INNER JOIN      ProjectTeams pt ON wp.ProjectTeamId = pt.ID LEFT JOIN      statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE      wp.WorkPlanTaskType = 'Milestone'     AND wp.TaskDueDate < GETDATE()     AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY      OverdueMilestoneCount DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 11,
                column: "AnswerSQL",
                value: "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "AnswerSQL",
                value: "SELECT      RiskImpact,     RiskProbability,     COUNT(*) AS RiskCount FROM      RisksAndIssues WHERE      IssueRiskCategory = 'Risk'     AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))  AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 37,
                column: "AnswerSQL",
                value: "SELECT  COUNT(1) [HighRiskItemCount] FROM RisksAndIssues R JOIN ProjectTeams PT ON PT.ID = R.ProjectTeamId WHERE  RiskImpact = 'High'  AND IssueRiskCategory = 'Risk' AND PT.Title ={ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team] FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID JOIN WeeklyStatusStatuses WS ON WS.ID = PSE.WeeklyStatusStatusId WHERE  RP.ID = (SELECT ID  FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND WS.[KEY] IN ('AT_RISK','BEHIND_SCHEDULE')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT   W.ID,W.Title FROM Workplan W LEFT JOIN Workstreams WS On  W.WorkstreamId=Ws.ID WHERE WS.Title LIKE {Workstream}", "How can I export workplan for just Finance workstream from PMO app?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " SELECT COUNT(WR.ID) Count,'Risks' as Category FROM Workplan W JOIN WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WR On WR.WorkPlanId = W.ID LEFT JOIN RisksAndIssues RI ON RI.ID = WR.RisksAndIssueId WHERE RI.IssueRiskCategory = 'Risk'  UNION ALL SELECT COUNT(WR.ID) Count, 'Issues' as Category FROM Workplan W JOIN WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WR On WR.WorkPlanId = W.ID LEFT JOIN RisksAndIssues RI ON RI.ID = WR.RisksAndIssueId  WHERE RI.IssueRiskCategory = 'Issue'  UNION ALL SELECT COUNT(WA.ID) Count, 'Actions' as Category  FROM Workplan W JOIN WorkPlansToActionsForAssociatedActions WA On WA.WorkPlanId = W.ID  UNION ALL SELECT    COUNT(WD.ID) Count,    'Decisions' as Category  FROM    Workplan W    JOIN WorkPlansToDecisionsForAssociatedDecisions WD On WD.WorkPlanId = W.ID", "How many Risks, Issues, Actions and Decisions linked to Workplan Tasks?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT WSS.Title StatusValue ,'CurrentStatus' as Category FROM ProjectStatusEntries PSE LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT TOP 1 ReportingPeriods.ID FROM ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC) AND PT.Title like {ProjectTeam}  UNION SELECT WSS.Title StatusValue ,'PreviousStatus' as Category FROM ProjectStatusEntries PSE  LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT  ReportingPeriods.ID FROM   ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC Offset 1 Rows FETCH NEXT 1 ROWS ONLY) AND  PT.Title like {ProjectTeam}", "What's prior week and current week status for Finance function?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    RI.UniqueItemIdentifier,    RI.Title,    'Risks' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND RI.IssueRiskCategory = 'Risk'    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    RI.UniqueItemIdentifier,    RI.Title,    'Issues' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND RI.IssueRiskCategory = 'Issue'    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    A.UniqueItemIdentifier,    A.Title,    'Actions' as Category  FROM    Actions A    JOIN Statuses S ON A.ItemStatusId = S.ID    JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    D.UniqueItemIdentifier,    D.Title,    'Decisions' as Category  FROM    Decisions D    LEFT JOIN Statuses S ON D.ItemStatusId = S.ID      JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all Open Risks, Issues, Actions and Decisions in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as OpenInterdependencies, S.Title as Status FROM Interdependencies I LEFT JOIN InterdependencyStatuses S ON I.InterdependencyStatusId = S.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'Canceled') ", "List all Open Interdependencies." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE as CriticalTasks  FROM    Workplan W      JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    W.IsCritical = 1   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all critical workplan tasks in PMO app.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as ITIntProvider FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ProviderProjectTeamID WHERE PT.Title = {ProjectTeam} ", "List all Interdependencies where IT is Interdependency Provider." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as ITIntReceiver FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ReceiverProjectTeamId WHERE PT.Title = {ProjectTeam} ", "List all Interdependencies where IT is Interdependency Receiver.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    COUNT(W.ID) as CountofCriticalPath   FROM    Workplan W      JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    W.IsCritical = 1   AND TT.[Key] = 'PROJECT_MANAGEMENT'   and W.WorkPlanTaskType = 'Milestone'", "How many critical milestones in workplan in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    COUNT(W.ID) AS TasksDueIn30Days  FROM    Workplan W    LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID        JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    CAST(W.TaskDueDate AS DATE) <= CAST(GETDATE() + 30 AS DATE)    AND CAST(W.TaskDueDate AS DATE) >= CAST(GETDATE() AS DATE)    AND S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "How many tasks are due in next 30 days in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  W.UniqueItemIdentifier,W.Title TasksWithoutOwner FROM  WorkPlan W JOIN ProjectTeams PT On W.WorkstreamId = PT.ID  JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  W.TaskOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List any workplan items that do not have an owner assigned in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  RI.UniqueItemIdentifier,RI.Title ItemsWithoutOwner, IssueRiskCategory AS Category FROM  RisksAndIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID  WHERE  RI.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT  A.UniqueItemIdentifier,A.Title  ItemsWithoutOwner, 'Actions' AS Category FROM  Actions A LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  A.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'  UNION  SELECT  D.UniqueItemIdentifier,D.Title  ItemsWithoutOwner, 'Decisions' AS Category FROM  Decisions D LEFT JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  D.ItemOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List any RAID log items that do not have an owner assigned in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForProviderTasks WTP ON WTP.InterdependencyId = I.ID WHERE WTP.ID IS NULL  UNION  SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForReceiverTasks WTR ON WTR.InterdependencyId = I.ID WHERE WTR.ID IS NULL;", "List interdependencies that do not have a provider or receiver task linked." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT      UP.Title AS UserName  FROM      WorkPlan W JOIN UserProfiles UP ON UP.ID = W.TaskOwnerId JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      S.[Key] IN ('AT_RISK', 'BEHIND_SCHEDULE')     AND UP.Title IS NOT NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'", "Show me a list of all users that have behind schedule or at risk items assigned to them in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title RisksAndIssues, IssueRiskCategory FROM RisksAndIssues RI LEFT JOIN Statuses S ON S.ID = RI.ItemStatusId WHERE IsCritical IN (1) AND S.[Key] NOT IN ('CLOSED')", "Show me all Risks and issues that are flagged as Critical and not complete." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID)) ROW_NUM FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      CAST(W.Modified AS DATE) BETWEEN DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND PT.ItemIsActive=1 AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY      pt.title) A", "Which functions has made the least # of updates to their workplan in last week in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      W.Title AS [List of Critical Milestones due in next 15 days with no owners Assigned yet] FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      WorkPlanTaskType IN ('Milestone')     AND CAST(TaskDueDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 15, CAST(GETDATE() AS DATE))     AND IsCritical IN (1)     AND TaskOwnerId IS NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'", "Can you please list down all milestones due in next 15 days which are on critical path and have no owners assigned to them in PMO app??" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title as [List down all tasks associated with interdependencies] from Workplan where ID in ( SELECT WorkPlanId FROM WorkPlansToInterdependenciesForReceiverTasks UNION select WorkPlanId FROM WorkPlansToInterdependenciesForProviderTasks) AND WorkPlanTaskType='Task'", "Can you please list down all tasks associated with interdependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForReceiverTasks WPRT ON W.id = WPRT.WorkPlanId LEFT JOIN Interdependencies IR ON WPRT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task' UNION SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForProviderTasks WPPT ON W.id = WPPT.WorkPlanId LEFT JOIN Interdependencies IR ON WPPT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task'", "Can you please list down all tasks whose due date is less than the due date of the associated interdependency?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT W.Title AS WorkPlanItem FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT'  AND DATEPART(ww,TaskDueDate) = DATEPART(ww,GETDATE())", "List out workplan items due this week in PMO app." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT W.Title, W.WorkPlanTaskType, W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title [Status], UP.Title TaskOwner FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE UniqueItemIdentifier = 'HR.2.2.6'", "Provide details for workplan ID HR.2.2.6." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(RI.ID) IssueCoutnwithPendingStatus FROM RisksandIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN RiskAndIssueStatuses RS ON RI.ItemStatusID = RS.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE IssueRiskCategory = 'Issue' AND TT.[Key] = 'PROJECT_MANAGEMENT' AND RS.Title = 'Pending'", "How many issues are in pending status in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	UniqueItemIdentifier 	,Title 	,IssueRiskCategory FROM RisksAndIssues WHERE  	ItemOwnerId IS NULL  UNION ALL  SELECT  	UniqueItemIdentifier 	,Title 	,'Decision' FROM Decisions WHERE  	ItemOwnerId IS NULL  UNION ALL  SELECT  	UniqueItemIdentifier 	,Title 	,'Action' FROM Actions WHERE  	ItemOwnerId IS NULL", "List RAID items that do not have an owner assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId   LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId   LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)    AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all Workplan tasks that are 'Not Started' and planned start date has passed in PMO app.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId    LEFT JOIN ProjectTeams PT on PT.ID = W.ProjectTeamId   LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)    AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all Workplan tasks which are past due in PMO app.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT PT.Title as ProjectTeams_With_Items_At_Risk  FROM ProjectTeams PT    JOIN Workplan W ON W.ProjectTeamID = PT.ID    JOIN Statuses S ON S.ID = W.WorkplanTaskStatusID    JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[Key] = 'AT_RISK'   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "List all project teams with workplan items that are 'At Risk' in PMO app.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title FROM ProjectTeams WHERE ManageProjectStatus = 1 EXCEPT SELECT PT.Title FROM ProjectStatusEntries PSE LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodId = RP.ID LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamId = PT.ID WHERE RP.ID = (SELECT ID FROM ReportingPeriods 				WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE) 					) 	AND PSE.WeeklyStatusStatusId IS NOT NULL", "Which teams have not entered thier weekly status report for this reporting period?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  COUNT(R.ID) AS NewRisksCount FROM  RisksAndIssues R JOIN ProjectTeams PT ON R.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  IssueRiskCategory = 'Risk' AND DATEDIFF(day, R.Created, GETDATE()) <= 5 AND TT.[Key] = 'PROJECT_MANAGEMENT'", "How many new risks have been created in the last 5 days in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title Workplan FROM (     SELECT          DISTINCT W.Title, WorkPlanLinksTargetId AS TaskID     FROM          WorkPlanLinks WL         LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksTargetId      UNION      SELECT          DISTINCT W.Title, WorkPlanLinksSourceId AS TaskID     FROM          WorkPlanLinks WL         LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksSourceId ) AS SubQuery ORDER BY TaskID", "Which tasks have a predecessor or successor linked to them?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID) DESC) ROW_NUM FROM      RisksAndIssues RI JOIN ProjectTeams PT ON PT.ID = RI.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE PT.ItemIsActive=1 and RI.IssueRiskCategory = 'Risk' AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY      pt.title) A", "Which function has the highest number of risk associated with in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      W.Title AS [List of Milestones due in next 7 days and Not Updated Since Last 2 weeks] FROM      WorkPlan W JOIN Statuses S on s.ID = W.WorkPlanTaskStatusId JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      W.WorkPlanTaskType IN ('Milestone')     AND W.IsCritical IN (1)     AND CAST(W.Modified AS DATE) < DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND S.[Key] not in ('COMPLETED', 'CANCELLED') AND TT.[Key] =  'PROJECT_MANAGEMENT'", "Can you please list down all milestones due in next 7 days which are on critical path and have no update made to them in last 2 weeks in PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     RI.TITLE AS [Risk items that does not have Risk Mitigation plan] FROM     RisksAndIssues RI WHERE     RI.IssueRiskCategory = 'Risk'     AND RI.RiskMitigation IS NOT NULL", "Are there any risks items that doesn't have risk mitigation plan?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(R.ID) [Reports available in PMO] FROM Reports R JOIN MenuOptions M ON R.ParentMenuItemId = M.ID WHERE M.[Key] = 'PROJECT_ANALYTICS' AND R.ReportIsActive = 1", "How many Analytics reports are available for PMO App?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT      A.*,     B.StartOfWeek,     B.EndOfWeek FROM (     SELECT TOP 1         COUNT(*) AS NumberOfItemsDue,         DATEPART(week, TaskDueDate) AS WeekNumber     FROM          WorkPlan      LEFT JOIN          Statuses S ON S.ID = WorkPlan.WorkPlanTaskStatusId     WHERE          TaskDueDate IS NOT NULL AND          S.[key] <> 'COMPLETE'     GROUP BY          DATEPART(week, TaskDueDate)     ORDER BY          COUNT(*) DESC ) AS A INNER JOIN (     SELECT          DATEADD(week, DATEDIFF(week, 0, TaskDueDate), 0) AS StartOfWeek,         DATEADD(week, DATEDIFF(week, 0, TaskDueDate) + 1, 0) - 1 AS EndOfWeek,         DATEPART(week, TaskDueDate) AS WeekNumber     FROM          WorkPlan      LEFT JOIN          Statuses S ON S.ID = WorkPlan.WorkPlanTaskStatusId     WHERE          TaskDueDate IS NOT NULL AND          S.[key] <> 'COMPLETE'     GROUP BY          DATEPART(week, TaskDueDate),          DATEADD(week, DATEDIFF(week, 0, TaskDueDate), 0),          DATEADD(week, DATEDIFF(week, 0, TaskDueDate) + 1, 0) ) AS B ON B.WeekNumber = A.WeekNumber", "What week has the most workplan items due across the project?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT      A.*,     B.StartOfWeek,     B.EndOfWeek FROM (     SELECT TOP 1         COUNT(*) AS NumberOfItemsDue,         DATEPART(week, TaskDueDate) AS WeekNumber     FROM          WorkPlan      LEFT JOIN          Statuses S ON S.ID = WorkPlan.WorkPlanTaskStatusId     WHERE          TaskDueDate IS NOT NULL AND          S.[key] <> 'COMPLETE'     GROUP BY          DATEPART(week, TaskDueDate)     ORDER BY          COUNT(*) DESC ) AS A INNER JOIN (     SELECT          DATEADD(week, DATEDIFF(week, 0, TaskDueDate), 0) AS StartOfWeek,         DATEADD(week, DATEDIFF(week, 0, TaskDueDate) + 1, 0) - 1 AS EndOfWeek,         DATEPART(week, TaskDueDate) AS WeekNumber     FROM          WorkPlan      LEFT JOIN          Statuses S ON S.ID = WorkPlan.WorkPlanTaskStatusId     WHERE          TaskDueDate IS NOT NULL AND          S.[key] <> 'COMPLETE'     GROUP BY          DATEPART(week, TaskDueDate),          DATEADD(week, DATEDIFF(week, 0, TaskDueDate), 0),          DATEADD(week, DATEDIFF(week, 0, TaskDueDate) + 1, 0) ) AS B ON B.WeekNumber = A.WeekNumber", "What week has the most workplan items due across the project?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT      AccomplishmentsAndNextSteps.Title FROM     ProjectStatusEntries     LEFT OUTER JOIN AccomplishmentsAndNextSteps ON ProjectStatusEntries.ID = AccomplishmentsAndNextSteps.ProjectStatusEntryId     LEFT JOIN ProjectTeams PT ON PT.ID = ProjectStatusEntries.ProjectTeamId     LEFT OUTER JOIN ReportingPeriods ON ProjectStatusEntries.ReportingPeriodId = ReportingPeriods.ID  WHERE     AccomplishmentNextStepCategory IN '{AccomplishmentNextStepCategory}' AND ReportingPeriods.PeriodStartDate = (         SELECT TOP 1 PeriodStartDate          FROM ReportingPeriods          WHERE CAST (GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE)      ) ", "CE4-PMO", "What are the key action items scheduled for next week?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH CTE AS (SELECT COUNT(*) AS OverdueItemCount,        PT.Title AS [ProjectTeam With Most Overdue Item] FROM WorkPlan W LEFT JOIN [ProjectTeams] PT ON PT.ID = W.ProjectteamId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId WHERE  	  S.[Key] <> '{Key}'       AND CAST(W.TaskDueDate AS DATE ) < CAST(GETDATE() AS DATE) 	  AND PT.Title IS NOT NULL	 GROUP BY PT.Title ) SELECT Top 1 [ProjectTeam With Most Overdue Item] FROM CTE ORDER BY OverdueItemCount DESC", "CE4-PMO", "Which function has the most overdue items?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "What are our targets for this engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '{Username}' GROUP BY S.Title ", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '{Username}' ", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '{Username}' ", "How  many initiatives are assigned to me?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '{Username}' ", "Give my cost reduction initiatives.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(RevenueGrowthEstimate,0)) [RevenueGrowthTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title = {ProjectTeam}", "What are the revenue growth targets for Sales & Marketing?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(CostToAchieveEstimate,0)) [CostToAchieveTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title ={ProjectTeam}", "What are the cost to achieve targets for R&D?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT S.Title [Value Capture Stage] , COUNT(VI.ID) [No. of Initiatives] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} GROUP BY S.Title", "How many initiatives are there in IT across different stages?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Approved Initiaves] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} AND S.Title = 'Approved'", "List IT initiatives that are in approved stage.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiaves with Workplan] FROM ValueCaptureInitiatives VI JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WV ON VI.ID = WV.ValueCaptureInitiativeId", "List initiatives that have workplan item linked to them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT VI.Title [Initiaves with Risk] FROM ValueCaptureInitiatives VI JOIN RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RV ON VI.ID = RV.ValueCaptureInitiativeId LEFT JOIN RisksAndIssues RI ON RI.ID = RV.RisksAndIssueId WHERE RI.IssueRiskCategory = 'Risk'", "How many initiatives have Risks linked?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) [HeadCountCostReductionTarget] FROM [ValueCaptureTopDownEstimates] VT", "What is the total headcount cost reduction target?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Non Active Initiaves] FROM ValueCaptureInitiatives VI WHERE ISNULL(VI.IsItemActive ,0) = 0", "List initiatives that are not active." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages  VCS ON VCI.ValueCaptureStageId = VCS.ID WHERE VCS.Title = {ValueCaptureStage} ", "How many initiatives have been realized?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(NonHeadcountCostReductionEst) + SUM(RevenueGrowthEstimate) + SUM(HeadcountCostReductionEst) 'Total Top Down Target Value' FROM ValueCaptureTopDownEstimates", "What is the Total Top Down Target Value?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(RevenueGrowthEstimate) RevenueGrowth,SUM(TotalCostReduction) CostReduction FROM ValueCaptureTopDownEstimates", "Show my top down target values by cost reduction and revenue growth." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(ID) FROM ValueCaptureInitiatives WHERE IsItemActive = 1", "How many active initatives are there in my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title [Project Team] ,CAST(SUM(ISNULL(RevenueGrowthEstimate,0))/1000000 AS FLOAT) [Revenue Growth(in Million)] FROM [ValueCaptureTopDownEstimates] VT 	LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId GROUP BY PT.Title", "What is the total of revenue growth by team?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VC.Title AS [ValueCaptureInitiatives with no Owners] FROM ValueCaptureInitiatives VC WHERE ItemOwnerId IS NULL", "List out initiatives with no owners assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiative with Workplan at Risk] FROM WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WTVI LEFT JOIN Workplan W ON W.ID = WTVI.WorkPlanId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId LEFT JOIN ValueCaptureInitiatives VI on VI.ID = WTVI.ValueCaptureInitiativeId WHERE S.[KEY] = 'AT_RISK'", "List out initiatives that have at risk workplan task linked." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VP.Title AS ValueCapturepriority, COUNT(VC.ValueCapturePriorityId) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId where ValueCapturePriorityId is not null GROUP BY ValueCapturePriorityId, VP.Title", "Show me initiatives count by priority." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS UserProfile, COUNT(VC.ID) AS ValueCaptureOwnerwiseCount FROM ValueCaptureInitiatives VC LEFT JOIN UserProfiles UP ON UP.ID = VC.ItemOwnerId WHERE ItemOwnerId IS NOT NULL GROUP BY ItemOwnerId, UP.Title", "Show me initiatives count by owner.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Initiaves] FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Title = '{Username}'", "List initiatives where I'm assigned as the Owner.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(VCI.ID) as IdentifiedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Identified'", "How many initiatives are identified?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ApprovedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Approved' ", "How many initiatives have been approved?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ValidatedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Validated' ", "How many initiatives have been validated?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS [Value Capture Stage] ,COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCI.ValueCaptureStageId = VCS.ID GROUP BY VCS.Title", "Can you provide breakdown of the initiatives across various stages?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}'", "What initatives have I been assigned to?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VS.Title ValueCaptureStage FROM ValueCaptureInitiatives VI JOIN ValueCaptureStages VS ON VI.ValueCaptureStageID = VS.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}' ORDER BY ValueCaptureStage", "List my initatives by stages.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VT.Title ValueCaptureType FROM ValueCaptureInitiatives VI JOIN ValueCaptureTypes VT ON VI.ValueCaptureStageID = VT.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}' ORDER BY ValueCaptureType", "List my initatives by Value Capture Type.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title,SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(RevenueGrowthEstimate,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)) AS TotalTopDownTarget FROM ValueCaptureTopDownEstimates LEFT JOIN ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY PT.Title", "What is the top-down target for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS ValueCaptureStage, COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VC.ValueCaptureStageId GROUP BY ValueCaptureStageId, VCS.Title", "Show me initiatives count by stage.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT F.Title as Functions, COUNT(VC.FunctionId) AS ValueCaptureFunction FROM ValueCaptureInitiatives VC LEFT JOIN Functions f ON f.ID = VC.FunctionId WHERE FunctionId IS NOT NULL GROUP BY FunctionId, F.Title ", "Show me initiatives count by function.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT     CC.Title AS [Cost/Revenue Centers linked to PnL Line Items], CC.Account FROM CostCenters CC     LEFT JOIN ValueCaptureFinancialLineItems Fin         ON CC.FinancialLineItemId = Fin.ID     LEFT JOIN ValueCaptureFinancialLineItemTypes FinType         ON Fin.FinancialLineItemTypeId = FinType.ID WHERE FinType.Title = 'PnL'", "List all Cost/Revenue Centers linked to PnL Line Items." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(ID) AS [Top Down targets #],        SUM(ISNULL(NonHeadcountCostReductionEst, 0)) AS [Total Non Headcount Cost Reduction Estimate],        SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Revenue Growth Estimate],        SUM(ISNULL(CostToAchieveEstimate, 0)) AS [Total Cost To Achieve Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) AS [Total Headcount Cost Reduction Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates ", "Can you provide Top Down Targets KPIs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  VCTT.Title, PT.Title, VCTT.CostToAchieveEstimate, VCTT.NonHeadcountCostReductionEst, VCTT.HeadcountCostReductionEst, VCTT.RevenueGrowthEstimate FROM   ValueCaptureTopDownEstimates VCTT  Left Join ProjectTeams PT on PT.ID = VCTT.ProjectTeamId", "How many top down targets do we have for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) AS HeadcountCostReduction FROM ValueCaptureTopDownEstimates", "What is the Total Headcount Cost Reduction Target?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  SUM(ISNULL(NonHeadcountCostReductionEst,0)) AS NonHeadcountCostReduction FROM  ValueCaptureTopDownEstimates", "What is the Total Non Headcount Cost Reduction Target?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  SUM(ISNULL(RevenueGrowthEstimate,0)) AS RevenueGrowth FROM  ValueCaptureTopDownEstimates", "What is the Total Revenue Growth Target?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  SUM(ISNULL(CostToAchieveEstimate,0)) AS CostToAchive FROM  ValueCaptureTopDownEstimates", "What is the Total Cost to Achieve Target?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  PT.Title ProjectTeams, VBM.Title Benchmark FROM  ValueCaptureCostBenchmarks VBM LEFT JOIN  ProjectTeams PT ON PT.ID = VBM.ProjectTeamId", "Are there any benchmarks for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Amount as [Client Baseline PnL Amount], A.FinancialLineItem as FinancialLineItem FROM( SELECT F.ID, F.Ordinal, F.Title FinancialLineItem , SUM(C.Amount)/1000000 Amount FROM ValueCaptureFinancialLineItems F LEFT JOIN CostCenters C ON C.FinancialLineItemId = F.ID  GROUP BY F.ID, F.Title, F.Ordinal ) A WHERE A.Amount IS NOT NULL", "What is my program Client Baseline PnL?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     Title AS [Recommended Initiatives List] FROM     ValueCaptureRecommendedInitiatives", "List all recommended initiatives for my program.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 	SUM(a.[Total Top Down Target Value]) AS [Top Down Targets],      SUM(b.[Bottom Up Initiatives Value]) AS [Bottom Up Initiatives Value], 	( SUM(b.[Bottom Up Initiatives Value])-SUM(a.[Total Top Down Target Value])) AS Variance FROM ( SELECT   SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates) a, ( select SUM(Amount)*12 as [Bottom Up Initiatives Value]  FROM vwUnpivotEstimates where MYear='Y3M12' AND Recurring =1 ) b ", "Compare Top Down Targets vs Bottom Up Initiatives for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "   SELECT ISNULL(SUM(Act.AMOUNT),0.0) AS [Cumulative One Time Cost Amount] 			   FROM vwUnpivotActuals Act 			   LEFT JOIN ValueCaptureImpactTypes VCIT ON Act.ValueCaptureImpactTypeID=VCIT.ID 			   LEFT JOIN ValueCaptureInitiatives VCI ON Act.ValueCaptureInitiativeID=VCI.ID 			   WHERE VCIT.PositiveOrNegativeValues='Negative' 			   AND Act.Recurring<>1 			   AND VCI.Title = '{ValueCaptureInitiative}'", "What is cumulative one-time cost for all initiatives?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "  SELECT  	   WP.Title AS [Workplan Items Linked to Value Capture Initiatives]  	   FROM WorkPlan WP 	   LEFT JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WPVC  	             ON WP.ID=WPVC.WorkPlanId 	   LEFT JOIN ValueCaptureInitiatives VCI  	             ON VCI.ID=WPVC.ValueCaptureInitiativeId", "List all workplan items linked to VC initiatives." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "WITH EstimatedRunRates AS (     SELECT          E.ValueCaptureInitiativeId,  B.Title AS Initiative,          ISNULL(SUM(E.Amount),0) * 12 AS EstimatedAnnualizedRunRate     FROM          vwUnPivotEstimates E     JOIN          ValueCaptureInitiatives B ON E.ValueCaptureInitiativeId = B.ID     WHERE          E.Recurring = 1     GROUP BY          E.ValueCaptureInitiativeId, B.Title ), ActualRunRates AS (     SELECT          A.ValueCaptureInitiativeId, B.Title AS Initiative,          SUM(ISNULL(A.Amount, 0)) * 12 AS ActualAnnualizedRunRate     FROM          vwUnpivotActuals A     JOIN          ValueCaptureInitiatives B ON A.ValueCaptureInitiativeId = B.ID     WHERE          A.Recurring = 1      GROUP BY          A.ValueCaptureInitiativeId, B.Title )   SELECT Initiative AS [List of Initiatives with 10% differ between Estimate and Actual] FROM ( SELECT      E.ValueCaptureInitiativeId,E.Initiative,     E.EstimatedAnnualizedRunRate,     A.ActualAnnualizedRunRate,    IIF(E.EstimatedAnnualizedRunRate = 0 ,0 , (A.ActualAnnualizedRunRate - E.EstimatedAnnualizedRunRate) / E.EstimatedAnnualizedRunRate * 100) AS PercentageDifference FROM      EstimatedRunRates E JOIN      ActualRunRates A ON E.ValueCaptureInitiativeId = A.ValueCaptureInitiativeId ) A WHERE      PercentageDifference > 10", "List all initiatives that had an estimate annualized run-rate differed by more than 10% from the Actual annualized run-rate." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     CONCAT('Tracking Period of the Program is from ', CAST(MIN(StartDt) AS DATE), ' to ', CAST(MAX(EndDt) AS DATE)) FROM     ValueCaptureTransactionMonths WHERE     IsItemActive = 1", "What's the tracking period for my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = '{ProjectTeam}' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = '{ProjectTeam}'    ) Act         ON 1 = 1", "What's the variance for Legal function?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "	SELECT   	    COUNT(VCI.ID) AS [Initiative Count for Selected Evaluator Quad]  		     FROM ValueCaptureInitiatives VCI 		LEFT JOIN VCInitiativeEvaluatorQuadInfo VCQ  		    ON VCI.VCInitiativeEvaluatorQuadInfoId=VCQ.ID 		       WHERE VCQ.Title= {EvaluatorQuad}", "Based on Evaluator comparison, how many initiatives have 'High Benefit, High Complexity'?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = '{ProjectTeam}' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = '{ProjectTeam}'     ) Act         ON 1 = 1", "What is my variance for the IT initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title Initiatives FROM ValueCaptureInitiatives WHERE ValueCapturePriorityId = 1", "List all initiatives that requires attention.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT ISNULL(SUM(Est.Amount), 0.0) AS Estimates,        ISNULL(SUM(Act.Amount), 0.0) AS Actuals,        ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = 'Finance'           and VCI.Title = 'Initiative 1'           and Est.MYear = 'Y1M1' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = 'Finance'               AND VCI.Title = 'Initiative 1'               AND MYear = 'Y1M1'     ) Act         ON 1 = 1", "What is my Y1M1 Estimates, Actuals and Variance for Finance - [Initiative 1]?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "WITH EstimatedRunRates AS (     SELECT          E.ValueCaptureInitiativeId,  B.Title AS Initiative,          ISNULL(SUM(E.Amount),0) * 12 AS EstimatedAnnualizedRunRate     FROM          vwUnPivotEstimates E     JOIN          ValueCaptureInitiatives B ON E.ValueCaptureInitiativeId = B.ID 	LEFT JOIN ValueCaptureTransactionMonths VT ON VT.[Key] = E.MYear      WHERE          E.Recurring = 1 		AND (CAST( DATEADD(MM, 3 , GETDATE()) AS DATE) BETWEEN VT.StartDt AND VT.EndDt)     GROUP BY          E.ValueCaptureInitiativeId, B.Title ), ActualRunRates AS (     SELECT          A.ValueCaptureInitiativeId, B.Title AS Initiative,          SUM(ISNULL(A.Amount, 0)) * 12 AS ActualAnnualizedRunRate     FROM          vwUnpivotActuals A     JOIN          ValueCaptureInitiatives B ON A.ValueCaptureInitiativeId = B.ID  	LEFT JOIN ValueCaptureTransactionMonths VT ON VT.[Key] = A.MYear      WHERE          A.Recurring = 1  		AND (CAST( DATEADD(MM, 3 , GETDATE()) AS DATE) BETWEEN VT.StartDt AND VT.EndDt)     GROUP BY          A.ValueCaptureInitiativeId, B.Title )  SELECT Initiative AS [List of Initiatives with 10% differ between Estimate and Actual] FROM ( SELECT      E.ValueCaptureInitiativeId,E.Initiative,     E.EstimatedAnnualizedRunRate,     A.ActualAnnualizedRunRate,    IIF(E.EstimatedAnnualizedRunRate = 0 ,0 , ABS(A.ActualAnnualizedRunRate - E.EstimatedAnnualizedRunRate) / E.EstimatedAnnualizedRunRate * 100) AS PercentageDifference FROM      EstimatedRunRates E JOIN      ActualRunRates A ON E.ValueCaptureInitiativeId = A.ValueCaptureInitiativeId ) A WHERE      PercentageDifference > 10", "List initiatives that had an Estimate ARR that differed by 10% (higher or lower) from the Actual ARR for previous month." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS TotalNetSales FROM ValueCaptureEstimates VE LEFT JOIN ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId WHERE FLT.Title = 'PnL' -- pnl 	AND FL.Title in ('Net sales') -- net total", "In the PnL, what is my net sales?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS TotalOperatingPerformance FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'PnL'  AND FLG.Title in ('Total Operating Performance') ", "In the PnL, what is my total operating performance?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS GrossProfit FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'PnL'  AND FLG.Title in ('Gross Profit') ", "In the PnL, what is my gross profit?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [EBITA & Net Income] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'PnL' AND FLG.Title in ('EBITDA','Net Income') ", "In the PnL, what is my EBITDA and Net Income?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [Total Assets] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'Balance Sheet'  AND FLG.Title in ('Total Current Assets','Total Non-Current Assets') ", "In the Balance Sheet, what is my Total Assets?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [Total Liabilities] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'Balance Sheet'  AND FLG.Title in ('Total Current Liabilities','Total Non-Current Liabilities') ", "In the Balance Sheet, what is my Total Liabilities?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS [Total Equity] FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN  	vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN 	ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId LEFT JOIN 	ValueCaptureFinancialLineItemGroups FLG ON FLG.ID = FL.FinancialLineItemGroupId WHERE      FLT.Title = 'Balance Sheet'  AND FLG.Title in ('Equity') ", "In the Balance Sheet, what is my Total Equity?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  	SUM(NonHeadcountCostReductionEst) NonHeadcountCostReduction, 	SUM(HeadcountCostReductionEst) HeadcountCostReduction FROM ValueCaptureTopDownEstimateS", "What is the total of headcount and non-headcount related cost reduction?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH CTE AS  ( 	SELECT ProjectTeamID  	FROM ValueCaptureEstimates E 		LEFT JOIN ValueCaptureInitiatives I ON I.ID = E.ValueCaptureInitiativeId 	EXCEPT 	SELECT ProjectTeamID FROM [ValueCaptureTopDownEstimates] )  SELECT PT.Title [ProjectTeam]  FROM CTE LEFT JOIN ProjectTeams PT ON PT.ID = CTE.ProjectTeamID", "List out teams that have bottom-up estimates but missing top-down targets.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " DECLARE @CurrentYR NVARCHAR(10), @SQL NVARCHAR(MAX)  SELECT  TOP 1 @CurrentYR = [YEAR] FROM vwValueCaptureTransactionMonths WHERE RelativeYear = YEAR(GETDATE())   SELECT @SQL = ' ;WITH TopDownCTE AS ( 	SELECT  		PT.ID  PROJECTTEAMID 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(RevenueGrowthEstimate,0))  + SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM [ValueCaptureTopDownEstimates] VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 	GROUP BY  		PT.ID  		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(RevenueGrowthEstimate,0))  + SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM [ValueCaptureTopDownEstimates] VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 ) , BottomUPCTE AS ( 	SELECT  		VI.PROJECTTEAMID 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 	GROUP BY  		VI.PROJECTTEAMID 		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 )  SELECT  	COALESCE(T.PROJECTTEAMID,B.PROJECTTEAMID) ProjectTeamID 	,COALESCE(T.ProjectTeam,B.ProjectTeam) ProjectTeam 	,ISNULL(T.[Target],0) [TopDown Target(M)] 	,ISNULL(B.Estimate,0) [BottomUP Estimate(M)] 	,ISNULL(T.[Target],0) - ISNULL(B.Estimate,0) [Variance(M)] FROM TopDownCTE T 	FULL OUTER JOIN BottomUPCTE B ON T.PROJECTTEAMID = B.ProjectTeamId ORDER BY PROJECTTEAMID'  EXEC sp_executesql  @SQL", "What is the variance between Top down and bottom up? Provide overall variance and variance by the team." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "WITH EstimatedRunRates AS (     SELECT          E.ValueCaptureInitiativeId,  B.Title AS Initiative,          ISNULL(SUM(E.Amount),0) * 12 AS EstimatedAnnualizedRunRate     FROM          vwUnPivotEstimates E     JOIN          ValueCaptureInitiatives B ON E.ValueCaptureInitiativeId = B.ID     WHERE          E.Recurring = 1 		AND E.MYear IN ('Y1M12')     GROUP BY          E.ValueCaptureInitiativeId, B.Title ), ActualRunRates AS (     SELECT          A.ValueCaptureInitiativeId, B.Title AS Initiative,          SUM(ISNULL(A.Amount, 0)) * 12 AS ActualAnnualizedRunRate     FROM          vwUnpivotActuals A     JOIN          ValueCaptureInitiatives B ON A.ValueCaptureInitiativeId = B.ID     WHERE          A.Recurring = 1          AND A.MYear IN ('Y1M12')     GROUP BY          A.ValueCaptureInitiativeId, B.Title )   SELECT Initiative AS [List of Initiatives with 10% differ between Estimate and Actual] FROM ( SELECT      E.ValueCaptureInitiativeId,E.Initiative,     E.EstimatedAnnualizedRunRate,     A.ActualAnnualizedRunRate,    IIF(E.EstimatedAnnualizedRunRate = 0 ,0 , ABS(A.ActualAnnualizedRunRate - E.EstimatedAnnualizedRunRate) / E.EstimatedAnnualizedRunRate * 100) AS PercentageDifference FROM      EstimatedRunRates E JOIN      ActualRunRates A ON E.ValueCaptureInitiativeId = A.ValueCaptureInitiativeId ) A WHERE      PercentageDifference > 10", "List out initiatives that have an Estimate Annualized Run Rate that differed by 10% (higher or lower) from the Actual Annualized Run Rate for Y1M12." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      VC.Title AS ValueCaptureInitiativesReadyForApproval FROM      ValueCaptureInitiatives VC WHERE  	ReadyForApproval = 1", "List out validated initiatives that are ready to be approved." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      VI.title AS InitiativeImpactOnPnL FROM      ValueCaptureEstimates VE LEFT JOIN      ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId WHERE      FL.FinancialLineItemTypeId = 1 GROUP BY      VI.Title", "List out initiatives that are impacting PnL." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 148,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TOP 5     Initiatives,Amount FROM( SELECT VI.Title Initiatives ,sum(VE.Amount) Amount FROM      vwUnpivotActuals VE LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN 	ValueCaptureImpactTypes VIT ON VIT.ID = VE.ValueCaptureImpactTypeid GROUP BY      VI.Title,VE.Amount ) A  WHERE A.Amount > 0 ORDER BY  	Amount DESC", "List out top 5 initiatives that are positively impacting PnL." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "	SELECT TOP 5  		VI.Title [top 5 initiatives with best benefit score] 	FROM  		ValueCaptureInitiatives VI 	LEFT JOIN  		ValueCaptureImpacts VIM ON VIM.ID = VI.ValueCaptureImpactid 	WHERE  		VIM.ID in (1) 	ORDER BY  		VI.BenefitScore DESC", "List out top 5 initiatives with best benefit score that have high financial impact." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "	SELECT TOP 5  		VI.Title [top 5 most complex initiatives] 	FROM  		ValueCaptureInitiatives VI 	LEFT JOIN  		ValueCaptureImpacts VIM ON VIM.ID = VI.ValueCaptureImpactid 	WHERE  		VIM.ID in (3) 	ORDER BY  		VI.ComplexityScore DESC", "List out top 5 most complex initiatives that have low financial impact." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TOP 1 	PT.Title, SUM(ISNULL(HeadcountCostReductionEst,0)) AS MostHeadcountCostReduction FROM  	ValueCaptureTopDownEstimates LEFT JOIN  	ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY  	PT.Title ORDER BY  	SUM(ISNULL(HeadcountCostReductionEst,0)) DESC", "Which team has most amount of headcount related cost reduction?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Iniatives that are Cost Reduction and are Easy for Implementation Ease] FROM ValueCaptureInitiatives VI     LEFT JOIN ValueCaptureTypes VT         ON VT.ID = VI.ValueCaptureTypeID     LEFT JOIN ImplementationEase VD         ON VD.ID = VI.ImplementationEaseID WHERE VT.Title = 'Cost Reduction'       AND VD.Title = 'Easy'", "List all Iniatives that are Cost Reduction and are Easy for Implementation Ease." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VS.Title [Value Capture Stage],        VI.Title [Initiatives that have Talent intiative value drivers] FROM ValueCaptureInitiatives VI     LEFT JOIN ValueCaptureStages VS         ON VS.ID = VI.ValueCaptureStageID     LEFT JOIN ValueCaptureInitiativesToValueCaptureValueDriversForValueCaptureValueDrivers VD         ON VD.ValueCaptureInitiativeId = VI.ID     LEFT JOIN ValueCaptureValueDrivers VV         ON VD.ValueCaptureValueDriverId = VV.ID WHERE VV.Title = 'Talent' ", "Provide me a list of Iniatives by stage that have Talent iniative value drivers." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	Title as [Value Capture Initiatives with No Owners] FROM 	ValueCaptureInitiatives as VC WHERE 	ItemOwnerId IS NULL", "Show me all iniatives that do not have an owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ISNULL(VP.Title, 'No Priority') AS ValueCapturePriority,     COUNT(VC.ID) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId --WHERE VC.ValueCapturePriorityId IS NOT NULL GROUP BY VC.ValueCapturePriorityId, VP.Title  ---Stage---  SELECT ISNULL(VS.Title, 'No Stage Assigned') AS ValueCaptureStage,     COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VS ON VS.ID = VC.ValueCaptureStageId WHERE VC.ValueCaptureStageId IS NOT NULL GROUP BY VC.ValueCaptureStageId, VS.Title", "Provide me a count of initiatives by prioirty and stage." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT FL.Title [Balance sheet line items that do not have a linked account] FROM ValueCaptureFinancialLineItems FL     LEFT JOIN ValueCaptureFinancialLineItemTypes FLT         ON FLT.ID = FL.FinancialLineItemTypeID     LEFT JOIN BalanceSheetAccounts BLA         ON BLA.FinancialLineItemID = FL.ID WHERE FLT.Title = 'Balance Sheet'       AND BLA.ID IS NULL", "Show me all Balance sheet line items that do not have a linked account." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(E.Amount, 0) * 12) [Run Rate for iniatives in Approved, Committed and Realized] FROM ValueCaptureInitiatives VI     LEFT JOIN vwUnpivotEstimates E         ON E.ValueCaptureInitiativeId = VI.ID     LEFT JOIN ValueCaptureStages VS         ON VS.ID = VI.ValueCaptureStageID WHERE VS.Title IN ( 'Approved', 'Committed', 'Realized' )       AND MYear =       (           select [Key]           from ValueCaptureTransactionMonths           where CAST(EndDt AS date) =           (               SELECT MAX(CAST(EndDt AS date)) FROM ValueCaptureTransactionMonths           )       )", "What is the Run Rate for iniatives in Approved, Committed and Realized?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title [Project Team],        SUM(T.HeadCountCostReductionEst) [Total target Headcount cost redutciton] FROM ValueCaptureTopDownEstimates T     LEFT JOIN ProjectTeams PT         ON PT.ID = T.ProjectTeamId GROUP BY PT.Title", "Provide the total Headcount cost redutciton target listed out by project team and provide the number of FTEs." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(T.FTEs) [FTEs part of the top down targets for EMEIA] FROM ValueCaptureTopDownEstimates T     LEFT JOIN Regions R         ON R.ID = T.RegionID WHERE R.Title = 'EMEIA'", "How many FTEs are part of the top down targets for EMEIA?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	VI.Title [Initiatives with Benefit score over 75] FROM 	ValueCaptureInitiatives VI LEFT JOIN  	ProjectTeams PT ON PT.ID = VI.ProjectTeamId WHERE 	CAST(BenefitScore AS INT) > 75 	AND PT.Title IN ('HR')", "Which HR iniatives have a benefit score over 75?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 	VI.Title [Initiatives that have Risk associated with them] FROM 	ValueCaptureInitiatives VI LEFT JOIN  	ValueCaptureStages VS ON VS.ID = VI.ValueCaptureStageId LEFT JOIN  	RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs VIR ON VIR.ValueCaptureInitiativeId = VI.ID LEFT JOIN 	RisksAndIssues RI ON RI.ID = VIR.RisksAndIssueId WHERE  	RI.IssueRiskCategory IN ('Risk') 	AND VS.Title IN ('Committed')", "List out all Committed Iniatives that have a risk associated with them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	VI.Title [top 5 initiatives with best benefit score] FROM  	ValueCaptureInitiatives VI LEFT JOIN 	ValueCaptureImpactTypes VIT ON VIT.ID = VI.ValueCaptureImpactTypeId LEFT JOIN  	ValueCaptureImpacts VIM ON VIM.ID = VI.ValueCaptureImpactid WHERE  	VIM.[Key] in ('High') 	AND VIT.PositiveOrNegativeValues IN ('Positive')", "List all High Benefit iniatives that are in the Identified Stage." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 	VI.Title [Initiative is Approved with no Owner] FROM 	ValueCaptureInitiatives VI LEFT JOIN  	ValueCaptureStages VS ON VS.ID = VI.ValueCaptureStageId WHERE  	VS.Title IN ('Approved') 	AND 	VI.ItemOwnerId IS NULL", "Show me all iniatives that are approved and do not have an owner assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 164,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT 	COUNT(VI.ID) FROM 	ValueCaptureInitiatives VI LEFT JOIN 	UserProfiles UP ON UP.ID = VI.ItemOwnerId WHERE  	UP.EMail = '{Username}'", "CE4-VC", "How many iniatives are assigned to me?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 165,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT  	PT.Title ProjectTeam,SUM(ISNULL(NonHeadcountCostReductionEst,0)) NonHeadcountCostReductionTarget FROM  	ValueCaptureTopDownEstimates LEFT JOIN  	ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY  	PT.Title", "CE4-VC", "Provide the total non-headcount cost reduciton targets listed out by project team." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 166,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 167,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 168,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "How many assets are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(Title) AS TotalCount, BusinessEntityId FROM Nodes WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "Can you provide me number of processes by op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 172,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 173,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in Day1 state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(1) FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'  AND N.NodeTypeId = 3  AND N.ID NOT IN (   SELECT DISTINCT NTO.NodeId   FROM Nodes N1   LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null  )", "How many processes don't have an Owner assigned in Current State?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 175,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.ID 	,PR.Title FROM Nodes PR JOIN TransactionStates T ON T.ID = PR.TransactionStateId WHERE T.[Key] = 'DAY_ONE' 	AND PR.NodeTypeId = 3 	AND NOT EXISTS  				(SELECT 1 FROM [NodesToDispositionsForDispositionNew] D WHERE D.NodeId = PR.ID)", "List processes that don't have Disposition assigned in Day 1 state." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 176,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title FROM Dispositions", "List the Disposition options available." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 177,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [ProcessGroup] ,COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) [Enablers Count] FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 2 GROUP BY N.Title ORDER BY [Enablers Count] DESC", "How many enablers are associated with each Process Group?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 178,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title [Disposition] 	,COUNT(SD.SystemId) [SystemCount] FROM Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID GROUP BY D.Title", "List the total number of systems by disposition." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT PR.Title [Process] , D.DispositionId FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId  	LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 	AND D.NodeId IS NULL 	AND S.[Key] = 'DAY_ONE'", "Can you please list down all Day 1 processes where no disposition has been tagged?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 180,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT 	N.Title [Process]	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 3 GROUP BY N.Title HAVING COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) = 0", "Can you please list all processes with no Enablers?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.Title as MissingInDay1 FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'CURRENT_STATE' EXCEPT SELECT 	PR.Title [Process] FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'DAY_ONE'", "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title Enabler, 'Asset' as Category	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID LEFT JOIN [Assets] A ON A.ID = NA.AssetsId WHERE A.Title IS NOT NULL UNION ALL SELECT DISTINCT F.Title Enabler, 'Facility' as Category FROM Nodes N LEFT JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID LEFT JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE F.Title IS NOT NULL  UNION ALL SELECT DISTINCT S.Title Enabler , 'System' as Category FROM Nodes N LEFT JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID LEFT JOIN [Systems] S ON S.ID = NS.SystemsId WHERE S.Title IS NOT NULL  UNION ALL SELECT DISTINCT T.Title Enabler	, 'TPA' as Category FROM Nodes N LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID LEFT JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId WHERE T.Title IS NOT NULL ", "What Enablers are we tracking for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 183,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# Systems present in this functional OP model] FROM Systems", "How many Systems are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 184,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements", "How many TPAs are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ST.Title AS SystemType, COUNT(S.ID) [# Systems by Type] FROM Systems S LEFT JOIN SystemTypes ST ON ST.ID = S.TypeId WHERE S.TypeId IS NOT NULL GROUP BY ST.Title", "List the number of Systems by Type." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 186,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT O.Title AS Owners, Count(*) AS [# TPAs by Ownership] FROM ThirdPartyAgreements TP LEFT JOIN Owners O ON O.ID = TP.OwnerID WHERE TP.OwnerID IS NOT NULL GROUP BY O.Title", "List the number of TPAs by Ownership." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title [ProcessGroups With No Process] FROM Nodes WHERE nodetypeid = 2 AND ID NOT IN (SELECT NodeParentId FROM Nodes)", "List down all process groups with no process within them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(P.[ID]) AS [Number of Process across Op Model] FROM Nodes P     LEFT JOIN NodeTypes NType         ON P.NodeTypeId = NType.ID WHERE NType.Title = 'Process'", "List the number of processes across op models." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Modified >= DATEADD(hh, -1, GETDATE())", "List history of changes to the op model in the past one hour." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 190,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE FieldImpacted = 'Owner'       AND CAST(Modified AS DATE) = CAST(GETDATE() AS DATE)", "List the history of changes to Ownership in the op model today." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 191,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Title = 'Deleted'       AND DATEPART(wk, CAST(Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(Modified) = YEAR(GETDATE())", "List the history of deletes in the op model this week." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 192,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT T.Title [State], N.Title AS OperatingModel FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId  WHERE N.NodeTypeId = 1", "List the functional operating models across different states." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 193,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title Opmodel, L.Title LiveNote, Note FROM OperatingModelLiveNotes L LEFT JOIN Nodes N ON N.Id = L.NodeId", "Are there any Live Notes in this op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 194,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process FROM OperatingModelActivityLog OL  LEFT JOIN Nodes N ON N.ID = Ol.ObjectID WHERE OL.Title = 'Updated' AND Object = 'Process' AND (CAST(OL.Modified AS DATE) BETWEEN DATEADD(DAY, -7, CAST(GETDATE()AS DATE))  AND CAST(GETDATE() AS DATE))", "List down all processes which were updated since last 1 week." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 195,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT N.Title Process, OldValue, NewValue  FROM OperatingModelActivityLog PL  LEFT JOIN Nodes N ON N.ID = PL.ObjectID  WHERE Object = 'Process' AND PL.Title = 'Updated' AND FieldImpacted = 'Title' AND N.TransactionStateId = 6", "List down all processes which have been renamed in Day 1 op model.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 196,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT TOP 5     S.Title AS [Systems],     Count(NSS.NodesID) AS [ProcessCount] FROM Nodes N     LEFT JOIN [dbo].[NodesToSystemsForEnablerSystems] NSS         ON N.ID = NSS.NodesId     LEFT JOIN [dbo].[Systems] S         ON NSS.SystemsId = S.ID WHERE N.NodeTypeId = 3 GROUP BY S.Title ORDER BY Count(NSS.NodesID) DESC", "Please provide top 5 systems that are linked to various processes.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 197,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [List of processes for CurrentState where ownership is not present] FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'   AND N.NodeTypeId = 3   AND N.ID NOT IN (     SELECT DISTINCT NTO.NodeId     FROM Nodes N1     LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null   )", "For Current State, list all processes that doesn't have any ownership." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 198,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " SELECT     PR.Title [Process] FROM     Nodes                                    PR     JOIN         TransactionStates                    S             ON S.ID = PR.TransactionStateId     LEFT JOIN         NodesToDispositionsForDispositionNew D             ON D.NodeId = PR.ID WHERE     PR.NodeTypeId = 3     AND D.NodeId IS NULL     AND S.[Key] = 'DAY_ONE'", "For Day 1 Operating Models, list all processes that doesn't have any dispositions.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 199,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,       --SELECT * FROM PG_Hierarchy         ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT TOP 1     Functions AS [Function with highest number of processes] FROM (     SELECT Functions,            COUNT(ID) as [Process Count]     FROM ProcessCTE     GROUP BY Functions ) A order by [Process Count] desc    ", "Which function has the highest number of processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 200,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     N.Title AS [List of Processes with Disposition as TSA] FROM   Nodes   N   LEFT JOIN     NodesToDispositionsForDispositionNew NTD             ON NTD.NodeId = N.ID     LEFT JOIN         Dispositions                         D             ON NTD.DispositionId = D.ID WHERE     D.Title = 'TSA'     AND N.NodeTypeId = 3 -- To get the list of Processes", "List down all Processes which have their disposition set as TSA.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 201,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT       N.Title as [List of Processes with No Disposition] FROM Nodes N          WHERE N.NodeTypeId=3  		  AND N.ID NOT IN  		             (SELECT NodeId FROM NodesToDispositionsForDispositionNew)", "List down all processes with no Disposition assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 202,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(1)  [No. of Processes] FROM Nodes PR JOIN NODES P ON PR.NodeParentID = P.ID WHERE PR.NodeTypeId = 3 AND P.Title = '{FunctionName}'", "How many processes are there in the IT function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 203,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  PR.Title [Process] , COUNT(DISTINCT D.DispositionId) [DispostionCount] FROM Nodes PR JOIN NODES P ON PR.NodeParentID = P.ID LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 AND P.Title = 'COMMERCIAL' GROUP BY PR.Title HAVING COUNT(DISTINCT D.DispositionId) > 1", "List processes that have more than 1 disposition in IT.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 204,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ),       --SELECT * FROM PG_Hierarchy         ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT ISNULL(Geo.Title, 'Not Assigned') AS [Geographic Coverages],        COUNT(P.ProcessID) AS [Total Number of Process in Real Estate] FROM ProcessCTE P     LEFT JOIN ProcessAttributes PA         ON P.ProcessID = PA.NodeId     LEFT JOIN GeographicCoverages Geo         ON PA.GeographicCoverageId = Geo.ID WHERE P.Functions ='{FunctionName}' GROUP BY Geo.Title", "What is total number of processes in Real Estate grouped by geographic coverage?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 205,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      F.Title AS [Functions],     SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID ORDER BY 1,          2", "List all functions and sub-functions for functional operating model [Name].", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 206,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     O.Title     AS [Process Owner],     COUNT(N.ID) AS [Process Distribution] FROM     Nodes      N     LEFT JOIN         Owners O             ON N.OwnerId = O.ID WHERE     N.NodeTypeId = 3 GROUP BY     O.Title ", "Provide process owner distribution for my project.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 207,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID WHERE F.Title='{FunctionName}' ORDER BY 1", "List all sub-functions for Finance.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 208,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,       --SELECT * FROM PG_Hierarchy        ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT P.Processes,        ISNULL(S.Title, 'Not Assigned') AS [Systems],        ISNULL(TPA.Title, 'Not Assigned') AS [Third Party Agreements] FROM ProcessCTE P     LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA         ON P.ProcessID = NTPA.NodesId     LEFT JOIN ThirdPartyAgreements TPA         ON NTPA.ThirdPartyAgreementsId = TPA.ID     LEFT JOIN NodesToSystemsForEnablerSystems NSys         ON NSys.NodesId = P.ProcessID     LEFT JOIN Systems S         ON NSys.SystemsId = S.ID", "List all details for processes and systems, third party agreements linkages." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 209,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT A.Title Assest,        P.Title LinkedProcess FROM Assets A     LEFT JOIN ProcessAssetLinks PA         ON PA.AssetId = A.ID     LEFT JOIN Nodes P         ON P.ID = PA.ProcessId     LEFT JOIN NodeTypes NType         ON P.NodeTypeId = NType.ID WHERE NType.Title = 'Process'", "List the Assets and their linked processes.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 210,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title OpModel, BP.Title  EndToEndProcess FROM BusinessProcesses BP  LEFT JOIN Nodes N ON N.ID = BP.NodeId  WHERE N.NodeTypeId = 1", "Are there any end-to-end business processes in this op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL           AND TransactionStateID = 8     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ) SELECT N.Title [Function],        NTSS.Title AS [Field Status],        OPS.Title AS [Op Status] FROM NodeTrackers NT     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NT.ID     JOIN     (SELECT * FROM GetSelectedFunction WHERE HierId = 2 and Title = 'HR') N         ON N.ID = NT.NodeId     LEFT JOIN NodeTrackerStatusFields NTSS         ON NTSS.ID = NTS.NodeTrackerStatusFieldId     LEFT JOIN OpStatuses OPS         ON OPS.ID = NTS.NodeTrackerStatusStatusId", "In the Progress Tracker, what is the current status of HR?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 212,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "/*---- What is Automatic Workplan?----*/ SELECT D.Title Disposition,        WP.Title Tasks FROM NodesToWorkPlansForTask NWP     LEFT JOIN Nodes N         ON N.ID = NWP.NodeId     LEFT JOIN WorkPlan WP         ON NWP.WorkPlanId = WP.ID     LEFT JOIN NodesToDispositionsForDispositionNew ND         ON ND.NodeID = N.ID     LEFT JOIN Dispositions D         ON D.ID = ND.DispositionID WHERE N.NodeTypeId = 3", "In the Automatic Workplan, what are the defualt task lists for different dispostions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 213,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL           AND TransactionStateID = 6     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM Nodes ROOT     WHERE ROOT.ID in (                          select ID from GetSelectedFunction where HierId = 2 and Title = 'Finance'                      )     UNION ALL     SELECT CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ) SELECT COUNT(*) AS [Process with multiple dispositions for Finance] FROM PG_Hierarchy WHERE NodeTypeId = 3       AND ID IN (                     SELECT NodeID                     FROM                     (                         SELECT NodeId,                                Count(DispositionID) As [Disposition Count]                         FROM NodesToDispositionsForDispositionNew                         GROUP BY NodeId                         HAVING Count(DispositionID) > 1                     ) a                 )", "In Finance, how many processes have multiple dispositions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     N.Title AS [List of Process which doesn''t have ownership] FROM     Nodes                 N     JOIN         TransactionStates T             ON T.ID = N.TransactionStateId WHERE     OwnerId IS NULL     AND N.NodeTypeId = 3 -- To select Node Type = Process", "List down all processes with no Owners assigned.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 215,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT N.Title AS [List of Processes with more than one Disposition] FROM Nodes N WHERE N.NodeTypeId = 3       AND N.ID IN (                       SELECT NodeID                       FROM                       (                           SELECT NodeId,                                  COUNT(DispositionID) AS [Disposition Count]                           FROM NodesToDispositionsForDispositionNew                           GROUP BY NodeId                           HAVING COUNT(DispositionID) > 1                       ) a                   ) ", "List down all processes with more than one Disposition associated with them.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT ID,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.ID,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, TransactionStateID) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.TransactionStateID     FROM Nodes ROOT     WHERE ROOT.ID IN (                          SELECT ID                          FROM GetSelectedFunction                          WHERE HierId = 2                                AND Title = '{FunctionName}'                                AND TransactionStateId = 6                      )     UNION ALL     SELECT CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.TransactionStateId     FROM PG_Hierarchy PARENT,          Nodes CHILD     WHERE PARENT.id = CHILD.NodeParentID    ) SELECT Title AS [List of Processes Associated with Legal Function] FROM PG_Hierarchy ORDER BY ID,          title DESC ", "Can you please list all processes associated with Legal function?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 217,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     N.Title AS [List of all Processes associated with APAC] FROM     Nodes       N     LEFT JOIN         Regions R             ON N.CountryRegionID = R.ID WHERE     n.NodeTypeId = 3     AND R.Title = '{Region}'", "Can you please list all processes associated with APAC?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 218,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT P.Functions, A.Processes  FROM (SELECT DISTINCT [Processes]  		FROM ProcessCTE 		GROUP BY Processes  		HAVING COUNT(DISTINCT [Functions] ) > 1) A  	LEFT JOIN ProcessCTE P ON P.Processes = A.Processes ", "Can you please list all processes with same names across different functions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 219,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     [List of Processes with same name] FROM     (         SELECT             N.Title  AS [List of Processes with same name],             Count(1) AS [Repeat Count]         FROM             Nodes N         WHERE             N.NodeTypeId = 3         GROUP BY             N.Title         HAVING             COUNT(*) > 1     ) a ORDER BY     1 ", "Can you please list all processes with same name?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 220,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title AS [List of Processes tagged as ParentCo in Current State with Disposition set as TSA ] FROM Nodes N     LEFT JOIN NodesToOwnersForOwnerNew NTO         on N.ID = NTO.NodeId     LEFT JOIN Owners O         ON O.ID = NTO.OwnerId     LEFT JOIN NodeTypes NT         ON N.NodeTypeId = NT.ID     LEFT JOIN TransactionStates TS         ON N.TransactionStateId = TS.ID     LEFT JOIN NodesToDispositionsForDispositionNew NTD         ON N.ID = NTD.NodeId     LEFT JOIN Dispositions D         on NTD.DispositionId = D.ID WHERE NT.Title = 'Process'       AND TS.[Key] = 'CURRENT_STATE'       AND O.Title = 'Parent Co'       AND D.Title = 'TSA'", "Can you please list the processes which are tagged as 'ParentCo' in Current state having disposition set as 'TSA'?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 221,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     Title AS [Systems not tagged in any process] FROM     Systems WHERE     ID NOT IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes                               N                       LEFT JOIN                           NodesToSystemsForEnablerSystems NSS                               ON N.ID = NSS.NodesId  					  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId                    WHERE T.Title= 'Day 1' AND                        N.NodeTypeId = 3               )", "can you please list all systems which are not tagged to any process in Day 1 op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 222,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     ISNULL(ST.Title, 'System Type Not Assigned') AS [SystemsType],     S.Title                                      AS [Systems] FROM     Systems         S     LEFT JOIN         SystemTypes ST             ON S.TypeId = ST.ID", "Can you please provide of all systems based upon their type?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 223,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number for Multiple Deposition]  				FROM ProcessCTE 				WHERE ID IN ( 									SELECT NodeID 									FROM 									( 										SELECT NodeId, 											   Count(DispositionID) As [Disposition Count] 										FROM NodesToDispositionsForDispositionNew 										GROUP BY NodeId 										HAVING Count(DispositionID) > 1 									) a 								) 		GROUP BY [Functions] 				ORDER BY  COUNT(*) DESC    ", "Which function has the highest number of multi disposition processes?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 224,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes   WHERE NodeParentID IS NULL   UNION ALL         SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   WHERE Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2    UNION ALL         SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  WHERE  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS (  SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Live Without]  FROM ProcessCTE WHERE ID IN (	SELECT NodeId 				FROM NodesToDispositionsForDispositionNew ND  				LEFT JOIN Dispositions D ON D.ID = DispositionID  				WHERE D.Title = 'Live without'			 				) GROUP BY [Functions] ORDER BY  COUNT(*) DESC", "which function has the highest number of 'Live without'?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 225,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Processes tagged to them] , COUNT(*)  [No of Processes] 				FROM ProcessCTE 		GROUP BY [Functions] 				ORDER BY  COUNT(*) DESC    ", "Which function has the highest number of processes tagged to them? and how many?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 226,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ),       ProcessCTE AS (SELECT DISTINCT         F.Title AS [Functions],         f.id AS FunctionID     FROM     (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF             ON F.ID = SF.NodeParentID    ) SELECT Top 1     Functions as [Function with Highest Number of Enablers],     [Enablers Count] FROM (     SELECT P.Functions,            --,COUNT(DISTINCT TPA.ID) as TPACount            --,COUNT(DISTINCT S.ID) as SystemCount            --,COUNT(DISTINCT Asset.ID) as AssetCount            --,COUNT(DISTINCT Roles.ID) as RolesCount,            COUNT(DISTINCT TPA.ID) + COUNT(DISTINCT S.ID) + COUNT(DISTINCT Asset.ID) + COUNT(DISTINCT Roles.ID) AS [Enablers Count]     FROM ProcessCTE P         LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA --Third Party Aggrements             ON P.FunctionID = NTPA.NodesId         LEFT JOIN ThirdPartyAgreements TPA             ON NTPA.ThirdPartyAgreementsId = TPA.ID         LEFT JOIN NodesToSystemsForEnablerSystems NSys -- Systems             ON NSys.NodesId = P.FunctionID         LEFT JOIN Systems S             ON NSys.SystemsId = S.ID         LEFT JOIN NodesToAssetsForEnablerAssets NAsset --Assets             ON NAsset.NodesId = P.FunctionID         LEFT JOIN Assets Asset             ON Asset.ID = NAsset.AssetsId         LEFT JOIN NodesToRolesForEnablerRoles NRole --Roles             ON NAsset.NodesId = P.FunctionID         LEFT JOIN OrgRolesMaster Roles             ON Roles.ID = NRole.RolesId     GROUP BY P.Functions ) A ORDER BY [Enablers Count] DESC", "Which function has the highest number of enablers tagged to them? and how many?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 227,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID,  HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL    UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,    HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy(NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM Nodes ROOT   WHERE ROOT.ID in (select ID from GetSelectedFunction where HierId=2 and Title='Finance & Accounting')       UNION ALL        SELECT  CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  )   SELECT PG.Title, OldValue, NewValue  FROM PG_Hierarchy PG  JOIN (SELECT PL.ObjectID, OldValue, NewValue 			FROM OperatingModelActivityLog PL 			WHERE Object = 'Process' AND Title = 'Updated' AND PL.Modified = ( 			SELECT  Max(Modified) Modified FROM OperatingModelActivityLog  			WHERE ObjectID IN (SELECT ID FROM PG_Hierarchy PG WHERE NodeTypeId=3 )  			AND Object = 'Process' AND Title = 'Updated')) A ON A.ObjectID = PG.ID", "Which process within Finance has been updated most recently and what was the updates?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 228,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     N.Title AS [List of Process Associated with Workplan] FROM     Nodes                       N     LEFT JOIN         NodesToWorkPlansForTask NWP             ON N.ID = NWP.NodeId     LEFT JOIN         WorkPlan                WP             ON NWP.WorkPlanId = WP.ID WHERE     N.NodeTypeId = 3", "How many processes have been associated with a workplan item?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 229,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(DISTINCT n.ID) AS [No of Process having 2 owners or Dispositions] FROM Nodes n LEFT JOIN (     SELECT NodeId     FROM NodesToDispositionsForDispositionNew     GROUP BY NodeId     HAVING COUNT(DispositionID) = 2 ) d ON n.ID = d.NodeId LEFT JOIN (     SELECT NodeID     FROM NodesToOwnersForOwnerNew     GROUP BY NodeID     HAVING COUNT(OwnerId) = 2 ) o ON n.ID = o.NodeID WHERE n.NodeTypeId = 3 AND (d.NodeId IS NOT NULL OR o.NodeID IS NOT NULL)", "How many processes have 2 owners or dispositions? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 230,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	COUNT(SD.SystemId) [No of Systems are dispositioned rebuild] FROM  Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID WHERE D.Title = 'Rebuild'", "How many systems are dispositoned Rebuild?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 231,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT O.Title [Owners], COUNT(N.ID) [No of Processes]  FROM Nodes N  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID  LEFT JOIN NodesToOwnersForOwnerNew NOS ON NOS.NodeId = N.ID  LEFT JOIN Owners O ON O.ID = NOS.OwnerId  WHERE NT.Title = 'Process'  GROUP BY O.Title", "Provide a count of all processes by owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 232,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT  	R.Title Regions 	, COUNT(D.DispositionId) [Count of Disposition]  FROM NodesToDispositionsForDispositionNew D     JOIN Nodes N ON D.NodeId = N.ID 	JOIN TransactionStates S ON S.ID = N.TransactionStateId  	LEFT JOIN NodeTypes NT ON NT.ID = N.NodeTypeId  	LEFT JOIN Regions R ON R.ID = N.CountryRegionID  WHERE NT.Title = 'Process'  	AND S.[Key] = 'DAY_ONE' GROUP BY R.Title", "Show me the count of dispositions in Day 1 model by assigned region." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 233,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [Processes with no owner assigned] FROM Nodes N  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID WHERE NT.Title = 'Process'    AND N.ID NOT IN (     SELECT DISTINCT NTO.NodeId     FROM Nodes N1     LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	WHERE NTO.NodeId is not null   )", "List all processes that do not have an owner assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 234,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title AS [List of Teams with Disposition set as Unidentified] FROM Nodes N     LEFT JOIN NodeTypes NT         ON N.NodeTypeId = NT.ID 	LEFT JOIN  NodesToProjectTeamsForProjectTeam NP 	    ON N.ID = NP.NodesId  	LEFT JOIN ProjectTeams PT 	    ON PT.ID = NP.ProjectTeamsId      LEFT JOIN NodesToDispositionsForDispositionNew NTD         ON N.ID = NTD.NodeId     LEFT JOIN Dispositions D         on NTD.DispositionId = D.ID WHERE NT.Title = 'Process'       AND D.Title = 'Unidentified'", "Which teams have processes that are disposition 'Unidentified'?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 235,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [Processes with total headcount greater than 0] FROM Nodes N     LEFT JOIN vwHeadcount H         ON H.NodeId = N.ID     LEFT JOIN Nodetypes NT         ON NT.ID = N.NodeTypeId GROUP BY N.Title HAVING SUM(ISNULL(H.TotalHeadCount, 0)) > 0", "List all processes with total headcount greater than 0." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 236,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT F.Title [Facilities in North America that are leased or owned]   FROM Facilities F  LEFT JOIN FacilityOccupancyTypes FT ON FT.ID = F.FacilityOccupancyTypeID  LEFT JOIN Countries C ON C.ID = F.CountryID  LEFT JOIN Regions R ON R.ID = C.CountryRegionID WHERE FT.Title IN ('Leased', 'Owned') AND R.Title = 'North America'", "List all Facilities in North America that are leased or owned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 237,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(N.ID) [No of Proccesses to which TPA is assigned] FROM NodesToThirdPartyAgreementsForEnablerTpa NTP  INNER JOIN Nodes N ON N.ID = NTP.NodesId  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID WHERE NT.Title = 'Process'", "How many processes have a TPA assigned to them? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 238,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Enabler [Enabler with linked processes] FROM( SELECT DISTINCT A.Title Enabler 	 FROM Nodes N LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID INNER JOIN [Assets] A ON A.ID = NA.AssetsId  WHERE NTT.Title = 'Process'    UNION ALL  SELECT DISTINCT F.Title Enabler FROM Nodes N LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID INNER JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE NTT.Title = 'Process'    UNION ALL  SELECT DISTINCT S.Title Enabler  FROM Nodes N  LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID INNER JOIN [Systems] S ON S.ID = NS.SystemsId WHERE NTT.Title = 'Process'    UNION ALL  SELECT DISTINCT T.Title Enabler	 FROM Nodes N  LEFT JOIN NodeTypes NTT ON NTT.ID = N.NodeTypeId  INNER JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID INNER JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId  WHERE NTT.Title = 'Process'   ) A", "Show me all enablers that have a linked process." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 239,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title AS [List of Processes with Disposition set as TSA ] FROM Nodes N     LEFT JOIN NodeTypes NT         ON N.NodeTypeId = NT.ID     LEFT JOIN NodesToDispositionsForDispositionNew NTD         ON N.ID = NTD.NodeId     LEFT JOIN Dispositions D         on NTD.DispositionId = D.ID WHERE NT.Title = 'Process'       AND D.Title = 'TSA'", "List all processes that have the disposition TSA." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 240,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(N.ID) AS [No of Processes are Live Without]  FROM Nodes N  LEFT JOIN NodeTypes NT ON N.NodeTypeId = NT.ID WHERE NT.Title = 'Process' AND N.ID IN (	SELECT NodeId 				FROM NodesToDispositionsForDispositionNew ND  				LEFT JOIN Dispositions D ON D.ID = DispositionID  				WHERE D.Title = 'Live without'			 				) ", "How many processes are Live without? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 241,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     Title AS [Systems being used by Project Team and Process Group] FROM     Systems WHERE  ID IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes N                       LEFT JOIN NodesToSystemsForEnablerSystems NSS                        ON N.ID = NSS.NodesId  					  LEFT JOIN  NodesToProjectTeamsForProjectTeam NP 					  ON N.ID = NP.NodesId  					  LEFT JOIN NodeTypes NT ON NT.ID = N.NodeTypeId                    WHERE  NT.Title IN ('ProcessGroup')               )	", "List out the systems being used by Project Team and Process Group.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 242,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT N.Title [Process groups are not started on Followup Calls for Current State] FROM Nodes N     LEFT JOIN Nodetypes NT         ON NT.ID = N.NodeTypeId     LEFT JOIN NodeTrackers NTC         ON NTC.NodeID = N.ID     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NTC.ID     LEFT JOIN TransactionStates T         ON T.ID = N.TransactionStateID WHERE N.NodeOnTracker = 1       AND NT.Title = 'Process Group'       AND T.[Key] = 'CURRENT_STATE'       AND N.ID NOT IN (                           SELECT N.ID ProcessGroupID                           FROM Nodes N                               LEFT JOIN Nodetypes NT                                   ON NT.ID = N.NodeTypeId                               LEFT JOIN NodeTrackers NTC                                   ON NTC.NodeID = N.ID                               LEFT JOIN NodeTrackerStatuses NTS                                   ON NTS.NodeTrackerID = NTC.ID                               LEFT JOIN TransactionStates T                                   ON T.ID = N.TransactionStateID                           WHERE N.NodeOnTracker = 1                                 AND NT.Title = 'Process Group'                                 AND T.[Key] = 'CURRENT_STATE'                                 AND NTS.Title = 'FollowUp Call'                       )", "CE4-OM", "Which process groups are not started on Followup Calls for Current State?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 243,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL      UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ) SELECT O.Title [Client Owner for HR] FROM GetSelectedFunction N 	LEFT JOIN NodesToOwnersForOwnerNew NOS ON NOS.NodeId = N.ID  	LEFT JOIN Owners O ON O.ID = NOS.OwnerId  WHERE N.HierId = 2 AND N.Title = 'HR'", "CE4-OM", "Who is the Client owner for HR?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 244,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 245,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 246,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Provider." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 247,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Receiver." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 248,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "Can you provide me list of TSAs by phases?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 249,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "Show me the breakdown of TSAs by duration." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 250,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "How many TSAs does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 251,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    TST.Title as [Phase],    COUNT(TC.ID) AS [# TSAs across different phases] FROM    TSAItems TC    LEFT JOIN TSAPhases TST ON TC.PhaseID = TST.ID  Group BY    TST.Title ", "List the number of TSAs across different phases.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 252,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    T.Title PendingTSAItems  FROM    TSAItems T    JOIN TSAStatuses TS ON T.TSAItemTSAStatusId = TS.ID  WHERE    T.AuthorId = '{Username}'    AND TS.[key] = 'ACTIVE' ", "How many pending TSA items do I have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 253,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    Title as [Billing Period Name],    DeadlineDate as [Contribution Deadlines]  FROM    TSABillingPeriods ", "In Cost Tracking, how is my billing periods setup?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 254,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " SELECT    Title AS [TSA Phases]  FROM TSAPhases", "What are the different TSA phases in my project?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 255,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ProviderLegalEntityId = L.ID where  L.ServiceLocation is not null UNION  SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ReceiverLegalEntityId = L.ID   where  L.ServiceLocation is not null ", "List the TSA service locations in my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 256,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     TSA.Title AS [TSAs],     s.Title   AS [TSA Statuses] FROM     TSAItems        TSA     LEFT JOIN         TSAStatuses S             ON TSA.TSAItemTSAStatusId = S.ID     LEFT JOIN         Functions   F             ON TSA.FunctionId = F.ID WHERE     F.Title = '{FunctionName}'", "List the TSAs for the Finance fucntion with their status." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 257,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "DECLARE @UserProfiles int; SELECT    @UserProfiles = ID  FROM    UserProfiles  WHERE    EMail = 'sahil.garg5@gds.ey.com'; WITH TEMPCTE AS (   SELECT      FunctionId,      SubFunctionId    FROM      TSAReviewers    WHERE      ProviderApproverId = @UserProfiles      OR RecipientApproverId = @UserProfiles )  SELECT    TSA.Title as [List of TSA to be reviewed] FROM    TSAItems TSA    JOIN TEMPCTE ON TSA.FunctionId = TEMPCTE.FunctionId    AND tsa.SubFunctionId = TEMPCTE.SubFunctionId    JOIN TSAPhases TP ON TSA.PhaseId = TP.ID  WHERE    TP.[Key] IN ('ALIGNMENT', 'APPROVAL'); ", "List the TSAs that I need to review.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 258,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT   Title DefaultCurrency FROM  TSACurrencies  WHERE CurrencyCode ='{CurrencyCode}'", "What is the default currency setting?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 259,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT   CurrencyExchangeRateToUSD  FROM  TSACurrencies  WHERE CurrencyCode ={CurrencyCode}", "List the exchange rates we setup for the default currency on my project.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 260,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    DISTINCT F.Title as [Functions To Be Configured] FROM    TSAReviewers TR    LEFT JOIN Functions F on F.ID = TR.FunctionId ", "In Reviewer Assignments, what functions require to be configured properly?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 261,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    DISTINCT SF.Title as [SubFunctions To Be Configured] FROM    TSAReviewers TR    LEFT JOIN SubFunctions SF on SF.ID = TR.SubFunctionId ", "In Reviewer Assignments, what sub-functions need to be configured for my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 262,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT CASE WHEN SUM(DeadlineReminder) > 0 THEN 'Yes, email reminder before the deadline is setup' ELSE 'No, email reminder before the deadline is setup' END AS [Send an email reminder before the deadline?] FROM( SELECT     Title,     CAST( DeadlineReminder AS INT) DeadlineReminder FROM     TSABillingPeriods WHERE     DeadlineReminder = 1) A", "In Billing Periods, have we setup email reminder beofre deadline?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 263,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     Title        AS [Billing Period Name],     DeadlineDate AS [Contribution Deadlines] FROM     TSABillingPeriods", "List the billing periods with their Contribution deadlines." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 264,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT CASE WHEN SUM(SendEmailImmediately) > 0 THEN 'Yes, notifying data contributors by email ' ELSE 'No, not notifying data contributors by email ' END AS [Notify data contributors by email ?] FROM( SELECT     Title                AS [Billing Period Name],     CAST(SendEmailImmediately AS INT) SendEmailImmediately FROM     TSABillingPeriods WHERE     SendEmailImmediately = 1) A", "Are we not notifying data contributors by email for any billing periods?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 265,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     f.title   AS [Functions],     (t.TSAItemFTECostForServiceDurationUSD + t.MarkupOnFTECostUSD + t.ExternalMaterialCostUSD) AS [TSA Costs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND t.Duration IS NOT NULL", "What my TSA costs by  function?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 266,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "-- Total TSA Costs over Month SELECT month_year AS [By Month],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Month] FROM vwTSACostWaterfall GROUP BY date_,          month_year   -- Total TSA Costs over Quarter SELECT [quarter] AS [By Quarter],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Quarter] FROM vwTSACostWaterfall GROUP BY Concat(Year(date_), Datepart(quarter, date_)),          [quarter]   -- Total TSA Costs over Year SELECT [year] AS [By Year],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Year] FROM vwTSACostWaterfall GROUP BY [year]", "What is my total TSA costs over time?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 267,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     TSA.Title AS [TSAs] FROM     TSAItems        TSA     LEFT JOIN         TSAStatuses S             ON TSA.TSAItemTSAStatusId = S.ID WHERE          S.Title<>'{TSAStatus}'", "How many TSAs left to sign?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 268,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     COUNT(TSA.ID) AS [# TSAs signed by Function],     F.Title       AS Functions FROM     TSAItems        TSA     LEFT JOIN         TSAStatuses S             ON TSA.TSAItemTSAStatusId = S.ID     LEFT JOIN         Functions   F             ON TSA.FunctionId = F.ID WHERE     S.Title = '{TSAStatus}' GROUP BY     F.Title", "List number of signed TSAs by function.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 269,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     [year]                               AS [By Year],     ROUND(SUM(fte_cost_per_month), 2)    AS [TSA FTE Cost for Selected Year],     ROUND(SUM(nonfte_cost_per_month), 2) AS [TSA Non-FTE Cost for Selected Year] FROM     vwTSACostWaterfall WHERE      [year] = '{Year}' GROUP BY     [year]", "Break down the total TSA cost for this year by FTE and Non-FTE cost.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 270,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     Title AS [TSA Stages] FROM     TSAPhases ORDER BY     Ordinal", "List out stages that are preconfigured on this instance for TSA lifecycle." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 271,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Stg.Title AS [Stage Name],        COUNT(TSA.ID) AS [# TSAs Identified] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID WHERE Stg.[Key] = 'IDENTIFICATION' GROUP BY Stg.Title", "How many TSA have been identified?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 272,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     Title AS [TSAs that have duration for more than 18 months] FROM     TSAItems WHERE     Duration > 18", "List out TSAs that have duration for more than 18 months." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 273,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     TSA.Title AS [TSAs that are ready to be approved] FROM     TSAItems      TSA     LEFT JOIN         TSAPhases Stg             ON TSA.PhaseId = Stg.ID WHERE     Stg.[Key] IN ( 'ALIGNMENT','APPROVAL') ", "List out TSAs are that are ready to be approved." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 274,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     f.title                                                                                    AS [Functions],     SUM((t.TSAItemFTECostForServiceDurationUSD + t.MarkupOnFTECostUSD + t.ExternalMaterialCostUSD) )AS [TSA Costs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND t.Duration IS NOT NULL GROUP BY  f.title", "What is the total of TSA cost by function?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 275,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT TSA.Title AS [TSAs with Open Risks] FROM TSAItems TSA     LEFT JOIN RisksAndIssues RI         ON TSA.ID = RI.TSAItemId     LEFT JOIN Statuses S         ON RI.ProgressId = S.ID WHERE S.[Key] NOT IN ( 'COMPLETED', 'CLOSED' )", "List out TSA with Open risks.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 276,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT UP.Title AS [Buyer Side Reviewer for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.RecipientApproverID = UP.ID WHERE F.Title ='{FunctionName}'", "Who is the buy side reviewer for finance TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 277,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS [Buyer Side Reviewer for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.ProviderApproverId = UP.ID WHERE F.Title ='{FunctionName}'", "Who is the sell side reviewer for finance TSAs?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 278,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS [Approver for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.RecipientApproverID = UP.ID WHERE F.Title = '{FunctionName}'  UNION   SELECT UP.Title AS [Approver for Finance TSAs] FROM TSAReviewers TRev     LEFT JOIN Functions F         ON TRev.FunctionId = F.ID     LEFT JOIN UserProfiles UP         ON TRev.ProviderApproverId = UP.ID WHERE F.Title ='{FunctionName}'", "Who is the approver for finance TSAs?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 279,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT TSA.Title AS [TSAs with Behind Schedule Tasks Linked] FROM TSAItems TSA     LEFT JOIN Workplan WP         ON TSA.ID = WP.TSAItemId     LEFT JOIN Statuses S         ON WP.ProgressId = S.ID WHERE WP.WorkPlanTaskType = 'Task'       AND S.[Key] = 'BEHIND_SCHEDULE'", "List out TSAs with behind schedule tasks linked.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 280,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT f.title AS [Functions],        COUNT(t.ID) AS [TSAs Count],        (SUM(t.TSAItemFTECostForServiceDurationUSD) + SUM(t.MarkupOnFTECostUSD) + SUM(t.ExternalMaterialCostUSD)) AS [TSAs Costs] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN Functions f         ON t.FunctionId = f.id     LEFT OUTER JOIN TSADay1Dispositions TSADisp         ON t.TSADay1DispositionId = TSADisp.ID WHERE TSADisp.Title = 'rTSA (reverse TSA)'       AND s.title <> 'Canceled'       AND t.Duration IS NOT NULL GROUP BY f.title", "Show me the count and cost of reverse TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 281,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(TSA.ID) AS [TSA Items in Alignment phase for IT] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID WHERE Stg.[Key] = 'ALIGNMENT'       AND F.Title = '{FunctionName}'", "How many items are in the allignment phase for IT?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 282,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(TSA.ID) AS [# TSA items in the repository which have been modified in the last 7 days] FROM TSAItems TSA     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND CAST(TSA.Modified AS DATE)       BETWEEN CAST(GETDATE() - 7 AS DATE) AND CAST(GETDATE() AS DATE) ", "How many items in the repository have been modified in the last 7 days?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 283,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(TSA.ID) AS [# TSA items in the repository which have been created in the last 5 days] FROM TSAItems TSA     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND CAST(TSA.Created AS DATE)       BETWEEN CAST(GETDATE() - 5 AS DATE) AND CAST(GETDATE() AS DATE) ", "How many new items s in the repository have been created in the last 5 days?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 284,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT AVG(t.Duration) AS [Average Duration for IT TSAs],        (SUM(t.TSAItemFTECostForServiceDurationUSD) + SUM(t.MarkupOnFTECostUSD) + SUM(t.ExternalMaterialCostUSD)) AS [Cost for IT TSAs] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN Functions f         ON t.FunctionId = f.id WHERE s.title <> 'Canceled'       AND t.Duration IS NOT NULL       AND f.Title = 'IT' GROUP BY f.title", "What is the average duration and cost for IT TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 285,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSAs with Behind Schedule / At Risk Tasks Linked] FROM TSAItems TSA     LEFT JOIN Workplan WP         ON TSA.ID = WP.TSAItemId     LEFT JOIN Statuses S         ON WP.WorkPlanTaskStatusId = S.ID WHERE WP.WorkPlanTaskType = 'Task'       AND S.[Key] IN ('BEHIND_SCHEDULE','AT_RISK')", "Show me all TSAs that have a linked task that is At Risk or Behind Schedule." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 286,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSA Items that need review] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID WHERE Stg.[Key] IN ( 'ALIGNMENT', 'APPROVAL' )", "List all TSAs that need review." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 287,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title as [TSAs that need to be signed along with Item Owner], t.TSAItemPersonInCharge as [Item Owner] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]='SIGNED'", "List all TSAs that need to be signed along with the item owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 288,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [TSAs and rTSAs that have a markup greater than 5%]    ,TSADisp.Title AS [TSA / rTSA] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     	LEFT OUTER JOIN 	    TSADay1Dispositions TSADisp  		    ON t.TSADay1DispositionId=TSADisp.ID WHERE         s.title <> 'Canceled' 	  AND t.TSAItemMarkup >0.05", "Show me all TSAs and rTSAs that have a markup greater than 5%." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 289,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSAs are in a local currency that is not US dollar] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN TSACurrencies TSACurr         ON t.TSAItemLocalCurrencyId = TSACurr.ID     LEFT OUTER JOIN Currencies Curr         ON TSACurr.CurrencyID = Curr.ID WHERE Curr.Title <> 'USD ($)'       AND s.title <> 'Canceled'", "How many TSAs are in a local currency that is not US dollar?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 290,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title as [Inactive TSAs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]<>'ACTIVE' ", "List out all Inactive TSAs." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 291,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT t.Title as [TSAs that have duration less than 2 months] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]<>'CANCELED' AND t.Duration<2", "List All TSAs where the duration is less than 2 months." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 292,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT t.Title as [TSAs that have duration less than 2 months] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE s.[Key]<>'CANCELED' AND t.Duration>10", "List all TSAs where the duration is more than 10 months." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 293,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSAs due in next 45days] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id WHERE CAST(t.TSAItemEndDate AS DATE)       BETWEEN CAST(GETDATE() AS DATE) AND CAST(GETDATE() + 45 AS DATE)       AND s.title <> 'Canceled'", "List All TSAs due in next 45days." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 294,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [TSAs that have a markup above 1%] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     WHERE         s.title <> 'Canceled' 	  AND t.TSAItemMarkup >0.01", "List all TSAs where the markup is above 1%." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 295,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     t.Title AS [TSAs where Provider is not tagged] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id WHERE     s.title <> 'Canceled'     AND t.ProviderLegalEntityId IS NULL", "List all TSAs where provider is not tagged." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 296,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT    t.Title AS [TSAs where Receiver is not tagged] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     WHERE         s.title <> 'Canceled' 	  AND t.ReceiverLegalEntityId IS NULL", "List all TSAs where receiver is not tagged." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 297,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT    t.Title AS [Mixed Price Type TSAs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id   	LEFT OUTER JOIN 	PriceTypes Pric  	        ON t.TSAItemPriceTypeID=Pric.ID WHERE         s.title <> 'Canceled' 	  AND Pric.Title='Mixed price'", "List all mixed price type TSAs." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 298,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "	  SELECT    t.Title AS [List all TSAs which are not getting settled in USD]    FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     	LEFT OUTER JOIN 	    TSACurrencies TSACurr  		    ON t.TSAItemLocalCurrencyId=TSACurr.ID 	LEFT OUTER JOIN Currencies Curr 	        ON TSACurr.CurrencyID=Curr.ID WHERE         Curr.Title<>'USD ($)'  	  AND s.title <> 'Canceled'", "List all TSAs which are not getting settled n USD." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 299,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    t.Title AS [TSAs where User has provided comments] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     WHERE         s.title <> 'Canceled' 	  AND t.Comments IS NOT NULL", "List all TSAs where User has provided comments.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 300,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     ISNULL(Stg.Title, 'No Phase Assigned') AS [Phase],     (TSA.Title)                          AS [ TSAs title] FROM     TSAItems      TSA     LEFT JOIN         TSAPhases Stg             ON TSA.PhaseId = Stg.ID", "List all TSAs per phase." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 301,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     t.Title AS [TSAs assigned to function due in next 30 days] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND f.Title='{FunctionName}' 	AND CAST(t.TSAItemEndDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND CAST(GETDATE()+30 AS DATE)", "List All TSAs assigned to IT due in next 30 days.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 302,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSAs assigned to IT due in next 30 days] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN Functions f         ON t.FunctionId = f.id WHERE s.title <> 'Canceled'       AND f.Title = 'Finance'       AND CAST(t.TSAItemEndDate AS DATE)       BETWEEN CAST(GETDATE() AS DATE) AND CAST(GETDATE() + 15 AS DATE)       AND t.TSAItemMarkup <= 0.01       AND t.ReceiverLegalEntityId IS NULL", "List All TSAs assigned to finance due in next 15 days where markup is below 1% and has no receiver tagged." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 303,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSAs Items which are tagged as rTSA for Day 1 disposition] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN TSADay1Dispositions TSADisp         ON t.TSADay1DispositionId = TSADisp.ID WHERE s.title <> 'Canceled'       AND TSADisp.Title = 'rTSA (reverse TSA)'", "List all Items which are tagged as rTSA for Day 1 disposition." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 304,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT t.Title AS [TSA Items where total number of FTEs providing service is greater than 5] FROM TSAItems AS t     LEFT OUTER JOIN TSAStatuses s         ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN TSADay1Dispositions TSADisp         ON t.TSADay1DispositionId = TSADisp.ID WHERE s.title <> 'Canceled'       AND t.TSAItemNoOfFTEProvidingService > 5", "List all Items where total number of FTEs providing service is greater than 5." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 305,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSA items which have been in Detailing phase for more than 15 days] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID WHERE Stg.[Key] = 'DETAILING'       AND CAST(TSA.Modified AS DATE) < CAST(GETDATE() - 15 AS DATE)", "List all items which have been in Detailing phase for more than 15 days." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 306,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [Finance related TSA items with relevant SLA details],        ISNULL(TSA.TSAItemSLAName, ' ') AS [SLA Details] FROM TSAItems TSA     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND F.Title = 'Finance'", "CE4-TSA", "List all finance related items with relevant SLA details." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 307,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title AS [TSA items where external cost is greater than FTE cost for entire duration] FROM TSAItems TSA     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID     LEFT JOIN TSAStatuses S         ON TSA.TSAItemTSAStatusId = S.ID WHERE S.[Key] <> 'CANCELED'       AND (TSA.TSAItemExternalMaterialCost > TSA.TSAItemFTECostForServiceDuration)", "CE4-TSA", "List all items where external cost is greater than FTE cost for entire duration." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 308,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "What are the project teams that make up the governance structure for this engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 309,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "How many milestones does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 310,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "project-data", "How many interdependencies does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 311,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "project-data", "How many risks does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 312,
                column: "SuggestionText",
                value: "What PMO workplan templates are available?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 313,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                column: "SuggestionText",
                value: "What is the difference between Progress and Calculated Status?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 315,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 316,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 317,
                column: "SuggestionText",
                value: "How do I add a client user to the PMO app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 318,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 319,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Provide case studies/credentials for similar deals that EY has supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 321,
                column: "SuggestionText",
                value: "What cost savings levers are available?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 322,
                column: "SuggestionText",
                value: "What revenue growth levers are available?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 323,
                column: "SuggestionText",
                value: "What are typical one-time costs that we should be considering?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 324,
                column: "SuggestionText",
                value: "What are the IT cost savings levers that I should be thinking about?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 325,
                column: "SuggestionText",
                value: "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 326,
                column: "SuggestionText",
                value: "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 328,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 329,
                column: "SuggestionText",
                value: "What normative operating models are available?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 330,
                column: "SuggestionText",
                value: "How can I upload systems in bulk to the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 331,
                column: "SuggestionText",
                value: "What are the steps to setup the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 332,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 333,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "Provide case studies/credentials for similar deals that EY has supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 334,
                column: "SuggestionText",
                value: "What TSAs would you suggest?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 335,
                column: "SuggestionText",
                value: "What are examples for TSAs?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 336,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 337,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 338,
                column: "SuggestionText",
                value: "What PMO workplan templates are available?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 339,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 340,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 341,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 342,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 343,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 344,
                column: "SuggestionText",
                value: "What are the key risks for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 345,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 346,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 347,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 348,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 349,
                column: "SuggestionText",
                value: "What are the best cost saving initiatives?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 350,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 351,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 352,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 353,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 354,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What should be the frequency of tracking dollar values during the engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 355,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 356,
                column: "SuggestionText",
                value: "What is a normative operating model for the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 357,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are key considerations when defining an operating model for a {Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 358,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 359,
                column: "SuggestionText",
                value: "What are the corporate functions typically involved in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 360,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 361,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 362,
                column: "SuggestionText",
                value: "Provides templates of TSA.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 363,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What should be the typical duration for TSA?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 364,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Things I should keep in mind for longer duration TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 365,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 366,
                column: "SuggestionText",
                value: "What are the key risks for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 367,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.InsertData(
                table: "AssistantSuggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy", "VisibleToAssistant" },
                values: new object[,]
                {
                    { 368, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System", true },
                    { 369, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System", true }
                });
        }
    }
}
