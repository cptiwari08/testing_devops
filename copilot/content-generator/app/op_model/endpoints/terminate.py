from typing import Annotated
from uuid import UUID
import os
from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.dependencies import Container
from app.op_model.endpoints.paths import OPModelPaths
from app.op_model.schemas.op_model_output import OPModelOutput
from dependency_injector.wiring import Provide, inject
from fastapi import APIRouter, Depends, HTTPException, Path, Response, status

router = APIRouter()
log_mes_base = f"OP MODEL | {os.path.basename(__file__)[:-3]} | "

@router.post(
    OPModelPaths.terminate,
    tags=["OP Model"],
    status_code=status.HTTP_204_NO_CONTENT,
    responses={
        204: {"description": "OP Model aborted successfully"},
        200: {"description": "OP Model already completed"},
        404: {"description": "OP Model not found"},
        400: {"description": "Invalid JSON format"},
        422: {"description": "The OP Model has not been started"},
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
        logger.error(f"{log_mes_base} Error: OP Model not found")
        raise HTTPException(
            detail="OP Model not found", status_code=status.HTTP_404_NOT_FOUND
        )

    op_model: OPModelOutput | None = redis.get(
        instance_id_str, OPModelOutput
    )
    if not op_model:
        raise HTTPException(
            detail="The OP Model has not been started",
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )

    if op_model.runtimeStatus == RuntimeStatus.completed:
        return Response(
            status_code=status.HTTP_200_OK, content="OP Model already completed"
        )

    op_model.runtimeStatus = RuntimeStatus.aborted
    redis.update(instance_id_str, op_model)

    return Response(status_code=status.HTTP_204_NO_CONTENT)
