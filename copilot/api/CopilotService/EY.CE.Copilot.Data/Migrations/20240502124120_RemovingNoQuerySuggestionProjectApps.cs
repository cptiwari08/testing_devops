using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingNoQuerySuggestionProjectApps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 160);

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
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = ${Useremail}; ", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title; ", "CE4-OM", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'; ", "CE4-OM", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE'; ", "CE4-OM", "How many systems are tagged to Day 1 processes?" });

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
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID; ", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "CE4-PL", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title; ", "CE4-PL", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "CE4-PL", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title; ", "CE4-PL", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do you hide a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do you rename a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How to export report to PDF?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Provide case studies/credentials for similar deals that EY have supported in the past" });

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
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "Can I import an existing Operating Model in PowerPoint to use as my current state model?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a Process Group?" });

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
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the most common types of TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "How to configure TSA stages?" });

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
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "When was the deal announced?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best strategies for improving a company in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are recent examples of improvements being made in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best ways to track actuals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What should be the frequency of tracking dollar values during the engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What is a normative operating model for the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What are key considerations when defining an operating model for a ${Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "internet", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provides templates of TSA" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What should be the typical duration for TSA" });

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
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the best practices to build workplan, and track dependencies?" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "How much value have we already achieved from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = ${Useremail} GROUP BY S.Title; ", "How are my initiatives doing?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = @UserEmail; ", "Are there any risks or issues with initiatives that I’m responsible for?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "Which initiatives have fallen off track?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "DECLARE @Y INT; DECLARE @ColumnName NVARCHAR(50); DECLARE @SqlQuery NVARCHAR(MAX); DECLARE @TotalEstimate DECIMAL(18,2);  SELECT @Y = TRY_CAST(JSON_VALUE([Value], '$[0].defaultValue.Id') AS INT) FROM MetastoreGeneralSettings WHERE [Key] = 'VC_PROJECT_TIMEFRAME_CADENCE';  SET @ColumnName = 'Y' + CAST(@Y AS NVARCHAR(10)) + 'M12Estimate';  SET @SqlQuery = '     SELECT @TotalEstimate = (SUM(ISNULL (' + QUOTENAME(@ColumnName) + ', 0))*12)     FROM ValueCaptureEstimates VCE      JOIN ValueCaptureInitiatives VCI ON VCE.ValueCaptureInitiativeId = VCI.ID      WHERE ISNULL(VCE.Recurring,0) = 1      AND VCI.IsItemActive=1     AND ProjectTeamID IN (SELECT ID FROM ProjectTeams WHERE ManageValueCapture = 1 AND TeamTypeID = 1)     AND ValueCaptureValueTypeId = 1';  EXEC sp_executesql @SqlQuery, N'@TotalEstimate DECIMAL(18,2) OUTPUT', @TotalEstimate OUTPUT;  WITH SubtractedValue AS (    SELECT 'Total Top Down Target' AS KPI,    FORMAT((@TotalEstimate - (SUM(HeadcountCostReductionEst)+SUM(NonHeadcountCostReductionEst)+SUM(RevenueGrowthEstimate)))/1000000,'N2') AS 'Value (Million)'     FROM ValueCaptureTopDownEstimates ) SELECT * FROM SubtractedValue; ", "Are our initiative projections exceeding our targets?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "How will these initiatives impact the PnL?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = ${Useremail} ", "CE4-VC", "How  many initiatives are assigned to me? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = ${Useremail}; ", "CE4-VC", "Give my cost reduction initiatives " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title; ", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'; ", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title  FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE'; ", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "What team has the most Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "What team has the most Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "What are some of the processes that have High impact Risk/Issue linked?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets  are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title  FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "CE4-OM", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(Title) AS TotalCount, BusinessEntityId      FROM Nodes      WHERE NodeTypeId = 3      GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID; ", "CE4-OM", "Can you provide me number of processes by op model? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID; ", "CE4-OM", "How many processes we have in current state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM (     SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId      FROM Nodes N  LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId     WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE'     GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID; ", "CE4-OM", "How many processes we have in Day1 state? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "project-data", "Show me the count of TSAs by region." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "project-data", "How many of the TSAs are behind schedule?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "project-data", "List out TSAs that have high impact risks or issues associated with them." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "project-data", "Show me the breakdown of TSAs by duration" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Provider " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Receiver " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "project-data", "Can you provide me list of TSAs by phases? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "project-data", "Show me the breakdown of TSAs by duration  " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "project-data", "How many TSAs does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "What team has the most Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "What team has the most Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "What are some of the processes that have High impact Risk/Issue linked?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "CE4-PL", "project-data", "What are the project teams that make up the governance structure for this engagement? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title; ", "CE4-PL", "project-data", "How many milestones does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "CE4-PL", "project-data", "How many interdependencies does each team have " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title; ", "CE4-PL", "project-data", "How many risks does each team have? " });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do you hide a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do you rename a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How to export report to PDF?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "Can I import an existing Operating Model in PowerPoint to use as my current state model?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What is a Process Group?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "How to view side by side view of current State/Day 1/Future state?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What are the most common types of TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "How to configure TSA stages?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "How to set alerts/send email to TSA owners" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "How to export report to PPT?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 137,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "When was the deal announced?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best revenue growth initiatives?" });

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 142, null, "CE4-VC", null, "System", "internet", "What are the best strategies for improving a company in the ${Sector} sector?", null, "System" },
                    { 143, null, "CE4-VC", null, "System", "internet", "What are recent examples of improvements being made in the ${Sector} sector?", null, "System" },
                    { 144, null, "CE4-VC", null, "System", "internet", "What are the best ways to track actuals?", null, "System" },
                    { 145, null, "CE4-VC", null, "System", "internet", "What should be the frequency of tracking dollar values during the engagement?", null, "System" },
                    { 146, null, "CE4-VC", null, "System", "internet", "What are typical implications for cross border deals?", null, "System" },
                    { 147, null, "CE4-OM", null, "System", "internet", "What is a normative operating model for the ${Sector} sector?", null, "System" },
                    { 148, null, "CE4-OM", null, "System", "internet", "What are key considerations when defining an operating model for a ${Sector} sector company?", null, "System" },
                    { 149, null, "CE4-OM", null, "System", "internet", "What are examples of Day 1 process dispositions?", null, "System" },
                    { 150, null, "CE4-TSA", null, "System", "internet", "What are the corporate functions typically involved in the ${Sector} sector?", null, "System" },
                    { 151, null, "CE4-TSA", null, "System", "internet", "What are the typical services of the Sales and Marketing function?", null, "System" },
                    { 152, null, "CE4-TSA", null, "System", "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.", null, "System" },
                    { 153, null, "CE4-TSA", null, "System", "internet", "Provides templates of TSA", null, "System" },
                    { 154, null, "CE4-TSA", null, "System", "internet", "What should be the typical duration for TSA", null, "System" },
                    { 155, null, "CE4-TSA", null, "System", "internet", "Things I should keep in mind for longer duration TSAs", null, "System" },
                    { 156, null, "CE4-PL", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System" },
                    { 157, null, "CE4-PL", null, "System", "internet", "What are the key risks for a ${ProjectType} project?", null, "System" },
                    { 158, null, "CE4-PL", null, "System", "internet", "What are the key milestones for a ${ProjectType} project?", null, "System" },
                    { 159, null, "CE4-PL", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System" },
                    { 160, null, "CE4-PL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System" }
                });
        }
    }
}
