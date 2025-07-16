from unittest.mock import MagicMock


def test_start_do_not_found_instance_id(client):
    # Arrange
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths

    redis_mock = MagicMock()
    redis_mock.exists.return_value = False

    mocks = {
        "redis": redis_mock,
        "status_report_generator": MagicMock(),
    }

    payload = {
        "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
        "projectDetails": {
            "subSector": "Software",
            "sector": "Information Technology",
            "transactionType": "Buy&Integrate",
        },
        "projectTeam": {"id": "59", "title": "HR"},
        "reportingPeriod": {
            "id": "1111", 
            "title": "2024/10/22 - 2024/10/28",
            "periodStartDate": "2024-10-22",
            "periodEndDate": "2024-10-28"
        },
    }

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(StatusReportPaths.start, json=payload)
        
        # Print response for debugging
        print(f"Response status: {response.status_code}")
        print(f"Response content: {response.content}")

        # Assert
        assert response.status_code == 404


def test_start_status_report(client):
    # Arrange
    from datetime import datetime, timezone

    from app.core.schemas import RuntimeStatus
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from app.status_report.schemas.status_report_input import StatusReportInput
    from app.status_report.schemas.status_report_output import StatusReportOutput

    payload = {
        "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
        "projectTeam": {"title": "HR", "id": "59"},
        "reportingPeriod": {
            "id": "1111", 
            "title": "2024/10/22 - 2024/10/28",
            "periodStartDate": "2024-10-22",
            "periodEndDate": "2024-10-28"
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
    redis_mock.get.side_effect = ["", status_report_output]
    redis_mock.exists.return_value = True

    mocks = {
        "redis": redis_mock,
        "status_report_generator": MagicMock(),
    }

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(StatusReportPaths.start, json=payload)

        # Assert
        assert response.status_code == 202


def test_status_report_is_already_runnig(client):
    # Arrange
    from datetime import datetime, timezone

    from app.core.schemas import RuntimeStatus
    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from app.status_report.schemas.status_report_input import StatusReportInput
    from app.status_report.schemas.status_report_output import StatusReportOutput

    payload = {
        "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
        "projectTeam": {"title": "HR", "id": "59"},
        "reportingPeriod": {
            "id": "1111", 
            "title": "2024/10/22 - 2024/10/28",
            "periodStartDate": "2024-10-22",
            "periodEndDate": "2024-10-28"
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
        "status_report_generator": MagicMock(),
    }

    with app.container.override_providers(**mocks): # type: ignore
        # Act
        response = client.post(StatusReportPaths.start, json=payload)

        # Assert
        assert response.status_code == 202


def test_status_report_has_invalid_data_stored(client):
    # Arrange
    from unittest.mock import MagicMock

    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from fastapi import HTTPException, status

    payload = {
        "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
        "projectTeam": {"title": "HR", "id": "59"},
        "reportingPeriod": {
            "id": "1111", 
            "title": "2024/10/22 - 2024/10/28",
            "periodStartDate": "2024-10-22",
            "periodEndDate": "2024-10-28"
        },
        "projectDetails": {
            "sector": "Information Technology",
            "subSector": "Software",
            "transactionType": "Buy&Integrate",
        },
    }

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
        "status_report_generator": MagicMock(),
    }

    with app.container.override_providers(**mocks):  # type: ignore
        # Act
        response = client.post(StatusReportPaths.start, json=payload)

        # Assert
        assert response.status_code == 400


def test_start_handles_500_error(client):
    # Arrange
    from unittest.mock import MagicMock

    from app.main import app
    from app.status_report.endpoints.paths import StatusReportPaths
    from fastapi import HTTPException, status

    payload = {
        "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
        "projectTeam": {"title": "HR", "id": "59"},
        "reportingPeriod": {
            "id": "1111", 
            "title": "2024/10/22 - 2024/10/28",
            "periodStartDate": "2024-10-22",
            "periodEndDate": "2024-10-28"
        },
        "projectDetails": {
            "sector": "Information Technology",
            "subSector": "Software",
            "transactionType": "Buy&Integrate",
        },
    }

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
        "status_report_generator": MagicMock(),
    }

    with app.container.override_providers(**mocks): # type: ignore
        # Act
        response = client.post(StatusReportPaths.start, json=payload)

        # Assert
        assert response.status_code == 500
