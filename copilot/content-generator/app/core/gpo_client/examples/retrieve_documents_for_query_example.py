"""
Example script to retrieve documents for a query using the GPOCoreClient.
This script makes a real API call using centralized configuration.
"""

# Add the project root to sys.path for absolute imports when running as a script
import sys
import os
from datetime import datetime
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '../../../..')))

import asyncio
import logging
import argparse
from app.core.gpo_client.client import GPOCoreClient
from app.core.config import GPOConfig  # Import centralized config
from app.core.gpo_client.exceptions import GPOClientException

# Set up logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IkNOdjBPSTNSd3FsSEZFVm5hb01Bc2hDSDJYRSIsImtpZCI6IkNOdjBPSTNSd3FsSEZFVm5hb01Bc2hDSDJYRSJ9.eyJhdWQiOiJhcGk6Ly84ZGIyOTVlMC1lNDZlLTQzZGQtODhiMS1lMmU3YWQ2OTQ0ZTciLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC81Yjk3M2Y5OS03N2RmLTRiZWItYjI3ZC1hYTBjNzBiODQ4MmMvIiwiaWF0IjoxNzQ1NTk0OTg2LCJuYmYiOjE3NDU1OTQ5ODYsImV4cCI6MTc0NTYwMDU0NywiYWNyIjoiMSIsImFpbyI6IkFXUUFtLzhaQUFBQXJJR01taG0zSDlKb294OWIvTGdkTkZCeWRqVVZ3d1JaTyt5SjR5RndzdHJVLzFYUFEwNjFkUTlHaTN5TjdZbjEvSjAzQUcyNG8xRHdNWjlQZWFQR2lpbUs1MUx2R09iWnpVeDFqTmtPWENiekNjd3hFMmV0bWdzNlFEQlFtRStXIiwiYW1yIjpbInB3ZCIsInJzYSJdLCJhcHBpZCI6IjY0ZGI5MGMzLWM3MzktNGU5Zi1hZDk5LTY0NzA2NDg5YzY4MCIsImFwcGlkYWNyIjoiMCIsImRldmljZWlkIjoiYzlmNDBmZTItZTFmNy00NmNjLTgwY2UtN2U3MTQ2MmUxODI1IiwiZmFtaWx5X25hbWUiOiJVcmliZSIsImdpdmVuX25hbWUiOiJEaWVnbyIsImlwYWRkciI6Ijk4Ljk4LjI2LjkxIiwibmFtZSI6IkRpZWdvIFVyaWJlIiwib2lkIjoiNTM0YTM0NDAtMzllZC00ODY5LTg1YjQtNGM2YWFhNzE3NWFiIiwib25wcmVtX3NpZCI6IlMtMS01LTIxLTEwNzgwODE1MzMtMTEzMDA3NzE0LTcyNTM0NTU0My0zMTk0ODEiLCJyaCI6IjEuQVNFQW1ULVhXOTkzNjB1eWZhb01jTGhJTE9DVnNvMXU1TjFEaUxIaTU2MXBST2NoQVBzaEFBLiIsInNjcCI6InVzZXJfaW1wZXJzb25hdGlvbiIsInNpZCI6IjAwMjEyNDc5LTA2ZDQtODJkYS02OTkxLTA3MGU0NmNhNTZkNyIsInN1YiI6Il9YYk1MQ091VFBkZGcyREctUVRzLU1tbXBKRjRtaHdfd016Wk1lRjRERGciLCJ0aWQiOiI1Yjk3M2Y5OS03N2RmLTRiZWItYjI3ZC1hYTBjNzBiODQ4MmMiLCJ1bmlxdWVfbmFtZSI6IkRpZWdvLlVyaWJlQGV5LmNvbSIsInVwbiI6IkRpZWdvLlVyaWJlQGV5LmNvbSIsInV0aSI6IlhJalZxVzIxSWthOVRVdUFodVp2QUEiLCJ2ZXIiOiIxLjAifQ.LJj4xjvi9HipUShk6vr7qiLdQRtkYV6-2e-4Uq8ezfp9tuc4QjiAzZUdwOjWJvRIvAyjm86nYng-UQZSUEwAGB8ajWl1bQ2epgyd7_kHJCIQeANASb29mLkmLt_bi2Xzrk_c0mpbwP9OL2DgWqJKKw1L-g2VDltQFEZTnSxCRpQYYtNx6SxYMIh5ll7X-m9LUdQNYFTd8x4yIIZlbYUujLGn-Yczg4m-LGePdg0JFc7XsP0GVrOYvVbQD9ewQCn4VtADiLUmrAUg96s-k8MtHwqu4IiEiplsRTWGjJHRb3LomwEYkE_IRBt3sQLVVRI0-88FnBtxW6_06o-vs2l2OQ"  # Bearer token, burned in as requested

