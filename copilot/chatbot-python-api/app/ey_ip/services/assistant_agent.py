import asyncio
import time
from typing import Dict, Any, Optional

from app.core.config import AISearchConfig, MemorySize
from app.core.interfaces import IBaseLogger
from app.core.memory import MemoryManager
from app.core.prompt_manager import create_prompt_manager
from app.core.pydantic_models import LLMModels, MessageRequest, MetricInput
from app.core.utils import (
    calculate_final_score,
    extract_claim,
    extract_relevant_content,
    inject_project_description,
    load_prompt_template,
)
from app.ey_ip.services.ai_search import AISearchManager
from app.ey_ip.services.citing_sources import build_citing_sources
from app.ey_ip.tools.dummy import DummyTool
from app.ey_ip.tools.om import OMTool
from app.ey_ip.tools.pmo import PMOTool
from app.ey_ip.tools.tsa import TSATool
from app.ey_ip.tools.vc import VCTool
from app.metrics.rag_sql_evaluator import RagSQLEvaluator
from app.metrics.sql_evaluator import SQLEvaluator
from app.metrics.strategy_context import Context
from fastapi import HTTPException
from llama_index.agent.openai_legacy import ContextRetrieverOpenAIAgent
from llama_index.core.prompts import PromptTemplate
from starlette import status


