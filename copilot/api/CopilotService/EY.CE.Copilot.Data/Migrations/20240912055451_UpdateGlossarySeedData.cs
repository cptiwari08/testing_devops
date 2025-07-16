using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGlossarySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 2,
                column: "Context",
                value: "When asked for list of tasks, name of task, workplan items, include the columns WorkPlan.Title and WorkPlan.UniqueItemIdentifier in the select clause, and always filter by the workplan task type.");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 8,
                column: "Context",
                value: "For TSA, date columns to be used are  TSAItemEndDate and TSAItemStartDate");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 17,
                column: "Context",
                value: "When I ask something about me or other person use the table UserProfiles, user name is on column Title, and Email in column Email");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 19,
                column: "Context",
                value: "When asked for processes by op model, please use the table 'Nodes',use the column Title");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 20,
                column: "Context",
                value: "When asked for ANY NUMBER, current STATE or HOW MANY for op model, please use the table 'Nodes',use the column Title and apply a count");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 21,
                column: "Context",
                value: "When asked for HOW MANY processes and States, please use the table 'Nodes',use the column Title apply a count and use a LEFT with the table TransactionStated");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 28,
                column: "Context",
                value: "If you receive information about Team Type join with the ProjectTeams table, then with the TeamTypes table and filter by the ID in table TeamTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 2,
                column: "Context",
                value: "When asked for list of tasks, name of task, workplan items, and want you to list them, use the column WorkPlan.Title always if needed and WorkPlan.UniqueItemIdentifier when possible.");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 8,
                column: "Context",
                value: "For TSA, date columns to be used are TSAItemEndDate and TSAItemStartDate");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 17,
                column: "Context",
                value: "When I ask something about me or other person use the table UserProfiles to filter my email with the column Email or the table ActiveDirectory with columns AuthorID or EditorID");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 19,
                column: "Context",
                value: "When asked for processes by op model, please use the table 'Nodes', use the column Title");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 20,
                column: "Context",
                value: "When asked for ANY NUMBER, current STATE or HOW MANY for op model, please use the table 'Nodes', use the column Title and apply a count");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 21,
                column: "Context",
                value: "When asked for HOW MANY processes and States, please use the table 'Nodes', use the column Title apply a count and use a LEFT with the table TransactionStated");

            migrationBuilder.UpdateData(
                table: "AssistantGlossary",
                keyColumn: "ID",
                keyValue: 28,
                column: "Context",
                value: "If you receive information about Team Type join with the ProjectTeams table, include the filter. The name of the team type is on TeamTypes table in the column Title. Show this column if possible.");
        }
    }
}
