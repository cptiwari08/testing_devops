from app.core.logger import Logger
from app.core.utils import decode_jwt_token
from fastapi import Request, status
from jose import JWTError
from jose.exceptions import JWTClaimsError
from llama_index.core.callbacks import TokenCountingHandler
from starlette.middleware.base import BaseHTTPMiddleware, RequestResponseEndpoint
from starlette.responses import JSONResponse, Response


class TokenValidation(BaseHTTPMiddleware):

    async def dispatch(
        self, request: Request, call_next: RequestResponseEndpoint
    ) -> Response | JSONResponse:

        # Setting up default values for the request state
        # Setting here as is the first middleware to be called
        request.state.token_counter = TokenCountingHandler()
        request.state.identifiers = {}

        logger = Logger()
        token = request.headers.get("Authorization", None)
        if not token:
            message = "Missing token"
            logger.error(message)
            return JSONResponse(
                status_code=status.HTTP_401_UNAUTHORIZED,
                content={"message": message},
            )
        if request.url.path == "/docs" or request.url.path == "/openapi.json":
            response = await call_next(request)
            return response
        else:

            logger = Logger()

            try:
                _ = decode_jwt_token(token)
            except JWTClaimsError as e:
                logger.error(f"An error occurred with the audience: {e}")
                return JSONResponse(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    content={"message": "An error occurred. Please try again later."},
                )
            except JWTError as e:
                logger.error(f"Token validation failed: {e}")
                return JSONResponse(
                    status_code=status.HTTP_401_UNAUTHORIZED,
                    content={"message": "Token validation failed."},
                )

            except ValueError as e:
                logger.error(f"An error occurred with the key: {e}")
                return JSONResponse(
                    status_code=status.HTTP_400_BAD_REQUEST,
                    content={"message": "An error occurred. Please try again later."},
                )

            else:
                response = await call_next(request)
                return response
