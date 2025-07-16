import logging

from app.core.config import Config
from app.core.middlewares.request_context_middleware import get_instance_id
from azure.monitor.opentelemetry import configure_azure_monitor
from opentelemetry.instrumentation.logging import LoggingInstrumentor


class LoggerConfig:
    def __init__(self) -> None:
        if Config.ENABLE_APP_INSIGHTS in {"true"}:
            self.configure_azure_monitor()
        self.configure_logging()

    def configure_azure_monitor(self) -> None:
        configure_azure_monitor(
            connection_string=Config.APPLICATIONINSIGHTS_CONNECTION_STRING,
            logger_name="content_generator",
        )

    def configure_logging(self) -> None:
        logging.basicConfig(level=logging._nameToLevel[Config.LOG_LEVEL])
        self.logger: logging.Logger = logging.getLogger("content_generator")
        LoggingInstrumentor().instrument(set_logging_format=True)

    def debug(self, message: str) -> None:
        if instance_id := get_instance_id():
            message = f"[{instance_id}] {message}"
        self.logger.debug(message)

    def info(self, message: str) -> None:
        if instance_id := get_instance_id():
            message = f"[{instance_id}] {message}"
        self.logger.info(message)

    def warning(self, message: str) -> None:
        if instance_id := get_instance_id():
            message = f"[{instance_id}] {message}"
        self.logger.warning(message)

    def error(self, message: str) -> None:
        if instance_id := get_instance_id():
            message = f"[{instance_id}] {message}"
        self.logger.error(message)

    def critical(self, message: str) -> None:
        if instance_id := get_instance_id():
            message = f"[{instance_id}] {message}"
        self.logger.critical(message)
