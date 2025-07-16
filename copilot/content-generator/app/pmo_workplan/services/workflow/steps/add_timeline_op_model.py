from app.core.program_office_api import ProgramOfficeResponse
from app.core.prompt_manager import create_prompt_manager
from app.pmo_workplan.services.workflow.events.wp_teamtasks_event import (
    WPTeamTasksEvent,
)
from app.pmo_workplan.services.workflow.events.wp_timeline_event import (
    WPTeamTimelineEvent,
)
from llama_index.core import PromptTemplate
from app.pmo_workplan.utils import temporal_format_op_model, string_to_json_base
from app.pmo_workplan.services.workflow.events.wp_teamtasksom_event import (
    WPTeamTasksOMEvent,
)
import json
import numpy as np
import pandas as pd
import re
import datetime
from datetime import timedelta
import signal
import holidays
import platform

prompt_manager = create_prompt_manager()

def timeout_handler(signum, frame):
    raise TimeoutError("The operation timed out.")


if platform.system() != "Windows":
    signal.signal(signal.SIGALRM, timeout_handler)
else:
    signal.signal(signal.SIGABRT, timeout_handler)

def extract_letters(st):
    return "".join(re.findall("[A-Z]+", st))


def critical_path_method(dataset):
    ids = [item[0] for item in dataset]
    idx = [i for i in range(0, len(dataset))]
    idd = dict(zip(ids, idx))
    e_m = np.zeros((len(dataset), len(dataset)))
    l_m = np.zeros((len(dataset), len(dataset)))
    for i in range(0, e_m.shape[0]):
        if len(dataset[i][1]) != 0:
            a = idd[dataset[i][0]]
            for b in dataset[i][1]:
                b = idd[b]
                e_m[a, b] = e_m[a, b] + 1
                l_m[a, b] = l_m[a, b] + 1
    dates = np.zeros((len(dataset), 5))
    dates[:, -2:] = float("+inf")
    early = np.sum(e_m, axis=1)
    flag = True
    while np.sum(early, axis=0) != -early.shape[0]:
        j_lst = []
        for i in range(0, early.shape[0]):
            if early[i] == 0:
                early[i] = -1
                j_lst.append(i)
        if flag == True:
            for j in j_lst:
                dates[j, 0] = 0
                dates[j, 1] = dates[j, 0] + dataset[j][2]
                flag = False
        if flag == False:
            for j in j_lst:
                for i in range(0, early.shape[0]):
                    if e_m[i, j] == 1:
                        e_m[i, j] = 0
                        early[i] = np.clip(early[i] - 1, -1, float("+inf"))
                        if dates[i, 1] < dates[j, 1] + dataset[i][2]:
                            dates[i, 0] = dates[j, 1]
                            dates[i, 1] = dates[j, 1] + dataset[i][2]
    finish_time = np.max(dates[:, 1])
    late = np.sum(l_m, axis=0)
    flag = True
    while np.sum(late, axis=0) > -late.shape[0]:
        i_lst = []
        for i in range(0, late.shape[0]):
            if late[i] == 0:
                late[i] = -1
                i_lst.append(i)
        if flag == True:
            for i in i_lst:
                dates[i, 3] = finish_time
                dates[i, 2] = dates[i, 3] - dataset[i][2]
                dates[i, -1] = dates[i, 3] - dates[i, 1]
                flag = False
        if flag == False:
            for i in i_lst:
                for j in range(0, late.shape[0]):
                    if l_m[i, j] == 1:
                        l_m[i, j] = 0
                        late[j] = np.clip(late[j] - 1, -1, float("+inf"))
                        if dates[j, 3] > dates[i, 3] - dataset[i][2]:
                            dates[j, 3] = dates[i, 3] - dataset[i][2]
                            dates[j, 2] = dates[j, 3] - dataset[j][2]
                            dates[j, -1] = dates[j, 3] - dates[j, 1]
    dates = pd.DataFrame(dates, index=ids, columns=["ES", "EF", "LS", "LF", "Slack"])
    return dates


