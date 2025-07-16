using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantConfigurations",
                keyColumn: "ID",
                keyValue: 2,
                column: "Value",
                value: "{\"project-data\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": false},\"project-docs\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": true},\"ey-guidance\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": true},\"internet\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": true}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantConfigurations",
                keyColumn: "ID",
                keyValue: 2,
                column: "Value",
                value: "{\"project-data\": {\"isActive\": true,\"text-box-enabled\": false},\"project-docs\": {\"isActive\": true,\"text-box-enabled\": true},\"ey-guidance\": {\"isActive\": true,\"text-box-enabled\": true},\"internet\": {\"isActive\": true,\"text-box-enabled\": true}}");
        }
    }
}
