using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEYIPQueryAlias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 307,
                column: "AnswerSQL",
                value: "SELECT DISTINCT _TemplateFile FROM WorkPlan");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 313,
                column: "AnswerSQL",
                value: "SELECT Title FROM ValueCaptureInitiatives WHERE ValueCaptureType = 'Cost Saving' or  ValueCaptureType = 'Cost Reduction'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                column: "AnswerSQL",
                value: "SELECT Title FROM ValueCaptureInitiatives WHERE ValueCaptureType = 'Revenue Growth'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 315,
                column: "AnswerSQL",
                value: "SELECT Title FROM ValueCaptureInitiatives WHERE ValueCaptureType = 'One Time Cost'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 316,
                column: "AnswerSQL",
                value: "SELECT Title FROM ValueCaptureInitiatives WHERE (ValueCaptureType = 'Cost Reduction' OR  ValueCaptureType = 'Cost Saving') AND ProjectTeam = 'IT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 317,
                column: "AnswerSQL",
                value: "SELECT Title FROM ValueCaptureInitiatives WHERE ValueCaptureType = 'One Time Cost' AND ProjectTeam = 'Supply Chain & Operations'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                column: "AnswerSQL",
                value: "SELECT DISTINCT _TemplateFile FROM Nodes");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 324,
                column: "AnswerSQL",
                value: "SELECT Title FROM TSAItems");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 325,
                column: "AnswerSQL",
                value: "SELECT Title, ServiceInScopeDescription, [Function], SubFunction FROM TSAItems");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                column: "AnswerSQL",
                value: "SELECT DISTINCT _TemplateFile FROM WorkPlan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 307,
                column: "AnswerSQL",
                value: "SELECT DISTINCT WP._TemplateFile FROM WorkPlan WP");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 313,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Saving' or  VCI.ValueCaptureType = 'Cost Reduction'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Revenue Growth'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 315,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'One Time Cost'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 316,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE (VCI.ValueCaptureType = 'Cost Reduction' OR  VCI.ValueCaptureType = 'Cost Saving') AND VCI.ProjectTeam = 'IT'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 317,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'One Time Cost' AND VCI.ProjectTeam = 'Supply Chain & Operations'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                column: "AnswerSQL",
                value: "SELECT DISTINCT N._TemplateFile FROM Nodes N");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 324,
                column: "AnswerSQL",
                value: "SELECT TSA.Title FROM TSAItems TSA");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 325,
                column: "AnswerSQL",
                value: "SELECT TSA.Title, TSA.ServiceInScopeDescription, TSA.[Function], TSA.SubFunction FROM TSAItems TSA");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                column: "AnswerSQL",
                value: "SELECT DISTINCT WP._TemplateFile FROM WorkPlan WP");
        }
    }
}
