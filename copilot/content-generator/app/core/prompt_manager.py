from typing import Optional, Dict, Any
from abc import ABC, abstractmethod
import asyncio
from pathlib import Path

import aiohttp
import yaml
from pydantic import ValidationError

from app.core.config import PromptManagerConfig
from app.core.errors import PromptValidationError
from app.core.interfaces import IBaseLogger
from app.core.schemas import PromptTemplate


class NullLogger:
    """A null logger implementation that silently ignores all log messages."""
    
    def info(self, message: str) -> None:
        pass
    
    def error(self, message: str) -> None:
        pass
    
    def warning(self, message: str) -> None:
        pass
    
    def debug(self, message: str) -> None:
        pass


class BasePromptManager(ABC):
    """
    Abstract base class for prompt managers that defines the common interface.
    """
    
    def __init__(self, logger: Optional[IBaseLogger] = None) -> None:
        """
        Initialize the BasePromptManager.
        
        Args:
            logger: Logger instance for logging messages. If None, a NullLogger is used.
        """
        self._logger = logger if logger is not None else NullLogger()
        self._session = None
        self._config = PromptManagerConfig()
    
    @abstractmethod
    async def get_prompt(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt asynchronously using the manager's strategy.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw data without validation
            
        Returns:
            The formatted prompt as a string or raw data when raw_data=True
        """
        pass
    
    @abstractmethod
    def get_prompt_sync(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt synchronously using the manager's strategy.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw data without validation
            
        Returns:
            The formatted prompt as a string or raw data when raw_data=True
        """
        pass
    
    def _process_prompt_data(self, data: Any, raw_data: bool = False) -> Any:
        """
        Process prompt data based on the raw_data flag.
        
        Args:
            data: The data to process (typically from YAML or API)
            raw_data: If True, returns the raw data without validation
            
        Returns:
            The processed data - either raw or formatted as a prompt
            
        Raises:
            PromptValidationError: If there is a validation error when creating the PromptTemplate
        """
        # Return raw data without template validation if specified
        if raw_data:
            return data
            
        try:
            data = {"text": data} if isinstance(data, str) else data
            prompt_template = PromptTemplate(**data)
        except ValidationError as e:
            raise PromptValidationError(f"Error loading prompt template: {e}")

        result = prompt_template.text + "\n"
        if prompt_template.few_shots:
            for shot in prompt_template.few_shots:
                result += shot + "\n"

        return result
    
    async def _get_session(self) -> aiohttp.ClientSession:
        """Get or create an HTTP session for API calls."""
        if self._session is None:
            self._session = aiohttp.ClientSession()
        return self._session
    
    async def close(self) -> None:
        """Close the HTTP session when done."""
        if self._session:
            await self._session.close()
            self._session = None
    
    def map_agent_to_api_format(self, agent: str) -> str:
        """
        Maps a system agent name to the corresponding API agent name.
        
        Args:
            agent: The system agent name (e.g., 'pmo_workplan')
            
        Returns:
            The API agent name (e.g., 'WORKPLAN_GENERATOR') or empty string if not found
        """
        agent_mapping = {
            "pmo_workplan": "WORKPLAN_GENERATOR",
            # Add additional mappings as needed
        }
        
        return agent_mapping.get(agent.lower(), "")


class APIPromptManager(BasePromptManager):
    """
    Prompt manager implementation that retrieves prompts exclusively from an API.
    """
    
    async def get_prompt(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt from the API asynchronously.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw response without additional processing
            
        Returns:
            The formatted prompt as a string or raw data when raw_data=True
            
        Raises:
            ValueError: If the prompt cannot be retrieved
        """
        prompt_parameters = prompt_parameters or {}
        
        self._logger.info(f"Getting prompt '{key}' from API for agent '{agent}'")
        prompt = await self.get_prompt_from_api(agent, key, raw_data)
        
        if not prompt:
            raise ValueError(f"Could not retrieve prompt '{key}' from API")
        
        self._logger.info(f"Prompt '{key}' retrieved from API")
        
        # Format the prompt with the provided prompt_parameters if not raw_data
        if prompt_parameters and not raw_data:
            prompt = prompt.format(**prompt_parameters)
            
        return prompt
    
    def get_prompt_sync(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt from the API synchronously.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw response without additional processing
            
        Returns:
            The formatted prompt as a string or raw data when raw_data=True
            
        Raises:
            ValueError: If the prompt cannot be retrieved
        """
        prompt_parameters = prompt_parameters or {}
        
        self._logger.info(f"Getting prompt '{key}' from API synchronously for agent '{agent}'")
        
        # Run the async method in a synchronous context
        try:
            loop = asyncio.get_event_loop()
        except RuntimeError:
            # If no event loop exists in current thread, create one
            loop = asyncio.new_event_loop()
            asyncio.set_event_loop(loop)
        
        try:
            prompt = loop.run_until_complete(self.get_prompt_from_api(agent, key, raw_data))
        except Exception as e:
            self._logger.error(f"Error getting prompt synchronously: {e}")
            raise ValueError(f"Could not retrieve prompt '{key}' from API: {e}") from e
        
        if not prompt:
            raise ValueError(f"Could not retrieve prompt '{key}' from API")
        
        self._logger.info(f"Prompt '{key}' retrieved from API synchronously")
        
        # Format the prompt with the provided prompt_parameters if not raw_data
        if prompt_parameters and not raw_data:
            prompt = prompt.format(**prompt_parameters)
            
        return prompt
    
    async def get_prompt_from_api(self, agent: str, key: str, raw_data: bool = False) -> Optional[Any]:
        """
        Get a prompt from the API.
        
        Args:
            key: The key to identify the prompt
            agent: The agent name to use for retrieving the prompt
            raw_data: If True, returns the raw API response without processing
            
        Returns:
            The prompt as a string, raw API data, or None if not found
        """
        session = await self._get_session()
        headers = {}
        if self._config.ce_po_api_key:
            headers["ce-api-key"] = self._config.ce_po_api_key

        api_agent = self.map_agent_to_api_format(agent)
        
        payload: Dict[str, Any] = {"Key": key.upper(), "Agent": api_agent}
        
        async with session.post(
            f"{self._config.api_url}/GetByQuery", 
            json=payload,
            headers=headers
        ) as response:
            if response.status == 200:
                request = await response.json()
                if request and len(request) > 0:
                    # Otherwise extract and process the prompt content
                    if "prompt" in request[0]:
                        return self._process_prompt_data(request[0]["prompt"], raw_data)
                    elif "originalPrompt" in request[0]:
                        return self._process_prompt_data(request[0]["originalPrompt"], raw_data)
            
            return None


class YAMLPromptManager(BasePromptManager):
    """
    Prompt manager implementation that retrieves prompts exclusively from YAML files.
    """
    
    async def get_prompt(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt from a YAML file asynchronously.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw YAML data without validation
            
        Returns:
            The formatted prompt as a string or raw data when raw_data=True
            
        Raises:
            ValueError: If the prompt cannot be retrieved
        """
        # Since YAML loading is synchronous, we can just call the sync method
        return self.get_prompt_sync(agent, key, prompt_parameters, raw_data)
    
    def get_prompt_sync(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt from a YAML file synchronously.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw YAML data without validation (for dictionaries like acronyms)
            
        Returns:
            The formatted prompt as a string or raw data from YAML file when raw_data=True
            
        Raises:
            ValueError: If the prompt cannot be retrieved
        """
        prompt_parameters = prompt_parameters or {}
        
        self._logger.info(f"Getting prompt '{key}' from YAML for agent '{agent}'")
        prompt = self.get_prompt_from_yaml(agent, key, raw_data)
        
        self._logger.info(f"Prompt '{key}' retrieved from YAML file")
        
        # Format the prompt with the provided prompt_parameters
        if prompt_parameters and not raw_data:
            prompt = prompt.format(**prompt_parameters)
            
        return prompt
    
    def get_prompt_from_yaml(self, agent: str, key: str, raw_data: bool = False) -> Any:
        """
        Get a prompt from a YAML file.
        
        Args:
            key: The key to identify the prompt
            agent: The agent name to use for retrieving the prompt
            raw_data: If True, returns the raw YAML data without validation (for dictionaries like acronyms)
            
        Returns:
            The formatted prompt as a string or raw data from YAML file when raw_data=True
            
        Raises:
            PromptValidationError: If there is a validation error when creating the PromptTemplate
        """
        yaml_path = Path("app") / agent / "templates" / f"{key}.yaml"

        try:
            with open(yaml_path, "r") as file:
                data = yaml.safe_load(file)
        except FileNotFoundError:
            # Alternative path
            yaml_path = list((Path("app") / agent).rglob(f"*{key}.yaml"))[0]
            with open(yaml_path, "r") as file:
                data = yaml.safe_load(file)
        with open(yaml_path, "r") as file:
            data = yaml.safe_load(file)

        return self._process_prompt_data(data, raw_data)


class HybridPromptManager(APIPromptManager, YAMLPromptManager):
    """
    Prompt manager implementation that tries API first, then falls back to YAML.
    This implementation inherits methods from both API and YAML managers.
    """
    
    async def get_prompt(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt either from the API or from a YAML file asynchronously.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt in the API
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw data without validation
            
        Returns:
            The formatted prompt as a string or raw data when raw_data=True
            
        Raises:
            ValueError: If the prompt cannot be retrieved from any source
        """
        prompt = None
        prompt_parameters = prompt_parameters or {}
        
        # Try to get from API first
        try:
            self._logger.info(f"Getting prompt '{key}' from API for agent '{agent}'")
            prompt = await self.get_prompt_from_api(agent, key, raw_data)
            if prompt:
                self._logger.info(f"Prompt '{key}' retrieved from API")
        except Exception as e:
            self._logger.error(f"Error getting prompt '{key}' from API: {e}")
        
        # Fall back to YAML if needed
        if not prompt:
            self._logger.info(f"Getting prompt '{key}' from YAML for agent '{agent}'")
            prompt = self.get_prompt_from_yaml(agent, key, raw_data)
            self._logger.info(f"Prompt '{key}' retrieved from YAML file")
        
        if not prompt:
            raise ValueError(f"Could not retrieve prompt '{key}' from any source")
            
        # Format the prompt with the provided prompt_parameters if not raw_data
        if prompt_parameters and not raw_data:
            prompt = prompt.format(**prompt_parameters)
            
        return prompt
    
    def get_prompt_sync(self, agent: str, key: str, prompt_parameters: dict = None, raw_data: bool = False) -> Any:
        """
        Get a prompt either from the API or from a YAML file synchronously.
        
        Args:
            agent: The agent name (e.g., 'ey_ip')
            key: The key to identify the prompt in the API
            prompt_parameters: Dictionary of parameters to format the prompt with
            raw_data: If True, returns the raw data without validation
            
        Returns:
            The formatted prompt as a string or raw data when raw_data=True
            
        Raises:
            ValueError: If the prompt cannot be retrieved from any source
        """
        prompt = None
        prompt_parameters = prompt_parameters or {}
        
        # Try to get from API first
        try:
            self._logger.info(f"Getting prompt '{key}' from API synchronously for agent '{agent}'")    
            try:
                loop = asyncio.get_event_loop()
            except RuntimeError:
                loop = asyncio.new_event_loop()
                asyncio.set_event_loop(loop)
                
            prompt = loop.run_until_complete(self.get_prompt_from_api(agent, key, raw_data))
            if prompt:
                self._logger.info(f"Prompt '{key}' retrieved from API synchronously")
        except Exception as e:
            self._logger.error(f"Error getting prompt '{key}' from API synchronously: {e}")
        
        # Fall back to YAML if needed
        if not prompt:
            self._logger.info(f"Getting prompt '{key}' from YAML for agent '{agent}'")
            prompt = self.get_prompt_from_yaml(agent, key, raw_data)
            self._logger.info(f"Prompt '{key}' retrieved from YAML file")
        
        if not prompt:
            raise ValueError(f"Could not retrieve prompt '{key}' from any source")
            
        # Format the prompt with the provided prompt_parameters if not raw_data
        if prompt_parameters and not raw_data:
            prompt = prompt.format(**prompt_parameters)
            
        return prompt


def create_prompt_manager(logger: Optional[IBaseLogger] = None) -> BasePromptManager:
    """
    Factory function to create the appropriate PromptManager based on configuration.
    
    Args:
        logger: Logger instance for logging messages. Can be None.
        
    Returns:
        An instance of a BasePromptManager implementation
    """
    config = PromptManagerConfig()
    
    # By default, use the hybrid manager (for backward compatibility)
    if not hasattr(config, 'strategy') or config.strategy == 'hybrid':
        return HybridPromptManager(logger)
    elif config.strategy == 'api':
        return APIPromptManager(logger)
    elif config.strategy == 'yaml':
        return YAMLPromptManager(logger)
    else:
        # Default fallback
        return HybridPromptManager(logger)