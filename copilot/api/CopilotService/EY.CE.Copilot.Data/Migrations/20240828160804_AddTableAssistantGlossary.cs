using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAssistantGlossary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssistantGlossary",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Context = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSchema = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantGlossary", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "AssistantConfigurations",
                keyColumn: "ID",
                keyValue: 2,
                column: "Value",
                value: "\r\n                    [\r\n                        {\r\n                            \"key\": \"project-docs\",\r\n                            \"displayName\": \"Project Docs\",\r\n                            \"originalDisplayName\": \"Project Docs\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Reference information from the project documents that you have access to through SharePoint. Pick and choose which documents you want to focus on via the Document Hub.\",\r\n                            \"isDefault\": false,\r\n                            \"ordinal\": 1,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"project-data\",\r\n                            \"displayName\": \"Project Data\",\r\n                            \"originalDisplayName\": \"Project Data\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Ask questions about how this project is progressing. Examples of key insights to ask about include identifying existing risks, issues, actions, decisions, or initiatives that have fallen of track.\",\r\n                            \"isDefault\": false,\r\n                            \"ordinal\": 2,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"ey-guidance\",\r\n                            \"displayName\": \"EY Guidance\",\r\n                            \"originalDisplayName\": \"EY Guidance\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Ask questions against SaT best practices, Capital Edge help, and EY intellectual property such as workplan blueprints, cost saving initiatives, and normative operating models.\",\r\n                            \"isDefault\": true,\r\n                            \"ordinal\": 3,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"internet\",\r\n                            \"displayName\": \"Internet\",\r\n                            \"originalDisplayName\": \"Internet\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Open your search to the entire internet, protected by the wrapper of EY.ai.\",\r\n                            \"isDefault\": true,\r\n                            \"ordinal\": 4,\r\n                            \"isActive\": true\r\n                        }\r\n                    ]");

            migrationBuilder.InsertData(
                table: "AssistantGlossary",
                columns: new[] { "ID", "Context", "CreatedAt", "CreatedBy", "IsSchema", "TableName", "Tag", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "When asked for due times, past due, this week, slippage, among other similar, use the column TaskDueDate", null, "System", false, "|WorkPlan|", "static", null, "System" },
                    { 2, "When asked for list of tasks, name of task, workplan items, and want you to list them, use the column WorkPlan.Title always if needed and WorkPlan.UniqueItemIdentifier when possible.", null, "System", false, "|WorkPlan|", "static", null, "System" },
                    { 3, "When asked for risks in any case, try to include the columns Title and UniqueItemIdentifier if possible", null, "System", false, "|RisksAndIssues|", "static", null, "System" },
                    { 4, "When asked for scope of service use the column ServiceInScopeDescription from the table TSAItems", null, "System", false, "|TSAItems|", "static", null, "System" },
                    { 5, "When asked for services use the column Title from table TSAItems", null, "System", false, "|TSAItems|", "static", null, "System" },
                    { 6, "When asked for list of TSA ending in a specific month use the column TSAItemEndDate", null, "System", false, "|TSAItems|", "static", null, "System" },
                    { 7, "Duration should be calculated by subtracting a StartDate from an EndDate, formula is EndDate - StartDate", null, "System", false, "AllTables", "static", null, "System" },
                    { 8, "For TSA, date columns to be used are TSAItemEndDate and TSAItemStartDate", null, "System", false, "|TSAItems|", "static", null, "System" },
                    { 9, "When asked about plans of initiatives you MUST use the ValueCaptureInitiatives and ValueCaptureEstimates tables", null, "System", false, "|ValueCaptureInitiatives|", "static", null, "System" },
                    { 10, "When asked about ALREADY achieved initiatives you MUST use ValueCaptureActuals and ValueCaptureInitiatives tables", null, "System", false, "|ValueCaptureInitiatives|", "static", null, "System" },
                    { 11, "Team, teams is in the table ProjectTeams in the column Title", null, "System", false, "|WorkPlan|,|RisksAndIssues|,|TSAItems|,|Nodes|,|ValueCaptureIntitiatives|", "static", null, "System" },
                    { 12, "Overdue means past due date", null, "System", false, "|WorkPlan|,|RisksAndIssues|,|TSAItems|,|Nodes|,|ValueCaptureIntitiatives|", "static", null, "System" },
                    { 13, "When asked about processes it is referring to Operating Model processes, you should use Nodes tables and tables referring to Nodes", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 14, "When asked for Day 1 processes, use operating models tables and, join with TransactionStates and filter [key] column with 'DAY_ONE'", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 15, "When asked for Current State processes, use operating models tables and, join with TransactionStates and filter [key] column with 'CURRENT_STATE'", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 16, "When asked about initiatives use tables related to VC or ValueCapture", null, "System", false, "|ValueCaptureInitiatives|", "static", null, "System" },
                    { 17, "When I ask something about me or other person use the table UserProfiles to filter my email with the column Email or the table ActiveDirectory with columns AuthorID or EditorID", null, "System", false, "AllTables", "static", null, "System" },
                    { 18, "If you're asked targets and engagement you MUST use 'ValueCaptureTopDownEstimates' table", null, "System", false, "|ValueCaptureTopDownEstimates|", "static", null, "System" },
                    { 19, "When asked for processes by op model, please use the table 'Nodes', use the column Title", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 20, "When asked for ANY NUMBER, current STATE or HOW MANY for op model, please use the table 'Nodes', use the column Title and apply a count", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 21, "When asked for HOW MANY processes and States, please use the table 'Nodes', use the column Title apply a count and use a LEFT with the table TransactionStated", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 22, "When asked for op models 'Systems' with 'Processes', please use the bridge table NodesSystemsForEnablerSystems as main tables", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 23, "When asked for op models 'Assets', please use the bridge table NodesToAssetsForEnablerAssets as main tables", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 24, "When asked for op models 'Processes Dispositions', please use the bridge table NodesToDispositionsForDispositionNew as main tables", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 25, "When asked for op models 'Systems', please use the bridge table NodesSystemsForEnablerSystems as main tables", null, "System", false, "|Nodes|", "static", null, "System" },
                    { 26, "A status is Open when is not Cancelled, completed, closed, deleted, rejected or on hold", null, "System", false, "AllTables", "static", null, "System" },
                    { 27, "When asked about 'IT' team or 'IT' project team, please filter using the column Title from ProjectTeams table", null, "System", false, "AllTables", "static", null, "System" },
                    { 28, "If you receive information about Team Type join with the ProjectTeams table, include the filter. The name of the team type is on TeamTypes table in the column Title. Show this column if possible.", null, "System", false, "AllTables", "static", null, "System" }
                });

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                column: "SuggestionText",
                value: "Which teams have not entered their weekly status report for this reporting period?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssistantGlossary");

            migrationBuilder.UpdateData(
                table: "AssistantConfigurations",
                keyColumn: "ID",
                keyValue: 2,
                column: "Value",
                value: "{\"project-data\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": false},\"project-docs\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": true},\"ey-guidance\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": true},\"internet\": {\"isActive\": true,\"isQuestionTextBoxEnabled\": true}}");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                column: "SuggestionText",
                value: "Which teams have not entered thier weekly status report for this reporting period?");
        }
    }
}
