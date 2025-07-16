"""Session models for the GPO Core Python Client.

This module defines session-related response models for the GPO Core API.
"""

from datetime import datetime
from typing import Optional, List, Dict, Any
from pydantic import BaseModel, Field


class SessionResponse(BaseModel):
    """Response from creating a new session.
    
    This class represents the response received when creating a new session
    with the GPO Core API.
    """
    
    session_id: str = Field(
        ..., 
        description="The unique identifier for the session"
    )
    token: str = Field(
        ..., 
        description="The authentication token for the session"
    )
    expires_at: datetime = Field(
        ..., 
        description="The expiration timestamp for the session"
    )
    user_id: str = Field(
        ..., 
        description="The user ID associated with this session"
    )
    instance_id: str = Field(
        ..., 
        description="The instance ID associated with this session"
    )
    project_id: str = Field(
        ..., 
        description="The project ID associated with this session"
    )
    features: Optional[Dict[str, Any]] = Field(
        None, 
        description="Optional feature flags or settings for this session"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "allow"  # Allow additional fields for forward compatibility


class Conversation(BaseModel):
    """Information about a conversation.
    
    This class represents information about a conversation in the GPO Core API.
    """
    
    conversation_id: str = Field(
        ..., 
        description="The unique identifier for the conversation"
    )
    title: str = Field(
        ..., 
        description="The title of the conversation"
    )
    created_at: datetime = Field(
        ..., 
        description="When the conversation was created"
    )
    updated_at: datetime = Field(
        ..., 
        description="When the conversation was last updated"
    )
    message_count: int = Field(
        ...,
        description="The number of messages in the conversation"
    )
    metadata: Optional[Dict[str, Any]] = Field(
        None,
        description="Optional metadata associated with the conversation"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "allow"  # Allow additional fields for forward compatibility


class ConversationResponse(BaseModel):
    """Response from retrieving conversations.
    
    This class represents the response received when retrieving a list of
    conversations from the GPO Core API.
    """
    
    conversations: List[Conversation] = Field(
        ..., 
        description="List of conversations"
    )
    total_count: int = Field(
        ..., 
        description="Total number of conversations available"
    )
    page: int = Field(
        ..., 
        description="Current page number"
    )
    page_size: int = Field(
        ..., 
        description="Number of items per page"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "allow"  # Allow additional fields for forward compatibility