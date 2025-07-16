import json
import re
from uuid import UUID
from operator import itemgetter
from itertools import groupby

def create_citing_sources(project_docs, title:str, document_type="team-specific-project-docs"):
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
    citing_sources['content'] = f"Final generated output to be used to generate tsa for {title}"
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

        # Validate that the data is a list of dictionaries with required keys
        required_keys = {"title", "serviceInScopeDescription"}
        if isinstance(json_data, list) and all(
            isinstance(item, dict) for item in json_data
        ):
            for item in json_data:
                if not required_keys.issubset(item.keys()):
                    raise ValueError(
                        f"JSON content does not have the required keys: {required_keys}"
                    )

        else:
            raise ValueError("JSON content is not a list of dictionaries.")

        # If validation passes, return the json_data
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

def create_citing_sources_templates(templates_dict, title:str, document_type="ey-ip"):
    citing_sources = {}
    citing_sources['sourceName'] = document_type
    citing_sources['sourceType'] = "EYIP Database"
    citing_sources['content'] = f"Final generated output to be used to generate tsa for {title}"
    if not templates_dict:
        source_value = {"tableName": "TSAItems",
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

    source_value = {"tableName": "TSAItems",
                    "rowCount": row_number,
                    "sqlQuery": sqlQuery
                }
    citing_sources['sourceValue'] = [source_value]
    return citing_sources


def create_citing_sources_ait(ait_dict, title:str, document_type="app-inventory-tracker"):
    citing_sources = {}
    citing_sources['sourceName'] = document_type
    citing_sources['sourceType'] = "App Inventory Tracker App"
    citing_sources['content'] = f"Final generated output to be used to generate tsa for {title}"
    if not ait_dict:
        source_value = {"tableName": "AppInventory",
                    "rowCount": 0,
                    "sqlQuery": ""
                }
        citing_sources['sourceValue'] = [source_value]
        return citing_sources

    if (isinstance(ait_dict, dict)) and (ait_dict.get('ait_data')):
        row_number = row_count(ait_dict.get("ait_data")) 
        sqlQuery = ait_dict.get("sql_query", None)
        if not sqlQuery:
            sqlQuery = ""
    else: 
        row_number = 0
        sqlQuery = ""

    source_value = {"tableName": "AppInventory",
                    "rowCount": row_number,
                    "sqlQuery": sqlQuery
                }
    citing_sources['sourceValue'] = [source_value]
    return citing_sources

def create_citing_sources_op_models(op_models, title:str, document_type="op-model"):
    citing_sources = {}
    citing_sources['sourceName'] = document_type
    citing_sources['sourceType'] = title
    citing_sources['content'] = f"Final generated output to be used to generate tsa for {title}"
    if not op_models:
        source_value = {
                    "Category": "",
                    "rowCount": 0
                }
        citing_sources['sourceValue'] = [source_value]
        return citing_sources

    if (isinstance(op_models, list)):
        source_value = row_count(op_models) 
    else: 
        source_value = {
                    "Category": "",
                    "rowCount": 0
                }

    citing_sources['sourceValue'] = [source_value]
    return citing_sources

def row_count(templates):
    try:
        total_items = len(templates)
    except:
        total_items = 0
    return total_items

