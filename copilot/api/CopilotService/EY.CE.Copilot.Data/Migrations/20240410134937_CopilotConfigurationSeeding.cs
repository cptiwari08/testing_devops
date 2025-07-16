using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class CopilotConfigurationSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CopilotConfigurations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CopilotConfigurations", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "CopilotConfigurations",
                columns: new[] { "ID", "IsEnabled", "Key", "Title", "Value" },
                values: new object[] { 1, false, "PROJECT_CONTEXT", "Project Context", "" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CopilotConfigurations");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8735), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8744) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8749), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8749) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8750), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8751) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8752), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8752) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8753), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8754) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8756), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8756) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8759), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8759) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8760), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8761) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8762), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8762) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8764), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8764) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8765), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8766) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8767), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8767) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8769), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8769) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8770), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8771) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8771), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8772) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8773), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8773) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8774), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8775) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8777), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8777) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8778), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8779) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8780), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8781) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8782), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8782) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 22,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8783), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8784) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 23,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8785), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8785) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8786), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8787) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8788), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8788) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 26,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8789), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8790) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 27,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8791), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8791) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8792), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8793) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 29,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8794), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8794) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 30,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8795), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8796) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 31,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8797), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8797) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 32,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8798), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8799) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 33,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8800), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8800) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 34,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8802), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8803) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8804), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8804) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8805), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8806) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8807), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8807) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8808), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8809) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8810), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8810) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8811), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8812) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8813), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8813) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8814), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8815) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8816), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8817) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8818), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8818) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8819), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8819) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8821), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8821) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8822), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8823) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8824), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8824) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8825), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8826) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8827), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8828) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8829), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8829) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8830), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8831) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8832), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8832) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8833), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8834) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8835), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8835) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8837), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8837) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8838), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8839) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8840), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8840) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8841), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8842) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8879), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8880) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8881), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8882) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8883), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8883) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8884), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8885) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8886), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8886) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8888), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8888) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8890), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8890) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8891), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8892) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8893), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8893) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8894), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8895) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8896), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8896) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8897), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8898) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8899), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8899) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8900), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8901) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8902), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8902) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8903), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8904) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8905), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8905) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8906), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8906) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8908), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8908) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8909), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8910) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8911), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8911) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8912), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8913) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8914), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8914) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8915), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8916) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8917), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8917) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8918), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8919) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8920), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8921) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8921), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8922) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8923), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8924) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8925), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8925) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8926), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8927) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8928), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8928) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8929), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8930) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8930), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8931) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8932), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8933) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8934), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8934) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8935), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8936) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8937), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8937) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8938), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8939) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8940), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8940) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8941), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8942) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8943), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8944) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8945), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8945) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8946), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8947) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8948), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8948) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8949), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8950) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8951), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8951) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8952), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8953) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8954), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8955) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8956), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8956) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8957), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8958) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8959), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8959) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8960), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8961) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8962), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8962) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8963), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8964) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8965), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8965) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8966), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8967) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8968), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8968) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8970), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8970) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8971), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8972) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8973), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8973) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8974), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8975) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8976), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8976) });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8977), new DateTime(2024, 4, 8, 19, 18, 3, 317, DateTimeKind.Local).AddTicks(8978) });
        }
    }
}
