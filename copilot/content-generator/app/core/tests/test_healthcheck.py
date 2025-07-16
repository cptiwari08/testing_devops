from unittest.mock import MagicMock


def test_redis_healthcheck_failed(client):

    from app.main import app

    redis_mock = MagicMock()
    redis_mock.client = MagicMock()
    redis_mock.client.ping = MagicMock(side_effect=Exception("side effect exception"))

    mocks = {
        "redis": redis_mock,
    }
    with app.container.override_providers(**mocks):  # type: ignore
        response = client.get("/healthcheck")
        assert response.json()["redis_status"] == "failed"
        assert response.status_code == 200


def test_redis_healthcheck_ok(client):

    from app.main import app

    redis_mock = MagicMock()
    redis_mock.client = MagicMock()
    redis_mock.client.ping = MagicMock(return_value=True)

    mocks = {
        "redis": redis_mock,
    }
    with app.container.override_providers(**mocks):  # type: ignore
        response = client.get("/healthcheck")
        assert response.json()["redis_status"] == "ok"
        assert response.status_code == 200
