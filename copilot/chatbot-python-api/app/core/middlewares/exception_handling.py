import sys
import traceback
from datetime import datetime

from app.core.config import Logging as LoggingConfig
from app.core.logger import Logger
from fastapi import Request
from sqlalchemy.exc import ProgrammingError
from starlette.middleware.base import BaseHTTPMiddleware, RequestResponseEndpoint
from starlette.responses import JSONResponse, Response


class ExceptionHandler(BaseHTTPMiddleware):
    """
    A middleware for handling uncaught exceptions in a FastAPI application.
    This middleware catches uncaught exceptions and returns a 500 Internal Server Error response.
    It also logs the exception.
    """

    async def dispatch(
        self, request: Request, call_next: RequestResponseEndpoint
    ) -> Response | JSONResponse:
        """
        Handles the incoming request and catches any uncaught exceptions.
        Args:
            request: The incoming request.
            call_next: The next middleware or endpoint in the processing chain.
        Returns:
            The response from the next middleware or endpoint in the processing chain, or a 500 Internal
            Server Error response if an uncaught exception occurs.
        """
        request_start = datetime.now()
        try:
            response = await call_next(request)
            status_code = response.status_code
            return response
        except ProgrammingError as _:
            logger = Logger()
            logger.set_unique_identifiers(**request.state.identifiers)
            logger.error("Unhandled exception: ProgrammingError.")
            content = {
                "message": "Unhandled exception: ProgrammingError.",
            }
            status_code = 500
            return JSONResponse(
                status_code=status_code,
                content=content,
            )
        except Exception as _:
            logger = Logger()
            logger.set_unique_identifiers(**request.state.identifiers)

            tb_str = "".join(traceback.format_exception(*sys.exc_info()))
            logger.error(f"Unhandled exception: {tb_str}")
            content = {
                "message": "Unhandled exception",
            }
            if "DEBUG" == LoggingConfig.level:
                content.update({"exception": str(tb_str)})
            status_code = 500
            return JSONResponse(
                status_code=status_code,
                content=content,
            )
        finally:
            request_end = datetime.now()
            request_time = request_end - request_start
            miliseconds = request_time.total_seconds() * 1000
            backend = request.url.path.strip("/")
            total_tokens = sum(
                [
                    event_.total_token_count
                    for event_ in request.state.token_counter.llm_token_counts
                ]
            )
            message = (
                f"completion_tokens: {request.state.token_counter.prompt_llm_token_count}, "
                f"prompts_tokens: {request.state.token_counter.completion_llm_token_count}, "
                f"total_tokens: {total_tokens}, "
                f"http_status_code: {status_code}, "
                f"time: {miliseconds} ms, "
                f"source: {backend}"
            )
            logger = Logger()
            # logger.set_unique_identifiers(**request.state.identifiers)
            logger.info(message)
