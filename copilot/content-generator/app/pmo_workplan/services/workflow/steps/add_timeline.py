from app.core.program_office_api import ProgramOfficeResponse
from app.core.prompt_manager import create_prompt_manager
from app.pmo_workplan.services.workflow.events.wp_teamtasks_event import (
    WPTeamTasksEvent,
)
from app.pmo_workplan.services.workflow.events.wp_timeline_event import (
    WPTeamTimelineEvent,
)
from llama_index.core import PromptTemplate
from app.pmo_workplan.utils import temporal_format, string_to_json_base
from typing import List, Dict, Any, Union

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
    return "".join(re.findall("[a-zA-Z]+", st))


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


def generate_task_summary(input_json, team):
    """Generate a task summary from the input JSON."""
    data = load_json_data(input_json)
    inter_summary = data["inter_summary"]
    intra_summary = data["intra_summary"]

    inter_df = process_inter_summary(inter_summary)
    intra_df, last_tasks = process_intra_summary(intra_summary)
    updated_intra_df = update_dependencies(intra_df, inter_df, last_tasks)

    dataset = updated_intra_df.values.tolist()
    cpm = critical_path_method(dataset)
    today_date = datetime.date.today()
    project_data = cpm[["ES", "EF"]].reset_index().values.tolist()

    updated_project_data = calculate_dates(project_data, str(today_date))
    dict_timelines = {}
    for task in updated_project_data:
        group = extract_letters(task[0])
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

    for group in dict_timelines.keys():
        min_date, max_date = extract_min_max_dates(dict_timelines[group]["timelines"])
        dict_timelines[group].update({"StartDate": min_date, "TaskDueDate": max_date})

    return dict_timelines


async def add_timeline(
    workflow, event: WPTeamTasksEvent, context, llm
) -> WPTeamTimelineEvent | None:
    """
    Add dependencies between tasks and timeline to the project plan
    """
    result = event.result
    result_json = string_to_json_base(result)
    tasks = temporal_format(result_json)
    time_line = await context.get("duration_in_month", None)
    team = await context.get("original_team", None)
    teams_list = await context.get("teams")
    teams_list = [
        project_team.title for project_team in teams_list if project_team.title != team
    ]
    generation_time = prompt_manager.get_prompt_sync(agent="pmo_workplan",
                                                     key="time_generation")
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
        # Set an alarm for 5 seconds
        signal.alarm(5)
        tasks_copy = tasks.copy()
        dict_timelines = generate_task_summary(response.text, team)
        workplan_timeline = []
        for task in tasks:
            item = task["item"]
            new_dicto_task = task.copy()
            if item in dict_timelines.keys():
                new_dicto_task = align_tasks_with_timelines(task, dict_timelines[item])
                children = new_dicto_task["children"]
                new_dicto_task.pop("children")
                new_dicto_task.update(
                    {
                        "StartDate": dict_timelines[item]["StartDate"].strftime(
                            "%Y-%m-%dT%H:%M:%S"
                        )
                        + "+00:00",
                        "TaskDueDate": dict_timelines[item]["TaskDueDate"].strftime(
                            "%Y-%m-%dT%H:%M:%S"
                        )
                        + "+00:00",
                    }
                )
                for child in children:
                    if "item" in child.keys():
                        child["StartDate"] = (
                            dict_timelines[item]["TaskDueDate"].strftime(
                                "%Y-%m-%dT%H:%M:%S"
                            )
                            + "+00:00"
                        )
                        child["TaskDueDate"] = (
                            dict_timelines[item]["TaskDueDate"]
                            + datetime.timedelta(days=1)
                        ).strftime("%Y-%m-%dT%H:%M:%S") + "+00:00"
                        child.pop("item")

                new_dicto_task.update({"children": children})
                new_dicto_task.pop("item")
            workplan_timeline.append(new_dicto_task)

        signal.alarm(0)
    except Exception as e:
        for task in tasks_copy:
            for child in task["children"]:
                child.pop("item")
            task.pop("item")
        return WPTeamTimelineEvent(result=tasks)

    return WPTeamTimelineEvent(result=workplan_timeline)