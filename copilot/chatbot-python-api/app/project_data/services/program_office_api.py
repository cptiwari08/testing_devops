import json
from datetime import UTC, datetime, timedelta

import httpx
from app.core.errors import ProgramOfficeResponseError, ProgramOfficeTimeoutError
from app.core.utils import decode_jwt_token
from app.project_data.config import ProgramOfficeConfig


class ProgramOffice:
    async def run_query(self, payload, token):
        token_details = decode_jwt_token(token)

        url = f"{token_details['po_api_url']}/copilot/execute-query"
        headers = {
            "Accept": "application/json, text/plain, */*",
            "ce-auth": "",
            "Accept-Language": "en-US,en;q=0.9",
            "Authorization": token,
            "Connection": "keep-alive",
            "Content-Type": "application/json",
            "Origin": token_details["iss"],
            "Request-Timestamp": str(
                round((datetime.now(UTC) + timedelta(hours=1)).timestamp() * 1000)
            ),
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0",
        }

        async with httpx.AsyncClient() as client:
            response = await client.post(
                url,
                headers=headers,
                data=json.dumps(payload),
                timeout=ProgramOfficeConfig.timeout,
            )            
            #from app.core.save_results import save_project_data # TODO remove this import
            #save_project_data(payload["SqlQuery"], str(response), response.text)            
            try:
                response.raise_for_status()
            except httpx.TimeoutException:
                raise ProgramOfficeTimeoutError(url)
            except httpx.HTTPStatusError:
                raise ProgramOfficeResponseError(response)
            return response.json()
