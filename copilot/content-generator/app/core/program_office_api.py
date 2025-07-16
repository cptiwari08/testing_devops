import json
import time
from typing import Any

import httpx
from app.core.logging_config import LoggerConfig
from app.status_report.config import ProgramOfficeConfig
from pydantic import BaseModel


class ProgramOfficeResponse(BaseModel):
    status_code: str
    data: Any


class ProgramOffice:
    def __init__(
        self, program_office_config: ProgramOfficeConfig, logger: LoggerConfig
    ) -> None:
        self.program_office_config: ProgramOfficeConfig = program_office_config
        self.logger: LoggerConfig = logger

    async def run_query(self, payload: dict) -> httpx.Response:

        url = f"{self.program_office_config.host_program_office}/copilot/execute-query"
        headers = {
            "Accept": "application/json, text/plain, */*",
            "Accept-Language": "en-US,en;q=0.9",
            "ce-api-key": self.program_office_config.ce_po_api_key,
            "Connection": "keep-alive",
            "Content-Type": "application/json",
        }

        async with httpx.AsyncClient() as client:
            response: httpx.Response = await client.post(
                url,
                headers=headers,
                data=json.dumps(payload),  # type: ignore
                timeout=self.program_office_config.timeout,
            )
            return response

    async def run_sql(
        self, sql_query: str, tables: list[str] = []
    ) -> ProgramOfficeResponse:

        payload = {"sqlQuery": sql_query, "tables": tables}
        iterations = 2
        current_iteration = 1
        response_bool = False
        output = []
        status_code = "500"

        while current_iteration <= iterations and response_bool == False:
            current_iteration = current_iteration + 1
            # TODO: Implement tenacity
            try:
                response = await self.run_query(payload)
                response.raise_for_status()
                output = response.json()
                status_code = response.status_code
                response_bool = True
            except Exception as e:
                self.logger.error(
                    f"Error gathering data from Program Office \nPayload: \n{payload} \nError: \n{e}"
                )
            time.sleep(0.5)
        return ProgramOfficeResponse(status_code=str(status_code), data=output)

    async def run_stored_procedure(
        self, sp_name: str, parameters: dict = None
    ) -> ProgramOfficeResponse:
        """
        Execute a stored procedure with optional parameters.
        
        Args:
            sp_name: Name of the stored procedure to execute
            parameters: Dictionary of parameters to pass to the stored procedure
                       Format: {"@parametername": "value"}
        
        Returns:
            ProgramOfficeResponse with the execution results
        """
        url = f"{self.program_office_config.host_program_office}/proc/{sp_name}"
        headers = {
            "Accept": "application/json, text/plain, */*",
            "Accept-Language": "en-US,en;q=0.9",
            "ce-api-key": self.program_office_config.ce_po_api_key,
            "Connection": "keep-alive",
            "Content-Type": "application/json",
        }
        # Add @ in front of each parameter name
        if parameters:
            parameters = {f"@{key}": value for key, value in parameters.items()}
        payload = {"Parameters": parameters} if parameters else {}
        
        iterations = 2
        current_iteration = 1
        response_bool = False
        output = []
        status_code = "500"

        while current_iteration <= iterations and response_bool == False:
            current_iteration = current_iteration + 1
            try:
                async with httpx.AsyncClient() as client:
                    response = await client.post(
                        url,
                        headers=headers,
                        json=payload,
                        timeout=self.program_office_config.timeout,
                    )
                response.raise_for_status()
                output = response.json()
                status_code = response.status_code
                response_bool = True
            except Exception as e:
                self.logger.error(
                    f"Error executing stored procedure {sp_name}\nPayload: {payload}\nError: {e}"
                )
            time.sleep(0.5)
        return ProgramOfficeResponse(status_code=str(status_code), data=output)


