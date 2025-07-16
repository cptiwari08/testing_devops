"""Chat models for the GPO Core Python Client.

This module defines chat-related request and response models for the GPO Core API.
"""

from datetime import datetime
from enum import Enum
from typing import Optional, List, Dict, Any, Union
from pydantic import BaseModel, Field, root_validator


class Source(BaseModel):
    """Information about a source document referenced in a chat response.
    
    This class represents metadata about a source document used to generate
    a response in the GPO Core API.
    """
    
    id: str = Field(
        ..., 
        description="The unique identifier for the source"
    )
    title: str = Field(
        ..., 
        description="The title of the source document"
    )
    url: Optional[str] = Field(
        None, 
        description="Optional URL where the source can be accessed"
    )
    content_snippet: Optional[str] = Field(
        None, 
        description="Optional snippet of the source content"
    )
    metadata: Optional[Dict[str, Any]] = Field(
        None,
        description="Additional metadata about the source"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "allow"  # Allow additional fields for forward compatibility


class MessageRole(str, Enum):
    """Enumeration of possible message roles in a conversation."""
    
    SYSTEM = "system"
    USER = "user"
    ASSISTANT = "assistant"


class Message(BaseModel):
    """A message in a conversation.
    
    This class represents a message sent by either the user or the assistant.
    """
    
    role: MessageRole = Field(
        ...,
        description="The role of the message sender (system, user, or assistant)"
    )
    content: str = Field(
        ...,
        description="The content of the message"
    )
    message_id: Optional[str] = Field(
        None,
        description="The unique identifier for the message"
    )
    created_at: Optional[datetime] = Field(
        None,
        description="When the message was created"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        use_enum_values = True
        extra = "allow"  # Allow additional fields for forward compatibility


class ConversationRequest(BaseModel):
    """Request parameters for retrieving conversations.
    
    This class represents the request parameters for retrieving a list of
    conversations from the GPO Core API.
    """
    
    page: Optional[int] = Field(
        1,
        description="Page number to retrieve (1-indexed)",
        ge=1
    )
    page_size: Optional[int] = Field(
        10,
        description="Number of items per page",
        ge=1,
        le=100
    )
    filter: Optional[Dict[str, Any]] = Field(
        None,
        description="Optional filter criteria for conversations"
    )
    sort: Optional[str] = Field(
        "updated_at:desc",
        description="Sort order, in the format 'field:direction'"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "forbid"


class ChatRequest(BaseModel):
    """Request parameters for a chat completion.
    
    This class represents the request parameters for a chat completion
    operation in the GPO Core API.
    """
    
    messages: List[Message] = Field(
        ...,
        description="The messages to process, in chronological order"
    )
    temperature: Optional[float] = Field(
        0.7,
        description="Sampling temperature to use (0.0 to 1.0)",
        ge=0.0,
        le=1.0
    )
    max_tokens: Optional[int] = Field(
        None,
        description="Maximum number of tokens to generate",
        gt=0
    )
    include_sources: Optional[bool] = Field(
        True,
        description="Whether to include sources in the response"
    )
    search_options: Optional[Dict[str, Any]] = Field(
        None,
        description="Additional options for knowledge search"
    )
    
    @root_validator
    def validate_messages(cls, values: Dict[str, Any]) -> Dict[str, Any]:
        """Validate the messages list.
        
        Args:
            values: The values to validate
            
        Returns:
            The validated values
            
        Raises:
            ValueError: If the messages list is empty or invalid
        """
        messages = values.get("messages", [])
        if not messages:
            raise ValueError("At least one message is required")
        
        # Ensure the last message is from the user
        if messages[-1].role != MessageRole.USER:
            raise ValueError("The last message must be from the user")
        
        return values
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "forbid"


class StreamingChatRequest(ChatRequest):
    """Request parameters for a streaming chat completion.
    
    This class extends ChatRequest with streaming-specific parameters.
    """
    
    stream: bool = Field(
        True,
        description="Whether to stream the response (must be True for streaming)"
    )
    chunk_size: Optional[int] = Field(
        None,
        description="Size of chunks to return in the stream",
        gt=0
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "forbid"


class ChatResponseFormat(str, Enum):
    """Enumeration of possible chat response formats."""
    
    TEXT = "text"
    JSON = "json"


class ChatResponse(BaseModel):
    """Response from a chat completion.
    
    This class represents the response received when requesting a chat
    completion from the GPO Core API.
    """
    
    message: Message = Field(
        ...,
        description="The generated message"
    )
    sources: Optional[List[Source]] = Field(
        None,
        description="Sources referenced in the response"
    )
    conversation_id: str = Field(
        ...,
        description="The ID of the conversation"
    )
    created_at: datetime = Field(
        ...,
        description="When the response was created"
    )
    usage: Optional[Dict[str, int]] = Field(
        None,
        description="Token usage information"
    )
    finish_reason: Optional[str] = Field(
        None,
        description="The reason the generation finished"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "allow"  # Allow additional fields for forward compatibility


class ChatResponseChunk(BaseModel):
    """Chunk of a streaming chat response.
    
    This class represents a chunk of a streaming response received when
    requesting a chat completion from the GPO Core API.
    """
    
    delta: str = Field(
        ...,
        description="The text delta in this chunk"
    )
    conversation_id: str = Field(
        ...,
        description="The ID of the conversation"
    )
    is_final: bool = Field(
        False,
        description="Whether this is the final chunk"
    )
    sources: Optional[List[Source]] = Field(
        None,
        description="Sources referenced in the response (only in final chunk)"
    )
    finish_reason: Optional[str] = Field(
        None,
        description="The reason the generation finished (only in final chunk)"
    )
    usage: Optional[Dict[str, int]] = Field(
        None,
        description="Token usage information (only in final chunk)"
    )
    
    class Config:
        """Pydantic configuration."""
        validate_assignment = True
        extra = "allow"  # Allow additional fields for forward compatibility