using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSemiColon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 141);

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 31,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(*) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT wp.Title  from WorkPlan wp  LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone'", "List out behind schedule milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title  from RisksAndIssues RI  LEFT JOIN Statuses s on RI.ItemStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk'", "List out behind schedule risks" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.Title  from Interdependencies I  LEFT JOIN Statuses s on I.InterdependencyStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'", "List out behind schedule interdependencies" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "How many of my milestones are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "How many of my risks are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      YEAR(I.ItemDueDate) = YEAR(GETDATE())     AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))", "How many interdependencies are due this week" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueMilestoneCount FROM      WorkPlan wp INNER JOIN      ProjectTeams pt ON wp.ProjectTeamId = pt.ID LEFT JOIN      statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE      wp.WorkPlanTaskType = 'Milestone'     AND wp.TaskDueDate < GETDATE()     AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY      OverdueMilestoneCount DESC", "Which project team has the most overdue milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY ", "Which project team has the most overdue risks?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamTitle,     COUNT(*) AS OverdueDependencyCount FROM      Interdependencies I INNER JOIN      ProjectTeams ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      I.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      ReceiverTeam.Title ORDER BY      OverdueDependencyCount DESC", "Which project team has the most overdue interdependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      RiskImpact,     RiskProbability,     COUNT(*) AS RiskCount FROM      RisksAndIssues WHERE      IssueRiskCategory = 'Risk'     AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))  AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability", "Show me open risks broken down by impact and probability" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "CE4-VC", "What are our targets for this engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)/1000000 AS ValuetoAchieve FROM ValueCaptureEstimates VCE JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID WHERE ISNULL(VCE.Recurring,0) = 1 AND VCI.IsItemActive=1 AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1) AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery", "How much value are we planning to achieve from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = ${Useremail} GROUP BY S.Title ", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = @UserEmail ", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX); DECLARE @TotalEstimate DECIMAL(18,2);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT @TotalEstimate = (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)     FROM ValueCaptureEstimates VCE      JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID      WHERE ISNULL(VCE.Recurring,0) = 1      AND VCI.IsItemActive=1     AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1)     AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery, N'@TotalEstimate DECIMAL(18,2) OUTPUT', @TotalEstimate OUTPUT;  WITH SubtractedValue AS (    SELECT 'Total Top Down Target' AS KPI,    FORMAT((@TotalEstimate - (SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate)))/1000000,'N2') AS 'Value (Million)'     FROM ValueCaptureTopDownEstimates ) SELECT * FROM SubtractedValue ", "Are our initiative projections exceeding our targets?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = ${Useremail} ", "How  many initiatives are assigned to me? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = ${Useremail} ", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "How many assets  are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(Title) AS TotalCount, BusinessEntityId      FROM Nodes      WHERE NodeTypeId = 3      GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "Can you provide me number of processes by op model? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in current state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                column: "SuggestionText",
                value: "What is the difference between Progress and Calculated Status?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                column: "SuggestionText",
                value: "How do you hide a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                column: "SuggestionText",
                value: "How do you rename a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                column: "SuggestionText",
                value: "How to export report to PDF?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                column: "SuggestionText",
                value: "How do I add a client user to the PMO app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this ${ProjectType}");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "SuggestionText",
                value: "What revenue growth levers are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "SuggestionText",
                value: "What are typical one-time costs that we should be considering?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                column: "SuggestionText",
                value: "What are the IT cost savings levers that I should be thinking about?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                column: "SuggestionText",
                value: "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                column: "SuggestionText",
                value: "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "Help understand VC methodology.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "How can I upload systems in bulk to the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "What are the steps to setup the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "Can I import an existing Operating Model in PowerPoint to use as my current state model?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                column: "SuggestionText",
                value: "What reports are available in the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "SuggestionText",
                value: "What is a Process Group?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                column: "SuggestionText",
                value: "How to view side by side view of current State/Day 1/Future state?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "SuggestionText",
                value: "What are examples for TSAs?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                column: "SuggestionText",
                value: "Why are TSAs important?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                column: "SuggestionText",
                value: "What are the most common types of TSAs?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "SuggestionText",
                value: "How to configure TSA stages?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "SuggestionText",
                value: "How to set alerts/send email to TSA owners");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this ${ProjectType}");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                column: "SuggestionText",
                value: "When was the deal announced?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 124,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 125,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 126,
                column: "SuggestionText",
                value: "What are typical implications for cross border deals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a normative operating model for the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 128,
                column: "SuggestionText",
                value: "What are key considerations when defining an operating model for a ${Sector} sector company?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 129,
                column: "SuggestionText",
                value: "What are examples of Day 1 process dispositions?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 131,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 132,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 133,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 134,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 135,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 137,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 138,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 139,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 140,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 31,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title;");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "AnswerSQL",
                value: "SELECT pt.Title, COUNT(*) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title;");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "select wp.Title, ri.Title from WorkPlansToRisksAndIssuesForAssociatedRisksAndIssues WTRI left join workplan wp on wp.ID = WTRI.WorkPlanId left join RisksAndIssues ri on ri.ID = WTRI.RisksAndIssueId", "Which risks/issues have the most impact on the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT wp.Title  from WorkPlan wp  LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone'", "List out behind schedule milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title  from RisksAndIssues RI  LEFT JOIN Statuses s on RI.ItemStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk'", "List out behind schedule risks" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.Title  from Interdependencies I  LEFT JOIN Statuses s on I.InterdependencyStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'", "List out behind schedule interdependencies" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "How many of my milestones are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "How many of my risks are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      YEAR(I.ItemDueDate) = YEAR(GETDATE())     AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'));", "How many interdependencies are due this week" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueMilestoneCount FROM      WorkPlan wp INNER JOIN      ProjectTeams pt ON wp.ProjectTeamId = pt.ID LEFT JOIN      statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE      wp.WorkPlanTaskType = 'Milestone'     AND wp.TaskDueDate < GETDATE()     AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY      OverdueMilestoneCount DESC", "Which project team has the most overdue milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY ", "Which project team has the most overdue risks?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamTitle,     COUNT(*) AS OverdueDependencyCount FROM      Interdependencies I INNER JOIN      ProjectTeams ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      I.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      ReceiverTeam.Title ORDER BY      OverdueDependencyCount DESC;", "Which project team has the most overdue interdependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      RiskImpact,     RiskProbability,     COUNT(*) AS RiskCount FROM      RisksAndIssues WHERE      IssueRiskCategory = 'Risk'     AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))  AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability;", "CE4-PMO", "Show me open risks broken down by impact and probability" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "What are our targets for this engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)/1000000 AS ValuetoAchieve FROM ValueCaptureEstimates VCE JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID WHERE ISNULL(VCE.Recurring,0) = 1 AND VCI.IsItemActive=1 AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1) AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery; ", "How much value are we planning to achieve from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = ${Useremail} GROUP BY S.Title; ", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = @UserEmail; ", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX); DECLARE @TotalEstimate DECIMAL(18,2);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT @TotalEstimate = (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)     FROM ValueCaptureEstimates VCE      JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID      WHERE ISNULL(VCE.Recurring,0) = 1      AND VCI.IsItemActive=1     AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1)     AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery, N'@TotalEstimate DECIMAL(18,2) OUTPUT', @TotalEstimate OUTPUT;  WITH SubtractedValue AS (    SELECT 'Total Top Down Target' AS KPI,    FORMAT((@TotalEstimate - (SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate)))/1000000,'N2') AS 'Value (Million)'     FROM ValueCaptureTopDownEstimates ) SELECT * FROM SubtractedValue; ", "Are our initiative projections exceeding our targets?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = ${Useremail} ", "How  many initiatives are assigned to me? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = ${Useremail}; ", "CE4-VC", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title; ", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'; ", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE'; ", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "How many assets  are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(Title) AS TotalCount, BusinessEntityId      FROM Nodes      WHERE NodeTypeId = 3      GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID; ", "Can you provide me number of processes by op model? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID; ", "How many processes we have in current state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID; ", "CE4-OM", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title; ", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title; ", "PROJECT_LEVEL", "project-data", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                column: "SuggestionText",
                value: "What PMO workplan templates are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                column: "SuggestionText",
                value: "What is the difference between Progress and Calculated Status?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                column: "SuggestionText",
                value: "How do you hide a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                column: "SuggestionText",
                value: "How do you rename a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                column: "SuggestionText",
                value: "How to export report to PDF?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                column: "SuggestionText",
                value: "How do I add a client user to the PMO app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "SuggestionText",
                value: "What cost savings levers are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "SuggestionText",
                value: "What revenue growth levers are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                column: "SuggestionText",
                value: "What are typical one-time costs that we should be considering?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                column: "SuggestionText",
                value: "What are the IT cost savings levers that I should be thinking about?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                column: "SuggestionText",
                value: "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                column: "SuggestionText",
                value: "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "What normative operating models are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "How can I upload systems in bulk to the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "What are the steps to setup the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                column: "SuggestionText",
                value: "Can I import an existing Operating Model in PowerPoint to use as my current state model?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "SuggestionText",
                value: "What reports are available in the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                column: "SuggestionText",
                value: "What is a Process Group?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How to view side by side view of current State/Day 1/Future state?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "SuggestionText",
                value: "What TSAs would you suggest?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                column: "SuggestionText",
                value: "What are examples for TSAs?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                column: "SuggestionText",
                value: "Why are TSAs important?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                column: "SuggestionText",
                value: "What are the most common types of TSAs?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "SuggestionText",
                value: "How to configure TSA stages?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "How to set alerts/send email to TSA owners" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                column: "SuggestionText",
                value: "What PMO workplan templates are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                column: "SuggestionText",
                value: "When was the deal announced?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                column: "SuggestionText",
                value: "What are the best cost saving initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 124,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 125,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 126,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 128,
                column: "SuggestionText",
                value: "What is a normative operating model for the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 129,
                column: "SuggestionText",
                value: "What are key considerations when defining an operating model for a ${Sector} sector company?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 131,
                column: "SuggestionText",
                value: "What are the corporate functions typically involved in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 132,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 133,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 134,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 135,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Things I should keep in mind for longer duration TSAs" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 137,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 138,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 139,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 140,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 141, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System" });
        }
    }
}
