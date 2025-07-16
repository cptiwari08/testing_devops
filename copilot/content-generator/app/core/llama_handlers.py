from datetime import datetime

from app.core.logging_config import LoggerConfig
from llama_index.core.callbacks import TokenCountingHandler


class LoggerHandler(TokenCountingHandler):
    def __init__(self, logger: LoggerConfig, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.logger: LoggerConfig = logger

    def on_event_start(self, *args, **kwargs) -> str:
        self.event_start_time = datetime.now()
        return super().on_event_start(*args, **kwargs)

    def on_event_end(self, *args, **kwargs):
        super().on_event_end(*args, **kwargs)
        envent_end_time = datetime.now()
        event_elapsed_time = envent_end_time - self.event_start_time
        miliseconds = event_elapsed_time.total_seconds() * 1000
        for count in self.llm_token_counts:
            # llama index generate empty events with
            # no prompt and 0 token counts
            # in does cases the prompt is an empty string
            # that is why we do the following if statement
            #
            # this way we avoiding garbage logging
            if count.prompt:
                message = (
                    f"completion_tokens: {count.completion_token_count}, "
                    f"prompts_tokens: {count.prompt_token_count}, "
                    f"total_tokens: {count.total_token_count}, "
                    f"time: {miliseconds} ms, "
                    f"prompt: {count.prompt}"
                )
                self.logger.info(message)
        self.llm_token_counts = []
