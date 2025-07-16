using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyIncorrectSqlQueries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT wp.Title Workplan, pt.Title ProjectTeam, s.Title [Status] ,Wp.TaskDueDate, wp.StartDate, wp.IsCritical, UP.Title TaskOwner  from WorkPlan wp    LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID   left join ProjectTeams pt on pt.ID = wp.ProjectTeamId  LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId  LEFT JOIN UserProfiles UP ON wp.TaskOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, '{Username}') AND TT.[Key] = 'PROJECT_MANAGEMENT')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                column: "AnswerSQL",
                value: "SELECT TOP 1      CAST(RP.PeriodEndDate AS DATE) AS WeeklyStatusDueDate,     RP.Title AS PeriodTitle,     RP.PeriodStartDate AS StartDate,     RP.PeriodEndDate AS \"End Date\",     RP.Created AS CreatedDate,     RP.Modified AS LastModifiedDate FROM      ReportingPeriods RP  WHERE      CAST(GETDATE() AS DATE) BETWEEN CAST(RP.PeriodStartDate AS DATE) AND CAST(RP.PeriodEndDate AS DATE)  ORDER BY      RP.Modified DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "AnswerSQL",
                value: "SELECT W.UniqueItemIdentifier, W.Title CriticalTasks, PT.Title WorkStream, S.Title [Status] , W.TaskDueDate, W.StartDate, W.IsCritical, UP.Title TaskOwner  FROM WorkPlan W   LEFT JOIN Statuses S on W.WorkPlanTaskStatusId = S.ID   LEFT JOIN ProjectTeams PT on PT.ID = W.WorkstreamId LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId  LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE W.IsCritical = 1  AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                column: "AnswerSQL",
                value: "SELECT W.UniqueItemIdentifier, W.Title TasksWithoutOwner, PT.Title WorkStream, S.Title [Status] , W.TaskDueDate, W.StartDate, W.IsCritical FROM WorkPlan W   LEFT JOIN Statuses S on W.WorkPlanTaskStatusId = S.ID   LEFT JOIN ProjectTeams PT on PT.ID = W.WorkstreamId LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId  WHERE W.TaskOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                column: "AnswerSQL",
                value: "SELECT RI.TITLE AS [Risk items that does not have Risk Mitigation plan], RI.ItemDescription, RI.ItemDueDate, RI.RiskImpact, RI.RiskMitigation, RI.RiskProbability, PT.Title ProjectTeam, UP.Title ItemOwner  FROM RisksAndIssues RI  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID WHERE RI.IssueRiskCategory = 'Risk' AND RI.RiskMitigation IS NULL AND TT.[Key]='PROJECT_MANAGEMENT'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT wp.Title Workplan, pt.Title ProjectTeam, s.Title [Status] ,Wp.TaskDueDate, wp.StartDate, wp.IsCritical, UP.Title TaskOwner  from WorkPlan wp    LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID   left join ProjectTeams pt on pt.ID = wp.ProjectTeamId  LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId  LEFT JOIN UserProfiles UP ON wp.TaskOwnerId = UP.ID WHERE UP. Title = ( SELECT TOP 1 Title  FROM UserProfiles WHERE FREETEXT(Title, {'Username'}) AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 17,
                column: "AnswerSQL",
                value: "SELECT TOP 1      CAST(RP.PeriodEndDate AS DATE) AS WeeklyStatusDueDate,     RP.Title AS PeriodTitle,     RP.PeriodStartDate AS StartDate,     RP.PeriodEndDate AS End Date,     RP.Created AS CreatedDate,     RP.Modified AS LastModifiedDate FROM      ReportingPeriods RP  WHERE      CAST(GETDATE() AS DATE) BETWEEN CAST(RP.PeriodStartDate AS DATE) AND CAST(RP.PeriodEndDate AS DATE)  ORDER BY      RP.Modified DESC");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "AnswerSQL",
                value: "SELECT W.UniqueItemIdentifier, W.Title CriticalTasks, PT.Title WorkStream, S.Title [Status] , W.TaskDueDate, W.StartDate, W.IsCritical, UP.Title TaskOwner  FROM WorkPlan W   LEFT JOIN Statuses S on wp.WorkPlanTaskStatusId = S.ID   LEFT JOIN ProjectTeams PT on PT.ID = W.WorkstreamId LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId  LEFT JOIN UserProfiles UP ON W.TaskOwnerId = UP.ID WHERE W.IsCritical = 1  AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 53,
                column: "AnswerSQL",
                value: "SELECT W.UniqueItemIdentifier, W.Title TasksWithoutOwner, PT.Title WorkStream, S.Title [Status] , W.TaskDueDate, W.StartDate, W.IsCritical FROM WorkPlan W   LEFT JOIN Statuses S on wp.WorkPlanTaskStatusId = S.ID   LEFT JOIN ProjectTeams PT on PT.ID = W.WorkstreamId LEFT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId  WHERE W.TaskOwnerId IS NULL AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 74,
                column: "AnswerSQL",
                value: "SELECT RI.TITLE AS [Risk items that does not have Risk Mitigation plan], , RI.ItemDescription, RI.ItemDueDate, RI.RiskImpact, RI.RiskMitigation, RI.RiskProbability, PT.Title ProjectTeam, UP.Title ItemOwner  FROM RisksAndIssues RI  LEFT JOIN ProjectTeams PT ON RI.ProjectTeamID = PT.ID   LEFT JOIN TeamTypes TT on TT.ID  = PT.TeamTypeId  LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID WHERE RI.IssueRiskCategory = 'Risk' AND RI.RiskMitigation IS NULL AND TT.[Key]='PROJECT_MANAGEMENT'");
        }
    }
}
