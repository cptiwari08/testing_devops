using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOneSuggestionQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                column: "AnswerSQL",
                value: "SELECT COUNT(ID) AS NewRisksCount FROM RisksAndIssues WHERE IssueRiskCategory = 'Risk' AND DATEDIFF(day, Created, GETDATE()) <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 72,
                column: "AnswerSQL",
                value: "SELECT  	COUNT(ID) AS NewRisksCount FROM  	RisksAndIssues WHERE  	 IssueRiskCategory like $[IssueRiskCategory] 	AND DATEDIFF(day, Created, GETDATE()) <= 5");
        }
    }
}
