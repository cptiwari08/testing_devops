from app.core.ai_search import BaseAISearch as AISearch
from app.core.nltk import NLTKManager
from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import extract_claim
from azure.search.documents.models import VectorizedQuery
from fastapi import HTTPException
from starlette import status


async def few_shots_retriever(query_str, context: QueryPipelineContext):    
    context.logger.info("I executed the few shots retriever function component")
    auth_token = context.token
    index_name = extract_claim(auth_token, "metadata_index_name")
    if not index_name:
        raise HTTPException(
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
            detail="Invalid AI index name",
        )
    aisearch_instance_name = extract_claim(auth_token, "ai_search_instance_name")
    if not aisearch_instance_name:
        raise HTTPException(
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
            detail="Invalid AI Search instance name",
        )

    embedding = context.llm_models.embed_model.get_text_embedding(query_str)

    vector_query = VectorizedQuery(
        vector=embedding, k_nearest_neighbors=3, fields="embedding"
    )

    ai_search = AISearch(context.llm_models)
    await ai_search.async_set_client(index_name, aisearch_instance_name)

    nltk_ = NLTKManager()
    prj_intention = "prjsuggestions"
    filter=f"indexType eq '{prj_intention}'"
    async with ai_search.async_client as client:
        results = await client.search(
            search_text=nltk_.clean_and_tokenize_text(query_str),
            vector_queries=[vector_query],
            filter=filter,
            query_type="semantic",
            top=5,
            semantic_configuration_name="mySemanticConfig",
        )
        result_strs = []
        alt_result_strs = []
        async for result in results:
            question = (
                result["chunk"]
                + f". The TeamType ID is {context.message_request.context.appInfo.teamTypeIds[0]}"
            )
            sql_query = result["metadata"].get(prj_intention, {})["sqlQuery"]
            result_str = f"""\
            ## Question: {question}
            ## Rewritten query: {sql_query}"""
            alt_result_strs.append(result_str)
            if result["@search.reranker_score"] > 2 and (result["metadata"].get(prj_intention) is not None):
                result_strs.append(result_str)
        if not result_strs:
            result_strs = alt_result_strs[:2]
    final_results = "\n\n".join(result_strs)
    return final_results
