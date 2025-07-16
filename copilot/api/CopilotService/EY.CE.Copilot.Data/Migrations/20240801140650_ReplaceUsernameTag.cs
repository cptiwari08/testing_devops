using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceUsernameTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT W.Title, UP.Title as UserName AS WorkPlanItem FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, '{Username}') AND TT.[Key] = 'PROJECT_MANAGEMENT'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT W.Title, UP.Title as UserName AS WorkPlanItem FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID LEFT JOIN ProjectTeams PT ON W.ProjectTeamId = PT.ID LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, {'Username'}) AND TT.[Key] = 'PROJECT_MANAGEMENT'");
        }
    }
}
