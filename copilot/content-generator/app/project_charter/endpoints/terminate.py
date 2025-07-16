from typing import Annotated
from uuid import UUID

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.project_charter.endpoints.paths import ProjectCharterPaths
from app.project_charter.schemas.project_charter_output import ProjectCharterOutput
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, Depends, HTTPException, Path, Response, status

router = APIRouter()


@router.post(
    ProjectCharterPaths.terminate,
    tags=["Project Charter"],
    status_code=status.HTTP_204_NO_CONTENT,
    responses={
        204: {"description": "Project Charter aborted successfully"},
        200: {"description": "Project Charter already completed"},
        404: {"description": "Project Charter not found"},
        400: {"description": "Invalid JSON format"},
        422: {"description": "The Project Charter has not been started"},
        500: {"description": "An unexpected error occurred"},
    },
)
@inject
async def terminate(
    instance_id: Annotated[UUID, Path(example="3c389a84-6490-4dc1-a8e0-c1ad893c9534")],
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
) -> Response:

    instance_id_str = str(instance_id)
    if not redis.exists(instance_id_str):
        logger.error("Error: Project Charter not found")
        raise HTTPException(
            detail="Project Charter not found", status_code=status.HTTP_404_NOT_FOUND
        )

    project_charter: ProjectCharterOutput | None = redis.get(
        instance_id_str, ProjectCharterOutput
    )
    if not project_charter:
        raise HTTPException(
            detail="The Project Charter has not been started",
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )

    if project_charter.runtimeStatus == RuntimeStatus.completed:
        return Response(
            status_code=status.HTTP_200_OK, content="Project Charter already completed"
        )

    project_charter.runtimeStatus = RuntimeStatus.aborted
    redis.update(instance_id_str, project_charter)

    return Response(status_code=status.HTTP_204_NO_CONTENT)
