import logging
import os
import sys

from opencensus.ext.azure.log_exporter import AzureLogHandler


class Logger:
    """
    A class used for logging messages.
    This class provides methods for logging debug and info messages. It uses the standard Python
    logging module and also sends logs to Azure Application Insights.
    """

    def __init__(self):
        self._logger = logging.getLogger("my_logger")
        self._logger.setLevel(logging.DEBUG)
        self._properties = {"custom_dimensions": {}}
        self._message = "sat ingestion pod: {hostname} {message}"

        if not self._logger.handlers:
            formatter = logging.Formatter(
                "%(asctime)s - %(name)s - %(levelname)s - %(message)s"
            )

            # set console logging
            # setting the stream parameter in the StreamHandler is
            # important, if you don't do this the azure Log Handler
            # is going to bypass the stdout logs
            console_handler = logging.StreamHandler(stream=sys.stdout)
            console_handler.setFormatter(formatter)
            console_handler.setLevel(logging.DEBUG)
            
            # app insights handler
                        
            connection_string = (
                f"InstrumentationKey={os.getenv('APP_INSIGHTS_INSTRUMENTATION_KEY')}"
            )
            
            app_insights_handler = AzureLogHandler(connection_string=connection_string)
            app_insights_handler.setLevel(logging.DEBUG)

            self._logger.addHandler(console_handler)
            self._logger.addHandler(app_insights_handler)

    def debug(self, message: str) -> None:
        message = self._message.format(hostname=os.getenv("HOSTNAME"), message=message)
        self._logger.debug(message, extra=self._properties)

    def info(self, message: str) -> None:
        message = self._message.format(hostname=os.getenv("HOSTNAME"), message=message)
        self._logger.info(message, extra=self._properties)

    def warning(self, message: str) -> None:
        message = self._message.format(hostname=os.getenv("HOSTNAME"), message=message)
        self._logger.warning(message, extra=self._properties)

    def error(self, message: object) -> None:
        message = self._message.format(hostname=os.getenv("HOSTNAME"), message=message)
        self._logger.error(message, extra=self._properties, exc_info=True)

    def critical(self, message: str) -> None:
        message = self._message.format(hostname=os.getenv("HOSTNAME"), message=message)
        self._logger.critical(message, extra=self._properties)
