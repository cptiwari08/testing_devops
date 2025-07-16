"""
Authentication handler for the GPO Core API.

This module provides utilities for authentication and token management.
"""

import base64
import json
import logging
import time
from typing import Dict, Any, Optional
from datetime import datetime, timedelta

logger = logging.getLogger(__name__)

class AuthHandler:
    """Handler for GPO Core API authentication."""
    
    def __init__(self):
        """Initialize the authentication handler."""
        self.token_cache = {}
    
    def validate_token(self, token: str) -> bool:
        """
        Validate a JWT token.
        
        Args:
            token: JWT token to validate
            
        Returns:
            bool: Whether the token is valid
        """
        try:
            # Extract the claims
            claims = self.extract_token_claims(token)
            
            # Check expiration
            exp = claims.get('exp')
            if exp is not None:
                if datetime.fromtimestamp(exp) < datetime.now():
                    logger.warning("Token has expired")
                    return False
            
            # Token is valid
            return True
            
        except Exception as e:
            logger.error(f"Token validation failed: {str(e)}")
            return False
    
    def extract_token_claims(self, token: str) -> Dict[str, Any]:
        """
        Extract and validate claims from a JWT token.
        
        Args:
            token: JWT token
            
        Returns:
            Dict[str, Any]: Token claims
            
        Raises:
            ValueError: If token format is invalid
        """
        # JWT tokens are in format: header.payload.signature
        parts = token.split('.')
        if len(parts) != 3:
            raise ValueError("Invalid JWT token format")
            
        # Decode the payload (middle part)
        try:
            # Add padding if needed
            payload = parts[1]
            padding = '=' * (4 - len(payload) % 4) if len(payload) % 4 else ''
            payload += padding
            
            # Replace URL-safe chars and decode
            payload = payload.replace('-', '+').replace('_', '/')
            decoded = base64.b64decode(payload)
            
            return json.loads(decoded)
            
        except Exception as e:
            logger.error(f"Failed to decode token payload: {str(e)}")
            raise ValueError(f"Invalid JWT token payload: {str(e)}")
    
    def prepare_auth_header(self, token: str) -> Dict[str, str]:
        """
        Create authorization header with the provided token.
        
        Args:
            token: JWT token
            
        Returns:
            Dict[str, str]: Authorization header
        """
        return {'Authorization': f'Bearer {token}'}
    
    def is_token_expiring_soon(self, token: str, threshold_seconds: int = 300) -> bool:
        """
        Check if a token is expiring soon.
        
        Args:
            token: JWT token
            threshold_seconds: Number of seconds considered "soon"
            
        Returns:
            bool: Whether the token expires within the threshold
        """
        try:
            claims = self.extract_token_claims(token)
            exp = claims.get('exp')
            
            if exp is not None:
                expires_at = datetime.fromtimestamp(exp)
                threshold = timedelta(seconds=threshold_seconds)
                
                return datetime.now() + threshold >= expires_at
            
            # If no expiration claim, assume it's expiring soon to be safe
            return True
            
        except Exception:
            # If we can't check, assume it's expiring soon to be safe
            return True
    
    def cache_token(self, session_id: str, token: str) -> None:
        """
        Cache a token for future use.
        
        Args:
            session_id: Session identifier
            token: JWT token
        """
        try:
            claims = self.extract_token_claims(token)
            exp = claims.get('exp')
            
            self.token_cache[session_id] = {
                'token': token,
                'expires_at': datetime.fromtimestamp(exp) if exp else None
            }
            
        except Exception as e:
            logger.error(f"Failed to cache token: {str(e)}")
    
    def get_cached_token(self, session_id: str) -> Optional[str]:
        """
        Get a cached token if available and not expired.
        
        Args:
            session_id: Session identifier
            
        Returns:
            Optional[str]: Cached token or None if not available or expired
        """
        cache_entry = self.token_cache.get(session_id)
        if not cache_entry:
            return None
            
        token = cache_entry['token']
        expires_at = cache_entry['expires_at']
        
        if expires_at and expires_at <= datetime.now():
            # Token expired, remove from cache
            del self.token_cache[session_id]
            return None
            
        return token