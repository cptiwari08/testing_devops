import asyncio

import httpx
from app.core.errors import ProgramOfficeResponseError, ProgramOfficeTimeoutError
from app.core.pydantic_models import QueryPipelineContext
from app.core.prompt_manager import create_prompt_manager  # Added import
from app.project_data.functions.citing_sources_output_retriever import (
    citing_sources_output_parser,
)
from app.project_data.functions.citing_sources_retriever import citing_sources_retriever
from app.project_data.functions.context_parser import context_parser
from app.project_data.functions.context_parser_output import context_parser_output
from app.project_data.functions.context_retriever import context_retriever
from app.project_data.functions.denormalized_query_generator import (
    denormalized_query_generator,
)
from app.project_data.functions.few_shots_retriever import few_shots_retriever
from app.project_data.functions.query_output_paser import query_output_parser
from app.project_data.functions.query_parser import query_parser
from app.project_data.functions.query_response_parser import query_response_parser
from app.project_data.functions.query_response_parser_retry import (
    query_response_parser_retry,
)
from app.project_data.functions.question_rewritting_parser import (
    question_rewritting_parser,
)
from app.project_data.functions.question_rewritting_parser_output import (
    question_rewritting_parser_output,
)
from app.project_data.functions.table_parser import table_parser
from app.project_data.functions.table_parser_output import table_parser_output
from app.project_data.functions.table_retriever import table_retriever
from llama_index.core import PromptTemplate
from llama_index.core.query_pipeline import FnComponent, QueryPipeline


