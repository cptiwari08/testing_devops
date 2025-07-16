import traceback
from datetime import datetime, timezone

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.program_office_api import ProgramOffice
from app.status_report.config import ProgramOfficeConfig
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.project_charter.endpoints.paths import ProjectCharterPaths
from app.project_charter.schemas.project_charter_input import ProjectCharterInput
from app.project_charter.schemas.project_charter_output import (
    Output,
    ProjectCharterOutput,
)
from app.project_charter.services.project_charter_generator import (
    ProjectCharterGenerator,
)
from app.project_charter.services.ai_search import AISearch
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, BackgroundTasks, Depends, HTTPException, Query, status
from fastapi.responses import JSONResponse

router = APIRouter()


@router.post(
    ProjectCharterPaths.start,
    tags=["Project Charter"],
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
    input_: ProjectCharterInput,
    background_tasks: BackgroundTasks,
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
    program_office: ProgramOffice = Depends(Provide[Container.program_office]),
    program_office_config: ProgramOfficeConfig = Depends(Provide[Container.program_office_config]),
    ai_search: AISearch = Depends(Provide[Container.ai_search]),    
    clear_redis_key_on_start: bool = Query(False)
) -> JSONResponse:

    logger.info(f"Starting project charter, input: {input_.model_dump_json()}")

    instance_id = str(input_.instanceId)
    if clear_redis_key_on_start:
        redis.set(instance_id, "")

    response: dict[str, str] = {
        "instanceId": instance_id,
        "terminatePostUri": ProjectCharterPaths.terminate.format(
            instance_id=instance_id
        ),
    }

    if not redis.exists(instance_id):
        logger.error("Error: instanceId does not exist")
        raise HTTPException(
            detail="instanceId does not exist", status_code=status.HTTP_404_NOT_FOUND
        )

    if redis.get(instance_id, ProjectCharterOutput):
        logger.info("Project charter already running")
        return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)

    project_charter = ProjectCharterOutput(
        name="ProjectCharter",
        instanceId=input_.instanceId,
        runtimeStatus=RuntimeStatus.in_progress,
        input=input_,
        output=Output(),
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis.update(instance_id, project_charter)

    async def background_task(input_, redis, program_office, program_office_config, ai_search):
        try:
            project_charter_generator = ProjectCharterGenerator(
                input_, 
                redis=redis, 
                logger=logger, 
                program_office=program_office, 
                program_office_config=program_office_config, 
                ai_search=ai_search
            )
            await project_charter_generator.generate()
        except Exception as e:
            project_charter.lastUpdatedTime = datetime.now(timezone.utc)
            project_charter.runtimeStatus = RuntimeStatus.failed
            project_charter.errorMessage = str(e)
            redis.update(instance_id, project_charter)
            logger.error(f"Unhandled exception: {traceback.format_exc()}")

    background_tasks.add_task(background_task, input_, redis, program_office, program_office_config, ai_search)

    return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)
