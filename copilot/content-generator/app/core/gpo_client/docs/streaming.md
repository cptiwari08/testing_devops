# Streaming Responses

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document explains how to use the GPO Core API Python Client to work with streaming responses from the GPO API. Streaming enables receiving partial responses as they are generated, which is useful for building interactive real-time applications.

## Overview

Streaming allows you to:
- Show responses to users as they are generated (improving perceived responsiveness)
- Process partial results incrementally rather than waiting for the complete response
- Handle long-running requests more efficiently
- Implement real-time user interfaces like typing animations

The GPO Core API Python Client provides dedicated methods and models for handling streaming responses from the GPO API.

## Basic Usage

Here's how to make a basic streaming request:

```python
import asyncio
from app.core.gpo_client.client import GPOCoreClient
from app.core.gpo_client.models import (
    SessionContext, 
    ChatContext, 
    StreamingChatRequest
)

async def streaming_example():
    # Initialize client
    client = GPOCoreClient()
    
    # Create session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    session = await client.session(session_context)
    
    # Start conversation
    chat_context = ChatContext(
        session_id=session.session_id,
        token=session.token
    )
    conversation = await client.start_conversation(
        context=chat_context,
        request={"title": "Streaming example"}
    )
    
    # Configure streaming request
    context = ChatContext(
        session_id=session.session_id,
        conversation_id=conversation.conversation_id,
        token=session.token
    )
    
    # Create streaming request
    request = StreamingChatRequest(
        query="Explain the different tax regimes in Mexico for technology companies",
        use_openai=True,
        stream=True  # This flag enables streaming
    )
    
    # Process streaming response
    print("Streaming response:")
    async for chunk in client.chat_stream(context=context, request=request):
        if chunk.content:
            print(chunk.content, end="", flush=True)
        
        # Check if this is the final chunk
        if chunk.is_complete:
            print("\n\nStream complete!")
            
            # Process sources if available in the final chunk
            if chunk.sources:
                print("\nSources:")
                for source in chunk.sources:
                    print(f"- {source.title}")

if __name__ == "__main__":
    asyncio.run(streaming_example())
```

## Understanding StreamingChunk

Each chunk in a streaming response is represented by the `StreamingChunk` model:

```python
class StreamingChunk(BaseModel):
    content: Optional[str] = None  # Text content in this chunk
    sources: Optional[List[CitationSource]] = None  # Sources cited (usually in final chunk)
    is_complete: bool = False  # Whether this is the final chunk
    metadata: Optional[Dict[str, Any]] = None  # Additional metadata for this chunk
```

Key points about `StreamingChunk`:
- Not all chunks will contain content (some may be markers or metadata)
- Sources are typically provided in the final chunk
- The `is_complete` flag helps identify the end of the stream
- Metadata may contain additional information about the chunk

## Handling Streaming Sources

Sources (citations) are typically sent at the end of a streaming response. Here's how to handle them:

```python
async def process_streaming_with_sources():
    # ... initialize client, session, etc.
    
    # Prepare for collecting data
    full_content = ""
    all_sources = []
    
    # Process stream
    async for chunk in client.chat_stream(context=context, request=request):
        # Accumulate content
        if chunk.content:
            full_content += chunk.content
            print(chunk.content, end="", flush=True)
        
        # Collect sources
        if chunk.sources:
            all_sources.extend(chunk.sources)
        
        # Final chunk processing
        if chunk.is_complete:
            print("\n\nResponse complete. Processing sources...")
            
            # Process all collected sources
            for source in all_sources:
                print(f"\nSource: {source.title}")
                print(f"Relevance: {source.relevance_score}")
                if hasattr(source, 'content_excerpt') and source.content_excerpt:
                    print(f"Excerpt: {source.content_excerpt}")
    
    return {
        "content": full_content,
        "sources": [
            {"title": s.title, "relevance": s.relevance_score}
            for s in all_sources
        ]
    }
```

## Custom Stream Processing

You might want to perform custom processing on streaming chunks. Here's an example that shows how to process and transform chunks:

```python
import re
from html import escape

async def custom_stream_processing():
    # ... initialize client, session, etc.
    
    # Track whether we're inside a code block
    in_code_block = False
    
    # Process stream
    async for chunk in client.chat_stream(context=context, request=request):
        content = chunk.content or ""
        
        # Custom processing:
        # - Highlight keywords
        # - Format code blocks
        # - Count tokens
        
        # Example: highlight keywords
        highlighted_content = content
        keywords = ["tax", "deduction", "credit", "VAT", "income"]
        for keyword in keywords:
            pattern = re.compile(rf'(\b{keyword}\b)', re.IGNORECASE)
            highlighted_content = pattern.sub(r'<strong>\1</strong>', highlighted_content)
        
        # Example: detect code blocks
        if "```" in content:
            in_code_block = not in_code_block  # Toggle state
            
        # Apply different formatting based on whether we're in a code block
        if in_code_block:
            # Format as code (e.g., in a real app, apply syntax highlighting)
            print(f"<code>{escape(content)}</code>", end="", flush=True)
        else:
            # Regular text
            print(highlighted_content, end="", flush=True)
        
        # Handle end of stream
        if chunk.is_complete:
            print("\n<end of response>")
