import httpx
from app.core.config import CopilotApiConfig
from app.core.logging_config import LoggerConfig


class CopilotApi:
    def __init__(self, copilot_api_config: CopilotApiConfig, logger: LoggerConfig) -> None:
        self.copilot_api_config: CopilotApiConfig = copilot_api_config
        self.logger: LoggerConfig = logger

    async def call_endpoint(self, method, url, payload: dict):
        headers = headers = {
            "Accept": "application/json, text/plain, */*",
            "Accept-Language": "en-US,en;q=0.9",
            "ce-api-key": self.copilot_api_config.copilot_api_key,
            "Connection": "keep-alive",
            "Content-Type": "application/json",
        }
        url = f"{self.copilot_api_config.host_copilot_api}{url}"
        async with httpx.AsyncClient() as client:
            response = await client.request(
                method,
                url,
                headers=headers,
                data=payload,
                timeout=self.copilot_api_config.timeout,
            )

            try:
                response.raise_for_status()
            except Exception as e:
                self.logger.error(f"Error calling endpoint {url} \nError: \n{e}")
                return None
            return response.json()

    async def get_content_generator_sqls(self):
        url = "/ContentGenerator/data"
        return await self.call_endpoint("GET", url, {})

    async def get_project_context(self):
        url = "/configuration/PROJECT_CONTEXT"
        return await self.call_endpoint("GET", url, {})
