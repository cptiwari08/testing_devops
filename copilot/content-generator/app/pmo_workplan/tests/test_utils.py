from app.pmo_workplan.utils import (
    clean_parent_task_name,
    create_citing_sources_prj_dcs,
    extract_page_numbers,
    flatten_team_documents,
    format_output_json,
    merge_unique_dicts,
    string_to_json,
)


def test_format_output_json():
    # Arrange
    id = 1
    title = "Project Title"
    input_json_data = [
        {
            "summary_task": "Summary Task 1",
            "milestone": "Milestone 1",
            "tasks": ["Task 1.1", "Task 1.2"],
        }
    ]
    project_docs = [
        {
            "documentId": "doc1",
            "documentName": "Document 1",
            "chunk_id": "chunk1",
            "chunk_text": "Page 1",
        }
    ]

    # Act
    result = format_output_json(id, title, input_json_data, project_docs, {})

    # Assert
    assert result["projectTeam"]["id"] == id
    assert result["projectTeam"]["title"] == title
    assert len(result["content"]) == 1
    assert result["content"][0]["summary_task"] == "Summary Task 1"
    assert result["content"][0]["milestone"] == "Milestone 1"
    assert result["citingSources"][0]["sourceName"] == "team-specific-project-docs"

def test_create_citing_sources():
    # Arrange
    project_docs = [
        {
            "documentId": "doc1",
            "documentName": "Document 1",
            "chunk_id": "chunk1",
            "chunk_text": "Page 1",
        }
    ]
    title = "Project Title"

    # Act
    result = create_citing_sources_prj_dcs(project_docs, title)

    # Assert
    assert result["sourceName"] == "team-specific-project-docs"
    assert result["sourceType"] == "documents"
    assert (
        "Final generated output to be used to generate workplan for Project Title"
        in result["content"]
    )
    assert len(result["sourceValue"]) == 1
    assert result["sourceValue"][0]["documentId"] == "doc1"


def test_string_to_json():
    # Arrange
    json_string = """
    ```json
    [
        {
            "summary_task": "Summary Task 1",
            "milestone": "Milestone 1",
            "tasks": ["Task 1.1", "Task 1.2"]
        }
    ]
    ```
    """

    # Act
    result = string_to_json(json_string)

    # Assert
    assert isinstance(result, list)
    assert len(result) == 1
    assert result[0]["summary_task"] == "Summary Task 1"


def test_extract_page_numbers():
    # Arrange
    text = "Page 1 -- END for Page Number - 2 Page Number :3"

    # Act
    result = extract_page_numbers(text)

    # Assert
    assert result == [1, 2, 3]


def test_merge_unique_dicts():
    # Arrange
    list_of_lists = [
        [{"documentId": "doc1", "data": "data1"}],
        [{"documentId": "doc2", "data": "data2"}],
        [{"documentId": "doc1", "data": "data1"}],
    ]

    # Act
    result = merge_unique_dicts(list_of_lists)

    # Assert
    assert len(result) == 2
    assert result[0]["documentId"] == "doc1"
    assert result[1]["documentId"] == "doc2"


def test_clean_parent_task_name():
    # Arrange
    parent_str = "Parent Task | 123"

    # Act
    result = clean_parent_task_name(parent_str)

    # Assert
    assert result == "Parent Task"


def test_flatten_team_documents():
    # Arrange
    data = [
        {"title": "Summary Task 1", "workplantasktype": "Summary Task"},
        {
            "title": "Task 1.1",
            "parenttask": "Summary Task 1 | 1",
            "workplantasktype": "Task",
        },
        {
            "title": "Milestone 1",
            "parenttask": "Summary Task 1 | 1",
            "workplantasktype": "Milestone",
        },
    ]

    # Act
    result = flatten_team_documents(data)

    # Assert
    assert len(result) == 1
    assert result[0]["summary_task"] == "Summary Task 1"
    assert result[0]["milestone"] == "Milestone 1"
    assert result[0]["tasks"] == ["Task 1.1"]