```

## Implementing a Web Interface

Here's how you might use streaming responses with a web framework like FastAPI:

```python
from fastapi import FastAPI, WebSocket, Depends, HTTPException
from fastapi.responses import StreamingResponse
import json
import asyncio
from typing import AsyncGenerator

# ... imports for GPO client

app = FastAPI()

# WebSocket endpoint for streaming
@app.websocket("/api/chat/stream")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    
    try:
        # Receive query
        data = await websocket.receive_text()
        request_data = json.loads(data)
        
        query = request_data.get("query")
        session_id = request_data.get("session_id")
        conversation_id = request_data.get("conversation_id")
        token = request_data.get("token")
        
        # Create client and context
        client = GPOCoreClient()
        context = ChatContext(
            session_id=session_id,
            conversation_id=conversation_id,
            token=token
        )
        
        # Create streaming request
        request = StreamingChatRequest(
            query=query,
            use_openai=True,
            stream=True
        )
        
        # Process streaming response
        sources = []
        
        # Send chunks to websocket client
        async for chunk in client.chat_stream(context=context, request=request):
            response_data = {}
            
            if chunk.content:
                response_data["content"] = chunk.content
            
            if chunk.sources:
                sources.extend(chunk.sources)
                response_data["sources"] = [
                    {"title": s.title, "relevance": s.relevance_score}
                    for s in chunk.sources
                ]
            
            response_data["is_complete"] = chunk.is_complete
            
            # Send JSON response
            await websocket.send_text(json.dumps(response_data))
            
    except Exception as e:
        await websocket.send_text(json.dumps({"error": str(e)}))
    finally:
        await websocket.close()

# HTTP endpoint for streaming
@app.get("/api/chat/stream-http")
async def stream_chat_http(
    query: str,
    session_id: str,
    conversation_id: str,
    token: str
) -> StreamingResponse:
    
    async def generate() -> AsyncGenerator[str, None]:
        try:
            # Create client and context
            client = GPOCoreClient()
            context = ChatContext(
                session_id=session_id,
                conversation_id=conversation_id,
                token=token
            )
            
            # Create streaming request
            request = StreamingChatRequest(
                query=query,
                use_openai=True,
                stream=True
            )
            
            # Process streaming response
            async for chunk in client.chat_stream(context=context, request=request):
                if chunk.content:
                    # Yield each chunk as a server-sent event
                    yield f"data: {json.dumps({'content': chunk.content})}\n\n"
                
                if chunk.is_complete:
                    # Send final chunk with sources
                    sources = []
                    if chunk.sources:
                        sources = [
                            {"title": s.title, "relevance": s.relevance_score}
                            for s in chunk.sources
                        ]
                    
                    yield f"data: {json.dumps({'is_complete': True, 'sources': sources})}\n\n"
        
        except Exception as e:
            yield f"data: {json.dumps({'error': str(e)})}\n\n"
            
    return StreamingResponse(
        generate(),
        media_type="text/event-stream"
    )
```

### Client-side JavaScript example for WebSockets

```javascript
// Establish WebSocket connection
const socket = new WebSocket('ws://localhost:8000/api/chat/stream');

// Handle connection open
socket.addEventListener('open', (event) => {
    // Send query to the server
    socket.send(JSON.stringify({
        query: "What are the tax implications for tech startups in Mexico?",
        session_id: "session-12345",
        conversation_id: "conv-67890",
        token: "jwt-token-here"
    }));
});

// Variable to accumulate content
let fullContent = '';
let sources = [];

// Handle incoming messages
socket.addEventListener('message', (event) => {
    const data = JSON.parse(event.data);
    
    // Append content to UI
    if (data.content) {
        fullContent += data.content;
        document.getElementById('response').innerHTML = fullContent;
    }
    
    // Handle sources
    if (data.sources) {
        sources = data.sources;
        const sourcesList = document.getElementById('sources');
        sourcesList.innerHTML = '';
        
        sources.forEach(source => {
            const li = document.createElement('li');
            li.textContent = `${source.title} (Relevance: ${source.relevance_score})`;
            sourcesList.appendChild(li);
        });
    }
    
    // Handle completion
    if (data.is_complete) {
        console.log('Stream complete');
        document.getElementById('status').textContent = 'Response complete';
    }
    
    // Handle errors
    if (data.error) {
        console.error('Error:', data.error);
        document.getElementById('error').textContent = data.error;
    }
});

