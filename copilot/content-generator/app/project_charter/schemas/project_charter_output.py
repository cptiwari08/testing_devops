# This is the only file which do not follow pep8
# to expose pascal case to API Clients

from datetime import datetime
from typing import List

from app.project_charter.schemas.project_charter_input import ProjectCharterInput
from pydantic import UUID4, BaseModel, ConfigDict


class SourceValueDocument(BaseModel):
    documentId: UUID4
    documentName: str
    chunk_id: UUID4
    pages: List[int]
    chunk_text: str


class SourceValueEYIP(BaseModel):
    tableName: str
    rowCount: int
    sqlQuery: str


class CitingSource(BaseModel):
    sourceName: str
    sourceType: str
    content: str
    sourceValue: List[SourceValueDocument] | List[SourceValueEYIP]
    #sourceValue: List[str] | List[None]

class Response(BaseModel):
    sourceName: str
    content: List[str]
    status: str
    citingSources: List[dict]


class Output(BaseModel):
    response: List[Response] = []    

class ProjectCharterOutput(BaseModel):
    name: str
    instanceId: UUID4
    runtimeStatus: str
    errorMessage: str | None = None
    input: ProjectCharterInput
    output: Output
    createdTime: datetime | None = None
    lastUpdatedTime: datetime
    model_config: ConfigDict = {
        "json_schema_extra": {
            "examples": [
                {
                    "name": "ProjectCharterGenerator",
                    "instanceId": "3ea287ad968c44fdb3b4ca6e93c1b17a",
                    "runtimeStatus": "Completed",
                    "errorMessage": "Deserialization error",
                    "input": {
                        "projectTeam": {"title": "HR", "id": 100012},
                        "projectDetails": {
                            "sector": "Information Technology",
                            "subSector": "Software",
                            "transactionType": "Buy&Integrate",
                        },
                        "projectDocs": ["100001", "100002", "100003"],
                        "eyIP": ["100001", "100002", "100003"],
                        "sections": [
                            "Objective",
                            "InScope",
                            "OutofScope",
                            "Risks/Issues",
                            "Interdependecies",
                        ],
                    },
                    "output": {
                        "response": [
                            {
                                "sourceName": "Objective",
                                "content": "This is the output of objective",
                                "status": "200",
                                "citingSources": [
                                    {
                                        "sourceName": "team-specific-project-docs",
                                        "sourceType": "documents",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "documentId": "a5bf20f4-7f9e-42d7-a951-0e551f8a33fa",
                                                "documentName": "Value Capture _ Services & Tool_bench_a5bf20f4-7f9e-42d7-a951-0e551f8a33fa.pptx",
                                                "chunk_id": "1bf37f9f-f2d2-4ce6-a2ec-f7f0b996fdc3",
                                                "pages": [2],
                                                "chunk_text": "Establish cost targets across taxonomies. \nEnter potential impacts for FTEs, cost reduction, revenue growth and cost to achieve. \nMaintain accountability to drive future execution. ...",
                                            },
                                            {
                                                "documentId": "8a23f962-2257-4212-8eea-40b668092e04",
                                                "documentName": "EY Capital Edge Platform_8a23f962-2257-4212-8eea-40b668092e04.pptx",
                                                "chunk_id": "7b85cf41-985a-4460-99ba-1a6266beb7db",
                                                "pages": [1, 2],
                                                "chunk_text": "EY's Capital Edge Platform. \n\nEY-Parthenon | Page 1. ...",
                                            },
                                        ],
                                    },
                                    {
                                        "sourceName": "ey-ip",
                                        "sourceType": "EYIP Database",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "tableName": "ProjectCharters",
                                                "rowCount": 10,
                                                "sqlQuery": "select objectives from projectcharters where projectteam = 'Finance'",
                                            }
                                        ],
                                    },
                                ],
                                "chainOfThought": "",
                            },
                            {
                                "sourceName": "OutofScope",
                                "content": "This is the output of OutofScope",
                                "status": "200",
                                "citingSources": [
                                    {
                                        "sourceName": "team-specific-project-docs",
                                        "sourceType": "documents",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "documentId": "a5bf20f4-7f9e-42d7-a951-0e551f8a33fa",
                                                "documentName": "Value Capture _ Services & Tool_bench_a5bf20f4-7f9e-42d7-a951-0e551f8a33fa.pptx",
                                                "chunk_id": "1bf37f9f-f2d2-4ce6-a2ec-f7f0b996fdc3",
                                                "pages": [2],
                                                "chunk_text": "Establish cost targets across taxonomies. \nEnter potential impacts for FTEs, cost reduction, revenue growth and cost to achieve. \nMaintain accountability to drive future execution. ...",
                                            },
                                            {
                                                "documentId": "8a23f962-2257-4212-8eea-40b668092e04",
                                                "documentName": "EY Capital Edge Platform_8a23f962-2257-4212-8eea-40b668092e04.pptx",
                                                "chunk_id": "7b85cf41-985a-4460-99ba-1a6266beb7db",
                                                "pages": [1, 2],
                                                "chunk_text": "EY's Capital Edge Platform. \n\nEY-Parthenon | Page 1. ...",
                                            },
                                        ],
                                    },
                                    {
                                        "sourceName": "ey-ip",
                                        "sourceType": "EYIP Database",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "tableName": "ProjectCharters",
                                                "rowCount": 10,
                                                "sqlQuery": "select objectives from projectcharters where projectteam = 'Finance'",
                                            }
                                        ],
                                    },
                                ],
                                "chainOfThought": "",
                            },
                            {
                                "sourceName": "InScope",
                                "content": "This is the output of in scope",
                                "status": "200",
                                "citingSources": [
                                    {
                                        "sourceName": "team-specific-project-docs",
                                        "sourceType": "documents",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "documentId": "a5bf20f4-7f9e-42d7-a951-0e551f8a33fa",
                                                "documentName": "Value Capture _ Services & Tool_bench_a5bf20f4-7f9e-42d7-a951-0e551f8a33fa.pptx",
                                                "chunk_id": "1bf37f9f-f2d2-4ce6-a2ec-f7f0b996fdc3",
                                                "pages": [2],
                                                "chunk_text": "Establish cost targets across taxonomies. \nEnter potential impacts for FTEs, cost reduction, revenue growth and cost to achieve. \nMaintain accountability to drive future execution. ...",
                                            },
                                            {
                                                "documentId": "8a23f962-2257-4212-8eea-40b668092e04",
                                                "documentName": "EY Capital Edge Platform_8a23f962-2257-4212-8eea-40b668092e04.pptx",
                                                "chunk_id": "7b85cf41-985a-4460-99ba-1a6266beb7db",
                                                "pages": [1, 2],
                                                "chunk_text": "EY's Capital Edge Platform. \n\nEY-Parthenon | Page 1. ...",
                                            },
                                        ],
                                    },
                                    {
                                        "sourceName": "ey-ip",
                                        "sourceType": "EYIP Database",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "tableName": "ProjectCharters",
                                                "rowCount": 10,
                                                "sqlQuery": "select objectives from projectcharters where projectteam = 'Finance'",
                                            }
                                        ],
                                    },
                                ],
                                "chainOfThought": "",
                            },
                            {
                                "sourceName": "Risks/Issues",
                                "content": "This is the output of risks and Issues",
                                "status": "200",
                                "citingSources": [
                                    {
                                        "sourceName": "team-specific-project-docs",
                                        "sourceType": "documents",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "documentId": "a5bf20f4-7f9e-42d7-a951-0e551f8a33fa",
                                                "documentName": "Value Capture _ Services & Tool_bench_a5bf20f4-7f9e-42d7-a951-0e551f8a33fa.pptx",
                                                "chunk_id": "1bf37f9f-f2d2-4ce6-a2ec-f7f0b996fdc3",
                                                "pages": [2],
                                                "chunk_text": "Establish cost targets across taxonomies. \nEnter potential impacts for FTEs, cost reduction, revenue growth and cost to achieve. \nMaintain accountability to drive future execution. ...",
                                            },
                                            {
                                                "documentId": "8a23f962-2257-4212-8eea-40b668092e04",
                                                "documentName": "EY Capital Edge Platform_8a23f962-2257-4212-8eea-40b668092e04.pptx",
                                                "chunk_id": "7b85cf41-985a-4460-99ba-1a6266beb7db",
                                                "pages": [1, 2],
                                                "chunk_text": "EY's Capital Edge Platform. \n\nEY-Parthenon | Page 1. ...",
                                            },
                                        ],
                                    },
                                    {
                                        "sourceName": "ey-ip",
                                        "sourceType": "EYIP Database",
                                        "content": "Final generated output to be used to generate workplan for HR",
                                        "sourceValue": [
                                            {
                                                "tableName": "ProjectCharters",
                                                "rowCount": 10,
                                                "sqlQuery": "select objectives from projectcharters where projectteam = 'Finance'",
                                            }
                                        ],
                                    },
                                ],
                                "chainOfThought": "",
                            },
                        ]
                    },
                    "createdTime": "2024-10-22T09:00:00Z",
                    "lastUpdatedTime": "2024-10-28T18:00:00Z",
                }
            ]
        }
    }