def load_json_data(input_json):
    """Load and clean JSON data from the input string."""
    json_string = input_json.strip("```json\n").strip("```").strip()
    json_string = json_string.strip("```python\n").strip("```").strip()
    return json.loads(json_string)


def process_inter_summary(inter_summary):
    """Process inter_summary to create a DataFrame."""
    inter_data = [
        {"Source": key, "Target": value}
        for key, values in inter_summary.items()
        for value in values
    ]
    return pd.DataFrame(inter_data)


def process_intra_summary(intra_summary):
    """Process intra_summary to create a DataFrame and track dependencies."""
    task_list = []
    last_tasks = {group: [] for group in intra_summary.keys()}
    dependencies_tracker = {group: set() for group in intra_summary.keys()}

    for group, tasks in intra_summary.items():
        for task_name, task_info in tasks.items():
            duration = task_info["duration"]
            dependencies = task_info["dependencies"]
            dependencies_str = ", ".join(dependencies) if dependencies else ""
            task_list.append([task_name, group, duration, dependencies_str])
            dependencies_tracker[group].update(dependencies)

    intra_df = pd.DataFrame(
        task_list, columns=["Task", "Group", "Duration", "Dependencies"]
    )
    identify_last_tasks(intra_summary, dependencies_tracker, last_tasks)

    return intra_df, last_tasks


def identify_last_tasks(intra_summary, dependencies_tracker, last_tasks):
    """Identify last tasks for each group."""
    for group, tasks in intra_summary.items():
        for task_name in tasks.keys():
            if task_name not in dependencies_tracker[group]:
                last_tasks[group].append(task_name)


def update_dependencies(intra_df, inter_df, last_tasks):
    """Update dependencies in the intra_df based on inter_df and last_tasks."""
    for group in last_tasks.keys():
        sources = inter_df.loc[inter_df["Target"] == group, "Source"].values
        add_tasks = [task for source in sources for task in last_tasks[source]]
        if add_tasks:
            intra_df.loc[
                (intra_df["Group"] == group) & (intra_df["Dependencies"] == ""),
                "Dependencies",
            ] = ", ".join(add_tasks)

    intra_df["New_dependencies"] = intra_df["Dependencies"].apply(
        lambda x: x.split(", ") if x else []
    )
    return intra_df[["Task", "New_dependencies", "Duration"]]


def is_holiday(date, holiday_list):
    """Checks if a date is a holiday in a list of countries."""
    date_is_holiday = False
    date_is_holiday = date in holiday_list
    return date_is_holiday


def get_k_workday(ref_date, k, holiday_list):
    """
    Gets the workday K days after the reference date.
    Considers the holidays for the countries in "countries"
    """
    # adding a zero delta to avoid an assignment by reference
    current_date = ref_date + timedelta(days=0)

    while current_date.weekday() in [5, 6] or is_holiday(current_date, holiday_list):
        current_date += timedelta(days=1)
    
    workdays_count = 0
    i = 0 # iterator

    while workdays_count < k:
        # Monday is 0
        is_weekend_flag = (ref_date + timedelta(days=(i + 1))).weekday() in [5, 6]
        is_holiday_flag = is_holiday(current_date + timedelta(days=(i + 1)), holiday_list)
        if (not is_weekend_flag) and (not is_holiday_flag):
            workdays_count += 1
        i += 1
    
    return current_date + timedelta(days=i)


def calculate_dates(project_data, start_date, countries:list =["US"]):
    start_date = datetime.datetime.strptime(start_date, "%Y-%m-%d")
    holidays_list = holidays.country_holidays('US')
    # Move start date until it is a workday
    while start_date.weekday() in [5, 6] or is_holiday(start_date, countries):
        start_date += timedelta(days=1)
    
    updated_project_data = []

    for task in project_data:
        task_name, es, ef = task
        
        start_date_task = get_k_workday(start_date, es, holidays_list)
        end_date_task = get_k_workday(start_date, ef, holidays_list)
        updated_project_data.append(
            (task_name, es, ef, start_date_task.date(), end_date_task.date())
        )

    return updated_project_data


