from app.core.redis_service import RedisService
from app.dependencies import Container
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, Depends

router = APIRouter()

from app import __version__


@router.get("/healthcheck", tags=["Healthcheck"])
@inject
async def healthcheck(
    redis: RedisService = Depends(Provide[Container.redis]),
):
    client = redis.client
    try:
        if redis_status := client.ping():
            redis_status = "ok"
    except Exception:
        redis_status = "failed"

    return {
        "status": "ok",
        "version": __version__,
        "redis_status": redis_status,
    }
