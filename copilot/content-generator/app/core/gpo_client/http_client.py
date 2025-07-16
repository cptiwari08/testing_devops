"""
HTTP client for GPO Core API.

This module provides a low-level HTTP client for communicating with the GPO Core API.
"""

import json
import logging
import time
import asyncio
from typing import Dict, List, Optional, Any, Union, Tuple
import aiohttp
from dataclasses import asdict

from app.core.config import GPOConfig
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

logger = logging.getLogger(__name__)


class HttpClient:
    """
    HTTP client for GPO Core API.
    
    This class handles the HTTP communication with the GPO Core API,
    including request formatting and response parsing.
    """
    
    def __init__(self, config: GPOConfig):
        """
        Initialize the HTTP client.
        
        Args:
            config: Configuration for the client
        """
        self.config = config
        self.base_url = config.base_url.rstrip('/')
        self._session = None
        self._timeout = aiohttp.ClientTimeout(
            total=config.timeout,
            connect=config.timeout  # Using timeout for connect as well since connect_timeout_seconds doesn't exist
        )
        
    async def _get_session(self) -> aiohttp.ClientSession:
        """Get or create an aiohttp client session."""
        if self._session is None or self._session.closed:
            self._session = aiohttp.ClientSession(
                timeout=self._timeout,
                headers={"User-Agent": "GPOCoreClient"}
            )
        return self._session
    
    async def _close_session(self) -> None:
        """Close the aiohttp client session."""
        if self._session and not self._session.closed:
            await self._session.close()
            self._session = None
    
    def _build_url(self, endpoint: str) -> str:
        """
        Build a full URL from the endpoint.
        
        Args:
            endpoint: API endpoint path
            
        Returns:
            Full URL including base URL and endpoint
        """
        if endpoint.startswith(('http://', 'https://')):
            return endpoint
        
        # Ensure endpoint starts with /
        if not endpoint.startswith('/'):
            endpoint = f'/{endpoint}'
            
        return f"{self.base_url}{endpoint}"
    
    def _prepare_headers(
        self, 
        headers: Optional[Dict[str, str]] = None, 
        is_json: bool = True,
        is_binary: bool = False
    ) -> Dict[str, str]:
        """
        Prepare headers for a request.
        
        Args:
            headers: Optional custom headers
            is_json: Whether the request body is JSON
            is_binary: Whether to expect binary response
            
        Returns:
            Headers dictionary
        """
        result = headers or {}
        
        # Add content-type for JSON requests
        if is_json and "Content-Type" not in result:
            result["Content-Type"] = "application/json"
            
        # Add accept header for binary responses
        if is_binary and "Accept" not in result:
            result["Accept"] = "*/*"
        elif not is_binary and "Accept" not in result:
            result["Accept"] = "application/json"
            
        # Add API key if available
        if self.config.api_key and "Authorization" not in result:
            result["Authorization"] = f"Bearer {self.config.api_key}"
            
        return result
    
    def _prepare_request_data(self, data: Any) -> Union[str, Dict, bytes]:
        """
        Prepare request data for sending.
        
        Args:
            data: Request data to prepare
            
        Returns:
            Prepared data ready for sending
        """
        if data is None:
            return None
            
        # If it's already a string or bytes, return as-is
        if isinstance(data, (str, bytes)):
            return data
            
        # If it's a dict, just return it
        if isinstance(data, dict):
            return data
            
        # If it has a dict method (like our models), use it
        if hasattr(data, 'dict') and callable(data.dict):
            return data.dict()
            
        # If it's a dataclass, convert to dict
        if hasattr(data, '__dataclass_fields__'):
            return asdict(data)
            
        # Otherwise, try to convert to a dictionary
        try:
            return dict(data)
        except (TypeError, ValueError):
            # Last resort: try to JSON serialize
            return json.dumps(data)
    
    async def request(
        self,
        method: str,
        endpoint: str,
        params: Optional[Dict[str, Any]] = None,
        data: Optional[Any] = None,
        headers: Optional[Dict[str, str]] = None,
        is_binary: bool = False,
        retry_count: int = 0
    ) -> Any:
        """
        Send a request to the API.
        
        Args:
            method: HTTP method (GET, POST, etc.)
            endpoint: API endpoint path
            params: Optional query parameters
            data: Optional request body
            headers: Optional request headers
            is_binary: Whether to expect binary response
            retry_count: Current retry attempt number
            
        Returns:
            Response data (JSON-parsed dict or binary data)
            
        Raises:
            AuthenticationError: If authentication fails
            ConnectionError: If a connection error occurs
            TimeoutError: If the request times out
            InvalidRequestError: If the request is invalid
            RateLimitError: If rate limit is exceeded
            ServiceUnavailableError: If the service is unavailable
            GPOClientException: For any other API error
        """
        url = self._build_url(endpoint)
        prepared_headers = self._prepare_headers(headers, not is_binary, is_binary)
        prepared_data = None
        
        # Prepare data if needed
        if data is not None:
            prepared_data = self._prepare_request_data(data)
            
        session = await self._get_session()
        start_time = time.time()
        
        try:
            async with session.request(
                method=method,
                url=url,
                params=params,
                json=prepared_data if isinstance(prepared_data, (dict, list)) and not is_binary else None,
                data=prepared_data if not isinstance(prepared_data, (dict, list)) or is_binary else None,
                headers=prepared_headers,
                ssl=None if self.config.verify_ssl else False
            ) as response:
                duration_ms = int((time.time() - start_time) * 1000)
                
                # Log the request
                logger.debug(
                    f"API {method} {url} completed in {duration_ms}ms with status {response.status}"
                )
                
                # Handle binary response
                if is_binary and response.status == 200:
                    content = await response.read()
                    return {
                        "content": content,
                        "content_type": response.headers.get("Content-Type", "application/octet-stream")
                    }
                
                # For non-binary responses, always try to parse as JSON first
                try:
                    response_json = await response.json()
                except (json.JSONDecodeError, aiohttp.ContentTypeError):
                    # If not JSON, get the text
                    response_text = await response.text()
                    response_json = {"message": response_text}
                
                # Handle error responses
                if response.status >= 400:
                    await self._handle_error_response(response.status, response_json, endpoint)
                    
                return response_json
                
        except aiohttp.ClientConnectorError as e:
            # Connection error (DNS resolution failed, etc.)
            logger.error(f"Connection error for {url}: {str(e)}")
            
            if retry_count < self.config.max_retries:
                logger.info(f"Retrying request to {url}, attempt {retry_count + 1}/{self.config.max_retries}")
                await asyncio.sleep(self.config.retry_backoff_factor * (2 ** retry_count))
                return await self.request(
                    method, endpoint, params, data, headers, is_binary, retry_count + 1
                )
            
            raise ConnectionError(f"Failed to connect to {url}: {str(e)}")
            
        except aiohttp.ClientResponseError as e:
            logger.error(f"Response error for {url}: {str(e)}")
            raise GPOClientException(f"API response error: {str(e)}")
            
        except aiohttp.ClientPayloadError as e:
            logger.error(f"Payload error for {url}: {str(e)}")
            
            if retry_count < self.config.max_retries:
                logger.info(f"Retrying request to {url}, attempt {retry_count + 1}/{self.config.max_retries}")
                await asyncio.sleep(self.config.retry_backoff_factor * (2 ** retry_count))
                return await self.request(
                    method, endpoint, params, data, headers, is_binary, retry_count + 1
                )
            
            raise ConnectionError(f"Failed to receive data from {url}: {str(e)}")
            
        except asyncio.TimeoutError as e:
            logger.error(f"Timeout error for {url}: {str(e)}")
            
            if retry_count < self.config.max_retries:
                logger.info(f"Retrying request to {url}, attempt {retry_count + 1}/{self.config.max_retries}")
                await asyncio.sleep(self.config.retry_backoff_factor * (2 ** retry_count))
                return await self.request(
                    method, endpoint, params, data, headers, is_binary, retry_count + 1
                )
            
            raise TimeoutError(f"Request to {url} timed out after {self.config.timeout} seconds")
            
        except Exception as e:
            logger.error(f"Unexpected error for {url}: {str(e)}")
            raise GPOClientException(f"Unexpected error: {str(e)}")
    
    async def _handle_error_response(
        self,
        status_code: int,
        response_data: Dict[str, Any],
        endpoint: str
    ) -> None:
        """
        Handle an error response from the API.
        
        Args:
            status_code: HTTP status code
            response_data: Response data
            endpoint: API endpoint path
            
        Raises:
            AuthenticationError: If authentication fails (401)
            InvalidRequestError: If the request is invalid (400, 404)
            RateLimitError: If rate limit is exceeded (429)
            ServiceUnavailableError: If the service is unavailable (503)
            DocumentNotFoundError: If a document is not found (404 on document endpoints)
            DocumentAccessError: If document access fails (403 on document endpoints)
            GPOClientException: For any other API error
        """
        error_message = response_data.get("message", "Unknown error")
        error_code = response_data.get("code", "unknown")
        
        # Log the full error response for debugging
        logger.debug(f"API Error Response [{status_code}] for {endpoint}: {response_data}")
        
        # Handle different status codes
        if status_code == 401:
            # Log detailed information about auth failure
            logger.error(f"Authentication failed for {endpoint}: {error_message}")
            logger.error(f"Auth error details: {response_data}")
            raise AuthenticationError(f"Authentication failed: {error_message}")
            
        elif status_code == 400:
            raise InvalidRequestError(f"Invalid request: {error_message}")
            
        elif status_code == 404:
            # Check if this is a document endpoint
            if "/document" in endpoint.lower() or "/documents" in endpoint.lower():
                raise DocumentNotFoundError(f"Document not found: {error_message}")
            else:
                raise InvalidRequestError(f"Resource not found: {error_message}")
                
        elif status_code == 403:
            # Check if this is a document endpoint
            if "/document" in endpoint.lower() or "/documents" in endpoint.lower():
                raise DocumentAccessError(f"Document access denied: {error_message}")
            else:
                logger.error(f"Access denied for {endpoint}: {error_message}")
                logger.error(f"Authorization error details: {response_data}")
                raise AuthenticationError(f"Access denied: {error_message}")
                
        elif status_code == 429:
            raise RateLimitError(f"Rate limit exceeded: {error_message}")
            
        elif status_code == 503:
            raise ServiceUnavailableError(f"Service unavailable: {error_message}")
            
        else:
            logger.error(f"Unexpected API error ({status_code}) for {endpoint}: {response_data}")
            raise GPOClientException(f"API error ({status_code}): {error_message}")