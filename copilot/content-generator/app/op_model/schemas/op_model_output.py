# This is the only file which do not follow pep8
# to expose pascal case to API Clients
 
from datetime import datetime
from typing import List
 
from app.op_model.schemas.op_model_input import OPModelInput
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
 
class ProcessLvl3(BaseModel):
    title: str
    NodeType: dict
 
class ProcessLvl2(BaseModel):
    title: str
    NodeType: dict
    children: list[ProcessLvl3]
 
class ProcessLvl1(BaseModel):
    title: str
    NodeType: dict
    children: list[ProcessLvl2]
 
class Response(BaseModel):
    content: List[ProcessLvl1]
    status: str
    citingSources: List[dict]
 
 
class Output(BaseModel):
    response: List[Response] = []    
 
class OPModelOutput(BaseModel):
    name: str
    instanceId: UUID4
    runtimeStatus: str
    errorMessage: str | None = None
    input: OPModelInput
    output: Output
    createdTime: datetime | None = None
    lastUpdatedTime: datetime
    model_config: ConfigDict = {
        "json_schema_extra": {
            "examples": [
                {
                    "name": "OpModelGenerator",
                    "instanceId": "3ea287ad968c44fdb3b4ca6e93c1b17a",
                    "runtimeStatus": "Completed",
                    "errorMessage": "Deserialization error",
                    "input": {
                        "businessEntity": {
                        "id": 10001,
                        "title": "Day1 Op model Entity"
                        },
                        "projectDetails": {
                        "sector": "Information Technology",
                        "subSector": "Software",
                        "transactionType": "Buy&Integrate"
                        },
                        "projectDocs": [
                        "100001",
                        "100002",
                        "100003"
                        ],
                        "eyIP": [
                        "100001",
                        "100002",
                        "100003"
                        ],
                        "complexity": "High"
                    },
                    "output": {
                        "response": [
                        {
                            "content": [
                            {
                                "title": "IT",
                                "NodeType": {"key":"PROCESS_GROUP"},
                                "children": [
                                {
                                    "title": "Applications",
                                    "NodeType": {"key":"PROCESS_GROUP"},
                                    "children": [
                                    {
                                        "title": "Build applications",
                                        "NodeType": {"key":"PROCESS"}
                                    },
                                    {
                                        "title": "Build software",
                                        "NodeType": {"key":"PROCESS"}
                                    }
                                    ]
                                },
                                {
                                    "title": "Data Security",
                                    "NodeType": {"key":"PROCESS_GROUP"},
                                    "children": [
                                    {
                                        "title": "Build applications",
                                        "NodeType": {"key":"PROCESS"}
                                    },
                                    {
                                        "title": "Build software",
                                        "NodeType": {"key":"PROCESS"}
                                    }
                                    ]
                                },
                                {
                                    "title": "Cyber Security",
                                    "NodeType": {"key":"PROCESS_GROUP"},
                                    "children": [
                                    {
                                        "title": "Build applications",
                                        "NodeType": {"key":"PROCESS"}
                                    },
                                    {
                                        "title": "Build software",
                                        "NodeType": {"key":"PROCESS"}
                                    }
                                    ]
                                }
                                ]
                            },
                            {
                                "title": "HR",
                                "NodeType": {"key":"PROCESS_GROUP"},
                                "children": [
                                {
                                    "title": "Recruiting",
                                    "NodeType": {"key":"PROCESS_GROUP"},
                                    "children": [
                                    {
                                        "title": "Build applications",
                                        "NodeType": {"key":"PROCESS"}
                                    },
                                    {
                                        "title": "Build software",
                                        "NodeType": {"key":"PROCESS"}
                                    }
                                    ]
                                },
                                {
                                    "title": "Talent",
                                    "NodeType": {"key":"PROCESS_GROUP"},
                                    "children": [
                                    {
                                        "title": "Build applications",
                                        "NodeType": {"key":"PROCESS"}
                                    },
                                    {
                                        "title": "Build software",
                                        "NodeType": {"key":"PROCESS"}
                                    }
                                    ]
                                }
                                ]
                            }
                            ],
                            "status": "200",
                            "citingSources": [
                            {
                                "sourceName": "project-specific-project-docs",
                                "sourceType": "documents",
                                "content": "Final generated output to be used to generate workplan for all teams",
                                "sourceValue": [
                                {
                                    "documentId": "a5bf20f4-7f9e-42d7-a951-0e551f8a33fa",
                                    "documentName": "Value Capture _ Services & Tool_bench_a5bf20f4-7f9e-42d7-a951-0e551f8a33fa.pptx",
                                    "chunk_id": "1bf37f9f-f2d2-4ce6-a2ec-f7f0b996fdc3",
                                    "pages": [
                                    2
                                    ],
                                    "chunk_text": "Establish cost targets across taxonomies. ..."
                                },
                                {
                                    "documentId": "8a23f962-2257-4212-8eea-40b668092e04",
                                    "documentName": "EY Capital Edge Platform_8a23f962-2257-4212-8eea-40b668092e04.pptx",
                                    "chunk_id": "7b85cf41-985a-4460-99ba-1a6266beb7db",
                                    "pages": [
                                    1,
                                    2
                                    ],
                                    "chunk_text": "EY's Capital Edge Platform. ..."
                                }
                                ]
                            },
                            {
                                "sourceName": "ey-ip",
                                "sourceType": "EYIP Database",
                                "content": "Final generated output to be used to generate workplan for HR",
                                "sourceValue": [
                                {
                                    "tableName": "workplan",
                                    "rowCount": 33,
                                    "sqlQuery": ""
                                }
                                ]
                            }
                            ]
                        }
                        ],
                        "createdTime": "2024-06-06T22:02:17Z",
                        "lastUpdatedTime": "2024-06-06T22:02:30Z"
                    }
                    }
            ]
        }
    }
