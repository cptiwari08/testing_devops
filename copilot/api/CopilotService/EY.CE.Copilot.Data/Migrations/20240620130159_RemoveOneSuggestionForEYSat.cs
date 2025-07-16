using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOneSuggestionForEYSat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 363);

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 311,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 312,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 313,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this {ProjectType} project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Savings'", "CE4-VC", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 315,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Revenue Growth'", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 316,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'one-time'", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 317,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Savings' AND VCI.ProjectTeam = 'IT'", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 318,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'one-time' AND VCI.ProjectTeam = 'Supply Chain'", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 319,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 321,
                column: "SuggestionText",
                value: "Help understand VC methodology.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 322,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N._TemplateFile FROM Nodes N", "CE4-OM", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 323,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 324,
                column: "SuggestionText",
                value: "What are the steps to setup the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 325,
                column: "SuggestionText",
                value: "What reports are available in the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 326,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title FROM TSAItems TSA", "CE4-TSA", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 328,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title, TSA.ServiceInScopeDescription, TSA.[Function], TSA.SubFunction FROM TSAItems TSA", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 329,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 330,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 331,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT WP._TemplateFile FROM WorkPlan WP", "PROJECT_LEVEL", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 332,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 333,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 334,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 335,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this {ProjectType} project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 336,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 337,
                column: "SuggestionText",
                value: "What are the key risks for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 338,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 339,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 340,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 341,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 342,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 343,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 344,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 345,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 346,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 347,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 348,
                column: "SuggestionText",
                value: "What are typical implications for cross border deals?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 349,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a normative operating model for the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 350,
                column: "SuggestionText",
                value: "What are key considerations when defining an operating model for a {Sector} sector company?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 351,
                column: "SuggestionText",
                value: "What are examples of Day 1 process dispositions?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 352,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the {Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 353,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 354,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 355,
                column: "SuggestionText",
                value: "Provides templates of TSA.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 356,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 357,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 358,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 359,
                column: "SuggestionText",
                value: "What are the key risks for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 360,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 361,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 362,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 311,
                column: "SuggestionText",
                value: "How do I add a client user to Capital Edge?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 312,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 313,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 314,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-PMO", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 315,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Savings'", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 316,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Revenue Growth'", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 317,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'one-time'", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 318,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'Cost Savings' AND VCI.ProjectTeam = 'IT'", "What are the IT cost savings levers that I should be thinking about?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 319,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT VCI.Title FROM ValueCaptureInitiatives VCI WHERE VCI.ValueCaptureType = 'one-time' AND VCI.ProjectTeam = 'Supply Chain'", "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 320,
                column: "SuggestionText",
                value: "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 321,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 322,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 323,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT N._TemplateFile FROM Nodes N", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 324,
                column: "SuggestionText",
                value: "How can I upload systems in bulk to the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 325,
                column: "SuggestionText",
                value: "What are the steps to setup the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 326,
                column: "SuggestionText",
                value: "What reports are available in the Op Model app?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 327,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-OM", "Provide case studies/credentials for similar deals that EY has supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 328,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title FROM TSAItems TSA", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 329,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT TSA.Title, TSA.ServiceInScopeDescription, TSA.[Function], TSA.SubFunction FROM TSAItems TSA", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 330,
                column: "SuggestionText",
                value: "Why are TSAs important?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 331,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-TSA", "Provide case studies/credentials for similar deals that EY have supported in the past." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 332,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT DISTINCT WP._TemplateFile FROM WorkPlan WP", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 333,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 334,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 335,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 336,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "PROJECT_LEVEL", "ey-guidance", "Help me understand PMO methodology for this {ProjectType} project." });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 337,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 338,
                column: "SuggestionText",
                value: "What are the key risks for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 339,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 340,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 341,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 342,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 343,
                column: "SuggestionText",
                value: "What are the best cost saving initiatives?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 344,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 345,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 346,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 347,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 348,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 349,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 350,
                column: "SuggestionText",
                value: "What is a normative operating model for the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 351,
                column: "SuggestionText",
                value: "What are key considerations when defining an operating model for a {Sector} sector company?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 352,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 353,
                column: "SuggestionText",
                value: "What are the corporate functions typically involved in the {Sector} sector?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 354,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 355,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 356,
                column: "SuggestionText",
                value: "Provides templates of TSA.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 357,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 358,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Things I should keep in mind for longer duration TSAs?" });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 359,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 360,
                column: "SuggestionText",
                value: "What are the key risks for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 361,
                column: "SuggestionText",
                value: "What are the key milestones for a {ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 362,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.InsertData(
                table: "AssistantSuggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy", "VisibleToAssistant" },
                values: new object[] { 363, null, "PROJECT_LEVEL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System", true });
        }
    }
}
