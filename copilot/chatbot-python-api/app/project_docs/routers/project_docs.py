from typing import Annotated
import time

import tiktoken
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.callback_managers import CustomTokenCounterHandler
from app.core.config import OpenAIConfig
from app.core.interfaces import IBaseLogger
from app.core.logger import get_logger
from app.core.pydantic_models import LLMModels, MessageRequest, MessageResponse
from app.project_docs.services.project_docs import Service
from app.project_docs.services.project_docs_agentic import ServiceAgent
from fastapi import APIRouter, Depends, Header, Request, status
from fastapi.encoders import jsonable_encoder
from fastapi.responses import JSONResponse, Response
from llama_index.core.callbacks import CallbackManager
from llama_index.core.selectors import LLMSingleSelector

router = APIRouter()


@router.post("/project-docs", response_model=MessageResponse, tags=["Documents"])
async def project_docs(
    request: Request,
    message_request: MessageRequest,
    logger: Annotated[IBaseLogger, Depends(get_logger)],
    authorization: str = Header(...),
):
    """
    Handles a POST request for the project documents agent.
    This function accepts a MessageRequest and returns the result of the Service's execute method.

    Args:
        request (Request): The request object.
        message_request (MessageRequest): The MessageRequest to process.
        logger (Annotated[BaseLogger, Depends): The logger to use for the request.

    Returns:
        JSON: The response from the Service's execute method.
    """
    # Start timing the request for performance tracking
    request_start_time = time.time()
    
    # Log request received with truncated question for privacy
    question_preview = message_request.question[:50] + "..." if len(message_request.question) > 50 else message_request.question
    logger.info(
        f"Project Docs request received: '{question_preview}'", 
        extra_dims={
            "endpoint": "/project-docs",
            "chat_length": len(message_request.chatHistory) if message_request.chatHistory else 0,
            "question_length": len(message_request.question),
            "document_count": len(message_request.context.documents) if message_request.context.documents else 0
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
    logger.info("Models initialized successfully", extra_dims={"model_type": OpenAIConfig.model})

    try:
        # Log selection process start
        selection_start_time = time.time()
        logger.info("Starting query type classification")
        
        choices = [
                "complex_questions: This handles questions that require deep analysis, multiple steps of reasoning, calculations, comparisons across multiple documents, or technical details. This also includes requests for summaries of specific documents or the entire dataset. For example, 'What are the performance implications of the architecture described in these documents?', 'Compare the implementation approaches across these documents and identify potential conflicts.', 'Summarize the key points from these documents.'",
                "simple_questions: This handles straightforward questions about document content, basic information retrieval, or simple facts. For example, 'What is mentioned about X in this document?', 'Who is responsible for this task?' 'where are the Priority and Owner column'?"
            ]

        selector = LLMSingleSelector.from_defaults(llm=llm_models.llm)
        selector_result = selector.select(choices, query=message_request.question)
        selector_result_index = selector_result.selections[0].index == 0
        
        kind_request = selector_result_index if selector_result_index in [True, False] else True
        
        # Log query classification result
        selection_duration_ms = round((time.time() - selection_start_time) * 1000, 2)
        query_type = "complex" if kind_request else "simple"
        logger.info(
            f"Query classified as '{query_type}' type",
            extra_dims={
                "query_type": query_type,
                "classification_time_ms": selection_duration_ms
            }
        )
        
        # Log service execution start
        service_start_time = time.time()
        logger.info(f"Executing Project Docs {query_type} service")
        if kind_request:
            response = await ServiceAgent(logger).execute(
                message_request=message_request,
                authorization=authorization,
                llm_models=llm_models,
            )
        else:
            response = await Service(logger).execute(
                message_request=message_request,
                authorization=authorization,
                llm_models=llm_models,
            )
            
        # Log service execution completion with timing
        service_duration_ms = round((time.time() - service_start_time) * 1000, 2)
        logger.info(
            f"Project Docs service execution completed in {service_duration_ms}ms",
            extra_dims={
                "service_duration_ms": service_duration_ms,
                "service_type": "ServiceAgent" if kind_request else "Service"
            }
        )
        
        # Not setting a default 200 to the pop to avoid
        # complex logical debugging sessions, there must be
        # always an status code from the service response, but
        # if that not happen for some reason it should fail
        response_code = response.pop("status_code")  # type: ignore
        
        # Log response status
        logger.info(
            f"Project Docs response status: {response_code}",
            extra_dims={"status_code": response_code}
        )

        if response_code == status.HTTP_204_NO_CONTENT:
            logger.info("Project Docs returned no content response")
            return Response(status_code=status.HTTP_204_NO_CONTENT)

        # Log response details
        has_citing_sources = "citingSources" in response and response["citingSources"]
        has_followup = "followUpSuggestions" in response and response["followUpSuggestions"]
        has_score = "score" in response and response["score"] is not None
        
        logger.info(
            "Project Docs response generated",
            extra_dims={
                "response_type": response.get("backend", "unknown"),
                "has_citing_sources": has_citing_sources,
                "has_followup_suggestions": has_followup,
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
                "total_tokens": total_tokens,
                "query_type": query_type
            }
        )
        
        # This is necessary to avoid issues with the default
        # encoding in the response and the ToolOutput's llama-index class
        encoded_response = jsonable_encoder(response)

        # Log total request time as a metric
        total_time_ms = round((time.time() - request_start_time) * 1000, 2)
        logger.log_metric("total_request_time_ms", total_time_ms)
        
        # Final log of successful completion
        logger.info(
            f"Project Docs request completed successfully in {total_time_ms}ms",
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
            f"Error processing Project Docs request: {str(e)}",
            exc_info=True,
            extra_dims={
                "error_type": type(e).__name__,
                "error_time_ms": error_time_ms,
                "endpoint": "/project-docs"
            }
        )
        # Re-raise the exception to let FastAPI handle it
        raise
