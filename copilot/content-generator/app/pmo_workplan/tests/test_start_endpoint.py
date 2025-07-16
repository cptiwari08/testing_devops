from unittest.mock import MagicMock


def test_start_do_not_found_instance_id(client):
    # Arrange

    from app.main import app
    from app.pmo_workplan.endpoints.paths import WorkplanPaths

    redis_mock = MagicMock()
    redis_mock.exists.return_value = False

    mocks = {
        "redis": redis_mock,
    }

    payload = {
        "durationInMonth": 12,
        "instanceId": "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf",
        "projectDetails": {
            "sector": "Healthcare",
            "subSector": "Healthcare Technology",
            "transactionType": "Sell & Separate",
        },
        "projectDocs": [
            "b3cad6d7-aa82-45d6-97af-17f22484d7ac",
            "ca2ef4ae-778e-487e-a4c0-4e7a45612494",
            "40f0c817-9259-4ce4-b3c7-4ca821006774",
            "4497bc30-6e73-4729-a153-fba9bdde9d95",
        ],
        "projectTeams": [{"id": 10001, "title": "HR"}, {"id": 10002, "title": "IT"}],
    }

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(WorkplanPaths.start, json=payload)

        # Assert
        assert response.status_code == 404


def test_start_workplan(client):
    # Arrange
    from datetime import datetime, timezone
    from unittest.mock import MagicMock

    from app.core.schemas import RuntimeStatus
    from app.main import app
    from app.pmo_workplan.endpoints.paths import WorkplanPaths
    from app.pmo_workplan.schemas.workplan_input import WorkplanInput
    from app.pmo_workplan.schemas.workplan_output import Output, WorkplanOutput

    payload = {
        "durationInMonth": 12,
        "instanceId": "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf",
        "projectDetails": {
            "sector": "Healthcare",
            "subSector": "Healthcare Technology",
            "transactionType": "Sell & Separate",
        },
        "projectDocs": [
            "b3cad6d7-aa82-45d6-97af-17f22484d7ac",
            "ca2ef4ae-778e-487e-a4c0-4e7a45612494",
            "40f0c817-9259-4ce4-b3c7-4ca821006774",
            "4497bc30-6e73-4729-a153-fba9bdde9d95",
        ],
        "projectTeams": [{"id": 10001, "title": "HR"}, {"id": 10002, "title": "IT"}],
    }

    workplan_input = WorkplanInput(**payload)
    workplan_output = WorkplanOutput(
        name="WorkPlanGenerator",
        instanceId=payload["instanceId"],
        runtimeStatus=RuntimeStatus.in_progress,
        input=workplan_input,
        output=Output(),
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    # TODO: fix this test, this is basically mocking everything
    mocks = {
        "ai_search": MagicMock(),
        "ai_search_config": MagicMock(),
        "asset_manager_engine": MagicMock(),
        "asset_manager_config": MagicMock(),
        "embed_model": MagicMock(),
        "embed_model_config": MagicMock(),
        "key_vault": MagicMock(),
        "llm": MagicMock(),
        "llm_config": MagicMock(),
        "logger": MagicMock(),
        "redis": MagicMock(),
    }
    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(WorkplanPaths.start, json=payload)

        # Assert
        assert response.status_code == 202


def test_workplan_is_already_runnig(client):
    # Arrange
    from datetime import datetime, timezone

    from app.core.schemas import RuntimeStatus
    from app.main import app
    from app.pmo_workplan.endpoints.paths import WorkplanPaths
    from app.pmo_workplan.schemas.workplan_input import WorkplanInput
    from app.pmo_workplan.schemas.workplan_output import Output, WorkplanOutput

    payload = {
        "durationInMonth": 12,
        "instanceId": "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf",
        "projectDetails": {
            "sector": "Healthcare",
            "subSector": "Healthcare Technology",
            "transactionType": "Sell & Separate",
        },
        "projectDocs": [
            "b3cad6d7-aa82-45d6-97af-17f22484d7ac",
            "ca2ef4ae-778e-487e-a4c0-4e7a45612494",
            "40f0c817-9259-4ce4-b3c7-4ca821006774",
            "4497bc30-6e73-4729-a153-fba9bdde9d95",
        ],
        "projectTeams": [{"id": 10001, "title": "HR"}, {"id": 10002, "title": "IT"}],
    }
    workplan_input = WorkplanInput(**payload)
    workplan_output = WorkplanOutput(
        name="WorkPlanGenerator",
        instanceId=payload["instanceId"],
        runtimeStatus=RuntimeStatus.in_progress,
        input=workplan_input,
        output=Output(),
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.return_value = workplan_output

    mocks = {
        "redis": redis_mock,
    }

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(WorkplanPaths.start, json=payload)

        # Assert
        assert response.status_code == 202


def test_workplan_has_invalid_data_stored(client):
    # Arrange

    from app.main import app
    from app.pmo_workplan.endpoints.paths import WorkplanPaths
    from fastapi import HTTPException, status

    payload = {
        "durationInMonth": 12,
        "instanceId": "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf",
        "projectDetails": {
            "sector": "Healthcare",
            "subSector": "Healthcare Technology",
            "transactionType": "Sell & Separate",
        },
        "projectDocs": [
            "b3cad6d7-aa82-45d6-97af-17f22484d7ac",
            "ca2ef4ae-778e-487e-a4c0-4e7a45612494",
            "40f0c817-9259-4ce4-b3c7-4ca821006774",
            "4497bc30-6e73-4729-a153-fba9bdde9d95",
        ],
        "projectTeams": [{"id": 10001, "title": "HR"}, {"id": 10002, "title": "IT"}],
    }

    get_method_mock = MagicMock(
        side_effect=HTTPException(
            detail="Invalid JSON format", status_code=status.HTTP_400_BAD_REQUEST
        )
    )

    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.side_effect = get_method_mock

    mocks = {
        "redis": redis_mock,
    }
    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(WorkplanPaths.start, json=payload)

        # Assert
        assert response.status_code == 400


def test_start_handles_500_error(client):
    # Arrange

    from app.main import app
    from app.pmo_workplan.endpoints.paths import WorkplanPaths
    from fastapi import HTTPException, status

    payload = {
        "durationInMonth": 12,
        "instanceId": "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf",
        "projectDetails": {
            "sector": "Healthcare",
            "subSector": "Healthcare Technology",
            "transactionType": "Sell & Separate",
        },
        "projectDocs": [
            "b3cad6d7-aa82-45d6-97af-17f22484d7ac",
            "ca2ef4ae-778e-487e-a4c0-4e7a45612494",
            "40f0c817-9259-4ce4-b3c7-4ca821006774",
            "4497bc30-6e73-4729-a153-fba9bdde9d95",
        ],
        "projectTeams": [{"id": 10001, "title": "HR"}, {"id": 10002, "title": "IT"}],
    }

    get_method_mock = MagicMock(
        side_effect=HTTPException(
            detail="Invalid JSON format",
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        )
    )
    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.side_effect = get_method_mock

    mocks = {
        "redis": redis_mock,
    }
    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(WorkplanPaths.start, json=payload)

        # Assert
        assert response.status_code == 500
