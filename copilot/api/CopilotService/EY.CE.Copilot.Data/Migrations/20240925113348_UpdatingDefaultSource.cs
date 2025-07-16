using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingDefaultSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantConfigurations",
                keyColumn: "ID",
                keyValue: 2,
                column: "Value",
                value: "\r\n                    [\r\n                        {\r\n                            \"key\": \"project-docs\",\r\n                            \"displayName\": \"Project Docs\",\r\n                            \"originalDisplayName\": \"Project Docs\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Reference information from the project documents that you have access to through SharePoint. Pick and choose which documents you want to focus on via the Document Hub.\",\r\n                            \"isDefault\": false,\r\n                            \"ordinal\": 1,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"project-data\",\r\n                            \"displayName\": \"Project Data\",\r\n                            \"originalDisplayName\": \"Project Data\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Ask questions about how this project is progressing. Examples of key insights to ask about include identifying existing risks, issues, actions, decisions, or initiatives that have fallen of track.\",\r\n                            \"isDefault\": false,\r\n                            \"ordinal\": 2,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"ey-guidance\",\r\n                            \"displayName\": \"EY Guidance\",\r\n                            \"originalDisplayName\": \"EY Guidance\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Ask questions against SaT best practices, Capital Edge help, and EY intellectual property such as workplan blueprints, cost saving initiatives, and normative operating models.\",\r\n                            \"isDefault\": true,\r\n                            \"ordinal\": 3,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"internet\",\r\n                            \"displayName\": \"Internet\",\r\n                            \"originalDisplayName\": \"Internet\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Open your search to the entire internet, protected by the wrapper of EY.ai.\",\r\n                            \"isDefault\": false,\r\n                            \"ordinal\": 4,\r\n                            \"isActive\": true\r\n                        }\r\n                    ]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantConfigurations",
                keyColumn: "ID",
                keyValue: 2,
                column: "Value",
                value: "\r\n                    [\r\n                        {\r\n                            \"key\": \"project-docs\",\r\n                            \"displayName\": \"Project Docs\",\r\n                            \"originalDisplayName\": \"Project Docs\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Reference information from the project documents that you have access to through SharePoint. Pick and choose which documents you want to focus on via the Document Hub.\",\r\n                            \"isDefault\": false,\r\n                            \"ordinal\": 1,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"project-data\",\r\n                            \"displayName\": \"Project Data\",\r\n                            \"originalDisplayName\": \"Project Data\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Ask questions about how this project is progressing. Examples of key insights to ask about include identifying existing risks, issues, actions, decisions, or initiatives that have fallen of track.\",\r\n                            \"isDefault\": false,\r\n                            \"ordinal\": 2,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"ey-guidance\",\r\n                            \"displayName\": \"EY Guidance\",\r\n                            \"originalDisplayName\": \"EY Guidance\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Ask questions against SaT best practices, Capital Edge help, and EY intellectual property such as workplan blueprints, cost saving initiatives, and normative operating models.\",\r\n                            \"isDefault\": true,\r\n                            \"ordinal\": 3,\r\n                            \"isActive\": true\r\n                        },\r\n                        {\r\n                            \"key\": \"internet\",\r\n                            \"displayName\": \"Internet\",\r\n                            \"originalDisplayName\": \"Internet\",\r\n                            \"isQuestionTextBoxEnabled\": true,\r\n                            \"description\": \"Open your search to the entire internet, protected by the wrapper of EY.ai.\",\r\n                            \"isDefault\": true,\r\n                            \"ordinal\": 4,\r\n                            \"isActive\": true\r\n                        }\r\n                    ]");
        }
    }
}
