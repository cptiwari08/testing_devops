using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOneSqlQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                column: "AnswerSQL",
                value: "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                column: "AnswerSQL",
                value: "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY ");
        }
    }
}
