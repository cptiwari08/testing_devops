import json
import re

import json
import pandas as pd
import numpy as np
from typing import List, Dict, Any
import datetime
from copy import deepcopy


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
        add_tasks = [
            task for source in sources for task in last_tasks[source]
        ]  # Flatten the list
        if add_tasks:
            intra_df.loc[
                (intra_df["Group"] == group) & (intra_df["Dependencies"] == ""),
                "Dependencies",
            ] = ", ".join(add_tasks)

    intra_df["New_dependencies"] = intra_df["Dependencies"].apply(
        lambda x: x.split(", ") if x else []
    )
    return intra_df[["Task", "New_dependencies", "Duration"]]


def generate_task_summary(input_json):
    """Generate a task summary from the input JSON."""
    data = load_json_data(input_json)
    inter_summary = data["inter_summary"]
    intra_summary = data["intra_summary"]

    inter_df = process_inter_summary(inter_summary)
    intra_df, last_tasks = process_intra_summary(intra_summary)
    updated_intra_df = update_dependencies(intra_df, inter_df, last_tasks)

    dataset = updated_intra_df.values.tolist()
    cpm = critical_path_method(
        dataset
    )  # Assuming critical_path_method is defined elsewhere

    return cpm


def temporal_format(input_json_data):
    next_id = 1
    starts_with = "A"
    output_data = []
    for item in input_json_data:
        # Create root node (Summary Task)
        root_id = next_id
        next_id += 1
        root_node = {
            "item": starts_with,
            "title": item["summary_task"],
            "parentTask": None,
            "workPlanTaskType": "Summary Task",
            "children": [],
        }

        # Create milestone node
        milestone_id = next_id
        next_id += 1
        milestone_node = {
            "item": f"{starts_with}milestone",
            "title": item["milestone"],
            "parentTask": f"{item['summary_task']} | {root_id}",
            "workPlanTaskType": "Milestone",
            "children": [],
        }
        root_node["children"].append(milestone_node)

        # Create task nodes
        for index, task in enumerate(item["tasks"]):
            task_node = {
                "item": f"{starts_with}{index}",
                "title": task,
                "parentTask": f"{item['milestone']} | {milestone_id}",
                "workPlanTaskType": "Task",
            }
            root_node["children"].append(task_node)
        starts_with = chr(ord(starts_with) + 1)
        output_data.append(root_node)

    return output_data


def _create_milestone_node(item_prefix, milestone_name, parent_title):
    """Helper function to create a milestone node."""
    return {
        "item": f"{item_prefix}milestone",
        "title": milestone_name,
        "parentTask": parent_title,
        "workPlanTaskType": "Milestone",
        "children": []
    }

def _create_task_nodes(item_prefix, tasks, parent_title):
    """Helper function to create task nodes from a list of tasks."""
    task_nodes = []
    for index, task in enumerate(tasks):
        task_nodes.append({
            "item": f"{item_prefix}{index}",
            "title": task,
            "parentTask": parent_title,
            "workPlanTaskType": "Task",
        })
    return task_nodes

