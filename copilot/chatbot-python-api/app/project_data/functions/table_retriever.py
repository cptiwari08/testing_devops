from app.core.ai_search import BaseAISearch as AISearch
from app.core.nltk import NLTKManager
from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import extract_claim
from azure.search.documents.models import VectorizedQuery
from fastapi import HTTPException
from starlette import status


async def table_retriever(query_str, context: QueryPipelineContext):
    context.logger.info("I initiated the execution of the table retriever function component to gather the necessary data")
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
    prj_intention = "prjdata"

    async with ai_search.async_client as client:
        results = await client.search(
            search_text=nltk_.clean_and_tokenize_text(query_str),
            vector_queries=[vector_query],
            filter=f"indexType eq '{prj_intention}' and metadata/{prj_intention}/isMainTable eq 'True'",
            query_type="semantic",
            top=10,
            semantic_configuration_name="mySemanticConfig",
        )

        results_list = []
        async for i in results:
            table_name = i["metadata"].get(prj_intention, {})["tableName"]
            description = i["chunk"]
            table_dict = {"table_name": table_name, "description": description}
            results_list.append(table_dict)
    return str(results_list)
