from typing import Annotated
from uuid import UUID

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.tsa_generator.endpoints.paths import TSAPaths
from app.tsa_generator.schemas.tsa_generator_output import TSAGeneratorOutput
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, Depends, HTTPException, Path, Response, status

router = APIRouter()


@router.post(
    TSAPaths.terminate,
    tags=["TSA Generator"],
    status_code=status.HTTP_204_NO_CONTENT,
    responses={
        204: {"description": "Project TSA aborted successfully"},
        200: {"description": "Project TSA already completed"},
        404: {"description": "Project TSA not found"},
        400: {"description": "Invalid JSON format"},
        422: {"description": "The Project TSA has not been started"},
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
        logger.error("Error: Project TSA not found")
        raise HTTPException(
            detail="Project TSA not found", status_code=status.HTTP_404_NOT_FOUND
        )

    project_tsa: TSAGeneratorOutput | None = redis.get(
        instance_id_str, TSAGeneratorOutput
    )
    if not project_tsa:
        raise HTTPException(
            detail="The Project TSA has not been started",
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )

    if project_tsa.runtimeStatus == RuntimeStatus.completed:
        return Response(
            status_code=status.HTTP_200_OK, content="Project TSA already completed"
        )

    project_tsa.runtimeStatus = RuntimeStatus.aborted
    redis.update(instance_id_str, project_tsa)

    return Response(status_code=status.HTTP_204_NO_CONTENT)
