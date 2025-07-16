using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserNameTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = '${Username}'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = ${Username}");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "AnswerSQL",
                value: "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = ${Username}");
        }
    }
}
