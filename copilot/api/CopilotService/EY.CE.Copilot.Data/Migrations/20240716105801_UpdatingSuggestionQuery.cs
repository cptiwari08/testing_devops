using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingSuggestionQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "AnswerSQL",
                value: "SELECT RI.RiskImpact, RI.RiskProbability, COUNT(*) AS RiskCount FROM RisksAndIssues RI LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE RI.IssueRiskCategory = 'Risk' AND RI.ItemStatusId NOT IN (SELECT S.ID FROM statuses S WHERE S.[Key] IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD' )) AND (RI.RiskImpact IS NOT NULL OR RI.RiskProbability IS NOT NULL) GROUP BY RI.RiskImpact, RI.RiskProbability ORDER BY RI.RiskImpact, RI.RiskProbability");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                column: "AnswerSQL",
                value: "SELECT     RI.UniqueItemIdentifier,     RI.Title,    'Risks' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD' )     AND RI.IssueRiskCategory = 'Risk'     AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT     RI.UniqueItemIdentifier,     RI.Title,    'Issues' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (   'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD'   )     AND RI.IssueRiskCategory = 'Issue'     AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT     A.UniqueItemIdentifier,     A.Title,    'Actions' as Category  FROM    Actions A    JOIN Statuses S ON A.ItemStatusId = S.ID    JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (  'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD' )     AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT     D.UniqueItemIdentifier,     D.Title,    'Decisions' as Category  FROM    Decisions D    LEFT JOIN Statuses S ON D.ItemStatusId = S.ID      JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (   'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD'  )    AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as OpenInterdependencies, S.Title as Status FROM Interdependencies I LEFT JOIN InterdependencyStatuses S ON I.InterdependencyStatusId = S.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD')");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 270,
                column: "AnswerSQL",
                value: "SELECT TSA.Title AS [TSAs with Open Risks] FROM TSAItems TSA     LEFT JOIN RisksAndIssues RI         ON TSA.ID = RI.TSAItemId     LEFT JOIN Statuses S         ON RI.ProgressId = S.ID WHERE S.[Key] NOT IN ( 'COMPLETED', 'CLOSED', 'CANCELLED', 'DELETED', 'REJECTED',  'ON_HOLD')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "AnswerSQL",
                value: "SELECT RI.RiskImpact, RI.RiskProbability, COUNT(*) AS RiskCount FROM RisksAndIssues RI LEFT JOIN ProjectTeams pt on pt.ID = RI.ProjectTeamId LEFT JOIN TeamTypes TT on TT.ID = pt.TeamTypeId WHERE RI.IssueRiskCategory = 'Risk' AND RI.ItemStatusId NOT IN (SELECT S.ID FROM statuses S WHERE S.[Key] IN ('COMPLETED', 'CLOSED')) AND (RI.RiskImpact IS NOT NULL OR RI.RiskProbability IS NOT NULL) GROUP BY RI.RiskImpact, RI.RiskProbability ORDER BY RI.RiskImpact, RI.RiskProbability");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 46,
                column: "AnswerSQL",
                value: "SELECT    RI.UniqueItemIdentifier,    RI.Title,    'Risks' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND RI.IssueRiskCategory = 'Risk'    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    RI.UniqueItemIdentifier,    RI.Title,    'Issues' as Category  FROM    RisksAndIssues RI   JOIN Statuses S ON RI.ItemStatusId = S.ID  JOIN ProjectTeams PT ON RI.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND RI.IssueRiskCategory = 'Issue'    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    A.UniqueItemIdentifier,    A.Title,    'Actions' as Category  FROM    Actions A    JOIN Statuses S ON A.ItemStatusId = S.ID    JOIN ProjectTeams PT ON A.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )    AND TT.[Key] = 'PROJECT_MANAGEMENT' UNION  SELECT    D.UniqueItemIdentifier,    D.Title,    'Decisions' as Category  FROM    Decisions D    LEFT JOIN Statuses S ON D.ItemStatusId = S.ID      JOIN ProjectTeams PT ON D.ProjectTeamId = PT.ID JOIN TeamTypes TT ON PT.TeamTypeId = TT.ID WHERE    S.[KEY] NOT IN (     'COMPLETED', 'CANCELLED', 'CLOSED',      'ON_HOLD'   )   AND TT.[Key] = 'PROJECT_MANAGEMENT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "AnswerSQL",
                value: "SELECT I.UniqueItemIdentifier, I.Title as OpenInterdependencies, S.Title as Status FROM Interdependencies I LEFT JOIN InterdependencyStatuses S ON I.InterdependencyStatusId = S.ID  WHERE S.[KEY] NOT IN ('COMPLETED', 'Canceled') ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 270,
                column: "AnswerSQL",
                value: "SELECT TSA.Title AS [TSAs with Open Risks] FROM TSAItems TSA     LEFT JOIN RisksAndIssues RI         ON TSA.ID = RI.TSAItemId     LEFT JOIN Statuses S         ON RI.ProgressId = S.ID WHERE S.[Key] NOT IN ( 'COMPLETED', 'CLOSED' )");
        }
    }
}
