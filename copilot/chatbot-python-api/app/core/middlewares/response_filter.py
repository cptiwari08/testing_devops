import json
from json.decoder import JSONDecodeError

from app.core.config import ResponseOptions
from fastapi import Request
from starlette.middleware.base import BaseHTTPMiddleware, RequestResponseEndpoint
from starlette.responses import JSONResponse, Response


class ResponseFilter(BaseHTTPMiddleware):
    """
    A middleware for clean all the response before return them
    """

    async def dispatch(
        self, request: Request, call_next: RequestResponseEndpoint
    ) -> Response | JSONResponse:
        response = await call_next(request)

        # Read the response body as bytes
        response_body = [section async for section in response.body_iterator]  # type: ignore
        response_bytes = b"".join(response_body)

        try:
            response_dict = json.loads(response_bytes.decode("utf-8"))
        except JSONDecodeError:
            return response

        headers = dict(response.headers)

        # Remove the old Content-Length header, so it gets recalculated automatically
        headers.pop("content-length", None)

        # Do all the clean up here

        if ResponseOptions.return_sql.lower() not in {"true"}:
            response_dict["sql"] = None

        if ResponseOptions.return_chunks not in {"true"}:
            # This code has higher cognitive complexity because
            # both chunk_id and chunk_text are deeply nested
            # in the project-docs response. Since this code
            # is only within the scope of this middleware,
            # splitting it into several functions would add
            # more complexity due to the nesting, and exposing
            # those functions to the core module doesn't make sense.
            #
            # This code is not intended to change, but if it
            # needs to grow for any reason, refactor it to a
            # different logic instead of adding more complexity.
            citing_sources = response_dict.pop("citingSources", [])
            citing_sources = [] if citing_sources is None else citing_sources
            for source in citing_sources:
                for value_ in source.get("sourceValue", []):
                    if isinstance(value_, dict):
                        value_.pop("chunk_id", None)
                        value_.pop("chunk_text", None)
            response_dict["citingSources"] = citing_sources

        # Return the modified response
        return JSONResponse(
            content=response_dict,
            status_code=response.status_code,
            headers=headers,
            media_type=response.media_type,
        )
