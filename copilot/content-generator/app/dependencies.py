from app.core.asset_manager_engine import AssetManagerEngine
from app.core.azure_llm_models import AzureEmbeddings, AzureSyncLLM
from app.core.config import (
    Config,
    CopilotApiConfig,
    EmbeddingsConfig,
    OpenAIConfig,
    RedisConfig,
)
from app.core.copilot_api import CopilotApi
from app.core.key_vault import KeyVaultManager
from app.core.llama_handlers import LoggerHandler
from app.core.logging_config import LoggerConfig
from app.core.program_office_api import ProgramOffice
from app.core.redis_service import RedisService
from app.core.config import AISearchConfig
from app.core.config import AssetManagerConfig
from app.core.ai_search import AISearch
from app.status_report.config import ProgramOfficeConfig
from app.status_report.services.status_report_generator import StatusReportGenerator
from app.status_report.services.workflow.status_report_generator_workflow import (
    StatusReportGeneratorWorkflow,
)
from azure.monitor.opentelemetry import configure_azure_monitor
from dependency_injector import containers, providers
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor


class Container(containers.DeclarativeContainer):
    # Wiring
    wiring_config = containers.WiringConfiguration(
        modules=[
            "app.main",
            "app.core.healthcheck",
            "app.status_report.endpoints.start",
            "app.status_report.endpoints.terminate",
        ],
        packages=[
            "app.pmo_workplan.endpoints",
            "app.pmo_workplan.services",
            "app.pmo_workplan.services.workflow",
            "app.project_charter.endpoints",
            "app.project_charter.services",
            "app.project_charter.services.workflow",
            "app.tsa_generator.endpoints",
            "app.tsa_generator.services",
            "app.tsa_generator.services.workflow",
            "app.op_model.endpoints",
            "app.op_model.services",
            "app.op_model.services.workflow",
        ],
    )

    # configurations
    config = providers.Configuration()
    key_vault = providers.Singleton(KeyVaultManager)
    logger = providers.Singleton(LoggerConfig)

    program_office_config = providers.Singleton(
        ProgramOfficeConfig, key_vault=key_vault
    )
    program_office = providers.Factory(
        ProgramOffice, program_office_config=program_office_config, logger=logger
    )
    copilot_api_config = providers.Singleton(CopilotApiConfig, key_vault=key_vault)
    copilot_api = providers.Factory(
        CopilotApi, copilot_api_config=copilot_api_config, logger=logger
    )
    llm_config = providers.Singleton(OpenAIConfig, key_vault=key_vault)
    redis_config = providers.Singleton(RedisConfig, key_vault=key_vault)
    redis = providers.Singleton(RedisService, redis_config=redis_config, logger=logger)
    logger_handler = providers.Factory(
        LoggerHandler,
        logger=logger,
    )
    llm = providers.Factory(
        AzureSyncLLM,
        openai_config=llm_config,
        logger_handler=logger_handler,
    )

    status_report_generator_workflow = providers.Factory(
        StatusReportGeneratorWorkflow,
        timeout=None,
        verbose=True,
        logger=logger,
        program_office=program_office,
        copilot_api=copilot_api,
        llm=llm,
        redis=redis,
    )

    status_report_generator = providers.Factory(
        StatusReportGenerator,
        logger=logger,
        redis=redis,
        workflow=status_report_generator_workflow,
    )

    embed_model_config = providers.Singleton(EmbeddingsConfig, key_vault=key_vault)
    embed_model = providers.Factory(
        AzureEmbeddings, embeddings_config=embed_model_config
    )
    asset_manager_config = providers.Singleton(AssetManagerConfig, key_vault=key_vault)
    asset_manager_engine = providers.Singleton(
        AssetManagerEngine, asset_manager_config=asset_manager_config
    )
    ai_search_config = providers.Singleton(AISearchConfig, key_vault=key_vault)
    ai_search = providers.Factory(
        AISearch,
        ai_search_config=ai_search_config,
        llm=llm,
        embed_model=embed_model,
    )

    # Global dependencies
    configure_azure_monitor = providers.Callable(
        configure_azure_monitor(
            connection_string=Config.APPLICATIONINSIGHTS_CONNECTION_STRING,
            logger_name="content_generator",
        )
    )
    fastapi_instrumentor = providers.Callable(
        FastAPIInstrumentor(config.app).instrument_app
    )
