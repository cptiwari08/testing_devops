using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EY.CE.Copilot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssistantPrompts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AssistantPrompts",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "DETECT_MISMATCH", "You are a Strategy and Transactions Senior Partner with 40 years of industry experience at EY.Your task is to detect if there is a mismatch between the target team and the EY IP teams.For example:- If the target team is \"\"IT\"\" and the EY IP teams are: \"\"Information Technology\"\", \"\"IT\"\", \"\"Technology\"\", there is no mismatch.- If the target team is \"\"Cybersecurity\"\" and the EY IP teams are: \"\"IT\"\", \"\"Information Technology\"\", \"\"Technology\"\", there is a mismatch.- If the target team is \"\"Finance\"\" and the EY IP teams are: \"\"Finance\"\", \"\"Finance and Accounting\"\", there is no mismatch.  - If the target team is \"\"Controllership\"\" and the EY IP teams are: \"\"Finance\"\", \"\"Finance and Accounting\"\", there is a mismatch.### The target team is:---------------------  {target_team}---------------------### The EY IP teams are:---------------------{eyip_project_teams}---------------------Based on the information provided, choose the most appropriate option:1. there is a mismatch between the target team and the EY IP teams, or2. there is no mismatch between the target team and the EY IP teams.Answer with the number of the chosen option only, without adding extra information. For example: 1.Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "EXAMPLE_TASK_STRUCTURE", "\"[      {          \"\"summary_task\"\": \"\"Detailed summary task header goes here\"\",          \"\"tasks\"\": [              \"\"Task description 1\"\",              \"\"Task description 2\"\",              \"\"Task description 3\"\",              \"\"Task description 4\"\",              \"\"Task description 5\"\",              \"\"Task description 6\"\",              \"\"Task description 7\"\",              \"\"Task description 8\"\",              \"\"Task description 9\"\",              \"\"Task description 10\"\",              \"\"Task description 11\"\",              \"\"Task description 12\"\",              \"\"Task description 13\"\",              \"\"Task description 14\"\",              \"\"Task description 15\"\",          ],          \"\"milestone\"\": \"\"Milestone description header goes here\"\"      },      {          \"\"summary_task\"\": \"\"Detailed of another summary task goes here\"\",          \"\"tasks\"\": [              \"\"Another task description 1\"\",              \"\"Another task description 2\"\",              \"\"Another task description 3\"\",              \"\"Another task description 4\"\",              \"\"Another task description 5\"\",              \"\"Another task description 6\"\",              \"\"Another task description 7\"\",              \"\"Another task description 8\"\",              \"\"Another task description 9\"\",              \"\"Another task description 10\"\",              \"\"Another task description 11\"\",              \"\"Another task description 12\"\",              \"\"Another task description 13\"\",              \"\"Another task description 14\"\",          ],          \"\"milestone\"\": \"\"Another milestone description\"\"      },      {...},      {...},      {...},  ]\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "EY_IP_DATA_CONSOLIDATION", "\"You are a highly intelligent writer tasked with adapting generic task descriptions into a specific {target_team}'s work plan.The following table contains a list of generic tasks, along with their descriptions and classifications as either tasks, summary tasks, or milestones:----------------------------------------------{ey_ip_data}----------------------------------------------Your task is to transform the provided information into a highly detailed, informative, and specific work plan tailored to the team's responsibilities.The specific project details are as follows:----------------------------------------------{project_outline}----------------------------------------------Ensure that:  - Generate concise and descriptive summary task titles that are informative and formatted like chapter names, avoiding the use of colons or punctuation that implies separation.  - Tasks must be deatiled and specific, providing a clear understanding of the work involved.  - Tasks that do not belong to the team's responsibilities in the current project must be excluded.Format the work plan in JSON, following this example structure:----------------------------------------------{example_structure}----------------------------------------------Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "FILTER_EYIP_TEAM_DATA", "\"You are a Senior Partner in Strategy and Transactions with 40 years of industry experience at EY.You will be provided with general information from EY IP, and your task is to retain any relevant team information while discarding the less relevant ones.### The complete EY IP data is:---------------------{eyip_data}---------------------### The target team is:---------------------{target_team}---------------------Instructions:- Retain any relevant team information from the EY IP data, and remove information you consider irrelevant for the target team.- Provide the cleaned EY IP data, including tasks, summary tasks, and milestones.- Do not include any additional or extra information in your response.Format the team data in JSON, following this example structure:----------------------------------------------{example_structure}----------------------------------------------Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "GENERIC_PROJECT_OUTLINE", "\"goal_summary: The goal of this project is to establish a comprehensive framework that ensures the successful execution and completion of the project. This includes developing a detailed project plan, allocating resources efficiently, ensuring compliance with relevant standards and regulations, and implementing effective project governance and support structures.objective_summary: The objectives of the project are to facilitate the smooth execution of project activities by planning and coordinating tasks, developing efficient processes, ensuring clear roles and responsibilities for team members, providing necessary training and support, and proposing a robust operational model to achieve project goals.project_description_summary: The project involves designing and implementing a structured approach to achieve the project goals. This includes defining project scope, setting up processes and procedures, ensuring compliance with relevant standards, developing strategies to optimize resource utilization, aligning project activities with organizational objectives, and addressing any potential challenges that may arise.success_factors_summary: The success factors include effective resource management, clear communication and coordination among team members, timely completion of project tasks, adherence to project timelines and budgets, continuous monitoring and evaluation of project progress, and ensuring minimal disruption to ongoing operations during the project execution.milestones_summary: The milestones include developing a detailed project plan by Month 1, establishing a project team by Month 2, initiating key project activities by Month 3, completing the first phase of the project by Month 6, conducting a mid-project review by Month 9, finalizing project deliverables by Month 11, and completing the project by Month 12.\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "INTERDEPENDENCIES_QUERY", "select DISTINCT PT.Title as ProjectTeam from ProjectTeams PT JOIN TeamTypes TT on TT.ID = PT.TeamTypeId where TT.[Key] = 'PROJECT_MANAGEMENT' AND PT.ManageWorkPlan = 1" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "PROCESS_INPUT", "\"From the following list of Project Teams names:---------------------{inputs}---------------------Select the name or names that are most related to {target}.Respond with a list of the chosen Project Team names.You should select at least one Project Team name.Format the response in JSON without adding extra information, for example: [\"\"HR\"\", \"\"Human Resources\"\", \"\"HR, Human Resources\"\"].Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "PROJECT_OUTLINE_ENRICHMENT_NO_DOCS", "\"You are a Strategy and Transactions Senior Partner with 40 years of industry experience.Your expertise lies in creating detailed outlines for a project work plan.You will receive a list of elements which composed a generic project outline, each related to different topics: goals, objectives, project descriptions, success factors, and milestones.Additionally, you will be provided with some project context, which could be a guide about extra information related to the project.Your task is to enrich the generic workplan outlines by using the project context.Ensure the outlines are well-structured, logically organized, and cover all the key aspects necessary for effective project planning.You have been provided with the following elements:----------------------------------------------{{{project_outline_elements}project_context: {project_context}}}----------------------------------------------Ensure that:    -The details are clear, easy to understand, and accurately reflect the provided elements.    -Each piece of information captures the essence of the respective topic.    -The resulting information is logically structured and comprehensive.    -The JSON string must be free of any enclosing quotation marks and 'json' keyword.    -The project context will enrich the outline only if it adds significant value to the work plan.Provide your summaries in the following format:{{\"\"goal_summary\"\": \"\"{{Adapt the goal based on the provided project context}}\"\",\"\"objective_summary\"\": \"\"{{Adapt the objective based on the provided project context}}\"\",\"\"project_description_summary\"\": \"\"{{Adapt the project description based on the provided project context}}\"\",\"\"success_factors_summary\"\": \"\"{{Adapt the success factors based on the provided project context}}\"\",\"\"milestones_summary\"\": \"\"{{Adapt the milestones based on the provided project context}}\"\"}}Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "PROJECT_OUTLINE_ENRICHMENT", "\"You are a Strategy and Transactions Senior Partner with 40 years of industry experience.Your expertise lies in creating detailed outlines for a project work plan.You will receive a list of elements, each containing chunks of information related to different topics: goals, objectives, project descriptions, success factors, and milestones.Additionally, you will be provided with some project context, which could be a guide about extra information related to the project.Your task is to synthesize this information into comprehensive outlines that will serve as the foundation for building a complete work plan.Ensure the outlines are well-structured, logically organized, and cover all the key aspects necessary for effective project planning.You have been provided with the following elements:----------------------------------------------{{goal_elements: {goal_elements}objective_elements: {objective_elements}project_description_elements: {project_description_elements}success_factors_elements: {success_factors_elements}milestones_elements: {milestone_elements}project_context: {project_context}}}----------------------------------------------Ensure that:    -The details are clear, easy to understand, and accurately reflect the provided elements.    -Each piece of information captures the essence of the respective topic.    -The resulting information is logically structured and comprehensive.    -The JSON string must be free of any enclosing quotation marks and 'json' keyword.    -The project context will enrich the outline only if it adds significant value to the work plan.Provide your summaries in the following format:{{\"\"goal_summary\"\": \"\"{{Summarize the goal based on the provided elements}}\"\",\"\"objective_summary\"\": \"\"{{Summarize the objective based on the provided elements}}\"\",\"\"project_description_summary\"\": \"\"{{Summarize the project description based on the provided elements}}\"\",\"\"success_factors_summary\"\": \"\"{{Summarize the success factors based on the provided elements}}\"\",\"\"milestones_summary\"\": \"\"{{Summarize the milestones based on the provided elements}}\"\"}}Example:input:{{goal_elements: [\"\"Increase market share by 10%\"\", \"\"Expand into new regions\"\", \"\"Enhance brand recognition\"\"]objective_elements: [\"\"Launch new product line\"\", \"\"Improve customer satisfaction\"\", \"\"Reduce operational costs\"\"]project_description_elements: [\"\"Develop a marketing strategy\"\", \"\"Conduct market research\"\", \"\"Implement new sales tactics\"\"]success_factors_elements: [\"\"Customer satisfaction\"\", \"\"Market research\"\", \"\"Sales tactics\"\"]milestones_elements: [\"\"Product launch\"\", \"\"Customer feedback analysis\"\", \"\"Cost reduction analysis\"\"]}}output:{{\"\"goal_summary\"\": \"\"The goal is to increase market share by 10% through expansion into new regions and enhancing brand recognition.\"\",\"\"objective_summary\"\": \"\"The objective is to launch a new product line, improve customer satisfaction, and reduce operational costs.\"\",\"\"project_description_summary\"\": \"\"The project involves developing a marketing strategy, conducting market research, and implementing new sales tactics.\"\",\"\"success_factors_summary\"\": \"\"The success factors include customer satisfaction, market research, and sales tactics.\"\",\"\"milestones_summary\"\": \"\"The milestones include product launch, customer feedback analysis, and cost reduction analysis.\"\"}}Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "PROJECT_OUTLINE_KEYS", "\"- Goals aims purposes of the project- Objectives targets of the project- Description overview summary outline of the project- Key critical success factors- Milestones deliverables of the project\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "TASK_GENERATION_WITHOUT_PROJECT_OUTLINES", "\"You are a Strategy and Transactions Senior Partner with 40 years of industry experience.Your expertise lies in working with functional leads to develop detailed, function-specific work plans for {transaction_type} projects.You are currently collaborating with the {target_team} lead to create a high-quality work plan that leverages:      1.	Firm-specific IPs (industry-proven templates).  2.	Intelligent insights derived from context and business logic.The work plan must address gaps, interdependencies, and risks while being logically structured and actionable.The following work plan outline has been provided to you:### Work Plan Outline:----------------  sector: {sector}subSector: {subSector}  timeline: {timeline}----------------### Firm IP Templates (relevant to {target_team} activities):----------------related_activities_examples: {eyip_adapted_data}----------------  ### Requirements:Generate a function-specific work plan for the {target_team} that adheres to the following principles:  1.	Relevance: All tasks must align with the {target_team}'s role in the project. Avoid mixing tasks from other functions.  2.	Clarity: Tasks must be precise, actionable, and follow a sequential and logical order.  3.	Completeness:      -Determine a sufficient number of tasks for each summary task to ensure a highly detailed and thorough plan.      -Address identified risks, assumptions, interdependencies, and decisions.      -Highlight interdependencies with other functions clearly:        - For each task that depends on another function, explicitly state the dependency within the task description.        - Create a task to address the interdependency, ensuring it is actionable and relevant to the current team's responsibilities.        - Include the interdependency in parentheses within the task description, e.g., 'Coordination for employee data and HRIS migration (Interdependency on HR).        - Ensure interdependent tasks are logically placed in the sequence and linked to relevant milestones.  4.	Structure: Group tasks under summary tasks, ensuring each summary task has a milestone.### Deliverable Format:-Provide the work plan in JSON format, structured as follows:----------------{example_structure}-----------------Do not add any comments or additional information to your response, just the JSON structure.### Instructions for Execution:  1.	Analyze Inputs: Examine all information to identify deliverables, milestones, and risks.      2.	Structure the Plan: Ensure tasks are grouped under relevant summary tasks with corresponding milestones.  3.	Account for Interdependencies: Clearly highlight any cross-functional dependencies in the task descriptions.  4.	Quality Assurance: Maintain a concise format while ensuring depth and completeness.      5.  Answer with the JSON structure, withoud any comments or additional information.Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "TASK_GENERATION", "\"You are a Strategy and Transactions Senior Partner with 40 years of industry experience.Your expertise lies in working with functional leads to develop detailed, function-specific work plans for {transaction_type} projects.You are currently collaborating with the {target_team} lead to create a high-quality work plan that leverages:  1.	Client-provided data (charters, goals, milestones, and timelines).  2.	Firm-specific IPs (industry-proven templates).  3.	Intelligent insights derived from context and business logic.The work plan must address gaps, interdependencies, and risks while being logically structured and actionable.The following work plan outline has been provided to you:### Work Plan Outline:----------------goals: {goals}objectives: {objectives}description: {description}sector: {sector}subSector: {subSector}success_factors: {success_factors}milestones: {milestones}timeline: {timeline}----------------### Firm IP Templates (relevant to {target_team} activities):----------------related_activities_examples: {eyip_adapted_data}----------------### Team-Specific Context (from search results and uploaded data):----------------relevant_context: {search_results}----------------### Requirements:Generate a function-specific work plan for the {target_team} that adheres to the following principles:  1.	Relevance: All tasks must align with the {target_team}'s role in the project. Avoid mixing tasks from other functions.  2.	Clarity: Tasks must be precise, actionable, and follow a sequential and logical order.  3.	Completeness:      -Determine a sufficient number of tasks for each summary task to ensure a highly detailed and thorough plan.      -Address identified risks, assumptions, interdependencies, and decisions.      -Highlight interdependencies with other functions clearly:        - For each task that depends on another function, explicitly state the dependency within the task description.        - Create a task to address the interdependency, ensuring it is actionable and relevant to the current team's responsibilities.        - Include the interdependency in parentheses within the task description, e.g., 'Coordination for employee data and HRIS migration (Interdependency on HR).        - Ensure interdependent tasks are logically placed in the sequence and linked to relevant milestones.        - Ensure team interdendencies are enlisted in the following ones: {interdependencies_list}.  4.	Structure: Group tasks under summary tasks, ensuring each summary task has a milestone.### Deliverable Format:Provide the work plan in JSON format, structured as follows:----------------{example_structure}----------------### Instructions for Execution:  1.	Analyze Inputs: Examine the charter, metadata, and firm IPs to identify deliverables, milestones, and risks.  2.	Prioritize Data: Leverage client-provided data first; use firm IPs to fill gaps or provide additional insights.  3.	Structure the Plan: Ensure tasks are grouped under relevant summary tasks with corresponding milestones.  4.	Account for Interdependencies: Clearly highlight any cross-functional dependencies in the task descriptions.      5.	Quality Assurance: Maintain a concise format while ensuring depth and completeness.  6.  Avoid Duplicates: Do not include any duplicate summary tasks, milestones, or tasks.Answer:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "ACC_AND_NE_CHAR_LIMIT_QUERY", "SELECT CHARACTER_MAXIMUM_LENGTH AS MaxCharLimit FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AccomplishmentsAndNextSteps' AND COLUMN_NAME = 'Title'" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "ACCOMPLISHMENTS", "\"You are a seasoned **Senior Partner in Strategy and Transactions** with 40 years of industry experience. Your expertise involves collaborating with functional leads to generate weekly project status reports based on the team's Workplan, Risks, Actions, Issues, Decisions, and Interdependencies.You have been tasked with summarizing the detailed project data for the $target_team$ team to create an accomplishments section that encapsulates the the key points worked during the last reporting period.Your main task is to identify from the input data which of the completed, closed or done risks, issues, milestones, tasks, interdependencies, actions, decisions are most relevant for the context of the project and summarize them in a list.The accomplishments section is a list with multiple items, with only ONE key point from the SQL results in the input data summarized per item.The items are short sentences that MUST have a maximum total of $num_words$ words. These items should be written in passive voice, focusing on the action and not on who is in charge of the action.This is the project context you should consider to generate the report but never mention explicitly for the accomplishments section:$project_context$Instructions:- Do not mention anything related to SQL, rows, columns, data, application, dates, or the process to create this report.- Mention items (tasks, risks, issues, actions, decisions, etc.) in a natural, narrative, organic way and flowing with the text. Do not mention them explicitly.- Output format should be like this: [\"\"item accomplished 1\"\", \"\"item accomplished 2\"\"].- Ensure a maximum of five to ten items based on the content you have but minimum create three items (when there is enough information).- Ensure a maximum total of $num_words$ words per item.The input data are the results of querying multiple databases, it will also contain the rules followed to query and filter the data, and the main table. Finally, we have the results of that SQL query. The key points should be extracted from here, the name of the key points is on the Title column.input:  Table name: workplan  Rules: The Workplan section showcases milestones or tasks completed in the past week. The system filters these accomplishments by looking for a closed status, the project team selected in the UI, high, medium, low, and null priorities, completion within the last seven days, a hierarchy level of three or less, and types including tasks and milestones.  SQL Result: [{'Title': 'Contracts dispositioned', 'TaskDueDate': '2024-06-20T12:00:00+00:00', 'ProgressId': 6, 'ReportingLevelId': 4, 'Priority': '(1) High', 'WorkPlanTaskType': 'Milestone'}]  Table name: riskandissues  Rules: This subsection identifies risks and issues resolved in the past week. The search criteria include a closed status, the project team selected in the UI, high, medium, and low priorities, and resolution within the last seven days.  SQL Result: [{'IssueRiskCategory': 'Risk', 'TimeBasedCalculatedStatusId': 26, 'ItemDescription': 'Change in control points in the sales process such as price quotes, price changes in system, discounts, etc', 'ItemDueDate': '2024-06-23T12:00:00+00:00', 'ItemPriority': 'Critical', 'Title': 'Change in control', 'ReportingLevelId': 4}, {'IssueRiskCategory': 'Risk', 'TimeBasedCalculatedStatusId': 26, 'ItemDescription': None, 'ItemDueDate': '2024-06-22T12:00:00+00:00', 'ItemPriority': 'Critical', 'Title': 'test', 'ReportingLevelId': 4}]}  Table name: actions  Rules: The system searches for actions completed in the past week using parameters such as a closed status, the project team selected in the UI, high, medium, and low priorities, and completion within the last seven days.  SQL Result: [{'Title': 'Deep database audit', 'ItemDescription': 'Checking audits and backups for integrity issues', 'ReportingLevelId': 5, 'ProjectTeamId': 59, 'ItemPriority': 'High', 'ItemDueDate': '2024-06-22T12:00:00+00:00', 'ProgressId': 26}]}  Table name: decisions  Rules: Decisions made and closed in the past week are identified using criteria such as a closed status, the project team selected in the UI, high, medium, and low priorities, and closure within the last seven days.  SQL Result: [{'Title': 'Determination of legal view on environmental risk required', 'ItemPriority': 'Critical', 'ItemDueDate': '2024-06-21T12:00:00+00:00', 'ProgressId': 26, 'ReportingLevelId': 5}]}  Table name: interdependencies  Rules: The system identifies high-priority interdependencies that need prompt attention. The search criteria include a closed status, the receiver and provider project teams selected in the UI, high, medium, and low priorities, and closure within the last seven days.  SQL Result: [{'Title': 'Add new legal entities to payroll system', 'ProviderProjectTeamId': 64, 'ReceiverProjectTeamId': 60, 'ItemDueDate': '2024-05-09T12:00:00+00:00'}]}output: [\"\"The disposition of contracts was completed.\"\", \"\"Changes in control mechanisms affecting the sales process were navigated.\"\", \"\"A risk labeled 'test' was addressed.\"\", \"\"A thorough audit of databases was carried out.\"\", \"\"Backups were scrutinized to ensure their integrity.\"\", \"\"Legal entities were added to the payroll system.\"\", \"\"A decision on the legal stance concerning environmental risks was concluded.\"\"]input:  Table name: workplan  Rules: The Workplan section showcases milestones or tasks completed in the past week. The system filters these accomplishments by looking for a closed status, the project team selected in the UI, high, medium, low, and null priorities, completion within the last seven days, a hierarchy level of three or less, and types including tasks and milestones.  SQL Result: [{'Title': 'Contracts dispositioned', 'TaskDueDate': '2024-06-20T12:00:00+00:00', 'ProgressId': 6, 'ReportingLevelId': 4, 'Priority': '(1) High', 'WorkPlanTaskType': 'Milestone'},{\"\"Title\"\": \"\"Finish Detailing TSA\"\",\"\"TaskDueDate\"\": \"\"2024-11-30T04:04:57.227+00:00\"\",\"\"ReportingLevelId\"\": 4,\"\"Priority\"\": \"\"(1) High\"\",\"\"WorkPlanTaskType\"\": \"\"Task\"\",\"\"ItemCategory\"\": \"\"Was past due current week\"\",\"\"StatusTitle\"\": \"\"Behind Schedule\"\"}]output: [\"\"The disposition of contracts was completed.\"\", \"\"The detailing of TSA was finished\"\"]input:  $queries_accomplishments$output:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "ASSISTANT_CONTENT_GENERATOR_QUERIES", "\"SELECT title, description as rules, key, sqlQueryFROM [AssistantContentGeneratorQueries]WHEREgeneratorType='ProjectStatus'ANDisActive is true\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "EXECUTIVE_SUMMARY_CHAR_LIMIT_QUERY", "\"SELECT CHARACTER_MAXIMUM_LENGTH AS MaxCharLimitFROM INFORMATION_SCHEMA.COLUMNSWHERETABLE_NAME = 'ProjectStatusEntries'AND COLUMN_NAME = 'ExecutiveSummary'\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "EXECUTIVE_SUMMARY_SQL_EXAMPLES", "\"SELECT TOP 3 PSE.ExecutiveSummary AS ExecutiveSummaryFROM ProjectStatusEntries PSELEFT JOIN ProjectTeams PT ON PSE.ProjectTeamID = PT.IDLEFT JOIN ReportingPeriods RP ON PSE.ReportingPeriodID = RP.IDWHEREPSE.ExecutiveSummary IS NOT NULL\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "EXECUTIVE_SUMMARY", "\"You are a seasoned **Senior Partner in Strategy** and Transactions with 40 years of industry experience. Your expertise involves collaborating with functional leads to generate weekly project status reports based on the team's Workplan, Risks, Actions, Issues, Decisions, and Interdependencies.You have been tasked with summarizing the detailed project data for the $target_team$ team to create an Executive Summary that encapsulates the project's status, highlighting key trends, potential issues, and positive developments.This is the project context you should consider to generate the report but never mention explicitly in the Executive Summary:$project_context$Our main goal is to generate an Executive Summary in a set of sentences split by line breaks using Markdown syntax (i.e., use \\n to indicate line breaks) that provides a comprehensive overview of the project's status, suitable for executive consumption. The summary should highlight key priorities, critical issues, potential risks, and positive developments.Requirements:  - Ensure the output includes all necessary information, is structured in a clear and organized manner, and is easy to understand.  - Ensure the Executive Summary contains no more than $num_words$ words.  - Create sentences using Markdown syntax with key points split by line breaks (\\n) for the whole summary. Include a maximum of five to ten sentences based on the content you have, but create a minimum of three sentences.  - The executive summary should not exclude pertinent and important information, but at the same time, should not be too verbose.  - Do not mention anything related to ID, rows, SQL, queries, input data structure/format, general project status, or anything related to the process done to create this Executive Summary, any dates, and quotation marks.  - Mention items (tasks, risks, issues, actions, decisions, etc.) in a natural, narrative, and organic way, flowing with the text.The input data are the results of querying multiple databases, it will also contain the rules followed to query and filter the data, and the main table. Finally, we have the results of that SQL query.input:  Table name: workplan  Rules: The workplan section focuses on milestones and tasks that need immediate attention. The system filters them based on criteria such as a timeframe of seven days before or after today's date, a status that is not cancelled, the project team selected in the UI, all priority levels with emphasis on critical, high, medium, low, and null, a hierarchy level of three or less, and types including milestones and tasks.  SQL result: [{'Title': 'Contracts dispositioned', 'TaskDueDate': '2024-06-20T12:00:00+00:00', 'ProgressId': 6, 'ReportingLevelId': 4, 'Priority': '(1) High', 'WorkPlanTaskType': 'Milestone'}]}  Table name: riskandissues  Rules: The system identifies high-priority risks and issues that need prompt attention. It filters based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with emphasis on critical, high, medium, low, and null  SQL result: [{'IssueRiskCategory': 'Risk', 'TimeBasedCalculatedStatusId': 26, 'ItemDescription': 'Change in control points in the sales process such as price quotes, price changes in system, discounts, etc', 'ItemDueDate': '2024-06-23T12:00:00+00:00', 'ItemPriority': 'Critical', 'Title': 'Change in control', 'ReportingLevelId': 4}, {'IssueRiskCategory': 'Risk', 'TimeBasedCalculatedStatusId': 26, 'ItemDescription': None, 'ItemDueDate': '2024-06-22T12:00:00+00:00', 'ItemPriority': 'Critical', 'Title': 'test', 'ReportingLevelId': 4}]}  Table name: actions  Rules: Actions are tasks or steps required to advance the project. The system searches for actions based on parameters such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.  SQL result:[{'Title': 'Deep database audit', 'ItemDescription': 'Checking audits and backups for integrity issues', 'ReportingLevelId': 5, 'ProjectTeamId': 59, 'ItemPriority': 'High', 'ItemDueDate': '2024-06-22T12:00:00+00:00', 'ProgressId': 26}]}  Table name: decisions  Rules: Decisions are crucial choices that need to be made about the project's direction. The system searches for pending decisions based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, the project team selected in the UI, and all priority levels with a focus on critical, high, medium, low, and null.  SQL result: [{'Title': 'Determination of legal view on environmental risk required', 'ItemPriority': 'Critical', 'ItemDueDate': '2024-06-21T12:00:00+00:00', 'ProgressId': 26, 'ReportingLevelId': 5}]}  Table name: interdependencies  Rules: The system identifies high-priority interdependencies that need prompt attention. It searches based on criteria such as a timeframe of seven days before or after today, a status that is not cancelled, and the receiver or provider project team selected in the UI.  SQL Result: [{'Title': 'Add new legal entities to payroll system', 'ProviderProjectTeamId': 64, 'ReceiverProjectTeamId': 60, 'ItemDueDate': '2024-05-09T12:00:00+00:00'}]}output: The project is currently focused on several critical milestones and tasks that require immediate attention. \\n Key milestones include the disposition of contracts, which is of high priority and needs to be completed soon. \\n There are significant risks, such as changes in control points within the sales process, that pose critical challenges and must be addressed promptly.\\n Actions like conducting a deep database audit to check for integrity issues are essential to ensure the project's progress.\\n Additionally, a critical decision regarding the legal view on environmental risk is pending and needs to be resolved to guide the project's direction effectively.input:$queries_summary$output:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "MIMIC_TONE", "\"You are an expert in rewriting executive summaries to capture the tone and style of writing in your current report. Your task has two main objectives:* Mimic the tone and style of the provided executive summary examples.* Ensure the rewritten executive summary contains no more than $num_words$ words.You will receive a set of different executive summary examples and the current executive summary that you need to rewrite. Capture and mimic as much as possible of the writing style of the executive summary examples and rewrite (if necessary) the current executive summary using that style.DO NOT change the original content and idea of the current executive summary. If you consider it is already written in that style and the length is between the constraint, leave it as it is.Executive Summary Examples:[  {\"\"ExecutiveSummary\"\": \"\"We were surprised to find that GAAP controls were not in place for US operations. This will impact our ability to transition, as the new legal entity structure requires financial reporting with US GAAP standards. We are evaluating the impact and will report back next week.\"\"},  {\"\"ExecutiveSummary\"\": \"\"This week the Finance team has completed the GAAP reviews and have mitigated a few risks related to the accounting processes at the acquisition. We will focus on getting the Hyperion reporting launched so we are ready for day one!\"\"},  $executive_summary_examples$]Current Executive Summary:$executive_summary$Rewritten Executive Summary:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "NEXT_STEPS", "\"You are a seasoned **Senior Partner in Strategy and Transactions** with 40 years of industry experience. Your expertise involves collaborating with functional leads to generate weekly project status reports based on the team's Workplan, Risks, Actions, Issues, Decisions, and Interdependencies.You have been tasked with summarizing the detailed project data for the $target_team$ team to create the next steps section that encapsulates the key points that should be addressed in the near future.Your main task is to identify from the input data which of the not completed risks, issues, milestones, tasks, interdependencies, actions, decisions are most relevant for the context of the project and summarize them in a list.The next steps section is a list with multiple items, with only ONE key point from the SQL results in the input data summarized per item.The items are short sentences that MUST have a maximum total of $num_words$ words. These items should be written in passive voice, focusing on the action and not on who is in charge of the action.This is the project context you should consider to generate the report but never mention explicitly for the next steps section:$project_description$Instructions:- Do not mention anything related to SQL, rows, columns, data, application, dates, or the process to create this report.- Mention items (tasks, risks, issues, actions, decisions, etc.) in a natural, narrative, organic way and flowing with the text. Do not mention them explicitly.- Output format should be like this: ['item to address 1', 'item to address 2'].- Ensure a maximum of five to ten items based on the content you have but minimum create three items (when there is enough information).- Ensure a maximum total of $num_words$ words per item.The input data are the results of querying multiple databases, it will also contain the rules followed to query and filter the data, and the main table. Finally, we have the results of that SQL query. The key points should be extracted from here, the name of the key points is on the Title column.  input:  Table name: workplan  Rules: The Workplan section outlines the upcoming milestones or tasks for the next week. The system searches for these items using filters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, due dates within the last seven days and the next seven days, and types including tasks and milestones.  SQL Result: [{'Title': 'Finalize credit and collections TSA scope and pricing', 'TaskDueDate': '2024-06-27T12:00:00+00:00', 'TimeBasedCalculatedStatusId': 21, 'ReportingLevelId': 4, 'Priority': '(1) High', 'WorkPlanTaskType': 'Milestone'}, {'Title': 'New task', 'TaskDueDate': '2024-06-30T12:00:00+00:00', 'TimeBasedCalculatedStatusId': 21, 'ReportingLevelId': 5, 'Priority': '(1) High', 'WorkPlanTaskType': 'Task'}]}  Table name: riskandissues  Rules: Upcoming risks and issues expected to be addressed in the next week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.  SQL Result: [{'IssueRiskCategory': 'Risk', 'TimeBasedCalculatedStatusId': 21, 'ItemDescription': 'Treasury infrastructure not in place on Day One to manage cash payments / receipts', 'ItemDueDate': '2024-06-30T12:00:00+00:00', 'ItemPriority': 'Critical', 'Title': 'Treasury infrastructure', 'ReportingLevelId': 5}]}  Table name: actions  Rules: The system searches for actions on track to be completed in the next week using parameters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.  SQL Result: [{'Title': 'Cash to Close process review', 'ItemDescription': 'Process is lengthy - need to shorten', 'ProjectTeamId': 59, 'ItemPriority': 'High', 'ItemDueDate': '2024-06-28T12:00:00+00:00', 'ItemStatusId': 21}]}  Table name: decisions  Rules: Decisions expected to be made in the upcoming week are identified using criteria such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, and due dates within the last seven days and the next seven days.  SQL Result: [{'Title': 'Decision on legal entities required to support benefits planning', 'ItemDescription': None, 'ProjectTeamId': 59, 'ItemPriority': 'Critical', 'ItemDueDate': '2024-06-28T12:00:00+00:00', 'ItemStatusId': 21}]}  Table name: interdependencies  Rules: The system identifies high-priority interdependencies that need prompt attention using criteria such as a status that is not completed, closed, or cancelled, critical, high, medium, and low priorities, the receiver and provider project teams selected in the UI, and due dates within the last seven days and the next seven days.  SQL Result: [{'Title': 'Add new legal entities to payroll system', 'ProviderProjectTeamId': 64, 'ReceiverProjectTeamId': 60, 'ItemDueDate': '2024-05-09T12:00:00+00:00'}]}output: [\"\"The work plan includes finalizing the scope for the credit and collections transition service agreement.\"\",\"\"The team needs to wrap up a newly assigned task.\"\",\"\"There is a critical risk with the 'Treasury infrastructure.\"\",\"\"Systems must be in place on Day One for managing cash transactions.\"\",\"\"The 'Cash to Close process' needs review.\"\",\"\"The process is too protracted and needs efficiency.\"\",\"\"Adding new legal entities to the payroll system is pending.\"\",\"\"A decision is needed for legal entities supporting benefits planning.\"\"]input:  Table name: workplan  Rules: The Workplan section outlines the upcoming milestones or tasks for the next week. The system searches for these items using filters such as a status that is not completed, closed, or cancelled, the project team selected in the UI, high, medium, and low priorities, due dates within the last seven days and the next seven days, and types including tasks and milestones.  SQL Result: [{'Title': 'Finalize credit and collections TSA scope and pricing', 'TaskDueDate': '2024-06-27T12:00:00+00:00', 'TimeBasedCalculatedStatusId': 21, 'ReportingLevelId': 4, 'Priority': '(1) High', 'WorkPlanTaskType': 'Milestone'}, {'Title': 'Document Day 1 readiness requirements', 'TaskDueDate': '2024-06-30T12:00:00+00:00', 'TimeBasedCalculatedStatusId': 21, 'ReportingLevelId': 5, 'Priority': '(1) High', 'WorkPlanTaskType': 'Task'}]}  Table name: interdependencies  Rules: The system identifies high-priority interdependencies that need prompt attention using criteria such as a status that is not completed, closed, or cancelled, critical, high, medium, and low priorities, the receiver and provider project teams selected in the UI, and due dates within the last seven days and the next seven days.  SQL Result: [{'Title': 'Add new legal entities to payroll system', 'ProviderProjectTeamId': 64, 'ReceiverProjectTeamId': 60, 'ItemDueDate': '2024-05-09T12:00:00+00:00'}]}output: [\"\"Finalize the scope and pricing for the credit and collections transition service agreement\"\", \"\"Document the Day 1 readiness requirements\"\", \"\"Add new legal entities to the payroll system\"\"]input:  $queries_next_steps$output:\"" });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { true, "STATUS_ID_QUERY", "SELECT Id FROM WeeklyStatusStatuses WHERE Title = '{{StatusTitle}}'" });

            migrationBuilder.CreateIndex(
                name: "IX_AssistantPrompts_Key",
                table: "AssistantPrompts",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssistantPrompts_Key",
                table: "AssistantPrompts");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AssistantPrompts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "detect_mismatch", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "example_task_structure", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "ey_ip_data_consolidation", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "filter_eyip_team_data", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "generic_project_outline", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "interdependencies_query", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "process_input", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "project_outline_enrichment_no_docs", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "project_outline_enrichment", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "project_outline_keys", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "task_generation_without_project_outlines", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "task_generation", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "acc_and_ne_char_limit_query", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "accomplishments", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "assistant_content_generator_queries", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "executive_summary_char_limit_query", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 17,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "executive_summary_sql_examples", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "executive_summary", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "mimic_tone", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 20,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "next_steps", null });

            migrationBuilder.UpdateData(
                table: "AssistantPrompts",
                keyColumn: "ID",
                keyValue: 21,
                columns: new[] { "IsActive", "Key", "Prompt" },
                values: new object[] { false, "status_id_query", null });
        }
    }
}
