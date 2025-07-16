using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusCode_Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "AssistantChatHistory",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "AssistantChatHistory");
        }
    }
}
