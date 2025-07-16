from app.core.ai_search import BaseAISearch as AISearch
from app.core.pydantic_models import QueryPipelineContext
from app.core.utils import extract_claim
from azure.search.documents.models import VectorizedQuery
from fastapi import HTTPException
from llama_index.core import Document, VectorStoreIndex
from llama_index.core.base.base_retriever import BaseRetriever
from starlette import status


async def search_by_tag(client,clean_query,vector_query,n,new_doc,tag_type,main_table, prj_intention):
    filter = f"indexType eq '{prj_intention}' and (search.ismatch('|{main_table}|','metadata/{prj_intention}/tableName') or metadata/{prj_intention}/tableName eq 'AllTables') and metadata/{prj_intention}/isSchema eq 'False' and metadata/{prj_intention}/tag eq '{tag_type}'"
    results = await client.search(
            search_text=clean_query,
            vector_queries=[vector_query],
            filter=filter,
            query_type="semantic",
            top=n,
            semantic_configuration_name="mySemanticConfig",
        )

    async for result in results:
            if result["@search.reranker_score"] and result["@search.reranker_score"] > 1:
                metadata = {"tableName": result["metadata"].get(prj_intention, {})["tableName"], "tag": result["metadata"].get(prj_intention, {})["tag"]}
                doc = Document(
                    doc_id=result["id"],
                    embedding=result["embedding"],
                    text=result["chunk"],
                    metadata=metadata,
                )
                new_doc.append(doc)

    return new_doc


async def context_retriever(
    query_str, tables_list, context: QueryPipelineContext
) -> BaseRetriever:
    """
    Initializes a context retriever based on documents extracted from AI
    Search.

    The similarity search is configured to return the top 3 similar items.

    Returns:
        An instance of a retriever object configured to perform similarity
        searches on the created VectorStoreIndex.
    """
    context.logger.info("I moved on to execute the context retriever function component, which helps in understanding the surrounding context of the data")
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
    
    #Question that contains the team type id
    original_query = query_str
    #Question without the team type id
    query_str = query_str[: query_str.find("The team type id")]

    embedding = context.llm_models.embed_model.get_text_embedding(query_str)

    vector_query = VectorizedQuery(
        vector=embedding, k_nearest_neighbors=3, fields="embedding"
    )

    main_table = tables_list[0]
    ai_search = AISearch(context.llm_models)
    await ai_search.async_set_client(index_name, aisearch_instance_name)

    prj_intention = "prjgloss"
    async with ai_search.async_client as client:
        new_doc = []
        doc_static = await search_by_tag(client,query_str,vector_query,10,new_doc,"static",main_table, prj_intention)
        doc_dinamyc = await search_by_tag(client,query_str,vector_query,10,doc_static,"dynamic",main_table, prj_intention)
        filter_ai_search = f""
        for i in tables_list:
            filter_ai_search += f"search.ismatch('{i}','metadata/{prj_intention}/tableName') or "

        results_schema = await client.search(
            filter=f"indexType eq '{prj_intention}' and metadata/{prj_intention}/isSchema eq 'True' and (" + filter_ai_search + f"search.ismatch('TeamTypes','metadata/{prj_intention}/tableName'))"
        )

        schema = ""
        workplan_context = """Apply task type filters based on the user question. Use "WP.WorkPlanTaskType = 'Task'" for tasks, "WP.WorkPlanTaskType = 'Milestone'" for milestones.""" if main_table == "WorkPlan" else ""
        team_type_id = 0
        async for result in results_schema:
            table_schema = str(result["chunk"])
            result_str = table_schema + " "
            schema = schema+ result_str
            if main_table in table_schema and " ProjectTeamId" in table_schema:
                team_type_id = 1

            if "TeamTypes" in table_schema:
                team_type_schema = result_str

        if team_type_id:
            schema = schema + "Important: When joining with the Project Teams table, also join with the Team Types table and filter using TeamTypes.ID. ONLY filter by project team, if it is mention in the user question"
            user_question = original_query
        else:
            schema = schema.replace(team_type_schema,"")
            user_question = query_str

        schema = schema + workplan_context

    index = VectorStoreIndex.from_documents(
        doc_dinamyc, embed_model=context.llm_models.embed_model
    )
    retriever = index.as_retriever(similarity_top_k=10)
    nodes = retriever.retrieve(query_str)
    return nodes, schema, user_question
