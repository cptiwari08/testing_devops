"""
Document Retrieval Example

This example demonstrates how to use the GPO Core API client for retrieving documents
based on natural language queries and specific document IDs.
"""

import asyncio
import logging
from typing import List
import os

from gpo_client.client import GPOCoreClient
from gpo_client.models.context import SessionContext, ChatContext
from gpo_client.models.request import ChatRequest
from gpo_client.models.response import Document, DocumentContent
from gpo_client.config import GPOConfig
from gpo_client.exceptions import GPOClientException

# Set up logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

# Example configuration
API_BASE_URL = "https://satknowledgeassistant-dev.ey.com"

async def example_get_relevant_documents():
    """
    Example of retrieving relevant documents based on a natural language query.
    """
    # Configure client
    config = GPOConfig(base_url=API_BASE_URL)
    client = GPOCoreClient(config)
    
    # Define the query
    query = "What are the tax implications of transfer pricing between related entities?"
    
    logger.info(f"Searching for documents related to: '{query}'")
    
    try:
        # Get relevant documents
        documents = await client.get_relevant_documents(query)
        
        # Display results
        logger.info(f"Found {len(documents)} relevant documents:")
        for i, doc in enumerate(documents, 1):
            logger.info(f"{i}. {doc.title} (ID: {doc.document_guid})")
            logger.info(f"   Type: {doc.file_type}, Size: {doc.size} bytes")
            logger.info(f"   Created: {doc.created_at}")
            if hasattr(doc, 'categories') and doc.categories:
                logger.info(f"   Categories: {', '.join(doc.categories)}")
            logger.info("")
            
    except GPOClientException as e:
        logger.error(f"Error retrieving documents: {e}")
        
async def example_get_document_by_id():
    """
    Example of retrieving a specific document by its ID.
    """
    # Configure client
    config = GPOConfig(base_url=API_BASE_URL)
    client = GPOCoreClient(config)
    
    # Define the document ID
    # Note: Replace with an actual document GUID from your system
    document_guid = "d8a8b8c7-9e54-4bbf-8e7c-a15679f54321"
    
    logger.info(f"Retrieving document with ID: {document_guid}")
    
    try:
        # Get document
        document_content = await client.get_document_by_id(document_guid)
        
        # Display results
        doc = document_content.document
        logger.info(f"Retrieved document: {doc.title} (ID: {doc.document_guid})")
        logger.info(f"Type: {doc.file_type}, Size: {doc.size} bytes")
        logger.info(f"Content type: {document_content.content_type}")
        
        # Save document to file
        output_dir = "retrieved_documents"
        os.makedirs(output_dir, exist_ok=True)
        
        filename = f"{output_dir}/{doc.filename}"
        with open(filename, "wb") as f:
            f.write(document_content.content)
            
        logger.info(f"Document saved to: {filename}")
        
        # If text content is available, print a preview
        if document_content.text_content:
            preview_length = min(500, len(document_content.text_content))
            logger.info(f"Text content preview: {document_content.text_content[:preview_length]}...")
            
    except GPOClientException as e:
        logger.error(f"Error retrieving document: {e}")
        
async def example_retrieve_documents_for_query():
    """
    Example of the comprehensive document retrieval approach that combines
    search and retrieval in one operation.
    """
    # Configure client
    config = GPOConfig(base_url=API_BASE_URL)
    client = GPOCoreClient(config)
    
    # Define the query
    query = "What are the requirements for filing form 1099-MISC?"
    
    logger.info(f"Performing comprehensive search and retrieval for: '{query}'")
    
    try:
        # Execute the comprehensive document retrieval
        result = await client.retrieve_documents_for_query(query)
        
        # Display results
        logger.info(f"Found and retrieved {len(result.documents)} documents in {result.search_time_ms}ms")
        
        # Process each document
        for i, doc_data in enumerate(result.documents, 1):
            metadata = doc_data['metadata']
            content = doc_data['content']
            
            logger.info(f"{i}. {metadata.title} (ID: {metadata.document_guid})")
            logger.info(f"   Type: {metadata.file_type}, Size: {metadata.size} bytes")
            
            # Save document to file
            output_dir = "retrieved_documents"
            os.makedirs(output_dir, exist_ok=True)
            
            filename = f"{output_dir}/{metadata.filename}"
            with open(filename, "wb") as f:
                f.write(content.content)
                
            logger.info(f"   Document saved to: {filename}")
            
            # If text content is available, print a preview
            if content.text_content:
                preview_length = min(200, len(content.text_content))
                logger.info(f"   Text content preview: {content.text_content[:preview_length]}...")
                
            logger.info("")
            
    except GPOClientException as e:
        logger.error(f"Error in document retrieval: {e}")
        
