import asyncio
import json
import time
from typing import Any

from app.core.config import MemorySize
from app.core.interfaces import IBaseLogger
from app.core.memory import MemoryManager
from app.core.pydantic_models import LLMModels, MessageRequest, QueryPipelineContext
from app.project_data.functions.calculation_check import calculation_check
from app.project_data.functions.merge_responses import merge_responses
from app.project_data.functions.process_responses import walker_agent
from app.project_data.functions.split_question import split_question
from app.project_data.functions.suggestions_retriever import suggestions_retriever
from app.project_data.functions.identify_if_suggestion import identify_if_suggestion
from app.project_data.services.response_post_processing import response_post_processing
from app.project_data.services.splited_query_pipeline_pdata import SplitedPipelinePData
from app.project_data.services.splited_query_pipeline_standard import (
    SplitedPipelineStandard,
)
from app.project_data.services.suggestion_pipeline import get_suggestion_pipeline


class Service:
    def __init__(self, logger: IBaseLogger, llm_models: LLMModels) -> None:
        super().__init__()
        self._logger = logger
        self.llm_models = llm_models
        self.splited_pipeline_pdata = None
        self.splited_pipeline_standard = None
        
        # Log service initialization
        self._logger.info("I initialized the Project Data Service.", extra_dims={"component": "Project Data", "service": "project_data"})

    # Even Any is not a good tying that is what the arun method returns
    # is on the llama-index side
    async def execute(self, message_request: MessageRequest, authorization: str) -> Any:
        # Track execution start time for performance metrics
        request_start_time = time.time()
        execution_metrics = {
            "component": "Project Data",
            "operation": "query_execution",
            "chat_length": len(message_request.chatHistory) if message_request.chatHistory else 0,
            "question_length": len(message_request.question) if message_request.question else 0
        }
        
        # Enhanced request logging with truncated question for privacy/length
        question_preview = message_request.question[:50] + "..." if len(message_request.question) > 50 else message_request.question
        """ self._logger.info(
            f"Processing Project Data request: '{question_preview}'", 
            extra_dims=execution_metrics
        ) """
        self._logger.info(
            f"I am currently processing the project data request: '{question_preview}'", 
            extra_dims=execution_metrics
        )

        context = QueryPipelineContext(
            logger=self._logger,
            token=authorization,
            message_request=message_request,
            llm_models=self.llm_models,
            callback_handlers=self.llm_models.llm.callback_manager.handlers,
        )
        question = message_request.question

        self._logger.info("I'm processing the question for Project Data.")
        
        # Log memory generation
        memory_start_time = time.time()
        self._logger.info(f"Generating chat memory (size: {MemorySize.project_data})")
        chat_history = MemoryManager.generate_chat_messages(
            message_request.chatHistory, size=MemorySize.project_data  # type: ignore
        )
        
        chat_history_as_string = ""
        if chat_history:
            messages = []
            for (
                message
            ) in chat_history:  # currently the memory is limited to 6 message only
                messages.append(f"- {message.role.value.upper()}: {message.content}")

            chat_history_as_string = "\n".join(messages)
            
        self._logger.info(
            f"Chat memory generated with {len(chat_history)} messages",
            extra_dims={
                "memory_message_count": len(chat_history),
                "memory_generation_time_ms": round((time.time() - memory_start_time) * 1000, 2)
            }
        )
            
        is_suggestion = (
            message_request.context
            and message_request.context.suggestion
            and message_request.context.suggestion.sqlQuery
        )
        
        # Direct suggestion query path
        if is_suggestion:
            # Log suggestion processing
            self._logger.info("Generating response with explicit suggestion query", 
                             extra_dims={"execution_path": "suggestion_direct"})
            
            suggestion_start = time.time()
            pipeline = get_suggestion_pipeline(context)
            
            # Log SQL query details
            sql_query = message_request.context.suggestion.sqlQuery  # type: ignore
            sql_query_length = len(sql_query) if sql_query else 0
            self._logger.info(f"Processing SQL suggestion query ({sql_query_length} chars)",
                             extra_dims={"sql_query_length": sql_query_length})
            
            # Execute suggestion pipeline
            pipeline_start = time.time()
            raw_response = await pipeline.arun_multi(
                {
                    "input": {
                        "query_str": message_request.question,
                        "suggestion": sql_query,
                    },
                    "send_team_type": {},
                }
            )
            pipeline_duration = round((time.time() - pipeline_start) * 1000, 2)
            self._logger.info(f"Suggestion pipeline executed in {pipeline_duration}ms", 
                             extra_dims={"pipeline_duration_ms": pipeline_duration})
            
            # Process response
            post_processing_start = time.time()
            responses = await response_post_processing(raw_response, context, sql_query)  # type: ignore
            post_processing_duration = round((time.time() - post_processing_start) * 1000, 2)
            
            # Log total suggestion path timing
            total_suggestion_time = round((time.time() - suggestion_start) * 1000, 2)
            self._logger.info(f"Suggestion response generated in {total_suggestion_time}ms", 
                             extra_dims={
                                 "total_suggestion_time_ms": total_suggestion_time,
                                 "post_processing_time_ms": post_processing_duration,
                                 "response_type": "suggestion"
                             })
            
            # Log total request time
            total_time = round((time.time() - request_start_time) * 1000, 2)
            self._logger.log_metric("total_request_time_ms", total_time, 
                                   extra_dims={"execution_path": "suggestion_direct"})
            
            return responses
        else:
            # Log suggestions retriever start
            suggestion_retrieval_start = time.time()
            self._logger.info("I am retrieving the matching suggestion templates")
            
            suggestions = await suggestions_retriever(
                query_str=question, context=context
            )
            
            suggestion_count = len(suggestions.keys()) if suggestions else 0            
            self._logger.info(f"I have retrieved {suggestion_count} suggestion templates",
                             extra_dims={
                                 "suggestion_count": suggestion_count,
                                 "retrieval_time_ms": round((time.time() - suggestion_retrieval_start) * 1000, 2)
                             })

            # Check if the question matches a suggestion template
            match_check_start = time.time()
            check_if_suggestion = await identify_if_suggestion(
                question=question, question_bank= ", ".join(suggestions.keys()), llm_models=context.llm_models
            )

            check_if_suggestion_str = str(check_if_suggestion).replace("\n","")
            match_check_duration = round((time.time() - match_check_start) * 1000, 2)
            
            # Free text question that matches a suggestion template
            if check_if_suggestion_str != "No match":
                self._logger.info(
                    f"Question matched suggestion template: '{check_if_suggestion_str[:50]}...' if len(check_if_suggestion_str) > 50 else check_if_suggestion_str",
                    extra_dims={
                        "execution_path": "suggestion_match",
                        "match_check_time_ms": match_check_duration
                    }
                )
                
                # Process matched suggestion
                matched_start = time.time()
                self._logger.info("Generating response with matched suggestion query")
                
                pipeline = get_suggestion_pipeline(context)
                
                # Get SQL query from matched suggestion
                sql_query = suggestions[check_if_suggestion_str]["sql_query"]
                sql_query_length = len(sql_query) if sql_query else 0
                self._logger.info(f"Using matched SQL query ({sql_query_length} chars)",
                                 extra_dims={"sql_query_length": sql_query_length})
                
                # Execute suggestion pipeline
                pipeline_start = time.time()
                raw_response = await pipeline.arun_multi(
                    {
                        "input": {
                            "query_str": check_if_suggestion_str,
                            "suggestion": sql_query,
                        },
                        "send_team_type": {},
                    }
                )
                pipeline_duration = round((time.time() - pipeline_start) * 1000, 2)
                self._logger.info(f"Matched suggestion pipeline executed in {pipeline_duration}ms", 
                                 extra_dims={"pipeline_duration_ms": pipeline_duration})
                
                # Personalize SQL query with user's email
                personalized_sql_query = str(sql_query).replace(r'{Username}', message_request.context.user.email)  # type: ignore
                
                # Process response
                post_processing_start = time.time()
                responses = await response_post_processing(raw_response, context, personalized_sql_query)  # type: ignore
                post_processing_duration = round((time.time() - post_processing_start) * 1000, 2)
                
                # Log total matched suggestion path timing
                total_matched_time = round((time.time() - matched_start) * 1000, 2)
                self._logger.info(f"Matched suggestion response generated in {total_matched_time}ms", 
                                 extra_dims={
                                     "total_matched_time_ms": total_matched_time,
                                     "post_processing_time_ms": post_processing_duration,
                                     "response_type": "matched_suggestion"
                                 })
                
                # Log total request time
                total_time = round((time.time() - request_start_time) * 1000, 2)
                self._logger.log_metric("total_request_time_ms", total_time, 
                                       extra_dims={"execution_path": "suggestion_match"})
                
                return responses
            else:
                # Log no match found, proceeding with question splitting                
                self._logger.info("No matching suggestion was found, so I will proceed with splitting the question.",
                                 extra_dims={
                                     "execution_path": "complex_question",
                                     "match_check_time_ms": match_check_duration
                                 })
                
                # Question splitting process
                splitting_start = time.time()
                self._logger.info("I split the complex question into sub-questions")
                
                questions = await split_question(
                    question, chat_history_as_string, context.llm_models
                )
                sub_questions = questions["sub_questions"]
                rewritten_question = questions["rewritten_question"]
                
                self._logger.info(f"Question splitting resulting in a total of {len(sub_questions)}, which are: {sub_questions}", 
                                 extra_dims={
                                     "sub_question_count": len(sub_questions),
                                     "splitting_time_ms": round((time.time() - splitting_start) * 1000, 2)
                                 })
                
                # Categorize sub-questions by calculation needs
                calculation_check_start = time.time()
                has_calculation = []
                do_not_has_calculation = []

                async def process_sub_questions(sub_question: str) -> None:
                    check_start = time.time()
                    is_calculation = await calculation_check(sub_question, self.llm_models)
                    check_duration = round((time.time() - check_start) * 1000, 2)
                    
                    self._logger.debug(
                        f"Sub-question calculation check: {is_calculation}",
                        extra_dims={
                            "sub_question_preview": sub_question[:30] + "..." if len(sub_question) > 30 else sub_question,
                            "requires_calculation": is_calculation,
                            "check_duration_ms": check_duration
                        }
                    )
                    
                    if is_calculation:
                        has_calculation.append(sub_question)
                    else:
                        do_not_has_calculation.append(sub_question)

                await asyncio.gather(
                    *[process_sub_questions(sub_question) for sub_question in sub_questions]
                )
                
                self._logger.info(
                    f"I categorized the sub-questions: {len(has_calculation)} require calculation, {len(do_not_has_calculation)} don't",
                    extra_dims={
                        "calculation_questions": len(has_calculation),
                        "standard_questions": len(do_not_has_calculation),
                        "categorization_time_ms": round((time.time() - calculation_check_start) * 1000, 2)
                    }
                )
                
                # Process all sub-questions in parallel
                tasks = []
                pipeline_start = time.time()
                self._logger.info("I initialized the pipeline execution for all sub-questions")
                
                # Process calculation questions
                for question_ in has_calculation:
                    self._logger.debug(f"Setting up calculation pipeline for: {question_[:30]}...")
                    pdpipeline = SplitedPipelinePData(context)
                    data = {
                        "question_rewritting_prompt": {
                            "query_str": question_,
                            "user_email": message_request.context.user.email,  # type: ignore
                            "team_type_id": str(message_request.context.appInfo.teamTypeIds),  # type: ignore
                        },
                    }
                    task = asyncio.create_task(pdpipeline.execute_pipelines(data))
                    tasks.append(task)
                    
                # Process non-calculation questions
                for question_ in do_not_has_calculation:
                    self._logger.debug(f"I am setting up standard pipeline for: {question_[:30]}...")
                    pipeline = SplitedPipelineStandard(context)
                    data = {
                        "question_rewritting_prompt": {
                            "query_str": question_,
                            "user_email": message_request.context.user.email,  # type: ignore
                            "team_type_id": str(message_request.context.appInfo.teamTypeIds),  # type: ignore
                        },
                        "send_teamtype": {
                        }
                    }
                    task = asyncio.create_task(pipeline.execute_pipelines(data))
                    tasks.append(task)

                # Execute all pipelines in parallel
                self._logger.info(f"I executed {len(tasks)} pipeline(s) in parallel")
                results = await asyncio.gather(*tasks)
                pipeline_duration = round((time.time() - pipeline_start) * 1000, 2)
                self._logger.info(f"After all previous steps all pipelines completed successfully in {pipeline_duration}ms", 
                                 extra_dims={"pipeline_duration_ms": pipeline_duration})
                
                results_qp = results
                results = zip(sub_questions, results)

                # Merge responses or use walker agent based on calculation needs
                response_merge_start = time.time()
                if not has_calculation:
                    self._logger.info("I then merged the responses from the standard questions")
                    final_answer = await merge_responses(
                        rewritten_question, results, context.llm_models
                    )
                    self._logger.info("The standard responses were merged successfully")
                else:
                    self._logger.info("I used the walker agent to process calculation responses")
                    final_answer = await walker_agent(
                        question=rewritten_question,
                        evidence=results,
                        llm_models=context.llm_models,
                    )
                    self._logger.info(f"Walker agent processing completed with this response: {final_answer}")
                
                response_merge_duration = round((time.time() - response_merge_start) * 1000, 2)
                self._logger.info(f"I completed the response consolidation in {response_merge_duration}ms", 
                                 extra_dims={"response_merge_time_ms": response_merge_duration})
                
                # Post-process all individual responses
                post_processing_start = time.time()
                self._logger.info("I engaged in post-processing of the individual responses")
                tasks = []
                for result in results_qp:
                    task = asyncio.create_task(response_post_processing(result, context))
                    tasks.append(task)
                responses = await asyncio.gather(*tasks)
                post_processing_duration = round((time.time() - post_processing_start) * 1000, 2)
                self._logger.info(f"The post-processing was completed in {post_processing_duration}ms", 
                                 extra_dims={"post_processing_time_ms": post_processing_duration})
                
                # Aggregate source information
                sources_start = time.time()
                self._logger.info("I aggregated the source information")
                set_page_keys = list()
                set_table_keys = list()
                for response in responses:
                    for value_ in response.get("citingSources", []):
                        if value_['sourceType']=='page-key':
                            if value_['sourceValue']:
                                set_page_keys.append(value_['sourceValue'][0])
                        elif value_['sourceType']=='table':
                            set_table_keys.extend(value_['sourceValue'])
                citing_sources = [{"sourceName": "project-data", "sourceType": "page-key", "sourceValue": set_page_keys},
                                {"sourceName": "project-data", "sourceType": "table", "sourceValue": list(set(set_table_keys))}]

                self._logger.info(
                    f"I aggregated the source information, which included a total of {len(set_page_keys)} pages and {len(set(set_table_keys))} tables, specifically: {set_page_keys} and {set_table_keys}", 
                    extra_dims={
                        "page_source_count": len(set_page_keys),
                        "table_source_count": len(set(set_table_keys)),
                        "source_aggregation_time_ms": round((time.time() - sources_start) * 1000, 2)
                    }
                )

                # Aggregate SQL information
                sql_start = time.time()
                self._logger.info("I successfully aggregated the SQL information")
                final_sql = {"query": [], "result": []}
                for response in responses:
                    sql_data = response.get("sql", {})
                    final_sql["query"].append(sql_data.get("query", ""))
                    final_sql["result"].append(sql_data.get("result", ""))
                final_sql["query"] = json.dumps(final_sql["query"])  # type: ignore
                final_sql["result"] = json.dumps(final_sql["result"])  # type: ignore
                
                self._logger.info(f"The SQL information was successfully aggregated", 
                                 extra_dims={"sql_aggregation_time_ms": round((time.time() - sql_start) * 1000, 2)})

                # Prepare final response
                response={
                    "backend": "project-data",
                    "response": final_answer,
                    "citingSources": citing_sources,
                    "sql": final_sql,
                    "chainOfThoughts": self._logger.get_log_messages()
                }
                
                # Log completion metrics
                response_length = len(final_answer) if final_answer else 0
                total_time = round((time.time() - request_start_time) * 1000, 2)
                
                self._logger.info(
                    f"Complex question response generated ({response_length} chars) in {total_time}ms",
                    extra_dims={
                        "response_length": response_length,
                        "execution_path": "complex_question",
                        "response_type": "merged_subquestions",
                        "has_calculation": len(has_calculation) > 0
                    }
                )
                
                # Log total request time
                self._logger.log_metric("total_request_time_ms", total_time, 
                                       extra_dims={
                                           "execution_path": "complex_question",
                                           "subquestion_count": len(sub_questions)
                                       })
                
                return response
