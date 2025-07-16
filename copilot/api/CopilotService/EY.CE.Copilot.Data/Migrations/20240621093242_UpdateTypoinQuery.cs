using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypoinQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Saving' or  VCI.ValueCaptureType = 'Cost Reduction'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                column: "AnswerSQL",
                value: "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Saving' or  VCI.ValueCaptureType = 'Cost Reduction'What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?");
        }
    }
}
