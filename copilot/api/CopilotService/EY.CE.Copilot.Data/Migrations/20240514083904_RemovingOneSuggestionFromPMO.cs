using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingOneSuggestionFromPMO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112);

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "SuggestionText",
                value: "What are typical implications for cross border deals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a normative operating model for the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "What are key considerations when defining an operating model for a ${Sector} sector company?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "SuggestionText",
                value: "What are examples of Day 1 process dispositions?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
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
                column: "SuggestionText",
                value: "When was the deal announced?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "What are the best cost saving initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "What is a normative operating model for the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "SuggestionText",
                value: "What are key considerations when defining an operating model for a ${Sector} sector company?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                column: "SuggestionText",
                value: "What are the corporate functions typically involved in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Things I should keep in mind for longer duration TSAs" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 112, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System" });
        }
    }
}