def temporal_format_op_model(input_json_data):
    next_id = 1
    starts_with = "A"
    output_data = []
    
    for item in input_json_data:
        grandparent_node = {
            "item": starts_with,
            "title": item["summary_task"],
            "workPlanTaskType": "Summary Task",
            "children": []
        }
        
        parents = item.get("summary_tasks_of_grandparent_name", [])
        
        if not parents:
            # Simple case - no parent nodes
            milestone_node = _create_milestone_node(starts_with, item["milestone"], grandparent_node["title"])
            grandparent_node["children"].append(milestone_node)
            
            task_nodes = _create_task_nodes(starts_with, item["tasks"], grandparent_node["title"])
            grandparent_node["children"].extend(task_nodes)
            
            output_data.append(grandparent_node)
            starts_with = chr(ord(starts_with) + 1)
            continue
        
        # Process parent nodes
        parent_id = "A"
        for parent in parents:
            next_id += 1
            parent_node = {
                "item": f"{starts_with}{parent_id}",
                "title": parent["summary_task"],
                "parentTask": grandparent_node["title"],
                "workPlanTaskType": "Summary Task",
                "children": []
            }
            
            parent_children = parent.get("summary_tasks_of_parent_name", [])
            
            if parent_children:
                # Process has children
                process_id = "A"
                for process in parent_children:
                    process_node = {
                        "item": f"{starts_with}{parent_id}{process_id}",
                        "title": process["summary_task"],
                        "parentTask": parent_node["title"],
                        "workPlanTaskType": "Summary Task",
                        "children": []
                    }
                    
                    milestone_node = _create_milestone_node(
                        f"{starts_with}{parent_id}{process_id}", 
                        process["milestone"], 
                        process_node["title"]
                    )
                    process_node["children"].append(milestone_node)
                    
                    task_nodes = _create_task_nodes(
                        f"{starts_with}{parent_id}{process_id}", 
                        process["tasks"], 
                        process_node["title"]
                    )
                    process_node["children"].extend(task_nodes)
                    
                    parent_node["children"].append(process_node)
                    process_id = chr(ord(process_id) + 1)
            else:
                # Process has no children
                milestone_node = _create_milestone_node(
                    f"{starts_with}{parent_id}", 
                    parent["milestone"], 
                    parent_node["title"]
                )
                parent_node["children"].append(milestone_node)
                
                task_nodes = _create_task_nodes(
                    f"{starts_with}{parent_id}", 
                    parent["tasks"], 
                    milestone_node["title"]
                )
                parent_node["children"].extend(task_nodes)
                
                parent_id = chr(ord(parent_id) + 1)
                
            grandparent_node["children"].append(parent_node)
            
        starts_with = chr(ord(starts_with) + 1)
        output_data.append(grandparent_node)
        
    return output_data


def format_output_json(id: int, title: str, workplan_timeline, project_docs, templates):
    output_data = {
        "projectTeam": {"id": id, "title": title},
        "content": [],
        "citingSources": {},
        "chainOfThoughts": ""
    }
    output_data["content"] = workplan_timeline
    citing_sources = []
    citing_sources_prj_dcs = create_citing_sources_prj_dcs(project_docs, title)
    citing_sources.append(citing_sources_prj_dcs)
    citing_sources_templates = create_citing_sources_templates(templates, title)
    citing_sources.append(citing_sources_templates)
    output_data["citingSources"] = citing_sources

    return output_data


def create_citing_sources_prj_dcs(
    project_docs, title: str, document_type="team-specific-project-docs"
):
    citing_sources = {}
    citing_sources["sourceName"] = document_type
    citing_sources["sourceType"] = "documents"
    citing_sources["content"] = (
        f"Final generated output to be used to generate workplan for {title}"
    )
    project_docs_list = []
    chunks_ids = []
    for doc in project_docs:
        if doc["chunk_id"] not in chunks_ids:
            current_chunk = {}
            page_numbers = extract_page_numbers(doc["chunk_text"])
            current_chunk["documentId"] = doc["documentId"]
            current_chunk["documentName"] = doc["documentName"]
            current_chunk["chunk_id"] = doc["chunk_id"]
            current_chunk["pages"] = page_numbers
            current_chunk["chunk_text"] = doc["chunk_text"]
            project_docs_list.append(current_chunk)
    citing_sources["sourceValue"] = project_docs_list
    return citing_sources


def create_citing_sources_templates(templates_dict, title: str, document_type="ey-ip"):
    citing_sources = {}
    citing_sources["sourceName"] = document_type
    citing_sources["sourceType"] = "EYIP Database"
    citing_sources["content"] = (
        f"Final generated output to be used to generate workplan for {title}"
    )
    row_number = (
        row_count(templates_dict.get("eyip")) if templates_dict.get("sql_okay") else 0
    )
    source_value = {
        "tableName": "workplan",
        "rowCount": row_number,
        "sqlQuery": templates_dict.get("sql_query"),
    }
    citing_sources["sourceValue"] = [source_value]
    return citing_sources