async def main():
    """
    Run a real query against the GPO API using the retrieve_documents_for_query method.

    # Default mode - using centralized configuration
    python -m app.core.gpo_client.examples.retrieve_documents_for_query_example

    # Debug mode - PDFs will be stored
    python -m app.core.gpo_client.examples.retrieve_documents_for_query_example --debug-pdf

    # Custom query with PDF storage
    python -m app.core.gpo_client.examples.retrieve_documents_for_query_example --debug-pdf --query "EY tax policies"
    
    # Using custom API key instead of key from KeyVault
    python -m app.core.gpo_client.examples.retrieve_documents_for_query_example --use-custom-api-key
    """
    # Parse command-line arguments
    parser = argparse.ArgumentParser(description='Retrieve documents for a query')
    parser.add_argument('--query', default="What is EY?", help='The query to search for')
    parser.add_argument('--debug-pdf', action='store_true', help='Enable PDF storage to filesystem')
    parser.add_argument('--base-url', help='Override the API base URL')
    parser.add_argument('--use-custom-api-key', action='store_true', help='Use the custom API key defined in this script')
    args = parser.parse_args()
    
    client = None
    
    try:
        # Create a centralized GPOConfig instance
        # The constructor will automatically get the API key from KeyVault if use_key_vault is True
        config = GPOConfig()
        
        # Apply command line overrides if provided
        if args.debug_pdf:
            config.debug_pdf_storage = True
            
        if args.base_url:
            config.base_url = args.base_url
        
        # Initialize client with centralized configuration and optional API key
        if args.use_custom_api_key:
            client = GPOCoreClient(config, api_key=API_KEY)
        else:
            client = GPOCoreClient(config)

        # Get the query from command-line args or use default
        query = args.query

        # Call the method and print the result
        result = await client.retrieve_documents_for_query(query)
        
        # Extract answer and document response
        answer = result.get('answer')
        document_query_response = result.get('document_query_response')
        
        # Log results
        logger.info(f"Found {len(document_query_response.documents)} documents for query: '{query}'")
        
        print("\n=== AI ANSWER ===")
        print(f"Message ID: {answer.message_id}")
        print(f"Content: {answer.content}")
        print(f"Created at: {answer.created_at}")
        
        print("\n=== DOCUMENTS ===")
        for idx, doc in enumerate(document_query_response.documents, 1):
            metadata = doc.get('metadata')
            content = doc.get('content')
            print(f"\nDocument {idx}:")
            print(f"Title: {metadata.title}")
            print(f"File: {metadata.filename}")
            print(f"GUID: {metadata.document_guid}")
            if metadata.categories:
                print(f"Categories: {', '.join(metadata.categories)}")
            if hasattr(content, 'document') and content.document:
                print(f"Content Preview: {content.document}")
                
                if config.debug_pdf_storage:
                    # Show the path where the PDF was saved
                    year_month = datetime.now().strftime("%Y-%m")
                    base_dir = os.path.dirname(os.path.abspath(__file__))
                    docs_path = os.path.join(base_dir, "..", "documents", year_month)
                    print(f"PDF saved to: {os.path.join(docs_path, metadata.document_guid + '.pdf')}")
        
        print(f"\nSearch time: {document_query_response.search_time_ms}ms")
    except GPOClientException as e:
        logger.error(f"API Client Error: {e}")
        print(f"Error: {e}")
    except Exception as e:
        logger.exception("Unexpected error.")
        print(f"Unexpected error: {e}")
    finally:
        if client:
            await client.close()

if __name__ == "__main__":
    asyncio.run(main())
