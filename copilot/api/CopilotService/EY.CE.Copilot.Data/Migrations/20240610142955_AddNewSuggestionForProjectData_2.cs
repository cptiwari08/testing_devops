using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSuggestionForProjectData_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT Amount as [Client Baseline PnL Amount], A.FinancialLineItem as FinancialLineItem FROM( SELECT F.ID, F.Ordinal, F.Title FinancialLineItem , SUM(C.Amount)/1000000 Amount FROM ValueCaptureFinancialLineItems F LEFT JOIN CostCenters C ON C.FinancialLineItemId = F.ID  GROUP BY F.ID, F.Title, F.Ordinal ) A WHERE A.Amount IS NOT NULL", "CE4-VC", "What is my program Client Baseline PnL?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT     Title AS [Recommended Initiatives List] FROM     ValueCaptureRecommendedInitiatives", "CE4-VC", "List all recommended initiatives for my program.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT 	SUM(a.[Total Top Down Target Value]) AS [Top Down Targets],      SUM(b.[Bottom Up Initiatives Value]) AS [Bottom Up Initiatives Value], 	( SUM(b.[Bottom Up Initiatives Value])-SUM(a.[Total Top Down Target Value])) AS Variance FROM ( SELECT   SUM(ISNULL(HeadcountCostReductionEst, 0)) + SUM(ISNULL(NonHeadcountCostReductionEst, 0))        + SUM(ISNULL(RevenueGrowthEstimate, 0)) AS [Total Top Down Target Value] FROM ValueCaptureTopDownEstimates) a, ( select SUM(Amount)*12 as [Bottom Up Initiatives Value]  FROM vwUnpivotEstimates where MYear='Y3M12' AND Recurring =1 ) b ", "CE4-VC", "Compare Top Down Targets vs Bottom Up Initiatives for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "   SELECT ISNULL(SUM(Act.AMOUNT),0.0) AS [Cumulative One Time Cost Amount] 			   FROM vwUnpivotActuals Act 			   LEFT JOIN ValueCaptureImpactTypes VCIT ON Act.ValueCaptureImpactTypeID=VCIT.ID 			   LEFT JOIN ValueCaptureInitiatives VCI ON Act.ValueCaptureInitiativeID=VCI.ID 			   WHERE VCIT.PositiveOrNegativeValues='Negative' 			   AND Act.Recurring<>1 			   AND VCI.Title = ${ValueCaptureInitiative}", "CE4-VC", "What is cumulative one-time cost for all initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "  SELECT  	   WP.Title AS [Workplan Items Linked to Value Capture Initiatives]  	   FROM WorkPlan WP 	   LEFT JOIN WorkPlanToValueCaptureInitiativesForValueCaptureInitiativeIDs WPVC  	             ON WP.ID=WPVC.WorkPlanId 	   LEFT JOIN ValueCaptureInitiatives VCI  	             ON VCI.ID=WPVC.ValueCaptureInitiativeId", "CE4-VC", "List all workplan items linked to VC initiatives." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "WITH EstimatedRunRates AS (     SELECT          E.ValueCaptureInitiativeId,  B.Title AS Initiative,          ISNULL(SUM(E.Amount),0) * 12 AS EstimatedAnnualizedRunRate     FROM          vwUnPivotEstimates E     JOIN          ValueCaptureInitiatives B ON E.ValueCaptureInitiativeId = B.ID     WHERE          E.Recurring = 1     GROUP BY          E.ValueCaptureInitiativeId, B.Title ), ActualRunRates AS (     SELECT          A.ValueCaptureInitiativeId, B.Title AS Initiative,          SUM(ISNULL(A.Amount, 0)) * 12 AS ActualAnnualizedRunRate     FROM          vwUnpivotActuals A     JOIN          ValueCaptureInitiatives B ON A.ValueCaptureInitiativeId = B.ID     WHERE          A.Recurring = 1      GROUP BY          A.ValueCaptureInitiativeId, B.Title )   SELECT Initiative AS [List of Initiatives with 10% differ between Estimate and Actual] FROM ( SELECT      E.ValueCaptureInitiativeId,E.Initiative,     E.EstimatedAnnualizedRunRate,     A.ActualAnnualizedRunRate,    IIF(E.EstimatedAnnualizedRunRate = 0 ,0 , (A.ActualAnnualizedRunRate - E.EstimatedAnnualizedRunRate) / E.EstimatedAnnualizedRunRate * 100) AS PercentageDifference FROM      EstimatedRunRates E JOIN      ActualRunRates A ON E.ValueCaptureInitiativeId = A.ValueCaptureInitiativeId ) A WHERE      PercentageDifference > 10", "CE4-VC", "List all initiatives that had an estimate annualized run-rate differed by more than 10% from the Actual annualized run-rate?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT     CONCAT('Tracking Period of the Program is from ', CAST(MIN(StartDt) AS DATE), ' to ', CAST(MAX(EndDt) AS DATE)) FROM     ValueCaptureTransactionMonths WHERE     IsItemActive = 1", "CE4-VC", "What's the tracking period for my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT  ISNULL(SUM(Act.Amount), 0.0) - ISNULL(SUM(Est.Amount), 0.0) AS Variance FROM (     SELECT SUM(Est.Amount) AS Amount     FROM vwUnpivotEstimates Est         LEFT JOIN ValueCaptureInitiatives VCI             ON Est.ValueCaptureInitiativeId = VCI.ID         LEFT JOIN ProjectTeams PT             ON VCI.ProjectTeamId = PT.ID     WHERE PT.Title = ${ProjectTeam}' ) Est     JOIN     (         SELECT SUM(Act.Amount) AS Amount         FROM vwUnpivotActuals Act             LEFT JOIN ValueCaptureInitiatives VCI                 ON Act.ValueCaptureInitiativeId = VCI.ID             LEFT JOIN ProjectTeams PT                 ON VCI.ProjectTeamId = PT.ID         WHERE PT.Title = ${ProjectTeam}     ) Act         ON 1 = 1", "CE4-VC", "What's the variance for Legal function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "	SELECT   	    COUNT(VCI.ID) AS [Initiative Count for Selected Evaluator Quad]  		     FROM ValueCaptureInitiatives VCI 		LEFT JOIN VCInitiativeEvaluatorQuadInfo VCQ  		    ON VCI.VCInitiativeEvaluatorQuadInfoId=VCQ.ID 		       WHERE VCQ.Title= ${EvaluatorQuad}", "CE4-VC", "Based on Evaluator comparison, how many initiatives have 'High Benefit, High Complexity'?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' ", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'DAY_ONE' AND A.Title IS NOT NULL ", "How many assets are tagged to Day 1 processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title FROM NodesToAssetsForEnablerAssets NA LEFT JOIN Nodes N ON N.ID = NA.NodesId LEFT JOIN Assets A ON A.ID = NA.AssetsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' AND A.Title IS NOT NULL ", "How many assets are tagged to Current State processes? " });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(Title) AS TotalCount, BusinessEntityId FROM Nodes WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "Can you provide me number of processes by op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "How many processes we have in Day1 state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(1) FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'  AND N.NodeTypeId = 3  AND N.ID NOT IN (   SELECT DISTINCT NTO.NodeId   FROM Nodes N1   LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null  )", "How many processes don't have an Owner assigned in Current State?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.ID 	,PR.Title FROM Nodes PR JOIN TransactionStates T ON T.ID = PR.TransactionStateId WHERE T.[Key] = 'DAY_ONE' 	AND PR.NodeTypeId = 3 	AND NOT EXISTS  				(SELECT 1 FROM [NodesToDispositionsForDispositionNew] D WHERE D.NodeId = PR.ID)", "List processes that don't have Disposition assigned in Day 1 state." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title FROM Dispositions", "List the Disposition options available." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [ProcessGroup] ,COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) [Enablers Count] FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 2 GROUP BY N.Title ORDER BY [Enablers Count] DESC", "How many enablers are associated with each Process Group?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title [Disposition] 	,COUNT(SD.SystemId) [SystemCount] FROM Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID GROUP BY D.Title", "List the total number of systems by disposition." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT PR.Title [Process] , D.DispositionId FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId  	LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 	AND D.NodeId IS NULL 	AND S.[Key] = 'DAY_ONE'", "Can you please list down all Day 1 processes where no disposition has been tagged?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT 	N.Title [Process]	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 3 GROUP BY N.Title HAVING COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) = 0", "Can you please list all processes with no Enablers?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.Title as MissingInDay1 FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'CURRENT_STATE' EXCEPT SELECT 	PR.Title [Process] FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'DAY_ONE'", "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title Enabler, 'Asset' as Category	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID LEFT JOIN [Assets] A ON A.ID = NA.AssetsId WHERE A.Title IS NOT NULL UNION ALL SELECT DISTINCT F.Title Enabler, 'Facility' as Category FROM Nodes N LEFT JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID LEFT JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE F.Title IS NOT NULL  UNION ALL SELECT DISTINCT S.Title Enabler , 'System' as Category FROM Nodes N LEFT JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID LEFT JOIN [Systems] S ON S.ID = NS.SystemsId WHERE S.Title IS NOT NULL  UNION ALL SELECT DISTINCT T.Title Enabler	, 'TPA' as Category FROM Nodes N LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID LEFT JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId WHERE T.Title IS NOT NULL ", "What Enablers are we tracking for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# Systems present in this functional OP model] FROM Systems", "How many Systems are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements", "How many TPAs are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ST.Title AS SystemType, COUNT(S.ID) [# Systems by Type] FROM Systems S LEFT JOIN SystemTypes ST ON ST.ID = S.TypeId WHERE S.TypeId IS NOT NULL GROUP BY ST.Title", "List the number of Systems by Type." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT O.Title AS Owners, Count(*) AS [# TPAs by Ownership] FROM ThirdPartyAgreements TP LEFT JOIN Owners O ON O.ID = TP.OwnerID WHERE TP.OwnerID IS NOT NULL GROUP BY O.Title", "List the number of TPAs by Ownership." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 148,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Title [ProcessGroups With No Process] FROM Nodes WHERE nodetypeid = 2 AND ID NOT IN (SELECT NodeParentId FROM Nodes)", "CE4-OM", "List down all process groups with no process within them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT COUNT(P.[ID]) AS [Number of Process across Op Model] FROM Nodes P     LEFT JOIN NodeTypes NType         ON P.NodeTypeId = NType.ID WHERE NType.Title = 'Process'", "CE4-OM", "List the number of processes across op models." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Modified >= DATEADD(hh, -1, GETDATE())", "CE4-OM", "List history of changes to the op model in the past one hour." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE FieldImpacted = 'Owner'       AND CAST(Modified AS DATE) = CAST(GETDATE() AS DATE)", "CE4-OM", "List the history of changes to Ownership in the op model today." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Title = 'Deleted'       AND DATEPART(wk, CAST(Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(Modified) = YEAR(GETDATE())", "CE4-OM", "List the history of deletes in the op model this week." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT T.Title [State], N.Title AS OperatingModel FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId  WHERE N.NodeTypeId = 1", "CE4-OM", "List the functional operating models across different states." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT N.Title Opmodel, L.Title LiveNote, Note FROM OperatingModelLiveNotes L LEFT JOIN Nodes N ON N.Id = L.NodeId", "CE4-OM", "Are there any Live Notes in this op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process FROM OperatingModelActivityLog OL  LEFT JOIN Nodes N ON N.ID = Ol.ObjectID WHERE OL.Title = 'Updated' AND Object = 'Process' AND (CAST(OL.Modified AS DATE) BETWEEN DATEADD(DAY, -7, CAST(GETDATE()AS DATE))  AND CAST(GETDATE() AS DATE))", "CE4-OM", "List down all processes which were updated since last 1 week?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process, OldValue, NewValue  FROM OperatingModelActivityLog PL  LEFT JOIN Nodes N ON N.ID = PL.ObjectID  WHERE Object = 'Process' AND PL.Title = 'Updated' AND FieldImpacted = 'Title' AND N.TransactionStateId = 6", "CE4-OM", "List down all processes which have been renamed in Day 1 op model." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT TOP 5     S.Title AS [Systems],     Count(NSS.NodesID) AS [ProcessCount] FROM Nodes N     LEFT JOIN [dbo].[NodesToSystemsForEnablerSystems] NSS         ON N.ID = NSS.NodesId     LEFT JOIN [dbo].[Systems] S         ON NSS.SystemsId = S.ID WHERE N.NodeTypeId = 3 GROUP BY S.Title ORDER BY Count(NSS.NodesID) DESC", "CE4-OM", "Please provide top 5 systems that are linked to various processes", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT N.Title [List of processes for CurrentState where ownership is not present] FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'   AND N.NodeTypeId = 3   AND N.ID NOT IN (     SELECT DISTINCT NTO.NodeId     FROM Nodes N1     LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null   )", "CE4-OM", "For Current State, list all processes that doesn't have any ownership.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { " SELECT     PR.Title [Process] FROM     Nodes                                    PR     JOIN         TransactionStates                    S             ON S.ID = PR.TransactionStateId     LEFT JOIN         NodesToDispositionsForDispositionNew D             ON D.NodeId = PR.ID WHERE     PR.NodeTypeId = 3     AND D.NodeId IS NULL     AND S.[Key] = 'DAY_ONE'", "CE4-OM", "For Day 1 Operating Models, list all processes that doesn't have any dispositions." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            NodeTypeId,            HierId = 1     FROM Nodes     WHERE NodeParentID IS NULL     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            Nodes.NodeTypeId,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy ([Functions], NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.Title [Functions],            ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM GetSelectedFunction ROOT     WHERE HierId = 2     UNION ALL     SELECT PARENT.[Functions],            CHILD.NodeParentID,            CHILD.ID,            CHILD.Title,            PARENT.TITLE,            PARENT.ID,            CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    )  ,       --SELECT * FROM PG_Hierarchy         ProcessCTE AS (SELECT DISTINCT         P.Functions,         P.Title Processes,         P.ID     FROM     (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 2) SF         LEFT JOIN         (SELECT * FROM PG_Hierarchy WHERE NodeTypeId = 3) P             ON P.NodeParentID = SF.ID     WHERE P.Title IS NOT NULL    ) SELECT TOP 1     Functions AS [Function with highest number of processes] FROM (     SELECT Functions,            COUNT(ID) as [Process Count]     FROM ProcessCTE     GROUP BY Functions ) A order by [Process Count] desc    ", "CE4-OM", "Which function has the highest number of processes?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT     N.Title AS [List of Processes with Disposition as TSA] FROM   Nodes   N   LEFT JOIN     NodesToDispositionsForDispositionNew NTD             ON NTD.NodeId = N.ID     LEFT JOIN         Dispositions                         D             ON NTD.DispositionId = D.ID WHERE     D.Title = 'TSA'     AND N.NodeTypeId = 3 -- To get the list of Processes", "CE4-OM", "List down all Processes which have their disposition set as TSA." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT       N.Title as [List of Processes with No Disposition] FROM Nodes N          WHERE N.NodeTypeId=3  		  AND N.ID NOT IN  		             (SELECT NodeId FROM NodesToDispositionsForDispositionNew)", "CE4-OM", "List down all processes with no Disposition assigned." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 164,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "project-data", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 165,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Provider." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 166,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "project-data", "Give me the list of TSAs by Receiver." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 167,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "project-data", "Can you provide me list of TSAs by phases?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 168,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "project-data", "Show me the breakdown of TSAs by duration." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "project-data", "How many TSAs does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT    TST.Title as [Phase],    COUNT(TC.ID) AS [# TSAs across different phases] FROM    TSAItems TC    LEFT JOIN TSAPhases TST ON TC.PhaseID = TST.ID  Group BY    TST.Title ", "CE4-TSA", "project-data", "List the number of TSAs across different phases." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT    T.Title PendingTSAItems  FROM    TSAItems T    JOIN TSAStatuses TS ON T.TSAItemTSAStatusId = TS.ID  WHERE    T.AuthorId = ${Useremail}    AND TS.[key] = 'ACTIVE' ", "CE4-TSA", "project-data", "How many pending TSA items do I have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 172,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    Title as [Billing Period Name],    DeadlineDate as [Contribution Deadlines]  FROM    TSABillingPeriods ", "CE4-TSA", "project-data", "In Cost Tracking, how is my billing periods setup?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 173,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { " SELECT    Title AS [TSA Phases]  FROM TSAPhases", "CE4-TSA", "project-data", "What are the different TSA phases in my project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ProviderLegalEntityId = L.ID where  L.ServiceLocation is not null UNION  SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ReceiverLegalEntityId = L.ID   where  L.ServiceLocation is not null ", "CE4-TSA", "project-data", "List the TSA service locations in my project.", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 175,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT     TSA.Title AS [TSAs],     s.Title   AS [TSA Statuses] FROM     TSAItems        TSA     LEFT JOIN         TSAStatuses S             ON TSA.TSAItemTSAStatusId = S.ID     LEFT JOIN         Functions   F             ON TSA.FunctionId = F.ID WHERE     F.Title = ${FunctionName}", "CE4-TSA", "project-data", "List the TSAs for the Finance fucntion with their status." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 176,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "project-data", "What are the project teams that make up the governance structure for this engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 177,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "project-data", "How many milestones does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 178,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "project-data", "How many interdependencies does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "project-data", "How many risks does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 180,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 183,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 184,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 186,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Provide case studies/credentials for similar deals that EY has supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 190,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 191,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 192,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 193,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 194,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 195,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 196,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "ey-guidance", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 197,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 198,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 199,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 200,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 201,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 202,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 203,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 204,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 205,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-TSA", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 206,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 207,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 208,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 209,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 210,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 212,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the key risks for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 213,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the key milestones for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 215,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 217,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 218,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 219,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best strategies for improving a company in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 220,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are recent examples of improvements being made in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 221,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best ways to track actuals?" });

            migrationBuilder.InsertData(
                table: "AssistantSuggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy", "VisibleToAssistant" },
                values: new object[,]
                {
                    { 222, null, "CE4-VC", null, "System", "internet", "What should be the frequency of tracking dollar values during the engagement?", null, "System", true },
                    { 223, null, "CE4-VC", null, "System", "internet", "What are typical implications for cross border deals?", null, "System", true },
                    { 224, null, "CE4-OM", null, "System", "internet", "What is a normative operating model for the {Sector} sector?", null, "System", true },
                    { 225, null, "CE4-OM", null, "System", "internet", "What are key considerations when defining an operating model for a {Sector} sector company?", null, "System", true },
                    { 226, null, "CE4-OM", null, "System", "internet", "What are examples of Day 1 process dispositions?", null, "System", true },
                    { 227, null, "CE4-TSA", null, "System", "internet", "What are the corporate functions typically involved in the {Sector} sector?", null, "System", true },
                    { 228, null, "CE4-TSA", null, "System", "internet", "What are the typical services of the Sales and Marketing function?", null, "System", true },
                    { 229, null, "CE4-TSA", null, "System", "internet", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.", null, "System", true },
                    { 230, null, "CE4-TSA", null, "System", "internet", "Provides templates of TSA.", null, "System", true },
                    { 231, null, "CE4-TSA", null, "System", "internet", "What should be the typical duration for TSA?", null, "System", true },
                    { 232, null, "CE4-TSA", null, "System", "internet", "Things I should keep in mind for longer duration TSAs?", null, "System", true },
                    { 233, null, "PROJECT_LEVEL", null, "System", "internet", "Generate a basic workplan template for my project.", null, "System", true },
                    { 234, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key risks for a {ProjectType} project?", null, "System", true },
                    { 235, null, "PROJECT_LEVEL", null, "System", "internet", "What are the key milestones for a {ProjectType} project?", null, "System", true },
                    { 236, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System", true },
                    { 237, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 237);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT D.Title, STRING_AGG(N.Title, ', ') AS NodeTitles FROM NodesToDispositionsForDispositionNew ND LEFT JOIN Nodes N ON N.ID = ND.NodeId LEFT JOIN Dispositions D ON D.ID = ND.DispositionId LEFT JOIN TransactionStates T on T.ID = N.TransactionStateId Where T.[Key] = 'DAY_ONE' GROUP BY D.Title ", "CE4-OM", "Provide a summary of Day 1 process dispositions.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT DISTINCT S.Title FROM NodesToSystemsForEnablerSystems NS LEFT JOIN Nodes N ON N.ID = NS.NodesId LEFT JOIN Systems S ON S.ID = NS.SystemsId LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE' ", "CE4-OM", "How many systems are tagged to Current State processes?", true });

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
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(Title) AS TotalCount, BusinessEntityId FROM Nodes WHERE NodeTypeId = 3 GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "Can you provide me number of processes by op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'CURRENT_STATE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT Nodes.Title, a.TotalCount FROM ( SELECT COUNT(N.Title) AS TotalCount, BusinessEntityId FROM Nodes N LEFT JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE NodeTypeId = 3 AND T.[Key] = 'DAY_ONE' GROUP BY BusinessEntityId ) a INNER JOIN Nodes ON a.BusinessEntityId = Nodes.ID ", "CE4-OM", "How many processes we have in Day1 state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT COUNT(1) FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId WHERE T.[Key] = 'CURRENT_STATE'  AND N.NodeTypeId = 3  AND N.ID NOT IN (   SELECT DISTINCT NTO.NodeId   FROM Nodes N1   LEFT JOIN NodesToOwnersForOwnerNew NTO ON NTO.NodeId = N1.ID 	where NTO.NodeId is not null  )", "CE4-OM", "How many processes don't have an Owner assigned in Current State?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.ID 	,PR.Title FROM Nodes PR JOIN TransactionStates T ON T.ID = PR.TransactionStateId WHERE T.[Key] = 'DAY_ONE' 	AND PR.NodeTypeId = 3 	AND NOT EXISTS  				(SELECT 1 FROM [NodesToDispositionsForDispositionNew] D WHERE D.NodeId = PR.ID)", "List processes that don't have Disposition assigned in Day 1 state." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title FROM Dispositions", "List the Disposition options available." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title [ProcessGroup] ,COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) [Enablers Count] FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 2 GROUP BY N.Title ORDER BY [Enablers Count] DESC", "How many enablers are associated with each Process Group?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT D.Title [Disposition] 	,COUNT(SD.SystemId) [SystemCount] FROM Dispositions D 	JOIN [SystemsToDispositionsForDispositionDay1New] SD ON SD.DispositionId = D.ID GROUP BY D.Title", "List the total number of systems by disposition." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT PR.Title [Process] , D.DispositionId FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId  	LEFT JOIN NodesToDispositionsForDispositionNew D ON D.NodeId = PR.ID WHERE PR.NodeTypeId = 3 	AND D.NodeId IS NULL 	AND S.[Key] = 'DAY_ONE'", "Can you please list down all Day 1 processes where no disposition has been tagged?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { " SELECT 	N.Title [Process]	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] A ON A.NodesId = N.ID LEFT JOIN [NodesToFacilitiesForEnablerFacilities] F ON F.NodesId = N.ID LEFT JOIN [NodesToSystemsForEnablerSystems] S ON S.NodesId = N.ID LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] T ON T.NodesId = N.ID WHERE NodeTypeId = 3 GROUP BY N.Title HAVING COUNT(A.AssetsId) + COUNT(F.FacilitiesId) +COUNT(S.SystemsId) + COUNT(T.ThirdPartyAgreementsId) = 0", "Can you please list all processes with no Enablers?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT PR.Title as MissingInDay1 FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'CURRENT_STATE' EXCEPT SELECT 	PR.Title [Process] FROM Nodes PR 	JOIN TransactionStates S ON S.ID = PR.TransactionStateId WHERE PR.NodeTypeId = 3 	AND S.[Key] = 'DAY_ONE'", "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT A.Title Enabler, 'Asset' as Category	 FROM Nodes N LEFT JOIN [NodesToAssetsForEnablerAssets] NA ON NA.NodesId = N.ID LEFT JOIN [Assets] A ON A.ID = NA.AssetsId WHERE A.Title IS NOT NULL UNION ALL SELECT DISTINCT F.Title Enabler, 'Facility' as Category FROM Nodes N LEFT JOIN [NodesToFacilitiesForEnablerFacilities] NF ON NF.NodesId = N.ID LEFT JOIN [Facilities] F ON F.ID = NF.FacilitiesId WHERE F.Title IS NOT NULL  UNION ALL SELECT DISTINCT S.Title Enabler , 'System' as Category FROM Nodes N LEFT JOIN [NodesToSystemsForEnablerSystems] NS ON NS.NodesId = N.ID LEFT JOIN [Systems] S ON S.ID = NS.SystemsId WHERE S.Title IS NOT NULL  UNION ALL SELECT DISTINCT T.Title Enabler	, 'TPA' as Category FROM Nodes N LEFT JOIN [NodesToThirdPartyAgreementsForEnablerTpa] NT ON NT.NodesId = N.ID LEFT JOIN [ThirdPartyAgreements] T ON T.ID = NT.ThirdPartyAgreementsId WHERE T.Title IS NOT NULL ", "What Enablers are we tracking for this project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# Systems present in this functional OP model] FROM Systems", "How many Systems are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Count(ID) AS [# TPAs present in this functional OP model] FROM ThirdPartyAgreements", "How many TPAs are there in this functional op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT ST.Title AS SystemType, COUNT(S.ID) [# Systems by Type] FROM Systems S LEFT JOIN SystemTypes ST ON ST.ID = S.TypeId WHERE S.TypeId IS NOT NULL GROUP BY ST.Title", "List the number of Systems by Type." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT O.Title AS Owners, Count(*) AS [# TPAs by Ownership] FROM ThirdPartyAgreements TP LEFT JOIN Owners O ON O.ID = TP.OwnerID WHERE TP.OwnerID IS NOT NULL GROUP BY O.Title", "List the number of TPAs by Ownership." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT Title [ProcessGroups With No Process] FROM Nodes WHERE nodetypeid = 2 AND ID NOT IN (SELECT NodeParentId FROM Nodes)", "List down all process groups with no process within them." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(P.[ID]) AS [Number of Process across Op Model] FROM Nodes P     LEFT JOIN NodeTypes NType         ON P.NodeTypeId = NType.ID WHERE NType.Title = 'Process'", "List the number of processes across op models." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Modified >= DATEADD(hh, -1, GETDATE())", "List history of changes to the op model in the past one hour." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE FieldImpacted = 'Owner'       AND CAST(Modified AS DATE) = CAST(GETDATE() AS DATE)", "List the history of changes to Ownership in the op model today." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT [Object],        Details,        FieldImpacted,        OldValue AS [Old Value],        NewValue AS [New Value],        [Title] AS [Activity] FROM OperatingModelActivityLog WHERE Title = 'Deleted'       AND DATEPART(wk, CAST(Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(Modified) = YEAR(GETDATE())", "List the history of deletes in the op model this week." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT T.Title [State], N.Title AS OperatingModel FROM Nodes N JOIN TransactionStates T ON T.ID = N.TransactionStateId  WHERE N.NodeTypeId = 1", "List the functional operating models across different states." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT N.Title Opmodel, L.Title LiveNote, Note FROM OperatingModelLiveNotes L LEFT JOIN Nodes N ON N.Id = L.NodeId", "Are there any Live Notes in this op model?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 146,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process FROM OperatingModelActivityLog OL  LEFT JOIN Nodes N ON N.ID = Ol.ObjectID WHERE OL.Title = 'Updated' AND Object = 'Process' AND (CAST(OL.Modified AS DATE) BETWEEN DATEADD(DAY, -7, CAST(GETDATE()AS DATE))  AND CAST(GETDATE() AS DATE))", "List down all processes which were updated since last 1 week?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N.Title Process, OldValue, NewValue  FROM OperatingModelActivityLog PL  LEFT JOIN Nodes N ON N.ID = PL.ObjectID  WHERE Object = 'Process' AND PL.Title = 'Updated' AND FieldImpacted = 'Title' AND N.TransactionStateId = 6", "List down all processes which have been renamed in Day 1 op model." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 148,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT F.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN Functions F on F.ID = T.FunctionId Group By F.ID, F.Title ", "CE4-TSA", "Show me the count of TSAs by function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 149,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT SF.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN SubFunctions SF on SF.ID = T.SubFunctionId where T.SubFunctionId IS NOT NULL Group By SF.ID, SF.Title ", "CE4-TSA", "Show me the count of TSAs by sub function." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ProviderLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Provider." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT LE.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN LegalEntities LE on LE.ID = T.ReceiverLegalEntityId Group By LE.ID, LE.Title ", "CE4-TSA", "Give me the list of TSAs by Receiver." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT P.Title, T.Title from TSAItems T LEFT JOIN TSAPhases P on P.ID = T.PhaseId Group By P.ID, P.Title ", "CE4-TSA", "Can you provide me list of TSAs by phases?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT T.Title, T.Duration as 'Duration in Month' from TSAItems T where T.Duration is not null ", "CE4-TSA", "Show me the breakdown of TSAs by duration." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title, Count(1) as 'Number Of TSAs' from TSAItems T LEFT JOIN ProjectTeams PT on PT.ID = T.ProjectTeamId Group by PT.ID, PT.Title ", "CE4-TSA", "How many TSAs does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT    TST.Title as [Phase],    COUNT(TC.ID) AS [# TSAs across different phases] FROM    TSAItems TC    LEFT JOIN TSAPhases TST ON TC.PhaseID = TST.ID  Group BY    TST.Title ", "CE4-TSA", "List the number of TSAs across different phases." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT    T.Title PendingTSAItems  FROM    TSAItems T    JOIN TSAStatuses TS ON T.TSAItemTSAStatusId = TS.ID  WHERE    T.AuthorId = ${Useremail}    AND TS.[key] = 'ACTIVE' ", "CE4-TSA", "How many pending TSA items do I have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { "SELECT    Title as [Billing Period Name],    DeadlineDate as [Contribution Deadlines]  FROM    TSABillingPeriods ", "CE4-TSA", "In Cost Tracking, how is my billing periods setup?", false });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { " SELECT    Title AS [TSA Phases]  FROM TSAPhases", "CE4-TSA", "What are the different TSA phases in my project?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ProviderLegalEntityId = L.ID where  L.ServiceLocation is not null UNION  SELECT    L.ServiceLocation  FROM    TSAItems T    JOIN LegalEntities L ON T.ReceiverLegalEntityId = L.ID   where  L.ServiceLocation is not null ", "CE4-TSA", "List the TSA service locations in my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT PT.Title as Team, TT.Title as 'Type'  from ProjectTeams PT LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] != 'CAPITAL_EDGE_SUPPORT' order by TT.Title ", "PROJECT_LEVEL", "What are the project teams that make up the governance structure for this engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(wp.WorkPlanTaskType) AS MilestoneCount FROM WorkPlan wp LEFT JOIN ProjectTeams pt ON pt.ID = wp.ProjectTeamId WHERE wp.WorkPlanTaskType = 'Milestone' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many milestones does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamName,     COUNT(1) AS InterdependencyCount FROM      Interdependencies AS I INNER JOIN      ProjectTeams AS ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID GROUP BY      ReceiverTeam.Title ", "PROJECT_LEVEL", "How many interdependencies does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT pt.Title, COUNT(1) AS RiskCount FROM RisksAndIssues RI left join ProjectTeams pt on pt.ID = ri.ProjectTeamId WHERE IssueRiskCategory = 'Risk' GROUP BY pt.Title ", "PROJECT_LEVEL", "How many risks does each team have?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 164,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 165,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 166,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 167,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 168,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 169,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I add a client user to the PMO app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 170,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "Provide case studies/credentials for similar deals that EY has supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 172,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { null, "CE4-PMO", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project.", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 173,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 174,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText", "VisibleToAssistant" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What revenue growth levers are available?", true });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 175,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 176,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 177,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 178,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                columns: new[] { "AnswerSQL", "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "ey-guidance", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 180,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 181,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 182,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 183,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 184,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What reports are available in the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "Provide case studies/credentials for similar deals that EY has supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 186,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 190,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 191,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 192,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 193,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 194,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 195,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 196,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key risks for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 197,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key milestones for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 198,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 199,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 200,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 201,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 202,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 203,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best strategies for improving a company in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 204,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are recent examples of improvements being made in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 205,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are the best ways to track actuals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 206,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What should be the frequency of tracking dollar values during the engagement?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 207,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-VC", "internet", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 208,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What is a normative operating model for the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 209,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What are key considerations when defining an operating model for a {Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 210,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-OM", "internet", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 212,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 213,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provides templates of TSA." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 215,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What should be the typical duration for TSA?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Things I should keep in mind for longer duration TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 217,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 218,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the key risks for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 219,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the key milestones for a {ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 220,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the best practices to run weekly status meetings?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 221,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "What are the best practices to build workplan, and track dependencies?" });
        }
    }
}
