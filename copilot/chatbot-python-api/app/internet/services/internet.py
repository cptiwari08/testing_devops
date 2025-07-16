import time
import openai
from app.core.config import MemorySize
from app.core.memory import MemoryManager
from app.core.pydantic_models import LLMModels, MessageRequest, MetricInput
from app.core.utils import (
    extract_relevant_content,
    retry_decorator,
)
from app.core.prompt_manager import create_prompt_manager
from app.metrics.answer_accuracy import AnswerAccuracy
from app.metrics.strategy_context import Context
from llama_index.core.chat_engine import SimpleChatEngine


class Service:
    """
    This class provides a method execute using the AzureSyncLLM model
    and the SimpleChatEngine. It uses the LlamaMemoryManager to generate a llama memory from the chat history.
    """

    def __init__(self, logger) -> None:
        self._logger = logger
        # Initialize prompt manager using the factory function
        self._prompt_manager = create_prompt_manager(logger)
        self._logger.info("Internet Service initialized", extra_dims={"component": "Internet", "service": "internet_qa"})

    @retry_decorator(openai.APITimeoutError)  # type: ignore
    async def execute(
        self, message_request: MessageRequest, llm_models: LLMModels
    ) -> dict:
        """
        This method uses the AzureSyncLLM model and the SimpleChatEngine.
        It generates a llama memory from the chat history and uses it to update the chat engine's memory.
        It then sends a message to the chat engine and returns the response.
        Args:
            conversation_request: The conversation request to process.
        Returns:
            A dictionary containing the source of the response, the response, the citing sources, and the raw response.
        """
        # Track execution start time for performance metrics
        start_time = time.time()
        
        # Log request information with truncated question for privacy
        question_preview = message_request.question[:50] + "..." if len(message_request.question) > 50 else message_request.question
        self._logger.info(
            f"Processing Internet request: '{question_preview}'",
            extra_dims={
                "component": "Internet",
                "operation": "internet_qa",
                "chat_length": len(message_request.chatHistory) if message_request.chatHistory else 0,
                "question_length": len(message_request.question)
            }
        )
        
        self._logger.info("Starting internet")
        
        # Generate chat history
        memory_start_time = time.time()
        self._logger.info("Generating chat history")
        chat_history = MemoryManager.generate_chat_messages(
            message_request.chatHistory, size=MemorySize.internet  # type: ignore
        )
        memory_time = round((time.time() - memory_start_time) * 1000, 2)
        self._logger.info(
            f"Generated chat history with {len(chat_history)} messages in {memory_time}ms", 
            extra_dims={
                "history_message_count": len(chat_history),
                "memory_time_ms": memory_time
            }
        )

        # Prepare chat engine options
        chat_engine_start_time = time.time()
        self._logger.info("Preparing chat engine options")
        chat_engine_options = {
            "llm": llm_models.llm,
            "chat_history": chat_history,
        }
        
        # Process project description if available
        project_desc_start_time = time.time()
        project_description_context = ""
        if message_request.context.projectDescription:
            self._logger.info("Extracting relevant content from project description")
            project_description_context = extract_relevant_content(
                message_request.context.projectDescription,
                message_request.question,
                llm_models=llm_models,
            )
            project_desc_length = len(project_description_context)
            original_length = len(message_request.context.projectDescription)
            compression_ratio = round(project_desc_length / original_length, 2) if original_length > 0 else 0
            
            project_desc_time = round((time.time() - project_desc_start_time) * 1000, 2)
            self._logger.info(
                f"Extracted relevant project description ({project_desc_length} chars)", 
                extra_dims={
                    "original_length": original_length,
                    "extracted_length": project_desc_length,
                    "compression_ratio": compression_ratio,
                    "extraction_time_ms": project_desc_time
                }
            )
        
        # Get prompt templates
        prompt_start_time = time.time()
        self._logger.info("Retrieving prompt templates")
        
        # Get acronyms via prompt manager
        acronym_start_time = time.time()
        self._logger.info("Retrieving acronyms")
        acronyms = await self._prompt_manager.get_prompt(
            agent="project_data",
            key="acronyms",
            raw_data=True
        )
        
        formatted_acronyms = ""
        # Parse the acronyms content if it's not already formatted
        if isinstance(acronyms, dict):
            self._logger.info(f"Formatting {len(acronyms)} acronyms")
            for key, value in acronyms.items():
                formatted_acronyms += f"'{key}' = '{value}' \n "
        else:
            formatted_acronyms = acronyms
        
        acronym_time = round((time.time() - acronym_start_time) * 1000, 2)
        self._logger.info(
            "Acronyms processing completed", 
            extra_dims={
                "acronym_time_ms": acronym_time,
                "acronyms_count": len(acronyms) if isinstance(acronyms, dict) else 0
            }
        )
        
        # Get the chat engine prompt via prompt manager
        engine_prompt_start_time = time.time()
        self._logger.info("Retrieving chat engine prompt")
        chat_engine_prompt = await self._prompt_manager.get_prompt(
            agent="internet",
            key="chat_engine_context",
            prompt_parameters={
                "project_description": project_description_context,
                "acronyms": formatted_acronyms,
            }
        )
        
        engine_prompt_time = round((time.time() - engine_prompt_start_time) * 1000, 2)
        self._logger.info(
            "Chat engine prompt retrieved", 
            extra_dims={"prompt_time_ms": engine_prompt_time}
        )
        
        chat_engine_options |= {"system_prompt": chat_engine_prompt}
        
        # Create chat engine
        self._logger.info("Creating chat engine")
        chat_engine = SimpleChatEngine.from_defaults(**chat_engine_options)
        chat_engine_time = round((time.time() - chat_engine_start_time) * 1000, 2)
        self._logger.info(
            "Chat engine created", 
            extra_dims={"chat_engine_time_ms": chat_engine_time}
        )

        # Generate response
        response_start_time = time.time()
        payload = {"message": message_request.question}
        self._logger.info("Calling llama_index SimpleChatEngine")
        raw_response = await chat_engine.achat(**payload)
        response_time = round((time.time() - response_start_time) * 1000, 2)
        response_length = len(raw_response.response) if hasattr(raw_response, 'response') else 0
        
        self._logger.info(
            f"Generated response ({response_length} chars) in {response_time}ms", 
            extra_dims={
                "response_length": response_length,
                "response_time_ms": response_time
            }
        )

        # Calculate score
        metric_start_time = time.time()
        self._logger.info("Calculating response quality score")
        metric_input = MetricInput(
            user_input=message_request.question,
            llm_response=raw_response.response,
            retrieved_context=None,
            project_description=project_description_context,
        )
        
        self._logger.info("Calculating score")
        current_metric = Context(AnswerAccuracy())
        score = await current_metric.run(metric_input)
        metric_time = round((time.time() - metric_start_time) * 1000, 2)
        
        self._logger.info(
            f"Score calculation completed: {score}", 
            extra_dims={
                "score": score,
                "metric_time_ms": metric_time
            }
        )

        # Prepare response
        response_prep_start_time = time.time()
        self._logger.info("Preparing final response")
        response = {
            "backend": "internet",
            "response": raw_response.response,
            "citingSources": [
                {
                    "sourceName": "internet",
                    "sourceType": "internet",
                    "sourceValue": [],
                },
            ],
            "rawResponse": raw_response,
            "score": score,
            "chainOfThoughts": ""
        }
        response_prep_time = round((time.time() - response_prep_start_time) * 1000, 2)
        
        # Log total execution time
        total_time = round((time.time() - start_time) * 1000, 2)
        self._logger.log_metric(
            "total_execution_time_ms", 
            total_time,
            extra_dims={
                "service": "internet",
                "has_score": score is not None,
                "response_length": response_length
            }
        )
        
        self._logger.info(
            f"Internet service execution completed in {total_time}ms", 
            extra_dims={
                "total_execution_time_ms": total_time,
                "response_prep_time_ms": response_prep_time
            }
        )
        
        return response
