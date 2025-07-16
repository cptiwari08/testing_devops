using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingProjectDocumentSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 140);

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title", "project-data", "How many milestones does each team have?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(*) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title", "project-data", "How many risks does each team have?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(*) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title", "project-data", "How many interdependencies does each team have?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT wp.Title  from WorkPlan wp  LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone'", "project-data", "List out behind schedule milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT RI.Title  from RisksAndIssues RI  LEFT JOIN Statuses s on RI.ItemStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk'", "project-data", "List out behind schedule risks" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT I.Title  from Interdependencies I  LEFT JOIN Statuses s on I.InterdependencyStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'", "project-data", "List out behind schedule interdependencies" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "CE4-PMO", "project-data", "How many of my milestones are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "CE4-PMO", "project-data", "How many of my risks are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      YEAR(I.ItemDueDate) = YEAR(GETDATE())     AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))", "CE4-PMO", "project-data", "How many interdependencies are due this week" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT     pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueMilestoneCount FROM      WorkPlan wp INNER JOIN      ProjectTeams pt ON wp.ProjectTeamId = pt.ID LEFT JOIN      statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE      wp.WorkPlanTaskType = 'Milestone'     AND wp.TaskDueDate < GETDATE()     AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY      OverdueMilestoneCount DESC", "CE4-PMO", "project-data", "Which project team has the most overdue milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc", "CE4-PMO", "project-data", "Which project team has the most overdue risks?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamTitle,     COUNT(*) AS OverdueDependencyCount FROM      Interdependencies I INNER JOIN      ProjectTeams ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      I.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      ReceiverTeam.Title ORDER BY      OverdueDependencyCount DESC", "CE4-PMO", "project-data", "Which project team has the most overdue interdependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      RiskImpact,     RiskProbability,     COUNT(*) AS RiskCount FROM      RisksAndIssues WHERE      IssueRiskCategory = 'Risk'     AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))  AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability", "CE4-PMO", "project-data", "Show me open risks broken down by impact and probability" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "CE4-VC", "project-data", "What are our targets for this engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)/1000000 AS ValuetoAchieve FROM ValueCaptureEstimates VCE JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID WHERE ISNULL(VCE.Recurring,0) = 1 AND VCI.IsItemActive=1 AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1) AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery", "CE4-VC", "project-data", "How much value are we planning to achieve from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '${Username}' GROUP BY S.Title ", "CE4-VC", "project-data", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '${Username}' ", "CE4-VC", "project-data", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX); DECLARE @TotalEstimate DECIMAL(18,2);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT @TotalEstimate = (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)     FROM ValueCaptureEstimates VCE      JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID      WHERE ISNULL(VCE.Recurring,0) = 1      AND VCI.IsItemActive=1     AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1)     AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery, N'@TotalEstimate DECIMAL(18,2) OUTPUT', @TotalEstimate OUTPUT;  WITH SubtractedValue AS (    SELECT 'Total Top Down Target' AS KPI,    FORMAT((@TotalEstimate - (SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate)))/1000000,'N2') AS 'Value (Million)'     FROM ValueCaptureTopDownEstimates ) SELECT * FROM SubtractedValue ", "CE4-VC", "project-data", "Are our initiative projections exceeding our targets?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '${Username}' ", "CE4-VC", "project-data", "How  many initiatives are assigned to me? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '${Username}' ", "CE4-VC", "project-data", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "project-data", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "project-data", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "CE4-OM", "project-data", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "CE4-OM", "project-data", "How many assets  are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "CE4-OM", "project-data", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(Title) AS TotalCount, BusinessEntityId      FROM Nodes      WHERE NodeTypeId = 3      GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "project-data", "Can you provide me number of processes by op model? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "project-data", "How many processes we have in current state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "project-data", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "project-data", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "project-data", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do you hide a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do you rename a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How to export report to PDF?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "Can I import an existing Operating Model in PowerPoint to use as my current state model?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "What is a Process Group?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "ey-guidance", "How to view side by side view of current State/Day 1/Future state?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "ey-guidance", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the most common types of TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "How to configure TSA stages?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "How to set alerts/send email to TSA owners" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "AppAffinity",
                value: "PROJECT_LEVEL");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "When was the deal announced?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best strategies for improving a company in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are recent examples of improvements being made in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best ways to track actuals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What should be the frequency of tracking dollar values during the engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What is a normative operating model for the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are key considerations when defining an operating model for a ${Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "Provides templates of TSA" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What should be the typical duration for TSA" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "Things I should keep in mind for longer duration TSAs" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "internet", "What are the best practices to build workplan, and track dependencies?" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "project-docs", "What is the governance structure for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "project-docs", "List out various recurring meetings along with cadence" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "project-docs", "What are key dates for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "project-docs", "What do we know about the target and buyer for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "project-docs", "What is EY’s scope of work?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { null, "project-docs", "Who is the program sponsor for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "project-docs", "What is the governance structure for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "project-docs", "List out various recurring meetings along with cadence" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "project-docs", "What are key dates for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "project-docs", "What do we know about the target and buyer for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "project-docs", "What is EY’s scope of work ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "project-docs", "Who is the program sponsor for this project ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "project-docs", "What is the governance structure for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "project-docs", "List out various recurring meetings along with cadence" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "project-docs", "What are key dates for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "project-docs", "What do we know about the target and buyer for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "project-docs", "What is EY’s scope of work ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "project-docs", "Who is the program sponsor for this project ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "project-docs", "What is the governance structure for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "project-docs", "List out various recurring meetings along with cadence" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "project-docs", "What are key dates for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "project-docs", "What do we know about the target and buyer for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "project-docs", "What is EY’s scope of work ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "project-docs", "Who is the program sponsor for this project ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "project-docs", "What is the governance structure for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "project-docs", "List out various recurring meetings along with cadence" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "project-docs", "What are key dates for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "project-docs", "What do we know about the target and buyer for this project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "project-docs", "What is EY’s scope of work ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "PROJECT_LEVEL", "project-docs", "Who is the program sponsor for this project ?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title", "CE4-PMO", "How many milestones does each team have?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(*) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title", "CE4-PMO", "How many risks does each team have?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(*) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title", "CE4-PMO", "How many interdependencies does each team have?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT wp.Title  from WorkPlan wp  LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone'", "CE4-PMO", "List out behind schedule milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT RI.Title  from RisksAndIssues RI  LEFT JOIN Statuses s on RI.ItemStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk'", "CE4-PMO", "List out behind schedule risks" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT I.Title  from Interdependencies I  LEFT JOIN Statuses s on I.InterdependencyStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'", "CE4-PMO", "List out behind schedule interdependencies" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "CE4-PMO", "How many of my milestones are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'", "CE4-PMO", "How many of my risks are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      YEAR(I.ItemDueDate) = YEAR(GETDATE())     AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))", "CE4-PMO", "How many interdependencies are due this week" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT     pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueMilestoneCount FROM      WorkPlan wp INNER JOIN      ProjectTeams pt ON wp.ProjectTeamId = pt.ID LEFT JOIN      statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE      wp.WorkPlanTaskType = 'Milestone'     AND wp.TaskDueDate < GETDATE()     AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY      OverdueMilestoneCount DESC", "project-data", "Which project team has the most overdue milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc", "project-data", "Which project team has the most overdue risks?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamTitle,     COUNT(*) AS OverdueDependencyCount FROM      Interdependencies I INNER JOIN      ProjectTeams ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      I.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      ReceiverTeam.Title ORDER BY      OverdueDependencyCount DESC", "project-data", "Which project team has the most overdue interdependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "Source", "SuggestionText" },
                values: new object[] { "SELECT      RiskImpact,     RiskProbability,     COUNT(*) AS RiskCount FROM      RisksAndIssues WHERE      IssueRiskCategory = 'Risk'     AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))  AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability", "project-data", "Show me open risks broken down by impact and probability" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT 'Total Top Down Target' AS KPI, FORMAT((SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate))/1000000,'N2') AS 'Value (Million)' FROM ValueCaptureTopDownEstimates ", "CE4-VC", "project-data", "What are our targets for this engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)/1000000 AS ValuetoAchieve FROM ValueCaptureEstimates VCE JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID WHERE ISNULL(VCE.Recurring,0) = 1 AND VCI.IsItemActive=1 AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1) AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery", "CE4-VC", "project-data", "How much value are we planning to achieve from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = '${Username}' GROUP BY S.Title ", "CE4-VC", "project-data", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = '${Username}' ", "CE4-VC", "project-data", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX); DECLARE @TotalEstimate DECIMAL(18,2);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT @TotalEstimate = (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)     FROM ValueCaptureEstimates VCE      JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID      WHERE ISNULL(VCE.Recurring,0) = 1      AND VCI.IsItemActive=1     AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1)     AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery, N'@TotalEstimate DECIMAL(18,2) OUTPUT', @TotalEstimate OUTPUT;  WITH SubtractedValue AS (    SELECT 'Total Top Down Target' AS KPI,    FORMAT((@TotalEstimate - (SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate)))/1000000,'N2') AS 'Value (Million)'     FROM ValueCaptureTopDownEstimates ) SELECT * FROM SubtractedValue ", "CE4-VC", "project-data", "Are our initiative projections exceeding our targets?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = '${Username}' ", "CE4-VC", "project-data", "How  many initiatives are assigned to me? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = '${Username}' ", "CE4-VC", "project-data", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "project-data", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "project-data", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "CE4-OM", "project-data", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "CE4-OM", "project-data", "How many assets  are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "CE4-OM", "project-data", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(Title) AS TotalCount, BusinessEntityId      FROM Nodes      WHERE NodeTypeId = 3      GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "project-data", "Can you provide me number of processes by op model? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "project-data", "How many processes we have in current state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "project-data", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "project-data", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "project-data", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "project-data", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "project-data", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "project-data", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "project-data", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "project-data", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "project-data", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "project-data", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do you hide a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do you rename a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How to export report to PDF?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "AppAffinity",
                value: "CE4-PMO");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "Can I import an existing Operating Model in PowerPoint to use as my current state model?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What is a Process Group?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "How to view side by side view of current State/Day 1/Future state?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What are the most common types of TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "How to configure TSA stages?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "How to set alerts/send email to TSA owners" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "Source", "SuggestionText" },
                values: new object[] { "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 111, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "How do I link a Workplan Task to RAID?", null, "System" },
                    { 112, null, "PROJECT_LEVEL", null, "System", "ey-guidance", "Help me understand PMO methodology for this ${ProjectType}", null, "System" },
                    { 113, null, "CE4-PMO", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System" },
                    { 114, null, "CE4-PMO", null, "System", "internet", "What are the key risks for a ${ProjectType} project?", null, "System" },
                    { 115, null, "CE4-PMO", null, "System", "internet", "What are the key milestones for a ${ProjectType} project?", null, "System" },
                    { 116, null, "CE4-PMO", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System" },
                    { 117, null, "CE4-PMO", null, "System", "internet", "When was the deal announced?", null, "System" },
                    { 118, null, "CE4-PMO", null, "System", "internet", "What are some of the similar deals that have happened in the past?", null, "System" },
                    { 119, null, "CE4-PMO", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System" },
                    { 120, null, "CE4-VC", null, "System", "internet", "What are the best cost saving initiatives?", null, "System" },
                    { 121, null, "CE4-VC", null, "System", "internet", "What are the best revenue growth initiatives?", null, "System" },
                    { 122, null, "CE4-VC", null, "System", "internet", "What are the best strategies for improving a company in the ${Sector} sector?", null, "System" },
                    { 123, null, "CE4-VC", null, "System", "internet", "What are recent examples of improvements being made in the ${Sector} sector?", null, "System" },
                    { 124, null, "CE4-VC", null, "System", "internet", "What are the best ways to track actuals?", null, "System" },
                    { 125, null, "CE4-VC", null, "System", "internet", "What should be the frequency of tracking dollar values during the engagement?", null, "System" },
                    { 126, null, "CE4-VC", null, "System", "internet", "What are typical implications for cross border deals?", null, "System" },
                    { 127, null, "CE4-OM", null, "System", "internet", "What is a normative operating model for the ${Sector} sector?", null, "System" },
                    { 128, null, "CE4-OM", null, "System", "internet", "What are key considerations when defining an operating model for a ${Sector} sector company?", null, "System" },
                    { 129, null, "CE4-OM", null, "System", "internet", "What are examples of Day 1 process dispositions?", null, "System" },
                    { 130, null, "CE4-TSA", null, "System", "internet", "What are the corporate functions typically involved in the ${Sector} sector?", null, "System" },
                    { 131, null, "CE4-TSA", null, "System", "internet", "What are the typical services of the Sales and Marketing function?", null, "System" },
                    { 132, null, "CE4-TSA", null, "System", "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.", null, "System" },
                    { 133, null, "CE4-TSA", null, "System", "internet", "Provides templates of TSA", null, "System" },
                    { 134, null, "CE4-TSA", null, "System", "internet", "What should be the typical duration for TSA", null, "System" },
                    { 135, null, "CE4-TSA", null, "System", "internet", "Things I should keep in mind for longer duration TSAs", null, "System" },
                    { 136, null, "PROJECT_LEVEL", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System" },
                    { 137, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key risks for a ${ProjectType} project?", null, "System" },
                    { 138, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key milestones for a ${ProjectType} project?", null, "System" },
                    { 139, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System" },
                    { 140, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System" }
                });
        }
    }
}
