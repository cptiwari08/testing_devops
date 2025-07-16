from typing import Annotated
import time

import tiktoken
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.callback_managers import CustomTokenCounterHandler
from app.core.config import OpenAIConfig
from app.core.interfaces import IBaseLogger
from app.core.logger import get_logger
from app.core.pydantic_models import LLMModels, MessageRequest, MessageResponse
from app.ey_ip.services.assistant_agent import Service
from fastapi import APIRouter, Depends, Header, Request, status
from fastapi.encoders import jsonable_encoder
from fastapi.responses import JSONResponse, Response
from llama_index.core.callbacks import CallbackManager

router = APIRouter()


@router.post("/ey-ip", response_model=MessageResponse, tags=["EY IP"])
async def agent(
    request: Request,
    message_request: MessageRequest,
    logger: Annotated[IBaseLogger, Depends(get_logger)],
    authorization: str = Header(...),
):
    """
    Handles a POST request for the EY IP agent.
    This function accepts a MessageRequest and returns the result of the Service's execute method.
    - **Args**:
        - *request*: The request object.
        - *message_request*: The MessageRequest to process.
        - *logger*: The logger to use for the request.
        of the llama index agent. If not provided, defaults to None.
    - **Returns**:
        - *JSON*: The jsonable encoded response from the agent.
    """
    # Start timing the request for performance tracking
    request_start_time = time.time()
    
    # Log request received
    question_preview = message_request.question[:50] + "..." if len(message_request.question) > 50 else message_request.question
    logger.info(
        f"EY.IP request received: '{question_preview}'", 
        extra_dims={
            "endpoint": "/ey-ip",
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
    
    # Log model initialization
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
    
    logger.info(f"I set the identifiers: {identifiers}", extra_dims={"model_type": OpenAIConfig.model})
    try:
        # Log service execution start
        logger.info("Executing EY.IP service")
        service_start_time = time.time()
        
        agent_response = await Service(logger, llm_models).execute(
            message_request=message_request,
            authorization=authorization,
        )
        
        # Log service execution completion with timing
        service_duration_ms = round((time.time() - service_start_time) * 1000, 2)
        logger.info(
            f"EY.IP service execution completed in {service_duration_ms}ms",
            extra_dims={"service_duration_ms": service_duration_ms}
        )

        # Not setting a default 200 to the pop to avoid
        # complex logical debugging sessions, there must be
        # always an status code from the agent response, but
        # if that not happen for some reason it should fail
        response_code = agent_response.pop("status_code")  # type: ignore
        
        # Log response status
        logger.info(
            f"EY.IP response status: {response_code}",
            extra_dims={"status_code": response_code}
        )
        
        if response_code == status.HTTP_204_NO_CONTENT:
            logger.info("EY.IP returned no content response")
            return Response(status_code=status.HTTP_204_NO_CONTENT)

        # This is necessary to avoid issues with the default
        # encoding in the response and the ToolOutput's llama-index class
        encoded_response = jsonable_encoder(agent_response)
        
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
        
        logger.info(
            f"EY.IP request completed successfully in {total_time_ms}ms",
            extra_dims={
                "total_time_ms": total_time_ms,
                "response_size": len(str(encoded_response))
            }
        )
        
        return JSONResponse(content=encoded_response, status_code=response_code)
        
    except Exception as e:
        # Log detailed error information
        error_time_ms = round((time.time() - request_start_time) * 1000, 2)
        logger.error(
            f"Error processing EY.IP request: {str(e)}",
            exc_info=True,
            extra_dims={
                "error_type": type(e).__name__,
                "error_time_ms": error_time_ms,
                "endpoint": "/ey-ip"
            }
        )
        # Re-raise the exception to let FastAPI handle it
        raise
