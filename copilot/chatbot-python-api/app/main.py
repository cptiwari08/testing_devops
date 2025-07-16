import os

import openai
from app.core.error_handlers import (
    key_vault_http_connection_error,
    missing_token_claim_error,
    non_integer_chat_history_handler,
    non_negative_integer_chat_history_handler,
    not_supported_query_type_error,
    openai_api_error,
    openai_authentication_error,
    openai_bad_request_error,
    openai_connection_error,
    program_office_response_error,
    program_office_timeout_error,
    prompt_validation_error,
    retry_error_exception_handler,
    secret_not_found_error_handler,
)
from app.core.errors import (
    KeyVaultHTTPConnectionError,
    MissingTokenClaimError,
    NonIntegerChatHistoryError,
    NonNegativeChatHistoryError,
    NotSupportedQueryTypeError,
    ProgramOfficeResponseError,
    ProgramOfficeTimeoutError,
    PromptValidationError,
    SecretNotFoundError,
)
from app.core.middlewares.exception_handling import ExceptionHandler
from app.core.middlewares.response_filter import ResponseFilter
from app.core.middlewares.token_validation import TokenValidation
from app.ey_ip.routers.assistant_agent import router as ey_ip_agent_router
from app.internet.routers.internet import router as internet_router
from app.project_data.routers.project_data import router as query_pipeline_router
from app.project_docs.routers.project_docs import router as project_docs
from fastapi import FastAPI
from tenacity import RetryError

from . import __version__

app = FastAPI(
    version=__version__,
)

# Disable deepeval telemetry
os.environ["DEEPEVAL_TELEMETRY_OPT_OUT"] = "YES"

# Middlewares
app.add_middleware(TokenValidation)
app.add_middleware(ExceptionHandler)
app.add_middleware(ResponseFilter)

# Exception handlers
app.add_exception_handler(MissingTokenClaimError, missing_token_claim_error)  # type: ignore
app.add_exception_handler(KeyVaultHTTPConnectionError, key_vault_http_connection_error)  # type: ignore
app.add_exception_handler(
    ProgramOfficeResponseError, program_office_response_error  # type: ignore
)
app.add_exception_handler(ProgramOfficeTimeoutError, program_office_timeout_error)  # type: ignore
app.add_exception_handler(RetryError, retry_error_exception_handler)
app.add_exception_handler(NonIntegerChatHistoryError, non_integer_chat_history_handler)
app.add_exception_handler(
    NonNegativeChatHistoryError, non_negative_integer_chat_history_handler
)
app.add_exception_handler(NotSupportedQueryTypeError, not_supported_query_type_error)
app.add_exception_handler(openai.APIError, openai_api_error)
app.add_exception_handler(openai.APIConnectionError, openai_connection_error)
app.add_exception_handler(openai.AuthenticationError, openai_authentication_error)
app.add_exception_handler(openai.BadRequestError, openai_bad_request_error)
app.add_exception_handler(PromptValidationError, prompt_validation_error)
app.add_exception_handler(SecretNotFoundError, secret_not_found_error_handler)

# Routers
app.include_router(ey_ip_agent_router)
app.include_router(internet_router)
app.include_router(query_pipeline_router)
app.include_router(project_docs)
