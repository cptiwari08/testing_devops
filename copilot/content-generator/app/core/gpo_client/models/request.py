"""
Request models for the GPO Core API client.

This module defines the request models used to interact with the GPO Core API.
"""

from typing import Dict, List, Optional, Any, Union
from pydantic import BaseModel, Field

class ChatRequest(BaseModel):
    """
    Request model for chat operations.
    
    This model represents a chat request to the GPO Core API.
    """
    
    query: str = Field(..., description="User's query text")
    response_type: Optional[str] = Field("LLM", description="Type of response expected (LLM, Question, QuestionAnswering)")
    is_sensitive: Optional[bool] = Field(False, description="Whether the query contains sensitive data")
    options: Optional[Dict[str, Any]] = Field(None, description="Additional options for the request")
    
    class Config:
        title = "Chat Request"
        allow_population_by_field_name = True
        
class ChatSensitiveRequest(ChatRequest):
    """
    Request model for sensitive chat operations.
    
    This model extends the base chat request with fields specific to sensitive data handling.
    """
    
    chat_history: Optional[List[Dict[str, Any]]] = Field(None, description="Previous chat history")
    sensitivity_level: Optional[str] = Field("medium", description="Level of sensitivity (low, medium, high)")
    compliance_rules: Optional[List[str]] = Field(None, description="Specific compliance rules to enforce")
    
    class Config:
        title = "Sensitive Chat Request"
        allow_population_by_field_name = True

class DocumentSearchRequest(BaseModel):
    """
    Request model for document search operations.
    
    This model represents a document search request.
    """
    
    search_text: Optional[str] = Field(None, description="Text to search for in documents")
    page_number: int = Field(1, description="Page number for pagination")
    page_size: int = Field(20, description="Number of items per page")
    sort_column: Optional[str] = Field(None, description="Column to sort by")
    sort_order: str = Field("Ascending", description="Sort direction (Ascending or Descending)")
    filters: Optional[Dict[str, Any]] = Field(None, description="Filters to apply to the search")
    
    class Config:
        title = "Document Search Request"
        allow_population_by_field_name = True
        
class DocumentQueryRequest(BaseModel):
    """
    Request model for natural language document queries.
    
    This model is used for semantic document searches using natural language.
    """
    
    query: str = Field(..., description="Natural language query to find documents")
    top_k: Optional[int] = Field(5, description="Maximum number of documents to return")
    filters: Optional[Dict[str, Any]] = Field(None, description="Filters to apply to the search")
    retrieve_content: Optional[bool] = Field(False, description="Whether to retrieve full document content")
    
    class Config:
        title = "Document Query Request"
        allow_population_by_field_name = True
        
class ConversationRequest(BaseModel):
    """
    Request model for creating or updating conversations.
    
    This model represents a request to create or update a conversation.
    """
    
    title: Optional[str] = Field(None, description="Title of the conversation")
    metadata: Optional[Dict[str, Any]] = Field(None, description="Additional metadata")
    
    class Config:
        title = "Conversation Request"
        allow_population_by_field_name = True

class FeedbackRequest(BaseModel):
    """
    Request model for chat feedback operations.
    """
    message_guid: str = Field(..., description="Unique identifier for the message")
    like: Optional[bool] = Field(None, description="Whether the message was liked")
    feedback: Optional[str] = Field(None, description="Optional feedback text")
    handoff: Optional[bool] = Field(False, description="Whether this is a handoff feedback")
    suggest_next_response: Optional[bool] = Field(False, description="Suggest next response flag")

    class Config:
        title = "Feedback Request"
        allow_population_by_field_name = True

class HandoffFeedbackRequest(BaseModel):
    """
    Request model for handoff feedback operations.
    """
    conversation_reference_id: str = Field(..., description="Conversation reference ID")
    is_clarified: bool = Field(..., description="Whether the handoff was clarified")

    class Config:
        title = "Handoff Feedback Request"
        allow_population_by_field_name = True

class RatingRequest(BaseModel):
    """
    Request model for submitting a rating.
    """
    conversation_reference_id: str = Field(..., description="Conversation reference ID")
    rating: int = Field(..., description="Rating value (e.g., 1-5)")
    rating_comment: Optional[str] = Field(None, description="Optional comment for the rating")

    class Config:
        title = "Rating Request"
        allow_population_by_field_name = True

class RatingsFilterRequest(BaseModel):
    """
    Request model for filtering ratings.
    """
    search_text: Optional[str] = Field(None, description="Text to search in ratings")
    sort_order: Optional[str] = Field("Ascending", description="Sort order")
    sort_column: Optional[str] = Field(None, description="Sort column")
    page_number: Optional[int] = Field(1, description="Page number")
    page_size: Optional[int] = Field(20, description="Page size")
    rating_values: Optional[List[int]] = Field(None, description="List of rating values to filter")
    is_comment_not_provided: Optional[bool] = Field(None, description="Filter for missing comments")

    class Config:
        title = "Ratings Filter Request"
        allow_population_by_field_name = True

class MetricsRequest(BaseModel):
    """
    Request model for metrics endpoints (generic).
    """
    filter: Optional[Dict[str, Any]] = Field(None, description="Filter for metrics query")
    question_details_filters: Optional[Dict[str, Any]] = Field(None, description="Question details filters")
    search_text: Optional[str] = Field(None, description="Optional search text")
    time_zone: Optional[str] = Field(None, description="Time zone for metrics")

    class Config:
        title = "Metrics Request"
        allow_population_by_field_name = True