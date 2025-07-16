# This is the only file which do not follow pep8
# to expose pascal case to API Clients
from uuid import UUID

from pydantic import BaseModel, ConfigDict


class ProjectDetails(BaseModel):
    sector: str | None
    subSector: str | None
    transactionType: str | None


class ProjectTeam(BaseModel):
    id: str
    title: str


class ReportingPeriod(BaseModel):
    id: str
    title: str
    periodStartDate: str
    periodEndDate: str


class StatusReportInput(BaseModel):
    instanceId: UUID
    projectTeam: ProjectTeam
    reportingPeriod: ReportingPeriod
    projectDetails: ProjectDetails

    model_config: ConfigDict = {
        "json_schema_extra": {
            "examples": [
                {
                    "instanceId": "6bd23cc3-9f08-42d6-910b-32975a232d9a",
                    "projectTeam": {"title": "HR", "id": "59"},
                    "reportingPeriod": {
                        "title": "2024/10/22 - 2024/10/28",
                        "id": "1111",
                        "periodStartDate": "2024/10/22",
                        "periodEndDate": "2024/10/28"
                    },
                    "projectDetails": {
                        "sector": "Information Technology",
                        "subSector": "Software",
                        "transactionType": "Buy&Integrate",
                    },
                }
            ]
        }
    }
