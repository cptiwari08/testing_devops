from app.core.pydantic_models import QueryPipelineContext
from app.core.prompt_manager import create_prompt_manager  # Added import
from app.project_data.functions.citing_sources_output_retriever import (
    citing_sources_output_parser,
)
from app.project_data.functions.citing_sources_retriever import citing_sources_retriever
from app.project_data.functions.query_parser import query_parser
from app.project_data.functions.query_response_parser import query_response_parser
from app.project_data.functions.response_output_parser import response_output_parser
from app.project_data.functions.send_teamtype_info import send_teamtype
from llama_index.core import PromptTemplate
from llama_index.core.query_pipeline import FnComponent, InputComponent, QueryPipeline


def get_suggestion_pipeline(context: QueryPipelineContext) -> QueryPipeline:
    """
    Creates a suggestion pipeline for processing query suggestions.
    
    Args:
        context: The query pipeline context containing configuration and models
        
    Returns:
        A configured QueryPipeline instance
    """
    # Initialize prompt manager with context logger
    prompt_manager = create_prompt_manager(context.logger)
    
    # Load templates using the synchronous prompt manager method
    final_answer = prompt_manager.get_prompt_sync(
        agent="project_data",
        key="final_answer_retry"
    )
    
    context.logger.info("Setting up suggestion pipeline modules, links and chains")

    # Setting up the shared context in functions before
    # being passed as FnComponent
    query_parser_ = lambda query_output: query_parser(query_output, context)
    query_response_parser_ = lambda query_output: query_response_parser(
        query_output, context
    )
    response_output_parser_ = lambda query_output: response_output_parser(
        query_output, context
    )
    send_teamtype_ = lambda: send_teamtype(context)
    citing_sources_retriever_ = lambda query_str: citing_sources_retriever(query_str, context)
    citing_sources_output_parser_ = lambda citing_sources_output: citing_sources_output_parser(
        citing_sources_output, context
    )
    modules = {
        "final_answer_llm": context.llm_models.llm,
        "final_answer_prompt": PromptTemplate(final_answer),
        "input": InputComponent(),
        "query_parser": FnComponent(fn=query_parser_, async_fn=query_parser_),
        "query_response_parser": FnComponent(
            fn=query_response_parser_, async_fn=query_response_parser_
        ),
        "response_output_parser": FnComponent(fn=response_output_parser_),
        "send_team_type": FnComponent(fn=send_teamtype_),
        "citing_sources_retriever": FnComponent(
            fn=citing_sources_retriever_, async_fn=citing_sources_retriever_
        ),
        "citing_sources_output_parser": FnComponent(fn=citing_sources_output_parser_),
    }
    query_pipeline = QueryPipeline(modules=modules, verbose=False)
    query_pipeline.add_link(
        "input", "query_parser", src_key="suggestion", dest_key="query_output"
    )
    query_pipeline.add_link(
        "query_parser", "query_response_parser", dest_key="query_output"
    )
    query_pipeline.add_link(
        "input", "final_answer_prompt", src_key="query_str", dest_key="query_str"
    )
    query_pipeline.add_link(
        "query_response_parser", "response_output_parser", dest_key="query_output"
    )
    query_pipeline.add_link(
        "query_parser", "final_answer_prompt", dest_key="generated_sql_query"
    )
    query_pipeline.add_link(
        "query_response_parser", "final_answer_prompt", dest_key="sql_query_result"
    )
    # citing sources
    query_pipeline.add_link(
        "input", "citing_sources_retriever", src_key="query_str", dest_key="query_str"
    )
    query_pipeline.add_link(
        "citing_sources_retriever", "citing_sources_output_parser", dest_key="citing_sources_output"
    )
    query_pipeline.add_link(
        "send_team_type",
        "final_answer_prompt",
        dest_key="team_name",
    )
    query_pipeline.add_chain(["final_answer_prompt", "final_answer_llm"])

    return query_pipeline
