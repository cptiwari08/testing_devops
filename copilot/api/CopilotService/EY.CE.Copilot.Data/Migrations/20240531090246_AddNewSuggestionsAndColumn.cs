using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSuggestionsAndColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisibleToAssistant",
                table: "AssistantSuggestions",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 1,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 2,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 3,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 4,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 5,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 6,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '{Username}'", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '{Username}'", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 9,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 10,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 11,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 12,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 14,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 15,
                column: "VisibleToAssistant",
                value: true);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT W.Title AS  WorkPlanItem FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID WHERE UP.Title = {Username} ", "CE4-PMO", "List out workplan items assigned to User", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT TOP 1 CAST(PeriodEndDate AS DATE) AS WeeklyStatusDueDate FROM ReportingPeriods WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE) ORDER BY Modified DESC", "CE4-PMO", "When is my weekly status report due?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS FunctionalOwner FROM ProjectTeams PT JOIN Userprofiles UP ON PT.ItemOwnerID = UP.ID WHERE PT.Title = {ProjectTeam}", "CE4-PMO", "Who is the functional owner of HR team?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT WP.Title FROM Workplan WP JOIN ProjectTeams PT ON WP.ProjectTeamId = PT.ID JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.ID WHERE S.[Key] = 'BEHIND_SCHEDULE' AND PT.Title = {ProjectTeam}", "CE4-PMO", "List out Behind Schedule items for HR team", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title FROM Workplan WHERE DATEPART(ww,TaskDueDate) = DATEPART(ww,GETDATE()) + 1", "CE4-PMO", "List out workplan items due next week", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SF.Title FROM Subfunctions SF JOIN Functions F ON SF.FunctionID = F.ID WHERE F.Title = {ProjectTeam}", "CE4-PMO", "List out sub functions of Finance", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT WS.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate)", "CE4-PMO", "What was the status of Finance team last week?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT AN.Title, AN.AccomplishmentNextstepCategory FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) ORDER BY AN.AccomplishmentNextstepCategory", "CE4-PMO", "What were the accomplishment and next steps of Finance team last week?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title  FROM Interdependencies  WHERE CAST(ItemDueDate AS DATE) < CAST(GETDATE() AS DATE)", "CE4-PMO", "List out overdue interdependencies", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT TOP 1 PT.Title Team, SUM(Counts) TotalInterdependencies FROM ( SELECT ProviderProjectTeamID TeamID, Count(ID) Counts FROM Interdependencies WHERE CAST(ItemDueDate AS DATE) < CAST(GETDATE() AS DATE) GROUP BY ProviderProjectTeamID UNION ALL SELECT ReceiverProjectTeamID, Count(ID) FROM Interdependencies WHERE CAST(ItemDueDate AS DATE) < CAST(GETDATE() AS DATE) GROUP BY ReceiverProjectTeamID ) SUB JOIN ProjectTeams PT ON SUB.TeamID = PT.ID GROUP BY PT.Title ORDER BY TotalInterdependencies DESC", "CE4-PMO", "List out team with most overdue interdependencies", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title AS RisksWithNoMitigation FROM RisksandIssues WHERE RiskMitigation IS NULL AND IssueRiskCategory = 'Risk'", "CE4-PMO", "List out Risks with no mitigation plan in place", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title AS RisksWithNoOwner FROM RisksandIssues WHERE ItemOwnerID IS NULL AND IssueRiskCategory = 'Risk'", "CE4-PMO", "List out Risks with no owners assigned to them", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title As Milestones FROM Workplan W WHERE WorkplanTaskType = 'Milestone' AND CAST(StartDate AS DATE) > CAST(GETDATE() AS DATE) AND EXISTS (SELECT 1 FROM [dbo].[WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues] WHERE WorkplanID = W.ID)", "CE4-PMO", "Show me upcoming milestones that have risks linked to it?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title AS Team FROM ProjectTeams PT JOIN TeamTypes TT ON PT.TeamTypeID = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT'", "CE4-PMO", "How many teams do I have in PMO?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Child.Title AS Teams FROM ProjectTeams Parent JOIN ProjectTeams Child ON Child.ParentProjectTeamId = Parent.ID WHERE Parent.Title = {ProjectTeam}", "CE4-PMO", "What are child teams of Finance?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Parent.Title AS ParentTeam FROM ProjectTeams Parent JOIN ProjectTeams Child ON Child.ParentProjectTeamId = Parent.ID WHERE Child.Title = {ProjectTeam}", "CE4-PMO", "What is the parent team of Tax", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT AN.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN UserProfiles UP ON PT.ItemOwnerID = UP.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE UP.EMail = {Username} AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Accomplishment'", "CE4-PMO", "What are my accomplishments from last reporting period ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT AN.Title AS NextSteps FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT ID FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Next Step'", "CE4-PMO", "What are next steps for Finance team?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT I.Title FROM Interdependencies I JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.EMail = {Username} ", "CE4-PMO", "List the interdependencies for which I'm the provider ", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Count(ID) IssueCoutnwithNoMitigationPlan FROM RisksandIssues WHERE RiskMitigation IS NULL AND IssueRiskCategory = 'Issue'", "CE4-PMO", "How many issues don't have a mitigation plan ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UniqueItemIdentifier,Title FROM WORKPLAN WHERE TaskOwnerId IS NULL AND WorkPlanTaskType = 'Task'", "CE4-PMO", "List tasks that have missing owners", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(1) [HighRiskItemCount] FROM RisksAndIssues R JOIN ProjectTeams PT ON PT.ID = R.ProjectTeamId WHERE RiskImpact = 'High' AND IssueRiskCategory = 'Risk' AND PT.Title ={ProjectTeam}", "CE4-PMO", "How many high risk items are there for IT ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  W.Title,'Workplan' [Category] FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID WHERE UP.Title = {Username} UNION ALL  SELECT RI.Title,IssueRiskCategory [Category] FROM RisksAndIssues RI LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID WHERE UP.Title = {Username} UNION ALL SELECT A.Title,'Action' [Category] FROM Actions A LEFT JOIN UserProfiles UP ON A.ItemOwnerId = UP.ID WHERE UP.Title = {Username} UNION ALL  SELECT D.Title,'Decision' [Category] FROM Decisions D LEFT JOIN UserProfiles UP ON D.ItemOwnerId = UP.ID WHERE UP.Title = {Username} UNION ALL  SELECT I.Title,'Interdependecy as Provider' [Category] FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.Title = {Username}  UNION ALL  SELECT I.Title,'Interdependecy as Receiver' [Category] FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ReceiverOwnerId = UP.ID WHERE UP.Title = {Username}", "CE4-PMO", "List all workplan, raid and interdependencies assigned to Amil Shah", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title [Project Team] FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID JOIN WeeklyStatusStatuses WS ON WS.ID = PSE.WeeklyStatusStatusId WHERE  RP.ID = (SELECT ID  FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND WS.[KEY] IN ('AT_RISK','BEHIND_SCHEDULE')", "CE4-PMO", "Are there any project teams that are 'At Risk' or 'Behind Schedule'", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Count(W.ID) as TasksCompletedLastWeek FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID  WHERE  CAST(ActualEndDate AS DATE) >= DATEADD(week,DATEDIFF(week, 0, GETDATE()) -1,0) AND CAST(ActualEndDate AS DATE) < DATEADD(week, DATEDIFF(week, 0, GETDATE()), 0) AND S.[KEY] IN ('COMPLETED')", "CE4-PMO", "How many workplan tasks were completed last week?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(W.ID) as TasksCompletedThisWeek  FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID  WHERE CAST(ActualEndDate AS DATE) >= CAST(DATEADD(week,DATEDIFF(week,0,GETDATE()), 0) AS DATE) AND CAST(ActualEndDate AS DATE) < CAST( DATEADD(week,DATEDIFF(week,0, GETDATE()) + 1,0) AS DATE) AND S.[KEY] IN ('COMPLETED') ", "CE4-PMO", "How many workplan tasks are completed this week?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(W.ID) as TasksDueThisWeek  FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID WHERE CAST(TaskDueDate AS DATE) >= CAST(DATEADD(week, DATEDIFF(week, 0, GETDATE()),0) AS DATE) AND CAST(TaskDueDate AS DATE) < CAST( DATEADD(week, DATEDIFF(week,0, GETDATE()) + 1, 0) AS DATE) AND S.[KEY] NOT IN ('COMPLETED', 'CANCELLED') ", "project-data", "How many workplan tasks that are due this week?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(W.ID) as TasksDueNextWeek FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID  WHERE CAST(TaskDueDate AS DATE) >=  CAST(DATEADD(week, DATEDIFF(week,0, GETDATE()) + 1,0) AS DATE)   AND CAST(TaskDueDate AS DATE) < CAST(DATEADD(week,DATEDIFF(week,0,GETDATE()) + 2,0) AS DATE)   AND S.[KEY] NOT IN ('COMPLETED', 'CANCELLED') ", "project-data", "How many workplan tasks that are due next week?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT W.ID,W.Title FROM Workplan W LEFT JOIN Workstreams WS On  W.WorkstreamId=Ws.ID WHERE WS.Title LIKE {Workstream}", "project-data", "How can I export workplan for just Finance workstream?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " SELECT COUNT(WR.ID) Count,'Risks' as Category FROM Workplan W JOIN WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WR On WR.WorkPlanId = W.ID LEFT JOIN RisksAndIssues RI ON RI.ID = WR.RisksAndIssueId WHERE RI.IssueRiskCategory = 'Risk'  UNION ALL SELECT COUNT(WR.ID) Count, 'Issues' as Category FROM Workplan W JOIN WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WR On WR.WorkPlanId = W.ID LEFT JOIN RisksAndIssues RI ON RI.ID = WR.RisksAndIssueId  WHERE RI.IssueRiskCategory = 'Issue'  UNION ALL SELECT COUNT(WA.ID) Count, 'Actions' as Category  FROM Workplan W JOIN WorkPlansToActionsForAssociatedActions WA On WA.WorkPlanId = W.ID  UNION ALL SELECT    COUNT(WD.ID) Count,    'Decisions' as Category  FROM    Workplan W    JOIN WorkPlansToDecisionsForAssociatedDecisions WD On WD.WorkPlanId = W.ID", "project-data", "How many Risks, Issues, Actions and Decisions linked to Workplan Tasks?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT WSS.Title StatusValue ,'CurrentStatus' as Category FROM ProjectStatusEntries PSE LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT TOP 1 ReportingPeriods.ID FROM ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC) AND PT.Title like {ProjectTeam}  UNION SELECT WSS.Title StatusValue ,'PreviousStatus' as Category FROM ProjectStatusEntries PSE  LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT  ReportingPeriods.ID FROM   ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC Offset 1 Rows FETCH NEXT 1 ROWS ONLY) AND  PT.Title like {ProjectTeam}", "project-data", "What's prior week and current week status for Finance function?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT RI.UniqueItemIdentifier, RI.Title, 'Risks' as Category FROM RisksAndIssues RI LEFT JOIN Statuses S ON RI.ItemStatusId = S.ID WHERE S.[KEY] NOT IN ('COMPLETED', 'CANCELLED', 'CLOSED', 'ON_HOLD') AND RI.IssueRiskCategory = 'Risk' UNION  SELECT RI.UniqueItemIdentifier, RI.Title, 'Issues' as Category FROM RisksAndIssues RI LEFT JOIN Statuses S ON RI.ItemStatusId = S.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'CANCELLED', 'CLOSED', 'ON_HOLD') AND RI.IssueRiskCategory = 'Issue'  UNION  SELECT A.UniqueItemIdentifier,  A.Title,  'Actions' as Category FROM  Actions A  LEFT JOIN Statuses S ON A.ItemStatusId = S.ID WHERE  S.[KEY] NOT IN (   'COMPLETED', 'CANCELLED', 'CLOSED',   'ON_HOLD'  ) UNION SELECT  D.UniqueItemIdentifier,  D.Title,  'Decisions' as Category FROM  Decisions D  LEFT JOIN Statuses S ON D.ItemStatusId = S.ID WHERE  S.[KEY] NOT IN (   'COMPLETED', 'CANCELLED', 'CLOSED',   'ON_HOLD'  ) ", "project-data", "List all Open Risks, Issues, Actions and Decisions", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as OpenInterdependencies, S.Title as Status FROM Interdependencies I LEFT JOIN InterdependencyStatuses S ON I.InterdependencyStatusId = S.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'Canceled') ", "project-data", "List all Open Interdependencies", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " SELECT W.UniqueItemIdentifier, W.TITLE as CriticalTasks FROM Workplan W WHERE W.IsCritical = 1", "project-data", "List all critical workplan tasks?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as ITIntProvider FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ProviderProjectTeamID WHERE PT.Title = {ProjectTeam} ", "project-data", "List all Interdependencies where IT is Interdependency Provider", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title as ITIntReceiver FROM Interdependencies I LEFT JOIN PROJECTTEAMS PT ON PT.ID = I.ReceiverProjectTeamId WHERE PT.Title = {ProjectTeam} ", "CE4-PMO", "project-data", "List all Interdependencies where IT is Interdependency Receiver", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(ID) as CountofCriticalPath FROM Workplan W WHERE W.IsCritical = 1 and W.WorkPlanTaskType = 'Milestone'", "CE4-PMO", "project-data", "How many critical milestones in workplan?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(W.ID) AS TasksDueIn30Days FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID WHERE CAST(W.TaskDueDate AS DATE) <= CAST(GETDATE() + 30 AS DATE) AND CAST(W.TaskDueDate AS DATE) >= CAST(GETDATE() AS DATE) AND S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED')", "CE4-PMO", "project-data", "How many tasks are due in next 30 days?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UniqueItemIdentifier,Title TasksWithoutOwner FROM WorkPlan W WHERE W.TaskOwnerId IS NULL ", "CE4-PMO", "project-data", "List any workplan items that do not have an owner assigned.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UniqueItemIdentifier,Title TasksWithoutOwner FROM WorkPlan W WHERE W.TaskOwnerId IS NULL", "CE4-PMO", "project-data", "List any workplan items that do not have an owner assigned.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UniqueItemIdentifier,Title ItemsWithoutOwner, 'RisksAndIssues' AS Category FROM RisksAndIssues RI WHERE RI.ItemOwnerId IS NULL UNION SELECT UniqueItemIdentifier,Title ItemsWithoutOwner, 'Actions' AS Category FROM Actions RI WHERE RI.ItemOwnerId IS NULL UNION SELECT UniqueItemIdentifier,Title ItemsWithoutOwner, 'Decisions' AS Category FROM Decisions RI WHERE RI.ItemOwnerId IS NULL", "CE4-PMO", "project-data", "List any RAID log items that do not have an owner assigned.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForProviderTasks WTP ON WTP.InterdependencyId = I.ID WHERE WTP.ID IS NULL  UNION  SELECT I.UniqueItemIdentifier, I.Title AS InterdependencyWithoutTasks FROM Interdependencies I LEFT JOIN WorkPlansToInterdependenciesForReceiverTasks WTR ON WTR.InterdependencyId = I.ID WHERE WTR.ID IS NULL;", "CE4-PMO", "project-data", "List interdependencies that do not have a provider or receiver task linked.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT UP.Title AS UserName FROM WorkPlan W LEFT JOIN UserProfiles UP ON UP.ID = W.TaskOwnerId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId WHERE S.[Key] IN ('AT_RISK','BEHIND_SCHEDULE') AND UP.Title IS NOT NULL", "CE4-PMO", "project-data", "Show me a list of all users that have behind schedule or at risk items assigned to them ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT RI.Title RisksAndIssues, IssueRiskCategory FROM RisksAndIssues RI LEFT JOIN Statuses S ON S.ID = RI.ItemStatusId WHERE IsCritical IN (1) AND S.[Key] NOT IN ('CLOSED')", "CE4-PMO", "project-data", "Show me all Risks and issues that are flagged as Critical and not complete", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT ProjectTeam FROM( SELECT PT.Title AS Projectteam, COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID)) ROW_NUM FROM WorkPlan W LEFT JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId WHERE CAST(W.Modified AS DATE) BETWEEN DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND PT.ItemIsActive=1 GROUP BY pt.title) A", "CE4-PMO", "project-data", "Which functions has made the least # of updates to their workplan in last week?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title AS [List of Critical Milestones due in next 15 days with no owners Assigned yet] FROM WorkPlan WHERE WorkPlanTaskType IN ('Milestone') AND CAST(TaskDueDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 15, CAST(GETDATE() AS DATE)) AND IsCritical IN (1) AND TaskOwnerId IS NULL", "CE4-PMO", "project-data", "Can you please list down all milestones due in next 15 days which are on critical path and have no owners assigned to them?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title as [List down all tasks associated with interdependencies] from Workplan where ID in ( SELECT WorkPlanId FROM WorkPlansToInterdependenciesForReceiverTasks UNION select WorkPlanId FROM WorkPlansToInterdependenciesForProviderTasks) AND WorkPlanTaskType='Task'", "CE4-PMO", "project-data", "Can you please list down all tasks associated with interdependencies?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForReceiverTasks WPRT ON W.id = WPRT.WorkPlanId LEFT JOIN Interdependencies IR ON WPRT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task' UNION SELECT W.Title AS WorkplanAssociatedWithInterdependencies FROM WorkPlan W INNER JOIN WorkPlansToInterdependenciesForProviderTasks WPPT ON W.id = WPPT.WorkPlanId LEFT JOIN Interdependencies IR ON WPPT.InterdependencyId = IR.ID WHERE CAST(W.TaskDueDate AS DATE) < CAST(IR.ItemDueDate AS DATE) AND W.WorkPlanTaskType='Task'", "CE4-PMO", "project-data", "Can you please list down all tasks whose due date is less than the due date of the associated interdependency", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "CE4-VC", "project-data", "What are our targets for this engagement?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '{Username}' GROUP BY S.Title ", "CE4-VC", "project-data", "How are my initiatives doing?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '{Username}' ", "CE4-VC", "project-data", "Are there any risks or issues with initiatives that I’m responsible for?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '{Username}' ", "CE4-VC", "project-data", "How  many initiatives are assigned to me? ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '{Username}' ", "CE4-VC", "project-data", "Give my cost reduction initiatives ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(RevenueGrowthEstimate,0)) [RevenueGrowthTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title = {ProjectTeam}", "CE4-VC", "project-data", "What are the revenue growth targets for Sales & Marketing ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(CostToAchieveEstimate,0)) [CostToAchieveTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title ={ProjectTeam}", "CE4-VC", "project-data", "What are the cost to achieve targets for R&D ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT S.Title [Value Capture Stage] , COUNT(VI.ID) [No. of Initiatives] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} GROUP BY S.Title", "CE4-VC", "project-data", "How many initiatives are there in IT across different stages ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Approved Initiaves] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} AND S.Title = 'Approved'", "CE4-VC", "project-data", "List IT initiatives that are in approved stage", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Initiaves with Workplan] FROM ValueCaptureInitiatives VI JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WV ON VI.ID = WV.ValueCaptureInitiativeId", "CE4-VC", "project-data", "List initiatives that have workplan item linked to them", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT VI.Title [Initiaves with Risk] FROM ValueCaptureInitiatives VI JOIN RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RV ON VI.ID = RV.ValueCaptureInitiativeId LEFT JOIN RisksAndIssues RI ON RI.ID = RV.RisksAndIssueId WHERE RI.IssueRiskCategory = 'Risk'", "CE4-VC", "project-data", "How many initiatives have Risks linked ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) [HeadCountCostReductionTarget] FROM [ValueCaptureTopDownEstimates] VT", "CE4-VC", "project-data", "What is the total headcount cost reduction target ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Non Active Initiaves] FROM ValueCaptureInitiatives VI WHERE ISNULL(VI.IsItemActive ,0) = 0", "CE4-VC", "project-data", "List initiatives that are not active", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages  VCS ON VCI.ValueCaptureStageId = VCS.ID WHERE VCS.Title = {ValueCaptureStage} ", "CE4-VC", "project-data", "How many initiatives have been realized?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(NonHeadcountCostReductionEst) + SUM(RevenueGrowthEstimate) + SUM(HeadcountCostReductionEst) 'Total Top Down Target Value' FROM ValueCaptureTopDownEstimates", "CE4-VC", "project-data", "What is the Total Top Down Target Value ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(RevenueGrowthEstimate) RevenueGrowth,SUM(TotalCostReduction) CostReduction FROM ValueCaptureTopDownEstimates", "project-data", "Show my top down target values by cost reduction and revenue growth", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(ID) FROM ValueCaptureInitiatives WHERE IsItemActive = 1", "project-data", "How many active initatives are there in my project ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title [Project Team] ,CAST(SUM(ISNULL(RevenueGrowthEstimate,0))/1000000 AS FLOAT) [Revenue Growth(in Million)] FROM [ValueCaptureTopDownEstimates] VT 	LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId GROUP BY PT.Title", "project-data", "What is the total of revenue growth by team?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VC.Title AS [ValueCaptureInitiatives with no Owners] FROM ValueCaptureInitiatives VC WHERE ItemOwnerId IS NULL", "project-data", "List out initiatives with no owners assigned.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Initiative with Workplan at Risk] FROM WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WTVI LEFT JOIN Workplan W ON W.ID = WTVI.WorkPlanId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId LEFT JOIN ValueCaptureInitiatives VI on VI.ID = WTVI.ValueCaptureInitiativeId WHERE S.[KEY] = 'AT_RISK'", "project-data", "List out initiatives that have at risk workplan task linked.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VP.Title AS ValueCapturepriority, COUNT(VC.ValueCapturePriorityId) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId where ValueCapturePriorityId is not null GROUP BY ValueCapturePriorityId, VP.Title", "project-data", "Show me initiatives count by priority.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS UserProfile, COUNT(VC.ID) AS ValueCaptureOwnerwiseCount FROM ValueCaptureInitiatives VC LEFT JOIN UserProfiles UP ON UP.ID = VC.ItemOwnerId WHERE ItemOwnerId IS NOT NULL GROUP BY ItemOwnerId, UP.Title", "project-data", "Show me initiatives count by owner.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Initiaves] FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Title = {Username}", "CE4-VC", "project-data", "List initiatives where I'm assigned as the Owner", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as IdentifiedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Identified'", "CE4-VC", "project-data", "How many initiatives are identified?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ApprovedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Approved' ", "CE4-VC", "project-data", "How many initiatives have been approved?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ValidatedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Validated' ", "CE4-VC", "project-data", "How many initiatives have been validated?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS [Value Capture Stage] ,COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCI.ValueCaptureStageId = VCS.ID GROUP BY VCS.Title", "CE4-VC", "project-data", "Can you provide breakdown of the initiatives across various stages?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title AS Initiatives FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username}", "CE4-VC", "project-data", "What initatives have I been assigned to ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VS.Title ValueCaptureStage FROM ValueCaptureInitiatives VI JOIN ValueCaptureStages VS ON VI.ValueCaptureStageID = VS.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureStage", "CE4-VC", "project-data", "List my initatives by stages", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VT.Title ValueCaptureType FROM ValueCaptureInitiatives VI JOIN ValueCaptureTypes VT ON VI.ValueCaptureStageID = VT.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureType", "CE4-VC", "project-data", "List my initatives by Value Capture Type", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title,SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(RevenueGrowthEstimate,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)) AS TotalTopDownTarget FROM ValueCaptureTopDownEstimates LEFT JOIN ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY PT.Title", "CE4-VC", "project-data", "What is the top-down target for this project?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS ValueCaptureStage, COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VC.ValueCaptureStageId GROUP BY ValueCaptureStageId, VCS.Title", "CE4-VC", "project-data", "Show me initiatives count by stage.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT F.Title as Functions, COUNT(VC.FunctionId) AS ValueCaptureFunction FROM ValueCaptureInitiatives VC LEFT JOIN Functions f ON f.ID = VC.FunctionId WHERE FunctionId IS NOT NULL GROUP BY FunctionId, F.Title ", "CE4-VC", "project-data", "Show me initiatives count by function.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "project-data", "Provide a summary of Day 1 process dispositions.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "project-data", "How many systems are tagged to Current State processes?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "CE4-OM", "project-data", "How many systems are tagged to Day 1 processes?", true });

            migrationBuilder.InsertData(
                table: "AssistantSuggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy", "VisibleToAssistant" },
                values: new object[,]
                {
                    { 100, "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "CE4-OM", null, "System", "project-data", "How many assets are tagged to Day 1 processes? ", null, "System", true },
                    { 101, "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "CE4-OM", null, "System", "project-data", "How many assets are tagged to Current State processes? ", null, "System", true },
                    { 102, "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(Title) AS TotalCount, BusinessEntityId FROM Nodes WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", null, "System", "project-data", "Can you provide me number of processes by op model? ", null, "System", true },
                    { 103, "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", null, "System", "project-data", "How many processes we have in current state? ", null, "System", true },
                    { 104, "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", null, "System", "project-data", "How many processes we have in Day1 state? ", null, "System", true },
                    { 105, "SELECT COUNT(1) FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'  AND N.NodeTypeId = 3  AND N.ID NOT IN (   SELECT DISTINCT NTO.NodeId   FROM Nodes N1   LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null  )", "CE4-OM", null, "System", "project-data", "How many processes don't have an Owner assigned in Current State ?", null, "System", true },
                    { 106, "SELECT PR.ID 	,PR.Title FROM Nodes PR JOIN TransactionStates T ON T.ID = PR.TransactionStateId WHERE T.[Key] = 'DAY_ONE' 	AND PR.NodeTypeId = 3 	AND NOT EXISTS  				(SELECT 1 FROM [NodesToDispositionsForDispositionNew] D WHERE D.NodeId = PR.ID)", "CE4-OM", null, "System", "project-data", "List processes that don't have Disposition assigned in Day 1 state", null, "System", true },
                    { 107, "SELECT Title FROM Dispositions", "CE4-OM", null, "System", "project-data", "List the Disposition options available ", null, "System", true },
                    { 108, "SELECT N.Title [ProcessGroup] ,COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) [Enablers Count] FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 2 GROUP BY N.Title ORDER BY [Enablers Count] DESC", "CE4-OM", null, "System", "project-data", "How many enablers are associated with each Process Group ?", null, "System", true },
                    { 109, "SELECT D.Title [Disposition] 	,COUNT(SD.SystemId) [SystemCount] FROM Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID GROUP BY D.Title", "CE4-OM", null, "System", "project-data", "List the total number of systems by disposition", null, "System", true },
                    { 110, " SELECT PR.Title [Process] , D.DispositionId FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId  	LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 	AND D.NodeId IS NULL 	AND S.[Key] = 'DAY_ONE'", "CE4-OM", null, "System", "project-data", "Can you please list down all Day 1 processes where no disposition has been tagged?", null, "System", true },
                    { 111, " SELECT 	N.Title [Process]	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 3 GROUP BY N.Title HAVING COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) = 0", "CE4-OM", null, "System", "project-data", "Can you please list all processes with no Enablers?", null, "System", true },
                    { 112, "SELECT PR.Title as MissingInDay1 FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'CURRENT_STATE' EXCEPT SELECT 	PR.Title [Process] FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'DAY_ONE'", "CE4-OM", null, "System", "project-data", "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state", null, "System", true },
                    { 113, "SELECT DISTINCT A.Title Enabler, 'Asset' as Category	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID LEFT JOIN [Assets] A ON A.ID = NA.AssetsId WHERE A.Title IS NOT NULL UNION ALL SELECT DISTINCT F.Title Enabler, 'Facility' as Category FROM Nodes N LEFT JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID LEFT JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE F.Title IS NOT NULL  UNION ALL SELECT DISTINCT S.Title Enabler , 'System' as Category FROM Nodes N LEFT JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID LEFT JOIN [Systems] S ON S.ID = NS.SystemsId WHERE S.Title IS NOT NULL  UNION ALL SELECT DISTINCT T.Title Enabler	, 'TPA' as Category FROM Nodes N LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID LEFT JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId WHERE T.Title IS NOT NULL ", "CE4-OM", null, "System", "project-data", "What Enablers are we tracking for this project ?", null, "System", true },
                    { 114, "SELECT Count(ID) AS [# Systems present in this functional OP model] FROM Systems", "CE4-OM", null, "System", "project-data", "How many Systems are there in this functional op model ?", null, "System", true },
                    { 115, "SELECT Count(ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements", "CE4-OM", null, "System", "project-data", "How many TPAs are there in this functional op model ?", null, "System", true },
                    { 116, "SELECT ST.Title AS SystemType, COUNT(S.ID) [# Systems by Type] FROM Systems S LEFT JOIN SystemTypes ST ON ST.ID = S.TypeId WHERE S.TypeId IS NOT NULL GROUP BY ST.Title", "CE4-OM", null, "System", "project-data", "Lsit the number of Systems by Type", null, "System", true },
                    { 117, "SELECT O.Title AS Owners, Count(*) AS [# TPAs by Ownership] FROM ThirdPartyAgreements TP LEFT JOIN Owners O ON O.ID = TP.OwnerID WHERE TP.OwnerID IS NOT NULL GROUP BY O.Title", "CE4-OM", null, "System", "project-data", "List the number of TPAs by Ownership", null, "System", true },
                    { 118, "SELECT Title [ProcessGroups With No Process] FROM Nodes WHERE nodetypeid = 2 AND ID NOT IN (SELECT NodeParentId FROM Nodes)", "CE4-OM", null, "System", "project-data", "List down all process groups with no process within them?", null, "System", true },
                    { 119, "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", null, "System", "project-data", "Show me the count of TSAs by function.", null, "System", true },
                    { 120, "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", null, "System", "project-data", "Show me the count of TSAs by sub function.", null, "System", true },
                    { 121, "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", null, "System", "project-data", "Give me the list of TSAs by Provider ", null, "System", true },
                    { 122, "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", null, "System", "project-data", "Give me the list of TSAs by Receiver ", null, "System", true },
                    { 123, "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", null, "System", "project-data", "Can you provide me list of TSAs by phases? ", null, "System", true },
                    { 124, "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", null, "System", "project-data", "Show me the breakdown of TSAs by duration  ", null, "System", true },
                    { 125, "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", null, "System", "project-data", "How many TSAs does each team have? ", null, "System", true },
                    { 126, "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", null, "System", "project-data", "What are the project teams that make up the governance structure for this engagement? ", null, "System", true },
                    { 127, "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", null, "System", "project-data", "How many milestones does each team have? ", null, "System", true },
                    { 128, "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", null, "System", "project-data", "How many interdependencies does each team have ", null, "System", true },
                    { 129, "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", null, "System", "project-data", "How many risks does each team have? ", null, "System", true },
                    { 130, null, "CE4-PMO", null, "System", "ey-guidance", "What PMO workplan templates are available?", null, "System", true },
                    { 131, null, "CE4-PMO", null, "System", "ey-guidance", "What are the best practices to run weekly status meetings with the client?", null, "System", true },
                    { 132, null, "CE4-PMO", null, "System", "ey-guidance", "What is the difference between Progress and Calculated Status?", null, "System", true },
                    { 133, null, "CE4-PMO", null, "System", "ey-guidance", "How do you add a new field to the workplan?", null, "System", true },
                    { 134, null, "CE4-PMO", null, "System", "ey-guidance", "How do I set alerts/send email for various item owners?", null, "System", true },
                    { 135, null, "CE4-PMO", null, "System", "ey-guidance", "How do I add a client user to the PMO app?", null, "System", true },
                    { 136, null, "CE4-PMO", null, "System", "ey-guidance", "How do I link a Workplan Task to RAID?", null, "System", true },
                    { 137, null, "CE4-PMO", null, "System", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past", null, "System", true },
                    { 138, null, "CE4-PMO", null, "System", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project.", null, "System", true },
                    { 139, null, "CE4-VC", null, "System", "ey-guidance", "What cost savings levers are available?", null, "System", true },
                    { 140, null, "CE4-VC", null, "System", "ey-guidance", "What revenue growth levers are available?", null, "System", true },
                    { 141, null, "CE4-VC", null, "System", "ey-guidance", "What are typical one-time costs that we should be considering?", null, "System", true },
                    { 142, null, "CE4-VC", null, "System", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?", null, "System", true },
                    { 143, null, "CE4-VC", null, "System", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?", null, "System", true },
                    { 144, null, "CE4-VC", null, "System", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?", null, "System", true },
                    { 145, null, "CE4-VC", null, "System", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past", null, "System", true },
                    { 146, null, "CE4-VC", null, "System", "ey-guidance", "Help understand VC methodology.", null, "System", true },
                    { 147, null, "CE4-OM", null, "System", "ey-guidance", "What normative operating models are available?", null, "System", true },
                    { 148, null, "CE4-OM", null, "System", "ey-guidance", "How can I upload systems in bulk to the Op Model app?", null, "System", true },
                    { 149, null, "CE4-OM", null, "System", "ey-guidance", "What are the steps to setup the Op Model app?", null, "System", true },
                    { 150, null, "CE4-OM", null, "System", "ey-guidance", "What reports are available in the Op Model app?", null, "System", true },
                    { 151, null, "CE4-OM", null, "System", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past", null, "System", true },
                    { 152, null, "CE4-TSA", null, "System", "ey-guidance", "What TSAs would you suggest?", null, "System", true },
                    { 153, null, "CE4-TSA", null, "System", "ey-guidance", "What are examples for TSAs?", null, "System", true },
                    { 154, null, "CE4-TSA", null, "System", "ey-guidance", "Why are TSAs important?", null, "System", true },
                    { 155, null, "CE4-TSA", null, "System", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past.", null, "System", true },
                    { 156, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "What PMO workplan templates are available?", null, "System", true },
                    { 157, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "What are the best practices to run weekly status meetings with the client?", null, "System", true },
                    { 158, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "How do you add a new field to the workplan?", null, "System", true },
                    { 159, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "How do I link a Workplan Task to RAID?", null, "System", true },
                    { 160, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project.", null, "System", true },
                    { 161, null, "CE4-PMO", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System", true },
                    { 162, null, "CE4-PMO", null, "System", "internet", "What are the key risks for a {ProjectType} project?", null, "System", true },
                    { 163, null, "CE4-PMO", null, "System", "internet", "What are the key milestones for a {ProjectType} project?", null, "System", true },
                    { 164, null, "CE4-PMO", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System", true },
                    { 165, null, "CE4-PMO", null, "System", "internet", "What are some of the similar deals that have happened in the past?", null, "System", true },
                    { 166, null, "CE4-PMO", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System", true },
                    { 167, null, "CE4-VC", null, "System", "internet", "What are the best cost saving initiatives?", null, "System", true },
                    { 168, null, "CE4-VC", null, "System", "internet", "What are the best revenue growth initiatives?", null, "System", true },
                    { 169, null, "CE4-VC", null, "System", "internet", "What are the best strategies for improving a company in the {Sector} sector?", null, "System", true },
                    { 170, null, "CE4-VC", null, "System", "internet", "What are recent examples of improvements being made in the {Sector} sector?", null, "System", true },
                    { 171, null, "CE4-VC", null, "System", "internet", "What are the best ways to track actuals?", null, "System", true },
                    { 172, null, "CE4-VC", null, "System", "internet", "What should be the frequency of tracking dollar values during the engagement?", null, "System", true },
                    { 173, null, "CE4-VC", null, "System", "internet", "What are typical implications for cross border deals?", null, "System", true },
                    { 174, null, "CE4-OM", null, "System", "internet", "What is a normative operating model for the {Sector} sector?", null, "System", true },
                    { 175, null, "CE4-OM", null, "System", "internet", "What are key considerations when defining an operating model for a {Sector} sector company?", null, "System", true },
                    { 176, null, "CE4-OM", null, "System", "internet", "What are examples of Day 1 process dispositions?", null, "System", true },
                    { 177, null, "CE4-TSA", null, "System", "internet", "What are the corporate functions typically involved in the {Sector} sector?", null, "System", true },
                    { 178, null, "CE4-TSA", null, "System", "internet", "What are the typical services of the Sales and Marketing function?", null, "System", true },
                    { 179, null, "CE4-TSA", null, "System", "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.", null, "System", true },
                    { 180, null, "CE4-TSA", null, "System", "internet", "Provides templates of TSA", null, "System", true },
                    { 181, null, "CE4-TSA", null, "System", "internet", "What should be the typical duration for TSA", null, "System", true },
                    { 182, null, "CE4-TSA", null, "System", "internet", "Things I should keep in mind for longer duration TSAs", null, "System", true },
                    { 183, null, "PROJECT_LEVEL", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System", true },
                    { 184, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key risks for a {ProjectType} project?", null, "System", true },
                    { 185, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key milestones for a {ProjectType} project?", null, "System", true },
                    { 186, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System", true },
                    { 187, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187);

            migrationBuilder.DropColumn(
                name: "VisibleToAssistant",
                table: "AssistantSuggestions");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 7,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 8,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "CE4-VC", "What are our targets for this engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)/1000000 AS ValuetoAchieve FROM ValueCaptureEstimates VCE JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID WHERE ISNULL(VCE.Recurring,0) = 1 AND VCI.IsItemActive=1 AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1) AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery", "CE4-VC", "How much value are we planning to achieve from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '${Username}' GROUP BY S.Title ", "CE4-VC", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '${Username}' ", "CE4-VC", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX); DECLARE @TotalEstimate DECIMAL(18,2);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT @TotalEstimate = (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)     FROM ValueCaptureEstimates VCE      JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID      WHERE ISNULL(VCE.Recurring,0) = 1      AND VCI.IsItemActive=1     AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1)     AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery, N'@TotalEstimate DECIMAL(18,2) OUTPUT', @TotalEstimate OUTPUT;  WITH SubtractedValue AS (    SELECT 'Total Top Down Target' AS KPI,    FORMAT((@TotalEstimate - (SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate)))/1000000,'N2') AS 'Value (Million)'     FROM ValueCaptureTopDownEstimates ) SELECT * FROM SubtractedValue ", "CE4-VC", "Are our initiative projections exceeding our targets?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '${Username}' ", "CE4-VC", "How  many initiatives are assigned to me? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '${Username}' ", "CE4-VC", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "CE4-OM", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets  are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(Title) AS TotalCount, BusinessEntityId      FROM Nodes      WHERE NodeTypeId = 3      GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "Can you provide me number of processes by op model? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in current state? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "Help me understand PMO methodology for this ${ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "ey-guidance", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "ey-guidance", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "ey-guidance", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "Help me understand PMO methodology for this ${ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "internet", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "internet", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "internet", "What are the best strategies for improving a company in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "internet", "What are recent examples of improvements being made in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "internet", "What are the best ways to track actuals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "internet", "What should be the frequency of tracking dollar values during the engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "internet", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "internet", "What is a normative operating model for the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "internet", "What are key considerations when defining an operating model for a ${Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "internet", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "internet", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "internet", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "internet", "Provides templates of TSA" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "internet", "What should be the typical duration for TSA" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "internet", "Things I should keep in mind for longer duration TSAs" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "internet", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "internet", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "internet", "What are the best practices to build workplan, and track dependencies?" });
        }
    }
}
