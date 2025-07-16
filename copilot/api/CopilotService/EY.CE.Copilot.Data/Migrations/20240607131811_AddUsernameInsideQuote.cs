using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameInsideQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT W.Title AS  WorkPlanItem FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID WHERE UP.Title = '{Username}' ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "AnswerSQL",
                value: "SELECT AN.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN UserProfiles UP ON PT.ItemOwnerID = UP.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE UP.EMail = '{Username}' AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Accomplishment'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                column: "AnswerSQL",
                value: "SELECT I.Title FROM Interdependencies I JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.EMail = '{Username}' ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "AnswerSQL",
                value: "SELECT  W.Title,'Workplan' [Category] FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID WHERE UP.Title = '{Username}' UNION ALL  SELECT RI.Title,IssueRiskCategory [Category] FROM RisksAndIssues RI LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID WHERE UP.Title = '{Username}' UNION ALL SELECT A.Title,'Action' [Category] FROM Actions A LEFT JOIN UserProfiles UP ON A.ItemOwnerId = UP.ID WHERE UP.Title = '{Username}' UNION ALL  SELECT D.Title,'Decision' [Category] FROM Decisions D LEFT JOIN UserProfiles UP ON D.ItemOwnerId = UP.ID WHERE UP.Title = '{Username}' UNION ALL  SELECT I.Title,'Interdependecy as Provider' [Category] FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.Title = '{Username}'  UNION ALL  SELECT I.Title,'Interdependecy as Receiver' [Category] FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ReceiverOwnerId = UP.ID WHERE UP.Title = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "AnswerSQL",
                value: "SELECT VI.Title [Initiaves] FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Title = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "AnswerSQL",
                value: "SELECT VI.Title AS Initiatives FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "AnswerSQL",
                value: "SELECT VI.Title AS Initiatives, VS.Title ValueCaptureStage FROM ValueCaptureInitiatives VI JOIN ValueCaptureStages VS ON VI.ValueCaptureStageID = VS.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}' ORDER BY ValueCaptureStage");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "AnswerSQL",
                value: "SELECT VI.Title AS Initiatives, VT.Title ValueCaptureType FROM ValueCaptureInitiatives VI JOIN ValueCaptureTypes VT ON VI.ValueCaptureStageID = VT.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = '{Username}' ORDER BY ValueCaptureType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "AnswerSQL",
                value: "SELECT W.Title AS  WorkPlanItem FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID WHERE UP.Title = {Username} ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "AnswerSQL",
                value: "SELECT AN.Title FROM ProjectStatusEntries PSE JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.ID JOIN UserProfiles UP ON PT.ItemOwnerID = UP.ID JOIN AccomplishmentsAndNextSteps AN ON PSE.ID = AN.ProjectStatusEntryId JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.ID WHERE UP.EMail = {Username} AND RP.ID = (SELECT ID -1 FROM ReportingPeriods WHERE GETDATE() BETWEEN PeriodStartDate AND PeriodEndDate) AND AN.AccomplishmentNextstepCategory = 'Accomplishment'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                column: "AnswerSQL",
                value: "SELECT I.Title FROM Interdependencies I JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.EMail = {Username} ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "AnswerSQL",
                value: "SELECT  W.Title,'Workplan' [Category] FROM Workplan W LEFT JOIN UserProfiles UP ON W.TaskOwnerID = UP.ID WHERE UP.Title = {Username} UNION ALL  SELECT RI.Title,IssueRiskCategory [Category] FROM RisksAndIssues RI LEFT JOIN UserProfiles UP ON RI.ItemOwnerId = UP.ID WHERE UP.Title = {Username} UNION ALL SELECT A.Title,'Action' [Category] FROM Actions A LEFT JOIN UserProfiles UP ON A.ItemOwnerId = UP.ID WHERE UP.Title = {Username} UNION ALL  SELECT D.Title,'Decision' [Category] FROM Decisions D LEFT JOIN UserProfiles UP ON D.ItemOwnerId = UP.ID WHERE UP.Title = {Username} UNION ALL  SELECT I.Title,'Interdependecy as Provider' [Category] FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ProviderOwnerId = UP.ID WHERE UP.Title = {Username}  UNION ALL  SELECT I.Title,'Interdependecy as Receiver' [Category] FROM Interdependencies I LEFT JOIN UserProfiles UP ON I.ReceiverOwnerId = UP.ID WHERE UP.Title = {Username}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "AnswerSQL",
                value: "SELECT VI.Title [Initiaves] FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Title = {Username}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "AnswerSQL",
                value: "SELECT VI.Title AS Initiatives FROM ValueCaptureInitiatives VI JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "AnswerSQL",
                value: "SELECT VI.Title AS Initiatives, VS.Title ValueCaptureStage FROM ValueCaptureInitiatives VI JOIN ValueCaptureStages VS ON VI.ValueCaptureStageID = VS.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureStage");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "AnswerSQL",
                value: "SELECT VI.Title AS Initiatives, VT.Title ValueCaptureType FROM ValueCaptureInitiatives VI JOIN ValueCaptureTypes VT ON VI.ValueCaptureStageID = VT.ID JOIN UserProfiles UP ON VI.ItemOwnerId = UP.ID WHERE UP.Email = {Username} ORDER BY ValueCaptureType");
        }
    }
}
