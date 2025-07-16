using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewActionSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AssistantSuggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "IsIncluded", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy", "VisibleToAssistant" },
                values: new object[,]
                {
                    { 369, "SELECT  A.UniqueItemIdentifier,  A.Title , PT.Title ProjectTeam, U.Title ItemOwner  FROM    Actions A      LEFT JOIN Statuses S ON A.ItemStatusId = S.ID      LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID   LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID    LEFT JOIN UserProfiles U ON A.ItemOwnerId = U.ID   WHERE    PT.Title = 'IT'  AND TT.[Key] = 'PROJECT_MANAGEMENT'", "CE4-PMO", null, "System", false, "project-data", "How many actions are assigned to IT team?", null, "System", false },
                    { 370, "SELECT       PT.Title AS ProjectTeam,                COUNT(A.UniqueItemIdentifier) AS ActionCount    FROM       Actions A      LEFT JOIN       ProjectTeams PT ON A.ProjectTeamId = PT.ID    LEFT JOIN       TeamTypes TT ON PT.TeamTypeId = TT.ID    WHERE       TT.[Key] = 'PROJECT_MANAGEMENT'   GROUP BY       PT.Title                           ORDER BY       ActionCount DESC", "CE4-PMO", null, "System", true, "project-data", "Could you provide the number of action items categorized by project team?", null, "System", true },
                    { 371, "SELECT  A.UniqueItemIdentifier,  A.Title, PT.Title AS ProjectTeam, U.Title AS ItemOwner, S.Title  FROM Actions A      LEFT JOIN Statuses S ON A.ItemStatusId = S.ID      LEFT JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID   LEFT JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID    LEFT JOIN UserProfiles U ON A.ItemOwnerId = U.ID   WHERE S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED', 'ON_HOLD')   AND TT.[Key] = 'PROJECT_MANAGEMENT'", "CE4-PMO", null, "System", true, "project-data", "List all Open Actions in PMO app.", null, "System", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 371);
        }
    }
}
