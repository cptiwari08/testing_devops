from contextvars import ContextVar

from fastapi import Response
from starlette.middleware.base import BaseHTTPMiddleware, RequestResponseEndpoint
from starlette.requests import Request

INSTANCE_ID_CTX_KEY = ""

_instance_id_ctx_var: ContextVar[str] = ContextVar(INSTANCE_ID_CTX_KEY, default="")


def get_instance_id() -> str:
    return _instance_id_ctx_var.get()


class RequestContextMiddleware(BaseHTTPMiddleware):

    async def dispatch(
        self, request: Request, call_next: RequestResponseEndpoint
    ) -> Response:

        instance_id: str | None = None
        instance_id_token = None

        if request.method == "POST":
            body_content = await request.body()
            if body_content:
                body = await request.json()
                instance_id = body.get("instanceId")

                if instance_id:
                    instance_id_token = _instance_id_ctx_var.set(instance_id)

        response = await call_next(request)

        if instance_id_token:
            _instance_id_ctx_var.reset(instance_id_token)

        return response
