"""
Context models for GPO Core API client.

This module contains context models used for sessions, authentication and chat operations.
"""

from typing import Dict, List, Optional, Any, Union
from dataclasses import dataclass, field
from datetime import datetime
import uuid


@dataclass
class SessionContext:
    """
    Session context for creating a session with the API.
    
    This context is used when initiating a session with the GPO Core API.
    """
    
    instance_id: str
    """A unique identifier for the client instance."""
    
    user_id: str
    """The user's identifier (could be email or system-assigned ID)."""
    
    project_id: Optional[str] = None
    """Optional project identifier."""
    
    session_start_time: datetime = field(default_factory=datetime.now)
    """The time when the session started."""
    
    properties: Dict[str, Any] = field(default_factory=dict)
    """Additional properties to send with session creation."""
    
    def dict(self) -> Dict[str, Any]:
        """Convert the context to a dictionary for API communication."""
        result = {
            "instanceId": self.instance_id,
            "userId": self.user_id,
            "sessionStartTime": self.session_start_time.isoformat(),
        }
        
        if self.project_id:
            result["projectId"] = self.project_id
            
        if self.properties:
            result.update(self.properties)
            
        return result


@dataclass
class ChatContext:
    """
    Context for chat operations.
    
    This context is used for all chat-related operations with the GPO Core API.
    It holds the session ID and any conversation-specific identifiers.
    """
    
    session_id: Optional[str] = None
    """Session identifier from a previous session creation."""
    
    token: Optional[str] = None
    """Authentication token for API requests."""
    
    conversation_id: Optional[str] = None
    """Conversation identifier for ongoing conversations."""
    
    user_id: Optional[str] = None
    """The user's identifier for this chat context."""
    
    document_guids: List[str] = field(default_factory=list)
    """List of document GUIDs for context-aware operations."""
    
    def dict(self) -> Dict[str, Any]:
        """Convert the context to a dictionary for API communication."""
        result = {}
        
        if self.session_id:
            result["sessionId"] = self.session_id
            
        if self.conversation_id:
            result["conversationId"] = self.conversation_id
            
        if self.user_id:
            result["userId"] = self.user_id
            
        if self.document_guids:
            result["documentGuids"] = self.document_guids
            
        return result