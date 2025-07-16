from datetime import datetime, timezone
from unittest.mock import MagicMock


def test_status_report_aborted_successfully(client):
    # Arrange
    from app.core.schemas import RuntimeStatus
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from app.status_report.schemas.status_report_input import StatusReportInput
    from app.status_report.schemas.status_report_output import StatusReportOutput

    instance_id = "6bd23cc3-9f08-42d6-910b-32975a232d9a"
    payload = {
        "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
        "projectTeam": {"title": "HR", "id": "59"},
        "reportingPeriod": {
            "title": "2024/10/22 - 2024/10/28", 
            "id": "1111",
            "periodStartDate": "2024-10-22",  # Added required field
            "periodEndDate": "2024-10-28"     # Added required field
        },
        "projectDetails": {
            "sector": "Information Technology",
            "subSector": "Software",
            "transactionType": "Buy&Integrate",
        },
    }
    status_report_input = StatusReportInput(**payload)
    status_report_output = StatusReportOutput(
        name="StatusReportGenerator",
        instanceId=payload["instanceId"],
        runtimeStatus=RuntimeStatus.in_progress,
        input=status_report_input,
        output=None,
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.return_value = status_report_output

    mocks = {
        "redis": redis_mock,
    }

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(
            StatusReportPaths.terminate.format(instance_id=instance_id)
        )

        # Assert
        assert response.status_code == 204


def test_status_report_already_completed(client):
    # Arrange
    from datetime import datetime, timezone

    from app.core.schemas import RuntimeStatus
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from app.status_report.schemas.status_report_input import StatusReportInput
    from app.status_report.schemas.status_report_output import StatusReportOutput

    instance_id = "6bd23cc3-9f08-42d6-910b-32975a232d9a"
    payload = {
        "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
        "projectTeam": {"title": "HR", "id": "59"},
        "reportingPeriod": {
            "title": "2024/10/22 - 2024/10/28", 
            "id": "1111",
            "periodStartDate": "2024-10-22",  # Added required field
            "periodEndDate": "2024-10-28"     # Added required field
        },
        "projectDetails": {
            "sector": "Information Technology",
            "subSector": "Software",
            "transactionType": "Buy&Integrate",
        },
    }
    status_report_input = StatusReportInput(**payload)
    status_report_output = StatusReportOutput(
        name="StatusReportGenerator",
        instanceId=payload["instanceId"],
        runtimeStatus=RuntimeStatus.completed,
        input=status_report_input,
        output=None,
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.return_value = status_report_output

    mocks = {
        "redis": redis_mock,
    }

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(
            StatusReportPaths.terminate.format(instance_id=instance_id)
        )

        # Assert
        assert response.status_code == 200


def test_status_report_not_found(client):
    # Arrange
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths

    redis_mock = MagicMock()
    redis_mock.exists.return_value = False

    mocks = {"redis": redis_mock}

    instance_id = "6bd23cc3-9f08-42d6-910b-32975a232d9a"

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(
            StatusReportPaths.terminate.format(instance_id=instance_id)
        )

        # Assert
        assert response.status_code == 404


def test_invalid_json_format(client):
    # Arrange
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from fastapi import HTTPException, status

    class MockExceptionError:
        def __init__(self, *args, **kwargs):
            raise HTTPException(
                detail="Invalid JSON format", status_code=status.HTTP_400_BAD_REQUEST
            )

    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.side_effect = MockExceptionError

    mocks = {
        "redis": redis_mock,
    }

    instance_id = "6bd23cc3-9f08-42d6-910b-32975a232d9a"

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(StatusReportPaths.terminate.format(instance_id=instance_id))

        # Assert
        assert response.status_code == 400


def test_the_status_report_has_not_been_started(client):
    # Arrange
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths

    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.return_value = ""

    mocks = {
        "redis": redis_mock,
    }

    instance_id = "6bd23cc3-9f08-42d6-910b-32975a232d9a"

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(StatusReportPaths.terminate.format(instance_id=instance_id))

        # Assert
        assert response.status_code == 422


def test_an_unexpected_error_occurred(client):
    # Arrange
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from fastapi import HTTPException, status

    class MockExceptionError:
        def __init__(self, *args, **kwargs):
            raise HTTPException(
                detail=f"An unexpected error occurred",
                status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            )

    redis_mock = MagicMock()
    redis_mock.exists.return_value = True
    redis_mock.get.side_effect = MockExceptionError

    mocks = {
        "redis": redis_mock,
    }

    instance_id = "6bd23cc3-9f08-42d6-910b-32975a232d9a"

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(StatusReportPaths.terminate.format(instance_id=instance_id))

        # Assert
        assert response.status_code == 500
