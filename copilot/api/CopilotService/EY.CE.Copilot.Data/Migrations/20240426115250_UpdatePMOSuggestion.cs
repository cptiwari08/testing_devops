using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePMOSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 147);

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 33,
                column: "SuggestionText",
                value: "How many interdependencies does each team have?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                column: "AnswerSQL",
                value: "SELECT wp.Title  from WorkPlan wp  LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND wp.WorkPlanTaskType = 'Milestone'");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                column: "AnswerSQL",
                value: "SELECT RI.Title  from RisksAndIssues RI  LEFT JOIN Statuses s on RI.ItemStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'  AND RI.IssueRiskCategory = 'Risk'");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.Title  from Interdependencies I  LEFT JOIN Statuses s on I.InterdependencyStatusId = s.ID  where s.[Key] = 'BEHIND_SCHEDULE'", "List out behind schedule interdependencies" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID LEFT JOIN UserProfiles up on wp.TaskOwnerId = up.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = ${Username}", "How many of my milestones are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID LEFT JOIN UserProfiles up on RI.ItemOwnerId = up.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'))   AND up.EMail = ${Username}", "How many of my risks are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      YEAR(I.ItemDueDate) = YEAR(GETDATE())     AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'));", "How many interdependencies are due this week" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueMilestoneCount FROM      WorkPlan wp INNER JOIN      ProjectTeams pt ON wp.ProjectTeamId = pt.ID LEFT JOIN      statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE      wp.WorkPlanTaskType = 'Milestone'     AND wp.TaskDueDate < GETDATE()     AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY      OverdueMilestoneCount DESC", "Which project team has the most overdue milestones" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY ", "Which project team has the most overdue risks?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamTitle,     COUNT(*) AS OverdueDependencyCount FROM      Interdependencies I INNER JOIN      ProjectTeams ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      I.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      ReceiverTeam.Title ORDER BY      OverdueDependencyCount DESC;", "Which project team has the most overdue interdependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      RiskImpact,     RiskProbability,     COUNT(*) AS RiskCount FROM      RisksAndIssues WHERE      IssueRiskCategory = 'Risk'     AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED'))  AND (RiskImpact IS NOT NULL   OR RiskProbability IS NOT NULL  ) GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability;", "Show me open risks broken down by impact and probability" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "What are our targets for this engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "How much value are we planning to achieve from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { null, "CE4-VC", "How much value have we already achieved from our initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "SuggestionText",
                value: "How are my initiatives doing?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "SuggestionText",
                value: "Are there any risks or issues with initiatives that I’m responsible for?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "SuggestionText",
                value: "Which initiatives have fallen off track?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                column: "SuggestionText",
                value: "Are our initiative projections exceeding our targets?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                column: "SuggestionText",
                value: "How will these initiatives impact the PnL?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                column: "SuggestionText",
                value: "What team has the most Current State processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                column: "SuggestionText",
                value: "What team has the most Day 1 processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "SuggestionText",
                value: "What are some of the processes that have High impact Risk/Issue linked?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                column: "SuggestionText",
                value: "Show me the count of TSAs by function.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                column: "SuggestionText",
                value: "Show me the count of TSAs by sub function.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                column: "SuggestionText",
                value: "Show me the count of TSAs by region.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                column: "SuggestionText",
                value: "How many of the TSAs are behind schedule?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                column: "SuggestionText",
                value: "List out TSAs that have high impact risks or issues associated with them.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                column: "SuggestionText",
                value: "Show me the breakdown of TSAs by duration");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "Provide a summary of Day 1 process dispositions." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "How many systems are tagged to Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "How many systems are tagged to Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                column: "SuggestionText",
                value: "What team has the most Current State processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                column: "SuggestionText",
                value: "What team has the most Day 1 processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                column: "SuggestionText",
                value: "What are some of the processes that have High impact Risk/Issue linked?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "ey-guidance", "What is the difference between Progress and Calculated Status?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                column: "SuggestionText",
                value: "How do you hide a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                column: "SuggestionText",
                value: "How do you rename a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                column: "SuggestionText",
                value: "How to export report to PDF?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                column: "SuggestionText",
                value: "How do I add a client user to the PMO app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this ${ProjectType}");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What cost savings levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What revenue growth levers are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical one-time costs that we should be considering?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "What are the IT cost savings levers that I should be thinking about?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                column: "SuggestionText",
                value: "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "Help understand VC methodology.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What normative operating models are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How can I upload systems in bulk to the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are the steps to setup the Op Model app?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                column: "SuggestionText",
                value: "Can I import an existing Operating Model in PowerPoint to use as my current state model?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "What reports are available in the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                column: "SuggestionText",
                value: "What is a Process Group?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                column: "SuggestionText",
                value: "How to view side by side view of current State/Day 1/Future state?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What TSAs would you suggest?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are examples for TSAs?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Why are TSAs important?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "SuggestionText",
                value: "What are the most common types of TSAs?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                column: "SuggestionText",
                value: "Provide case studies/credentials for similar deals that EY have supported in the past");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                column: "SuggestionText",
                value: "How to configure TSA stages?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "SuggestionText",
                value: "How to set alerts/send email to TSA owners");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What PMO workplan templates are available?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the best practices to run weekly status meetings with the client?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "How do you add a new field to the workplan?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "SuggestionText",
                value: "How do I link a Workplan Task to RAID?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "SuggestionText",
                value: "Help me understand PMO methodology for this ${ProjectType}");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PMO", "internet", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                column: "SuggestionText",
                value: "When was the deal announced?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                column: "SuggestionText",
                value: "What are some of the similar deals that have happened in the past?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best cost saving initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best revenue growth initiatives?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best strategies for improving a company in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 127,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 128,
                column: "SuggestionText",
                value: "What are the best ways to track actuals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 129,
                column: "SuggestionText",
                value: "What should be the frequency of tracking dollar values during the engagement?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 130,
                column: "SuggestionText",
                value: "What are typical implications for cross border deals?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a normative operating model for the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are key considerations when defining an operating model for a ${Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the corporate functions typically involved in the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What are the typical services of the Sales and Marketing function?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 137,
                column: "SuggestionText",
                value: "Provides templates of TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 138,
                column: "SuggestionText",
                value: "What should be the typical duration for TSA");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 139,
                column: "SuggestionText",
                value: "Things I should keep in mind for longer duration TSAs");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "Generate a basic workplan template for my project." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the key risks for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PL", "What are the key milestones for a ${ProjectType} project?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 143,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 144,
                column: "SuggestionText",
                value: "What are the best practices to build workplan, and track dependencies?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 33,
                column: "SuggestionText",
                value: "How many dependencies does each team have?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 35,
                column: "AnswerSQL",
                value: "SELECT wp.Title from WorkPlan wp LEFT JOIN Statuses s on wp.WorkPlanTaskStatusId = s.ID where s.[Key] = 'BEHIND_SCHEDULE'");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 36,
                column: "AnswerSQL",
                value: "SELECT RI.Title from RisksAndIssues RI LEFT JOIN Statuses s on RI.ItemStatusId = s.ID where s.[Key] = 'BEHIND_SCHEDULE'");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 37,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT I.Title from Interdependencies I LEFT JOIN Statuses s on I.InterdependencyStatusId = s.ID where s.[Key] = 'BEHIND_SCHEDULE'", "List out behind schedule dependencies" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 38,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "How many milestones were assigned to me this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 39,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "How many risks were assigned to me this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 40,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { null, "How many dependencies were assigned to me this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 41,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS MilestonesDueThisWeek FROM WorkPlan wp LEFT JOIN statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE wp.WorkPlanTaskType = 'Milestone'   AND YEAR(wp.TaskDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, wp.TaskDueDate) = DATEPART(WEEK, GETDATE())   AND (s.[Key] IS NULL OR s.[Key]  NOT IN ('COMPLETED', 'CLOSED'));", "How many of my milestones are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 42,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT COUNT(*) AS RisksAndIssuesDueThisWeek FROM RisksAndIssues RI LEFT JOIN statuses S ON RI.ItemStatusId = S.ID WHERE RI.IssueRiskCategory = 'Risk'   AND YEAR(RI.ItemDueDate) = YEAR(GETDATE())   AND DATEPART(WEEK, RI.ItemDueDate) = DATEPART(WEEK, GETDATE())   AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'));", "How many risks are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 43,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT      COUNT(*) AS InterdependenciesDueThisWeek FROM      Interdependencies I LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      YEAR(I.ItemDueDate) = YEAR(GETDATE())     AND DATEPART(WEEK, I.ItemDueDate) = DATEPART(WEEK, GETDATE())     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED'));", "How many dependencies are due this week?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 44,
                columns: new[] { "AnswerSQL", "SuggestionText" },
                values: new object[] { "SELECT     pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueMilestoneCount FROM      WorkPlan wp INNER JOIN      ProjectTeams pt ON wp.ProjectTeamId = pt.ID LEFT JOIN      statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE      wp.WorkPlanTaskType = 'Milestone'     AND wp.TaskDueDate < GETDATE()     AND (s.[Key] IS NULL OR s.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY      OverdueMilestoneCount DESC", "Which project team has the most overdue milestones?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 45,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(*) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY ", "CE4-PMO", "Which project team has the most overdue risks?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 46,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      ReceiverTeam.Title AS ReceiverTeamTitle,     COUNT(*) AS OverdueDependencyCount FROM      Interdependencies I INNER JOIN      ProjectTeams ReceiverTeam ON I.ReceiverProjectTeamId = ReceiverTeam.ID LEFT JOIN      statuses S ON I.InterdependencyStatusId = S.ID WHERE      I.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      ReceiverTeam.Title ORDER BY      OverdueDependencyCount DESC;", "CE4-PMO", "Which project team has the most overdue dependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 47,
                columns: new[] { "AnswerSQL", "AppAffinity", "SuggestionText" },
                values: new object[] { "SELECT      RiskImpact,     RiskProbability,     COUNT(*) AS RiskCount FROM      RisksAndIssues WHERE      IssueRiskCategory = 'Risk'     AND ItemStatusId NOT IN (SELECT ID FROM statuses WHERE [Key] IN ('COMPLETED', 'CLOSED')) AND (RiskImpact IS NOT NULL  OR RiskProbability IS NOT NULL ) GROUP BY      RiskImpact,     RiskProbability ORDER BY      RiskImpact, RiskProbability;", "CE4-PMO", "Show me open risks broken down by impact and probability" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 48,
                column: "SuggestionText",
                value: "What are our targets for this engagement?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 49,
                column: "SuggestionText",
                value: "How much value are we planning to achieve from our initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 50,
                column: "SuggestionText",
                value: "How much value have we already achieved from our initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 51,
                column: "SuggestionText",
                value: "How are my initiatives doing?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 52,
                column: "SuggestionText",
                value: "Are there any risks or issues with initiatives that I’m responsible for?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 53,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Which initiatives have fallen off track?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 54,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Are our initiative projections exceeding our targets?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 55,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "How will these initiatives impact the PnL?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 56,
                column: "SuggestionText",
                value: "Provide a summary of Day 1 process dispositions.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 57,
                column: "SuggestionText",
                value: "How many systems are tagged to Current State processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 58,
                column: "SuggestionText",
                value: "How many systems are tagged to Day 1 processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 59,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What team has the most Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 60,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What team has the most Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 61,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are some of the processes that have High impact Risk/Issue linked?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 62,
                column: "SuggestionText",
                value: "What are the corporate functions typically involved in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 63,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 64,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 65,
                column: "SuggestionText",
                value: "Show me the count of TSAs by function.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 66,
                column: "SuggestionText",
                value: "Show me the count of TSAs by sub function.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 67,
                column: "SuggestionText",
                value: "Show me the count of TSAs by region.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 68,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "How many of the TSAs are behind schedule?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 69,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "List out TSAs that have high impact risks or issues associated with them." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 70,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Show me the breakdown of TSAs by duration" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 71,
                column: "SuggestionText",
                value: "Provide a summary of Day 1 process dispositions.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 72,
                column: "SuggestionText",
                value: "How many systems are tagged to Current State processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 73,
                column: "SuggestionText",
                value: "How many systems are tagged to Day 1 processes?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 74,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "What team has the most Current State processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 75,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "What team has the most Day 1 processes?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 76,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "project-data", "What are some of the processes that have High impact Risk/Issue linked?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 77,
                column: "SuggestionText",
                value: "What PMO workplan templates are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 78,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 79,
                column: "SuggestionText",
                value: "What is the difference between Progress and Calculated Status?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 80,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 81,
                column: "SuggestionText",
                value: "How do you hide a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 82,
                column: "SuggestionText",
                value: "How do you rename a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 83,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 84,
                column: "SuggestionText",
                value: "How to export report to PDF?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 85,
                column: "SuggestionText",
                value: "How do I set alerts/send email for various item owners?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 86,
                column: "SuggestionText",
                value: "How do I add a client user to the PMO app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 87,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 88,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 89,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 90,
                column: "SuggestionText",
                value: "What cost savings levers are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 91,
                column: "SuggestionText",
                value: "What revenue growth levers are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 92,
                column: "SuggestionText",
                value: "What are typical one-time costs that we should be considering?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 93,
                column: "SuggestionText",
                value: "What are the IT cost savings levers that I should be thinking about?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 94,
                column: "SuggestionText",
                value: "What are the one-time costs I should be expecting to incur for Supply Chain related initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 95,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "How do we evaluate our initiatives against one another to create an objective prioritization of what should be done first?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 96,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 97,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "Help understand VC methodology." });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 98,
                column: "SuggestionText",
                value: "What normative operating models are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 99,
                column: "SuggestionText",
                value: "How can I upload systems in bulk to the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 100,
                column: "SuggestionText",
                value: "What are the steps to setup the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 101,
                column: "SuggestionText",
                value: "Can I import an existing Operating Model in PowerPoint to use as my current state model?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 102,
                column: "SuggestionText",
                value: "What reports are available in the Op Model app?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 103,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 104,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a Process Group?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 105,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "How to view side by side view of current State/Day 1/Future state?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 106,
                column: "SuggestionText",
                value: "What TSAs would you suggest?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 107,
                column: "SuggestionText",
                value: "What are examples for TSAs?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 108,
                column: "SuggestionText",
                value: "Why are TSAs important?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 109,
                column: "SuggestionText",
                value: "What are the most common types of TSAs?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 110,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provide case studies/credentials for similar deals that EY have supported in the past" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 111,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "How to configure TSA stages?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 112,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "How to set alerts/send email to TSA owners" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 113,
                column: "SuggestionText",
                value: "What PMO workplan templates are available?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 114,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings with the client?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 115,
                column: "SuggestionText",
                value: "How do you add a new field to the workplan?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 116,
                column: "SuggestionText",
                value: "How to export report to PPT?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 117,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "How do I set alerts/send email for various item owners?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 118,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "How do I link a Workplan Task to RAID?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 119,
                columns: new[] { "AppAffinity", "Source", "SuggestionText" },
                values: new object[] { "CE4-PL", "ey-guidance", "Help me understand PMO methodology for this ${ProjectType}" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 120,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 121,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 122,
                column: "SuggestionText",
                value: "What are the key milestones for a ${ProjectType} project?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 123,
                column: "SuggestionText",
                value: "What are the best practices to run weekly status meetings?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 124,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "When was the deal announced?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 125,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are some of the similar deals that have happened in the past?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 126,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-PMO", "What are the best practices to build workplan, and track dependencies?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 127,
                column: "SuggestionText",
                value: "What are the best cost saving initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 128,
                column: "SuggestionText",
                value: "What are the best revenue growth initiatives?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 129,
                column: "SuggestionText",
                value: "What are the best strategies for improving a company in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 130,
                column: "SuggestionText",
                value: "What are recent examples of improvements being made in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 131,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are the best ways to track actuals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 132,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What should be the frequency of tracking dollar values during the engagement?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 133,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-VC", "What are typical implications for cross border deals?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 134,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What is a normative operating model for the ${Sector} sector?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 135,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are key considerations when defining an operating model for a ${Sector} sector company?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 136,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-OM", "What are examples of Day 1 process dispositions?" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 137,
                column: "SuggestionText",
                value: "What are the corporate functions typically involved in the ${Sector} sector?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 138,
                column: "SuggestionText",
                value: "What are the typical services of the Sales and Marketing function?");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 139,
                column: "SuggestionText",
                value: "Draft a TSA for the Groceries Retail sector, Merchandising and Product Management function, and Endcap and Display Management service.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 140,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Provides templates of TSA" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 141,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "What should be the typical duration for TSA" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 142,
                columns: new[] { "AppAffinity", "SuggestionText" },
                values: new object[] { "CE4-TSA", "Things I should keep in mind for longer duration TSAs" });

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 143,
                column: "SuggestionText",
                value: "Generate a basic workplan template for my project.");

            migrationBuilder.UpdateData(
                table: "Suggestions",
                keyColumn: "ID",
                keyValue: 144,
                column: "SuggestionText",
                value: "What are the key risks for a ${ProjectType} project?");

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "ID", "AnswerSQL", "AppAffinity", "CreatedAt", "CreatedBy", "Source", "SuggestionText", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 145, null, "CE4-PL", null, "System", "internet", "What are the key milestones for a ${ProjectType} project?", null, "System" },
                    { 146, null, "CE4-PL", null, "System", "internet", "What are the best practices to run weekly status meetings?", null, "System" },
                    { 147, null, "CE4-PL", null, "System", "internet", "What are the best practices to build workplan, and track dependencies?", null, "System" }
                });
        }
    }
}
