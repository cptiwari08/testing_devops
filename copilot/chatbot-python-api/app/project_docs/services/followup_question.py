import json

from app.core.config import MMRConfig
from app.core.prompt_manager import create_prompt_manager
from llama_index.core import Document, VectorStoreIndex
from llama_index.core.chat_engine import SimpleChatEngine


async def generate_followup_questions(
    question, llm_models, chat_history, data_chunks
) -> list:
    """
    Generates follow-up questions based on the provided question and data chunks.
    
    Args:
        question: The original user question
        llm_models: Models for generating embeddings and LLM responses
        chat_history: History of the chat for context
        data_chunks: Retrieved data chunks related to the question
        
    Returns:
        list: Generated follow-up questions, empty list if errors occur
    """
    try:
        if len(data_chunks) == 0:
            return []
            
        # Initialize prompt manager
        prompt_manager = create_prompt_manager()
        
        # Get prompt using the async prompt manager method
        followup_question_prompt = await prompt_manager.get_prompt(
            agent="project_docs",
            key="followup_question",
            prompt_parameters={
                "user_prompt": question,
                "context": data_chunks,
            }
        )

        chat_engine_options = {
            "llm": llm_models.llm,
            "chat_history": chat_history,
        }
        chat_engine_options |= {"system_prompt": followup_question_prompt}
        chat_engine = SimpleChatEngine.from_defaults(**chat_engine_options)

        payload = {"message": ""}

        raw_response_ = await chat_engine.achat(**payload)
        generated_follow_up_questions = json.loads(raw_response_.response)

        question_list = []

        for i, question in enumerate(generated_follow_up_questions):
            doc = Document(
                doc_id=str(i),
                embedding=llm_models.embed_model.get_text_embedding(question),
                text=question,
            )
            question_list.append(doc)

        index = VectorStoreIndex.from_documents(
            question_list, embed_model=llm_models.embed_model
        )
        retriever = index.as_retriever(
            vector_store_query_mode="mmr",
            similarity_top_k=int(MMRConfig.mmr_num_questions),
            vector_store_kwargs={"mmr_threshold": float(MMRConfig.mmr_threshold)},
        )
        nodes = retriever.retrieve(question)

        followup_questions = [{"id": 0, "suggestionText": node.text} for node in nodes]
        return followup_questions
    except Exception:
        # No matter what happens the followup suggestion
        # should never break the main flow, that is why
        # we're doing the try except for the complete function
        return []