// Handle errors
socket.addEventListener('error', (event) => {
    console.error('WebSocket Error:', event);
    document.getElementById('error').textContent = 'Connection error';
});

// Handle connection close
socket.addEventListener('close', (event) => {
    console.log('Connection closed', event.code, event.reason);
    document.getElementById('status').textContent = 'Connection closed';
});
```

## Error Handling in Streams

Handling errors during streaming requires special consideration:

```python
async def streaming_with_error_handling():
    client = GPOCoreClient()
    
    try:
        # ... initialize session, conversation, etc.
        
        # Process stream
        try:
            async for chunk in client.chat_stream(context=context, request=request):
                # Process chunks normally
                if chunk.content:
                    print(chunk.content, end="", flush=True)
        except Exception as stream_error:
            # Handle errors that occur during streaming
            print(f"\nError during streaming: {stream_error}")
            
            # You might want to handle specific streaming errors
            if "connection reset" in str(stream_error).lower():
                print("Connection was reset. This could be due to network issues.")
            elif "timeout" in str(stream_error).lower():
                print("Stream timed out. The response might be too large.")
            
            # Attempt recovery or provide graceful degradation
            print("\nAttempting to get a non-streaming response instead...")
            
            # Switch to non-streaming request as fallback
            non_streaming_request = ChatRequest(
                query=request.query,
                use_openai=request.use_openai
            )
            
            response = await client.chat(context=context, request=non_streaming_request)
            print(f"\nFallback response: {response.message_content}")
    
    except Exception as e:
        # Handle any other errors
        print(f"An error occurred: {e}")
        # Implement appropriate error handling and recovery
```

## Performance Considerations

When working with streaming responses, keep these performance considerations in mind:

1. **Network Efficiency**: 
   - Streaming requires a persistent connection
   - Make sure your infrastructure supports long-lived connections
   - Consider timeouts and connection limits

2. **Memory Management**:
   - For large responses, be mindful of how you accumulate content
   - Consider windowing or truncating very large responses
   - Release resources properly when you're done with a stream

3. **UI Responsiveness**:
   - Update UIs incrementally, but not for every tiny chunk
   - Consider batching updates to avoid UI flickering
   - Implement proper loading states for a smooth user experience

4. **Error Handling**:
   - Have proper error recovery for interrupted streams
   - Implement reconnection logic where appropriate
   - Always provide feedback to users if the stream fails

5. **Backpressure**:
   - Consider implementing backpressure if processing can't keep up with chunk arrival
   - This is particularly important for high-volume streams

## Example: Stream with Backpressure

```python
import asyncio
import time

async def stream_with_backpressure():
    client = GPOCoreClient()
    
    # ... initialize session, conversation, etc.
    
    # Configure processing parameters
    chunk_buffer = []
    max_buffer_size = 10  # Maximum chunks to buffer
    processing_delay = 0.1  # Simulated processing time in seconds
    
    # Process stream with backpressure
    paused = False
    
    async for chunk in client.chat_stream(context=context, request=request):
        # Add chunk to buffer
        chunk_buffer.append(chunk)
        
        # Apply backpressure if buffer gets too full
        if len(chunk_buffer) >= max_buffer_size and not paused:
            print("\n[Applying backpressure - pausing stream]")
            paused = True
            
        # Process chunks from buffer
        while chunk_buffer and (paused or chunk.is_complete):
            next_chunk = chunk_buffer.pop(0)
            
            # Process the chunk (with simulated delay)
            if next_chunk.content:
                print(next_chunk.content, end="", flush=True)
                await asyncio.sleep(processing_delay)  # Simulate processing
            
            # Check if we can resume
            if paused and len(chunk_buffer) <= max_buffer_size // 2:
                print("\n[Releasing backpressure - resuming stream]")
                paused = False
        
        # Final processing for last chunk
        if chunk.is_complete:
            print("\nStream complete!")
```

## Conclusion

Streaming responses provide a more interactive and efficient way to handle large or incremental responses from the GPO API. By following the patterns and best practices in this document, you can implement robust streaming functionality in your applications.

For more information on related topics, see:
- [API Reference](api_reference.md) for detailed information on streaming methods and models
- [Error Handling](error_handling.md) for more information on handling errors
- [Integration Guide](integration.md) for examples of integrating streaming with existing services