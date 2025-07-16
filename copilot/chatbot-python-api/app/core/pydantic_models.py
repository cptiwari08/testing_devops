"""
This files use camel case instead of snake case
for atributes to keep standard JSON payloads
with the orchestrator
"""

from typing import Any, Dict, List, Literal, Optional, Union

from pydantic import UUID4, BaseModel, Field


class Page(BaseModel):
    pageNumber: int = Field(..., description="The page number of the document")
    metaData: Optional[Any] = Field(
        None, description="Additional metadata for the page, if any"
    )


class ChatMessage(BaseModel):
    messageId: int = Field(
        None, description="The unique identifier for the chat message"
    )
    role: Literal["user", "assistant", "system"] = Field(
        ...,
        description="The role of the entity sending the message (e.g., 'user' or 'assistant')",
    )
    content: str = Field(..., description="The content of the chat message")


class ChatHistory(BaseModel):
    chatHistory: Optional[List[ChatMessage]] = Field(
        None, description="The chat history between the user and the assistant"
    )


class User(BaseModel):
    email: str | None = Field(..., description="email")
    familyName: str | None = Field(None, description="Family Name")
    givenName: str | None = Field(None, description="Given Name")


class Suggestion(BaseModel):
    id: str = Field(None, description="Suggestion ID")
    sqlQuery: str | None = Field(None, description="Suggestion SQL Query")
    source: str = Field(None, description="Backend Source")

class AppInfo(BaseModel):
    key: str = Field(default= "PROJECT_MANAGEMENT", description="Key name app")
    name: str = Field(default= "PMO", description="App name")
    teamTypeIds: List[Any] = Field(default=[1], description="Team Type IDs")


class Context(BaseModel):
    projectDescription: Optional[str] = Field(
        "", description="Optional project description"
    )
    suggestion: Optional[Suggestion] = Field(None, description="Suggestion")
    user: Optional[User] = Field(..., description="User")
    documents: Optional[List[UUID4]] = Field(
        None, description="Document IDs list that an user has permissions to query"
    )
    appInfo: Optional[AppInfo] = Field(AppInfo(), description="App info")


class MessageRequest(BaseModel):
    chatId: str = Field(..., description="The unique identifier for the conversation")
    instanceId: str = Field(
        ..., description="The unique instance identifier for the orchestrator call"
    )
    projectFriendlyId: str = Field(None, description="Project Friendly ID")
    question: str = Field(..., description="Optional text prompt submitted by the user")
    chatHistory: Optional[List[ChatMessage]] = Field(
        None, description="An optional list of chat history messages"
    )
    context: Context = Field(
        ..., description="An optional, structured context object for the conversation"
    )


class AgentChatResponse(BaseModel):
    response: str
    sources: List
    source_nodes: List  # not camel case because this is returned by llama-index


class CitingSources(BaseModel):
    sourceName: str = Field(..., description="Source name")
    sourceType: str = Field(..., description="Source type")
    sourceValue: List[Any] = Field(..., description="Source value")


class SQLResponseData(BaseModel):
    query: str = Field(None, description="SQL Query")
    result: str = Field(None, description="SQL Result")


class Score(BaseModel):
    metric: str
    value: float
    reason: str

class FollowUpSuggestions(BaseModel):
    id: int = Field(None, description="Suggestion ID")
    suggestionText: str = Field(None, description="Suggestion Text")


class MessageResponse(BaseModel):
    backend: str = Field(..., description="Backend name")
    sql: SQLResponseData = Field(None, description="SQL Query and Result")
    response: str = Field(..., description="Backend answer")
    score: Optional[Score] = Field(None, description="Score calculated")
    citingSources: Optional[List[CitingSources]] = Field(
        None, description="Citing sources"
    )
    followUpSuggestions: Optional[List[FollowUpSuggestions]] = Field(
        None, description="Follow up suggestions"
    )
    rawResponse: Optional[Union[Dict, List, AgentChatResponse]] = Field(
        None, description="Optional raw response from llama index"
    )


class PromptTemplate(BaseModel):
    text: str = Field(..., description="Prompt data")
    few_shots: Optional[list[str]] = Field(None, description="Few shots data")


class MSServerConfig(BaseModel):
    tenant_id: str | None
    client_id: str | None
    client_secret: str | None
    server: str | None
    database: str | None
    scopes: list[str] = ["https://database.windows.net//.default"]



class MetricInput(BaseModel):
    user_input: str
    llm_response: str
    retrieved_context: Optional[List[str]] = None
    threshold: float = 0.5
    include_reason: bool = False
    project_description: Optional[str] = None


class LLMModels(BaseModel):
    embed_model: Any
    llm: Any


class QueryPipelineContext(BaseModel):
    # type hint as Any, because logging and IBaseLogger are producing issues
    logger: Any
    token: str
    message_request: Optional[MessageRequest]
    llm_models: LLMModels
    callback_handlers: Optional[Any] = None
