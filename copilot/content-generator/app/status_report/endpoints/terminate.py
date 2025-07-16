from typing import Annotated
from uuid import UUID

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.status_report.endpoints.paths import StatusReportPaths
from app.status_report.schemas.status_report_output import StatusReportOutput
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, Depends, HTTPException, Path, Response, status

router = APIRouter()


@router.post(
    StatusReportPaths.terminate,
    tags=["Status Report"],
    status_code=status.HTTP_204_NO_CONTENT,
    responses={
        204: {"description": "Status report aborted successfully"},
        200: {"description": "Status report already completed"},
        404: {"description": "Status report not found"},
        400: {"description": "Invalid JSON format"},
        422: {"description": "The status report has not been started"},
        500: {"description": "An unexpected error occurred"},
    },
)
@inject
async def terminate(
    instance_id: Annotated[UUID, Path(example="6bd23cc3-9f08-42d6-910b-32975a232d9a")],
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
) -> Response:

    instance_id_str = str(instance_id)
    if not redis.exists(instance_id_str):
        logger.error("Status report not found")
        raise HTTPException(
            detail="Status report not found", status_code=status.HTTP_404_NOT_FOUND
        )

    status_report: StatusReportOutput | None = redis.get(instance_id_str, StatusReportOutput)
    if not status_report:
        raise HTTPException(
            detail="The status report has not been started",
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )

    if status_report.runtimeStatus == RuntimeStatus.completed:
        return Response(
            status_code=status.HTTP_200_OK, content="Status report already completed"
        )

    status_report.runtimeStatus = RuntimeStatus.aborted
    redis.update(instance_id_str, status_report)

    return Response(status_code=status.HTTP_204_NO_CONTENT)
