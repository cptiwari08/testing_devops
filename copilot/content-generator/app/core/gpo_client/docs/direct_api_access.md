# Direct Access to the GPO Core API

[← Back to Main Documentation](../README.md) | [Documentation Index](./README.md)

This document provides detailed instructions for executing queries directly against the GPO Core API without going through the client library. This functionality is useful for manual testing, debugging, or when more granular control over API requests is needed.

## Table of Contents

- [Introduction](#introduction)
- [Prerequisites](#prerequisites)
- [Authentication and Communication Flow](#authentication-and-communication-flow)
- [Payload Examples](#payload-examples)
- [Postman Collection](#postman-collection)
- [Generating Payloads from the Client](#generating-payloads-from-the-client)
- [Curl Commands](#curl-commands)
- [Security Considerations](#security-considerations)
- [Troubleshooting](#troubleshooting)

## Introduction

The GPO Core Python Client normally manages all communications with the GPO Core API server. However, there are situations where it's useful to be able to access the API endpoints directly, for example:

- Debugging integration issues
- Manual testing of new features
- Validation of server responses
- Performance or behavior auditing

This document shows how to manually construct the payloads needed to interact with the GPO Core API and how to use Postman to make these requests.

## Prerequisites

To use the GPO Core API directly, you'll need:

1. The base URL of the GPO Core API server for your environment (development, testing, production)
2. Valid credentials for authentication
3. [Postman](https://www.postman.com/downloads/) or another similar HTTP client
4. Basic knowledge of the GPO Core API endpoints and payload formats

## Authentication and Communication Flow

Communication with the GPO Core API follows a sequential flow that involves several steps:

1. **Create a session**: First, you must establish a session, which will provide you with a JWT token
2. **Start a conversation**: With the session token, you can start a conversation
3. **Send chat messages**: With the session and conversation established, you can send messages

This flow is important, as each step depends on the previous one:

```
┌──────────────┐     ┌─────────────────┐     ┌───────────────┐
│              │     │                 │     │               │
│  1. Session  │────►│ 2. Conversation │────►│  3. Messages  │
│              │     │                 │     │               │
└──────────────┘     └─────────────────┘     └───────────────┘
```

## Payload Examples

### 1. Create a Session

**Endpoint**: `POST /api/User/session`

**Headers**:
```
Content-Type: application/json
```

**Payload**:
```json
{
  "instance_id": "capital-edge-instance-001",
  "user_id": "user-123",
  "project_id": "project-456",
  "environment": "development"
}
```

**Response**:
```json
{
  "session_id": "sess_abc123xyz",
  "token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expires_at": "2025-04-16T23:59:59Z"
}
```

### 2. Start a Conversation

**Endpoint**: `POST /api/Chat/start-conversation`

**Headers**:
```
Content-Type: application/json
Authorization: Bearer <session_token>
```

**Payload**:
```json
{
  "session_id": "sess_abc123xyz",
  "title": "Query about Taxes in Mexico",
  "metadata": {
    "source": "postman_example",
    "purpose": "testing"
  }
}
```

**Response**:
```json
{
  "conversation_id": "conv_789xyz",
  "title": "Query about Taxes in Mexico",
  "created_at": "2025-04-16T14:30:00Z"
}
```

### 3. Send a Chat Message

**Endpoint**: `POST /api/Chat`

**Headers**:
```
Content-Type: application/json
Authorization: Bearer <session_token>
```

**Payload**:
```json
{
  "query": "What are the tax implications of a merger between companies in Mexico?",
  "conversationReferenceId": "conv_789xyz",
  "isSensitiveInfo": false,
  "responseType": "Question"
}
```

**Response**:
```json
{
  "message_id": "msg_456def",
  "response": "The tax implications of a merger between companies in Mexico include several important aspects...",
  "sources": [
    {
      "title": "Income Tax Law",
      "url": "...",
      "snippet": "..."
    }
  ],
  "created_at": "2025-04-16T14:31:00Z"
}
```

### 4. Send a Chat Message with Sensitive Data Support

**Endpoint**: `POST /api/Chat/SensitiveDataSupport`

**Headers**:
```
Content-Type: application/json
Authorization: Bearer <session_token>
```

**Payload**:
```json
{
  "query": "Here is my tax information: [sensitive data]",
  "conversationReferenceId": "conv_789xyz",
  "isSensitiveInfo": true,
  "responseType": "Question",
  "chatHistory": [
    {
      "question": "I need help with my tax situation",
      "answers": [
        {
          "answer": "I'd be happy to help. What specific tax information would you like to discuss?",
          "isMessageLiked": true
        }
      ]
    }
  ]
}
```

**Response**:
```json
{
  "message_id": "msg_789ghi",
  "response": "I've reviewed the tax information you provided...",
  "sources": [
    {
      "title": "Tax Processing Guidelines",
      "url": "...",
      "snippet": "..."
    }
  ],
  "created_at": "2025-04-16T14:35:00Z"
}
```

### 5. Get Document Information

**Endpoint**: `GET /api/Documents/{documentGuid}/download`

**Headers**:
```
Authorization: Bearer <session_token>
```

**Response**:
Binary file content or document details depending on the document type.

### 6. Search for Messages

**Endpoint**: `POST /api/Messages/search`

**Headers**:
```
Content-Type: application/json
Authorization: Bearer <session_token>
```

**Payload**:
```json
{
  "searchText": "merger tax implications",
  "startDate": "2025-01-01T00:00:00Z"
}
```

**Response**:
```json
{
  "messages": [
    {
      "message_id": "msg_123abc",
      "conversation_id": "conv_456def",
      "content": "What are the tax implications of a merger between companies in Mexico?",
      "timestamp": "2025-03-15T10:30:00Z",
      "is_user_message": true
    },
    {
      "message_id": "msg_789ghi",
      "conversation_id": "conv_456def",
      "content": "The tax implications of a merger between companies in Mexico include...",
      "timestamp": "2025-03-15T10:30:15Z",
      "is_user_message": false,
      "documents": [
        {
          "document_id": "doc_123",
          "title": "Mexico Tax Guidelines 2025",
          "relevance": 0.92
        }
      ]
    }
  ],
  "total_count": 2
}
```

## Postman Collection

To facilitate testing, you can use the following Postman collection that includes all the necessary endpoints. You can copy and paste this JSON into Postman to import the collection:

```json
{
  "info": {
    "name": "GPO Core API Direct Access",
    "description": "Collection for direct access to the GPO Core API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "base_url",
      "value": "https://gpo-core-api.example.com",
      "type": "string"
    },
    {
      "key": "token",
      "value": "YOUR_SESSION_TOKEN_HERE",
      "type": "string"
    },
    {
      "key": "session_id",
      "value": "YOUR_SESSION_ID_HERE",
      "type": "string"
    },
    {
      "key": "conversation_id",
      "value": "YOUR_CONVERSATION_ID_HERE",
      "type": "string"
    }
  ],
  "item": [
    {
      "name": "1. Create Session",
      "request": {
        "method": "POST",
        "header": [
          {"key": "Content-Type", "value": "application/json"}
        ],
        "url": {
          "raw": "{{base_url}}/api/User/session",
          "host": ["{{base_url}}"],
          "path": ["api", "User", "session"]
        },
        "body": {
          "mode": "raw",
          "raw": "{\n  \"instance_id\": \"capital-edge-instance-001\",\n  \"user_id\": \"user-123\",\n  \"project_id\": \"project-456\",\n  \"environment\": \"development\"\n}"
        },
        "description": "Creates a new session with the GPO Core API"
      },
      "event": [
        {
          "listen": "test",
          "script": {
            "exec": [
              "var jsonData = JSON.parse(responseBody);",
              "pm.collectionVariables.set(\"token\", jsonData.token);",
              "pm.collectionVariables.set(\"session_id\", jsonData.session_id);",
              "console.log(\"Session token and ID saved in variables\");"
            ],
            "type": "text/javascript"
          }
        }
      ]
    },
    {
      "name": "2. Start Conversation",
      "request": {
        "method": "POST",
        "header": [
          {"key": "Content-Type", "value": "application/json"},
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Chat/start-conversation",
          "host": ["{{base_url}}"],
          "path": ["api", "Chat", "start-conversation"]
        },
        "body": {
          "mode": "raw",
          "raw": "{\n  \"session_id\": \"{{session_id}}\",\n  \"title\": \"Query about Taxes in Mexico\",\n  \"metadata\": {\n    \"source\": \"postman_example\",\n    \"purpose\": \"testing\"\n  }\n}"
        },
        "description": "Starts a new conversation"
      },
      "event": [
        {
          "listen": "test",
          "script": {
            "exec": [
              "var jsonData = JSON.parse(responseBody);",
              "pm.collectionVariables.set(\"conversation_id\", jsonData.conversation_id);",
              "console.log(\"Conversation ID saved in variables\");"
            ],
            "type": "text/javascript"
          }
        }
      ]
    },
    {
      "name": "3. Send Chat Message",
      "request": {
        "method": "POST",
        "header": [
          {"key": "Content-Type", "value": "application/json"},
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Chat",
          "host": ["{{base_url}}"],
          "path": ["api", "Chat"]
        },
        "body": {
          "mode": "raw",
          "raw": "{\n  \"query\": \"What are the tax implications of a merger between companies in Mexico?\",\n  \"conversationReferenceId\": \"{{conversation_id}}\",\n  \"isSensitiveInfo\": false,\n  \"responseType\": \"Question\"\n}"
        },
        "description": "Sends a chat message and gets a response"
      }
    },
    {
      "name": "4. Chat Message with Sensitive Data",
      "request": {
        "method": "POST",
        "header": [
          {"key": "Content-Type", "value": "application/json"},
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Chat/SensitiveDataSupport",
          "host": ["{{base_url}}"],
          "path": ["api", "Chat", "SensitiveDataSupport"]
        },
        "body": {
          "mode": "raw",
          "raw": "{\n  \"query\": \"Here is my tax information: [sensitive data]\",\n  \"conversationReferenceId\": \"{{conversation_id}}\",\n  \"isSensitiveInfo\": true,\n  \"responseType\": \"Question\",\n  \"chatHistory\": [\n    {\n      \"question\": \"I need help with my tax situation\",\n      \"answers\": [\n        {\n          \"answer\": \"I'd be happy to help. What specific tax information would you like to discuss?\",\n          \"isMessageLiked\": true\n        }\n      ]\n    }\n  ]\n}"
        },
        "description": "Sends a chat message with sensitive data and gets a response with enhanced security processing"
      }
    },
    {
      "name": "5. Get Chat History",
      "request": {
        "method": "GET",
        "header": [
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Chat/history?searchText=tax",
          "host": ["{{base_url}}"],
          "path": ["api", "Chat", "history"],
          "query": [
            {"key": "searchText", "value": "tax"}
          ]
        },
        "description": "Gets chat history with optional search filter"
      }
    },
    {
      "name": "6. Get Conversation Messages",
      "request": {
        "method": "GET",
        "header": [
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Chat/{{conversation_id}}/message",
          "host": ["{{base_url}}"],
          "path": ["api", "Chat", "{{conversation_id}}", "message"]
        },
        "description": "Gets all messages in a specific conversation"
      }
    },
    {
      "name": "7. Search Documents",
      "request": {
        "method": "GET",
        "header": [
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Documents",
          "host": ["{{base_url}}"],
          "path": ["api", "Documents"]
        },
        "description": "Gets all available documents"
      }
    },
    {
      "name": "8. Get Document Categories",
      "request": {
        "method": "GET",
        "header": [
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Documents/Documentcategories?documentCategoryType=Category",
          "host": ["{{base_url}}"],
          "path": ["api", "Documents", "Documentcategories"],
          "query": [
            {"key": "documentCategoryType", "value": "Category"}
          ]
        },
        "description": "Gets document categories"
      }
    },
    {
      "name": "9. Download Document",
      "request": {
        "method": "GET",
        "header": [
          {"key": "Authorization", "value": "Bearer {{token}}"}
        ],
        "url": {
          "raw": "{{base_url}}/api/Documents/00000000-0000-0000-0000-000000000000/download",
          "host": ["{{base_url}}"],
          "path": ["api", "Documents", "00000000-0000-0000-0000-000000000000", "download"]
        },
        "description": "Downloads a specific document (replace the UUID with an actual document ID)"
      }
    }
  ]
}
```

## Generating Payloads from the Client

The GPO Core Python Client includes a utility to generate payloads that you can use in Postman. This is useful when you want to ensure that your payload follows the exact format expected by the server.

```python
from gpo_client import GPOCoreClient
from gpo_client.models import SessionContext, ChatContext, ChatRequest
import json

# Function to generate payloads for Postman
def generate_payloads_for_postman():
    """Generates payloads that can be used directly in Postman"""
    
    # Initialize client (just to get the configuration, no API calls will be made)
    client = GPOCoreClient()
    base_url = client.config.base_url
    
    # 1. Payload for creating a session
    session_context = SessionContext(
        instance_id="capital-edge-instance-001",
        user_id="user-123",
        project_id="project-456"
    )
    
    session_payload = {
        "instance_id": session_context.instance_id,
        "user_id": session_context.user_id,
        "project_id": session_context.project_id,
        "environment": session_context.environment
    }
    
    print("=== CREATE SESSION ===")
    print(f"POST {base_url}/api/User/session")
    print("Headers:")
    print("Content-Type: application/json")
    print("\nBody:")
    print(json.dumps(session_payload, indent=2))
    print("\n")
    
    # For the remaining examples, we assume you've obtained a session token
    # You should replace this placeholder with an actual token from a session response
    token_example = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.example_token"
    session_id_example = "sess_abc123"
    
    # 2. Payload for starting a conversation
    conversation_payload = {
        "session_id": session_id_example,
        "title": "Test Query",
        "metadata": {
            "source": "postman_example",
            "region": "LATAM"
        }
    }
    
    print("=== START CONVERSATION ===")
    print(f"POST {base_url}/api/Chat/start-conversation")
    print("Headers:")
    print("Content-Type: application/json")
    print(f"Authorization: Bearer {token_example}")
    print("\nBody:")
    print(json.dumps(conversation_payload, indent=2))
    print("\n")
    
    # 3. Payload for a chat message
    conversation_id_example = "conv_xyz789"
    
    chat_request = ChatRequest(
        query="What are the tax implications of a merger between companies in Mexico?",
        use_openai=True,
        client_metadata={
            "source": "postman_direct_query",
            "version": "1.0"
        }
    )
    
    chat_payload = {
        "query": chat_request.query,
        "conversationReferenceId": conversation_id_example,
        "isSensitiveInfo": False,
        "responseType": "Question"
    }
    
    print("=== SEND CHAT MESSAGE ===")
    print(f"POST {base_url}/api/Chat")
    print("Headers:")
    print("Content-Type: application/json")
    print(f"Authorization: Bearer {token_example}")
    print("\nBody:")
    print(json.dumps(chat_payload, indent=2))
    
    # Returns all payloads for use in Postman
    return {
        "session": {
            "endpoint": f"{base_url}/api/User/session",
            "method": "POST",
            "headers": {"Content-Type": "application/json"},
            "body": session_payload
        },
        "conversation": {
            "endpoint": f"{base_url}/api/Chat/start-conversation",
            "method": "POST",
            "headers": {
                "Content-Type": "application/json",
                "Authorization": f"Bearer {token_example}"
            },
            "body": conversation_payload
        },
        "chat": {
            "endpoint": f"{base_url}/api/Chat",
            "method": "POST",
            "headers": {
                "Content-Type": "application/json",
                "Authorization": f"Bearer {token_example}"
            },
            "body": chat_payload
        }
    }
```

## Curl Commands

You can also use curl commands to interact with the API directly from the command line:

### 1. Create Session

```bash
curl -X POST \
  'https://gpo-core-api.example.com/api/User/session' \
  -H 'Content-Type: application/json' \
  -d '{
    "instance_id": "capital-edge-instance-001",
    "user_id": "user-123",
    "project_id": "project-456",
    "environment": "development"
  }'
```

### 2. Start Conversation

```bash
curl -X POST \
  'https://gpo-core-api.example.com/api/Chat/start-conversation' \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer YOUR_SESSION_TOKEN' \
  -d '{
    "session_id": "YOUR_SESSION_ID",
    "title": "Query about Taxes in Mexico",
    "metadata": {
      "source": "curl_example",
      "purpose": "testing"
    }
  }'
```

### 3. Send Chat Message

```bash
curl -X POST \
  'https://gpo-core-api.example.com/api/Chat' \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer YOUR_SESSION_TOKEN' \
  -d '{
    "query": "What are the tax implications of a merger between companies in Mexico?",
    "conversationReferenceId": "YOUR_CONVERSATION_ID",
    "isSensitiveInfo": false,
    "responseType": "Question"
  }'
```

### 4. Get Document Information

```bash
curl -X GET \
  'https://gpo-core-api.example.com/api/Documents/YOUR_DOCUMENT_GUID/download' \
  -H 'Authorization: Bearer YOUR_SESSION_TOKEN'
```

### 5. Search Messages

```bash
curl -X POST \
  'https://gpo-core-api.example.com/api/Messages/search' \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer YOUR_SESSION_TOKEN' \
  -d '{
    "searchText": "merger tax implications",
    "startDate": "2025-01-01T00:00:00Z"
  }'
```

## Security Considerations

When accessing the GPO Core API directly, keep in mind the following security considerations:

1. **Token protection**: JWT tokens are sensitive credentials and should be treated as such
2. **Isolated environments**: Use separate environments for testing and production
3. **Data cleansing**: Do not include personally identifiable information (PII) in test queries
4. **Activity logging**: Log all direct interactions with the API for auditing purposes
5. **Temporary use**: Use direct access only for testing or debugging, not as a permanent solution
6. **Sensitive data handling**: Use the SensitiveDataSupport endpoint for any queries containing sensitive information

## Troubleshooting

If you encounter issues when directly accessing the GPO Core API:

1. **Authentication issues**: Ensure your token hasn't expired - tokens typically have a limited lifetime
2. **Error responses**: Check the error message in the response body for specific details
3. **Rate limiting**: If you receive 429 (Too Many Requests) errors, reduce your request frequency
4. **Missing headers**: Verify all required headers are included, especially Authorization and Content-Type
5. **Payload format**: Double-check your JSON payload structure against the examples

For additional issues, refer to the [Troubleshooting](./troubleshooting.md) section or contact the support team.