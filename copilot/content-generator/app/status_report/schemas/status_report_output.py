# This is the only file which do not follow pep8
# to expose pascal case to API Clients

from datetime import datetime

from app.core.schemas import RuntimeStatus
from app.status_report.schemas.status_report_input import StatusReportInput
from pydantic import UUID4, BaseModel, ConfigDict


class SourceValue(BaseModel):
    rowCount: int | None
    tableName: str | None
    sqlQuery: str | None
    status: str | None


class CitingSources(BaseModel):
    sourceName: str
    sourceType: str
    sourceValue: list[SourceValue] = []
    context: str | None


class OverallStatusContent(BaseModel):
    id: str | int | None
    title: str | None


class SourceResponse(BaseModel):
    sourceName: str
    status: str | None = None
    content: str | list[str] | OverallStatusContent | None = None
    citingSources: list[CitingSources] | None = None


class StatusReport(BaseModel):
    response: list[SourceResponse] = []


class StatusReportOutput(BaseModel):
    name: str
    instanceId: UUID4
    runtimeStatus: RuntimeStatus
    errorMessage: str | None = None
    input: StatusReportInput
    output: StatusReport | None = None
    createdTime: datetime | None = None
    lastUpdatedTime: datetime

    model_config: ConfigDict = {
        "json_schema_extra": {
            "examples": [
                {
                    "name": "StatuReportGenerator",
                    "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
                    "runtimeStatus": "Completed",
                    "input": {
                        "projectTeam": {"title": "HR", "id": "100012"},
                        "reportingPeriod": {
                            "title": "2024/10/22 - 2024/10/28",
                            "id": "1111",
                        },
                        "projectDetails": {
                            "sector": "Information Technology",
                            "subSector": "Software",
                            "transactionType": "Buy&Integrate",
                        },
                    },
                    "output": {
                        "statusReport": {
                            "projectTeam": {"title": "HR", "id": "100012"},
                            "overallStatus": "Behind Schedule",
                            "executiveSummary": "This is my executive summary",
                            "nextSteps": [
                                "Finish my task",
                                "Get more task",
                                "Do something",
                            ],
                            "accomplishments": [
                                "Finish my task",
                                "Get more task",
                                "Do something",
                            ],
                        }
                    },
                    "createdTime": "2024-06-06T22:02:17Z",
                    "lastUpdatedTime": "2024-06-06T22:02:30Z",
                }
            ]
        }
    }
