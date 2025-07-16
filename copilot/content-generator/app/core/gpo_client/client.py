"""
GPO Core API Client

This module provides the main client for interacting with the GPO Core API.
"""
import io
import os
from PyPDF2 import PdfReader

import asyncio
import logging
import uuid
import json
from typing import Dict, List, Optional, Any, Union

from .models.response import Message, Document
from app.core.config import GPOConfig
from .http_client import HttpClient
from .auth import AuthHandler
from .exceptions import (
    AuthenticationError, 
    ConnectionError,
    DocumentAccessError,
    DocumentNotFoundError,
    GPOClientException,
    InvalidRequestError,
    RateLimitError,
    ServiceUnavailableError,
    TimeoutError
)
from .models.context import SessionContext, ChatContext
from .models.request import (
    ChatRequest, 
    ChatSensitiveRequest,
    FeedbackRequest,
    HandoffFeedbackRequest,
    RatingRequest,
    RatingsFilterRequest,
    MetricsRequest
)
from .models.response import (
    SessionResponse, 
    ConversationResponse,
    ChatResponse,
    SensitiveChatResponse, 
    Document,
    DocumentContent,
    DocumentSearchResult,
    DocumentQueryResponse,
    Message, 
    ConversationHistory,
    StreamingChunk
)

import logging
import time
from datetime import datetime

logger = logging.getLogger(__name__)

