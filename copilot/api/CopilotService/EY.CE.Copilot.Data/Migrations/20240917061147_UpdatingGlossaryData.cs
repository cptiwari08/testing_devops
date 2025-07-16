using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingGlossaryData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 10,
                column: "Context",
                value: "When asked about ALREADY achived initiatives you MUST use ValueCaptureActuals and ValueCaptureInitiatives tables");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 27,
                column: "Context",
                value: "When asked about 'IT' team or 'IT' project team, please filter using the column Title from the ProjectTeams table in the following way: WHERE ProjectTeams.Title = 'IT'");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "Context", "TableName" },
                values: new object[] { "When asked about something related to Account Disabled, Project Disabled, Project Enabled or similar, use the Key column in the table AccountStatuses", "|AccountStatuses|" });

            migrationBuilder.InsertData(
                table: "AssistantGlossary",
                columns: new[] { "ID", "Context", "CreatedAt", "CreatedBy", "IsSchema", "TableName", "Tag", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 29, "If you receive information about Team Type join with the ProjectTeams table, then with the TeamTypes table and filter by the ID in table TeamTypes", null, "System", false, "AllTables", "static", null, "System" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 29);

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 10,
                column: "Context",
                value: "When asked about ALREADY achieved initiatives you MUST use ValueCaptureActuals and ValueCaptureInitiatives tables");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 27,
                column: "Context",
                value: "When asked about 'IT' team or 'IT' project team, please filter using the column Title from ProjectTeams table");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 28,
                columns: new[] { "Context", "TableName" },
                values: new object[] { "If you receive information about Team Type join with the ProjectTeams table, then with the TeamTypes table and filter by the ID in table TeamTypes", "AllTables" });
        }
    }
}
