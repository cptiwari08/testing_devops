using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToChatHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ChatHistorys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ChatHistorys");
        }
    }
}
