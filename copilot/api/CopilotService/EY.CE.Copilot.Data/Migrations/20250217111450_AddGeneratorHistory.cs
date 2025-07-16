using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneratorHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssistantGeneratorHistory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstanceId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantGeneratorHistory", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssistantGeneratorHistory");
        }
    }
}
