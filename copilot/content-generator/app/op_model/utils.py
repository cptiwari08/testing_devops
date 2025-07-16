import json
import re
from uuid import UUID
from llama_index.core.base.llms.types  import CompletionResponse

def create_citing_sources(project_docs, document_type="team-specific-project-docs"):
    if not project_docs:        
        return {
            "sourceName": "team-specific-project-docs",
            "sourceType": "documents",
            "content": "",
            "sourceValue": [{
                "documentId": UUID("00000000-0000-0000-0000-000000000000"),
                "documentName": "",
                "chunk_id": UUID("00000000-0000-0000-0000-000000000000"),
                "pages": [],
                "chunk_text": ""
            }]
        }
    citing_sources = {}
    citing_sources['sourceName'] = document_type
    citing_sources['sourceType'] = "documents"
    citing_sources['content'] = f"Final generated output to be used to generate operating model"
    project_docs_list = []
    chunks_ids = []
    if project_docs:
        for doc in project_docs:
            if doc['chunk_id'] not in chunks_ids:
                current_chunk = {}
                page_numbers = extract_page_numbers(doc['chunk_text'])
                current_chunk['documentId'] = doc['documentId']
                current_chunk['documentName'] = doc['documentName']
                current_chunk['chunk_id'] = doc['chunk_id']
                current_chunk['pages'] = page_numbers
                current_chunk['chunk_text'] = doc['chunk_text']
                project_docs_list.append(current_chunk)
    citing_sources['sourceValue'] = project_docs_list
    return citing_sources

def string_to_json(string):
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


def extract_page_numbers(text):
    pattern1 = re.compile(r'Page (\d+)|-- END for Page Number - (\d+)')
    pattern2 = re.compile(r'Page Number :(\d+)')

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
            if dictionary['documentId'] not in seen_ids:
                seen_ids.add(dictionary['documentId'])
                unique_dicts.append(dictionary)
    return unique_dicts
def clean_parent_task_name(parent_str):
    if parent_str is None:
        return None
    # Strip off the " | [nnn]" part if present
    return re.split(r'\|', parent_str)[0].strip()

def flatten_team_documents(data):
    # Identify all summary tasks
    # A summary task is an entry with workplantasktype == "Summary Task"
    summary_tasks = [doc for doc in data if doc.get("workplantasktype") == "Summary Task"]

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
            "milestone": milestone if milestone else ""
        }
        results.append(result_obj)

    return results

def create_citing_sources_templates(templates_dict, document_type="ey-ip"):
    citing_sources = {}
    citing_sources['sourceName'] = document_type
    citing_sources['sourceType'] = "EYIP Database"
    citing_sources['content'] = f"Final generated output to be used to generate operating model"
    if not templates_dict:
        source_value = {"tableName": "Nodes",
                    "rowCount": 0,
                    "sqlQuery": ""
                }
        citing_sources['sourceValue'] = [source_value]
        return citing_sources

    if (isinstance(templates_dict, dict)) and (templates_dict.get('eyip')):
        row_number = row_count(templates_dict.get("eyip")) 
        sqlQuery = templates_dict.get("sql_query", None)
        if not sqlQuery:
            sqlQuery = ""
    else: 
        row_number = 0
        sqlQuery = ""

    source_value = {"tableName": "Nodes",
                    "rowCount": row_number,
                    "sqlQuery": sqlQuery
                }
    citing_sources['sourceValue'] = [source_value]
    return citing_sources

def row_count(templates):
    try:
        total_items = len(templates)
    except:
        total_items = 0
    return total_items

def transform_operating_model(data, business_function=None):
    transformed = []
    # For lvl1 in data:
    if data:
        lvl1_node = {
            "title": data["process_lvl_1"],
            "NodeType": {"key": "PROCESS_GROUP"},
            "children": []
        }
        for lvl2 in data.get("sub_processes", []):
            lvl2_node = {
                "title": lvl2["process_lvl_2"],
                "NodeType": {"key": "PROCESS_GROUP"},
                "children": []
            }
            for lvl3_title in lvl2.get("processes", []):
                lvl3_node = {
                    "title": lvl3_title,
                    "NodeType": {"key": "PROCESS"}
                }
                lvl2_node["children"].append(lvl3_node)
            lvl1_node["children"].append(lvl2_node)
    else:
        lvl1_node = {
            "title": business_function,
            "NodeType": {"key": "PROCESS_GROUP"},
            "children": []
        }
    
    transformed.append(lvl1_node)
    
    # If business_function is not None:
    for node in transformed:
        node["business_function"] = business_function
    return transformed

def clean_json_response(response):
    if isinstance(response, CompletionResponse):
        clean_response = response.text.strip()
    else:
        clean_response = response.strip()

    if "```python" in clean_response:
        clean_response = re.sub(r"\s+"," ", re.sub(r"^```python\n|\n```$", "", clean_response))
    if "```json" in clean_response:
        clean_response = re.sub(r"\s+"," ", re.sub(r"^```json\n|\n```$", "", clean_response))

    clean_response = re.sub(r"{{","{", clean_response)
    clean_response = re.sub(r"}}","}", clean_response)
    
    if re.search(r"{\s?'|{\s+'", clean_response):
            clean_response = re.sub(r"'","\"",clean_response)

    return clean_response

def safe_json_loads(json_string):
    """
    Safely loads a JSON string, handling common issues like trailing commas.
    """
    try:
        return json.loads(json_string)
    except json.JSONDecodeError as e:
        raise ValueError(f"Invalid JSON format: {e}") from e
    except Exception as e:
        raise ValueError(f"An error occurred while processing JSON: {e}") from e
    
def loads_clean_json_response(response):
    """
    Cleans and loads a JSON response, handling various formatting issues.
    """
    clean_response = clean_json_response(response)
    return safe_json_loads(clean_response)