async def example_workflow_with_manual_steps():
    """
    Example of a complete workflow involving manual session and conversation management.
    This demonstrates how to use the individual steps for more control.
    """
    # Configure client
    config = GPOConfig(base_url=API_BASE_URL)
    client = GPOCoreClient(config)
    
    try:
        # 1. Create a session
        logger.info("Creating a session...")
        session_context = SessionContext(
            instance_id="capital-edge-instance",
            user_id="document-service-user",
            project_id="retrieval-system"
        )
        session_response = await client.session(session_context)
        
        logger.info(f"Session created with ID: {session_response.session_id}")
        
        # 2. Create a chat context with session information
        chat_context = ChatContext(
            session_id=session_response.session_id,
            token=session_response.token
        )
        
        # 3. Start a conversation
        conversation = await client.start_conversation(chat_context, None)
        logger.info(f"Conversation started with ID: {conversation.conversation_id}")
        
        # Update the chat context with the conversation ID
        chat_context.conversation_id = conversation.conversation_id
        
        # 4. Send a chat message to get document recommendations
        query = "Show me compliance documents related to anti-money laundering regulations"
        logger.info(f"Sending query: {query}")
        
        chat_request = ChatRequest(query=query)
        chat_response = await client.chat(chat_context, chat_request)
        
        logger.info(f"Received response with message ID: {chat_response.message_id}")
        logger.info(f"Response: {chat_response.message_content[:200]}...")
        
        # 5. Get the conversation messages to extract document references
        logger.info("Getting conversation messages...")
        messages = await client.get_conversation_messages(
            chat_context, conversation.conversation_id
        )
        
        # 6. Extract document references from messages
        document_guids = set()
        for message in messages:
            if not message.is_user_message and message.documents:
                for doc in message.documents:
                    document_guids.add(doc.document_guid)
        
        logger.info(f"Found {len(document_guids)} document references")
        
        # 7. Retrieve each referenced document
        for doc_guid in document_guids:
            logger.info(f"Retrieving document with ID: {doc_guid}")
            document_content = await client.get_document(chat_context, doc_guid)
            
            doc = document_content.document
            logger.info(f"Retrieved document: {doc.title}")
            logger.info(f"Type: {doc.file_type}, Size: {doc.size} bytes")
            
            # Save document to file
            output_dir = "retrieved_documents"
            os.makedirs(output_dir, exist_ok=True)
            
            filename = f"{output_dir}/{doc.filename}"
            with open(filename, "wb") as f:
                f.write(document_content.content)
                
            logger.info(f"Document saved to: {filename}")
            
    except GPOClientException as e:
        logger.error(f"Error in workflow: {e}")

async def main():
    """Main function to run the examples."""
    logger.info("Starting document retrieval examples")
    
    # Uncomment the example you want to run:
    
    # Example 1: Get relevant documents based on a query
    await example_get_relevant_documents()
    
    # Example 2: Get a specific document by ID
    # await example_get_document_by_id()
    
    # Example 3: Comprehensive document search and retrieval
    # await example_retrieve_documents_for_query()
    
    # Example 4: Complete workflow with manual steps
    # await example_workflow_with_manual_steps()
    
    logger.info("Examples completed")

if __name__ == "__main__":
    # Set API base URL from environment variable if available
    if 'GPO_API_BASE_URL' in os.environ:
        API_BASE_URL = os.environ['GPO_API_BASE_URL']
    
    # Run the async examples
    asyncio.run(main())