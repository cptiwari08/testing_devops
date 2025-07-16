from typing import Annotated
from uuid import UUID

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.pmo_workplan.endpoints.paths import WorkplanPaths
from app.pmo_workplan.schemas.workplan_output import WorkplanOutput
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, Depends, HTTPException, Path, Response, status

router = APIRouter()


@router.post(
    WorkplanPaths.terminate,
    tags=["Workplan"],
    status_code=status.HTTP_204_NO_CONTENT,
    responses={
        204: {"description": "Workplan aborted successfully"},
        200: {"description": "Workplan already completed"},
        404: {"description": "Workplan not found"},
        400: {"description": "Invalid JSON format"},
        422: {"description": "The workplan has not been started"},
        500: {"description": "An unexpected error occurred"},
    },
)
@inject
async def terminate(
    instance_id: Annotated[UUID, Path(example="deb954ee-e7c1-4b6e-8eff-15c8dad74bdf")],
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
) -> Response:

    instance_id_str = str(instance_id)
    if not redis.exists(instance_id_str):
        logger.error("Error: Workplan not found")
        raise HTTPException(
            detail="Workplan not found", status_code=status.HTTP_404_NOT_FOUND
        )

    workplan: WorkplanOutput | None = redis.get(instance_id_str, WorkplanOutput)
    if not workplan:
        raise HTTPException(
            detail="The workplan has not been started",
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )

    if workplan.runtimeStatus == RuntimeStatus.completed:
        return Response(
            status_code=status.HTTP_200_OK, content="Workplan already completed"
        )

    workplan.runtimeStatus = RuntimeStatus.aborted
    redis.update(instance_id_str, workplan)

    return Response(status_code=status.HTTP_204_NO_CONTENT)
