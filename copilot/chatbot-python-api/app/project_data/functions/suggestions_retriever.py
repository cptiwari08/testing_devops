from app.core.ai_search import BaseAISearch as AISearch
from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import extract_claim
from azure.search.documents.models import VectorizedQuery
from fastapi import HTTPException
from starlette import status


async def suggestions_retriever(query_str, context: QueryPipelineContext):
    context.logger.info("Executing few shots retriever function component")
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

    
    prj_intention = "prjsuggestions"
    filter=f"indexType eq '{prj_intention}' and metadata/prjsuggestions/visibleToAssistant eq true"
    async with ai_search.async_client as client:
        results = await client.search(
            search_text=query_str,
            vector_queries=[vector_query],
            filter=filter,
            query_type="semantic",
            top=5,
            semantic_configuration_name="mySemanticConfig",
        )
        result_dict = {}
        async for result in results:
            question = result["chunk"]
            sql_query = result["metadata"].get(prj_intention, {})["sqlQuery"]
            result_dict[question] = {"sql_query":sql_query,"score":result["@search.reranker_score"]}
    return result_dict
