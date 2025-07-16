import logging

from app.core.config import Config
from app.core.healthcheck import router as healthcheck_router
from app.core.middlewares.request_context_middleware import RequestContextMiddleware
from app.dependencies import Container
from app.pmo_workplan.endpoints.start import router as pmo_workplan_start
from app.pmo_workplan.endpoints.terminate import router as pmo_workplan_cancel
from app.project_charter.endpoints.start import router as project_charter_start
from app.project_charter.endpoints.terminate import router as project_charter_terminate
from app.status_report.endpoints.start import router as status_report_start
from app.status_report.endpoints.terminate import router as status_report_terminate
from app.tsa_generator.endpoints.start import router as tsa_start
from app.tsa_generator.endpoints.terminate import router as tsa_terminate
from app.op_model.endpoints.start import router as op_model_start
from app.op_model.endpoints.terminate import router as op_model_terminate
from dependency_injector.wiring import Provide, inject
from fastapi import FastAPI

from . import __version__


@inject
def create_app(
    configure_azure_monitor=Provide[Container.configure_azure_monitor],
    fastapi_instrumentor=Provide[Container.fastapi_instrumentor],
) -> FastAPI:
    container = Container()

    app = FastAPI(
        title="Content Generators",
        version=__version__,
    )
    container.config.from_dict(
        {
            "app": app
        }
    )

    if Config.ENABLE_APP_INSIGHTS in {"true"}:
        logging.basicConfig(level=logging._nameToLevel[Config.LOG_LEVEL])
        configure_azure_monitor()
        fastapi_instrumentor()

    app.container = container  # type: ignore

    app.add_middleware(RequestContextMiddleware)

    # Routers
    app.include_router(healthcheck_router)

    app.include_router(pmo_workplan_start)
    app.include_router(pmo_workplan_cancel)

    app.include_router(status_report_start)
    app.include_router(status_report_terminate)

    app.include_router(project_charter_start)
    app.include_router(project_charter_terminate)

    app.include_router(tsa_start)
    app.include_router(tsa_terminate)

    app.include_router(op_model_start)
    app.include_router(op_model_terminate)

    return app


app = create_app()
