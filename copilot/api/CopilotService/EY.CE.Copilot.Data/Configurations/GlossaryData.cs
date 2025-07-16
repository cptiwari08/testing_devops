using EY.CE.Copilot.Data.Models;
using EY.CE.Copilot.Data.Static;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EY.CE.Copilot.Data.Configurations
{
    public class GlossaryData : IEntityTypeConfiguration<Glossary>
    {
        public void Configure(EntityTypeBuilder<Glossary> modelBuilder)
        {
            Glossary[] glossaries = new Glossary[]
            {
                new Glossary
                {
                    ID = 1,
                    Context = "When asked for due times, past due, this week, slippage, among other similar, use the column TaskDueDate",
                    TableName = "|WorkPlan|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 2,
                    Context = "When asked for list of tasks, name of task, workplan items, include the columns WorkPlan.Title and WorkPlan.UniqueItemIdentifier in the select clause, and always filter by the workplan task type.",
                    TableName = "|WorkPlan|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 3,
                    Context = "When asked for risks in any case, try to include the columns Title and UniqueItemIdentifier if possible",
                    TableName = "|RisksAndIssues|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 4,
                    Context = "When asked for scope of service use the column ServiceInScopeDescription from the table TSAItems",
                    TableName = "|TSAItems|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 5,
                    Context = "When asked for services use the column Title from table TSAItems",
                    TableName = "|TSAItems|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 6,
                    Context = "When asked for list of TSA ending in a specific month use the column TSAItemEndDate",
                    TableName = "|TSAItems|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 7,
                    Context = "Duration should be calculated by subtracting a StartDate from an EndDate, formula is EndDate - StartDate",
                    TableName = "AllTables",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 8,
                    Context = "For TSA, date columns to be used are  TSAItemEndDate and TSAItemStartDate",
                    TableName = "|TSAItems|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 9,
                    Context = "When asked about plans of initiatives you MUST use the ValueCaptureInitiatives and ValueCaptureEstimates tables",
                    TableName = "|ValueCaptureInitiatives|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 10,
                    Context = "When asked about ALREADY achived initiatives you MUST use ValueCaptureActuals and ValueCaptureInitiatives tables",
                    TableName = "|ValueCaptureInitiatives|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 11,
                    Context = "Team, teams is in the table ProjectTeams in the column Title",
                    TableName = "|WorkPlan|,|RisksAndIssues|,|TSAItems|,|Nodes|,|ValueCaptureIntitiatives|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 12,
                    Context = "Overdue means past due date",
                    TableName = "|WorkPlan|,|RisksAndIssues|,|TSAItems|,|Nodes|,|ValueCaptureIntitiatives|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 13,
                    Context = "When asked about processes it is referring to Operating Model processes, you should use Nodes tables and tables referring to Nodes",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 14,
                    Context = "When asked for Day 1 processes, use operating models tables and, join with TransactionStates and filter [key] column with 'DAY_ONE'",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 15,
                    Context = "When asked for Current State processes, use operating models tables and, join with TransactionStates and filter [key] column with 'CURRENT_STATE'",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 16,
                    Context = "When asked about initiatives use tables related to VC or ValueCapture",
                    TableName = "|ValueCaptureInitiatives|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 17,
                    Context = "When I ask something about me or other person use the table UserProfiles, user name is on column Title, and Email in column Email",
                    TableName = "AllTables",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 18,
                    Context = "If you're asked targets and engagement you MUST use 'ValueCaptureTopDownEstimates' table",
                    TableName = "|ValueCaptureTopDownEstimates|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 19,
                    Context = "When asked for processes by op model, please use the table 'Nodes',use the column Title",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 20,
                    Context = "When asked for ANY NUMBER, current STATE or HOW MANY for op model, please use the table 'Nodes',use the column Title and apply a count",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 21,
                    Context = "When asked for HOW MANY processes and States, please use the table 'Nodes',use the column Title apply a count and use a LEFT with the table TransactionStated",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 22,
                    Context = "When asked for op models 'Systems' with 'Processes', please use the bridge table NodesSystemsForEnablerSystems as main tables",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 23,
                    Context = "When asked for op models 'Assets', please use the bridge table NodesToAssetsForEnablerAssets as main tables",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 24,
                    Context = "When asked for op models 'Processes Dispositions', please use the bridge table NodesToDispositionsForDispositionNew as main tables",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 25,
                    Context = "When asked for op models 'Systems', please use the bridge table NodesSystemsForEnablerSystems as main tables",
                    TableName = "|Nodes|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 26,
                    Context = "A status is Open when is not Cancelled, completed, closed, deleted, rejected or on hold",
                    TableName = "AllTables",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 27,
                    Context = "When asked about 'IT' team or 'IT' project team, please filter using the column Title from the ProjectTeams table in the following way: WHERE ProjectTeams.Title = 'IT'",
                    TableName = "AllTables",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 28,
                    Context = "When asked about something related to Account Disabled, Project Disabled, Project Enabled or similar, use the Key column in the table AccountStatuses",
                    TableName = "|AccountStatuses|",
                    Tag = "static",
                    IsSchema = false
                },
                new Glossary
                {
                    ID = 29,
                    Context = "If you receive information about Team Type join with the ProjectTeams table, then with the TeamTypes table and filter by the ID in table TeamTypes",
                    TableName = "AllTables",
                    Tag = "static",
                    IsSchema = false
                }
            };
        
            

            var glossariesArray = glossaries.ToArray();

            modelBuilder.ToTable("AssistantGlossary");

            modelBuilder.HasData(glossariesArray);
        }
    }
}
