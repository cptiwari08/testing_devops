import logging
import os
import sys
import time
import traceback
from contextlib import contextmanager
from typing import Any, Dict, Optional, List, Tuple

from app.core.config import Logging as LoggingConfig
from app.core.interfaces import IBaseLogger
from opencensus.ext.azure.log_exporter import AzureLogHandler


class Logger(IBaseLogger):
    """
    A class used for logging messages.
    This class provides methods for logging debug and info messages. It uses the standard Python
    logging module and also sends logs to Azure Application Insights.
    """

    def __init__(self):
        self._logger = logging.getLogger("my_logger")
        self._logger.setLevel(logging.DEBUG)  # Always set base level to DEBUG to capture everything
        self._properties = {"custom_dimensions": {}}
        self._instance_id = None
        self._chat_id = None
        self._project_friendly_id = None
        # Initialize log history storage
        self._log_history = []
        uvicorn_logger = logging.getLogger("uvicorn")
        
        # Determine if terminal display should be enabled (for pytest and debugging)
        self._show_in_terminal = os.getenv("SHOW_LOGS_IN_TERMINAL", "").lower() in {"true", "1", "yes"}
        self._terminal_log_level = os.getenv("TERMINAL_LOG_LEVEL", "DEBUG").upper()
        
        levels = {
            "DEBUG": logging.DEBUG,
            "INFO": logging.INFO,
            "WARNING": logging.WARNING,
            "ERROR": logging.ERROR,
            "CRITICAL": logging.CRITICAL,
            "NONE": logging.CRITICAL + 1,
        }
        
        # Get console log level from environment variable, default to DEBUG for development
        console_log_level = os.getenv("CONSOLE_LOG_LEVEL", "DEBUG").upper()
        if console_log_level not in levels:
            console_log_level = "DEBUG"  # Fallback to DEBUG if invalid level specified
            
        if not self._logger.handlers:
            # Check if uvicorn has any handlers, and use its formatter if available
            if uvicorn_logger.handlers:
                formatter = uvicorn_logger.handlers[0].formatter
            else:
                # Define a default formatter with colors for better visibility in terminal
                formatter = logging.Formatter(
                    "%(asctime)s - %(name)s - \033[1;36m%(levelname)s\033[0m - %(message)s"
                )

            # set console logging with potentially different level than app insights
            console_handler = logging.StreamHandler(stream=sys.stdout)
            console_handler.setFormatter(formatter)
            console_handler.setLevel(levels[console_log_level])  # Use environment variable level
            self._logger.addHandler(console_handler)
            
            # Add special pytest-compatible terminal handler with colored output
            if self._show_in_terminal:
                # Create a more visible formatter for terminal output during pytest/debug
                terminal_formatter = logging.Formatter(
                    "\033[1;33m%(asctime)s\033[0m - \033[1;35m%(name)s\033[0m - \033[1;36m%(levelname)s\033[0m - \033[1;37m%(message)s\033[0m"
                )
                # This handler will show logs directly in terminal even during pytest
                terminal_handler = logging.StreamHandler(stream=sys.stderr)
                terminal_handler.setFormatter(terminal_formatter)
                terminal_handler.setLevel(levels[self._terminal_log_level])
                self._logger.addHandler(terminal_handler)
                self._logger.debug("Terminal logging enabled at level: " + self._terminal_log_level)
            
            # Add a debug file handler for comprehensive logging
            try:
                debug_log_dir = os.getenv("DEBUG_LOG_DIR", "logs")
                os.makedirs(debug_log_dir, exist_ok=True)
                debug_file_handler = logging.FileHandler(f"{debug_log_dir}/debug.log")
                debug_file_handler.setLevel(logging.DEBUG)
                debug_file_handler.setFormatter(logging.Formatter(
                    "%(asctime)s - %(name)s - %(levelname)s - %(message)s"
                ))
                self._logger.addHandler(debug_file_handler)
            except Exception as e:
                # Don't fail initialization if log file can't be created
                print(f"Warning: Could not create debug log file: {str(e)}")

            # Useful for local development, set ENABLE_APP_INSIGHTS
            # to other value instead of true and logs are not going
            # to be send to app insights, useful for local development
            # additionally you can set instance and chat id to be
            # empty strings when logging to app insights is disabled
            #  helping a lot with logging on local environments
            if os.getenv("ENABLE_APP_INSIGHTS", "true").lower() in {"true"}:
                # app insights handler
                connection_string = f"InstrumentationKey={LoggingConfig.app_insights_instrumentation_key}"
                app_insights_handler = AzureLogHandler(
                    connection_string=connection_string
                )
                # App Insights gets the level from configuration
                app_insights_handler.setLevel(levels[LoggingConfig.level.upper()])
                self._logger.addHandler(app_insights_handler)
                
        # Log initialization of the logger
        self._logger.debug("Logger initialized with console level: " + console_log_level)
        # Add to history
        self._add_to_history("DEBUG", "Logger initialized with console level: " + console_log_level)

    def _format_message(self, message: str) -> str:
        """Format message with identifiers if they exist."""
        if self._instance_id or self._chat_id:
            return f"[instanceId:{self._instance_id}] [chatId:{self._chat_id}] [projectFriendlyId:{self._project_friendly_id}] {message}"
        return message
    
    def _add_to_history(self, level: str, message: str, extra_dims: Optional[Dict[str, Any]] = None):
        """
        Add a log entry to the internal history.
        
        Args:
            level: Log level (DEBUG, INFO, etc.)
            message: Message to store
            extra_dims: Optional additional dimensions to store
        """
        timestamp = time.strftime("%Y-%m-%d %H:%M:%S")
        log_entry = {
            "timestamp": timestamp,
            "level": level,
            "message": message,
            "extra_dims": extra_dims or {}
        }
        self._log_history.append(log_entry)
    
    def _print_to_terminal(self, level: str, message: str, extra_dims: Optional[Dict[str, Any]] = None):
        """
        Print log message directly to terminal when enabled, useful for pytest and debugging.
        This provides more visible output even when pytest captures stdout/stderr.
        
        Args:
            level: Log level (DEBUG, INFO, etc.)
            message: Message to display
            extra_dims: Optional additional dimensions to display
        """
        if not self._show_in_terminal:
            return
            
        # Get terminal level as int for comparison
        levels = {
            "DEBUG": 10,
            "INFO": 20,
            "WARNING": 30,
            "ERROR": 40,
            "CRITICAL": 50
        }
        msg_level = levels.get(level, 0)
        terminal_level = levels.get(self._terminal_log_level, 10)
        
        # Only print if message level is >= terminal level
        if msg_level < terminal_level:
            return
            
        # Color mapping for different log levels
        colors = {
            "DEBUG": "\033[1;34m",  # Blue
            "INFO": "\033[1;32m",   # Green
            "WARNING": "\033[1;33m", # Yellow
            "ERROR": "\033[1;31m",   # Red
            "CRITICAL": "\033[1;37;41m", # White on Red background
        }
        reset = "\033[0m"
        timestamp = time.strftime("%Y-%m-%d %H:%M:%S")
        
        # Basic colored output
        colored_output = f"{colors.get(level, '')}{timestamp} - {level} - {message}{reset}"
        
        # Add extra dimensions in a readable format if provided
        if extra_dims:
            extra_str = " | " + " | ".join(f"{k}={v}" for k, v in extra_dims.items())
            colored_output += f"{colors.get('INFO', '')}{extra_str}{reset}"
        
        # Print directly to stderr (works better with pytest)
        print(colored_output, file=sys.stderr, flush=True)

    def debug(self, message: str, extra_dims: Optional[Dict[str, Any]] = None) -> None:
        """
        Log a debug message with optional additional dimensions.
        
        Args:
            message: The message to log
            extra_dims: Optional additional custom dimensions to include
        """
        formatted_message = self._format_message(message)
        props = self._get_properties_with_extras(extra_dims)
        self._logger.debug(formatted_message, extra=props)
        self._print_to_terminal("DEBUG", formatted_message, extra_dims)
        self._add_to_history("DEBUG", formatted_message, extra_dims)

    def info(self, message: str, extra_dims: Optional[Dict[str, Any]] = None) -> None:
        """
        Log an info message with optional additional dimensions.
        
        Args:
            message: The message to log
            extra_dims: Optional additional custom dimensions to include
        """
        formatted_message = self._format_message(message)
        props = self._get_properties_with_extras(extra_dims)
        self._logger.info(formatted_message, extra=props)
        self._print_to_terminal("INFO", formatted_message, extra_dims)
        self._add_to_history("INFO", formatted_message, extra_dims)

    def warning(self, message: str, extra_dims: Optional[Dict[str, Any]] = None) -> None:
        """
        Log a warning message with optional additional dimensions.
        
        Args:
            message: The message to log
            extra_dims: Optional additional custom dimensions to include
        """
        formatted_message = self._format_message(message)
        props = self._get_properties_with_extras(extra_dims)
        self._logger.warning(formatted_message, extra=props)
        self._print_to_terminal("WARNING", formatted_message, extra_dims)
        self._add_to_history("WARNING", formatted_message, extra_dims)

    def error(self, message: str, exc_info: bool = False, extra_dims: Optional[Dict[str, Any]] = None) -> None:
        """
        Log an error message with optional exception info and additional dimensions.
        
        Args:
            message: The message to log
            exc_info: Whether to include exception info
            extra_dims: Optional additional custom dimensions to include
        """
        formatted_message = self._format_message(message)
        props = self._get_properties_with_extras(extra_dims)
        
        # Include exception details if available and requested
        exception_info = ""
        if exc_info and sys.exc_info()[0]:
            exception_info = traceback.format_exc()
            self._logger.error(formatted_message, exc_info=True, extra=props)
            # Add exception stack trace to terminal output
            self._print_to_terminal("ERROR", f"{formatted_message}\n{exception_info}", extra_dims)
        else:
            self._logger.error(formatted_message, extra=props)
            self._print_to_terminal("ERROR", formatted_message, extra_dims)
            
        # Add to history with exception info if available
        error_dims = dict(extra_dims or {})
        if exception_info:
            error_dims["exception_info"] = exception_info
        self._add_to_history("ERROR", formatted_message, error_dims)

    def critical(self, message: str, exc_info: bool = True, extra_dims: Optional[Dict[str, Any]] = None) -> None:
        """
        Log a critical message with optional exception info and additional dimensions.
        
        Args:
            message: The message to log
            exc_info: Whether to include exception info
            extra_dims: Optional additional custom dimensions to include
        """
        formatted_message = self._format_message(message)
        props = self._get_properties_with_extras(extra_dims)
        
        # Always include exception details for critical errors unless explicitly disabled
        exception_info = ""
        if exc_info and sys.exc_info()[0]:
            exception_info = traceback.format_exc()
            self._logger.critical(formatted_message, exc_info=exc_info, extra=props)
            # Add exception stack trace to terminal output
            self._print_to_terminal("CRITICAL", f"{formatted_message}\n{exception_info}", extra_dims)
        else:
            self._logger.critical(formatted_message, exc_info=exc_info, extra=props)
            self._print_to_terminal("CRITICAL", formatted_message, extra_dims)
            
        # Add to history with exception info if available
        critical_dims = dict(extra_dims or {})
        if exception_info:
            critical_dims["exception_info"] = exception_info
        self._add_to_history("CRITICAL", formatted_message, critical_dims)

    def set_unique_identifiers(
        self, instance_id: str, chat_id: str, project_friendly_id: str
    ) -> None:
        """
        Set unique identifiers for the logger instance.
        
        Args:
            instance_id: The instance ID
            chat_id: The chat ID
            project_friendly_id: The project friendly ID
        """
        self._instance_id = instance_id
        self._chat_id = chat_id
        self._project_friendly_id = project_friendly_id
        self._properties = {
            "custom_dimensions": {
                "instanceId": instance_id,
                "chatId": chat_id,
                "projectFriendlyId": project_friendly_id,
            }
        }
        
        # Add identifier change to history
        self._add_to_history(
            "INFO", 
            f"Set identifiers: instanceId={instance_id}, chatId={chat_id}, projectFriendlyId={project_friendly_id}",
            {"identifiers_updated": True}
        )
    
    def _get_properties_with_extras(self, extra_dims: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
        """
        Merge base properties with any extra dimensions.
        
        Args:
            extra_dims: Additional dimensions to include in the log
            
        Returns:
            Dictionary with merged properties
        """
        if not extra_dims:
            return self._properties
            
        result = dict(self._properties)
        result["custom_dimensions"] = {
            **self._properties.get("custom_dimensions", {}),
            **extra_dims
        }
        return result
        
    @contextmanager
    def operation_context(self, operation_name: str, include_timing: bool = True, 
                         log_level: str = "INFO", extra_dims: Optional[Dict[str, Any]] = None) -> None:
        """
        Context manager for logging operations with timing information.
        
        Args:
            operation_name: Name of the operation being performed
            include_timing: Whether to include timing information
            log_level: Log level to use (INFO, DEBUG, etc.)
            extra_dims: Additional dimensions to include in the logs
            
        Usage:
            with logger.operation_context("fetch_data"):
                data = fetch_data()
        """
        log_method = getattr(self, log_level.lower(), self.info)
        
        # Starting message
        log_method(f"Starting operation: {operation_name}", extra_dims=extra_dims)
        
        start_time = time.time() if include_timing else None
        try:
            yield
            # Success message with timing if requested
            if include_timing:
                duration_ms = round((time.time() - start_time) * 1000, 2)
                op_dims = {"operation_duration_ms": duration_ms}
                if extra_dims:
                    op_dims.update(extra_dims)
                log_method(f"Completed operation: {operation_name} (took {duration_ms}ms)", 
                          extra_dims=op_dims)
            else:
                log_method(f"Completed operation: {operation_name}", extra_dims=extra_dims)
                
        except Exception as e:
            # Log failure with exception details
            if include_timing:
                duration_ms = round((time.time() - start_time) * 1000, 2)
                err_dims = {
                    "operation_duration_ms": duration_ms,
                    "error_type": type(e).__name__,
                    "error_details": str(e)
                }
                if extra_dims:
                    err_dims.update(extra_dims)
                self.error(f"Failed operation: {operation_name} (took {duration_ms}ms): {str(e)}", 
                          exc_info=True, extra_dims=err_dims)
            else:
                err_dims = {
                    "error_type": type(e).__name__,
                    "error_details": str(e)
                }
                if extra_dims:
                    err_dims.update(extra_dims)
                self.error(f"Failed operation: {operation_name}: {str(e)}", 
                          exc_info=True, extra_dims=err_dims)
            raise
            
    def log_metric(self, metric_name: str, value: float, extra_dims: Optional[Dict[str, Any]] = None) -> None:
        """
        Log a metric value with optional additional dimensions.
        
        Args:
            metric_name: Name of the metric
            value: Numeric value of the metric
            extra_dims: Optional additional dimensions to include
        """
        metric_dims = {"metric_name": metric_name, "metric_value": value}
        if extra_dims:
            metric_dims.update(extra_dims)
            
        self.info(f"Metric: {metric_name}={value}", extra_dims=metric_dims)
            
    def log_tool_execution(self, tool_name: str, successful: bool, 
                          execution_time_ms: Optional[float] = None,
                          error_details: Optional[str] = None) -> None:
        """
        Log information about a tool execution.
        
        Args:
            tool_name: Name of the tool that was executed
            successful: Whether the execution was successful
            execution_time_ms: Optional execution time in milliseconds
            error_details: Optional error details if execution failed
        """
        dims = {
            "tool_name": tool_name,
            "execution_successful": successful
        }
        
        if execution_time_ms is not None:
            dims["execution_time_ms"] = execution_time_ms
            
        if error_details:
            dims["error_details"] = error_details
            
        status = "successfully" if successful else "with errors"
        time_info = f" (took {execution_time_ms}ms)" if execution_time_ms is not None else ""
        
        message = f"Tool '{tool_name}' executed {status}{time_info}"
        if error_details and not successful:
            message = f"{message}. Error: {error_details}"
            self.error(message, extra_dims=dims)
        else:
            self.info(message, extra_dims=dims)
    
    def get_log_history(self, level: Optional[str] = None) -> List[Dict[str, Any]]:
        """
        Get the history of logs as a list of dictionaries.
        
        Args:
            level: Optional log level to filter by (DEBUG, INFO, etc.)
            
        Returns:
            List of log entries, each as a dictionary
        """
        if level is None:
            return self._log_history
        
        level = level.upper()
        return [entry for entry in self._log_history if entry["level"] == level]
    
    def get_log_messages(self, level: Optional[str] = None, include_level: bool = True, 
                        include_timestamp: bool = False, separator: str = ", ") -> str:
        """
        Get log messages as a string with the specified separator.
        
        This function retrieves log messages from the history and formats them as a single string.
        It provides several formatting options to customize the output for different use cases.
        
        Args:
            level: Optional log level to filter by (DEBUG, INFO, etc.). If None, returns all logs.
                   Valid values: "DEBUG", "INFO", "WARNING", "ERROR", "CRITICAL"
            include_level: Whether to include the log level in each message (e.g., "INFO - Message").
                          Useful for distinguishing between different types of logs.
            include_timestamp: Whether to include the timestamp in each message 
                              (e.g., "2025-03-25 10:15:30 - Message").
                              Useful for chronological analysis.
            separator: String used to join the individual log messages. Common options:
                      - ", " (default): Compact inline format (Message1, Message2)
                      - "\n": One message per line for readability
                      - " | ": Pipe-separated format for structured parsing
            
        Returns:
            String containing the formatted log messages joined by the specified separator.
        
        Examples:
            # Get all logs in default format: "INFO - Message1, DEBUG - Message2"
            all_logs = logger.get_log_messages()
            
            # Get only error logs: "ERROR - Database connection failed"
            error_logs = logger.get_log_messages(level="ERROR")
            
            # Get logs without level prefix: "User authenticated, Config loaded"
            clean_logs = logger.get_log_messages(include_level=False)
            
            # Get timestamped logs on separate lines: 
            # "2025-03-25 10:15:30 - INFO - Server started
            #  2025-03-25 10:15:35 - ERROR - Database connection failed"
            detailed_logs = logger.get_log_messages(include_timestamp=True, separator="\n")
        
        Notes:
            - Messages are retrieved in chronological order (oldest first)
            - When filtering by level, the case is insensitive ("error" works the same as "ERROR")
            - Individual parts of each log message are joined with " - " (e.g., "INFO - Message")
            - If both include_timestamp and include_level are True, the format will be:
              "TIMESTAMP - LEVEL - MESSAGE"
        """
        filtered_logs = self.get_log_history(level)
        messages = []
        
        for entry in filtered_logs:
            msg_parts = []
            
            if include_timestamp:
                msg_parts.append(entry["timestamp"])
                
            if include_level:
                msg_parts.append(entry["level"])
                
            msg_parts.append(entry["message"])
            
            messages.append(" - ".join(msg_parts))
            
        return separator.join(messages)
    
    def clear_log_history(self) -> None:
        """
        Clear the log history.
        """
        self._log_history = []
        self._logger.debug("Log history cleared")
        self._add_to_history("DEBUG", "Log history cleared")


def get_logger():
    """
    Factory function to create a logger
    mainly used to create a logger
    through a dependency injection
    """
    try:
        yield Logger()
    except Exception as e:
        logging.error("error getting logger instance")
        raise e
