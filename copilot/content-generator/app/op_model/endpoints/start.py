import traceback, os
from datetime import datetime, timezone
from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.program_office_api import ProgramOffice
from app.status_report.config import ProgramOfficeConfig
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.op_model.endpoints.paths import OPModelPaths
from app.op_model.schemas.op_model_input import OPModelInput
from app.op_model.schemas.op_model_output import (
    Output,
    OPModelOutput,
)
from app.op_model.services.op_model_generator import (
    OPModelGenerator,
)
from app.core.ai_search import AISearch
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, BackgroundTasks, Depends, HTTPException, Query, status
from fastapi.responses import JSONResponse

router = APIRouter()
log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "


@router.post(
    OPModelPaths.start,
    tags=["OP Model"],
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
    input_: OPModelInput,
    background_tasks: BackgroundTasks,
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
    program_office: ProgramOffice = Depends(Provide[Container.program_office]),
    program_office_config: ProgramOfficeConfig = Depends(Provide[Container.program_office_config]),
    ai_search: AISearch = Depends(Provide[Container.ai_search]),    
    clear_redis_key_on_start: bool = Query(False)
) -> JSONResponse:

    logger.info(f"{log_mes_base} Starting OP model, input: {input_.model_dump_json()}")

    instance_id = str(input_.instanceId)
    if clear_redis_key_on_start:
        redis.set(instance_id, "")

    response: dict[str, str] = {
        "instanceId": instance_id,
        "terminatePostUri": OPModelPaths.terminate.format(
            instance_id=instance_id
        ),
    }

    if not redis.exists(instance_id):
        logger.error(f"{log_mes_base} Error: instanceId does not exist")
        raise HTTPException(
            detail="instanceId does not exist", status_code=status.HTTP_404_NOT_FOUND
        )

    if redis.get(instance_id, OPModelOutput):
        logger.info(f"{log_mes_base} OP model already running")
        return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)

    op_model = OPModelOutput(
        name="OPModel",
        instanceId=input_.instanceId,
        runtimeStatus=RuntimeStatus.in_progress,
        input=input_,
        output=Output(),
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis.update(instance_id, op_model)

    async def background_task(input_, redis, program_office, program_office_config, ai_search):
        try:
            op_model_generator = OPModelGenerator(
                input_, 
                redis=redis, 
                logger=logger, 
                program_office=program_office, 
                program_office_config=program_office_config, 
                ai_search=ai_search
            )
            await op_model_generator.generate()
        except Exception as e:
            op_model.lastUpdatedTime = datetime.now(timezone.utc)
            op_model.runtimeStatus = RuntimeStatus.failed
            op_model.errorMessage = str(e)
            redis.update(instance_id, op_model)
            logger.error(f"{log_mes_base} Unhandled exception: {traceback.format_exc()}")

    background_tasks.add_task(background_task, input_, redis, program_office, program_office_config, ai_search)

    return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)
