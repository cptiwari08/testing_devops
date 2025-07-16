from typing import Annotated
import time

import tiktoken
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.callback_managers import CustomTokenCounterHandler
from app.core.config import OpenAIConfig
from app.core.interfaces import IBaseLogger
from app.core.logger import get_logger
from app.core.pydantic_models import LLMModels, MessageRequest, MessageResponse
from app.project_data.services.project_data import Service
from fastapi import APIRouter, Depends, Header, Request
from fastapi.encoders import jsonable_encoder
from fastapi.responses import JSONResponse, Response
from llama_index.core.callbacks import CallbackManager

router = APIRouter()


@router.post("/project-data", response_model=MessageResponse, tags=["Project Data"])
async def query_planning(
    request: Request,
    message_request: MessageRequest,
    logger: Annotated[IBaseLogger, Depends(get_logger)],
    authorization: str = Header(...),
) -> dict:
    """
    Handles a POST request for project data processing.
    This function accepts a MessageRequest and logs the process, then returns the result of the Service's execute method.

    - **Args**:
        - *request* (Request): The request object, used for managing request-specific state.
        - *message_request* (MessageRequest): The MessageRequest containing the query and associated metadata.
        - *logger* (BaseLogger): The logger instance, dependency-injected, to log throughout the request's lifecycle.
          If not provided, defaults to None. Must match the pattern "^true$".

    - **Returns**:
        - *JSON Respone*: A JSON Response containing the 'backend' key with the service name, and 'response' key with the message
          content from the service's response.
    """
    # Start timing the request for performance tracking
    request_start_time = time.time()
    
    # Log request received with truncated question for privacy
    question_preview = message_request.question[:50] + "..." if len(message_request.question) > 50 else message_request.question
    """ logger.info(
        f"Project Data request received: '{question_preview}'", 
        extra_dims={
            "endpoint": "/project-data",
            "chat_length": len(message_request.chatHistory) if message_request.chatHistory else 0,
            "question_length": len(message_request.question)
        }
    ) """
    logger.info(
        f"I received a project data request asking, '{question_preview}'", 
        extra_dims={
            "endpoint": "/project-data",
            "chat_length": len(message_request.chatHistory) if message_request.chatHistory else 0,
            "question_length": len(message_request.question)
        }
    )
    
    # Set the unique identifiers for the request.
    identifiers = {
        "instance_id": message_request.instanceId,
        "chat_id": message_request.chatId,
        "project_friendly_id": message_request.projectFriendlyId,
    }
    request.state.identifiers = identifiers
    
    # Log model initialization start
    logger.info("Initializing token counter and LLM models")
    token_counter = CustomTokenCounterHandler(
        tokenizer=tiktoken.encoding_for_model(OpenAIConfig.model).encode  # type: ignore
    )
    callback_manager = CallbackManager([token_counter])
    llm = AzureSyncLLM(callback_manager).get_model()
    embed_model = AzureEmbeddings().get_model()
    llm_models = LLMModels(llm=llm, embed_model=embed_model)
    logger.set_unique_identifiers(**identifiers)
    request.state.token_counter = token_counter
    logger.info("Models initialized successfully", extra_dims={"model_type": OpenAIConfig.model})

    try:
        # Log service execution start        
        logger.info("Now, I am executing the Project Data service.")
        service_start_time = time.time()
        
        response = await Service(logger=logger, llm_models=llm_models).execute(
            message_request=message_request, authorization=authorization
        )
        
        # Log service execution completion with timing
        service_duration_ms = round((time.time() - service_start_time) * 1000, 2)
        logger.info(
            f"Project Data service execution completed in {service_duration_ms}ms",
            extra_dims={"service_duration_ms": service_duration_ms}
        )
        
        # Log response details
        response_type = response.get("backend", "unknown")
        has_citing_sources = "citingSources" in response and response["citingSources"]
        has_sql = "sql" in response and response["sql"]
        
        logger.info(
            f"Project Data response generated",
            extra_dims={
                "response_type": response_type,
                "has_citing_sources": has_citing_sources,
                "has_sql": has_sql,
                "response_length": len(response.get("response", "")) if response.get("response") else 0
            }
        )
        
        # Log token usage metrics
        prompt_tokens = token_counter.prompt_llm_token_count
        completion_tokens = token_counter.completion_llm_token_count
        total_tokens = prompt_tokens + completion_tokens
        
        logger.log_metric(
            "token_usage", 
            total_tokens,
            extra_dims={
                "prompt_tokens": prompt_tokens,
                "completion_tokens": completion_tokens,
                "total_tokens": total_tokens
            }
        )
        
        # Log total request time as a metric
        total_time_ms = round((time.time() - request_start_time) * 1000, 2)
        logger.log_metric("total_request_time_ms", total_time_ms)
        
        # Final log of successful completion
        logger.info(
            f"Project Data request completed successfully in {total_time_ms}ms",
            extra_dims={
                "total_time_ms": total_time_ms,
                "response_size": len(str(response)) if response else 0
            }
        )
        
        # Create properly encoded JSON response
        encoded_response = jsonable_encoder(response)
        return JSONResponse(content=encoded_response)
        
    except Exception as e:
        # Log detailed error information
        error_time_ms = round((time.time() - request_start_time) * 1000, 2)
        logger.error(
            f"Error processing Project Data request: {str(e)}",
            exc_info=True,
            extra_dims={
                "error_type": type(e).__name__,
                "error_time_ms": error_time_ms,
                "endpoint": "/project-data"
            }
        )
        # Re-raise the exception to let FastAPI handle it
        raise
