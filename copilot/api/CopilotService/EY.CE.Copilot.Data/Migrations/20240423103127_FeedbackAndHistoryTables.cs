using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackAndHistoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Suggestions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Suggestions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ChatHistorys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuggestionId = table.Column<long>(type: "bigint", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeToResolve = table.Column<long>(type: "bigint", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatHistorys", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CopilotFeedbacks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FeedbackText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CopilotFeedbacks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MessageFeedbacks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLiked = table.Column<bool>(type: "bit", nullable: false),
                    ChatId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FeedbackText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageFeedbacks", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatHistorys");

            migrationBuilder.DropTable(
                name: "CopilotFeedbacks");

            migrationBuilder.DropTable(
                name: "MessageFeedbacks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Suggestions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Suggestions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8453), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8469) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8473), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8473) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8474), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8475) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8476), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8476) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8477), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8478) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8479), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8480) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8481), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8482) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8483), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8483) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8484), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8485) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8486), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8486) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8487), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8488) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8488), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8489) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8490), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8491) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8491), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8492) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8493), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8493) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8494), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8494) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8495), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8496) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8497), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8498) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8499), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8499) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8500), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8501) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8502), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8502) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8503), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8503) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8504), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8505) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8505), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8506) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8507), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8507) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8508), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8509) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8510), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8510) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8511), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8511) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8512), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8513) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8514), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8514) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8515), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8515) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8516), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8517) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8518), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8518) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8520), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8520) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8521), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8521) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8522), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8523) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8569), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8569) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8570), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8571) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8571), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8572) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8573), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8573) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8574), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8575) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8576), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8576) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8577), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8578) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8578), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8579) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8580), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8580) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8581), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8581) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8582), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8583) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8584), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8584) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8585), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8585) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8587), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8587) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8588), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8588) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8589), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8590) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8590), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8591) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8592), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8592) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8593), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8594) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8595), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8595) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8596), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8596) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8597), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8598) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8599), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8599) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8600), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8600) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8601), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8602) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8602), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8603) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8604), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8604) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8605), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8606) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8607), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8607) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8609), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8610) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8610), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8611) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8612), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8612) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8613), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8613) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8614), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8615) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8615), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8616) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8617), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8617) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8618), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8618) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8619), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8620) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8621), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8621) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8622), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8622) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8623), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8623) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8625), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8625) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8626), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8626) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8627), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8628) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8628), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8629) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8630), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8630) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8631), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8631) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8632), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8633) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8633), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8634) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8635), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8635) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8636), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8637) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8638), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8638) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8639), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8639) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8640), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8640) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8641), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8642) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8643), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8643) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8644), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8644) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8645), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8646) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8647), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8647) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8648), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8648) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8649), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8650) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8650), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8651) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8652), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8652) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8653), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8653) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8654), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8655) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8656), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8656) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8657), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8657) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8658), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8659) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8659), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8660) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8661), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8661) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8662), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8663) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8664), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8664) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8665), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8665) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8666), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8667) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8668), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8668) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8669), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8669) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8670), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8671) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8671), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8672) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8749), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8750) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8751), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8751) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8752), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8753) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8754), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8755) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8755), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8756) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8757), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8757) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8758), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8758) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8759), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8760) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8761), new DateTime(2024, 4, 10, 19, 19, 36, 877, DateTimeKind.Local).AddTicks(8761) });
        }
    }
}