def extract_min_max_dates(data):
    min_date = None
    max_date = None

    for item in data:
        for _, value in item.items():
            start_date = value["StartDate"]
            task_due_date = value["TaskDueDate"]

            if min_date is None or start_date < min_date:
                min_date = start_date
            if min_date is None or task_due_date < min_date:
                min_date = task_due_date

            if max_date is None or start_date > max_date:
                max_date = start_date
            if max_date is None or task_due_date > max_date:
                max_date = task_due_date

    return min_date, max_date

def extract_min_max_dates_tasks(data):
    min_date = None
    max_date = None

    for item in data:
        if item['workPlanTaskType'] == 'Task':
            start_date = item["StartDate"]
            task_due_date = item["TaskDueDate"]

            if min_date is None or start_date < min_date:
                min_date = start_date
            if min_date is None or task_due_date < min_date:
                min_date = task_due_date

            if max_date is None or start_date > max_date:
                max_date = start_date
            if max_date is None or task_due_date > max_date:
                max_date = task_due_date

    return min_date, max_date


def extract_min_max_dates_process(data):
    min_date = None
    max_date = None

    for item in data:
        start_date = item["StartDate"]
        task_due_date = item["TaskDueDate"]

        if min_date is None or start_date < min_date:
            min_date = start_date
        if min_date is None or task_due_date < min_date:
            min_date = task_due_date

        if max_date is None or start_date > max_date:
            max_date = start_date
        if max_date is None or task_due_date > max_date:
            max_date = task_due_date

    return min_date, max_date


def align_tasks_with_timelines(tasks_dict, timelines_dict):
    timelines = timelines_dict["timelines"]
    task_timeline_mapping = {}
    for timeline in timelines:
        for task_item, dates in timeline.items():
            task_timeline_mapping[task_item] = dates

    def align_children(children):
        for child in children:
            item = child["item"]
            if item in task_timeline_mapping:
                child["StartDate"] = (
                    task_timeline_mapping[item]["StartDate"].strftime(
                        "%Y-%m-%dT%H:%M:%S"
                    )
                    + "+00:00"
                )
                child["TaskDueDate"] = (
                    task_timeline_mapping[item]["TaskDueDate"].strftime(
                        "%Y-%m-%dT%H:%M:%S"
                    )
                    + "+00:00"
                )
                child.pop("item")
            if "children" in child and child["children"]:
                align_children(child["children"])

    align_children(tasks_dict["children"])

    return tasks_dict


def max_nested_dicts(structure):
    if isinstance(structure, dict):
        return 1 + max((max_nested_dicts(v) for v in structure.values()), default=0)
    return 0


