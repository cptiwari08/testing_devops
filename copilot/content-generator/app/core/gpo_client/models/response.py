"""
Response models for the GPO Core API client.

This module defines the response models that represent data returned from the GPO Core API.
"""

import time
from typing import Dict, List, Optional, Any, Union
from dataclasses import dataclass, field
from datetime import datetime
from pydantic import BaseModel, Field


@dataclass
class SessionResponse:
    """
    Response from the session creation endpoint /api/User/session.
    
    This model contains session information and user details.
    According to API documentation, the actual response includes:
    - isSuccess: Whether the session creation was successful
    - userRoles: Roles assigned to the user
    - guid: Unique identifier for the user/session
    - emailId: User's email address
    - displayName: User's displayed name
    """
    
    session_id: str
    """Unique identifier for the session (mapped from 'guid' in API response)."""
    
    token: str
    """Authentication token for subsequent API calls."""
    
    expiration: Optional[datetime] = None
    """Optional expiration timestamp for the token."""
    
    user_info: Optional[Dict[str, Any]] = None
    """User information including isSuccess, userRoles, emailId, and displayName."""
    
    def __post_init__(self):
        """
        Validate required fields after initialization.
        """
        if not self.session_id:
            raise ValueError("session_id is required")
        if not isinstance(self.user_info, dict) and self.user_info is not None:
            raise TypeError("user_info must be a dictionary or None")


@dataclass
class ConversationResponse:
    """
    Response from conversation creation or update.
    
    This model contains information about a created or updated conversation.
    The API returns 'conversationReferenceId' and 'welcomeTextMessage' fields.
    """
    
    conversationReferenceId: str
    """Unique identifier for the conversation (returned as 'conversationReferenceId' by API)."""
    
    welcomeTextMessage: Optional[str] = None
    """Welcome message for the conversation."""
    
    created_at: Optional[Union[str, datetime]] = None
    """Timestamp when the conversation was created (may not be returned by API)."""
    
    title: Optional[str] = None
    """Optional title for the conversation (may not be returned by API)."""

    @property
    def conversation_id(self) -> str:
        """
        Compatibility property to access the conversation ID.
        
        Returns:
            str: The conversation reference ID.
        """
        return self.conversationReferenceId


@dataclass
class ChatResponse:
    """
    Response from a chat message via /api/Chat endpoint.
    
    This model contains the AI's response to a user query.
    According to the API documentation and actual responses, the structure includes:
    - isSuccess: Whether the request was successful
    - messages: List of message objects containing the actual response content
    - status: Status code
    - code: Additional code information
    - message: Optional message string
    
    Within the messages array, each message contains:
    - conversationGuid: ID of the conversation
    - messageGuid: ID of the message
    - messageText: The actual response content
    - user: User information
    - documents: Referenced documents
    """
    
    # Required fields
    message_content: str
    """Text response to the user's query (extracted from messages[0].messageText)."""
    
    conversation_id: str
    """Conversation identifier this message belongs to (extracted from messages[0].conversationGuid)."""
    
    # Optional fields
    message_id: Optional[str] = None
    """Unique identifier for the message (extracted from messages[0].messageGuid)."""
    
    is_success: Optional[bool] = None
    """Whether the API request was successful."""
    
    created_at: Optional[Union[str, datetime]] = None
    """Timestamp when the message was created (extracted from messages[0].messageCreatedDate)."""
    
    documents: List[Any] = field(default_factory=list)
    """Referenced documents in the response (extracted from messages[0].documents)."""
    
    status_code: Optional[int] = None
    """Status code from the API response."""
    
    raw_messages: List[Dict[str, Any]] = field(default_factory=list)
    """The complete messages array from the API response for advanced processing."""
    
    metadata: Dict[str, Any] = field(default_factory=dict)
    """Additional metadata about the response."""
    
    def __post_init__(self):
        """
        Validate and adjust fields after initialization.
        """
        # Ensure we have a valid conversation_id
        if not self.conversation_id:
            raise ValueError("conversation_id is required")
        # Ensure we have content
        if not self.message_content:
            raise ValueError("message_content is required")
    
    @classmethod
    def from_api_response(cls, response: Dict[str, Any]) -> "ChatResponse":
        """
        Create a ChatResponse from a raw API response.
        
        Args:
            response: The raw API response dictionary
            
        Returns:
            ChatResponse: A properly mapped ChatResponse object
            
        Raises:
            ValueError: If the response doesn't contain required data
        """
        # Extract required data from the response
        is_success = response.get("isSuccess", False)
        messages = response.get("messages", [])
        status_code = response.get("status", 0)
        
        if not messages:
            raise ValueError("API response doesn't contain any messages")
        
        # Extract data from the first message (most common case)
        first_msg = messages[0]
        
        # Create and return the ChatResponse
        return cls(
            message_content=first_msg.get("messageText", ""),
            conversation_id=first_msg.get("conversationGuid", ""),
            message_id=first_msg.get("messageGuid"),
            is_success=is_success,
            created_at=first_msg.get("messageCreatedDate"),
            documents=first_msg.get("documents", []),
            status_code=status_code,
            raw_messages=messages,
            metadata={
                "code": response.get("code"),
                "message": response.get("message"),
                "user": first_msg.get("user", {}),
                "additionalInfo": first_msg.get("additionalInfo"),
                "score": first_msg.get("score"),
            }
        )


@dataclass
class SensitiveChatResponse(ChatResponse):
    """
    Response from a sensitive chat message.
    
    This model extends the chat response with sensitive data handling information.
    """
    
    sensitive_content_detected: bool = False
    """Whether sensitive content was detected in the query."""
    
    compliance_status: Dict[str, Any] = field(default_factory=dict)
    """Compliance status information."""


