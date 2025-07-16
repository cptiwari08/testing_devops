using EY.CE.Copilot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EY.CE.Copilot.Data.Configurations
{
    public class ContentGenerationData : IEntityTypeConfiguration<ContentGeneratorQuery>
    {
        public void Configure(EntityTypeBuilder<ContentGeneratorQuery> modelBuilder)
        {
            ContentGeneratorQuery[] data =
            {
                new ContentGeneratorQuery
                {
                    ID = 1,
                    APP = AppType.PMO,
                    Title="Project Status Executive Summary Workplan",
                    Description="The workplan section specifically targets milestones that require immediate attention. The system filters milestones based on the following criteria:Timeframe: Next 7 daysCalculated Status: Not CompletedPriority: High, Medium, etc.Reporting Level: Executive, etc.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      WP.Title,      WP.TaskDueDate,      WP.ReportingLevelId,      WP.Priority,      WP.WorkPlanTaskType,      CASE          WHEN WP.Priority = '(1) High' THEN 1         WHEN WP.Priority = '(2) Normal' THEN 2         WHEN WP.Priority = '(3) Low' THEN 3         WHEN WP.Priority IS NULL OR WP.Priority= '' THEN 4     END AS PriorityRank,     CASE          WHEN WP.TaskDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN WP.TaskDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title as StatusTitle FROM Workplan WP JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task') AND S.[Key] NOT IN ('CANCELLED') AND WP.HierarchyLevel <= 3 AND WP.TaskDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}}) AND WP.ProjectTeamId = {{ProjectTeam}}),HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.TaskDueDate,  PT.ReportingLevelId,  PT.Priority,  PT.WorkPlanTaskType, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="EXECUTIVE_SUMMARY_WORKPLAN"
                },
                new ContentGeneratorQuery
                {
                    ID = 2,
                    APP = AppType.PMO,
                    Title="Project Status Executive Summary Risks and Issues",
                    Description="The system identifies high-priority risks and issues that need to be addressed promptly. The criteria for this search are:Timeframe: Whole history and next 7 daysCalculated Status: Not CompletedPriority: High or CriticalReporting Level: Executive",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      RAI.IssueRiskCategory,      RAI.ItemStatusId ,      RAI.ItemDescription,      RAI.ItemDueDate,      RAI.ItemPriority,      RAI.Title,      RAI.ReportingLevelId,      CASE          WHEN RAI.ItemPriority = 'Critical' THEN 1         WHEN RAI.ItemPriority = 'High' THEN 2         WHEN RAI.ItemPriority = 'Medium' THEN 3         WHEN RAI.ItemPriority = 'Low' THEN 4         WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  RAI.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN RAI.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM RisksAndIssues RAI JOIN Statuses S ON RAI.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED') AND RAI.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND RAI.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.IssueRiskCategory,  PT.ItemStatusId ,  PT.ItemDescription,  PT.ItemDueDate,  PT.ItemPriority,  PT.Title,  PT.ReportingLevelId, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="EXECUTIVE_SUMMARY_RISKS_ISSUES"
                },
                new ContentGeneratorQuery
                {
                    ID = 3,
                    APP = AppType.PMO,
                    Title="Project Status Executive Summary Actions",
                    Description="Actions are tasks or steps that need to be taken to move the project forward. The system searches for actions using the following parameters:Timeframe: Whole history and next 7 daysProgress Status: Not CompletedPriority: High or CriticalReporting Level: Executive",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      ACT.Title,      ACT.ItemDescription,      ACT.ProjectTeamId,      ACT.ItemPriority,      ACT.ItemDueDate,      ACT.ReportingLevelId,      CASE          WHEN ACT.ItemPriority = 'Critical' THEN 1         WHEN ACT.ItemPriority = 'High' THEN 2         WHEN ACT.ItemPriority = 'Medium' THEN 3         WHEN ACT.ItemPriority = 'Low' THEN 4         WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  ACT.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ACT.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Actions ACT JOIN Statuses S ON ACT.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED') AND ACT.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND ACT.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="EXECUTIVE_SUMMARY_ACTIONS"
                },
                new ContentGeneratorQuery
                {
                    ID = 4,
                    APP = AppType.PMO,
                    Title="Project Status Executive Summary Decisions",
                    Description="Decisions are critical choices that need to be made regarding the project's direction. The system searches for decisions that are pending using these criteria:Timeframe: Whole history and next 7 daysCalculated Status: Not CompletedPriority: High or CriticalReporting Level: Executive",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      DCS.Title,      DCS.ItemDescription,      DCS.ProjectTeamId,      DCS.ItemPriority,      DCS.ItemDueDate,      DCS.ReportingLevelId,      CASE          WHEN DCS.ItemPriority = 'Critical' THEN 1         WHEN DCS.ItemPriority = 'High' THEN 2         WHEN DCS.ItemPriority = 'Medium' THEN 3         WHEN DCS.ItemPriority = 'Low' THEN 4         WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  DCS.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN DCS.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Decisions DCS JOIN Statuses S ON DCS.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED') AND DCS.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND DCS.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="EXECUTIVE_SUMMARY_DECISIONS"
                },
                new ContentGeneratorQuery
                {
                    ID = 5,
                    APP = AppType.PMO,
                    Title="Project Status Executive Summary Interdependencies",
                    Description="The system identifies high-priority interdependencies that need to be addressed promptly. The criteria for this search are: Timeframe: Whole history and next 7 days Calculated Status: Not Completed Priority: High or Critical",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH FilteredInterdependencies AS ( SELECT      ID.Title,      ID.ProviderProjectTeamId,      ID.ReceiverProjectTeamId,      ID.ItemDueDate,      ID.ReportingLevelId,     CASE          WHEN  ID.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ID.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Interdependencies ID JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id  WHERE S.[Key] NOT IN ('CANCELLED') AND ID.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}}) ) SELECT  Title,  ProviderProjectTeamId,  ReceiverProjectTeamId,  ItemDueDate, ItemCategory, StatusTitle FROM FilteredInterdependencies;  ",
                    Key="EXECUTIVE_SUMMARY_INTERDEPENDENCIES"
                },
                new ContentGeneratorQuery
                {
                    ID = 6,
                    APP = AppType.PMO,
                    Title="Project Status Accomplishments Workplan",
                    Description="The Workplan section highlights the milestones or tasks that have been successfully completed in the past week. Filters include: Timeframe: Last 7 days Calculated Status: Closed Priority: High Reporting Level: Executive or Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      WP.Title,      WP.TaskDueDate,      WP.ReportingLevelId,      WP.Priority,      WP.WorkPlanTaskType,      CASE          WHEN WP.Priority = '(1) High' THEN 1         WHEN WP.Priority = '(2) Normal' THEN 2         WHEN WP.Priority = '(3) Low' THEN 3         WHEN WP.Priority IS NULL OR  WP.Priority = '' THEN 4     END AS PriorityRank,     CASE          WHEN  WP.TaskDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN WP.TaskDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Workplan WP JOIN Statuses S ON WP.WorkPlanTaskStatusId= S.Id WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task') AND S.[Key]  IN ('CLOSED', 'COMPLETED') AND WP.TaskDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND WP.HierarchyLevel <= 3 AND WP.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.TaskDueDate,  PT.ReportingLevelId,  PT.Priority,  PT.WorkPlanTaskType, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="ACCOMPLISHMENTS_WORKPLAN"
                },
                new ContentGeneratorQuery
                {
                    ID = 7,
                    APP = AppType.PMO,
                    Title="Project Status Accomplishments Risks and Issues",
                    Description="This section identifies risks and issues resolved within the last 7 days. Filters include: Timeframe: Last 7 days Calculated Status: Closed Priority: High or Critical Reporting Level: Executive or Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      RAI.IssueRiskCategory,      RAI.TimeBasedCalculatedStatusId,      RAI.ItemDescription,      RAI.ItemDueDate,      RAI.ItemPriority,      RAI.Title,      RAI.ReportingLevelId,      CASE          WHEN RAI.ItemPriority = 'Critical' THEN 1         WHEN RAI.ItemPriority = 'High' THEN 2         WHEN RAI.ItemPriority = 'Medium' THEN 3         WHEN RAI.ItemPriority = 'Low' THEN 4         WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  RAI.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN RAI.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM RisksAndIssues RAI JOIN Statuses S ON RAI.ItemStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND RAI.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}      AND RAI.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.IssueRiskCategory,  PT.ItemDescription,  PT.ItemDueDate,  PT.ItemPriority,  PT.Title,  PT.ReportingLevelId, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="ACCOMPLISHMENTS_RISKS_ISSUES"
                },
                new ContentGeneratorQuery
                {
                    ID = 8,
                    APP = AppType.PMO,
                    Title="Project Status Accomplishments Actions",
                    Description="Actions completed in the past week. Filters include: Progress Status: Closed Priority: High or Critical Reporting Level: Executive or Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      ACT.Title,      ACT.ItemDescription,      ACT.ProjectTeamId,      ACT.ItemPriority,      ACT.ItemDueDate,      CASE          WHEN ACT.ItemPriority = 'Critical' THEN 1         WHEN ACT.ItemPriority = 'High' THEN 2         WHEN ACT.ItemPriority = 'Medium' THEN 3         WHEN ACT.ItemPriority = 'Low' THEN 4         WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  ACT.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ACT.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Actions ACT JOIN Statuses S ON ACT.ItemStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND ACT.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}   AND ACT.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;   ",
                    Key="ACCOMPLISHMENTS_ACTIONS"
                },
                new ContentGeneratorQuery
                {
                    ID = 9,
                    APP = AppType.PMO,
                    Title="Project Status Accomplishments Decisions",
                    Description="Decisions made and closed in the past week. Filters include: Calculated Status: Closed Priority: High or Critical Reporting Level: Executive or Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      DCS.Title,      DCS.ItemDescription,      DCS.ProjectTeamId,      DCS.ItemPriority,      DCS.ItemDueDate,      CASE          WHEN DCS.ItemPriority = 'Critical' THEN 1         WHEN DCS.ItemPriority = 'High' THEN 2         WHEN DCS.ItemPriority = 'Medium' THEN 3         WHEN DCS.ItemPriority = 'Low' THEN 4         WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  DCS.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN DCS.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Decisions DCS JOIN Statuses S ON DCS.ItemStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND DCS.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}   AND DCS.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;   ",
                    Key="ACCOMPLISHMENTS_DECISIONS"
                },
                new ContentGeneratorQuery
                {
                    ID = 10,
                    APP = AppType.PMO,
                    Title="Project Status Accomplishments Interdependencies",
                    Description="The system identifies high-priority interdependencies that need to be addressed promptly. The criteria for this search are: Calculated Status: Closed Priority: Critical, High Reporting Level: Executive, Global",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH FilteredInterdependencies AS ( SELECT      ID.Title,      ID.ProviderProjectTeamId,      ID.ReceiverProjectTeamId,      ID.ItemDueDate,      ID.ReportingLevelId,     CASE          WHEN  ID.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ID.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Interdependencies ID JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id WHERE S.[Key] IN ('CLOSED', 'COMPLETED') AND ID.ItemDueDate BETWEEN {{periodStartDate}} AND {{periodEndDate}}   AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}}) ) SELECT  Title,  ProviderProjectTeamId,  ReceiverProjectTeamId,  ItemDueDate, ItemCategory, StatusTitle FROM FilteredInterdependencies;  ",
                    Key="ACCOMPLISHMENTS_INTERDEPENDENCIES"
                },

                new ContentGeneratorQuery
                {
                    ID = 11,
                    APP = AppType.PMO,
                    Title="Project Status Next Steps Workplan",
                    Description="The Workplan section outlines the upcoming milestones or tasks for the next week. Filters include: Timeframe: Next 7 days Calculated Status: On Track Priority: High, Medium, etc. Reporting Level: Executive, Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      WP.Title,      WP.TaskDueDate,      WP.ReportingLevelId,      WP.Priority,      WP.WorkPlanTaskType,     CASE          WHEN WP.Priority = '(1) High' THEN 1         WHEN WP.Priority = '(2) Normal' THEN 2         WHEN WP.Priority = '(3) Low' THEN 3         WHEN WP.Priority IS NULL OR WP.Priority = '' THEN 4     END AS PriorityRank,     CASE          WHEN  WP.TaskDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN WP.TaskDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Workplan WP JOIN Statuses S ON WP.WorkPlanTaskStatusId = S.Id WHERE WP.WorkPlanTaskType IN ('Milestone', 'Task') AND S.[Key] NOT IN ('CLOSED', 'CANCELLED', 'COMPLETED')  AND WP.HierarchyLevel <= 3 AND WP.TaskDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND WP.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.TaskDueDate,  PT.ReportingLevelId,  PT.Priority,  PT.WorkPlanTaskType, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank; ",
                    Key="NEXT_STEPS_WORKPLAN"
                },
                new ContentGeneratorQuery
                {
                    ID = 12,
                    APP = AppType.PMO,
                    Title="Project Status Next Steps Risks and Issues",
                    Description="The system identifies high-priority risks and issues that need to be addressed promptly. Filters include: Timeframe: Next 7 days Calculated Status: On Track Priority: High, Medium, etc. Reporting Level: Executive, Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      RAI.IssueRiskCategory,      S.Title AS ItemStatus,     RAI.ItemDescription,      RAI.ItemDueDate,      RAI.ItemPriority,      RAI.Title,      RAI.ReportingLevelId,     CASE          WHEN RAI.ItemPriority = 'Critical' THEN 1         WHEN RAI.ItemPriority = 'High' THEN 2         WHEN RAI.ItemPriority = 'Medium' THEN 3         WHEN RAI.ItemPriority = 'Low' THEN 4         WHEN RAI.ItemPriority IS NULL OR RAI.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  RAI.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN RAI.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM RisksAndIssues RAI JOIN Statuses S ON RAI.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED') AND RAI.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND RAI.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.IssueRiskCategory,  PT.ItemDescription,  PT.ItemDueDate,  PT.ItemPriority,  PT.Title,  PT.ReportingLevelId, PT.ItemStatus, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="NEXT_STEPS_RISKS_ISSUES"
                },
                new ContentGeneratorQuery
                {
                    ID = 13,
                    APP = AppType.PMO,
                    Title="Project Status Next Steps Actions",
                    Description="Actions are tasks or steps that need to be taken to move the project forward. Filters include: Timeframe: Next 7 days Calculated Status: On Track Priority: High, Medium, etc. Reporting Level: Executive, Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      ACT.Title,      ACT.ItemDescription,      ACT.ProjectTeamId,      ACT.ItemPriority,      ACT.ItemDueDate,      S.Title AS ItemStatus,     CASE          WHEN ACT.ItemPriority = 'Critical' THEN 1         WHEN ACT.ItemPriority = 'High' THEN 2         WHEN ACT.ItemPriority = 'Medium' THEN 3         WHEN ACT.ItemPriority = 'Low' THEN 4         WHEN ACT.ItemPriority IS NULL OR ACT.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  ACT.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ACT.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Actions ACT JOIN Statuses S ON ACT.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED') AND ACT.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND ACT.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemStatus, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;  ",
                    Key="NEXT_STEPS_ACTIONS"
                },
                new ContentGeneratorQuery
                {
                    ID = 14,
                    APP = AppType.PMO,
                    Title="Project Status Next Steps Decisions",
                    Description="Decisions are critical choices that need to be made regarding the project's direction. Filters include: Timeframe: Next 7 days Calculated Status: On Track Priority: High, Medium, etc. Reporting Level: Executive, Global.",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH PriorityTasks AS ( SELECT      DCS.Title,      DCS.ItemDescription,      DCS.ProjectTeamId,      DCS.ItemPriority,      DCS.ItemDueDate,      S.Title AS ItemStatus,     CASE          WHEN DCS.ItemPriority = 'Critical' THEN 1         WHEN DCS.ItemPriority = 'High' THEN 2         WHEN DCS.ItemPriority = 'Medium' THEN 3         WHEN DCS.ItemPriority = 'Low' THEN 4         WHEN DCS.ItemPriority IS NULL OR DCS.ItemPriority = '' THEN 5     END AS PriorityRank,     CASE          WHEN  DCS.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN DCS.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Decisions DCS JOIN Statuses S ON DCS.ItemStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED') AND DCS.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})   AND DCS.ProjectTeamId = {{ProjectTeam}} ), HighestPriority AS ( SELECT MIN(PriorityRank) AS MinPriorityRank FROM PriorityTasks ) SELECT  PT.Title,  PT.ItemDescription,  PT.ProjectTeamId,  PT.ItemPriority,  PT.ItemDueDate, PT.ItemStatus, PT.ItemCategory, PT.StatusTitle FROM PriorityTasks PT JOIN HighestPriority HP ON PT.PriorityRank = HP.MinPriorityRank;   ",
                    Key="NEXT_STEPS_DECISIONS"
                },
                new ContentGeneratorQuery
                {
                    ID = 15,
                    APP = AppType.PMO,
                    Title="Project Status Next Steps Interdependencies",
                    Description="The system identifies high-priority interdependencies that need to be addressed promptly. The criteria for this search are: Calculated Status: On Track Priority: Critical, High, Medium, Low (cascading) Reporting Level: Executive, Global",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH FilteredInterdependencies AS ( SELECT      ID.Title,      ID.ProviderProjectTeamId,      ID.ReceiverProjectTeamId,      ID.ItemDueDate,     S.Title AS ItemStatus,     CASE          WHEN  ID.ItemDueDate <= {{periodEndDate}} THEN 'Was past due during reporting period'          WHEN ID.ItemDueDate > {{periodEndDate}} THEN 'Will be past due next reporting period'      END AS ItemCategory,     S.Title AS StatusTitle FROM Interdependencies ID JOIN InterdependencyStatuses S ON ID.InterdependencyStatusId = S.Id WHERE S.[Key] NOT IN ('CANCELLED', 'CLOSED', 'COMPLETED')    AND ID.ItemDueDate BETWEEN {{periodStartDate}} AND DATEADD(day, 7, {{periodEndDate}})    AND (ID.ProviderProjectTeamId = {{ProjectTeam}} OR ID.ReceiverProjectTeamId = {{ProjectTeam}}) ) SELECT  Title,  ProviderProjectTeamId,  ReceiverProjectTeamId,  ItemDueDate, ItemStatus, ItemCategory, StatusTitle FROM FilteredInterdependencies;  ",
                    Key="NEXT_STEPS_INTERDEPENDENCIES"
                },
                new ContentGeneratorQuery
                {
                    ID = 16,
                    APP = AppType.PMO,
                    Title="Project Status Overall Status",
                    Description="The Project Status section is crucial for understanding the overall health of the project. The system evaluates the project's history to determine the overall status as: - At Risk: If any item is marked as \"At Risk\". - Behind Schedule: If any item is marked as \"Behind Schedule\". - On Track: If no items are marked as \"At Risk\" or \"Behind Schedule\".",
                    GeneratorType=GeneratorType.ProjectStatus,
                    SQLQuery="WITH StatusCheck AS ( SELECT     s.[Key],     s.ID FROM     WorkPlan wp JOIN     Statuses s ON wp.WorkPlanTaskStatusId = s.ID WHERE     wp.ProjectTeamId = {{ProjectTeam}}     AND wp.TaskDueDate < {{periodEndDate}}    AND wp.TaskDueDate >= DATEADD(DAY, -7, {{periodEndDate}}) ), FinalStatus AS ( SELECT     wss.ID,   wss.[Key],     wss.Title FROM     StatusCheck sc JOIN     WeeklyStatusStatuses wss ON sc.[Key] = wss.[Key] ) SELECT CASE     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE')     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'AT_RISK') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'AT_RISK')     WHEN EXISTS (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'ON_TRACK') THEN (SELECT TOP 1 ID FROM FinalStatus WHERE [Key] = 'ON_TRACK')     ELSE NULL END AS StatusId, CASE     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'BEHIND_SCHEDULE')     WHEN EXISTS (SELECT 1 FROM FinalStatus WHERE [Key] = 'AT_RISK') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'AT_RISK')     WHEN EXISTS (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'ON_TRACK') THEN (SELECT TOP 1 Title FROM FinalStatus WHERE [Key] = 'ON_TRACK')     ELSE NULL END AS StatusTitle;",
                    Key="PROJECT_STATUS_OVERALL_STATUS"
                }
            };
            var ContentGeneratorQueryData = data.ToArray();

            modelBuilder.ToTable("AssistantContentGeneratorQueries");

            modelBuilder.HasData(ContentGeneratorQueryData);
        }
    }

    static class AppType
    {
        public const string PMO = "PMO";
    }

    static class GeneratorType
    {
        public const string ProjectStatus = "ProjectStatus";
    }
}