class GPOCoreClient:
    """
    Client for interacting with the GPO Core API.
    
    This client provides methods for all operations available in the API, with
    special focus on document management and retrieval capabilities.
    """
    
    def __init__(self, config: GPOConfig, api_key: Optional[str] = None):
        """
        Initialize the GPO Core API client.
        
        Args:
            config: Configuration for the client
            api_key: Optional API key that overrides the one from config
        """
        self.config = config
        
        # Override the API key if one was provided
        if api_key:
            self.config.api_key = api_key
            
        self.http_client = HttpClient(config)
        self.auth_handler = AuthHandler()
    
    async def close(self) -> None:
        """Close the client and release any resources."""
        await self.http_client._close_session()
    
    async def session(self, context: SessionContext) -> SessionResponse:
        """
        Create a new session with the API.
        
        Args:
            context: Session context with user information
            
        Returns:
            SessionResponse: Information about the created session
            
        Raises:
            AuthenticationError: If session creation fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
        """
        try:
            endpoint = "/api/User/session"
            
            # Add additional logging for troubleshooting
            logger.debug(f"Creating session with context: {context.dict()}")
            logger.debug(f"Using base URL: {self.config.base_url}")
            
            result = await self.http_client.request(
                "POST", 
                endpoint, 
                data=context.dict()
            )
            
            # Log response for debugging (remove sensitive info in production)
            logger.debug(f"Session API response keys: {result.keys() if isinstance(result, dict) else 'Not a dict'}")
            
            # Check for explicit error messages in the response
            if isinstance(result, dict) and result.get("isSuccess") == False:
                error_msg = result.get("message", "Unknown error")
                logger.error(f"Session creation failed: {error_msg}")
                raise AuthenticationError(f"Authentication failed: {error_msg}")
            
            # In this API, the authentication token is the API key itself
            # As per the rule: "API_KEY is my personal token of session, always"
            token = self.config.api_key
            
            # Map API response fields to SessionResponse fields
            # The guid from response will be the session_id
            mapped = {
                "session_id": result.get("guid", ""),
                "token": token,  # Use API key as token
                "expiration": None,  # Not provided by API
                "user_info": {
                    "isSuccess": result.get("isSuccess", False),
                    "userRoles": result.get("userRoles", []),
                    "emailId": result.get("emailId", ""),
                    "displayName": result.get("displayName", "")
                }
            }
            
            session_response = SessionResponse(**mapped)
            
            # Log successful session creation
            logger.info(f"Session created successfully for user: {result.get('emailId', 'Unknown')}")
            
            return session_response
            
        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error creating session: {str(e)}")
            raise ConnectionError(f"Failed to create session: {str(e)}")
    
    async def start_conversation(
        self, 
        context: ChatContext, 
        knowledge_assistant_tag: Optional[str] = None
    ) -> ConversationResponse:
        """
        Start a new conversation.
        
        Args:
            context: Chat context with session information
            knowledge_assistant_tag: Optional tag for the knowledge assistant
            
        Returns:
            ConversationResponse: Information about the created conversation
            
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
        """
        try:
            # Set up endpoint with optional query parameter
            endpoint = "/api/Chat/start-conversation"
            params = {}
            if knowledge_assistant_tag:
                params["knowledgeAssistantTag"] = knowledge_assistant_tag
            
            # API_KEY is the personal token of session, always
            token = self.config.api_key
            
            # Prepare auth header with the API key as token
            headers = self.auth_handler.prepare_auth_header(token)
            
            # Log for debugging (remove sensitive info in production)
            logger.debug(f"Starting conversation with auth header: {headers.keys()}")
            
            result = await self.http_client.request(
                "POST", 
                endpoint, 
                headers=headers,
                params=params
            )
            
            logger.info(f"Conversation started successfully, mapping result to ConversationResponse")
            
            # Log the actual API response keys for debugging (remove in production)
            logger.debug(f"API response keys: {result.keys() if isinstance(result, dict) else 'Not a dict'}")
            
            # Map the result to the ConversationResponse model
            # The API returns 'conversationReferenceId' rather than 'conversation_id'
            conversation_response = ConversationResponse(
                conversationReferenceId=result.get('conversationReferenceId', ''),
                welcomeTextMessage=result.get('welcomeTextMessage', None),
                created_at=datetime.now(),  # Use current time as fallback
                title=None  # Title not provided by this endpoint
            )
            
            return conversation_response
            
        except GPOClientException as e:
            logger.error(f"Error starting conversation: {str(e)}")
            raise
        except Exception as e:
            logger.error(f"Unexpected error starting conversation: {str(e)}")
            raise ConnectionError(f"Failed to start conversation: {str(e)}")
    
    async def chat(
        self, 
        context: ChatContext, 
        request: ChatRequest
    ) -> ChatResponse:
        """
        Send a chat message and get a response.
        
        Args:
            context: Chat context with session and conversation info
            request: Chat request with query
            
        Returns:
            ChatResponse: Response from the API
            
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
        """
        try:
            endpoint = "/api/Chat"
            
            # Validate required fields
            if not context.conversation_id:
                raise InvalidRequestError("conversation_id is required in context")
            
            # Prepare the request data
            data = {
                **request.dict(),
                "conversationReferenceId": context.conversation_id
            }
            
            # Use API key as the token for authentication
            # This follows the rule: "API_KEY is my personal token of session, always"
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            result = await self.http_client.request(
                "POST", 
                endpoint, 
                headers=headers,
                data=data
            )
            
            # Use the new factory method to create a ChatResponse from the API result
            chat_response = ChatResponse.from_api_response(result)
            logger.debug(f"Chat response created successfully with message ID: {chat_response.message_id}")
            
            return chat_response
            
        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error in chat operation: {str(e)}")
            raise ConnectionError(f"Chat operation failed: {str(e)}")
    
    async def sensitive_chat(
        self, 
        context: ChatContext, 
        request: ChatSensitiveRequest
    ) -> SensitiveChatResponse:
        """
        Send a sensitive chat message and get a response.
        
        Args:
            context: Chat context with session and conversation info
            request: Sensitive chat request with query and chat history
            
        Returns:
            SensitiveChatResponse: Response from the API
            
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
        """
        try:
            endpoint = "/api/Chat/SensitiveDataSupport"
            
            # Validate required fields
            if not context.conversation_id:
                raise InvalidRequestError("conversation_id is required in context")
            
            # Prepare the request data
            data = {
                **request.dict(),
                "conversationReferenceId": context.conversation_id,
                "isSensitiveInfo": True
            }
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            result = await self.http_client.request(
                "POST", 
                endpoint, 
                headers=headers,
                data=data
            )
            
            # Use the base ChatResponse factory method to handle the common API response fields
            chat_response = ChatResponse.from_api_response(result)
            
            # Convert to SensitiveChatResponse with additional sensitive-specific fields
            sensitive_response = SensitiveChatResponse(
                message_content=chat_response.message_content,
                conversation_id=chat_response.conversation_id,
                message_id=chat_response.message_id,
                is_success=chat_response.is_success,
                created_at=chat_response.created_at,
                documents=chat_response.documents,
                status_code=chat_response.status_code,
                raw_messages=chat_response.raw_messages,
                metadata=chat_response.metadata,
                # SensitiveChatResponse specific fields
                sensitive_content_detected=result.get("sensitiveContentDetected", False),
                compliance_status=result.get("complianceStatus", {})
            )
            
            return sensitive_response
            
        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error in sensitive chat operation: {str(e)}")
            raise ConnectionError(f"Sensitive chat operation failed: {str(e)}")
    
    async def get_chat_history(
        self, 
        context: ChatContext, 
        search_text: Optional[str] = None
    ) -> List[ConversationHistory]:
        """
        Get chat history for the authenticated user.
        
        Args:
            context: Chat context with session info
            search_text: Optional text to search in history
            
        Returns:
            List[ConversationHistory]: List of conversation history items
            
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
        """
        try:
            endpoint = "/api/Chat/history"
            params = {}
            if search_text:
                params["searchText"] = search_text
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            logger.debug(f"Requesting chat history with params: {params}")
            result = await self.http_client.request(
                "GET", 
                endpoint, 
                headers=headers,
                params=params
            )
            
            # Process the API response
            history_items = []
            
            if isinstance(result, dict) and result.get("isSuccess") and "clientChatHistories" in result:
                # Parse the raw API response into ChatHistoryMessage objects
                raw_chat_histories = result.get("clientChatHistories", [])
                logger.debug(f"Received {len(raw_chat_histories)} chat history items")
                
                # Organize messages by conversation
                # Since we don't have direct conversation_id in the API, we need to infer it
                
                # Strategy: Group messages by conversation based on timestamps and user patterns
                message_groups = self.group_messages_by_conversation(raw_chat_histories)
                
                # Convert each group into a ConversationHistory object
                for conv_id, messages in message_groups.items():
                    # Sort messages by timestamp
                    messages.sort(key=lambda m: m.get("messageCreatedDate", ""))
                    first_msg_time = messages[0].get("messageCreatedDate", datetime.now().isoformat())
                    last_msg_time = messages[-1].get("messageCreatedDate", datetime.now().isoformat())
                    
                    # Map messages to our internal Message model
                    mapped_messages = []
                    for msg in messages:
                        try:
                            mapped_msg = Message(
                                message_id=msg.get("messageGuid", ""),
                                content=msg.get("messageText", ""),
                                created_at=msg.get("messageCreatedDate", ""),
                                is_user_message=msg.get("messageType", 0) == 2,  # 2 = user, 1 = bot
                                conversation_id=conv_id,
                                documents=msg.get("documents", []),
                                metadata={
                                    "additionalInfo": msg.get("additionalInfo"),
                                    "isMessageLiked": msg.get("isMessageLiked"),
                                    "user": msg.get("user", {})
                                }
                            )
                            mapped_messages.append(mapped_msg)
                        except Exception as e:
                            logger.warning(f"Failed to map message: {e}")
                    
                    # Create a ConversationHistory from the group
                    try:
                        # Extract conversation title from the first user message if available
                        title = next((m.get("messageText", "").strip() for m in messages if m.get("messageType") == 2), "Conversation")
                        # Truncate title if too long
                        if len(title) > 30:
                            title = title[:27] + "..."
                        
                        conv_history = ConversationHistory(
                            conversation_id=conv_id,
                            title=title,
                            created_at=first_msg_time,
                            last_message_at=last_msg_time,
                            message_count=len(messages),
                            messages=mapped_messages
                        )
                        history_items.append(conv_history)
                    except Exception as e:
                        logger.warning(f"Failed to create ConversationHistory: {e}")
            else:
                logger.warning(f"Unexpected API response format: {type(result)}")
                if isinstance(result, dict):
                    logger.debug(f"Response keys: {result.keys()}")
            
            return history_items
            
        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error getting chat history: {str(e)}")
            raise ConnectionError(f"Failed to get chat history: {str(e)}")
            
    async def group_messages_by_conversation(self, messages: List[Dict]) -> Dict[str, List[Dict]]:
        """
        Group messages by conversation based on temporal proximity and user patterns.
        
        This is a heuristic-based approach since the API doesn't provide explicit conversation IDs.
        
        Args:
            messages: List of message dictionaries from the API
            
        Returns:
            Dict[str, List[Dict]]: Dictionary mapping conversation IDs to lists of messages
        """
        if not messages:
            return {}
            
        # Sort messages by timestamp to establish timeline
        sorted_msgs = sorted(messages, key=lambda m: m.get("messageCreatedDate", ""))
        
        # Dictionary to hold conversation groups
        conversation_groups = {}
        
        # Time threshold to determine if messages are in the same conversation (15 minutes)
        time_threshold_minutes = 15
        
        current_group_id = None
        current_group = []
        last_timestamp = None
        
        for msg in sorted_msgs:
            timestamp = msg.get("messageCreatedDate", "")
            
            # If this is the first message or we can't extract timestamp, start a new group
            if not last_timestamp or not timestamp:
                if current_group_id and current_group:
                    conversation_groups[current_group_id] = current_group
                current_group_id = str(uuid.uuid4())
                current_group = [msg]
                last_timestamp = timestamp
                continue
                
            # Try to parse timestamps
            try:
                last_time = datetime.fromisoformat(last_timestamp.replace('Z', '+00:00'))
                current_time = datetime.fromisoformat(timestamp.replace('Z', '+00:00'))
                time_diff = (current_time - last_time).total_seconds() / 60
                
                # If time difference exceeds threshold, start a new conversation
                if time_diff > time_threshold_minutes:
                    if current_group_id and current_group:
                        conversation_groups[current_group_id] = current_group
                    current_group_id = str(uuid.uuid4())
                    current_group = [msg]
                else:
                    # Same conversation, add to current group
                    current_group.append(msg)
            except (ValueError, TypeError):
                # If timestamp parsing fails, just add to current group
                current_group.append(msg)
                
            last_timestamp = timestamp
            
        # Add the last group if not empty
        if current_group_id and current_group:
            conversation_groups[current_group_id] = current_group
            
        return conversation_groups
    
    async def get_conversation_messages(
        self, 
        context: ChatContext, 
        conversation_guid: str
    ) -> List[Message]:
        """
        Get messages in a conversation.
        
        Args:
            context: Chat context with session info
            conversation_guid: GUID of the conversation
            
        Returns:
            List[Message]: List of messages in the conversation
            
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
            InvalidRequestError: If conversation_guid is invalid
        """
        try:
            endpoint = f"/api/Chat/{conversation_guid}/message"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header with API key and add session ID to associate user with conversation
            headers = self.auth_handler.prepare_auth_header(token)
            
            # Add user information to query parameters to ensure proper user-conversation mapping
            params = {}
            if context.session_id:
                params["userGuid"] = context.session_id
                
            logger.debug(f"Fetching messages for conversation {conversation_guid} with user {context.session_id}")
            
            # First, verify if the conversation exists in history (to ensure user has access)
            try:
                history = await self.get_chat_history(context)
                conversation_exists = any(item.conversation_id == conversation_guid for item in history)
                
                if not conversation_exists:
                    logger.warning(f"Conversation {conversation_guid} not found in user's history")
                    # Try to get the conversation anyway, the API will return an appropriate error
            except Exception as e:
                # Continue even if history check fails
                logger.warning(f"Couldn't verify conversation in history: {str(e)}")
            
            # Make the actual request to get messages
            result = await self.http_client.request(
                "GET", 
                endpoint, 
                headers=headers,
                params=params
            )
            
            # Check if we got a valid response (should be a list of messages)
            if not isinstance(result, list):
                logger.warning(f"Unexpected response format for conversation messages: {type(result)}")
                if isinstance(result, dict) and result.get("message"):
                    raise InvalidRequestError(f"API error: {result.get('message')}")
                return []
            
            # Convert result to list of message objects
            messages = []
            for item in result:
                try:
                    # Map API response fields to our Message model
                    message = Message(
                        message_id=item.get("messageGuid", ""),
                        content=item.get("messageText", ""),
                        created_at=item.get("messageCreatedDate", ""),
                        is_user_message=item.get("messageTypeId") == 0, # Assuming 0=user, 1=assistant
                        conversation_id=item.get("conversationGuid", conversation_guid),
                        documents=item.get("documents", [])
                    )
                    messages.append(message)
                except Exception as msg_error:
                    logger.warning(f"Error converting message: {str(msg_error)}")
                    continue
                    
            return messages
            
        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error getting conversation messages: {str(e)}")
            # If conversation doesn't exist, provide a more specific error message
            if "Invalid conversation" in str(e) and "mapping" in str(e):
                # Try to start a new conversation if this one failed
                logger.info(f"Conversation {conversation_guid} is invalid, it may have expired")
                raise ConnectionError(f"Conversation {conversation_guid} is invalid or has expired. Please start a new conversation.")
            raise ConnectionError(f"Failed to get conversation messages: {str(e)}")
    
    async def delete_conversations(
        self, 
        context: ChatContext, 
        conversation_guids: List[str]
    ) -> bool:
        """
        Delete one or more conversations.
        
        Args:
            context: Chat context with session info
            conversation_guids: List of conversation GUIDs to delete
            
        Returns:
            bool: True if successful
            
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
            InvalidRequestError: If conversation_guids is invalid
        """
        try:
            endpoint = "/api/Chat"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            await self.http_client.request(
                "DELETE", 
                endpoint, 
                headers=headers,
                data=conversation_guids
            )
            
            return True
            
        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error deleting conversations: {str(e)}")
            raise ConnectionError(f"Failed to delete conversations: {str(e)}")
            
    async def get_document(
        self, 
        context: ChatContext,
        document_guid: str
    ) -> DocumentContent:
        """
        Get document content by GUID, extracting text from the PDF and optionally saving to filesystem.
        
        Args:
            context: Chat context with session info
            document_guid: GUID of the document to retrieve
        
        Returns:
            DocumentContent: Document content with extracted text
        
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
            InvalidRequestError: If document_guid is invalid
            DocumentNotFoundError: If the document is not found
        """
        try:
            endpoint = f"/api/Documents/{document_guid}/download"
            headers = self.auth_handler.prepare_auth_header(context.token)
            
            # Request the document as binary data
            result = await self.http_client.request(
                "GET", 
                endpoint, 
                headers=headers,
                is_binary=True
            )
            
            content = result.get("content", b"")
            
            # Extract text from PDF bytes
            text_content = ""
            if content:
                try:
                    pdf_stream = io.BytesIO(content)
                    reader = PdfReader(pdf_stream)
                    text_content = "\n".join(page.extract_text() or "" for page in reader.pages)
                    
                    # Save the PDF to filesystem only if debug_pdf_storage is enabled
                    if self.config.debug_pdf_storage:
                        # Define the documents storage directory structure
                        base_docs_dir = os.path.join(os.path.dirname(os.path.abspath(__file__)), "documents")
                        year_month = datetime.now().strftime("%Y-%m")
                        docs_dir = os.path.join(base_docs_dir, year_month)
                        
                        # Create directories if they don't exist
                        os.makedirs(docs_dir, exist_ok=True)
                        
                        # Save the PDF file
                        filename = f"{document_guid}.pdf"
                        file_path = os.path.join(docs_dir, filename)
                        
                        with open(file_path, 'wb') as f:
                            f.write(content)
                        
                        logger.info(f"PDF document saved to {file_path} (debug_pdf_storage enabled)")
                    
                except Exception as pdf_err:
                    logger.error(f"Failed to process PDF: {pdf_err}")
                    text_content = ""
            else:
                logger.warning(f"No content received for document {document_guid}")
            
            return DocumentContent(document=text_content)
            
        except GPOClientException as e:
            if "not found" in str(e).lower():
                raise DocumentNotFoundError(f"Document with ID {document_guid} not found")
            raise
        except Exception as e:
            logger.error(f"Error getting document: {str(e)}")
            raise DocumentAccessError(f"Failed to access document {document_guid}: {str(e)}")

    # === Specialized Document Retrieval Methods ===
    
    async def get_relevant_documents(self, query: str) -> dict:
        """
        Get the answer and a list of documents relevant to the given query.
        This method sends a chat request and returns the AI's answer as a Message instance,
        along with any referenced documents as Document instances.

        Args:
            query: Natural language query to find relevant documents
        Returns:
            dict: { 'answer': Message, 'documents': List[Document] }
        Raises:
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
        """
        try:
            # Create a session for authentication
            session_context = SessionContext(
                instance_id="document-retrieval",
                user_id=str(uuid.uuid4()),
                project_id="document-search"
            )
            session_response = await self.session(session_context)

            # Create a chat context
            chat_context = ChatContext(
                session_id=session_response.session_id,
                token=session_response.token
            )

            # Start a conversation
            conversation_response = await self.start_conversation(chat_context)
            chat_context.conversation_id = conversation_response.conversation_id

            # Send a chat request to get the answer and document recommendations
            chat_request = ChatRequest(
                query=f"I need documents about: {query}",
                response_type="Question"
            )
            chat_response = await self.chat(chat_context, chat_request)

            # Build the answer as a Message instance
            first_msg = chat_response.raw_messages[0] if chat_response.raw_messages else {}
            answer = Message(
                message_id=first_msg.get("messageGuid", chat_response.message_id or ""),
                content=first_msg.get("messageText", chat_response.message_content or ""),
                created_at=first_msg.get("messageCreatedDate", chat_response.created_at),
                is_user_message=first_msg.get("messageType", 1) == 2,  # 2=user, 1=bot
                conversation_id=first_msg.get("conversationGuid", chat_response.conversation_id),
                documents=first_msg.get("documents", chat_response.documents or []),
                metadata={
                    "user": first_msg.get("user", {}),
                    "additionalInfo": first_msg.get("additionalInfo"),
                    "score": first_msg.get("score"),
                }
            )

            # Build Document instances for each referenced document
            documents = []
            for doc in (first_msg.get("documents") or []):
                # Defensive mapping: only map if documentGuid is present
                if doc.get("documentGuid"):
                    documents.append(Document(
                        document_guid=doc.get("documentGuid"),
                        title=doc.get("documentName", ""),
                        filename=doc.get("documentName", ""),
                        file_type=doc.get("fileType", "unknown"),
                        size=doc.get("size", 0),
                        created_at=doc.get("createdAt", ""),
                        modified_at=doc.get("modifiedAt", ""),
                        author=doc.get("author"),
                        categories=[doc.get("category")] if doc.get("category") else [],
                        tags=doc.get("tags", []),
                        metadata={
                            "pages": doc.get("pages"),
                            "videoUrl": doc.get("videoUrl"),
                            "isVideoDocument": doc.get("isVideoDocument"),
                            "version": doc.get("version"),
                            "active": doc.get("active"),
                            "subCategory": doc.get("subCategory"),
                            "solution": doc.get("solution"),
                            "discoverUrl": doc.get("discoverUrl"),
                            "isDiscoverFile": doc.get("isDiscoverFile"),
                        },
                        summary=doc.get("summary")
                    ))
            # Clean up the conversation
            await self.delete_conversations(
                chat_context,
                [conversation_response.conversation_id]
            )

            return {"answer":answer, "documents": documents}

        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error retrieving relevant documents: {str(e)}")
            raise ConnectionError(f"Failed to retrieve relevant documents: {str(e)}")
    
    async def get_document_by_id(self, document_guid: str) -> DocumentContent:
        """
        Get a document by its unique identifier.
        
        This method retrieves the full content of a document based on its GUID.
        
        Args:
            document_guid: GUID of the document to retrieve
            
        Returns:
            DocumentContent: Document content and metadata
            
        Raises:
            DocumentNotFoundError: If the document is not found
            DocumentAccessError: If document access fails
        """
        try:
            # Create a session for authentication
            session_context = SessionContext(
                instance_id="document-retrieval",
                user_id=str(uuid.uuid4()),
                project_id="document-access"
            )
            session_response = await self.session(session_context)
            
            # Create a chat context
            chat_context = ChatContext(
                session_id=session_response.session_id,
                token=session_response.token
            )
            
            # Get the document
            document_content = await self.get_document(chat_context, document_guid)
            
            return document_content
            
        except DocumentNotFoundError:
            # Re-raise document not found error
            raise
        except GPOClientException as e:
            logger.error(f"Error retrieving document by ID: {str(e)}")
            raise DocumentAccessError(f"Failed to retrieve document {document_guid}: {str(e)}")
        except Exception as e:
            logger.error(f"Unexpected error retrieving document by ID: {str(e)}")
            raise DocumentAccessError(f"Failed to retrieve document {document_guid}: {str(e)}")
    
    async def retrieve_documents_for_query(self, query: str) -> Dict:
        """
        Comprehensive document retrieval based on a query.
        
        This method combines document search and retrieval in a single operation.
        It searches for relevant documents and retrieves their content.
        
        Args:
            query: Natural language query to find relevant documents
            
        Returns:
            Dict: Dictionary containing both answer (Message) and DocumentQueryResponse with documents
                 {'answer': Message, 'document_query_response': DocumentQueryResponse}
            
        Raises:
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
            DocumentAccessError: If document access fails
        """
        try:
            start_time = time.time()
            
            # 1. Get relevant documents and answer based on the query
            response = await self.get_relevant_documents(query)
            
            # Extract answer and documents from the response
            answer = response.get('answer')
            documents = response.get('documents', [])
            
            if not documents:
                # No documents found, prepare empty response
                document_query_response = DocumentQueryResponse(
                    query=query,
                    documents=[],
                    search_time_ms=int((time.time() - start_time) * 1000)
                )
                return {'answer': answer, 'document_query_response': document_query_response}
            
            # 2. Retrieve each document's content
            docs_with_content = []
            for document in documents:
                try:
                    # Get the full content of each document
                    document_content = await self.get_document_by_id(document.document_guid)
                    
                    # Add to results
                    docs_with_content.append({
                        "metadata": document,
                        "content": document_content
                    })
                    
                except (DocumentNotFoundError, DocumentAccessError) as e:
                    # Log the error but continue with other documents
                    logger.warning(f"Could not retrieve document {document.document_guid}: e")
            
            # Calculate total search and retrieval time
            search_time_ms = int((time.time() - start_time) * 1000)
            
            # 3. Create DocumentQueryResponse for the documents
            document_query_response = DocumentQueryResponse(
                query=query,
                documents=docs_with_content,
                search_time_ms=search_time_ms
            )
            
            # 4. Return comprehensive response with both answer and documents
            return {
                'answer': answer,
                'document_query_response': document_query_response
            }
            
        except GPOClientException:
            # Re-raise client exceptions
            raise
        except Exception as e:
            logger.error(f"Error in comprehensive document retrieval: {str(e)}")
            raise ConnectionError(f"Failed to retrieve documents for query: {str(e)}")
    
    async def submit_chat_feedback(self, context: ChatContext, feedback: FeedbackRequest) -> bool:
        """
        Submit feedback for a chat message.
        Args:
            context: Chat context with session info
            feedback: FeedbackRequest model
        Returns:
            bool: True if feedback was submitted successfully
        Raises:
            AuthenticationError, ConnectionError, TimeoutError
        """
        try:
            endpoint = "/api/Chat/feedback"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            await self.http_client.request(
                "POST",
                endpoint,
                headers=headers,
                data=feedback.dict()
            )
            return True
        except GPOClientException:
            raise
        except Exception as e:
            logger.error(f"Error submitting chat feedback: {str(e)}")
            raise ConnectionError(f"Failed to submit chat feedback: {str(e)}")
    
    async def submit_handoff_feedback(self, context: ChatContext, handoff_feedback: HandoffFeedbackRequest) -> bool:
        """
        Submit handoff feedback for a conversation.
        Args:
            context: Chat context with session info
            handoff_feedback: HandoffFeedbackRequest model
        Returns:
            bool: True if handoff feedback was submitted successfully
        Raises:
            AuthenticationError, ConnectionError, TimeoutError
        """
        try:
            endpoint = "/api/Chat/handoff-feedback"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            await self.http_client.request(
                "POST",
                endpoint,
                headers=headers,
                data=handoff_feedback.dict()
            )
            return True
        except GPOClientException:
            raise
        except Exception as e:
            logger.error(f"Error submitting handoff feedback: {str(e)}")
            raise ConnectionError(f"Failed to submit handoff feedback: {str(e)}")

    async def submit_rating(self, context: ChatContext, rating: RatingRequest) -> bool:
        """
        Submit a rating for a conversation.
        Args:
            context: Chat context with session info
            rating: RatingRequest model
        Returns:
            bool: True if rating was submitted successfully
        Raises:
            AuthenticationError, ConnectionError, TimeoutError
        """
        try:
            endpoint = "/api/Ratings"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            await self.http_client.request(
                "POST",
                endpoint,
                headers=headers,
                data=rating.dict()
            )
            return True
        except GPOClientException:
            raise
        except Exception as e:
            logger.error(f"Error submitting rating: {str(e)}")
            raise ConnectionError(f"Failed to submit rating: {str(e)}")

    async def get_ratings(self, context: ChatContext, filter_request: RatingsFilterRequest) -> Any:
        """
        Retrieve ratings with optional filters.
        Args:
            context: Chat context with session info
            filter_request: RatingsFilterRequest model
        Returns:
            Any: Ratings data (should be modeled for production)
        Raises:
            AuthenticationError, ConnectionError, TimeoutError
        """
        try:
            endpoint = "/api/Ratings/filter"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            result = await self.http_client.request(
                "POST",
                endpoint,
                headers=headers,
                data=filter_request.dict()
            )
            return result
        except GPOClientException:
            raise
        except Exception as e:
            logger.error(f"Error retrieving ratings: {str(e)}")
            raise ConnectionError(f"Failed to retrieve ratings: {str(e)}")

    async def export_ratings(self, context: ChatContext, filter_request: RatingsFilterRequest) -> Any:
        """
        Export ratings data with optional filters.
        Args:
            context: Chat context with session info
            filter_request: RatingsFilterRequest model
        Returns:
            Any: Exported ratings data (should be modeled for production)
        Raises:
            AuthenticationError, ConnectionError, TimeoutError
        """
        try:
            endpoint = "/api/Ratings/export"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            result = await self.http_client.request(
                "POST",
                endpoint,
                headers=headers,
                data=filter_request.dict()
            )
            return result
        except GPOClientException:
            raise
        except Exception as e:
            logger.error(f"Error exporting ratings: {str(e)}")
            raise ConnectionError(f"Failed to export ratings: {str(e)}")

    async def get_document_categories(self, context: ChatContext, category_type: Optional[str] = None) -> Any:
        """
        Retrieve document categories.
        Args:
            context: Chat context with session info
            category_type: Optional category type (Category or SubCategory)
        Returns:
            Any: Document categories data (should be modeled for production)
        Raises:
            AuthenticationError, ConnectionError, TimeoutError
        """
        try:
            endpoint = "/api/Documents/Documentcategories"
            
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            params = {}
            if category_type:
                params["documentCategoryType"] = category_type
            result = await self.http_client.request(
                "GET",
                endpoint,
                headers=headers,
                params=params
            )
            return result
        except GPOClientException:
            raise
        except Exception as e:
            logger.error(f"Error retrieving document categories: {str(e)}")
            raise ConnectionError(f"Failed to retrieve document categories: {str(e)}")

    async def get_metric(self, context: ChatContext, endpoint: str, metrics_request: MetricsRequest) -> Any:
        """
        Generic method to retrieve metrics from a metrics endpoint.
        Args:
            context: Chat context with session info
            endpoint: API endpoint for the metric
            metrics_request: MetricsRequest model
        Returns:
            Any: Metrics data (should be modeled for production)
        Raises:
            AuthenticationError, ConnectionError, TimeoutError
        """
        try:
            # Use API key as the token for authentication
            token = self.config.api_key
            
            # Prepare auth header
            headers = self.auth_handler.prepare_auth_header(token)
            
            result = await self.http_client.request(
                "POST",
                endpoint,
                headers=headers,
                data=metrics_request.dict()
            )
            return result
        except GPOClientException:
            raise
        except Exception as e:
            logger.error(f"Error retrieving metric from {endpoint}: {str(e)}")
            raise ConnectionError(f"Failed to retrieve metric from {endpoint}: {str(e)}")
