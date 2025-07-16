using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAliasAndConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AssistantConfigurations",
                columns: new[] { "ID", "IsEnabled", "Key", "Title", "Value" },
                values: new object[] { 2, true, "SOURCE_CONFIGS", "Source Configuration", "{\"project-data\": {\"isActive\": true,\"text-box-enabled\": false},\"project-docs\": {\"isActive\": true,\"text-box-enabled\": true},\"ey-guidance\": {\"isActive\": true,\"text-box-enabled\": true},\"internet\": {\"isActive\": true,\"text-box-enabled\": true}}" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "AnswerSQL",
                value: "SELECT RI.RiskImpact, RI.RiskProbability, COUNT(*) AS RiskCount FROM RisksAndIssues RI LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE RI.IssueRiskCategory = 'Risk' AND RI.ItemStatusId NOT IN (SELECT S.ID FROM statuses S WHERE S.[Key] IN ('COMPLETED', 'CLOSED')) AND (RI.RiskImpact IS NOT NULL OR RI.RiskProbability IS NOT NULL) GROUP BY RI.RiskImpact, RI.RiskProbability ORDER BY RI.RiskImpact, RI.RiskProbability");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                column: "AnswerSQL",
                value: "SELECT TOP 1 CAST(RP.PeriodEndDate AS DATE) AS WeeklyStatusDueDate FROM ReportingPeriods RP WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(RP.PeriodStartDate AS DATE) AND CAST(RP.PeriodEndDate AS DATE) ORDER BY RP.Modified DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "AnswerSQL",
                value: "SELECT W.Title AS WorkPlanItem FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT' AND DATEPART(ww,W.TaskDueDate) = DATEPART(ww,GETDATE()) + 1");

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
                keyValue: 25,
                column: "AnswerSQL",
                value: "SELECT TOP 1 PT.Title Team, SUM(Counts) TotalInterdependencies FROM ( SELECT IDP.ProviderProjectTeamID TeamID, Count(IDP.ID) Counts FROM Interdependencies IDP WHERE CAST(IDP.ItemDueDate AS DATE) < CAST(GETDATE() AS DATE) GROUP BY IDP.ProviderProjectTeamID UNION ALL SELECT IDR.ReceiverProjectTeamID, Count(IDR.ID) FROM Interdependencies IDR WHERE CAST(IDR.ItemDueDate AS DATE) < CAST(GETDATE() AS DATE) GROUP BY IDR.ReceiverProjectTeamID ) SUB JOIN ProjectTeams PT ON SUB.TeamID = PT.ID GROUP BY PT.Title ORDER BY TotalInterdependencies DESC");

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
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team]  FROM ProjectStatusEntries PSE  LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID  LEFT JOIN WeeklyStatusStatuses WS ON WS.ID = PSE.WeeklyStatusStatusId  WHERE  RP.ID = (SELECT RPID.ID  FROM ReportingPeriods RPID WHERE GETDATE() BETWEEN RPID.PeriodStartDate AND RPID.PeriodEndDate) AND WS.[KEY] IN ('AT_RISK','BEHIND_SCHEDULE') AND TT.[Key]='PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 42,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedLastWeek  FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID    JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID   JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  CAST(W.ActualEndDate AS DATE) >= CAST(DATEADD(week,DATEDIFF(week,0,GETDATE()), 0) AS DATE)   AND CAST(W.ActualEndDate AS DATE) < CAST( DATEADD(week,DATEDIFF(week,0, GETDATE()) + 1,0) AS DATE)   AND S.[KEY] NOT IN ('COMPLETED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 45,
                column: "AnswerSQL",
                value: "SELECT WSS.Title StatusValue ,'CurrentStatus' as Category FROM ProjectStatusEntries PSE LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT TOP 1 RPID.ID FROM ReportingPeriods RPID Where cast(RPID.PeriodStartDate as date) <= cast(getDate() as date) Order BY RPID.PeriodEndDate DESC) AND PT.Title like {ProjectTeam}  UNION SELECT WSS.Title StatusValue ,'PreviousStatus' as Category FROM ProjectStatusEntries PSE  LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT  RPID.ID FROM   ReportingPeriods RPID Where cast(RPID.PeriodStartDate as date) <= cast(getDate() as date) Order BY RPID.PeriodEndDate DESC Offset 1 Rows FETCH NEXT 1 ROWS ONLY) AND  PT.Title like {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "AnswerSQL",
                value: "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID)) ROW_NUM FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      CAST(W.Modified AS DATE) BETWEEN DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND PT.ItemIsActive=1 AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY PT.Title) A");

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
                value: "SELECT W.Title as [List down all tasks associated with interdependencies] from Workplan W where W.ID in ( SELECT WIRT.WorkPlanId FROM WorkPlansToInterdependenciesForReceiverTasks WIRT UNION select WIPT.WorkPlanId FROM WorkPlansToInterdependenciesForProviderTasks WIPT) AND W.WorkPlanTaskType='Task'");

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
                keyValue: 69,
                column: "AnswerSQL",
                value: "SELECT PT.Title FROM ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE ManageProjectStatus = 1 AND  TT.[Key] = 'PROJECT_MANAGEMENT'  EXCEPT   SELECT PT.Title  FROM ProjectStatusEntries PSE  LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodId = RP.ID  LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamId = PT.ID  WHERE RP.ID = (SELECT RPID.ID FROM ReportingPeriods RPID WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(RPID.PeriodStartDate AS DATE) AND CAST(RPID.PeriodEndDate AS DATE)      )   AND PSE.WeeklyStatusStatusId IS NOT NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                column: "AnswerSQL",
                value: "SELECT COUNT(R.ID) AS NewRisksCount FROM  RisksAndIssues R JOIN ProjectTeams PT ON R.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE R.IssueRiskCategory = 'Risk' AND DATEDIFF(day, R.Created, GETDATE()) <= 5 AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                column: "AnswerSQL",
                value: "SELECT      ANS.Title FROM     ProjectStatusEntries PSE    LEFT OUTER JOIN AccomplishmentsAndNextSteps ANS ON PSE.ID = ANS.ProjectStatusEntryId     LEFT JOIN ProjectTeams PT ON PT.ID = PSE.ProjectTeamId     LEFT OUTER JOIN ReportingPeriods RP ON PSE.ReportingPeriodId = RP.ID  WHERE     ANS.AccomplishmentNextStepCategory IN '{AccomplishmentNextStepCategory}' AND RP.PeriodStartDate = (         SELECT TOP 1 RPD.PeriodStartDate          FROM ReportingPeriods RPD       WHERE CAST (GETDATE() AS DATE) BETWEEN CAST(RPD.PeriodStartDate AS DATE) AND CAST(RPD.PeriodEndDate AS DATE)      ) ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                column: "AnswerSQL",
                value: ";WITH CTE AS (SELECT COUNT(*) AS OverdueItemCount,      PT.Title AS [ProjectTeam With Most Overdue Item]  FROM WorkPlan W  LEFT JOIN [ProjectTeams] PT ON PT.ID = W.ProjectteamId   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId  WHERE  S.[Key] <> '{Key}'    AND TT.[Key]='PROJECT_MANAGEMENT'    AND CAST(W.TaskDueDate AS DATE ) < CAST(GETDATE() AS DATE) AND PT.Title IS NOT NULL GROUP BY PT.Title )  SELECT Top 1 [ProjectTeam With Most Overdue Item] FROM CTE ORDER BY OverdueItemCount DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                column: "AnswerSQL",
                value: "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(VCTDE.HeadcountCostReductionEst)+SUM(VCTDE.NonHeadcountCostReductionEst)+SUM(VCTDE.RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(VT.RevenueGrowthEstimate,0)) [RevenueGrowthTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(VT.CostToAchieveEstimate,0)) [CostToAchieveTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title ={ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(VT.HeadcountCostReductionEst,0)) [HeadCountCostReductionTarget] FROM ValueCaptureTopDownEstimates VT");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "AnswerSQL",
                value: "SELECT SUM(VCTDE.NonHeadcountCostReductionEst) + SUM(VCTDE.RevenueGrowthEstimate) + SUM(VCTDE.HeadcountCostReductionEst) 'Total Top Down Target Value' FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "AnswerSQL",
                value: "SELECT SUM(VCTDE.RevenueGrowthEstimate) RevenueGrowth,SUM(VCTDE.TotalCostReduction) CostReduction FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                column: "AnswerSQL",
                value: "SELECT COUNT(VCI.ID) FROM ValueCaptureInitiatives VCI WHERE VCI.IsItemActive = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team] ,CAST(SUM(ISNULL(VT.RevenueGrowthEstimate,0))/1000000 AS FLOAT) [Revenue Growth(in Million)] FROM ValueCaptureTopDownEstimates VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId GROUP BY PT.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "AnswerSQL",
                value: "SELECT VC.Title AS [ValueCaptureInitiatives with no Owners] FROM ValueCaptureInitiatives VC WHERE VC.ItemOwnerId IS NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "AnswerSQL",
                value: "SELECT VP.Title AS ValueCapturepriority, COUNT(VC.ValueCapturePriorityId) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId where VC.ValueCapturePriorityId is not null GROUP BY VC.ValueCapturePriorityId, VP.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "AnswerSQL",
                value: "SELECT UP.Title AS UserProfile, COUNT(VC.ID) AS ValueCaptureOwnerwiseCount FROM ValueCaptureInitiatives VC LEFT JOIN UserProfiles UP ON UP.ID = VC.ItemOwnerId WHERE VC.ItemOwnerId IS NOT NULL GROUP BY VC.ItemOwnerId, UP.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "AnswerSQL",
                value: "SELECT PT.Title,SUM(ISNULL(VCTDE.NonHeadcountCostReductionEst,0)) + SUM(ISNULL(VCTDE.RevenueGrowthEstimate,0)) + SUM(ISNULL(VCTDE.HeadcountCostReductionEst,0)) AS TotalTopDownTarget FROM ValueCaptureTopDownEstimates VCTDE LEFT JOIN ProjectTeams PT ON PT.ID = VCTDE.ProjectTeamId GROUP BY PT.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "AnswerSQL",
                value: "SELECT VCS.Title AS ValueCaptureStage, COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VC.ValueCaptureStageId GROUP BY VC.ValueCaptureStageId, VCS.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 111,
                column: "AnswerSQL",
                value: "SELECT F.Title as Functions, COUNT(VC.FunctionId) AS ValueCaptureFunction FROM ValueCaptureInitiatives VC LEFT JOIN Functions F ON F.ID = VC.FunctionId WHERE VC.FunctionId IS NOT NULL GROUP BY VC.FunctionId, F.Title ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                column: "AnswerSQL",
                value: "SELECT COUNT(VCTDE.ID) AS [Top Down targets #],        SUM(ISNULL(VCTDE.NonHeadcountCostReductionEst, 0)) AS [Total Non Headcount Cost Reduction Estimate],        SUM(ISNULL(VCTDE.RevenueGrowthEstimate, 0)) AS [Total Revenue Growth Estimate],        SUM(ISNULL(VCTDE.CostToAchieveEstimate, 0)) AS [Total Cost To Achieve Estimate],        SUM(ISNULL(VCTDE.HeadcountCostReductionEst, 0)) AS [Total Headcount Cost Reduction Estimate],        SUM(ISNULL(VCTDE.HeadcountCostReductionEst, 0)) + SUM(ISNULL(VCTDE.NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(VCTDE.RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "AnswerSQL",
                value: "SELECT  VCTT.Title, PT.Title, VCTT.CostToAchieveEstimate, VCTT.NonHeadcountCostReductionEst, VCTT.HeadcountCostReductionEst, VCTT.RevenueGrowthEstimate FROM   ValueCaptureTopDownEstimates VCTT  LEFT JOIN ProjectTeams PT on PT.ID = VCTT.ProjectTeamId");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(VCTDE.HeadcountCostReductionEst,0)) AS HeadcountCostReduction FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(VCTDE.NonHeadcountCostReductionEst,0)) AS NonHeadcountCostReduction FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(VCTDE.RevenueGrowthEstimate,0)) AS RevenueGrowth FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(VCTDE.CostToAchieveEstimate,0)) AS CostToAchive FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120,
                column: "AnswerSQL",
                value: "SELECT A.Amount as [Client Baseline PnL Amount], A.FinancialLineItem as FinancialLineItem FROM( SELECT F.ID, F.Ordinal, F.Title FinancialLineItem , SUM(C.Amount)/1000000 Amount FROM ValueCaptureFinancialLineItems F LEFT JOIN CostCenters C ON C.FinancialLineItemId = F.ID  GROUP BY F.ID, F.Title, F.Ordinal ) A WHERE A.Amount IS NOT NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121,
                column: "AnswerSQL",
                value: "SELECT VCRI.Title AS [Recommended Initiatives List] FROM ValueCaptureRecommendedInitiatives VCRI");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122,
                column: "AnswerSQL",
                value: "SELECT 	SUM(a.[Total Top Down Target Value]) AS [Top Down Targets],      SUM(b.[Bottom Up Initiatives Value]) AS [Bottom Up Initiatives Value], 	( SUM(b.[Bottom Up Initiatives Value])-SUM(a.[Total Top Down Target Value])) AS Variance FROM ( SELECT   SUM(ISNULL(VCTDE.HeadcountCostReductionEst, 0)) + SUM(ISNULL(VCTDE.NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(VCTDE.RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates VCTDE) a, ( select SUM(Amount)*12 as [Bottom Up Initiatives Value]  FROM vwUnpivotEstimates where MYear='Y3M12' AND Recurring =1 ) b ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                column: "AnswerSQL",
                value: "SELECT CONCAT('Tracking Period of the Program is from ', CAST(MIN(VCTM.StartDt) AS DATE), ' to ', CAST(MAX(VCTM.EndDt) AS DATE)) FROM ValueCaptureTransactionMonths VCTM WHERE VCTM.IsItemActive = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                column: "AnswerSQL",
                value: "SELECT ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = '{ProjectTeam}' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = '{ProjectTeam}'    ) Act         ON 1 = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                column: "AnswerSQL",
                value: "SELECT VCI.Title Initiatives FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCapturePriorityId = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS TotalNetSales FROM ValueCaptureEstimates VE LEFT JOIN ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId WHERE FLT.Title = 'PnL' AND FL.Title in ('Net sales')", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                column: "AnswerSQL",
                value: "SELECT SUM(VCTDE.NonHeadcountCostReductionEst) NonHeadcountCostReduction, SUM(VCTDE.HeadcountCostReductionEst) HeadcountCostReduction FROM ValueCaptureTopDownEstimates VCTDE");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                column: "AnswerSQL",
                value: ";WITH CTE AS  ( 	SELECT I.ProjectTeamId  	FROM ValueCaptureEstimates E 		LEFT JOIN ValueCaptureInitiatives I ON I.ID = E.ValueCaptureInitiativeId 	EXCEPT 	SELECT VCTDE.ProjectTeamId FROM ValueCaptureTopDownEstimates VCTDE )  SELECT PT.Title [ProjectTeam]  FROM CTE LEFT JOIN ProjectTeams PT ON PT.ID = CTE.ProjectTeamId");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                column: "AnswerSQL",
                value: " DECLARE @CurrentYR NVARCHAR(10), @SQL NVARCHAR(MAX)  SELECT  TOP 1 @CurrentYR = [YEAR] FROM vwValueCaptureTransactionMonths WHERE RelativeYear = YEAR(GETDATE())   SELECT @SQL = ' ;WITH TopDownCTE AS ( 	SELECT  		PT.ID  PROJECTTEAMID 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(VT.RevenueGrowthEstimate,0))  + SUM(ISNULL(VT.NonHeadcountCostReductionEst,0)) + SUM(ISNULL(VT.HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM ValueCaptureTopDownEstimates VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 	GROUP BY  		PT.ID  		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(VT.RevenueGrowthEstimate,0))  + SUM(ISNULL(VT.NonHeadcountCostReductionEst,0)) + SUM(ISNULL(VT.HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM ValueCaptureTopDownEstimates VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 ) , BottomUPCTE AS ( 	SELECT  		VI.ProjectTeamId 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 	GROUP BY  		VI.PROJECTTEAMID 		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 )  SELECT  	COALESCE(T.PROJECTTEAMID,B.PROJECTTEAMID) ProjectTeamID 	,COALESCE(T.ProjectTeam,B.ProjectTeam) ProjectTeam 	,ISNULL(T.[Target],0) [TopDown Target(M)] 	,ISNULL(B.Estimate,0) [BottomUP Estimate(M)] 	,ISNULL(T.[Target],0) - ISNULL(B.Estimate,0) [Variance(M)] FROM TopDownCTE T 	FULL OUTER JOIN BottomUPCTE B ON T.ProjectTeamId = B.ProjectTeamId ORDER BY ProjectTeamId'  EXEC sp_executesql  @SQL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                column: "AnswerSQL",
                value: "SELECT VC.Title AS ValueCaptureInitiativesReadyForApproval FROM ValueCaptureInitiatives VC WHERE VC.ReadyForApproval = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                column: "AnswerSQL",
                value: "SELECT TOP 5 A.Initiatives, A.Amount FROM( SELECT VI.Title Initiatives ,sum(VE.Amount) Amount FROM vwUnpivotActuals VE LEFT JOIN ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN ValueCaptureImpactTypes VIT ON VIT.ID = VE.ValueCaptureImpactTypeid GROUP BY VI.Title,VE.Amount ) A WHERE A.Amount > 0 ORDER BY A.Amount DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                column: "AnswerSQL",
                value: "SELECT TOP 1 PT.Title, SUM(ISNULL(VCTDE.HeadcountCostReductionEst,0)) AS MostHeadcountCostReduction FROM ValueCaptureTopDownEstimates VCTDE LEFT JOIN ProjectTeams PT ON PT.ID = VCTDE.ProjectTeamId GROUP BY PT.Title ORDER BY SUM(ISNULL(VCTDE.HeadcountCostReductionEst,0)) DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                column: "AnswerSQL",
                value: "SELECT VC.Title as [Value Capture Initiatives with No Owners] FROM ValueCaptureInitiatives as VC WHERE VC.ItemOwnerId IS NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                column: "AnswerSQL",
                value: "SELECT ISNULL(VP.Title, 'No Priority') AS ValueCapturePriority,     COUNT(VC.ID) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId WHERE VC.ValueCapturePriorityId IS NOT NULL GROUP BY VC.ValueCapturePriorityId, VP.Title SELECT ISNULL(VS.Title, 'No Stage Assigned') AS ValueCaptureStage,     COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VS ON VS.ID = VC.ValueCaptureStageId WHERE VC.ValueCaptureStageId IS NOT NULL GROUP BY VC.ValueCaptureStageId, VS.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(E.Amount, 0) * 12) [Run Rate for iniatives in Approved, Committed and Realized] FROM ValueCaptureInitiatives VI LEFT JOIN vwUnpivotEstimates E ON E.ValueCaptureInitiativeId = VI.ID LEFT JOIN ValueCaptureStages VS ON VS.ID = VI.ValueCaptureStageID WHERE VS.Title IN ( 'Approved', 'Committed', 'Realized' ) AND MYear = ( select VCTMK.[Key] from ValueCaptureTransactionMonths VCTMK where CAST(VCTMK.EndDt AS date) = (SELECT MAX(CAST(VCTM.EndDt AS date)) FROM ValueCaptureTransactionMonths VCTM))");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                column: "AnswerSQL",
                value: "SELECT VI.Title [Initiatives with Benefit score over 75] FROM ValueCaptureInitiatives VI LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId WHERE CAST(VI.BenefitScore AS INT) > 75 AND PT.Title IN ('HR')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                column: "AnswerSQL",
                value: "SELECT ns.Title, a.TotalCount FROM ( SELECT COUNT(Title) AS TotalCount, BusinessEntityId FROM Nodes nds WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ns ON a.BusinessEntityId = ns.ID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                column: "AnswerSQL",
                value: "SELECT ns.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ns ON a.BusinessEntityId = ns.ID ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                column: "AnswerSQL",
                value: "SELECT ns.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ns ON a.BusinessEntityId = ns.ID ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                column: "AnswerSQL",
                value: "SELECT Title FROM Dispositions d");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                column: "AnswerSQL",
                value: "SELECT Count(ID) AS [# Systems present in this functional OP model] FROM Systems s");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                column: "AnswerSQL",
                value: "SELECT Count(ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements tpa");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                column: "AnswerSQL",
                value: "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog opma WHERE Modified >= DATEADD(hh, -1, GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188,
                column: "AnswerSQL",
                value: "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog opma WHERE FieldImpacted = 'Owner'       AND CAST(Modified AS DATE) = CAST(GETDATE() AS DATE)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189,
                column: "AnswerSQL",
                value: "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog opma WHERE Title = 'Deleted'       AND DATEPART(wk, CAST(Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(Modified) = YEAR(GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 196,
                column: "AnswerSQL",
                value: " SELECT PR.Title [Process] FROM Nodes PR     JOIN TransactionStates  S  ON S.ID = PR.TransactionStateId    LEFT JOIN  NodesToDispositionsForDispositionNew D             ON D.NodeId = PR.ID WHERE     PR.NodeTypeId = 3     AND D.NodeId IS NULL     AND S.[Key] = 'DAY_ONE'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 197,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes  n   WHERE NodeParentID IS NULL     UNION ALL     SELECT nds.id,  nds.Title,   nds.NodeParentID, nds.NodeTypeId, HierId = HierId + 1     FROM Nodes nds, GetSelectedFunction  where nds.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  , ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID     FROM     (SELECT * FROM PG_Hierarchy pgh WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT TOP 1     Functions AS [Function with highest number of processes] FROM (     SELECT Functions,            COUNT(ID) as [Process Count]     FROM ProcessCTE  tc   GROUP BY Functions ) A order by [Process Count] desc    ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 198,
                column: "AnswerSQL",
                value: "SELECT     N.Title AS [List of Processes with Disposition as TSA] FROM   Nodes   N   LEFT JOIN     NodesToDispositionsForDispositionNew NTD             ON NTD.NodeId = N.ID     LEFT JOIN         Dispositions  D ON NTD.DispositionId = D.ID WHERE     D.Title = 'TSA'     AND N.NodeTypeId = 3");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 199,
                column: "AnswerSQL",
                value: "SELECT N.Title as [List of Processes with No Disposition] FROM Nodes N WHERE N.NodeTypeId=3 AND N.ID NOT IN (SELECT NodeId FROM NodesToDispositionsForDispositionNew ndd)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 202,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes nd    WHERE NodeParentID IS NULL     UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            nds.NodeTypeId,            HierId = HierId + 1     FROM Nodes nds,          GetSelectedFunction     where nds.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ), ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT ISNULL(Geo.Title, 'Not Assigned') AS [Geographic Coverages],        COUNT(P.ProcessID) AS [Total Number of Process in Real Estate] FROM ProcessCTE P     LEFT JOIN ProcessAttributes PA         ON P.ProcessID = PA.NodeId     LEFT JOIN GeographicCoverages Geo         ON PA.GeographicCoverageId = Geo.ID WHERE P.Functions ='{FunctionName}' GROUP BY Geo.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 203,
                column: "AnswerSQL",
                value: ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes nd    WHERE NodeParentID IS NULL     UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            nds.TransactionStateId,            HierId = HierId + 1     FROM Nodes nds,          PG_Hierarchy     WHERE nds.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      F.Title AS [Functions],     SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy pg WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy pg WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID ORDER BY 1,          2");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 205,
                column: "AnswerSQL",
                value: ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes  nd   WHERE NodeParentID IS NULL     UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            nds.TransactionStateId,            HierId = HierId + 1     FROM Nodes nds,          PG_Hierarchy     WHERE nds.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy pg WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy pg WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID WHERE F.Title='{FunctionName}' ORDER BY 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 206,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes nd    WHERE NodeParentID IS NULL     UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            nds.NodeTypeId,            HierId = HierId + 1     FROM Nodes nds,          GetSelectedFunction     where nds.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,        ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT P.Processes,        ISNULL(S.Title, 'Not Assigned') AS [Systems],        ISNULL(TPA.Title, 'Not Assigned') AS [Third Party Agreements] FROM ProcessCTE P     LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA         ON P.ProcessID = NTPA.NodesId     LEFT JOIN ThirdPartyAgreements TPA         ON NTPA.ThirdPartyAgreementsId = TPA.ID     LEFT JOIN NodesToSystemsForEnablerSystems NSys         ON NSys.NodesId = P.ProcessID     LEFT JOIN Systems S         ON NSys.SystemsId = S.ID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 209,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes nd    WHERE NodeParentID IS NULL           AND TransactionStateID = 8     UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            HierId = HierId + 1     FROM Nodes nds,          GetSelectedFunction     where nds.NodeParentID = GetSelectedFunction.id    ) SELECT N.Title [Function],        NTSS.Title AS [Field Status],        OPS.Title AS [Op Status] FROM NodeTrackers NT     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NT.ID     JOIN     (SELECT * FROM GetSelectedFunction gsf WHERE HierId = 2 and Title = 'HR') N         ON N.ID = NT.NodeId     LEFT JOIN NodeTrackerStatusFields NTSS         ON NTSS.ID = NTS.NodeTrackerStatusFieldId     LEFT JOIN OpStatuses OPS         ON OPS.ID = NTS.NodeTrackerStatusStatusId");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 210,
                column: "AnswerSQL",
                value: "SELECT D.Title Disposition,        WP.Title Tasks FROM NodesToWorkPlansForTask NWP     LEFT JOIN Nodes N         ON N.ID = NWP.NodeId     LEFT JOIN WorkPlan WP         ON NWP.WorkPlanId = WP.ID     LEFT JOIN NodesToDispositionsForDispositionNew ND         ON ND.NodeID = N.ID     LEFT JOIN Dispositions D         ON D.ID = ND.DispositionID WHERE N.NodeTypeId = 3");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes nd    WHERE NodeParentID IS NULL           AND TransactionStateID = 6     UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            HierId = HierId + 1     FROM Nodes nds,          GetSelectedFunction     where nds.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM Nodes ROOT     WHERE ROOT.ID in (select ID from GetSelectedFunction gsf where HierId = 2 and Title = 'Finance' )     UNION ALL     SELECT CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ) SELECT COUNT(*) AS [Process with multiple dispositions for Finance] FROM PG_Hierarchy pg WHERE NodeTypeId = 3       AND ID IN (                     SELECT NodeID                     FROM                     (                         SELECT NodeId,                                Count(DispositionID) As [Disposition Count]                         FROM NodesToDispositionsForDispositionNew   ndf     GROUP BY NodeId                         HAVING Count(DispositionID) > 1                     ) a                 )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 212,
                column: "AnswerSQL",
                value: "SELECT     N.Title AS [List of Process which doesn''t have ownership] FROM     Nodes                 N     JOIN         TransactionStates T             ON T.ID = N.TransactionStateId WHERE     OwnerId IS NULL     AND N.NodeTypeId = 3");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 213,
                column: "AnswerSQL",
                value: "SELECT N.Title AS [List of Processes with more than one Disposition] FROM Nodes N WHERE N.NodeTypeId = 3       AND N.ID IN ( SELECT NodeID   FROM   (    SELECT NodeId, COUNT(DispositionID) AS [Disposition Count]                           FROM NodesToDispositionsForDispositionNew    ndf                       GROUP BY NodeId                           HAVING COUNT(DispositionID) > 1                       ) a                   ) ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT ID,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes  nd   WHERE NodeParentID IS NULL     UNION ALL     SELECT nds.ID,            nds.Title,            nds.NodeParentID,            nds.TransactionStateId,            HierId = HierId + 1     FROM Nodes nds,          GetSelectedFunction     where nds.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, TransactionStateID) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.TransactionStateID     FROM Nodes ROOT     WHERE ROOT.ID IN (                          SELECT ID                          FROM GetSelectedFunction         gsf                 WHERE HierId = 2                                AND Title = '{FunctionName}'                                AND TransactionStateId = 6                      )     UNION ALL     SELECT CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.TransactionStateId     FROM PG_Hierarchy PARENT,          Nodes CHILD     WHERE PARENT.id = CHILD.NodeParentID    ) SELECT Title AS [List of Processes Associated with Legal Function] FROM PG_Hierarchy pgh ORDER BY ID,          title DESC ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nd WHERE NodeParentID IS NULL  UNION ALL        SELECT    nds.id,nds.Title,nds.NodeParentID,  nds.NodeTypeId,  HierId = HierId+1 FROM Nodes nds, GetSelectedFunction   where nds.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes FROM (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT P.Functions, A.Processes  FROM (SELECT DISTINCT [Processes]  		FROM ProcessCTE 	pte	GROUP BY Processes  		HAVING COUNT(DISTINCT [Functions] ) > 1) A  	LEFT JOIN ProcessCTE P ON P.Processes = A.Processes ", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 219,
                column: "AnswerSQL",
                value: "SELECT     Title AS [Systems not tagged in any process] FROM     Systems s WHERE     ID NOT IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes                               N                       LEFT JOIN                           NodesToSystemsForEnablerSystems NSS                               ON N.ID = NSS.NodesId  					  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId                    WHERE T.Title= 'Day 1' AND                        N.NodeTypeId = 3               )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 220,
                column: "AnswerSQL",
                value: "SELECT     ISNULL(ST.Title, 'System Type Not Assigned') AS [SystemsType],     S.Title                                      AS [Systems] FROM     Systems         S     LEFT JOIN         SystemTypes ST             ON S.TypeId = ST.ID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 221,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nd  WHERE NodeParentID IS NULL  UNION ALL        SELECT    nds.id,nds.Title,nds.NodeParentID,  nds.NodeTypeId,  HierId = HierId+1 FROM Nodes nds, GetSelectedFunction   where nds.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 2) SF LEFT JOIN (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number for Multiple Deposition]  FROM ProcessCTE p WHERE ID IN ( SELECT NodeID FROM ( SELECT NodeId,    Count(DispositionID) As [Disposition Count] FROM NodesToDispositionsForDispositionNew ndn GROUP BY NodeId HAVING Count(DispositionID) > 1 ) a ) GROUP BY [Functions] ORDER BY  COUNT(*) DESC", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 222,
                column: "AnswerSQL",
                value: ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes n   WHERE NodeParentID IS NULL   UNION ALL         SELECT    nds.id,nds.Title,nds.NodeParentID,  nds.NodeTypeId,  HierId = HierId+1 FROM Nodes nds, GetSelectedFunction   WHERE nds.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2    UNION ALL         SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  WHERE  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS (  SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy pg WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy gg WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Live Without]  FROM ProcessCTE pp WHERE ID IN (	SELECT NodeId 				FROM NodesToDispositionsForDispositionNew ND  				LEFT JOIN Dispositions D ON D.ID = DispositionID  				WHERE D.Title = 'Live without'			 				) GROUP BY [Functions] ORDER BY  COUNT(*) DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 223,
                column: "AnswerSQL",
                value: ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nd  WHERE NodeParentID IS NULL  UNION ALL        SELECT    nds.id,nds.Title,nds.NodeParentID,  nds.NodeTypeId,  HierId = HierId+1 FROM Nodes nds, GetSelectedFunction   where nds.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy pp WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy pp WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Processes tagged to them] , COUNT(*)  [No of Processes] 				FROM ProcessCTE 	ppt	GROUP BY [Functions] 				ORDER BY  COUNT(*) DESC    ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 224,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes   n  WHERE NodeParentID IS NULL     UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            nds.TransactionStateId,             HierId = HierId + 1     FROM Nodes nds,          PG_Hierarchy     WHERE nds.NodeParentID = PG_Hierarchy.id    ),       ProcessCTE AS (SELECT DISTINCT          F.Title AS [Functions],         f.id AS FunctionID     FROM     (SELECT * FROM PG_Hierarchy pp WHERE HierId = 2) F         LEFT JOIN         (SELECT * FROM PG_Hierarchy gg WHERE HierId = 3) SF             ON F.ID = SF.NodeParentID    ) SELECT Top 1     Functions as [Function with Highest Number of Enablers],     [Enablers Count] FROM (     SELECT P.Functions,            COUNT(DISTINCT TPA.ID) + COUNT(DISTINCT S.ID) + COUNT(DISTINCT Asset.ID) + COUNT(DISTINCT Roles.ID) AS [Enablers Count]     FROM ProcessCTE P         LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA           ON P.FunctionID = NTPA.NodesId         LEFT JOIN ThirdPartyAgreements TPA             ON NTPA.ThirdPartyAgreementsId = TPA.ID          LEFT JOIN NodesToSystemsForEnablerSystems NSys              ON NSys.NodesId = P.FunctionID         LEFT JOIN Systems S             ON NSys.SystemsId = S.ID         LEFT JOIN NodesToAssetsForEnablerAssets NAsset              ON NAsset.NodesId = P.FunctionID         LEFT JOIN Assets Asset             ON Asset.ID = NAsset.AssetsId          LEFT JOIN NodesToRolesForEnablerRoles NRole --Roles              ON NAsset.NodesId = P.FunctionID         LEFT JOIN OrgRolesMaster Roles             ON Roles.ID = NRole.RolesId     GROUP BY P.Functions ) A ORDER BY [Enablers Count] DESC", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 225,
                column: "AnswerSQL",
                value: ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID,  HierId = 1 FROM Nodes n WHERE NodeParentID IS NULL    UNION ALL        SELECT    nds.id,nds.Title,nds.NodeParentID,    HierId = HierId+1 FROM Nodes nds, GetSelectedFunction   where nds.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy(NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM Nodes ROOT   WHERE ROOT.ID in (select ID from GetSelectedFunction g where HierId=2 and Title='Finance & Accounting')       UNION ALL        SELECT  CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  )   SELECT PG.Title, OldValue, NewValue  FROM PG_Hierarchy PG  JOIN (SELECT PL.ObjectID, OldValue, NewValue 			FROM OperatingModelActivityLog PL 			WHERE Object = 'Process' AND Title = 'Updated' AND PL.Modified = ( 			SELECT  Max(Modified) Modified FROM OperatingModelActivityLog  	op		WHERE ObjectID IN (SELECT ID FROM PG_Hierarchy PG WHERE NodeTypeId=3 )  			AND Object = 'Process' AND Title = 'Updated')) A ON A.ObjectID = PG.ID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 227,
                column: "AnswerSQL",
                value: "SELECT COUNT(DISTINCT n.ID) AS [No of Process having 2 owners or Dispositions] FROM Nodes n LEFT JOIN (     SELECT NodeId     FROM NodesToDispositionsForDispositionNew  nn   GROUP BY NodeId     HAVING COUNT(DispositionID) = 2 ) d ON n.ID = d.NodeId LEFT JOIN (     SELECT NodeID     FROM NodesToOwnersForOwnerNew  no   GROUP BY NodeID     HAVING COUNT(OwnerId) = 2 ) o ON n.ID = o.NodeID WHERE n.NodeTypeId = 3 AND (d.NodeId IS NOT NULL OR o.NodeID IS NOT NULL)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 239,
                column: "AnswerSQL",
                value: "SELECT     Title AS [Systems being used by Project Team and Process Group] FROM     Systems s WHERE  ID IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes N                       LEFT JOIN NodesToSystemsForEnablerSystems NSS                        ON N.ID = NSS.NodesId  					  LEFT JOIN  NodesToProjectTeamsForProjectTeam NP 					  ON N.ID = NP.NodesId  					  LEFT JOIN NodeTypes NT ON NT.ID = N.NodeTypeId                    WHERE  NT.Title IN ('ProcessGroup')               )	");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 240,
                column: "AnswerSQL",
                value: "SELECT N.Title [Process groups are not started on Followup Calls for Current State] FROM Nodes N     LEFT JOIN Nodetypes NT         ON NT.ID = N.NodeTypeId     LEFT JOIN NodeTrackers NTC         ON NTC.NodeID = N.ID     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NTC.ID     LEFT JOIN TransactionStates T         ON T.ID = N.TransactionStateID WHERE N.NodeOnTracker = 1       AND NT.Title = 'Process Group'       AND T.[Key] = 'CURRENT_STATE'       AND N.ID NOT IN (                           SELECT N.ID ProcessGroupID                           FROM Nodes N                               LEFT JOIN Nodetypes NT                                   ON NT.ID = N.NodeTypeId                               LEFT JOIN NodeTrackers NTC                                   ON NTC.NodeID = N.ID                               LEFT JOIN NodeTrackerStatuses NTS                                   ON NTS.NodeTrackerID = NTC.ID                               LEFT JOIN TransactionStates T                                   ON T.ID = N.TransactionStateID                           WHERE N.NodeOnTracker = 1                                 AND NT.Title = 'Process Group'                                 AND T.[Key] = 'CURRENT_STATE'                                 AND NTS.Title = 'FollowUp Call'                       )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 241,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes n    WHERE NodeParentID IS NULL      UNION ALL     SELECT nds.id,            nds.Title,            nds.NodeParentID,            HierId = HierId + 1     FROM Nodes nds,          GetSelectedFunction     where nds.NodeParentID = GetSelectedFunction.id    ) SELECT O.Title [Client Owner for HR] FROM GetSelectedFunction N 	LEFT JOIN NodesToOwnersForOwnerNew NOS ON NOS.NodeId = N.ID  	LEFT JOIN Owners O ON O.ID = NOS.OwnerId  WHERE N.HierId = 2 AND N.Title = 'HR'", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 251,
                column: "AnswerSQL",
                value: "SELECT    Title as [Billing Period Name],    DeadlineDate as [Contribution Deadlines]  FROM    TSABillingPeriods tb");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 252,
                column: "AnswerSQL",
                value: " SELECT    Title AS [TSA Phases]  FROM TSAPhases tp");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 255,
                column: "AnswerSQL",
                value: "DECLARE @UserProfiles int; SELECT    @UserProfiles = ID  FROM    UserProfiles up  WHERE    EMail = '{Username}' ; WITH TEMPCTE AS (   SELECT      FunctionId,      SubFunctionId    FROM      TSAReviewers  tr  WHERE      ProviderApproverId = @UserProfiles      OR RecipientApproverId = @UserProfiles )  SELECT    TSA.Title as [List of TSA to be reviewed] FROM    TSAItems TSA    JOIN TEMPCTE ON TSA.FunctionId = TEMPCTE.FunctionId    AND tsa.SubFunctionId = TEMPCTE.SubFunctionId    JOIN TSAPhases TP ON TSA.PhaseId = TP.ID  WHERE    TP.[Key] IN ('ALIGNMENT', 'APPROVAL'); ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 256,
                column: "AnswerSQL",
                value: "SELECT   Title DefaultCurrency FROM  TSACurrencies tc WHERE CurrencyCode ='{CurrencyCode}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 257,
                column: "AnswerSQL",
                value: "SELECT   CurrencyExchangeRateToUSD  FROM  TSACurrencies t WHERE CurrencyCode ={CurrencyCode}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 260,
                column: "AnswerSQL",
                value: "SELECT CASE WHEN SUM(DeadlineReminder) > 0 THEN 'Yes, email reminder before the deadline is setup' ELSE 'No, email reminder before the deadline is setup' END AS [Send an email reminder before the deadline?] FROM( SELECT     Title,     CAST( DeadlineReminder AS INT) DeadlineReminder FROM     TSABillingPeriods tb WHERE     DeadlineReminder = 1) A");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 261,
                column: "AnswerSQL",
                value: "SELECT     Title        AS [Billing Period Name],     DeadlineDate AS [Contribution Deadlines] FROM     TSABillingPeriods t");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 262,
                column: "AnswerSQL",
                value: "SELECT CASE WHEN SUM(SendEmailImmediately) > 0 THEN 'Yes, notifying data contributors by email ' ELSE 'No, not notifying data contributors by email ' END AS [Notify data contributors by email ?] FROM( SELECT     Title                AS [Billing Period Name],     CAST(SendEmailImmediately AS INT) SendEmailImmediately FROM     TSABillingPeriods tb WHERE     SendEmailImmediately = 1) A");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 264,
                column: "AnswerSQL",
                value: "SELECT month_year AS [By Month],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Month] FROM vwTSACostWaterfall  vw GROUP BY date_,          month_year  SELECT [quarter] AS [By Quarter],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Quarter] FROM vwTSACostWaterfall vwc GROUP BY Concat(Year(date_), Datepart(quarter, date_)),          [quarter]    SELECT [year] AS [By Year],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Year] FROM vwTSACostWaterfall cw GROUP BY [year]");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 267,
                column: "AnswerSQL",
                value: "SELECT     [year]                               AS [By Year],     ROUND(SUM(fte_cost_per_month), 2)    AS [TSA FTE Cost for Selected Year],     ROUND(SUM(nonfte_cost_per_month), 2) AS [TSA Non-FTE Cost for Selected Year] FROM     vwTSACostWaterfall wf WHERE      [year] = '{Year}' GROUP BY     [year]");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 268,
                column: "AnswerSQL",
                value: "SELECT     Title AS [TSA Stages] FROM     TSAPhases t ORDER BY     Ordinal");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 270,
                column: "AnswerSQL",
                value: "SELECT     Title AS [TSAs that have duration for more than 18 months] FROM     TSAItems ts WHERE     Duration > 18");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 272,
                column: "AnswerSQL",
                value: "SELECT     f.title  AS [Functions],     SUM((t.TSAItemFTECostForServiceDurationUSD + t.MarkupOnFTECostUSD + t.ExternalMaterialCostUSD) )AS [TSA Costs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND t.Duration IS NOT NULL GROUP BY  f.title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 279,
                column: "AnswerSQL",
                value: "SELECT COUNT(TSA.ID) AS [TSA Items in Alignment phase for IT] FROM TSAItems TSA LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID WHERE Stg.[Key] = 'ALIGNMENT'       AND F.Title = '{FunctionName}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 296,
                column: "AnswerSQL",
                value: "	  SELECT    t.Title AS [List all TSAs which are not getting settled in USD]    FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     	LEFT OUTER JOIN 	    TSACurrencies TSACurr  		    ON t.TSAItemLocalCurrencyId=TSACurr.ID 	LEFT OUTER JOIN Currencies Curr 	        ON TSACurr.CurrencyID=Curr.ID WHERE         Curr.Title<>'USD ($)'  	  AND s.title <> 'Canceled'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 310,
                column: "AnswerSQL",
                value: "SELECT DISTINCT WP._TemplateFile FROM WorkPlan WP");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 319,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Savings'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Revenue Growth'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 321,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'one-time'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 322,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Savings' AND VCI.ProjectTeam = 'IT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 323,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'one-time' AND VCI.ProjectTeam = 'Supply Chain'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                column: "AnswerSQL",
                value: "SELECT DISTINCT N._TemplateFile FROM Nodes N");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 332,
                column: "AnswerSQL",
                value: "SELECT TSA.Title FROM TSAItems TSA");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 333,
                column: "AnswerSQL",
                value: "SELECT TSA.Title, TSA.ServiceInScopeDescription, TSA.[Function], TSA.SubFunction FROM TSAItems TSA");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 336,
                column: "AnswerSQL",
                value: "SELECT DISTINCT WP._TemplateFile FROM WorkPlan WP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantConfigurations",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "AnswerSQL",
                value: "SELECT   RiskImpact,  RiskProbability,  COUNT(*) AS RiskCount  FROM  RisksAndIssues RI LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE IssueRiskCategory = 'Risk'  AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))   AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) AND TT.[Key] = 'PROJECT_MANAGEMENT' GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                column: "AnswerSQL",
                value: "SELECT TOP 1 CAST(PeriodEndDate AS DATE) AS WeeklyStatusDueDate FROM ReportingPeriods WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE) ORDER BY Modified DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "AnswerSQL",
                value: "SELECT W.Title AS WorkPlanItem FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT' AND DATEPART(ww,TaskDueDate) = DATEPART(ww,GETDATE()) + 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 22,
                column: "AnswerSQL",
                value: "SELECT WS.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN WeeklyStatusStatuses WS ON PSE.WeeklyStatusStatusId = WS.ID JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 23,
                column: "AnswerSQL",
                value: "SELECT AN.Title, AN.AccomplishmentNextstepCategory FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) ORDER BY AN.AccomplishmentNextstepCategory");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 24,
                column: "AnswerSQL",
                value: "SELECT Title  FROM Interdependencies  WHERE CAST(ItemDueDate AS DATE) < CAST(GETDATE() AS DATE)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 25,
                column: "AnswerSQL",
                value: "SELECT TOP 1 PT.Title Team, SUM(Counts) TotalInterdependencies FROM ( SELECT ProviderProjectTeamID TeamID, Count(ID) Counts FROM Interdependencies WHERE CAST(ItemDueDate AS DATE) < CAST(GETDATE() AS DATE) GROUP BY ProviderProjectTeamID UNION ALL SELECT ReceiverProjectTeamID, Count(ID) FROM Interdependencies WHERE CAST(ItemDueDate AS DATE) < CAST(GETDATE() AS DATE) GROUP BY ReceiverProjectTeamID ) SUB JOIN ProjectTeams PT ON SUB.TeamID = PT.ID GROUP BY PT.Title ORDER BY TotalInterdependencies DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 26,
                column: "AnswerSQL",
                value: "SELECT RI.Title AS RisksWithNoMitigation FROM RisksandIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE RiskMitigation IS NULL AND IssueRiskCategory = 'Risk' AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 27,
                column: "AnswerSQL",
                value: "SELECT RI.Title AS RisksWithNoMitigation FROM RisksandIssues RI LEFT JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE RI.ItemOwnerID IS NULL AND IssueRiskCategory = 'Risk' AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 28,
                column: "AnswerSQL",
                value: "SELECT W.Title As Milestones FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE WorkplanTaskType = 'Milestone'  AND CAST(StartDate AS DATE) > CAST(GETDATE() AS DATE) AND TT.[Key] = 'PROJECT_MANAGEMENT' AND EXISTS (SELECT 1 FROM [dbo].[WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues] WHERE WorkplanID = W.ID)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "AnswerSQL",
                value: "SELECT AN.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN UserProfiles UP ON PT.ItemOwnerID = UP.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE UP.EMail = '{Username}' AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Accomplishment'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 33,
                column: "AnswerSQL",
                value: "SELECT AN.Title AS NextSteps FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE PT.Title = {ProjectTeam} AND RP.ID = (SELECT ID FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Next Step'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team]  FROM ProjectStatusEntries PSE  LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID  LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID  LEFT JOIN WeeklyStatusStatuses WS ON WS.ID = PSE.WeeklyStatusStatusId  WHERE  RP.ID = (SELECT ID  FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND WS.[KEY] IN ('AT_RISK','BEHIND_SCHEDULE') AND TT.[Key]='PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 42,
                column: "AnswerSQL",
                value: "SELECT    Count(W.ID) as TasksCompletedLastWeek  FROM    Workplan W    JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID    JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID   JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  CAST(ActualEndDate AS DATE) >= CAST(DATEADD(week,DATEDIFF(week,0,GETDATE()), 0) AS DATE)   AND CAST(ActualEndDate AS DATE) < CAST( DATEADD(week,DATEDIFF(week,0, GETDATE()) + 1,0) AS DATE)   AND S.[KEY] NOT IN ('COMPLETED', 'CANCELLED')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 45,
                column: "AnswerSQL",
                value: "SELECT WSS.Title StatusValue ,'CurrentStatus' as Category FROM ProjectStatusEntries PSE LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT TOP 1 ReportingPeriods.ID FROM ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC) AND PT.Title like {ProjectTeam}  UNION SELECT WSS.Title StatusValue ,'PreviousStatus' as Category FROM ProjectStatusEntries PSE  LEFT JOIN WeeklyStatusStatuses WSS ON WSS.ID=PSE.WeeklyStatusStatusId LEFT JOIN ProjectTeams PT on PT.ID=PSE.ProjectTeamId WHERE PSE.ReportingPeriodId=(SELECT  ReportingPeriods.ID FROM   ReportingPeriods Where cast(ReportingPeriods.PeriodStartDate as date) <= cast(getDate() as date) Order BY PeriodEndDate DESC Offset 1 Rows FETCH NEXT 1 ROWS ONLY) AND  PT.Title like {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "AnswerSQL",
                value: "SELECT ProjectTeam FROM(   SELECT      PT.Title AS Projectteam,     COUNT(PT.ID) AS TeamUpdateCount, DENSE_RANK() OVER (ORDER BY COUNT(PT.ID)) ROW_NUM FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      CAST(W.Modified AS DATE) BETWEEN DATEADD(day, -14, CAST(GETDATE() AS DATE)) AND DATEADD(day, -7, CAST(GETDATE() AS DATE)) AND PT.ItemIsActive=1 AND TT.[Key] =  'PROJECT_MANAGEMENT' GROUP BY      pt.title) A");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                column: "AnswerSQL",
                value: "SELECT      W.Title AS [List of Critical Milestones due in next 15 days with no owners Assigned yet] FROM      WorkPlan W JOIN ProjectTeams PT ON PT.ID = W.ProjectTeamId JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE      WorkPlanTaskType IN ('Milestone')     AND CAST(TaskDueDate AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(day, 15, CAST(GETDATE() AS DATE))     AND IsCritical IN (1)     AND TaskOwnerId IS NULL AND TT.[Key] =  'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 60,
                column: "AnswerSQL",
                value: "SELECT Title as [List down all tasks associated with interdependencies] from Workplan where ID in ( SELECT WorkPlanId FROM WorkPlansToInterdependenciesForReceiverTasks UNION select WorkPlanId FROM WorkPlansToInterdependenciesForProviderTasks) AND WorkPlanTaskType='Task'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 62,
                column: "AnswerSQL",
                value: "SELECT W.Title AS WorkPlanItem FROM Workplan W LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE TT.[Key] = 'PROJECT_MANAGEMENT'  AND DATEPART(ww,TaskDueDate) = DATEPART(ww,GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                column: "AnswerSQL",
                value: "SELECT W.Title, W.WorkPlanTaskType, W.StartDate, W.TaskDueDate, PT.Title ProjectTeam, S.Title [Status], UP.Title TaskOwner FROM Workplan W LEFT JOIN Statuses S ON W.WorkPlanTaskStatusId = S.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE UniqueItemIdentifier = 'HR.2.2.6'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                column: "AnswerSQL",
                value: "SELECT PT.Title FROM ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId WHERE ManageProjectStatus = 1 AND  TT.[Key] = 'PROJECT_MANAGEMENT'  EXCEPT   SELECT PT.Title  FROM ProjectStatusEntries PSE  LEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodId = RP.ID  LEFT JOIN ProjectTeams PT ON PSE.ProjectTeamId = PT.ID  WHERE RP.ID = (SELECT ID FROM ReportingPeriods WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE)      )   AND PSE.WeeklyStatusStatusId IS NOT NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                column: "AnswerSQL",
                value: "SELECT  COUNT(R.ID) AS NewRisksCount FROM  RisksAndIssues R JOIN ProjectTeams PT ON R.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE  IssueRiskCategory = 'Risk' AND DATEDIFF(day, R.Created, GETDATE()) <= 5 AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 77,
                column: "AnswerSQL",
                value: "SELECT      AccomplishmentsAndNextSteps.Title FROM     ProjectStatusEntries     LEFT OUTER JOIN AccomplishmentsAndNextSteps ON ProjectStatusEntries.ID = AccomplishmentsAndNextSteps.ProjectStatusEntryId     LEFT JOIN ProjectTeams PT ON PT.ID = ProjectStatusEntries.ProjectTeamId     LEFT OUTER JOIN ReportingPeriods ON ProjectStatusEntries.ReportingPeriodId = ReportingPeriods.ID  WHERE     AccomplishmentNextStepCategory IN '{AccomplishmentNextStepCategory}' AND ReportingPeriods.PeriodStartDate = (         SELECT TOP 1 PeriodStartDate          FROM ReportingPeriods          WHERE CAST (GETDATE() AS DATE) BETWEEN CAST(PeriodStartDate AS DATE) AND CAST(PeriodEndDate AS DATE)      ) ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 78,
                column: "AnswerSQL",
                value: ";WITH CTE AS (SELECT COUNT(*) AS OverdueItemCount,      PT.Title AS [ProjectTeam With Most Overdue Item]  FROM WorkPlan W  LEFT JOIN [ProjectTeams] PT ON PT.ID = W.ProjectteamId   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId LEFT JOIN Statuses S ON S.ID = W.WorkPlanTaskStatusId  WHERE  	  S.[Key] <> '{Key}'    AND TT.[Key]='PROJECT_MANAGEMENT'    AND CAST(W.TaskDueDate AS DATE ) < CAST(GETDATE() AS DATE) 	  AND PT.Title IS NOT NULL	 GROUP BY PT.Title )  SELECT Top 1 [ProjectTeam With Most Overdue Item] FROM CTE ORDER BY OverdueItemCount DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 79,
                column: "AnswerSQL",
                value: "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(RevenueGrowthEstimate,0)) [RevenueGrowthTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title = {ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(CostToAchieveEstimate,0)) [CostToAchieveTarget] FROM [ValueCaptureTopDownEstimates] VT LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId WHERE PT.Title ={ProjectTeam}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) [HeadCountCostReductionTarget] FROM [ValueCaptureTopDownEstimates] VT");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "AnswerSQL",
                value: "SELECT SUM(NonHeadcountCostReductionEst) + SUM(RevenueGrowthEstimate) + SUM(HeadcountCostReductionEst) 'Total Top Down Target Value' FROM ValueCaptureTopDownEstimates");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "AnswerSQL",
                value: "SELECT SUM(RevenueGrowthEstimate) RevenueGrowth,SUM(TotalCostReduction) CostReduction FROM ValueCaptureTopDownEstimates");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 95,
                column: "AnswerSQL",
                value: "SELECT COUNT(ID) FROM ValueCaptureInitiatives WHERE IsItemActive = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "AnswerSQL",
                value: "SELECT PT.Title [Project Team] ,CAST(SUM(ISNULL(RevenueGrowthEstimate,0))/1000000 AS FLOAT) [Revenue Growth(in Million)] FROM [ValueCaptureTopDownEstimates] VT 	LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId GROUP BY PT.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "AnswerSQL",
                value: "SELECT VC.Title AS [ValueCaptureInitiatives with no Owners] FROM ValueCaptureInitiatives VC WHERE ItemOwnerId IS NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "AnswerSQL",
                value: "SELECT VP.Title AS ValueCapturepriority, COUNT(VC.ValueCapturePriorityId) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId where ValueCapturePriorityId is not null GROUP BY ValueCapturePriorityId, VP.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "AnswerSQL",
                value: "SELECT UP.Title AS UserProfile, COUNT(VC.ID) AS ValueCaptureOwnerwiseCount FROM ValueCaptureInitiatives VC LEFT JOIN UserProfiles UP ON UP.ID = VC.ItemOwnerId WHERE ItemOwnerId IS NOT NULL GROUP BY ItemOwnerId, UP.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "AnswerSQL",
                value: "SELECT PT.Title,SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(RevenueGrowthEstimate,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)) AS TotalTopDownTarget FROM ValueCaptureTopDownEstimates LEFT JOIN ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY PT.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "AnswerSQL",
                value: "SELECT VCS.Title AS ValueCaptureStage, COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VCS ON VCS.ID = VC.ValueCaptureStageId GROUP BY ValueCaptureStageId, VCS.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 111,
                column: "AnswerSQL",
                value: "SELECT F.Title as Functions, COUNT(VC.FunctionId) AS ValueCaptureFunction FROM ValueCaptureInitiatives VC LEFT JOIN Functions f ON f.ID = VC.FunctionId WHERE FunctionId IS NOT NULL GROUP BY FunctionId, F.Title ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                column: "AnswerSQL",
                value: "SELECT COUNT(ID) AS [Top Down targets #],        SUM(ISNULL(NonHeadcountCostReductionEst, 0)) AS [Total Non Headcount Cost Reduction Estimate],        SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Revenue Growth Estimate],        SUM(ISNULL(CostToAchieveEstimate, 0)) AS [Total Cost To Achieve Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) AS [Total Headcount Cost Reduction Estimate],        SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "AnswerSQL",
                value: "SELECT  VCTT.Title, PT.Title, VCTT.CostToAchieveEstimate, VCTT.NonHeadcountCostReductionEst, VCTT.HeadcountCostReductionEst, VCTT.RevenueGrowthEstimate FROM   ValueCaptureTopDownEstimates VCTT  Left Join ProjectTeams PT on PT.ID = VCTT.ProjectTeamId");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(HeadcountCostReductionEst,0)) AS HeadcountCostReduction FROM ValueCaptureTopDownEstimates");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "AnswerSQL",
                value: "SELECT  SUM(ISNULL(NonHeadcountCostReductionEst,0)) AS NonHeadcountCostReduction FROM  ValueCaptureTopDownEstimates");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                column: "AnswerSQL",
                value: "SELECT  SUM(ISNULL(RevenueGrowthEstimate,0)) AS RevenueGrowth FROM  ValueCaptureTopDownEstimates");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                column: "AnswerSQL",
                value: "SELECT  SUM(ISNULL(CostToAchieveEstimate,0)) AS CostToAchive FROM  ValueCaptureTopDownEstimates");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120,
                column: "AnswerSQL",
                value: "SELECT Amount as [Client Baseline PnL Amount], A.FinancialLineItem as FinancialLineItem FROM( SELECT F.ID, F.Ordinal, F.Title FinancialLineItem , SUM(C.Amount)/1000000 Amount FROM ValueCaptureFinancialLineItems F LEFT JOIN CostCenters C ON C.FinancialLineItemId = F.ID  GROUP BY F.ID, F.Title, F.Ordinal ) A WHERE A.Amount IS NOT NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121,
                column: "AnswerSQL",
                value: "SELECT     Title AS [Recommended Initiatives List] FROM     ValueCaptureRecommendedInitiatives");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122,
                column: "AnswerSQL",
                value: "SELECT 	SUM(a.[Total Top Down Target Value]) AS [Top Down Targets],      SUM(b.[Bottom Up Initiatives Value]) AS [Bottom Up Initiatives Value], 	( SUM(b.[Bottom Up Initiatives Value])-SUM(a.[Total Top Down Target Value])) AS Variance FROM ( SELECT   SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates) a, ( select SUM(Amount)*12 as [Bottom Up Initiatives Value]  FROM vwUnpivotEstimates where MYear='Y3M12' AND Recurring =1 ) b ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                column: "AnswerSQL",
                value: "SELECT     CONCAT('Tracking Period of the Program is from ', CAST(MIN(StartDt) AS DATE), ' to ', CAST(MAX(EndDt) AS DATE)) FROM     ValueCaptureTransactionMonths WHERE     IsItemActive = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                column: "AnswerSQL",
                value: "SELECT  ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = '{ProjectTeam}' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = '{ProjectTeam}'    ) Act         ON 1 = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                column: "AnswerSQL",
                value: "SELECT Title Initiatives FROM ValueCaptureInitiatives WHERE ValueCapturePriorityId = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { "SELECT  	ISNULL(SUM(UE.Amount)/1000000,0) AS TotalNetSales FROM ValueCaptureEstimates VE LEFT JOIN ValueCaptureFinancialLineItems FL ON FL.ID = VE.FinancialLineItemId LEFT JOIN ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN vwUnpivotEstimates UE ON UE.ID = VE.ID LEFT JOIN ValueCaptureFinancialLineItemTypes FLT ON FLT.ID = FL.FinancialLineItemTypeId WHERE FLT.Title = 'PnL' -- pnl 	AND FL.Title in ('Net sales') -- net total", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                column: "AnswerSQL",
                value: "SELECT  	SUM(NonHeadcountCostReductionEst) NonHeadcountCostReduction, 	SUM(HeadcountCostReductionEst) HeadcountCostReduction FROM ValueCaptureTopDownEstimateS");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                column: "AnswerSQL",
                value: ";WITH CTE AS  ( 	SELECT ProjectTeamID  	FROM ValueCaptureEstimates E 		LEFT JOIN ValueCaptureInitiatives I ON I.ID = E.ValueCaptureInitiativeId 	EXCEPT 	SELECT ProjectTeamID FROM [ValueCaptureTopDownEstimates] )  SELECT PT.Title [ProjectTeam]  FROM CTE LEFT JOIN ProjectTeams PT ON PT.ID = CTE.ProjectTeamID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                column: "AnswerSQL",
                value: " DECLARE @CurrentYR NVARCHAR(10), @SQL NVARCHAR(MAX)  SELECT  TOP 1 @CurrentYR = [YEAR] FROM vwValueCaptureTransactionMonths WHERE RelativeYear = YEAR(GETDATE())   SELECT @SQL = ' ;WITH TopDownCTE AS ( 	SELECT  		PT.ID  PROJECTTEAMID 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(RevenueGrowthEstimate,0))  + SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM [ValueCaptureTopDownEstimates] VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 	GROUP BY  		PT.ID  		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(RevenueGrowthEstimate,0))  + SUM(ISNULL(NonHeadcountCostReductionEst,0)) + SUM(ISNULL(HeadcountCostReductionEst,0)))/1000000  [Target] 	FROM [ValueCaptureTopDownEstimates] VT 		LEFT JOIN ProjectTeams PT ON PT.ID = VT.ProjectTeamId 	WHERE PT.ManageValueCapture = 1 ) , BottomUPCTE AS ( 	SELECT  		VI.PROJECTTEAMID 		,PT.Title [ProjectTeam] 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 	GROUP BY  		VI.PROJECTTEAMID 		,PT.Title 	UNION ALL 	SELECT  		-1 		,''Overall'' 		,(SUM(ISNULL(E.'+@CurrentYR+'M12Estimate,0))*12)/1000000 [Estimate] 	FROM ValueCaptureEstimates E 	 JOIN ValueCaptureInitiatives VI ON VI.ID =E.ValueCaptureInitiativeId 	 LEFT JOIN ProjectTeams PT ON PT.ID = VI.ProjectTeamId 	WHERE  		VI.IsItemActive = 1  		AND E.Recurring = 1  		AND PT.ManageValueCapture = 1 )  SELECT  	COALESCE(T.PROJECTTEAMID,B.PROJECTTEAMID) ProjectTeamID 	,COALESCE(T.ProjectTeam,B.ProjectTeam) ProjectTeam 	,ISNULL(T.[Target],0) [TopDown Target(M)] 	,ISNULL(B.Estimate,0) [BottomUP Estimate(M)] 	,ISNULL(T.[Target],0) - ISNULL(B.Estimate,0) [Variance(M)] FROM TopDownCTE T 	FULL OUTER JOIN BottomUPCTE B ON T.PROJECTTEAMID = B.ProjectTeamId ORDER BY PROJECTTEAMID'  EXEC sp_executesql  @SQL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                column: "AnswerSQL",
                value: "SELECT      VC.Title AS ValueCaptureInitiativesReadyForApproval FROM      ValueCaptureInitiatives VC WHERE  	ReadyForApproval = 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                column: "AnswerSQL",
                value: "SELECT TOP 5     Initiatives,Amount FROM( SELECT VI.Title Initiatives ,sum(VE.Amount) Amount FROM      vwUnpivotActuals VE LEFT JOIN      ValueCaptureInitiatives VI ON VI.id = VE.ValueCaptureInitiativeId LEFT JOIN 	ValueCaptureImpactTypes VIT ON VIT.ID = VE.ValueCaptureImpactTypeid GROUP BY      VI.Title,VE.Amount ) A  WHERE A.Amount > 0 ORDER BY  	Amount DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                column: "AnswerSQL",
                value: "SELECT TOP 1 	PT.Title, SUM(ISNULL(HeadcountCostReductionEst,0)) AS MostHeadcountCostReduction FROM  	ValueCaptureTopDownEstimates LEFT JOIN  	ProjectTeams PT ON PT.ID = ValueCaptureTopDownEstimates.Projectteamid GROUP BY  	PT.Title ORDER BY  	SUM(ISNULL(HeadcountCostReductionEst,0)) DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                column: "AnswerSQL",
                value: "SELECT  	Title as [Value Capture Initiatives with No Owners] FROM 	ValueCaptureInitiatives as VC WHERE 	ItemOwnerId IS NULL");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                column: "AnswerSQL",
                value: "SELECT ISNULL(VP.Title, 'No Priority') AS ValueCapturePriority,     COUNT(VC.ID) AS ValueCapturePriorityCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCapturePriorities VP ON VP.ID = VC.ValueCapturePriorityId --WHERE VC.ValueCapturePriorityId IS NOT NULL GROUP BY VC.ValueCapturePriorityId, VP.Title  ---Stage---  SELECT ISNULL(VS.Title, 'No Stage Assigned') AS ValueCaptureStage,     COUNT(VC.ID) AS ValueCaptureStageCount FROM ValueCaptureInitiatives VC LEFT JOIN ValueCaptureStages VS ON VS.ID = VC.ValueCaptureStageId WHERE VC.ValueCaptureStageId IS NOT NULL GROUP BY VC.ValueCaptureStageId, VS.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                column: "AnswerSQL",
                value: "SELECT SUM(ISNULL(E.Amount, 0) * 12) [Run Rate for iniatives in Approved, Committed and Realized] FROM ValueCaptureInitiatives VI     LEFT JOIN vwUnpivotEstimates E         ON E.ValueCaptureInitiativeId = VI.ID     LEFT JOIN ValueCaptureStages VS         ON VS.ID = VI.ValueCaptureStageID WHERE VS.Title IN ( 'Approved', 'Committed', 'Realized' )       AND MYear =       (           select [Key]           from ValueCaptureTransactionMonths           where CAST(EndDt AS date) =           (               SELECT MAX(CAST(EndDt AS date)) FROM ValueCaptureTransactionMonths           )       )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                column: "AnswerSQL",
                value: "SELECT  	VI.Title [Initiatives with Benefit score over 75] FROM 	ValueCaptureInitiatives VI LEFT JOIN  	ProjectTeams PT ON PT.ID = VI.ProjectTeamId WHERE 	CAST(BenefitScore AS INT) > 75 	AND PT.Title IN ('HR')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                column: "AnswerSQL",
                value: "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                column: "AnswerSQL",
                value: "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                column: "AnswerSQL",
                value: "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                column: "AnswerSQL",
                value: "SELECT d.Title FROM Dispositions d");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                column: "AnswerSQL",
                value: "SELECT Count(s.ID) AS [# Systems present in this functional OP model] FROM Systems s");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                column: "AnswerSQL",
                value: "SELECT Count(tpa.ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements tpa");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                column: "AnswerSQL",
                value: "SELECT [Object],        OMA.Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        OMA.Title AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.Modified >= DATEADD(hh, -1, GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188,
                column: "AnswerSQL",
                value: "SELECT [Object],        Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        OMA.Title AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.FieldImpacted = 'Owner'       AND CAST(OMA.Modified AS DATE) = CAST(GETDATE() AS DATE)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189,
                column: "AnswerSQL",
                value: "SELECT [Object],        Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        OMA.Title AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.Title = 'Deleted'       AND DATEPART(wk, CAST(OMA.Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(OMA.Modified) = YEAR(GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 196,
                column: "AnswerSQL",
                value: " SELECT     PR.Title [Process] FROM     Nodes                                    PR     JOIN         TransactionStates                    S             ON S.ID = PR.TransactionStateId     LEFT JOIN         NodesToDispositionsForDispositionNew D             ON D.NodeId = PR.ID WHERE     PR.NodeTypeId = 3     AND D.NodeId IS NULL     AND S.[Key] = 'DAY_ONE'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 197,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,       --SELECT * FROM PG_Hierarchy         ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT TOP 1     Functions AS [Function with highest number of processes] FROM (     SELECT Functions,            COUNT(ID) as [Process Count]     FROM ProcessCTE     GROUP BY Functions ) A order by [Process Count] desc    ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 198,
                column: "AnswerSQL",
                value: "SELECT     N.Title AS [List of Processes with Disposition as TSA] FROM   Nodes   N   LEFT JOIN     NodesToDispositionsForDispositionNew NTD             ON NTD.NodeId = N.ID     LEFT JOIN         Dispositions D  ON NTD.DispositionId = D.ID WHERE     D.Title = 'TSA'     AND N.NodeTypeId = 3 -- To get the list of Processes");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 199,
                column: "AnswerSQL",
                value: "SELECT       N.Title as [List of Processes with No Disposition] FROM Nodes N          WHERE N.NodeTypeId=3 AND N.ID NOT IN   (SELECT DN.NodeId FROM NodesToDispositionsForDispositionNew DN)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 202,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ),       --SELECT * FROM PG_Hierarchy         ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT ISNULL(Geo.Title, 'Not Assigned') AS [Geographic Coverages],        COUNT(P.ProcessID) AS [Total Number of Process in Real Estate] FROM ProcessCTE P     LEFT JOIN ProcessAttributes PA         ON P.ProcessID = PA.NodeId     LEFT JOIN GeographicCoverages Geo         ON PA.GeographicCoverageId = Geo.ID WHERE P.Functions ='{FunctionName}' GROUP BY Geo.Title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 203,
                column: "AnswerSQL",
                value: ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      F.Title AS [Functions],     SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID ORDER BY 1,          2");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 205,
                column: "AnswerSQL",
                value: ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ) SELECT DISTINCT      SF.[Title] AS  [Subfunctions] FROM (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F     LEFT JOIN     (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF         ON F.ID = SF.NodeParentID WHERE F.Title='{FunctionName}' ORDER BY 1");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 206,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,       --SELECT * FROM PG_Hierarchy        ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID AS [ProcessID]     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT P.Processes,        ISNULL(S.Title, 'Not Assigned') AS [Systems],        ISNULL(TPA.Title, 'Not Assigned') AS [Third Party Agreements] FROM ProcessCTE P     LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA         ON P.ProcessID = NTPA.NodesId     LEFT JOIN ThirdPartyAgreements TPA         ON NTPA.ThirdPartyAgreementsId = TPA.ID     LEFT JOIN NodesToSystemsForEnablerSystems NSys         ON NSys.NodesId = P.ProcessID     LEFT JOIN Systems S         ON NSys.SystemsId = S.ID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 209,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes  N    WHERE NodeParentID IS NULL           AND TransactionStateID = 8     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ) SELECT N.Title [Function],        NTSS.Title AS [Field Status],        OPS.Title AS [Op Status] FROM NodeTrackers NT     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NT.ID     JOIN     (SELECT * FROM GetSelectedFunction GSF WHERE HierId = 2 and Title = 'HR') N         ON N.ID = NT.NodeId     LEFT JOIN NodeTrackerStatusFields NTSS         ON NTSS.ID = NTS.NodeTrackerStatusFieldId     LEFT JOIN OpStatuses OPS         ON OPS.ID = NTS.NodeTrackerStatusStatusId");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 210,
                column: "AnswerSQL",
                value: "/*---- What is Automatic Workplan?----*/ SELECT D.Title Disposition,        WP.Title Tasks FROM NodesToWorkPlansForTask NWP     LEFT JOIN Nodes N         ON N.ID = NWP.NodeId     LEFT JOIN WorkPlan WP         ON NWP.WorkPlanId = WP.ID     LEFT JOIN NodesToDispositionsForDispositionNew ND         ON ND.NodeID = N.ID     LEFT JOIN Dispositions D         ON D.ID = ND.DispositionID WHERE N.NodeTypeId = 3");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes ND    WHERE NodeParentID IS NULL           AND TransactionStateID = 6     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM Nodes ROOT     WHERE ROOT.ID in (  select ID from GetSelectedFunction GSF where HierId = 2 and Title = 'Finance'  )     UNION ALL     SELECT CHILD.NodeParentID, CHILD.ID,   CHILD.Title, PARENT.TITLE,   PARENT.ID, CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ) SELECT COUNT(*) AS [Process with multiple dispositions for Finance] FROM PG_Hierarchy WHERE NodeTypeId = 3       AND ID IN (                     SELECT NodeID                     FROM                     (                         SELECT NodeId,                                Count(DispositionID) As [Disposition Count] FROM NodesToDispositionsForDispositionNew NDDN  GROUP BY NodeId HAVING Count(DispositionID) > 1 ) a  )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 212,
                column: "AnswerSQL",
                value: "SELECT     N.Title AS [List of Process which doesn''t have ownership] FROM  Nodes   N  JOIN  TransactionStates T  ON T.ID = N.TransactionStateId WHERE     OwnerId IS NULL     AND N.NodeTypeId = 3 -- To select Node Type = Process");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 213,
                column: "AnswerSQL",
                value: "SELECT N.Title AS [List of Processes with more than one Disposition] FROM Nodes N WHERE N.NodeTypeId = 3       AND N.ID IN (                       SELECT NodeID                       FROM                       (                           SELECT NodeId,                                  COUNT(DispositionID) AS [Disposition Count]                           FROM NodesToDispositionsForDispositionNew                           GROUP BY NodeId                           HAVING COUNT(DispositionID) > 1                       ) a                   ) ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT ID,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes  WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.ID,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, TransactionStateID) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.TransactionStateID     FROM Nodes ROOT     WHERE ROOT.ID IN (                          SELECT ID                          FROM GetSelectedFunction                          WHERE HierId = 2                                AND Title = '{FunctionName}'                                AND TransactionStateId = 6                      )     UNION ALL     SELECT CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.TransactionStateId     FROM PG_Hierarchy PARENT,          Nodes CHILD     WHERE PARENT.id = CHILD.NodeParentID    ) SELECT Title AS [List of Processes Associated with Legal Function] FROM PG_Hierarchy ORDER BY ID,          title DESC ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT P.Functions, A.Processes  FROM (SELECT DISTINCT [Processes]  		FROM ProcessCTE 		GROUP BY Processes  		HAVING COUNT(DISTINCT [Functions] ) > 1) A  	LEFT JOIN ProcessCTE P ON P.Processes = A.Processes ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 219,
                column: "AnswerSQL",
                value: "SELECT     Title AS [Systems not tagged in any process] FROM     Systems WHERE     ID NOT IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes                               N                       LEFT JOIN                           NodesToSystemsForEnablerSystems NSS                               ON N.ID = NSS.NodesId  					  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId                    WHERE T.Title= 'Day 1' AND                        N.NodeTypeId = 3               )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 220,
                column: "AnswerSQL",
                value: "SELECT     ISNULL(ST.Title, 'System Type Not Assigned') AS [SystemsType],     S.Title AS [Systems] FROM     Systems         S     LEFT JOIN         SystemTypes ST             ON S.TypeId = ST.ID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 221,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nod  WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number for Multiple Deposition]  				FROM ProcessCTE 				WHERE ID IN ( 									SELECT NodeID 									FROM 									( 										SELECT NodeId, 											   Count(DispositionID) As [Disposition Count] 										FROM NodesToDispositionsForDispositionNew 										GROUP BY NodeId 										HAVING Count(DispositionID) > 1 									) a 								) 		GROUP BY [Functions] 				ORDER BY  COUNT(*) DESC    ", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 222,
                column: "AnswerSQL",
                value: ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nod  WHERE NodeParentID IS NULL   UNION ALL         SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   WHERE Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2    UNION ALL         SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  WHERE  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS (  SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Live Without]  FROM ProcessCTE WHERE ID IN (	SELECT NodeId 				FROM NodesToDispositionsForDispositionNew ND  				LEFT JOIN Dispositions D ON D.ID = DispositionID  				WHERE D.Title = 'Live without'			 				) GROUP BY [Functions] ORDER BY  COUNT(*) DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 223,
                column: "AnswerSQL",
                value: ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID, NodeTypeId, HierId = 1 FROM Nodes nod WHERE NodeParentID IS NULL  UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,  Nodes.NodeTypeId,  HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy([Functions],NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.Title [Functions],ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM GetSelectedFunction ROOT WHERE HierId = 2       UNION ALL        SELECT  PARENT.[Functions],CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  ), ProcessCTE AS ( SELECT DISTINCT P.Functions, P.Title Processes, P.ID  FROM (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF 	LEFT JOIN (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P ON P.NodeParentID = SF.ID WHERE P.Title IS NOT NULL  )   SELECT Top 1 [Functions] AS [Function with Highest Number of Processes tagged to them] , COUNT(*)  [No of Processes] 				FROM ProcessCTE 		GROUP BY [Functions] 				ORDER BY  COUNT(*) DESC    ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 224,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH PG_Hierarchy AS (SELECT id,            Title,            NodeParentID,            TransactionStateID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.TransactionStateId,            HierId = HierId + 1     FROM Nodes,          PG_Hierarchy     WHERE Nodes.NodeParentID = PG_Hierarchy.id    ),       ProcessCTE AS (SELECT DISTINCT         F.Title AS [Functions],         f.id AS FunctionID     FROM     (SELECT * FROM PG_Hierarchy WHERE HierId = 2) F         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE HierId = 3) SF             ON F.ID = SF.NodeParentID    ) SELECT Top 1     Functions as [Function with Highest Number of Enablers],     [Enablers Count] FROM (     SELECT P.Functions,            --,COUNT(DISTINCT TPA.ID) as TPACount            --,COUNT(DISTINCT S.ID) as SystemCount            --,COUNT(DISTINCT Asset.ID) as AssetCount            --,COUNT(DISTINCT Roles.ID) as RolesCount,            COUNT(DISTINCT TPA.ID) + COUNT(DISTINCT S.ID) + COUNT(DISTINCT Asset.ID) + COUNT(DISTINCT Roles.ID) AS [Enablers Count]     FROM ProcessCTE P         LEFT JOIN NodesToThirdPartyAgreementsForEnablerTpa NTPA --Third Party Aggrements             ON P.FunctionID = NTPA.NodesId         LEFT JOIN ThirdPartyAgreements TPA             ON NTPA.ThirdPartyAgreementsId = TPA.ID         LEFT JOIN NodesToSystemsForEnablerSystems NSys -- Systems             ON NSys.NodesId = P.FunctionID         LEFT JOIN Systems S             ON NSys.SystemsId = S.ID         LEFT JOIN NodesToAssetsForEnablerAssets NAsset --Assets             ON NAsset.NodesId = P.FunctionID         LEFT JOIN Assets Asset             ON Asset.ID = NAsset.AssetsId         LEFT JOIN NodesToRolesForEnablerRoles NRole --Roles             ON NAsset.NodesId = P.FunctionID         LEFT JOIN OrgRolesMaster Roles             ON Roles.ID = NRole.RolesId     GROUP BY P.Functions ) A ORDER BY [Enablers Count] DESC", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 225,
                column: "AnswerSQL",
                value: ";WITH  GetSelectedFunction AS  (  SELECT id,Title,NodeParentID,  HierId = 1 FROM Nodes  WHERE NodeParentID IS NULL    UNION ALL        SELECT    Nodes.id,Nodes.Title,Nodes.NodeParentID,    HierId = HierId+1 FROM Nodes, GetSelectedFunction   where Nodes.NodeParentID = GetSelectedFunction.id )  ,PG_Hierarchy(NodeParentID, ID,Title,Parent,ParentID,NodeTypeId) AS  (  SELECT ROOT.NodeParentID,ROOT.id,ROOT.Title,ROOT.Title,ROOT.ID AS ParentID, ROOT.NodeTypeId FROM Nodes ROOT   WHERE ROOT.ID in (select ID from GetSelectedFunction where HierId=2 and Title='Finance & Accounting')       UNION ALL        SELECT  CHILD.NodeParentID,CHILD.ID,CHILD.Title,PARENT.TITLE,PARENT.ID,CHILD.NodeTypeId  FROM PG_Hierarchy PARENT ,Nodes   CHILD  where  PARENT.id=CHILD.NodeParentID  )   SELECT PG.Title, OldValue, NewValue  FROM PG_Hierarchy PG  JOIN (SELECT PL.ObjectID, OldValue, NewValue 			FROM OperatingModelActivityLog PL 			WHERE Object = 'Process' AND Title = 'Updated' AND PL.Modified = ( 			SELECT  Max(Modified) Modified FROM OperatingModelActivityLog  			WHERE ObjectID IN (SELECT ID FROM PG_Hierarchy PG WHERE NodeTypeId=3 )  			AND Object = 'Process' AND Title = 'Updated')) A ON A.ObjectID = PG.ID");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 227,
                column: "AnswerSQL",
                value: "SELECT COUNT(DISTINCT n.ID) AS [No of Process having 2 owners or Dispositions] FROM Nodes n LEFT JOIN (     SELECT NodeId     FROM NodesToDispositionsForDispositionNew  ndd   GROUP BY NodeId     HAVING COUNT(DispositionID) = 2 ) d ON n.ID = d.NodeId LEFT JOIN (     SELECT NodeID     FROM NodesToOwnersForOwnerNew  non   GROUP BY NodeID     HAVING COUNT(OwnerId) = 2 ) o ON n.ID = o.NodeID WHERE n.NodeTypeId = 3 AND (d.NodeId IS NOT NULL OR o.NodeID IS NOT NULL)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 239,
                column: "AnswerSQL",
                value: "SELECT     Title AS [Systems being used by Project Team and Process Group] FROM     Systems WHERE  ID IN (                   SELECT DISTINCT                       SystemsId                   FROM                       Nodes N                       LEFT JOIN NodesToSystemsForEnablerSystems NSS                        ON N.ID = NSS.NodesId  					  LEFT JOIN  NodesToProjectTeamsForProjectTeam NP 					  ON N.ID = NP.NodesId  					  LEFT JOIN NodeTypes NT ON NT.ID = N.NodeTypeId                    WHERE  NT.Title IN ('ProcessGroup')               )	");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 240,
                column: "AnswerSQL",
                value: "SELECT N.Title [Process groups are not started on Followup Calls for Current State] FROM Nodes N     LEFT JOIN Nodetypes NT         ON NT.ID = N.NodeTypeId     LEFT JOIN NodeTrackers NTC         ON NTC.NodeID = N.ID     LEFT JOIN NodeTrackerStatuses NTS         ON NTS.NodeTrackerID = NTC.ID     LEFT JOIN TransactionStates T         ON T.ID = N.TransactionStateID WHERE N.NodeOnTracker = 1       AND NT.Title = 'Process Group'       AND T.[Key] = 'CURRENT_STATE'       AND N.ID NOT IN (SELECT N.ID ProcessGroupID   FROM Nodes N  LEFT JOIN Nodetypes NT ON NT.ID = N.NodeTypeId LEFT JOIN NodeTrackers NTC  ON NTC.NodeID = N.ID LEFT JOIN NodeTrackerStatuses NTS ON NTS.NodeTrackerID = NTC.ID  LEFT JOIN TransactionStates T  ON T.ID = N.TransactionStateID   WHERE N.NodeOnTracker = 1 AND NT.Title = 'Process Group' AND T.[Key] = 'CURRENT_STATE' AND NTS.Title = 'FollowUp Call')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 241,
                columns: new[] { "AnswerSQL", "VisibleToAssistant" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL      UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ) SELECT O.Title [Client Owner for HR] FROM GetSelectedFunction N 	LEFT JOIN NodesToOwnersForOwnerNew NOS ON NOS.NodeId = N.ID  	LEFT JOIN Owners O ON O.ID = NOS.OwnerId  WHERE N.HierId = 2 AND N.Title = 'HR'", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 251,
                column: "AnswerSQL",
                value: "SELECT    tb.Title as [Billing Period Name],    tb.DeadlineDate as [Contribution Deadlines]  FROM    TSABillingPeriods tb");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 252,
                column: "AnswerSQL",
                value: " SELECT    tp.Title AS [TSA Phases]  FROM TSAPhases tp");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 255,
                column: "AnswerSQL",
                value: "DECLARE @UserProfiles int; SELECT    @UserProfiles = ID  FROM    UserProfiles up WHERE    EMail = '{Username}'; WITH TEMPCTE AS (   SELECT      tr.FunctionId,      tr.SubFunctionId    FROM      TSAReviewers tr   WHERE      tr.ProviderApproverId = @UserProfiles      OR tr.RecipientApproverId = @UserProfiles )  SELECT    TSA.Title as [List of TSA to be reviewed] FROM    TSAItems TSA    JOIN TEMPCTE ON TSA.FunctionId = TEMPCTE.FunctionId    AND tsa.SubFunctionId = TEMPCTE.SubFunctionId    JOIN TSAPhases TP ON TSA.PhaseId = TP.ID  WHERE    TP.[Key] IN ('ALIGNMENT', 'APPROVAL'); ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 256,
                column: "AnswerSQL",
                value: "SELECT   tsa.Title DefaultCurrency FROM  TSACurrencies tsa WHERE tsa.CurrencyCode ='{CurrencyCode}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 257,
                column: "AnswerSQL",
                value: "SELECT   tsa.CurrencyExchangeRateToUSD  FROM  TSACurrencies tsa WHERE tsa.CurrencyCode ={CurrencyCode}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 260,
                column: "AnswerSQL",
                value: "SELECT CASE WHEN SUM(DeadlineReminder) > 0 THEN 'Yes, email reminder before the deadline is setup' ELSE 'No, email reminder before the deadline is setup' END AS [Send an email reminder before the deadline?] FROM( SELECT     tsa.Title,     CAST( tsa.DeadlineReminder AS INT) DeadlineReminder FROM     TSABillingPeriods tsa WHERE     DeadlineReminder = 1) A");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 261,
                column: "AnswerSQL",
                value: "SELECT     tsa.Title        AS [Billing Period Name],     tsa.DeadlineDate AS [Contribution Deadlines] FROM     TSABillingPeriods tsa");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 262,
                column: "AnswerSQL",
                value: "SELECT CASE WHEN SUM(SendEmailImmediately) > 0 THEN 'Yes, notifying data contributors by email ' ELSE 'No, not notifying data contributors by email ' END AS [Notify data contributors by email ?] FROM( SELECT     tbp.Title                AS [Billing Period Name],     CAST(tbp.SendEmailImmediately AS INT) SendEmailImmediately FROM     TSABillingPeriods tbp WHERE     tbp.SendEmailImmediately = 1) A");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 264,
                column: "AnswerSQL",
                value: "-- Total TSA Costs over Month SELECT month_year AS [By Month],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Month] FROM vwTSACostWaterfall tcw GROUP BY date_,          month_year   -- Total TSA Costs over Quarter SELECT [quarter] AS [By Quarter],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Quarter] FROM vwTSACostWaterfall tcwf GROUP BY Concat(Year(date_), Datepart(quarter, date_)),          [quarter]   -- Total TSA Costs over Year SELECT [year] AS [By Year],        ROUND((SUM(fte_cost_per_month) + SUM(nonfte_cost_per_month)), 2) AS [Total TSA Costs Over Year] FROM vwTSACostWaterfall vtcwf GROUP BY [year]");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 267,
                column: "AnswerSQL",
                value: "SELECT tcwf.year  AS [By Year], ROUND(SUM(tcwf.fte_cost_per_month), 2)    AS [TSA FTE Cost for Selected Year],     ROUND(SUM(tcwf.nonfte_cost_per_month), 2) AS [TSA Non-FTE Cost for Selected Year] FROM     vwTSACostWaterfall tcwf WHERE      tcwf.year = '{Year}' GROUP BY     tcwf.year");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 268,
                column: "AnswerSQL",
                value: "SELECT     tp.Title AS [TSA Stages] FROM     TSAPhases tp ORDER BY     Ordinal");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 270,
                column: "AnswerSQL",
                value: "SELECT     ti.Title AS [TSAs that have duration for more than 18 months] FROM     TSAItems ti WHERE     ti.Duration > 18");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 272,
                column: "AnswerSQL",
                value: "SELECT     f.title AS [Functions],     SUM((t.TSAItemFTECostForServiceDurationUSD + t.MarkupOnFTECostUSD + t.ExternalMaterialCostUSD) )AS [TSA Costs] FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     LEFT OUTER JOIN         Functions   f             ON t.FunctionId = f.id WHERE     s.title <> 'Canceled'     AND t.Duration IS NOT NULL GROUP BY  f.title");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 279,
                column: "AnswerSQL",
                value: "SELECT COUNT(TSA.ID) AS [TSA Items in Alignment phase for IT] FROM TSAItems TSA     LEFT JOIN TSAPhases Stg         ON TSA.PhaseId = Stg.ID     LEFT JOIN Functions F         ON TSA.FunctionId = F.ID WHERE Stg.[Key] = 'ALIGNMENT'       AND F.Title = '{FunctionName}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 296,
                column: "AnswerSQL",
                value: "SELECT    t.Title AS [List all TSAs which are not getting settled in USD]    FROM     TSAItems        AS t     LEFT OUTER JOIN         TSAStatuses s             ON t.tsaitemtsastatusid = s.id     	LEFT OUTER JOIN 	    TSACurrencies TSACurr  		    ON t.TSAItemLocalCurrencyId=TSACurr.ID 	LEFT OUTER JOIN Currencies Curr 	        ON TSACurr.CurrencyID=Curr.ID WHERE         Curr.Title<>'USD ($)'  	  AND s.title <> 'Canceled'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 310,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 319,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 321,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 322,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 323,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 332,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 333,
                column: "AnswerSQL",
                value: null);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 336,
                column: "AnswerSQL",
                value: null);
        }
    }
}