def row_count(templates):
    total_items = 0
    try:
        for item in templates:
            total_items += 1  # Each dictionary has one summary task
            total_items += len(
                item.get("tasks")
            )  # Count the number of tasks in the 'tasks' list
            total_items += 1  # Each dictionary has one milestone
    except:
        pass
    return total_items

def string_to_json_base(string):
    # Use regular expression to extract the JSON part from the input string
    json_match = re.search(r"```json(.*?)```", string, re.DOTALL)

    if not json_match:
        # Try to process the string as is if it doesn't match the pattern
        json_string = string.strip()
    else:
        json_string = json_match.group(1).strip()

    try:
        # Convert the extracted JSON string into a Python object
        json_data = json.loads(json_string)                           
        return json_data

    except (json.JSONDecodeError, ValueError) as e:
        raise ValueError(f"Error processing JSON: {e}")


def string_to_json(string: str) -> List[Dict[str, Any]]:
    """   
    [
        {
            "summary_task": str,
            "summary_task_processes": [
                {
                    "process": str,
                    "tasks": [str, ...],
                    "milestone": str
                },
                ...
            ]
        },
        ...
    ]
    """
    # 1. 
    match = re.search(r"```json(.*?)```", string, re.DOTALL)
    json_string = match.group(1) if match else string
    json_string = json_string.strip()

    try:
        data = json.loads(json_string)
    except json.JSONDecodeError as exc:
        raise ValueError(f"Error decoding JSON: {exc}") from None

    # 2. 
    if isinstance(data, dict):
        data = [data]

    if not (isinstance(data, list) and all(isinstance(elem, dict) for elem in data)):
        raise ValueError("JSON error")

    # 3. Validate structure
    top_required = {"summary_task", "summary_task_processes"}
    process_required = {"process", "tasks", "milestone"}

    for item in data:
        if not top_required.issubset(item.keys()):
            raise ValueError(
                f"Missing keys {top_required}."
            )

        processes = item["summary_task_processes"]
        if not (isinstance(processes, list) and all(isinstance(p, dict) for p in processes)):
            raise ValueError("'summary_task_processes' list must contain dictionaries.")

        for proc in processes:
            if not process_required.issubset(proc.keys()):
                raise ValueError(
                    f"key missing in {process_required}."
                )
            if not isinstance(proc["tasks"], list):
                raise ValueError("'tasks' list must contain strings.")

    return data

def extract_page_numbers(text):
    pattern1 = re.compile(r"Page (\d+)|-- END for Page Number - (\d+)")
    pattern2 = re.compile(r"Page Number :(\d+)")

    page_numbers = set()

    matches1 = pattern1.findall(text)
    for match in matches1:
        for num in match:
            if num:
                page_numbers.add(int(num))

    matches2 = pattern2.findall(text)
    for num in matches2:
        page_numbers.add(int(num))

    return sorted(page_numbers)


def merge_unique_dicts(list_of_lists):
    unique_dicts = []
    seen_ids = set()

    for sublist in list_of_lists:
        for dictionary in sublist:
            if dictionary["documentId"] not in seen_ids:
                seen_ids.add(dictionary["documentId"])
                unique_dicts.append(dictionary)
    return unique_dicts


def clean_parent_task_name(parent_str):
    # If parent_str is None, just return None
    if parent_str is None:
        return None
    # Strip off the " | [nnn]" part if present
    return re.split(r"\|", parent_str)[0].strip()