def generate_task_summary(input_json, team):
    """Generate a task summary from the input JSON."""
    current_date = datetime.date.today()
    data = load_json_data(input_json)
    inter_summary = data["lvl_1_summary_tasks"]
    intra_summary = data["lvl_2_summary_tasks"]
    dict_timelines = {}
    for group in intra_summary.keys():

        if max_nested_dicts(intra_summary[group]) == 4:
            dict_timelines[group] = {}
            dict_timelines_sub_groups = {}
            subgroups = intra_summary[group].keys()
            for subgroup in subgroups:
                
                intra_df, last_tasks = process_intra_summary(intra_summary[group][subgroup])
                group_order = list(last_tasks.keys())
                def get_previous_dependencies(group):
                    idx = group_order.index(group)
                    if idx == 0:
                        return ''  # no previous group
                    prev_group = group_order[idx - 1]
                    return ",".join(last_tasks[prev_group])
                
                mask = (intra_df['Dependencies'] == '') & (intra_df['Group'] != group_order[0])
                intra_df.loc[mask, 'Dependencies'] = intra_df.loc[mask, 'Group'].apply(get_previous_dependencies)
                intra_df["New_dependencies"] = intra_df["Dependencies"].apply(
                        lambda x: x.split(", ") if x else []
                    )

                dataset = intra_df[['Task', 'New_dependencies', 'Duration']].values.tolist()
                cpm = critical_path_method(dataset)

                project_data = cpm[["ES", "EF"]].reset_index().values.tolist()

                updated_project_data = calculate_dates(project_data, str(current_date))
                for task in updated_project_data:
                    start_date = datetime.datetime.combine(task[3], datetime.time(12, 0, 0))
                    end_date = datetime.datetime.combine(task[4], datetime.time(12, 0, 0))
                    if subgroup not in dict_timelines_sub_groups:
                        dict_timelines_sub_groups[subgroup] = {
                            "timelines": [
                                {task[0]: {"StartDate": start_date, "TaskDueDate": end_date}}
                            ]
                        }
                    else:
                        dict_timelines_sub_groups[subgroup]["timelines"].append(
                            {task[0]: {"StartDate": start_date, "TaskDueDate": end_date}}
                            )
                min_dates = []
                max_dates = []
                for subgroup in dict_timelines_sub_groups.keys():
                    min_date, max_date = extract_min_max_dates(dict_timelines_sub_groups[subgroup]["timelines"])
                    min_dates.append(min_date)
                    max_dates.append(max_date)
                    dict_timelines_sub_groups[subgroup].update({"StartDate": min_date, "TaskDueDate": max_date})
            
                current_date = max_date.date()
            min_date = min(min_dates)
            max_date = max(max_dates)
            dict_timelines[group][subgroup] = dict_timelines_sub_groups[subgroup]
            dict_timelines[group].update({"StartDate": min_date, "TaskDueDate": max_date})
        else:
        
            intra_df, last_tasks = process_intra_summary(intra_summary[group])
            # Ensure the group order is preserved
            group_order = list(last_tasks.keys())

            # Function to get the last tasks of the *previous* group
            def get_previous_dependencies(group):
                idx = group_order.index(group)
                if idx == 0:
                    return ''  # no previous group
                prev_group = group_order[idx - 1]
                return ",".join(last_tasks[prev_group])

            mask = (intra_df['Dependencies'] == '') & (intra_df['Group'] != group_order[0])
            intra_df.loc[mask, 'Dependencies'] = intra_df.loc[mask, 'Group'].apply(get_previous_dependencies)
            intra_df["New_dependencies"] = intra_df["Dependencies"].apply(
                    lambda x: x.split(", ") if x else []
                )

            dataset = intra_df[['Task', 'New_dependencies', 'Duration']].values.tolist()
            cpm = critical_path_method(dataset)

            project_data = cpm[["ES", "EF"]].reset_index().values.tolist()

            updated_project_data = calculate_dates(project_data, str(current_date))
            
            for task in updated_project_data:
                start_date = datetime.datetime.combine(task[3], datetime.time(12, 0, 0))
                end_date = datetime.datetime.combine(task[4], datetime.time(12, 0, 0))
                if group not in dict_timelines:
                    dict_timelines[group] = {
                        "timelines": [
                            {task[0]: {"StartDate": start_date, "TaskDueDate": end_date}}
                        ]
                    }
                else:
                    dict_timelines[group]["timelines"].append(
                        {task[0]: {"StartDate": start_date, "TaskDueDate": end_date}}
                        )
                    
        # for group in dict_timelines.keys():
        timelines = dict_timelines[group].get("timelines", [])
        if not timelines:
            min_date = dict_timelines[group].get("StartDate")
            max_date = dict_timelines[group].get("TaskDueDate")
        else:
            min_date, max_date = extract_min_max_dates(dict_timelines[group]["timelines"])
        dict_timelines[group].update({"StartDate": min_date, "TaskDueDate": max_date})

        current_date = max_date.date()

    return dict_timelines


