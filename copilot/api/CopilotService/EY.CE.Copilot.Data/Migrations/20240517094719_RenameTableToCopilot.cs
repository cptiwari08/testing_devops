using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableToCopilot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Suggestions",
                table: "Suggestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageFeedbacks",
                table: "MessageFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CopilotFeedbacks",
                table: "CopilotFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CopilotConfigurations",
                table: "CopilotConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatHistorys",
                table: "ChatHistorys");

            migrationBuilder.RenameTable(
                name: "Suggestions",
                newName: "AssistantSuggestions");

            migrationBuilder.RenameTable(
                name: "MessageFeedbacks",
                newName: "AssistantMessageFeedbacks");

            migrationBuilder.RenameTable(
                name: "CopilotFeedbacks",
                newName: "AssistantFeedbacks");

            migrationBuilder.RenameTable(
                name: "CopilotConfigurations",
                newName: "AssistantConfigurations");

            migrationBuilder.RenameTable(
                name: "ChatHistorys",
                newName: "AssistantChatHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssistantSuggestions",
                table: "AssistantSuggestions",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssistantMessageFeedbacks",
                table: "AssistantMessageFeedbacks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssistantFeedbacks",
                table: "AssistantFeedbacks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssistantConfigurations",
                table: "AssistantConfigurations",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssistantChatHistory",
                table: "AssistantChatHistory",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssistantSuggestions",
                table: "AssistantSuggestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssistantMessageFeedbacks",
                table: "AssistantMessageFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssistantFeedbacks",
                table: "AssistantFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssistantConfigurations",
                table: "AssistantConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssistantChatHistory",
                table: "AssistantChatHistory");

            migrationBuilder.RenameTable(
                name: "AssistantSuggestions",
                newName: "Suggestions");

            migrationBuilder.RenameTable(
                name: "AssistantMessageFeedbacks",
                newName: "MessageFeedbacks");

            migrationBuilder.RenameTable(
                name: "AssistantFeedbacks",
                newName: "CopilotFeedbacks");

            migrationBuilder.RenameTable(
                name: "AssistantConfigurations",
                newName: "CopilotConfigurations");

            migrationBuilder.RenameTable(
                name: "AssistantChatHistory",
                newName: "ChatHistorys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suggestions",
                table: "Suggestions",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageFeedbacks",
                table: "MessageFeedbacks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CopilotFeedbacks",
                table: "CopilotFeedbacks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CopilotConfigurations",
                table: "CopilotConfigurations",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatHistorys",
                table: "ChatHistorys",
                column: "ID");
        }
    }
}
