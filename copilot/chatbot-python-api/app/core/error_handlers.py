import json

from app.core.errors import (
    KeyVaultHTTPConnectionError,
    MissingTokenClaimError,
    ProgramOfficeResponseError,
    ProgramOfficeTimeoutError,
    SecretNotFoundError,
)
from app.core.logger import Logger
from fastapi import HTTPException, Request, status
from starlette.responses import JSONResponse


async def retry_error_exception_handler(
    request: Request, _: Exception
) -> JSONResponse:
    """
    Handles RetryError exceptions. When a RetryError occurs, this handler logs the error and returns a 503 Service
    Unavailable response.
    Args:
        request: The request that caused the error.
        exc: The exception that was raised.
    Returns:
         A JSONResponse with a status code of 503 and a message indicating that the server was unable to respond
    within the expected time.
    """
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error("RetryError: 503 Service Unavailable")
    return JSONResponse(
        status_code=status.HTTP_503_SERVICE_UNAVAILABLE,
        content={
            "detail": "RetryError",
            "message": "The server was unable to respond to your request within the expected time. Please retry "
            "after a few minutes.",
        },
    )


async def openai_api_error(request: Request, exc: Exception) -> JSONResponse:
    """
    Handles OpenAI API errors. When an APIError occurs, this handler logs the error and returns a 400 Bad Request
    response.
    Args:
        request: The request that caused the error.
        exc: The exception that was raised.
        Returns:
            A JSONResponse with a status code of 400 and a message indicating that an error occurred while
            processing the request.
    """
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"OpenAI API returned an API Error: {exc}")
    return JSONResponse(
        status_code=status.HTTP_400_BAD_REQUEST,
        content={
            "detail": "APIError",
            "message": "OpenAI has returned an API error",
        },
    )


async def openai_connection_error(request: Request, exc: Exception) -> JSONResponse:
    """
    Handles OpenAI API Connection error.
    When an APIConnectionError occurs, this handler logs the error and returns a 503 Service Unavailable
    Args:
        request: The request that caused the error.
        exc: The exception that was raised.
    Returns:
        A JSONResponse with a status code of 503 and a message indicating that an error occurred while
        processing the request.
    """
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"OpenAI API request failed to connect: {exc}")
    return JSONResponse(
        status_code=status.HTTP_503_SERVICE_UNAVAILABLE,
        content={
            "detail": "APIConnectionError",
            "message": "OpenAI has returned a connection error",
        },
    )


async def openai_authentication_error(request: Request, exc: Exception) -> JSONResponse:
    """
    Handles OpenAI API authentication errors.
    When an AuthenticationError occurs, this handler logs the error and returns a 401 Unauthorized response.
    Args:
        request: The request that caused the error.
        exc: The exception that was raised.
    Returns:
        A JSONResponse with a status code of 401 and a message indicating that the request was not authorized.
    """
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"OpenAI API request was not authorized: {exc}")
    return JSONResponse(
        status_code=status.HTTP_401_UNAUTHORIZED,
        content={
            "detail": "AuthenticationError",
            "message": "Unauthorized. Access token is missing, invalid, audience is incorrect "
            "(https://cognitiveservices.azure.com), or have expired.",
        },
    )


async def openai_bad_request_error(request: Request, exc: Exception) -> JSONResponse:
    """
    Handles OpenAI API Bad request errors.
    When an InvalidRequestError occurs, this handler logs the error and returns a 400 Bad Request response.
    Args:
        request: The request that caused the error.
        exc: The exception that was raised.
    Returns:
        A JSONResponse with a status code of 400 and a message indicating that an error occurred while
        processing the request.
    """
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"OpenAI API request was invalid: {exc}")
    return JSONResponse(
        status_code=status.HTTP_400_BAD_REQUEST,
        content={
            "detail": "InvalidRequestError",
            "message": "OpenAI has returned bad request response",
        },
    )


async def prompt_validation_error(request: Request, exc: Exception) -> JSONResponse:
    """
    Asynchronous error handling function for prompt validation errors.


    Parameters:
    - request (Request): The request object, which should contain state identifiers
                         used for logging the error.
    - exc (Exception): The exception object that triggered the error handling.

    Returns:
    - JSONResponse: A response object with HTTP 500 status code indicating an internal
                    server error, along with a JSON content detailing the error type and message.
    """
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"Prompt validation error: {exc}")
    return JSONResponse(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        content={
            "detail": "PromptValidationError",
            "message": "An error occurred while processing a prompt template",
        },
    )


async def program_office_response_error(
    request: Request, exc: ProgramOfficeResponseError
) -> JSONResponse:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    error_msg = f"Program Office HTTP response: \nHTTP CODE: {exc.response.status_code}\nENDPOINT: {exc.response.request.url}"
    logger.error(error_msg)
    body = json.loads(exc.response.request.content.decode("utf-8"))
    return JSONResponse(
        status_code=exc.response.status_code,
        content={
            "backend": "project-data",  # This exception only happens on project-data
            "sql": {
                "query": body.get("SqlQuery", None),
            },
            "response": exc.response.text,
            "citingSources": None,
            "rawResponse": None,
            "chainOfThoughts": "",
        },
    )


async def program_office_timeout_error(
    request: Request, exc: ProgramOfficeTimeoutError
) -> JSONResponse:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    error_msg = f"Program Office HTTP response: \nHTTP CODE: {status.HTTP_408_REQUEST_TIMEOUT}\nENDPOINT: {exc.url}"
    logger.error(error_msg)
    return JSONResponse(
        status_code=status.HTTP_408_REQUEST_TIMEOUT,
        content={
            "detail": error_msg,
        },
    )


async def not_supported_query_type_error(
    request: Request, exc: Exception
) -> JSONResponse:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"SQL response error: {exc}")
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content={
            "detail": "NotSupportedQueryTypeError",
            "message": "SQL Query is invalid or not supported",
        },
    )


async def non_integer_chat_history_handler(
    request: Request, exc: Exception
) -> JSONResponse:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"error: {exc}")
    return JSONResponse(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        content={
            "detail": "NonIntegerChatHistoryError",
            "message": "Chat memory size must be a negative integer",
        },
    )


async def non_negative_integer_chat_history_handler(
    request: Request, exc: Exception
) -> JSONResponse:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(f"error: {exc}")
    return JSONResponse(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        content={
            "detail": "NonNegativeChatHistoryError",
            "message": "Chat memory size must be a negative integer",
        },
    )


async def secret_not_found_error_handler(
    request: Request, exc: SecretNotFoundError
) -> None:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(str(exc))
    raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)


async def key_vault_http_connection_error(
    request: Request, _: KeyVaultHTTPConnectionError
) -> None:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error("HTTP error getting key vault value")
    raise HTTPException(status_code=status.HTTP_502_BAD_GATEWAY)


async def missing_token_claim_error(
    request: Request, exc: MissingTokenClaimError
) -> None:
    logger = Logger()
    logger.set_unique_identifiers(**request.state.identifiers)
    logger.error(str(exc))
    raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR)
