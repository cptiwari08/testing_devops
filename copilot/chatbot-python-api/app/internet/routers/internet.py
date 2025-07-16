from typing import Annotated
import time

import tiktoken
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.callback_managers import CustomTokenCounterHandler
from app.core.config import OpenAIConfig
from app.core.interfaces import IBaseLogger
from app.core.logger import get_logger
from app.core.pydantic_models import LLMModels, MessageRequest, MessageResponse
from app.internet.services.internet import Service
from fastapi import APIRouter, Depends, Request, status
from fastapi.encoders import jsonable_encoder
from fastapi.responses import JSONResponse
from llama_index.core.callbacks import CallbackManager

router = APIRouter()


@router.post("/internet", response_model=MessageResponse, tags=["Internet"])
async def internet(
    request: Request,
    message_request: MessageRequest,
    logger: Annotated[IBaseLogger, Depends(get_logger)],
):
    """
    Handles a POST request for internet.
    This function accepts a MessageRequest and returns the result of the Service's execute method.
    - **Args**:
        - *request*: The request object.
        - *message_request*: The MessageRequest to process.
        - *logger*: The logger to use for the request.
    - **Returns**:
        - The result of the Service's execute method.
    """
    # Start timing the request for performance tracking
    request_start_time = time.time()
    
    # Log request received with truncated question for privacy
    question_preview = message_request.question[:50] + "..." if len(message_request.question) > 50 else message_request.question
    logger.info(
        f"Internet request received: '{question_preview}'", 
        extra_dims={
            "endpoint": "/internet",
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
    
    # Log model initialization
    logger.info("Initializing token counter and LLM models")
    token_counter = CustomTokenCounterHandler(
        tokenizer=tiktoken.encoding_for_model(OpenAIConfig.model).encode  # type: ignore
    )
    callback_manager = CallbackManager(handlers=[token_counter])
    embed_model = AzureEmbeddings().get_model()
    llm = AzureSyncLLM(callback_manager).get_model()
    llm_models = LLMModels(llm=llm, embed_model=embed_model)
    request.state.identifiers = identifiers
    request.state.token_counter = token_counter
    logger.set_unique_identifiers(**identifiers)
    logger.info("Models initialized successfully", extra_dims={"model_type": OpenAIConfig.model})

    try:
        # Log service execution start
        service_start_time = time.time()
        logger.info("Executing Internet service")
        
        response = await Service(logger).execute(message_request, llm_models)
        
        # Log service execution completion with timing
        service_duration_ms = round((time.time() - service_start_time) * 1000, 2)
        logger.info(
            f"Internet service execution completed in {service_duration_ms}ms",
            extra_dims={
                "service_duration_ms": service_duration_ms,
                "service_type": "Internet"
            }
        )
        
        # Log response details
        has_citing_sources = "citingSources" in response and response["citingSources"]
        has_score = "score" in response and response["score"] is not None
        
        logger.info(
            "Internet response generated",
            extra_dims={
                "response_type": response.get("backend", "unknown"),
                "has_citing_sources": has_citing_sources,
                "has_score": has_score,
                "response_length": len(response.get("response", "")) if response.get("response") else 0,
                "source_count": len(response.get("citingSources", [{}])[0].get("sourceValue", [])) if has_citing_sources else 0
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
            f"Internet request completed successfully in {total_time_ms}ms",
            extra_dims={
                "total_time_ms": total_time_ms,
                "response_size": len(str(response))
            }
        )
        
        return response
        
    except Exception as e:
        # Log detailed error information
        error_time_ms = round((time.time() - request_start_time) * 1000, 2)
        logger.error(
            f"Error processing Internet request: {str(e)}",
            exc_info=True,
            extra_dims={
                "error_type": type(e).__name__,
                "error_time_ms": error_time_ms,
                "endpoint": "/internet"
            }
        )
        # Re-raise the exception to let FastAPI handle it
        raise
