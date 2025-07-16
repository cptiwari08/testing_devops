using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEmailToUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                column: "AnswerSQL",
                value: "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = ${Username} GROUP BY S.Title ");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "AnswerSQL",
                value: "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = ${Username} ");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "AnswerSQL",
                value: "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = ${Username} ");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "AnswerSQL",
                value: "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = ${Username} ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                column: "AnswerSQL",
                value: "SELECT S.Title, COUNT(VI.ID) AS InitiativeCount FROM ValueCaptureStages S LEFT JOIN ValueCaptureInitiatives VI ON S.ID = VI.ValueCaptureStageId LEFT JOIN UserProfiles UP on UP.ID = VI.ItemOwnerId Where UP.EMail = ${Useremail} GROUP BY S.Title ");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "AnswerSQL",
                value: "SELECT RI.Title FROM RisksAndIssuesToValueCaptureInitiativesForValueCaptureInitiativeIDs RIV JOIN ValueCaptureInitiatives VI ON VI.ID = RIV.ValueCaptureInitiativeId JOIN RisksAndIssues RI ON RI.ID = RIV.RisksAndIssueId JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE U.Email = @UserEmail ");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "AnswerSQL",
                value: "select VI.Title from ValueCaptureInitiatives VI LEFT JOIN UserProfiles U on U.ID = VI.ItemOwnerId where U.EMail = ${Useremail} ");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "AnswerSQL",
                value: "SELECT VI.Title  FROM ValueCaptureInitiatives VI LEFT JOIN ValueCaptureTypes VT ON VT.ID = VI.ValueCaptureTypeId LEFT JOIN UserProfiles U ON U.ID = VI.ItemOwnerId WHERE VT.ID = 1 AND U.EMail = ${Useremail} ");
        }
    }
}
