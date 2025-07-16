using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSuggestionForProjectData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title FROM Workplan WHERE DATEPART(ww,TaskDueDate) = DATEPART(ww,GETDATE())", "CE4-PMO", "List out workplan items due this week", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT W.Title, W.WorkPlanTaskType, W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title [Status], UP.Title TaskOwner FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE UniqueItemIdentifier = 'HR.2.2.6'", "CE4-PMO", "Provide details for workplan ID HR.2.2.6", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Count(RI.ID) IssueCoutnwithPendingStatus FROM RisksandIssues RI JOIN RiskAndIssueStatuses RS ON RI.ItemStatusID = RS.ID WHERE RiskMitigation IS NULL 	AND IssueRiskCategory = 'Issue' 	AND RS.Title = 'Pending'", "CE4-PMO", "How many issues are in pending status ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  	UniqueItemIdentifier 	,Title 	,IssueRiskCategory FROM RisksAndIssues WHERE  	ItemOwnerId IS NULL  UNION ALL  SELECT  	UniqueItemIdentifier 	,Title 	,'Decision' FROM Decisions WHERE  	ItemOwnerId IS NULL  UNION ALL  SELECT  	UniqueItemIdentifier 	,Title 	,'Action' FROM Actions WHERE  	ItemOwnerId IS NULL", "CE4-PMO", "List RAID items that do not have an owner assigned", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE NotStartedWorkplansWithStartDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId  WHERE    S.[KEY] = 'NOT_STARTED'    AND CAST(W.StartDate as DATE)< CAST(     GETDATE() AS DATE   ) ", "CE4-PMO", "List all Workplan tasks that are 'Not Started' and planned start date has passed", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    W.UniqueItemIdentifier,    W.TITLE as WorkplansWithDueDatePassed  FROM    Workplan W    LEFT JOIN Statuses S On S.ID = W.WorkPlanTaskStatusId  WHERE    S.[KEY] NOT IN ( 'COMPLETED' , 'CANCELLED')   AND CAST(W.TaskDueDate as DATE)< CAST(GETDATE() AS DATE)", "CE4-PMO", "List all Workplan tasks which are past due", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT PT.Title as ProjectTeams_With_Items_At_Risk  FROM ProjectTeams PT    JOIN Workplan W ON W.ProjectTeamID = PT.ID    JOIN Statuses S ON S.ID = W.WorkplanTaskStatusID  WHERE    S.[Key] = 'AT_RISK'", "CE4-PMO", "List all project teams with workplan items that are 'At Risk'" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title FROM ProjectTeams WHERE ManageProjectStatus = 1 EXCEPT SELECT PT.Title FROM ProjectStatusEntries PSE LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodId = RP.ID LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamId = PT.ID WHERE RP.ID = (SELECT ID FROM ReportingPeriods 				WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE) 					) 	AND PSE.WeeklyStatusStatusId IS NOT NULL", "CE4-PMO", "Which teams have not entered thier weekly status report for this reporting period?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  	COUNT(ID) AS NewRisksCount FROM  	RisksAndIssues WHERE  	 IssueRiskCategory like $[IssueRiskCategory] 	AND DATEDIFF(day, Created, GETDATE()) <= 5", "CE4-PMO", "How many new risks have been created in the last 5 days?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Title Workplan FROM (     SELECT          DISTINCT W.Title, WorkPlanLinksTargetId AS TaskID     FROM          WorkPlanLinks WL         LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksTargetId      UNION      SELECT          DISTINCT W.Title, WorkPlanLinksSourceId AS TaskID     FROM          WorkPlanLinks WL         LEFT JOIN WorkPlan W ON W.ID = WL.WorkPlanLinksSourceId ) AS SubQuery ORDER BY TaskID", "CE4-PMO", "Which tasks have a predecessor or successor linked to them?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, 	DENSE_RANK() OVER (ORDER BY COUNT(PT.ID) DESC) ROW_NUM FROM      RisksAndIssues RI LEFT JOIN      ProjectTeams PT ON PT.ID = RI.ProjectTeamId WHERE PT.ItemIsActive=1 and RI.IssueRiskCategory = 'Risk' GROUP BY      pt.title) A", "CE4-PMO", "Which function has the highest number of risk associated with?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT      W.Title AS [List of Milestones due in next 7 days and Not Updated Since Last 2 weeks] FROM      WorkPlan W 	LEFT JOIN Statuses S on s.ID = W.WorkPlanTaskStatusId WHERE      W.WorkPlanTaskType IN ('Milestone')     AND W.IsCritical IN (1)     AND CAST(W.Modified AS DATE) < DATEADD(day, -14, CAST(GETDATE() AS DATE)) 	AND S.[Key] not in ('COMPLETED', 'CANCELLED')", "CE4-PMO", "Can you please list down all milestones due in next 7 days which are on critical path and have no update made to them in last 2 weeks?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     RI.TITLE AS [Risk items that does not have Risk Mitigation plan] FROM     RisksAndIssues RI WHERE     RI.IssueRiskCategory = 'Risk'     AND RI.RiskMitigation IS NOT NULL", "CE4-PMO", "Are there any risks items that doesn't have risk mitigation plan?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "What are our targets for this engagement?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '{Username}' GROUP BY S.Title ", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '{Username}' ", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '{Username}' ", "How  many initiatives are assigned to me? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '{Username}' ", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(RevenueGrowthEstimate,0)) [RevenueGrowthTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title = {ProjectTeam}", "What are the revenue growth targets for Sales & Marketing ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(CostToAchieveEstimate,0)) [CostToAchieveTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title ={ProjectTeam}", "What are the cost to achieve targets for R&D ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT S.Title [Value Capture Stage] , COUNT(VI.ID) [No. of Initiatives] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} GROUP BY S.Title", "How many initiatives are there in IT across different stages ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Approved Initiaves] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} AND S.Title = 'Approved'", "List IT initiatives that are in approved stage", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiaves with Workplan] FROM ValueCaptureInitiatives VI JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WV ON VI.ID = WV.ValueCaptureInitiativeId", "List initiatives that have workplan item linked to them" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT VI.Title [Initiaves with Risk] FROM ValueCaptureInitiatives VI JOIN RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RV ON VI.ID = RV.ValueCaptureInitiativeId LEFT JOIN RisksAndIssues RI ON RI.ID = RV.RisksAndIssueId WHERE RI.IssueRiskCategory = 'Risk'", "How many initiatives have Risks linked ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) [HeadCountCostReductionTarget] FROM [ValueCaptureTopDownEstimates] VT", "What is the total headcount cost reduction target ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Non Active Initiaves] FROM ValueCaptureInitiatives VI WHERE ISNULL(VI.IsItemActive ,0) = 0", "List initiatives that are not active", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages  VCS ON VCI.ValueCaptureStageId = VCS.ID WHERE VCS.Title = {ValueCaptureStage} ", "How many initiatives have been realized?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(NonHeadcountCostReductionEst) + SUM(RevenueGrowthEstimate) + SUM(HeadcountCostReductionEst) 'Total Top Down Target Value' FROM ValueCaptureTopDownEstimates", "What is the Total Top Down Target Value ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(RevenueGrowthEstimate) RevenueGrowth,SUM(TotalCostReduction) CostReduction FROM ValueCaptureTopDownEstimates", "Show my top down target values by cost reduction and revenue growth" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(ID) FROM ValueCaptureInitiatives WHERE IsItemActive = 1", "How many active initatives are there in my project ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title [Project Team] ,CAST(SUM(ISNULL(RevenueGrowthEstimate,0))/1000000 AS FLOAT) [Revenue Growth(in Million)] FROM [ValueCaptureTopDownEstimates] VT 	LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId GROUP BY PT.Title", "What is the total of revenue growth by team?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VC.Title AS [ValueCaptureInitiatives with no Owners] FROM ValueCaptureInitiatives VC WHERE ItemOwnerId IS NULL", "List out initiatives with no owners assigned.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Initiative with Workplan at Risk] FROM WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WTVI LEFT JOIN Workplan W ON W.ID = WTVI.WorkPlanId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId LEFT JOIN ValueCaptureInitiatives VI on VI.ID = WTVI.ValueCaptureInitiativeId WHERE S.[KEY] = 'AT_RISK'", "List out initiatives that have at risk workplan task linked.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VP.Title AS ValueCapturepriority, COUNT(VC.ValueCapturePriorityId) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId where ValueCapturePriorityId is not null GROUP BY ValueCapturePriorityId, VP.Title", "CE4-VC", "Show me initiatives count by priority." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT UP.Title AS UserProfile, COUNT(VC.ID) AS ValueCaptureOwnerwiseCount FROM ValueCaptureInitiatives VC LEFT JOIN UserProfiles UP ON UP.ID = VC.ItemOwnerId WHERE ItemOwnerId IS NOT NULL GROUP BY ItemOwnerId, UP.Title", "CE4-VC", "Show me initiatives count by owner." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiaves] FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Title = {Username}", "CE4-VC", "List initiatives where I'm assigned as the Owner" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as IdentifiedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Identified'", "CE4-VC", "How many initiatives are identified?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ApprovedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Approved' ", "CE4-VC", "How many initiatives have been approved?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ValidatedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Validated' ", "CE4-VC", "How many initiatives have been validated?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS [Value Capture Stage] ,COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCI.ValueCaptureStageId = VCS.ID GROUP BY VCS.Title", "CE4-VC", "Can you provide breakdown of the initiatives across various stages?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username}", "CE4-VC", "What initatives have I been assigned to ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VS.Title ValueCaptureStage FROM ValueCaptureInitiatives VI JOIN ValueCaptureStages VS ON VI.ValueCaptureStageID = VS.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureStage", "CE4-VC", "List my initatives by stages" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VT.Title ValueCaptureType FROM ValueCaptureInitiatives VI JOIN ValueCaptureTypes VT ON VI.ValueCaptureStageID = VT.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureType", "CE4-VC", "List my initatives by Value Capture Type" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title,SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(RevenueGrowthEstimate,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)) AS TotalTopDownTarget FROM ValueCaptureTopDownEstimates LEFT JOIN ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY PT.Title", "CE4-VC", "What is the top-down target for this project?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS ValueCaptureStage, COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VC.ValueCaptureStageId GROUP BY ValueCaptureStageId, VCS.Title", "CE4-VC", "Show me initiatives count by stage.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT F.Title as Functions, COUNT(VC.FunctionId) AS ValueCaptureFunction FROM ValueCaptureInitiatives VC LEFT JOIN Functions f ON f.ID = VC.FunctionId WHERE FunctionId IS NOT NULL GROUP BY FunctionId, F.Title ", "CE4-VC", "Show me initiatives count by function.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT     CC.Title AS [Cost/Revenue Centers linked to PnL Line Items], CC.Account FROM CostCenters CC     LEFT JOIN ValueCaptureFinancialLineItems Fin         ON CC.FinancialLineItemId = Fin.ID     LEFT JOIN ValueCaptureFinancialLineItemTypes FinType         ON Fin.FinancialLineItemTypeId = FinType.ID WHERE FinType.Title = 'PnL'", "CE4-VC", "List all Cost/Revenue Centers linked to PnL Line Items" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT COUNT(ID) AS [Top Down targets #],        SUM(ISNULL(NonHeadcountCostReductionEst, 0)) AS [Total Non Headcount Cost Reduction Estimate],        SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Revenue Growth Estimate],        SUM(ISNULL(CostToAchieveEstimate, 0)) AS [Total Cost To Achieve Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) AS [Total Headcount Cost Reduction Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates ", "CE4-VC", "Can you provide Top Down Targets KPIs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT  VCTT.Title, PT.Title, VCTT.CostToAchieveEstimate, VCTT.NonHeadcountCostReductionEst, VCTT.HeadcountCostReductionEst, VCTT.RevenueGrowthEstimate FROM   ValueCaptureTopDownEstimates VCTT  Left Join ProjectTeams PT on PT.ID = VCTT.ProjectTeamId", "CE4-VC", "How many top down targets do we have for this project ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) AS HeadcountCostReduction FROM ValueCaptureTopDownEstimates", "CE4-VC", "What is the Total Headcount Cost Reduction Target ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT  SUM(ISNULL(NonHeadcountCostReductionEst,0)) AS NonHeadcountCostReduction FROM  ValueCaptureTopDownEstimates", "CE4-VC", "What is the Total Non Headcount Cost Reduction Target ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT  SUM(ISNULL(RevenueGrowthEstimate,0)) AS RevenueGrowth FROM  ValueCaptureTopDownEstimates", "CE4-VC", "What is the Total Revenue Growth Target ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT  SUM(ISNULL(CostToAchieveEstimate,0)) AS CostToAchive FROM  ValueCaptureTopDownEstimates", "CE4-VC", "What is the Total Cost to Achieve Target ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT  PT.Title ProjectTeams, VBM.Title Benchmark FROM  ValueCaptureCostBenchmarks VBM LEFT JOIN  ProjectTeams PT ON PT.ID = VBM.ProjectTeamId", "CE4-VC", "Are there any benchmarks for this project ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "CE4-OM", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(Title) AS TotalCount, BusinessEntityId FROM Nodes WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "Can you provide me number of processes by op model? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in current state? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT COUNT(1) FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'  AND N.NodeTypeId = 3  AND N.ID NOT IN (   SELECT DISTINCT NTO.NodeId   FROM Nodes N1   LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null  )", "CE4-OM", "How many processes don't have an Owner assigned in Current State ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PR.ID 	,PR.Title FROM Nodes PR JOIN TransactionStates T ON T.ID = PR.TransactionStateId WHERE T.[Key] = 'DAY_ONE' 	AND PR.NodeTypeId = 3 	AND NOT EXISTS  				(SELECT 1 FROM [NodesToDispositionsForDispositionNew] D WHERE D.NodeId = PR.ID)", "CE4-OM", "List processes that don't have Disposition assigned in Day 1 state" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Title FROM Dispositions", "CE4-OM", "List the Disposition options available " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT N.Title [ProcessGroup] ,COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) [Enablers Count] FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 2 GROUP BY N.Title ORDER BY [Enablers Count] DESC", "CE4-OM", "How many enablers are associated with each Process Group ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT D.Title [Disposition] 	,COUNT(SD.SystemId) [SystemCount] FROM Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID GROUP BY D.Title", "CE4-OM", "project-data", "List the total number of systems by disposition" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { " SELECT PR.Title [Process] , D.DispositionId FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId  	LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 	AND D.NodeId IS NULL 	AND S.[Key] = 'DAY_ONE'", "CE4-OM", "project-data", "Can you please list down all Day 1 processes where no disposition has been tagged?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { " SELECT 	N.Title [Process]	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 3 GROUP BY N.Title HAVING COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) = 0", "CE4-OM", "project-data", "Can you please list all processes with no Enablers?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT PR.Title as MissingInDay1 FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'CURRENT_STATE' EXCEPT SELECT 	PR.Title [Process] FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'DAY_ONE'", "CE4-OM", "project-data", "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title Enabler, 'Asset' as Category	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID LEFT JOIN [Assets] A ON A.ID = NA.AssetsId WHERE A.Title IS NOT NULL UNION ALL SELECT DISTINCT F.Title Enabler, 'Facility' as Category FROM Nodes N LEFT JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID LEFT JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE F.Title IS NOT NULL  UNION ALL SELECT DISTINCT S.Title Enabler , 'System' as Category FROM Nodes N LEFT JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID LEFT JOIN [Systems] S ON S.ID = NS.SystemsId WHERE S.Title IS NOT NULL  UNION ALL SELECT DISTINCT T.Title Enabler	, 'TPA' as Category FROM Nodes N LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID LEFT JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId WHERE T.Title IS NOT NULL ", "CE4-OM", "project-data", "What Enablers are we tracking for this project ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# Systems present in this functional OP model] FROM Systems", "CE4-OM", "project-data", "How many Systems are there in this functional op model ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements", "CE4-OM", "project-data", "How many TPAs are there in this functional op model ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT ST.Title AS SystemType, COUNT(S.ID) [# Systems by Type] FROM Systems S LEFT JOIN SystemTypes ST ON ST.ID = S.TypeId WHERE S.TypeId IS NOT NULL GROUP BY ST.Title", "CE4-OM", "project-data", "Lsit the number of Systems by Type" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT O.Title AS Owners, Count(*) AS [# TPAs by Ownership] FROM ThirdPartyAgreements TP LEFT JOIN Owners O ON O.ID = TP.OwnerID WHERE TP.OwnerID IS NOT NULL GROUP BY O.Title", "CE4-OM", "project-data", "List the number of TPAs by Ownership" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Title [ProcessGroups With No Process] FROM Nodes WHERE nodetypeid = 2 AND ID NOT IN (SELECT NodeParentId FROM Nodes)", "CE4-OM", "project-data", "List down all process groups with no process within them?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT COUNT(P.[ID]) AS [Number of Process across Op Model] FROM Nodes P     LEFT JOIN NodeTypes NType         ON P.NodeTypeId = NType.ID WHERE NType.Title = 'Process'", "CE4-OM", "project-data", "List the number of processes across op models" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Modified >= DATEADD(hh, -1, GETDATE())", "CE4-OM", "project-data", "List history of changes to the op model in the past one hour" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE FieldImpacted = 'Owner'       AND CAST(Modified AS DATE) = CAST(GETDATE() AS DATE)", "CE4-OM", "project-data", "List the history of changes to Ownership in the op model today" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Title = 'Deleted'       AND DATEPART(wk, CAST(Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(Modified) = YEAR(GETDATE())", "CE4-OM", "project-data", "List the history of deletes in the op model this week" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT T.Title [State], N.Title AS OperatingModel FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId  WHERE N.NodeTypeId = 1", "CE4-OM", "project-data", "List the functional operating models across different states" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT N.Title Opmodel, L.Title LiveNote, Note FROM OperatingModelLiveNotes L LEFT JOIN Nodes N ON N.Id = L.NodeId", "CE4-OM", "project-data", "Are there any Live Notes in this op model ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process FROM OperatingModelActivityLog OL  LEFT JOIN Nodes N ON N.ID = Ol.ObjectID WHERE OL.Title = 'Updated' AND Object = 'Process' AND (CAST(OL.Modified AS DATE) BETWEEN DATEADD(DAY, -7, CAST(GETDATE()AS DATE))  AND CAST(GETDATE() AS DATE))", "CE4-OM", "project-data", "List down all processes which were updated since last 1 week?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process, OldValue, NewValue  FROM OperatingModelActivityLog PL  LEFT JOIN Nodes N ON N.ID = PL.ObjectID  WHERE Object = 'Process' AND PL.Title = 'Updated' AND FieldImpacted = 'Title' AND N.TransactionStateId = 6", "project-data", "List down all processes which have been renamed in Day 1 op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 148,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "project-data", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "project-data", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "project-data", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "project-data", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "project-data", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT    TST.Title as [Phase],    COUNT(TC.ID) AS [# TSAs across different phases] FROM    TSAItems TC    LEFT JOIN TSAPhases TST ON TC.PhaseID = TST.ID  Group BY    TST.Title ", "project-data", "List the number of TSAs across different phases " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT    T.Title PendingTSAItems  FROM    TSAItems T    JOIN TSAStatuses TS ON T.TSAItemTSAStatusId = TS.ID  WHERE    T.AuthorId = ${Useremail}    AND TS.[key] = 'ACTIVE' ", "CE4-TSA", "project-data", "How many pending TSA items do I have ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    Title as [Billing Period Name],    DeadlineDate as [Contribution Deadlines]  FROM    TSABillingPeriods ", "CE4-TSA", "project-data", "In Cost Tracking, how is my billing periods setup ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { " SELECT    Title AS [TSA Phases]  FROM TSAPhases", "CE4-TSA", "project-data", "What are the different TSA phases in my project ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ProviderLegalEntityId = L.ID where  L.ServiceLocation is not null UNION  SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ReceiverLegalEntityId = L.ID   where  L.ServiceLocation is not null ", "CE4-TSA", "project-data", "List the TSA service locations in my project", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "project-data", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "project-data", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "project-data", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "project-data", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 164,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 165,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 166,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 167,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 168,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 172,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 173,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 175,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 176,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 177,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 178,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 180,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 183,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 184,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 186,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What are examples for TSAs?" });

            migrationBuilder.InsertData(
                table: "AssistantSuggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy", "VisibleToAssistant" },
                values: new object[,]
                {
                    { 188, null, "CE4-TSA", null, "System", "ey-guidance", "Why are TSAs important?", null, "System", true },
                    { 189, null, "CE4-TSA", null, "System", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past.", null, "System", true },
                    { 190, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "What PMO workplan templates are available?", null, "System", true },
                    { 191, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "What are the best practices to run weekly status meetings with the client?", null, "System", true },
                    { 192, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "How do you add a new field to the workplan?", null, "System", true },
                    { 193, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "How do I link a Workplan Task to RAID?", null, "System", true },
                    { 194, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project.", null, "System", true },
                    { 195, null, "CE4-PMO", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System", true },
                    { 196, null, "CE4-PMO", null, "System", "internet", "What are the key risks for a {ProjectType} project?", null, "System", true },
                    { 197, null, "CE4-PMO", null, "System", "internet", "What are the key milestones for a {ProjectType} project?", null, "System", true },
                    { 198, null, "CE4-PMO", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System", true },
                    { 199, null, "CE4-PMO", null, "System", "internet", "What are some of the similar deals that have happened in the past?", null, "System", true },
                    { 200, null, "CE4-PMO", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System", true },
                    { 201, null, "CE4-VC", null, "System", "internet", "What are the best cost saving initiatives?", null, "System", true },
                    { 202, null, "CE4-VC", null, "System", "internet", "What are the best revenue growth initiatives?", null, "System", true },
                    { 203, null, "CE4-VC", null, "System", "internet", "What are the best strategies for improving a company in the {Sector} sector?", null, "System", true },
                    { 204, null, "CE4-VC", null, "System", "internet", "What are recent examples of improvements being made in the {Sector} sector?", null, "System", true },
                    { 205, null, "CE4-VC", null, "System", "internet", "What are the best ways to track actuals?", null, "System", true },
                    { 206, null, "CE4-VC", null, "System", "internet", "What should be the frequency of tracking dollar values during the engagement?", null, "System", true },
                    { 207, null, "CE4-VC", null, "System", "internet", "What are typical implications for cross border deals?", null, "System", true },
                    { 208, null, "CE4-OM", null, "System", "internet", "What is a normative operating model for the {Sector} sector?", null, "System", true },
                    { 209, null, "CE4-OM", null, "System", "internet", "What are key considerations when defining an operating model for a {Sector} sector company?", null, "System", true },
                    { 210, null, "CE4-OM", null, "System", "internet", "What are examples of Day 1 process dispositions?", null, "System", true },
                    { 211, null, "CE4-TSA", null, "System", "internet", "What are the corporate functions typically involved in the {Sector} sector?", null, "System", true },
                    { 212, null, "CE4-TSA", null, "System", "internet", "What are the typical services of the Sales and Marketing function?", null, "System", true },
                    { 213, null, "CE4-TSA", null, "System", "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.", null, "System", true },
                    { 214, null, "CE4-TSA", null, "System", "internet", "Provides templates of TSA", null, "System", true },
                    { 215, null, "CE4-TSA", null, "System", "internet", "What should be the typical duration for TSA", null, "System", true },
                    { 216, null, "CE4-TSA", null, "System", "internet", "Things I should keep in mind for longer duration TSAs", null, "System", true },
                    { 217, null, "PROJECT_LEVEL", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System", true },
                    { 218, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key risks for a {ProjectType} project?", null, "System", true },
                    { 219, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key milestones for a {ProjectType} project?", null, "System", true },
                    { 220, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System", true },
                    { 221, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 221);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "CE4-VC", "What are our targets for this engagement?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '{Username}' GROUP BY S.Title ", "CE4-VC", "How are my initiatives doing?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '{Username}' ", "CE4-VC", "Are there any risks or issues with initiatives that I’m responsible for?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '{Username}' ", "CE4-VC", "How  many initiatives are assigned to me? ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '{Username}' ", "CE4-VC", "Give my cost reduction initiatives ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(RevenueGrowthEstimate,0)) [RevenueGrowthTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title = {ProjectTeam}", "CE4-VC", "What are the revenue growth targets for Sales & Marketing ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SUM(ISNULL(CostToAchieveEstimate,0)) [CostToAchieveTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title ={ProjectTeam}", "CE4-VC", "What are the cost to achieve targets for R&D ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT S.Title [Value Capture Stage] , COUNT(VI.ID) [No. of Initiatives] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} GROUP BY S.Title", "CE4-VC", "How many initiatives are there in IT across different stages ?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Approved Initiaves] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId LEFT JOIN ValueCaptureStages S ON S.ID = VI.ValueCaptureStageId WHERE PT.Title = {ProjectTeam} AND S.Title = 'Approved'", "CE4-VC", "List IT initiatives that are in approved stage", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiaves with Workplan] FROM ValueCaptureInitiatives VI JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WV ON VI.ID = WV.ValueCaptureInitiativeId", "CE4-VC", "List initiatives that have workplan item linked to them" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT VI.Title [Initiaves with Risk] FROM ValueCaptureInitiatives VI JOIN RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RV ON VI.ID = RV.ValueCaptureInitiativeId LEFT JOIN RisksAndIssues RI ON RI.ID = RV.RisksAndIssueId WHERE RI.IssueRiskCategory = 'Risk'", "CE4-VC", "How many initiatives have Risks linked ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) [HeadCountCostReductionTarget] FROM [ValueCaptureTopDownEstimates] VT", "CE4-VC", "What is the total headcount cost reduction target ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Non Active Initiaves] FROM ValueCaptureInitiatives VI WHERE ISNULL(VI.IsItemActive ,0) = 0", "CE4-VC", "List initiatives that are not active", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages  VCS ON VCI.ValueCaptureStageId = VCS.ID WHERE VCS.Title = {ValueCaptureStage} ", "How many initiatives have been realized?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(NonHeadcountCostReductionEst) + SUM(RevenueGrowthEstimate) + SUM(HeadcountCostReductionEst) 'Total Top Down Target Value' FROM ValueCaptureTopDownEstimates", "What is the Total Top Down Target Value ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SUM(RevenueGrowthEstimate) RevenueGrowth,SUM(TotalCostReduction) CostReduction FROM ValueCaptureTopDownEstimates", "Show my top down target values by cost reduction and revenue growth" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(ID) FROM ValueCaptureInitiatives WHERE IsItemActive = 1", "How many active initatives are there in my project ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title [Project Team] ,CAST(SUM(ISNULL(RevenueGrowthEstimate,0))/1000000 AS FLOAT) [Revenue Growth(in Million)] FROM [ValueCaptureTopDownEstimates] VT 	LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId GROUP BY PT.Title", "What is the total of revenue growth by team?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VC.Title AS [ValueCaptureInitiatives with no Owners] FROM ValueCaptureInitiatives VC WHERE ItemOwnerId IS NULL", "List out initiatives with no owners assigned.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VI.Title [Initiative with Workplan at Risk] FROM WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WTVI LEFT JOIN Workplan W ON W.ID = WTVI.WorkPlanId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId LEFT JOIN ValueCaptureInitiatives VI on VI.ID = WTVI.ValueCaptureInitiativeId WHERE S.[KEY] = 'AT_RISK'", "List out initiatives that have at risk workplan task linked.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VP.Title AS ValueCapturepriority, COUNT(VC.ValueCapturePriorityId) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId where ValueCapturePriorityId is not null GROUP BY ValueCapturePriorityId, VP.Title", "Show me initiatives count by priority.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT UP.Title AS UserProfile, COUNT(VC.ID) AS ValueCaptureOwnerwiseCount FROM ValueCaptureInitiatives VC LEFT JOIN UserProfiles UP ON UP.ID = VC.ItemOwnerId WHERE ItemOwnerId IS NOT NULL GROUP BY ItemOwnerId, UP.Title", "Show me initiatives count by owner.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title [Initiaves] FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Title = {Username}", "List initiatives where I'm assigned as the Owner" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as IdentifiedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Identified'", "How many initiatives are identified?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ApprovedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Approved' ", "How many initiatives have been approved?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT COUNT(VCI.ID) as ValidatedInitiatives FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VCI.ValueCaptureStageId WHERE VCS.Title = 'Validated' ", "How many initiatives have been validated?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCS.Title AS [Value Capture Stage] ,COUNT(VCI.ID) AS InitativesCount FROM ValueCaptureInitiatives VCI LEFT JOIN ValueCaptureStages VCS ON VCI.ValueCaptureStageId = VCS.ID GROUP BY VCS.Title", "Can you provide breakdown of the initiatives across various stages?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username}", "What initatives have I been assigned to ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VS.Title ValueCaptureStage FROM ValueCaptureInitiatives VI JOIN ValueCaptureStages VS ON VI.ValueCaptureStageID = VS.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureStage", "List my initatives by stages" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title AS Initiatives, VT.Title ValueCaptureType FROM ValueCaptureInitiatives VI JOIN ValueCaptureTypes VT ON VI.ValueCaptureStageID = VT.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureType", "List my initatives by Value Capture Type" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT PT.Title,SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(RevenueGrowthEstimate,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)) AS TotalTopDownTarget FROM ValueCaptureTopDownEstimates LEFT JOIN ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY PT.Title", "What is the top-down target for this project?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT VCS.Title AS ValueCaptureStage, COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VC.ValueCaptureStageId GROUP BY ValueCaptureStageId, VCS.Title", "Show me initiatives count by stage.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AnswerSQL", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT F.Title as Functions, COUNT(VC.FunctionId) AS ValueCaptureFunction FROM ValueCaptureInitiatives VC LEFT JOIN Functions f ON f.ID = VC.FunctionId WHERE FunctionId IS NOT NULL GROUP BY FunctionId, F.Title ", "Show me initiatives count by function.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "CE4-OM", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets are tagged to Day 1 processes? ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets are tagged to Current State processes? ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(Title) AS TotalCount, BusinessEntityId FROM Nodes WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "Can you provide me number of processes by op model? ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in current state? ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT COUNT(1) FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'  AND N.NodeTypeId = 3  AND N.ID NOT IN (   SELECT DISTINCT NTO.NodeId   FROM Nodes N1   LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null  )", "CE4-OM", "How many processes don't have an Owner assigned in Current State ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PR.ID 	,PR.Title FROM Nodes PR JOIN TransactionStates T ON T.ID = PR.TransactionStateId WHERE T.[Key] = 'DAY_ONE' 	AND PR.NodeTypeId = 3 	AND NOT EXISTS  				(SELECT 1 FROM [NodesToDispositionsForDispositionNew] D WHERE D.NodeId = PR.ID)", "CE4-OM", "List processes that don't have Disposition assigned in Day 1 state" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Title FROM Dispositions", "CE4-OM", "List the Disposition options available ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT N.Title [ProcessGroup] ,COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) [Enablers Count] FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 2 GROUP BY N.Title ORDER BY [Enablers Count] DESC", "CE4-OM", "How many enablers are associated with each Process Group ?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT D.Title [Disposition] 	,COUNT(SD.SystemId) [SystemCount] FROM Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID GROUP BY D.Title", "CE4-OM", "List the total number of systems by disposition", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { " SELECT PR.Title [Process] , D.DispositionId FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId  	LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 	AND D.NodeId IS NULL 	AND S.[Key] = 'DAY_ONE'", "CE4-OM", "Can you please list down all Day 1 processes where no disposition has been tagged?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { " SELECT 	N.Title [Process]	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 3 GROUP BY N.Title HAVING COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) = 0", "CE4-OM", "Can you please list all processes with no Enablers?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PR.Title as MissingInDay1 FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'CURRENT_STATE' EXCEPT SELECT 	PR.Title [Process] FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'DAY_ONE'", "CE4-OM", "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title Enabler, 'Asset' as Category	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID LEFT JOIN [Assets] A ON A.ID = NA.AssetsId WHERE A.Title IS NOT NULL UNION ALL SELECT DISTINCT F.Title Enabler, 'Facility' as Category FROM Nodes N LEFT JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID LEFT JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE F.Title IS NOT NULL  UNION ALL SELECT DISTINCT S.Title Enabler , 'System' as Category FROM Nodes N LEFT JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID LEFT JOIN [Systems] S ON S.ID = NS.SystemsId WHERE S.Title IS NOT NULL  UNION ALL SELECT DISTINCT T.Title Enabler	, 'TPA' as Category FROM Nodes N LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID LEFT JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId WHERE T.Title IS NOT NULL ", "CE4-OM", "What Enablers are we tracking for this project ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# Systems present in this functional OP model] FROM Systems", "CE4-OM", "How many Systems are there in this functional op model ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements", "CE4-OM", "How many TPAs are there in this functional op model ?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT ST.Title AS SystemType, COUNT(S.ID) [# Systems by Type] FROM Systems S LEFT JOIN SystemTypes ST ON ST.ID = S.TypeId WHERE S.TypeId IS NOT NULL GROUP BY ST.Title", "CE4-OM", "Lsit the number of Systems by Type" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT O.Title AS Owners, Count(*) AS [# TPAs by Ownership] FROM ThirdPartyAgreements TP LEFT JOIN Owners O ON O.ID = TP.OwnerID WHERE TP.OwnerID IS NOT NULL GROUP BY O.Title", "CE4-OM", "List the number of TPAs by Ownership", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title [ProcessGroups With No Process] FROM Nodes WHERE nodetypeid = 2 AND ID NOT IN (SELECT NodeParentId FROM Nodes)", "List down all process groups with no process within them?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 148,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "What are the best practices to run weekly status meetings with the client?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { null, "PROJECT_LEVEL", "ey-guidance", "How do I link a Workplan Task to RAID?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "What are the key risks for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "internet", "What are the key milestones for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 164,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 165,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 166,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 167,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 168,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best strategies for improving a company in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are recent examples of improvements being made in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best ways to track actuals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 172,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What should be the frequency of tracking dollar values during the engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 173,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What is a normative operating model for the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 175,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What are key considerations when defining an operating model for a {Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 176,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 177,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "What are the corporate functions typically involved in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 178,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 180,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "Provides templates of TSA" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "What should be the typical duration for TSA" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "Things I should keep in mind for longer duration TSAs" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 183,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 184,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "internet", "What are the key risks for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "internet", "What are the key milestones for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 186,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "internet", "What are the best practices to build workplan, and track dependencies?" });
        }
    }
}