class Service:

    def __init__(self, logger: IBaseLogger, llm_models: LLMModels) -> None:
        """
        Initialize the Service with logger, LLM models, and appropriate prompt manager.
        
        Args:
            logger: Logger instance for logging messages
            llm_models: LLM models for generating responses
        """
        """
        Initialize the Service with logger, LLM models, and appropriate prompt manager.
        
        Args:
            logger: Logger instance for logging messages
            llm_models: LLM models for generating responses
        """
        super().__init__()
        self._logger = logger
        self._llm_models = llm_models
        # Use the factory function to create the appropriate prompt manager based on configuration
        self._prompt_manager = create_prompt_manager(logger)
        # Log service initialization with component information
        self._logger.info("EY.IP Service initialized", extra_dims={"component": "EY.IP", "service": "assistant_agent"})

    def get_tools(self) -> list:
        """
        Returns a list of tools.
        """
        # Log tool initialization before creating the tools
        self._logger.debug("Initializing EY.IP tools", extra_dims={"component": "EY.IP", "operation": "tool_init"})
        
        tools = [
            PMOTool(self._logger, self._llm_models),
            TSATool(self._logger, self._llm_models),
            VCTool(self._logger, self._llm_models),
            OMTool(self._logger, self._llm_models),
            DummyTool(self._logger, self._llm_models),
        ]
        
        # Log tools created with count and names
        self._logger.debug(
            f"Initialized {len(tools)} tools for EY.IP", 
            extra_dims={
                "component": "EY.IP", 
                "tool_count": len(tools),
                "tools": [tool.__class__.__name__ for tool in tools]
            }
        )
        return tools

    async def execute(
        self,
        message_request: MessageRequest,
        authorization: str,
    ) -> dict:
        """
        Executes the agent.
        Args:
            message_request: The MessageRequest to process.
        Returns:
            A dictionary containing the source of the response, the response, the citing sources, and the raw response.
        """
        # Track execution start time for performance metrics
        request_start_time = time.time()
        execution_metrics = {
            "component": "EY.IP",
            "operation": "agent_execution",
            "chat_length": len(message_request.chatHistory) if message_request.chatHistory else 0,
            "question_length": len(message_request.question) if message_request.question else 0
        }
        
        # Enhanced request logging with truncated question for privacy/length
        question_preview = message_request.question[:50] + "..." if len(message_request.question) > 50 else message_request.question
        self._logger.info(
            f"Processing EY.IP request: '{question_preview}'",
            extra_dims=execution_metrics
        )
        
        self._logger.info("Initializing agent")
        self._logger.info("Agent models retrieved")
        self._logger.info("Setting agent models")
        self._logger.info("Agent models set")

        # Enhanced project description logging
        project_description = message_request.context.projectDescription
        if project_description and (
            project_description_context := extract_relevant_content(
                project_description, message_request.question, self._llm_models
            )
        ):
            # Log metrics about project description processing
            desc_length = len(project_description)
            context_length = len(project_description_context)
            compression_ratio = round(context_length / desc_length, 2) if desc_length > 0 else 0
            
            self._logger.info(
                f"Extracted relevant project context ({context_length} chars from {desc_length} chars)",
                extra_dims={
                    "original_length": desc_length,
                    "extracted_length": context_length,
                    "compression_ratio": compression_ratio
                }
            )
            
            message_request.context.projectDescription = project_description_context
            
            prompt = await self._prompt_manager.get_prompt(
                agent="ey_ip",
                key="project_description",
                prompt_parameters={
                    "project_description": project_description_context,
                    "user_prompt": message_request.question
                },
            )
            message_request.question = prompt
            self._logger.info(
                "Question reformulated with project context",
                extra_dims={"new_question_length": len(prompt)}
            )

        # Enhanced SQL suggestion logging
        if (
            message_request.context
            and message_request.context.suggestion
            and message_request.context.suggestion.sqlQuery
        ):
            query = message_request.context.suggestion.sqlQuery
            self._logger.info(
                f"Processing SQL suggestion ({len(query)} chars)",
                extra_dims={"sql_query_length": len(query)}
            )
            
            prompt = await self._prompt_manager.get_prompt(
                agent="ey_ip",
                key="suggested_question",
                prompt_parameters={
                    "query": query,
                    "question": message_request.question
                },
            )
            message_request.question = prompt
            self._logger.info("Question reformulated with SQL suggestion")

        # Enhanced token processing logging
        token_start_time = time.time()
        token = authorization[len("Bearer ") :]
        self._logger.info("Getting AI Search Instance Name for token")
        aisearch_instance_name = extract_claim(token, "ai_search_instance_name")
        
        if not aisearch_instance_name:
            self._logger.error(
                "Invalid AI Search instance name in token",
                extra_dims={"token_status": "invalid", "missing_claim": "ai_search_instance_name"}
            )
            raise HTTPException(
                status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
                detail="Invalid AI Search instance name",
            )
        
        # Only log a preview of the instance name for security
        masked_instance = aisearch_instance_name[:4] + "..." if len(aisearch_instance_name) > 4 else "****"
        self._logger.info(
            f"AI Search instance name extracted: {masked_instance}",
            extra_dims={"token_status": "valid", "token_processing_ms": round((time.time() - token_start_time) * 1000, 2)}
        )

        # Enhanced AI Search setup logging
        search_setup_start = time.time()
        ai_search = AISearchManager(self._llm_models)
        self._logger.info(
            f"Setting up AI Search client for index {AISearchConfig.ey_ip_project_context_index}",
            extra_dims={"search_index": AISearchConfig.ey_ip_project_context_index}
        )
        
        await ai_search.async_set_client(
            AISearchConfig.ey_ip_project_context_index, aisearch_instance_name
        )
        self._logger.info(
            "AI Search client setup complete",
            extra_dims={"setup_time_ms": round((time.time() - search_setup_start) * 1000, 2)}
        )

        # Enhanced context retrieval logging
        context_start_time = time.time()
        self._logger.info("Retrieving context index")
        retriever = await ai_search.context_retriever(message_request.question)
        
        # Count retrieved sources if available
        context_count = len(retriever.get("sources", [])) if isinstance(retriever.get("sources"), list) else 0
        self._logger.info(
            f"Retrieved {context_count} context items", 
            extra_dims={
                "context_count": context_count,
                "retrieval_time_ms": round((time.time() - context_start_time) * 1000, 2)
            }
        )

        # Enhanced tool generation logging
        tool_generation_start = time.time()
        self._logger.info("Generating tools for agent")
        
        async def async_generate_tools(tool):
            tool_start_time = time.time()
            tool_name = tool.__class__.__name__
            
            try:
                self._logger.debug(f"Generating tool: {tool_name}")
                result = await asyncio.to_thread(tool.generate)
                tool_time_ms = round((time.time() - tool_start_time) * 1000, 2)
                
                # Log successful tool generation with timing
                self._logger.log_tool_execution(
                    tool_name=tool_name,
                    successful=True,
                    execution_time_ms=tool_time_ms
                )
                return result
            except Exception as e:
                tool_time_ms = round((time.time() - tool_start_time) * 1000, 2)
                
                # Log failed tool generation with error details
                self._logger.log_tool_execution(
                    tool_name=tool_name,
                    successful=False,
                    execution_time_ms=tool_time_ms,
                    error_details=str(e)
                )
                raise
        
        tools = await asyncio.gather(*[async_generate_tools(tool) for tool in self.get_tools()])
        total_tool_time = round((time.time() - tool_generation_start) * 1000, 2)
        
        self._logger.info(
            f"Generated {len(tools)} tools (took {total_tool_time}ms)",
            extra_dims={
                "tool_count": len(tools),
                "total_tool_generation_time_ms": total_tool_time
            }
        )
        self._logger.info("Tools generated")

        # Enhanced agent setup logging
        agent_setup_start = time.time()
        self._logger.info("Setting up agent with prompt template")
        prompt_text = await self._prompt_manager.get_prompt(
            agent="ey_ip",
            key="agent_prompt"
        )

        self.agent = ContextRetrieverOpenAIAgent.from_tools_and_retriever(
            tools,
            retriever.get("retrieved_index"),  # type: ignore
            llm=self._llm_models.llm,
            verbose=False,
            qa_prompt=PromptTemplate(template=prompt_text),
        )
        self._logger.info(
            "Agent initialization complete",
            extra_dims={"agent_setup_time_ms": round((time.time() - agent_setup_start) * 1000, 2)}
        )
        self._logger.info("Agent initialized")

        # Enhanced memory generation logging
        memory_start_time = time.time()        
        self._logger.info(f"I generated the chat memory with a size of {MemorySize.eyip}")
        memory = MemoryManager.generate_chat_messages(
            message_request.chatHistory, size=MemorySize.eyip  # type: ignore
        )
        self._logger.info(
            f"I generated the chat memory with {len(memory)} messages.",
            extra_dims={
                "memory_message_count": len(memory),
                "memory_generation_time_ms": round((time.time() - memory_start_time) * 1000, 2)
            }
        )

        # Enhanced response generation logging
        self._logger.info("Generating agent response")
        payload = {
            "message": message_request.question,
            "chat_history": memory,
        }
        
        response_start_time = time.time()
        raw_response = await self.agent.achat(**payload)  # type: ignore
        response_time_ms = round((time.time() - response_start_time) * 1000, 2)
        
        response_length = len(raw_response.response) if hasattr(raw_response, 'response') else 0
        self._logger.info(
            f"Agent response generated ({response_length} chars, took {response_time_ms}ms)",
            extra_dims={
                "response_length": response_length,
                "response_time_ms": response_time_ms
            }
        )
        self._logger.info("Agent response generated")

        # Enhanced citing sources logging
        sources_start_time = time.time()
        self._logger.info("Building citing sources")
        sources = await build_citing_sources(
            raw_response=raw_response, logger=self._logger
        )
        sources_time_ms = round((time.time() - sources_start_time) * 1000, 2)
        
        source_errors = sources.pop("source_errors")
        citing_sources = sources.pop("citing_sources")
        tables_list = sources.pop("tables_list")
        sql = sources.pop("sql")
        sql_schema = sources.pop("schema")
        
        source_error_count = sum(1 for error in source_errors if error)
        self._logger.info(
            f"Citing sources built (errors: {source_error_count}, tables: {len(tables_list) if tables_list else 0})",
            extra_dims={
                "source_errors": source_error_count,
                "table_count": len(tables_list) if tables_list else 0,
                "has_sql": bool(sql),
                "has_schema": bool(sql_schema),
                "citing_sources_time_ms": sources_time_ms
            }
        )
        self._logger.info("Citing sources built")

        # Enhanced metrics evaluation logging
        metrics_start_time = time.time()
        tasks = {}
        
        metric_input = MetricInput(
            user_input=message_request.question,
            llm_response=raw_response.response,
            retrieved_context=[sql],
        )
        
        rag_sql_evaluator = Context(RagSQLEvaluator())
        tasks["rag_sql_evaluator"] = asyncio.create_task(
            rag_sql_evaluator.run(metric_input)
        )
        
        if sql_schema:
            self._logger.info("Evaluating SQL quality")
            metric_input = MetricInput(
                user_input=message_request.question,
                llm_response=sql,
                retrieved_context=[sql_schema],
            )
            sql_evaluator = Context(SQLEvaluator())
            tasks["sql_evaluator"] = asyncio.create_task(
                sql_evaluator.run(metric_input)
            )
        
        results_list = await asyncio.gather(*tasks.values())
        results = dict(zip(tasks.keys(), results_list))
        
        sql_evaluator = results.get("sql_evaluator")
        rag_sql_evaluator = results.get("rag_sql_evaluator")
        
        self._logger.info(
            "Metrics evaluation complete",
            extra_dims={
                "evaluators": list(results.keys()),
                "metrics_evaluation_time_ms": round((time.time() - metrics_start_time) * 1000, 2)
            }
        )

        # Enhanced response preparation logging
        response_prep_start = time.time()
        response = {
            "backend": "ey-ip",
            "response": raw_response.response,
            "status_code": 200,
            "sql": {
                "query": sql,
            },
            "chainOfThoughts": ""
        }
        
        # Handle project description flag
        if "#PD" in raw_response.response:
            self._logger.info(
                "Project description flag detected in response",
                extra_dims={"response_type": "project_description"}
            )
            response_text = raw_response.response.replace("#PD", "")
            response.update(
                {
                    "response": response_text,
                    "status_code": 200,
                    "citingSources": [],
                }
            )
            # Log total request time as a metric
            total_time_ms = round((time.time() - request_start_time) * 1000, 2)
            self._logger.log_metric(
                "total_request_time_ms", 
                total_time_ms,
                extra_dims={"response_type": "project_description"}
            )
            return response

        # Handle SQL errors with enhanced logging
        if len(source_errors) > 0 and all(source_errors):
            self._logger.error(
                "Error in all attempts generating and executing SQL queries",
                extra_dims={
                    "error_count": len(source_errors),
                    "response_type": "sql_error"
                }
            )
            response.update(
                {
                    "response": "Error in all the atemps generating and executing the SQL queries",
                    "status_code": 500,
                }
            )
            self._logger.error("Error generating SQL query")
            self._logger.info("Agent response sent")
            # Log total request time as a metric for error case
            total_time_ms = round((time.time() - request_start_time) * 1000, 2)
            self._logger.log_metric(
                "total_request_time_ms", 
                total_time_ms,
                extra_dims={"response_type": "sql_error"}
            )
            return response

        # Enhanced logging for response types
        response_type = "unknown"
        if (
            tables_list
            and raw_response.sources[0].tool_name != "dummy_fallback_function"
        ):
            response.update(
                {
                    "citingSources": [citing_sources],
                    "rawResponse": raw_response,
                }
            )
            response_type = "with_sources"
            self._logger.info(
                "Response generated with citing sources",
                extra_dims={"response_type": response_type}
            )
        elif (
            raw_response.sources
            and raw_response.sources[0].tool_name != "dummy_fallback_function"
        ):
            response.update(
                {"citingSources": [], "rawResponse": raw_response, "status_code": 200}
            )
            response_type = "without_sources"
            self._logger.info(
                "Response generated without citing sources",
                extra_dims={"response_type": response_type}
            )
        else:
            # If you enter here there wasn't any sources
            # in the response, return No Content standard HTTP response
            log_message = (
                "The list of documents retrieved from db was empty"
                if not tables_list
                else "The list of documents was retrieved, but there wasn't any valid source"
            )
            self._logger.info(
                log_message,
                extra_dims={"response_type": "no_content"}
            )

            response.update(
                {
                    "response": "No Content",
                    "status_code": 204,
                }
            )
            response_type = "no_content"

        # Calculate and log final score
        final_score = calculate_final_score([sql_evaluator, rag_sql_evaluator])
        response |= {"score": final_score}
        self._logger.info(
            f"Response ready with final score: {final_score}",
            extra_dims={
                "final_score": final_score,
                "response_preparation_time_ms": round((time.time() - response_prep_start) * 1000, 2),
                "response_type": response_type
            }
        )

        # Log total request time as a metric
        total_time_ms = round((time.time() - request_start_time) * 1000, 2)
        self._logger.log_metric(
            "total_request_time_ms", 
            total_time_ms,
            extra_dims={"response_type": response_type}
        )

        return response
