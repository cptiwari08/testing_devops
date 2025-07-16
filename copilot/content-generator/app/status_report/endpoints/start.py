import traceback
from datetime import datetime, timezone

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.status_report.endpoints.paths import StatusReportPaths
from app.status_report.schemas.status_report_input import StatusReportInput
from app.status_report.schemas.status_report_output import StatusReportOutput
from app.status_report.services.status_report_generator import StatusReportGenerator
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, BackgroundTasks, Depends, HTTPException, Query, status
from fastapi.responses import JSONResponse

router = APIRouter()


@router.post(
    StatusReportPaths.start,
    tags=["Status Report"],
    status_code=status.HTTP_202_ACCEPTED,
    responses={
        202: {"description": "Accepted"},
        404: {"description": "instanceId does not exist"},
        400: {"description": "Invalid JSON format"},
        500: {"description": "An unexpected error occurred"},
    },
)
@inject
async def start(
    input_: StatusReportInput,
    background_tasks: BackgroundTasks,
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
    status_report_generator: StatusReportGenerator = Depends(
        Provide[Container.status_report_generator]
    ),
    clear_redis_key_on_start: bool = Query(False),
) -> JSONResponse:

    logger.info(f"Starting status report, input: {input_.model_dump_json()}")

    instance_id = str(input_.instanceId)
    if clear_redis_key_on_start:
        redis.set(instance_id, "")

    response: dict[str, str] = {
        "instanceId": instance_id,
        "terminatePostUri": StatusReportPaths.terminate.format(instance_id=instance_id),
    }

    if not redis.exists(instance_id):
        logger.error("instanceId does not exist")
        raise HTTPException(
            detail="instanceId does not exist", status_code=status.HTTP_404_NOT_FOUND
        )

    if redis.get(instance_id, StatusReportOutput):
        logger.warning("Status report already running")
        return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)

    status_report = StatusReportOutput(
        name="StatusReportGenerator",
        instanceId=input_.instanceId,
        runtimeStatus=RuntimeStatus.in_progress,
        input=input_,
        output=None,
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis.update(instance_id, status_report)

    async def background_task(input_) -> None:
        try:
            await status_report_generator.generate(input_)
        except Exception as e:
            status_report.lastUpdatedTime = datetime.now(timezone.utc)
            status_report.runtimeStatus = RuntimeStatus.failed
            status_report.errorMessage = str(e)
            redis.update(instance_id, status_report)
            logger.error(f"Unhandled exception: {traceback.format_exc()}")

    background_tasks.add_task(background_task, input_)

    return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)
