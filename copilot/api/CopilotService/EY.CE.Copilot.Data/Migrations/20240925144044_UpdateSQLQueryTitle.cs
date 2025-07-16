using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSQLQueryTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 371,
                column: "AnswerSQL",
                value: "SELECT  A.UniqueItemIdentifier,  A.Title, PT.Title AS ProjectTeam, U.Title AS ItemOwner, S.Title as Status  FROM Actions A      LEFT JOIN Statuses S ON A.ItemStatusId = S.ID      LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID   LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID    LEFT JOIN UserProfiles U ON A.ItemOwnerId = U.ID   WHERE S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED', 'ON_HOLD')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 371,
                column: "AnswerSQL",
                value: "SELECT  A.UniqueItemIdentifier,  A.Title, PT.Title AS ProjectTeam, U.Title AS ItemOwner, S.Title  FROM Actions A      LEFT JOIN Statuses S ON A.ItemStatusId = S.ID      LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID   LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID    LEFT JOIN UserProfiles U ON A.ItemOwnerId = U.ID   WHERE S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED', 'ON_HOLD')   AND TT.[Key] = 'PROJECT_MANAGEMENT'");
        }
    }
}