def flatten_team_documents(data):
    # Identify all summary tasks
    # A summary task is an entry with workplantasktype == "Summary Task"
    summary_tasks = [
        doc for doc in data if doc.get("workplantasktype") == "Summary Task"
    ]

    # Build a mapping: summary_task_title -> list of documents that have it as parent
    # Note: The parenttask field often ends with "| [xxx]", we remove that for matching
    children_map = {}
    for doc in data:
        parent_clean = clean_parent_task_name(doc.get("parenttask"))
        if parent_clean:
            children_map.setdefault(parent_clean, []).append(doc)

    # For each summary task, gather tasks and milestone
    results = []
    for summary in summary_tasks:
        summary_title = summary["title"]
        summary_children = children_map.get(summary_title, [])

        tasks = []
        milestone = None
        for child in summary_children:
            wpt_type = child.get("workplantasktype", "")
            if wpt_type == "Task":
                tasks.append(child["title"])
            elif wpt_type == "Milestone" and milestone is None:
                milestone = child["title"]

        result_obj = {
            "summary_task": summary_title,
            "tasks": tasks,
            "milestone": milestone if milestone else "",
        }
        results.append(result_obj)

    return results


def _iso(dt: datetime.datetime | None) -> str | None:
    """Return an ISO-8601 string with timezone if a datetime is given."""
    return dt.isoformat() if dt else None


def _min_max_dates(tasks: List[Dict[str, Any]]) -> tuple[str | None, str | None]:
    """Return (earliest StartDate, latest TaskDueDate) among a list of tasks."""
    if not tasks:
        return None, None
    starts = []
    ends = []
    for t in tasks:
        try:
            starts.append(datetime.datetime.fromisoformat(t["StartDate"]))
            ends.append(datetime.datetime.fromisoformat(t["TaskDueDate"]))
        except (KeyError, ValueError, TypeError):
            pass
    if not starts or not ends:
        return None, None
    return _iso(min(starts)), _iso(max(ends))


