using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingTagWIthXYZInSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100);

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are the best practices to optimize future state of ${XYZ} Workstream?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "What are the corporate functions typically involved in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Things I should keep in mind for longer duration TSAs" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 100, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System" });
        }
    }
}
