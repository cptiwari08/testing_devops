using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPunctuationForSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 4,
                column: "SuggestionText",
                value: "List out behind schedule milestones.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 5,
                column: "SuggestionText",
                value: "List out behind schedule risks.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 6,
                column: "SuggestionText",
                value: "List out behind schedule interdependencies.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 9,
                column: "SuggestionText",
                value: "How many interdependencies are due this week?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 10,
                column: "SuggestionText",
                value: "Which project team has the most overdue milestones?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "SuggestionText",
                value: "Show me open risks broken down by impact and probability.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 15,
                column: "SuggestionText",
                value: "How many interdependencies does each provider team have?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "SuggestionText",
                value: "List out workplan items assigned to User.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 19,
                column: "SuggestionText",
                value: "List out Behind Schedule items for HR team.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "SuggestionText",
                value: "List out workplan items due next week.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 21,
                column: "SuggestionText",
                value: "List out sub functions of Finance.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 24,
                column: "SuggestionText",
                value: "List out overdue interdependencies.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 25,
                column: "SuggestionText",
                value: "List out team with most overdue interdependencies.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 26,
                column: "SuggestionText",
                value: "List out Risks with no mitigation plan in place.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 27,
                column: "SuggestionText",
                value: "List out Risks with no owners assigned to them.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 28,
                column: "SuggestionText",
                value: "Show me upcoming milestones that have risks linked to it.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 31,
                column: "SuggestionText",
                value: "What is the parent team of Tax?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "SuggestionText",
                value: "What are my accomplishments from last reporting period?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                column: "SuggestionText",
                value: "List the interdependencies for which I'm the provider.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 35,
                column: "SuggestionText",
                value: "How many issues don't have a mitigation plan?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 36,
                column: "SuggestionText",
                value: "List tasks that have missing owners.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 37,
                column: "SuggestionText",
                value: "How many high risk items are there for IT?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "SuggestionText",
                value: "List all workplan, raid and interdependencies assigned to user.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "SuggestionText",
                value: "Are there any project teams that are 'At Risk' or 'Behind Schedule'?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "SuggestionText",
                value: "List all Open Risks, Issues, Actions and Decisions.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "SuggestionText",
                value: "List all Open Interdependencies.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "SuggestionText",
                value: "List all critical workplan tasks.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "SuggestionText",
                value: "List all Interdependencies where IT is Interdependency Provider.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 51,
                column: "SuggestionText",
                value: "List all Interdependencies where IT is Interdependency Receiver.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "SuggestionText",
                value: "Show me a list of all users that have behind schedule or at risk items assigned to them.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                column: "SuggestionText",
                value: "Show me all Risks and issues that are flagged as Critical and not complete.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                column: "SuggestionText",
                value: "Can you please list down all tasks whose due date is less than the due date of the associated interdependency?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                column: "SuggestionText",
                value: "List out workplan items due this week.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                column: "SuggestionText",
                value: "Provide details for workplan ID HR.2.2.6.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                column: "SuggestionText",
                value: "How many issues are in pending status?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                column: "SuggestionText",
                value: "List RAID items that do not have an owner assigned.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                column: "SuggestionText",
                value: "List all Workplan tasks that are 'Not Started' and planned start date has passed.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                column: "SuggestionText",
                value: "List all Workplan tasks which are past due.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                column: "SuggestionText",
                value: "List all project teams with workplan items that are 'At Risk'.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                column: "SuggestionText",
                value: "How  many initiatives are assigned to me?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                column: "SuggestionText",
                value: "Give my cost reduction initiatives.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "SuggestionText",
                value: "What are the revenue growth targets for Sales & Marketing?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                column: "SuggestionText",
                value: "What are the cost to achieve targets for R&D?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "SuggestionText",
                value: "How many initiatives are there in IT across different stages?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "SuggestionText",
                value: "List IT initiatives that are in approved stage.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                column: "SuggestionText",
                value: "List initiatives that have workplan item linked to them.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                column: "SuggestionText",
                value: "How many initiatives have Risks linked?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                column: "SuggestionText",
                value: "What is the total headcount cost reduction target?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                column: "SuggestionText",
                value: "List initiatives that are not active.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                column: "SuggestionText",
                value: "What is the Total Top Down Target Value?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "Show my top down target values by cost reduction and revenue growth.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "How many active initatives are there in my project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "List initiatives where I'm assigned as the Owner.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "SuggestionText",
                value: "What initatives have I been assigned to?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "SuggestionText",
                value: "List my initatives by stages.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "SuggestionText",
                value: "List my initatives by Value Capture Type.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "SuggestionText",
                value: "List all Cost/Revenue Centers linked to PnL Line Items.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 112,
                column: "SuggestionText",
                value: "How many top down targets do we have for this project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                column: "SuggestionText",
                value: "What is the Total Headcount Cost Reduction Target?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "SuggestionText",
                value: "What is the Total Non Headcount Cost Reduction Target?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "SuggestionText",
                value: "What is the Total Revenue Growth Target?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "SuggestionText",
                value: "What is the Total Cost to Achieve Target?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                column: "SuggestionText",
                value: "Are there any benchmarks for this project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123,
                column: "SuggestionText",
                value: "Can you provide me number of processes by op model?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                column: "SuggestionText",
                value: "How many processes we have in current state?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                column: "SuggestionText",
                value: "How many processes we have in Day1 state?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                column: "SuggestionText",
                value: "How many processes don't have an Owner assigned in Current State?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                column: "SuggestionText",
                value: "List processes that don't have Disposition assigned in Day 1 state.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                column: "SuggestionText",
                value: "List the Disposition options available.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                column: "SuggestionText",
                value: "How many enablers are associated with each Process Group?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                column: "SuggestionText",
                value: "List the total number of systems by disposition.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                column: "SuggestionText",
                value: "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                column: "SuggestionText",
                value: "What Enablers are we tracking for this project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                column: "SuggestionText",
                value: "How many Systems are there in this functional op model?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                column: "SuggestionText",
                value: "How many TPAs are there in this functional op model?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                column: "SuggestionText",
                value: "List the number of Systems by Type.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                column: "SuggestionText",
                value: "List the number of TPAs by Ownership.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                column: "SuggestionText",
                value: "List down all process groups with no process within them.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                column: "SuggestionText",
                value: "List the number of processes across op models.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                column: "SuggestionText",
                value: "List history of changes to the op model in the past one hour.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                column: "SuggestionText",
                value: "List the history of changes to Ownership in the op model today.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                column: "SuggestionText",
                value: "List the history of deletes in the op model this week.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                column: "SuggestionText",
                value: "List the functional operating models across different states.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                column: "SuggestionText",
                value: "Are there any Live Notes in this op model?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                column: "SuggestionText",
                value: "List down all processes which have been renamed in Day 1 op model.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                column: "SuggestionText",
                value: "Give me the list of TSAs by Provider.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                column: "SuggestionText",
                value: "Give me the list of TSAs by Receiver.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                column: "SuggestionText",
                value: "Can you provide me list of TSAs by phases?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                column: "SuggestionText",
                value: "Show me the breakdown of TSAs by duration.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                column: "SuggestionText",
                value: "How many TSAs does each team have?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                column: "SuggestionText",
                value: "List the number of TSAs across different phases.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                column: "SuggestionText",
                value: "How many pending TSA items do I have?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                column: "SuggestionText",
                value: "In Cost Tracking, how is my billing periods setup?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                column: "SuggestionText",
                value: "What are the different TSA phases in my project?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                column: "SuggestionText",
                value: "List the TSA service locations in my project.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                column: "SuggestionText",
                value: "What are the project teams that make up the governance structure for this engagement?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                column: "SuggestionText",
                value: "How many milestones does each team have?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                column: "SuggestionText",
                value: "How many interdependencies does each team have?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                column: "SuggestionText",
                value: "How many risks does each team have?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                column: "SuggestionText",
                value: "Provides templates of TSA.");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 215,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 4,
                column: "SuggestionText",
                value: "List out behind schedule milestones");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 5,
                column: "SuggestionText",
                value: "List out behind schedule risks");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 6,
                column: "SuggestionText",
                value: "List out behind schedule interdependencies");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 9,
                column: "SuggestionText",
                value: "How many interdependencies are due this week");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 10,
                column: "SuggestionText",
                value: "Which project team has the most overdue milestones");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 13,
                column: "SuggestionText",
                value: "Show me open risks broken down by impact and probability");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 15,
                column: "SuggestionText",
                value: "How many interdependencies does each provider team have ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 16,
                column: "SuggestionText",
                value: "List out workplan items assigned to User");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 19,
                column: "SuggestionText",
                value: "List out Behind Schedule items for HR team");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 20,
                column: "SuggestionText",
                value: "List out workplan items due next week");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 21,
                column: "SuggestionText",
                value: "List out sub functions of Finance");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 24,
                column: "SuggestionText",
                value: "List out overdue interdependencies");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 25,
                column: "SuggestionText",
                value: "List out team with most overdue interdependencies");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 26,
                column: "SuggestionText",
                value: "List out Risks with no mitigation plan in place");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 27,
                column: "SuggestionText",
                value: "List out Risks with no owners assigned to them");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 28,
                column: "SuggestionText",
                value: "Show me upcoming milestones that have risks linked to it?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 31,
                column: "SuggestionText",
                value: "What is the parent team of Tax");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 32,
                column: "SuggestionText",
                value: "What are my accomplishments from last reporting period ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 34,
                column: "SuggestionText",
                value: "List the interdependencies for which I'm the provider ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 35,
                column: "SuggestionText",
                value: "How many issues don't have a mitigation plan ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 36,
                column: "SuggestionText",
                value: "List tasks that have missing owners");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 37,
                column: "SuggestionText",
                value: "How many high risk items are there for IT ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 38,
                column: "SuggestionText",
                value: "List all workplan, raid and interdependencies assigned to Amil Shah");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 39,
                column: "SuggestionText",
                value: "Are there any project teams that are 'At Risk' or 'Behind Schedule'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 47,
                column: "SuggestionText",
                value: "List all Open Risks, Issues, Actions and Decisions");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "SuggestionText",
                value: "List all Open Interdependencies");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "SuggestionText",
                value: "List all critical workplan tasks?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "SuggestionText",
                value: "List all Interdependencies where IT is Interdependency Provider");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 51,
                column: "SuggestionText",
                value: "List all Interdependencies where IT is Interdependency Receiver");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "SuggestionText",
                value: "Show me a list of all users that have behind schedule or at risk items assigned to them ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 59,
                column: "SuggestionText",
                value: "Show me all Risks and issues that are flagged as Critical and not complete");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 63,
                column: "SuggestionText",
                value: "Can you please list down all tasks whose due date is less than the due date of the associated interdependency");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 64,
                column: "SuggestionText",
                value: "List out workplan items due this week");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 65,
                column: "SuggestionText",
                value: "Provide details for workplan ID HR.2.2.6");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 66,
                column: "SuggestionText",
                value: "How many issues are in pending status ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 67,
                column: "SuggestionText",
                value: "List RAID items that do not have an owner assigned");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 68,
                column: "SuggestionText",
                value: "List all Workplan tasks that are 'Not Started' and planned start date has passed");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 69,
                column: "SuggestionText",
                value: "List all Workplan tasks which are past due");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 70,
                column: "SuggestionText",
                value: "List all project teams with workplan items that are 'At Risk'");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 80,
                column: "SuggestionText",
                value: "How  many initiatives are assigned to me? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 81,
                column: "SuggestionText",
                value: "Give my cost reduction initiatives ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "SuggestionText",
                value: "What are the revenue growth targets for Sales & Marketing ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 83,
                column: "SuggestionText",
                value: "What are the cost to achieve targets for R&D ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "SuggestionText",
                value: "How many initiatives are there in IT across different stages ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "SuggestionText",
                value: "List IT initiatives that are in approved stage");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 86,
                column: "SuggestionText",
                value: "List initiatives that have workplan item linked to them");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 87,
                column: "SuggestionText",
                value: "How many initiatives have Risks linked ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 88,
                column: "SuggestionText",
                value: "What is the total headcount cost reduction target ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 89,
                column: "SuggestionText",
                value: "List initiatives that are not active");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 91,
                column: "SuggestionText",
                value: "What is the Total Top Down Target Value ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "Show my top down target values by cost reduction and revenue growth");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "How many active initatives are there in my project ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "List initiatives where I'm assigned as the Owner");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 104,
                column: "SuggestionText",
                value: "What initatives have I been assigned to ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 105,
                column: "SuggestionText",
                value: "List my initatives by stages");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "SuggestionText",
                value: "List my initatives by Value Capture Type");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 110,
                column: "SuggestionText",
                value: "List all Cost/Revenue Centers linked to PnL Line Items");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 112,
                column: "SuggestionText",
                value: "How many top down targets do we have for this project ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 113,
                column: "SuggestionText",
                value: "What is the Total Headcount Cost Reduction Target ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "SuggestionText",
                value: "What is the Total Non Headcount Cost Reduction Target ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "SuggestionText",
                value: "What is the Total Revenue Growth Target ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "SuggestionText",
                value: "What is the Total Cost to Achieve Target ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 117,
                column: "SuggestionText",
                value: "Are there any benchmarks for this project ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 123,
                column: "SuggestionText",
                value: "Can you provide me number of processes by op model? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 124,
                column: "SuggestionText",
                value: "How many processes we have in current state? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 125,
                column: "SuggestionText",
                value: "How many processes we have in Day1 state? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 126,
                column: "SuggestionText",
                value: "How many processes don't have an Owner assigned in Current State ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 127,
                column: "SuggestionText",
                value: "List processes that don't have Disposition assigned in Day 1 state");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 128,
                column: "SuggestionText",
                value: "List the Disposition options available ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 129,
                column: "SuggestionText",
                value: "How many enablers are associated with each Process Group ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 130,
                column: "SuggestionText",
                value: "List the total number of systems by disposition");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 133,
                column: "SuggestionText",
                value: "Can you please compare the Current state & day 1 operating model and list all processes which are missing in Day1 as compared to current state");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 134,
                column: "SuggestionText",
                value: "What Enablers are we tracking for this project ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 135,
                column: "SuggestionText",
                value: "How many Systems are there in this functional op model ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 136,
                column: "SuggestionText",
                value: "How many TPAs are there in this functional op model ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 137,
                column: "SuggestionText",
                value: "Lsit the number of Systems by Type");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 138,
                column: "SuggestionText",
                value: "List the number of TPAs by Ownership");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 139,
                column: "SuggestionText",
                value: "List down all process groups with no process within them?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 140,
                column: "SuggestionText",
                value: "List the number of processes across op models");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 141,
                column: "SuggestionText",
                value: "List history of changes to the op model in the past one hour");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 142,
                column: "SuggestionText",
                value: "List the history of changes to Ownership in the op model today");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 143,
                column: "SuggestionText",
                value: "List the history of deletes in the op model this week");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 144,
                column: "SuggestionText",
                value: "List the functional operating models across different states");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 145,
                column: "SuggestionText",
                value: "Are there any Live Notes in this op model ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 147,
                column: "SuggestionText",
                value: "List down all processes which have been renamed in Day 1 op model?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 150,
                column: "SuggestionText",
                value: "Give me the list of TSAs by Provider ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 151,
                column: "SuggestionText",
                value: "Give me the list of TSAs by Receiver ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 152,
                column: "SuggestionText",
                value: "Can you provide me list of TSAs by phases? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 153,
                column: "SuggestionText",
                value: "Show me the breakdown of TSAs by duration  ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 154,
                column: "SuggestionText",
                value: "How many TSAs does each team have? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 155,
                column: "SuggestionText",
                value: "List the number of TSAs across different phases ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 156,
                column: "SuggestionText",
                value: "How many pending TSA items do I have ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 157,
                column: "SuggestionText",
                value: "In Cost Tracking, how is my billing periods setup ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 158,
                column: "SuggestionText",
                value: "What are the different TSA phases in my project ?");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 159,
                column: "SuggestionText",
                value: "List the TSA service locations in my project");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 160,
                column: "SuggestionText",
                value: "What are the project teams that make up the governance structure for this engagement? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 161,
                column: "SuggestionText",
                value: "How many milestones does each team have? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 162,
                column: "SuggestionText",
                value: "How many interdependencies does each team have ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 163,
                column: "SuggestionText",
                value: "How many risks does each team have? ");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 171,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 179,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 185,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY has supported in the past");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 214,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 215,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "AssistantSuggestions",
                keyColumn: "ID",
                keyValue: 216,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs");
        }
    }
}