def transform_workplan(data_in: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
    """
    Convert the generator output (summary_tasks ▸ processes ▸ tasks) into the
    hierarchical structure expected by the Workplan Builder UI.

    * Top-level  : one node per `summary_task` -> workPlanTaskType = "summary_task"
    * Second-level: one node per `process`   -> workPlanTaskType = "process"
                   + `children` holding (a) the milestone and (b) each task.
    * Dates on a node are the min/max of all its descendant tasks.
    * Everything is copied, never mutated in-place.
    """
    if not isinstance(data_in, list):
        raise TypeError("Input must be a list of summary_task dictionaries")

    out: List[Dict[str, Any]] = []
    for st in data_in:
        st = deepcopy(st)

        processes = st.get("summary_task_processes", [])
        summary_children: List[Dict[str, Any]] = []
        summary_all_tasks: List[Dict[str, Any]] = []  

        for proc in processes:
            proc_tasks = proc.get("tasks", [])
            milestone_title = proc.get("milestone")
            milestone_node: Dict[str, Any] | None = (
                {
                    "title": milestone_title,
                    "workPlanTaskType": "Milestone",   
                    "StartDate": _min_max_dates(proc_tasks)[0],
                    "TaskDueDate": _min_max_dates(proc_tasks)[1],
                }
                if milestone_title
                else None
            )

            task_nodes = [
                {
                    "title": task.get("description"),
                    "workPlanTaskType": "Task",
                    "StartDate": task.get("StartDate"),
                    "TaskDueDate": task.get("TaskDueDate"),
                }
                for task in proc_tasks
            ]

            summary_all_tasks.extend(proc_tasks)

            parent_node: Dict[str, Any] = {
                "title": proc.get("process"),
                "workPlanTaskType": "process",
                "StartDate": _min_max_dates(proc_tasks)[0],
                "TaskDueDate": _min_max_dates(proc_tasks)[1],
                "children": [],
            }

            if milestone_node:
                parent_node["children"].append(milestone_node)
            parent_node["children"].extend(task_nodes)

            summary_children.append(parent_node)

        summary_node: Dict[str, Any] = {
            "title": st.get("summary_task"),
            "workPlanTaskType": "summary_task",
            "StartDate": _min_max_dates(summary_all_tasks)[0],
            "TaskDueDate": _min_max_dates(summary_all_tasks)[1],
            "children": summary_children,
        }
        out.append(summary_node)

    return out


def transform_data_nested_structure(data_in: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
    if not isinstance(data_in, list):
        raise TypeError("Input must be a list of summary_task dictionaries")

    out: List[Dict[str, Any]] = []
    for st in data_in:
        st = deepcopy(st)

        summary_children: List[Dict[str, Any]] = []

        grandparent_title = st.get("summary_task")
        grandparent_children = st.get("summary_tasks_of_grandparent_name", [])

        for parent in grandparent_children:
            parent_title = parent.get("summary_task")
            parent_children = parent.get("summary_tasks_of_parent_name", [])
            if parent_children:
                parent_node: Dict[str, Any] = {
                    "title": parent_title,
                    "workPlanTaskType": "Summary Task",
                    "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    "children": []
                }

                for process in parent_children:
                    process_title = process.get("summary_task")
                    tasks = process.get("tasks", [])
                    milestone_title = process.get("milestone")

                    milestone_node: Dict[str, Any] | None = (
                        {
                            "title": milestone_title,
                            "workPlanTaskType": "Milestone",
                            "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                            "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                        }
                        if milestone_title
                        else None
                    )

                    task_nodes = [
                        {
                            "title": task.get('description'),
                            "workPlanTaskType": "Task",
                            "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                            "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                        }
                        for task in tasks
                    ]

                    parent_node: Dict[str, Any] = {
                        "title": process_title,
                        "workPlanTaskType": "Summary Task",
                        "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                        "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                        "children": []
                    }

                    if milestone_node:
                        parent_node["children"].append(milestone_node)
                    parent_node["children"].extend(task_nodes)

                    parent_node["children"].append(parent_node)
            else:
                parent_node: Dict[str, Any] = {
                    "title": parent_title,
                    "workPlanTaskType": "Summary Task",
                    "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    "children": []
                }
                tasks = parent.get("tasks", [])
                milestone_title = parent.get("milestone")

                milestone_node: Dict[str, Any] | None = (
                    {
                        "title": milestone_title,
                        "workPlanTaskType": "Milestone",
                        "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                        "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    }
                    if milestone_title
                    else None
                )
                task_nodes = [
                    {
                        "title": task,
                        "workPlanTaskType": "Task",
                        "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                        "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    }
                    for task in tasks
                ]
                if milestone_node:
                    parent_node["children"].append(milestone_node)
                parent_node["children"].extend(task_nodes)

            summary_children.append(parent_node)

        # If grand_parent is having milestone add them as well
        grandparent_milestone_title = st.get("milestone")

        grandparent_milestone_node: Dict[str, Any] | None = (
                    {
                        "title": grandparent_milestone_title,
                        "workPlanTaskType": "Milestone",
                        "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                        "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    }
                    if grandparent_milestone_title
                    else None
                )
        
        if grandparent_milestone_node:
            summary_children.append(grandparent_milestone_node)

        # If grand_parent is having direct tasks then add them as well
        grandparent_tasks = st.get("tasks", [])

        if grandparent_tasks:
            for task in grandparent_tasks:
                task_node = {
                    "title": task,
                    "workPlanTaskType": "Task",
                    "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                    "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
                }
                summary_children.append(task_node)

        # Build the grandparent node
        grandparent_node: Dict[str, Any] = {
            "title": grandparent_title,
            "workPlanTaskType": "Summary Task",
            "StartDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
            "TaskDueDate": "2025-02-11T12:00:00+00:00",  # Placeholder date
            "children": summary_children,
        }
        out.append(grandparent_node)

    return out