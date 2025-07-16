"""
Exceptions for the GPO Core API client.

This module defines custom exceptions used throughout the client.
"""

class GPOClientException(Exception):
    """Base exception for all client errors."""
    pass

class AuthenticationError(GPOClientException):
    """Raised when authentication fails."""
    pass

class InvalidRequestError(GPOClientException):
    """Raised when a request has invalid parameters or format."""
    pass

class ConnectionError(GPOClientException):
    """Raised when a network connection issue occurs."""
    pass

class TimeoutError(GPOClientException):
    """Raised when a request times out."""
    pass

class RateLimitError(GPOClientException):
    """Raised when API rate limits are exceeded."""
    pass

class ServiceUnavailableError(GPOClientException):
    """Raised when the API service is unavailable."""
    pass

class DocumentNotFoundError(GPOClientException):
    """Raised when a requested document cannot be found."""
    pass

class DocumentAccessError(GPOClientException):
    """Raised when there's an issue accessing a document."""
    pass