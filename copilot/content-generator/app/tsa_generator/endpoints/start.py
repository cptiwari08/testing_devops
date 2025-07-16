import traceback
from datetime import datetime, timezone
from typing import Union, Annotated

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.program_office_api import ProgramOffice
from app.status_report.config import ProgramOfficeConfig
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.tsa_generator.endpoints.paths import TSAPaths
from app.tsa_generator.schemas.tsa_generator_input import (
    TSAGeneratorInput,
    tsa_generator_input_example
)
from app.tsa_generator.schemas.scope_generator_input import (
    ScopeGeneratorInput,
    scope_generator_input_example
)
from app.tsa_generator.schemas.tsa_generator_output import (
    Output,
    TSAGeneratorOutput,
)
from app.tsa_generator.services.tsa_generator import TSAGenerator
from app.tsa_generator.services.scope_generator import ScopeGenerator
from app.project_charter.services.ai_search import AISearch
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, BackgroundTasks, Depends, HTTPException, Query, status, Body
from fastapi.responses import JSONResponse

router = APIRouter()

examples = {
    "tsa_input": {
        "summary": "Example of a request for generating a TSA",
        "description": "This request will generate the TSA for the project.",
        "value": tsa_generator_input_example,
    },
    "scope_input": {
        "summary": "Example of a request for generating a Scope",
        "description": "This request will generate the Scope for the project. "
        "Service title and other related information must already be set in the TSA.",
        "value": scope_generator_input_example,
    },
}


@router.post(
    TSAPaths.start,
    tags=["TSA Generator"],
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
    input_: Annotated[Union[TSAGeneratorInput, ScopeGeneratorInput],
                      Body(openapi_examples=examples)],
    background_tasks: BackgroundTasks,
    logger: LoggerConfig = Depends(Provide[Container.logger]),
    redis: RedisService = Depends(Provide[Container.redis]),
    program_office: ProgramOffice = Depends(Provide[Container.program_office]),
    program_office_config: ProgramOfficeConfig = Depends(Provide[Container.program_office_config]),
    ai_search: AISearch = Depends(Provide[Container.ai_search]),    
    clear_redis_key_on_start: bool = Query(False)
) -> JSONResponse:

    logger.info(f"Starting project TSA, input: {input_.model_dump_json()}")
    logger.info(f"TSA Endpoint: Processing request for instanceId: {input_.instanceId}")

    instance_id = str(input_.instanceId)
    if clear_redis_key_on_start:
        logger.info(f"TSA Endpoint: Clearing Redis key for instanceId: {instance_id}")
        redis.set(instance_id, "")

    response: dict[str, str] = {
        "instanceId": instance_id,
        "terminatePostUri": TSAPaths.terminate.format(
            instance_id=instance_id
        ),
    }
    logger.info(f"TSA Endpoint: Response prepared: {response}")

    if not redis.exists(instance_id):
        logger.error(f"TSA Endpoint: Error - instanceId {instance_id} does not exist in Redis")
        raise HTTPException(
            detail="instanceId does not exist", status_code=status.HTTP_404_NOT_FOUND
        )

    if redis.get(instance_id, TSAGeneratorOutput):
        logger.info(f"TSA Endpoint: Project TSA already running for instanceId: {instance_id}")
        return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)

    # Detailed logging for TSA initialization
    logger.info(f"TSA Endpoint: Initializing new TSA Generator process with status: {RuntimeStatus.in_progress}")
    logger.info(f"TSA Endpoint: Teams count: {len(input_.projectTeams)}")

    project_tsa = TSAGeneratorOutput(
        name="TSAGenerator",
        instanceId=input_.instanceId,
        runtimeStatus=RuntimeStatus.in_progress,
        input=input_,
        output=Output(),
        createdTime=datetime.now(timezone.utc),
        lastUpdatedTime=datetime.now(timezone.utc),
    )

    redis.update(instance_id, project_tsa)
    logger.info(f"TSA Endpoint: Updated Redis with initial TSA output for instanceId: {instance_id}")

    async def background_task(input_, redis, program_office, program_office_config, ai_search):
        try:
            logger.info(f"TSA Endpoint: Background task started for instanceId: {input_.instanceId}")

            # Create appropriate generator based on input type
            if isinstance(input_, TSAGeneratorInput):
                logger.info(f"TSA Endpoint: Creating TSAGenerator instance with {len(input_.projectTeams)} project teams")
                project_tsa_generator = TSAGenerator(
                    input_,
                    redis=redis,
                    logger=logger,
                    program_office=program_office,
                    program_office_config=program_office_config,
                    ai_search=ai_search
                )
            else:
                logger.info(f"TSA Endpoint: Creating ScopeGenerator instance with {len(input_.projectTeams)} project teams")
                project_tsa_generator = ScopeGenerator(
                    input_,
                    redis=redis,
                    logger=logger,
                    program_office=program_office,
                    program_office_config=program_office_config,
                    ai_search=ai_search
                )

            logger.info(f"TSA Endpoint: Calling generate() method for instanceId: {input_.instanceId}")
            await project_tsa_generator.generate()
            logger.info(f"TSA Endpoint: Background task completed successfully for instanceId: {input_.instanceId}")

        except Exception as e:
            logger.error(f"TSA Endpoint: Background task failed for instanceId: {input_.instanceId}")
            logger.error(f"TSA Endpoint: Error details: {str(e)}")
            project_tsa.lastUpdatedTime = datetime.now(timezone.utc)
            project_tsa.runtimeStatus = RuntimeStatus.failed
            project_tsa.errorMessage = str(e)
            redis.update(instance_id, project_tsa)
            logger.error(f"TSA Endpoint: Unhandled exception: {traceback.format_exc()}")

    logger.info(f"TSA Endpoint: Adding background task for instanceId: {instance_id}")
    background_tasks.add_task(background_task, input_, redis, program_office, program_office_config, ai_search)
    logger.info(f"TSA Endpoint: Request processing completed, returning response for instanceId: {instance_id}")

    return JSONResponse(content=response, status_code=status.HTTP_202_ACCEPTED)
