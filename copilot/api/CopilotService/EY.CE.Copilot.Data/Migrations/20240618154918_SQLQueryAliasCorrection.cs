using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class SQLQueryAliasCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                keyValue: 211,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes ND    WHERE NodeParentID IS NULL           AND TransactionStateID = 6     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM Nodes ROOT     WHERE ROOT.ID in (  select ID from GetSelectedFunction GSF where HierId = 2 and Title = 'Finance'  )     UNION ALL     SELECT CHILD.NodeParentID, CHILD.ID,   CHILD.Title, PARENT.TITLE,   PARENT.ID, CHILD.NodeTypeId     FROM PG_Hierarchy PARENT,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ) SELECT COUNT(*) AS [Process with multiple dispositions for Finance] FROM PG_Hierarchy WHERE NodeTypeId = 3       AND ID IN (                     SELECT NodeID                     FROM                     (                         SELECT NodeId,                                Count(DispositionID) As [Disposition Count] FROM NodesToDispositionsForDispositionNew NDDN  GROUP BY NodeId HAVING Count(DispositionID) > 1 ) a  )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 267,
                column: "AnswerSQL",
                value: "SELECT tcwf.year  AS [By Year], ROUND(SUM(tcwf.fte_cost_per_month), 2)    AS [TSA FTE Cost for Selected Year],     ROUND(SUM(tcwf.nonfte_cost_per_month), 2) AS [TSA Non-FTE Cost for Selected Year] FROM     vwTSACostWaterfall tcwf WHERE      tcwf.year = '{Year}' GROUP BY     tcwf.year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 187,
                column: "AnswerSQL",
                value: "SELECT [Object],        OMA.Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        [OMA.Title] AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.Modified >= DATEADD(hh, -1, GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 188,
                column: "AnswerSQL",
                value: "SELECT [Object],        Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        [OMA.Title] AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.FieldImpacted = 'Owner'       AND CAST(OMA.Modified AS DATE) = CAST(GETDATE() AS DATE)");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 189,
                column: "AnswerSQL",
                value: "SELECT [Object],        Details,        OMA.FieldImpacted,        OMA.OldValue AS [Old Value],        OMA.NewValue AS [New Value],        [OMA.Title] AS [Activity] FROM OperatingModelActivityLog OMA WHERE OMA.Title = 'Deleted'       AND DATEPART(wk, CAST(OMA.Modified AS DATE)) = DATEPART(wk, CAST(GETDATE() AS DATE))       AND YEAR(OMA.Modified) = YEAR(GETDATE())");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 211,
                column: "AnswerSQL",
                value: ";WITH GetSelectedFunction AS (SELECT id,            Title,            NodeParentID,            HierId = 1     FROM Nodes ND    WHERE NodeParentID IS NULL           AND TransactionStateID = 6     UNION ALL     SELECT Nodes.id,            Nodes.Title,            Nodes.NodeParentID,            HierId = HierId + 1     FROM Nodes,          GetSelectedFunction     where Nodes.NodeParentID = GetSelectedFunction.id    ),       PG_Hierarchy (NodeParentID, ID, Title, Parent, ParentID, NodeTypeId) AS (SELECT ROOT.NodeParentID,            ROOT.id,            ROOT.Title,            ROOT.Title,            ROOT.ID AS ParentID,            ROOT.NodeTypeId     FROM Nodes ROOT     WHERE ROOT.ID in (  select ID from GetSelectedFunction GSF where HierId = 2 and Title = 'Finance'  )     UNION ALL     SELECT CHILD.NodeParentID, CHILD.ID,   CHILD.Title, PARENT.TITLE,   PARENT.ID, CHILD.NodeTypeId     FROM PG_Hierarchy PARENT PGHP,          Nodes CHILD     where PARENT.id = CHILD.NodeParentID    ) SELECT COUNT(*) AS [Process with multiple dispositions for Finance] FROM PG_Hierarchy WHERE NodeTypeId = 3       AND ID IN (                     SELECT NodeID                     FROM                     (                         SELECT NodeId,                                Count(DispositionID) As [Disposition Count] FROM NodesToDispositionsForDispositionNew NDDN  GROUP BY NodeId HAVING Count(DispositionID) > 1 ) a  )");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 267,
                column: "AnswerSQL",
                value: "SELECT     [tcwf.year]                               AS [By Year],     ROUND(SUM(tcwf.fte_cost_per_month), 2)    AS [TSA FTE Cost for Selected Year],     ROUND(SUM(tcwf.nonfte_cost_per_month), 2) AS [TSA Non-FTE Cost for Selected Year] FROM     vwTSACostWaterfall tcwf WHERE      [tcwf.year] = '{Year}' GROUP BY     [tcwf.year]");
        }
    }
}