async def add_timeline_op_model(
    workflow, event: WPTeamTasksOMEvent, context, llm
) -> WPTeamTimelineEvent | None:
    """
    Add dependencies between tasks and timeline to the project plan
    """
    result = event.result
    result_json = string_to_json_base(result)

    tasks = temporal_format_op_model(result_json)
    time_line = await context.get("duration_in_month", None)
    team = await context.get("original_team", None)
    teams_list = await context.get("teams")
    teams_list = [
        project_team.title for project_team in teams_list if project_team.title != team
    ]
    # Detect if there are 2 or 3 levels in the summary tasks
    generation_time = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                    key="time_generation_op_model_2levels")

    generation_time_template = PromptTemplate(generation_time)

    response = await llm.acomplete(
        generation_time_template.format(
            target_team=team,
            tasks=tasks,
            timeline=f"{time_line} months",
            teams_list=(", ".join(teams_list)),
        ),
    )
    try:
        #Set an alarm for 5 seconds
        signal.alarm(5)
        tasks_copy = tasks.copy()
        dict_timelines = generate_task_summary(response.text, team)
        workplan_timeline = []
        for group in tasks_copy[:]:
            process_timeline = []
            interes_group = group['item']
            if group['children'][0].get('children')[0].get('children'):
                item_childrens = []
                for items in group['children']:
                    items_copy = items.copy()
                    children = items_copy.get("children",[])
                    for element in items_copy['children']:
                        element_copy = element.copy()
                        current_element = align_tasks_with_timelines(element_copy, dict_timelines[interes_group][items['item']])
                        children = current_element.get("children",[])
                        min_date, max_date = extract_min_max_dates_tasks(children)
                        current_element.pop("children", None)  
                        current_element.update(
                                {
                            "StartDate": min_date,
                            "TaskDueDate": max_date,
                                }
                            )         
                        for child in children:
                            if "item" in child:
                                child["StartDate"] = min_date
                                child["TaskDueDate"] = max_date
                                child.pop("item")

                            current_element["children"] = children
                            current_element.pop("item", None)
                        item_childrens.append(current_element)
                    items_copy.pop("item", None)
                    items_copy.pop("children", None)
                    items_copy.update({'StartDate': dict_timelines[interes_group][items['item']]['StartDate'].strftime(
                                                "%Y-%m-%dT%H:%M:%S"
                                            ),
                                        'TaskDueDate': dict_timelines[interes_group][items['item']]['TaskDueDate'].strftime(
                                                "%Y-%m-%dT%H:%M:%S"
                                            ),
                                        'children': item_childrens})
                    process_timeline.append(items_copy)
            else:
                for items in group['children']:
                    items_copy = items.copy()
                    current_item = align_tasks_with_timelines(items_copy, dict_timelines[interes_group])
                    children = current_item.get("children",[])
                    min_date, max_date = extract_min_max_dates_tasks(children)
                    current_item.pop("children", None)  
                    current_item.update(
                            {
                        "StartDate": min_date,
                        "TaskDueDate": max_date,
                            }
                        )         
                    for child in children:
                        if "item" in child:
                            child["StartDate"] = min_date
                            child["TaskDueDate"] = max_date
                            child.pop("item")

                        current_item["children"] = children
                        current_item.pop("item", None)

                    process_timeline.append(current_item)      
            
            group_copy = group.copy()
            group_copy.pop("item", None)
            group_copy.pop("children", None)
            group_copy.update({'StartDate': dict_timelines[interes_group]['StartDate'].strftime(
                                            "%Y-%m-%dT%H:%M:%S"
                                        ),
                                'TaskDueDate': dict_timelines[interes_group]['TaskDueDate'].strftime(
                                            "%Y-%m-%dT%H:%M:%S"
                                        ),
                                'children': process_timeline})
            
            workplan_timeline.append(group_copy)

            signal.alarm(0)
    except Exception as e:
        for task in tasks_copy:
            for child in task["children"]:
                child.pop("item")
            task.pop("item")
        return WPTeamTimelineEvent(result=tasks)

    return WPTeamTimelineEvent(result=workplan_timeline)