@dataclass
class Document:
    """
    Document metadata.
    
    This model represents metadata about a document in the system.
    """
    
    document_guid: str
    """Unique identifier for the document."""
    
    title: str
    """Title of the document."""
    
    filename: str
    """Filename of the document."""
    
    file_type: str
    """Type of the document (PDF, DOCX, etc.)."""
    
    size: int
    """Size of the document in bytes."""
    
    created_at: Union[str, datetime]
    """Timestamp when the document was created."""
    
    modified_at: Union[str, datetime]
    """Timestamp when the document was last modified."""
    
    author: Optional[str] = None
    """Optional author of the document."""
    
    categories: List[str] = field(default_factory=list)
    """Categories assigned to the document."""
    
    tags: List[str] = field(default_factory=list)
    """Tags assigned to the document."""
    
    metadata: Dict[str, Any] = field(default_factory=dict)
    """Additional metadata about the document."""
    
    summary: Optional[str] = None
    """Optional summary of the document content."""


@dataclass
class DocumentContent:
    """
    Document content with extracted text only.
    This model contains only the extracted text from the PDF document.
    """
    document: str  # The extracted text content from the PDF
    """Extracted text content from the PDF document."""

    # Optionally, you can keep metadata fields if needed, but not the bytes or content_type
    # Add more fields here if you want to keep document metadata (e.g., title, guid, etc.)


@dataclass
class DocumentSearchResult:
    """
    Result of a document search from /api/Documents/managed.
    
    This model contains pagination information and document results.
    According to the API documentation, the response includes 'items'
    containing document metadata, along with pagination details.
    """
    
    items: List[Document]
    """List of documents matching the search criteria."""
    
    total_count: int
    """Total number of matching documents."""
    
    page_number: int
    """Current page number."""
    
    page_size: int
    """Number of items per page."""
    
    def __post_init__(self):
        """
        Validate required fields after initialization.
        """
        if not isinstance(self.items, list):
            raise TypeError("items must be a list of Document objects")


@dataclass
class DocumentQueryResponse:
    """
    Response for a document query.
    
    This model contains documents matching a natural language query.
    """
    
    query: str
    """The original query text."""
    
    documents: List[Dict[str, Any]]
    """List of documents with their content."""
    
    search_time_ms: int = field(default_factory=lambda: int(time.time() * 1000))
    """Time taken to perform the search in milliseconds."""


@dataclass
class Message:
    """
    Chat message.
    
    This model represents a message in a conversation.
    According to the API, message data may include query text, response,
    creation timestamp, and reference to source documents.
    """
    
    message_id: str
    """Unique identifier for the message."""
    
    content: str
    """Text content of the message."""
    
    created_at: Union[str, datetime]
    """Timestamp when the message was created."""
    
    is_user_message: bool
    """Whether the message was sent by the user."""
    
    conversation_id: str
    """Conversation identifier this message belongs to (may be called 'conversationReferenceId' in API)."""
    
    documents: List[Any] = field(default_factory=list)
    """Documents referenced in the message."""
    
    metadata: Dict[str, Any] = field(default_factory=dict)
    """Additional metadata about the message."""
    
    def __post_init__(self):
        """
        Validate required fields after initialization.
        """
        # Convert string ID to UUID if needed
        if not self.message_id:
            raise ValueError("message_id is required")
        if not self.conversation_id:
            raise ValueError("conversation_id is required")


@dataclass
class ConversationHistory:
    """
    Conversation history.
    
    This model represents conversation history information.
    """
    
    conversation_id: str
    """Unique identifier for the conversation."""
    
    title: str
    """Title of the conversation."""
    
    created_at: Union[str, datetime]
    """Timestamp when the conversation was created."""
    
    last_message_at: Union[str, datetime]
    """Timestamp when the last message was sent."""
    
    message_count: int
    """Number of messages in the conversation."""
    
    messages: List[Message] = field(default_factory=list)
    """Messages in the conversation."""


@dataclass
class StreamingChunk:
    """
    Streaming response chunk.
    
    This model represents a chunk of streaming response from the API.
    """
    
    content: str
    """Text content in this chunk."""
    
    is_complete: bool
    """Whether this is the final chunk."""
    
    metadata: Dict[str, Any] = field(default_factory=dict)
    """Additional metadata about the chunk."""


@dataclass
class ChatHistoryMessage:
    """
    Chat history message returned by the API.
    
    This model represents the actual message structure returned by the API's chat history endpoint.
    It maps directly to the response items in clientChatHistories.
    """
    
    messageGuid: str
    """Unique identifier for the message."""
    
    messageText: str
    """Text content of the message."""
    
    messageCreatedDate: str
    """Timestamp when the message was created."""
    
    messageType: int
    """Message type (1 = bot, 2 = user)."""
    
    user: Dict[str, Any]
    """User information associated with the message."""
    
    additionalInfo: Optional[str] = None
    """Additional information about the message, often contains JSON data."""
    
    isMessageLiked: Optional[bool] = None
    """Whether the message is liked."""
    
    isAnnotated: Optional[bool] = None
    """Whether the message is annotated."""
    
    conversationGuid: Optional[str] = None
    """Conversation identifier this message belongs to."""
    
    documents: List[Any] = field(default_factory=list)
    """Documents referenced in the message."""
    
    @property
    def is_user_message(self) -> bool:
        """
        Check if this is a user message based on the messageType.
        
        Returns:
            bool: True if this is a user message (messageType = 2), False otherwise.
        """
        return self.messageType == 2