class SplitedPipelinePData:
    def __init__(self, context: QueryPipelineContext):
        super().__init__()
        self.context = context
        questions_words = set(context.message_request.question.upper().split())
        
        # Initialize prompt manager
        self._prompt_manager = create_prompt_manager(context.logger)
        
        # Get acronyms synchronously
        acronyms = self._prompt_manager.get_prompt_sync(
            agent="project_data",
            key="acronyms",
            raw_data=True
        )
        
        self.list_acronyms = []
        
        # Process acronyms
        if isinstance(acronyms, dict):
            for acronym, description in acronyms.items():
                if acronym in questions_words:
                    self.list_acronyms.append((acronym, description))
        
        # Loading prompts synchronously
        self.table_reranker_prompt = self._prompt_manager.get_prompt_sync(
            agent="project_data",
            key="table_custom_reranker_prompt"
        )
        
        self.question_rewritting_prompt = self._prompt_manager.get_prompt_sync(
            agent="project_data",
            key="question_rewritting_NO_TEAM_TYPE"
        )
        
        self.query_rewritting_prompt = self._prompt_manager.get_prompt_sync(
            agent="project_data",
            key="sql_rewritting_raw"
        )
        
        self.query_rewritting_retry_prompt = self._prompt_manager.get_prompt_sync(
            agent="project_data",
            key="sql_rewritting_raw_retry"
        )
        
        self.final_answer = self._prompt_manager.get_prompt_sync(
            agent="project_data",
            key="final_answer_raw"
        )

        context.logger.info("I set up the pipeline modules, links, and chains")

        self.context_parser_ = lambda nodes: context_parser(nodes, context)
        self.context_retriever_ = lambda query_str, tables_list: context_retriever(
            query_str, tables_list, context
        )
        self.context_parser_output_ = lambda context_output: context_parser_output(
            context_output, context
        )
        self.denormalized_query_generator_ = (
            lambda tables_list: denormalized_query_generator(tables_list, context)
        )
        self.few_shots_retriever_ = lambda query_str: few_shots_retriever(
            query_str, context
        )
        self.question_rewritting_parser_ = (
            lambda question_output: question_rewritting_parser(question_output, context)
        )
        self.question_rewritting_parser_output_ = (
            lambda question_output: question_rewritting_parser_output(
                question_output, context
            )
        )
        self.table_retriever_ = lambda query_str: table_retriever(query_str, context)
        self.table_parser_ = lambda reranker_result_str: table_parser(
            reranker_result_str, context
        )
        self.table_parser_output_ = lambda table_output: table_parser_output(
            table_output, context
        )

        self.denormalized_query_generator_ = (
            lambda tables_list: denormalized_query_generator(tables_list, context)
        )
        self.few_shots_retriever_ = lambda query_str: few_shots_retriever(
            query_str, context
        )
        self.query_parser_ = lambda query_output: query_parser(query_output, context)
        self.query_output_parser_ = lambda query_output: query_output_parser(
            query_output, context
        )
        self.query_response_parser_ = lambda query_output: query_response_parser(
            query_output, context
        )
        self.query_response_parser_retry_ = (
            lambda query_output: query_response_parser_retry(query_output, context)
        )
        self.denormalized_query_generator_ = (
            lambda tables_list: denormalized_query_generator(tables_list, context)
        )
        self.citing_sources_retriever_ = lambda query_str: citing_sources_retriever(query_str, context)
        self.citing_sources_output_parser_ = lambda citing_sources_output: citing_sources_output_parser(
            citing_sources_output, context
        )

    def build_second_dict(self, input_dict: dict):
        map_new_names = {
            "question_rewritting_parser_output": "query_str",
            "few_shots_retriever": "few_shots_suggested_questions",
            "context_parser_output": "nodes",
            "denormalized_query_generator": "denormalized_query",
        }
        map_inputs = {
            "query_rewritting_prompt": [
                "question_rewritting_parser_output",
                "few_shots_retriever",
                "context_parser_output",
                "denormalized_query_generator",
            ]
        }
        new_maped_dicto = {}
        for key, value in map_inputs.items():
            inside_dicto = {}
            for item_ in value:
                inside_dicto[map_new_names[item_]] = input_dict[item_]["output"]
            new_maped_dicto[key] = inside_dicto
        return new_maped_dicto

    def build_final_dict(self, input_dict: dict):
        map_new_names = {
            "query_parser_output": "generated_sql_query",
            "context_parser_output": "sql_query_result",
            "question_rewritting_parser_output": "query_str",
        }

        map_inputs = {
            "final_answer_prompt": [
                "query_parser_output",
                "context_parser_output",
                "question_rewritting_parser_output",
            ]
        }
        new_maped_dicto = {}
        for key, value in map_inputs.items():
            inside_dicto = {}
            for item_ in value:
                inside_dicto[map_new_names[item_]] = input_dict[item_]["output"]
            new_maped_dicto[key] = inside_dicto
        return new_maped_dicto

    async def execute_pipelines(
        self, input_data: dict, number_tries=4, max_retries=2
    ):
        """Function to execute the pipeline"""
        first_pipeline = await self.execute_first_step(input_data)
        second_dict = self.build_second_dict(first_pipeline)
        ### The idea is to execute the second pipeline multiple times and keep the best?
        second_pipeline = await self.execute_second_step(second_dict)
        response_parser_retry = second_pipeline["query_response_parser_retry"]["output"]
        second_pipeline["query_response_parser"] = {
            "output": response_parser_retry["output"]
        }
        second_pipeline.pop("query_response_parser_retry")
        if response_parser_retry.get("intention") == "retry":  ## If the score is less than 0.6 (for the sql generated) we retry the pipeline
            for retry_attend_number in range(max_retries):
                retry_log_message = f"Retry #{retry_attend_number + 1} for question: {self.context.message_request.question}"  # type: ignore
                self.context.logger.info(retry_log_message)
                error_captured = ""
                if response_parser_retry.get("message_error"):
                    error_captured = (
                        f"System error:{response_parser_retry.get('message_error')}"
                    )
                query = second_pipeline["query_parser_output"]["output"]
                second_dict["query_rewritting_prompt"]["wrong_sql_query"] = query
                second_dict["query_rewritting_prompt"]["feedback"] = error_captured
                dict_retry = {
                    "query_rewritting_retry_prompt": second_dict["query_rewritting_prompt"]
                }
                second_pipeline = await self.build_retry_dict(
                    dict_retry, first_pipeline, number_tries
                )
                response_parser_retry = second_pipeline["query_response_parser_retry"]["output"]
                if response_parser_retry.get("intention") == "normal":
                    break

            if response_parser_retry.get("intention") == "retry":
                response = response_parser_retry["output"]
                url = response_parser_retry.get("url")
                try:
                    response.raise_for_status()
                except httpx.TimeoutException:
                    raise ProgramOfficeTimeoutError(url)
                except httpx.HTTPStatusError:
                    raise ProgramOfficeResponseError(response)
            second_pipeline["query_response_parser"] = {
                "output": response_parser_retry["output"]
            }
            second_pipeline.pop("query_response_parser_retry")

        first_pipeline.update(second_pipeline)
        final_pipeline_dict = self.build_final_dict(first_pipeline)
        final_pipeline = await self.execute_final_step(final_pipeline_dict)
        final_pipeline.update(first_pipeline)
        return final_pipeline

    async def build_retry_dict(
        self, input_dict: dict, pipeline_dicto: dict, number_tries: int
    ):
        tasks = {
            f"retry_number_{index}": asyncio.create_task(
                self.execute_retry_score(input_dict, pipeline_dicto, index)
            )
            for index in range(number_tries)
        }
        results_list = await asyncio.gather(*tasks.values())
        results = dict(zip(tasks.keys(), results_list))
        filtered_results = {
            key: value
            for key, value in results.items()
            if value["pipeline"]["query_response_parser_retry"]["output"]["intention"]
            == "normal"
        }
        if not filtered_results:
            second_pipeline = results.pop('retry_number_0')['pipeline']
        else:
            selected_retry = max(
                filtered_results,
                key=lambda k: len(filtered_results[k]["pipeline"]["query_response_parser_retry"]["output"]["output"])
            )
            second_pipeline = filtered_results[selected_retry]["pipeline"]

        return second_pipeline

    async def execute_retry_score(
        self, input_data: dict, previous_input: dict, number_tries
    ):
        """Function to execute the retries pipelines in parallel
        Args: input_data: dict, previous_input: dict"""
        final_pipeline = await self.execute_retry_step(input_data, number_tries)

        return {"pipeline": final_pipeline}

    async def execute_final_step(self, input_data: dict):
        """Execute the final step of the pipeline"""
        modules_final_pipeline = {
            "final_answer_llm": self.context.llm_models.llm,
            "final_answer_prompt": PromptTemplate(self.final_answer),
        }

        query_pipeline = QueryPipeline(modules=modules_final_pipeline, verbose=False)

        query_pipeline.add_chain(["final_answer_prompt", "final_answer_llm"])
        raw_response = await query_pipeline.arun_multi(input_data)
        return raw_response

    async def execute_retry_step(self, input_data: dict, number_retry: int):
        """Execute the retry step of the pipeline"""
        modules_second_pipeline = {
            "query_rewritting_retry_prompt": PromptTemplate(
                self.query_rewritting_retry_prompt
            ),
            "query_rewritting_llm": self.context.llm_models.llm,
            "query_parser": FnComponent(
                fn=self.query_parser_, async_fn=self.query_parser_
            ),
            "query_response_parser_retry": FnComponent(
                fn=self.query_response_parser_retry_,
                async_fn=self.query_response_parser_retry_,
            ),
            "query_parser_output": FnComponent(fn=self.query_output_parser_),
        }

        query_pipeline = QueryPipeline(modules=modules_second_pipeline, verbose=False)
        # Denormalized query generator
        query_pipeline.add_chain(
            ["query_rewritting_retry_prompt", "query_rewritting_llm"]
        )
        # Final response
        query_pipeline.add_link(
            "query_rewritting_llm", "query_parser", dest_key="query_output"
        )
        query_pipeline.add_link(
            "query_parser", "query_response_parser_retry", dest_key="query_output"
        )
        query_pipeline.add_link(
            "query_parser", "query_parser_output", dest_key="query_output"
        )
        raw_response = await query_pipeline.arun_multi(input_data)
        return raw_response

    async def execute_second_step(self, input_data: dict):
        """Execute the second step of the pipeline"""
        modules_second_pipeline = {
            "query_rewritting_prompt": PromptTemplate(self.query_rewritting_prompt),
            "query_rewritting_llm": self.context.llm_models.llm,
            "query_parser": FnComponent(
                fn=self.query_parser_, async_fn=self.query_parser_
            ),
            "query_response_parser_retry": FnComponent(
                fn=self.query_response_parser_retry_,
                async_fn=self.query_response_parser_retry_,
            ),
            "query_parser_output": FnComponent(fn=self.query_output_parser_),
        }

        query_pipeline = QueryPipeline(modules=modules_second_pipeline, verbose=False)
        # Denormalized query generator
        query_pipeline.add_chain(["query_rewritting_prompt", "query_rewritting_llm"])
        # Final response
        query_pipeline.add_link(
            "query_rewritting_llm", "query_parser", dest_key="query_output"
        )
        query_pipeline.add_link(
            "query_parser", "query_response_parser_retry", dest_key="query_output"
        )
        query_pipeline.add_link(
            "query_parser", "query_parser_output", dest_key="query_output"
        )
        raw_response = await query_pipeline.arun_multi(input_data)
        return raw_response

    async def execute_first_step(self, input_data: dict):
        """Execute the first step of the pipeline"""
        modules_first_pipeline = {
            "context_parser": FnComponent(fn=self.context_parser_),
            "context_retriever": FnComponent(
                fn=self.context_retriever_, async_fn=self.context_retriever_
            ),
            "context_parser_output": FnComponent(fn=self.context_parser_output_),
            "few_shots_retriever": FnComponent(
                fn=self.few_shots_retriever_, async_fn=self.few_shots_retriever_
            ),
            "question_rewritting_llm": self.context.llm_models.llm,
            "question_rewritting_parser": FnComponent(
                fn=self.question_rewritting_parser_
            ),
            "question_rewritting_prompt": PromptTemplate(
                self.question_rewritting_prompt
            ),
            "question_rewritting_parser_output": FnComponent(
                fn=self.question_rewritting_parser_output_
            ),
            "table_parser": FnComponent(fn=self.table_parser_),
            "table_reranker_custom_prompt": PromptTemplate(self.table_reranker_prompt),
            "table_reranker_llm": self.context.llm_models.llm,
            "table_retriever": FnComponent(
                fn=self.table_retriever_, async_fn=self.table_retriever_
            ),
            "table_parser": FnComponent(fn=self.table_parser_),
            # "send_teamtype": FnComponent(fn=self.send_teamtype_),
            # "send_teamtype_output": FnComponent(fn=self.send_teamtype_),
            "denormalized_query_generator": FnComponent(
                fn=self.denormalized_query_generator_,
                async_fn=self.denormalized_query_generator_,
            ),
            "citing_sources_retriever": FnComponent(
                fn=self.citing_sources_retriever_, async_fn=self.citing_sources_retriever_
                ),
            "citing_sources_output_parser": FnComponent(fn=self.citing_sources_output_parser_),
        }

        query_pipeline = QueryPipeline(modules=modules_first_pipeline, verbose=False)
        # Question rewriting
        query_pipeline.add_chain(
            ["question_rewritting_prompt", "question_rewritting_llm"]
        )
        query_pipeline.add_link(
            "question_rewritting_llm",
            "question_rewritting_parser",
            dest_key="question_output",
        )
        query_pipeline.add_link(
            "question_rewritting_parser",
            "question_rewritting_parser_output",
            dest_key="question_output",
        )
        # Table retriever
        query_pipeline.add_link(
            "question_rewritting_parser", "table_retriever", dest_key="query_str"
        )
        query_pipeline.add_link(
            "question_rewritting_parser",
            "table_reranker_custom_prompt",
            dest_key="query_str",
        )
        query_pipeline.add_link(
            "table_retriever", "table_reranker_custom_prompt", dest_key="descriptions"
        )
        query_pipeline.add_chain(["table_reranker_custom_prompt", "table_reranker_llm"])
        query_pipeline.add_link(
            "table_reranker_llm", "table_parser", dest_key="reranker_result_str"
        )
        # query_pipeline.add_link(
        #     "table_parser", "table_parser_output", dest_key="table_output"
        # )

        # Context retriever
        query_pipeline.add_link(
            "table_parser", "context_retriever", dest_key="tables_list"
        )
        query_pipeline.add_link(
            "question_rewritting_parser", "context_retriever", dest_key="query_str"
        )
        query_pipeline.add_link("context_retriever", "context_parser", dest_key="nodes")
        query_pipeline.add_link(
            "context_parser", "context_parser_output", dest_key="context_output"
        )
        # Few shots retriever
        query_pipeline.add_link(
            "question_rewritting_parser",
            "few_shots_retriever",
            dest_key="query_str",
        )
        # Denormalized query generator
        query_pipeline.add_link(
            "table_parser", "denormalized_query_generator", dest_key="tables_list"
        )
        # citing sources
        query_pipeline.add_link(
            "question_rewritting_parser", "citing_sources_retriever", dest_key="query_str"
        )
        query_pipeline.add_link(
            "citing_sources_retriever", "citing_sources_output_parser", dest_key="citing_sources_output"
        )

        # query_pipeline.add_link(
        #     "send_teamtype",
        #     "send_teamtype_output",
        #     dest_key="team_name",
        # )
        raw_response = await query_pipeline.arun_multi(input_data)
        return raw_response
