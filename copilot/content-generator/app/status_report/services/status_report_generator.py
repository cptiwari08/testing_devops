from datetime import datetime, timezone

from app.core.logging_config import LoggerConfig
from app.core.redis_service import RedisService
from app.core.schemas import RuntimeStatus
from app.status_report.schemas.status_report_input import StatusReportInput
from app.status_report.schemas.status_report_output import (
    StatusReport,
    StatusReportOutput,
)
from app.status_report.services.workflow.status_report_generator_workflow import (
    StatusReportGeneratorWorkflow,
)


class StatusReportGenerator:
    def __init__(
        self,
        logger: LoggerConfig,
        redis: RedisService,
        workflow: StatusReportGeneratorWorkflow,
    ) -> None:
        self.logger: LoggerConfig = logger
        self.redis: RedisService = redis
        self.workflow: StatusReportGeneratorWorkflow = workflow

    async def generate(self, input_: StatusReportInput) -> None:

        instance_id = str(input_.instanceId)
        result = await self.workflow.run(value=input_)
        self.logger.info(f"Status report generated result: {result}")
        runtimeStatus = RuntimeStatus.completed

        errorMessage = None
        output = None
        try:
            # we are expecting a dictionary that have a StatusReport schema,
            # but if we return a string it means that the generation failed and contains the error message
            output = StatusReport(**result)
        except Exception as e:
            self.logger.error(f"Status report generation failed: {e}, result: {result}")
            runtimeStatus = RuntimeStatus.failed
            errorMessage = result if isinstance(result, str) else str(e)

        current_status_report: StatusReportOutput | None = self.redis.get(
            instance_id, StatusReportOutput
        )

        if not current_status_report:
            self.logger.error("Status report not found")
            status_report_failed = StatusReportOutput(
                name="StatusReportGenerator",
                instanceId=input_.instanceId,
                runtimeStatus=RuntimeStatus.failed,
                input=input_,
                output=None,
                lastUpdatedTime=datetime.now(timezone.utc),
            )
            self.redis.update(instance_id, status_report_failed)
            return

        current_status_report.lastUpdatedTime = datetime.now(timezone.utc)
        if current_status_report.runtimeStatus == RuntimeStatus.aborted:
            self.logger.warning("Status report aborted")
            self.redis.update(instance_id, current_status_report)
            return

        current_status_report.runtimeStatus = runtimeStatus
        current_status_report.errorMessage = errorMessage
        current_status_report.output = output
        self.redis.update(instance_id, current_status_report)
        self.logger.info(
            f"PMO Status report finished with runtimeStatus: {runtimeStatus}"
        )
