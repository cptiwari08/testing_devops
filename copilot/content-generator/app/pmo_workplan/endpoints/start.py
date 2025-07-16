import traceback
from datetime import datetime, timezone

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.pmo_workplan.endpoints.paths import WorkplanPaths
from app.pmo_workplan.schemas.workplan_input import WorkplanInput
from app.pmo_workplan.schemas.workplan_output import Output, WorkplanOutput
from app.pmo_workplan.services.workplan_generator import WorkplanGenerator
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, BackgroundTasks, Depends, HTTPException, Query, status
from fastapi.responses import JSONResponse

router = APIRouter()


@router.post(
    WorkplanPaths.start,
    tags=["Workplan"],
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
    input_: WorkplanInput,
    background_tasks: BackgroundTasks,
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
    clear_redis_key_on_start: bool = Query(False),
) -> JSONResponse:

    logger.info(f"Starting workplan, input: {input_.model_dump_json()}")

    instance_id = str(input_.instanceId)
    if clear_redis_key_on_start:
        redis.set(instance_id, "")

    response: dict[str, str] = {
        "instanceId": instance_id,
        "terminatePostUri": WorkplanPaths.terminate.format(instance_id=instance_id),
    }

    if not redis.exists(instance_id):
        logger.error("Error: instanceId does not exist")
        raise HTTPException(
            detail="instanceId does not exist", status_code=status.HTTP_404_NOT_FOUND
        )

    if redis.get(instance_id, WorkplanOutput):
        logger.info("Workplan already running")
        return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)

    workplan = WorkplanOutput(
        name="WorkPlanGenerator",
        instanceId=input_.instanceId,
        runtimeStatus=RuntimeStatus.in_progress,
        input=input_,
        output=Output(),
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis.update(instance_id, workplan)

    async def background_task(input_, redis):
        try:
            workplan_generator = WorkplanGenerator(input_, redis)
            await workplan_generator.generate()
        except Exception as e:
            workplan.lastUpdatedTime = datetime.now(timezone.utc)
            workplan.runtimeStatus = RuntimeStatus.failed
            workplan.errorMessage = str(e)
            redis.update(instance_id, workplan)
            logger.error(f"Unhandled exception: {traceback.format_exc()}")

    background_tasks.add_task(background_task, input_, redis)

    return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)
