# This file does not follow PEP88
# to expose pascal case to API Clients

from datetime import datetime
from typing import List

from app.tsa_generator.schemas.tsa_generator_input import TSAGeneratorInput
from app.tsa_generator.schemas.scope_generator_input import ScopeGeneratorInput
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


class ResponseTSA(BaseModel):
    projectTeam: dict
    content: List[dict]
    status: str
    citingSources: List[dict]

class ResponseScope(BaseModel):
    content: str
    status: str
    sourceName: str


class Output(BaseModel):
    response: List[ResponseTSA] | List[ResponseScope] = []    


class TSAGeneratorOutput(BaseModel):
    name: str
    instanceId: UUID4
    runtimeStatus: str
    errorMessage: str | None = None
    input: TSAGeneratorInput | ScopeGeneratorInput
    output: Output
    createdTime: datetime | None = None
    lastUpdatedTime: datetime
    model_config: ConfigDict = {
        "json_schema_extra": {
            "examples": [
                {
                    "name": "TSAGenerator",
                    "instanceId": "3ea287ad968c44fdb3b4ca6e93c1b17a",
                    "runtimeStatus": "Completed",
                    "errorMessage": "Deserialization error",
                    "input": {
                        "projectTeams": [
                        {
                            "title": "HR",
                            "id": 10001
                        },
                        {
                            "title": "IT",
                            "id": 10002
                        }
                        ],
                        "projectDetails": {
                        "sector": "Information Technology",
                        "subSector": "Software",
                        "transactionType": "Integrate"
                        },
                        "projectDocs": [
                        "100001",
                        "100002",
                        "100003"
                        ],
                        "ceApps": {
                        "opModel": {
                            "includedComponents": [
                            "PROCESSES",
                            "SYSTEMS",
                            "FACILITIES",
                            "THIRD_PARTY_AGREEMENTS",
                            "ASSETS"
                            ],
                            "nodeMapping": [
                            {
                                "projectTeamId": 10001,
                                "mappedNodeId": 501
                            },
                            {
                                "projectTeamId": 10002,
                                "mappedNodeId": 602
                            }
                            ]
                        },
                        "appInventoryTracker": True
                        },
                        "eyIP": [
                        "10001",
                        "100002"
                        ]
                    },
                    "output": {
                        "response": [
                        {
                            "projectTeam": {
                            "title": "IT",
                            "id": 10002
                            },
                            "content": [
                            {
                                "title": "Ensure IT Systems Alignment",
                                "serviceInScopeDescription": "- Support for the transfer of inventory records, including physical stock and digital inventory management systems. - Coordination of inventory count and reconciliation to ensure accurate transition between the parties. - Integration of inventory systems between the seller and buyer to maintain continuity in tracking and management.",
                                "assistantGeneratorOrigins": [
                                "eyIP",
                                "OpModel"
                                ],
                                "associatedProcesses": [
                                10001,
                                10002
                                ],
                                "associatedSystems": [
                                10001,
                                10002
                                ],
                                "associatedFacilities": [
                                10001,
                                10002
                                ],
                                "associatedAssets": [
                                10001,
                                10002
                                ],
                                "associatedTPAs": [
                                10001,
                                10002
                                ],
                                "associatedAITs": [
                                10001,
                                10002
                                ]
                            },
                            {
                                "title": "Stock Valuation and Reporting",
                                "serviceInScopeDescription": "- Provision of detailed reports on the value of inventory, including stock levels, product conditions, and aging inventory. - Support in performing stock audits and valuations for accounting and financial reporting purposes. - Assistance in identifying obsolete or slow-moving stock to support post-merger planning.",
                                "assistantGeneratorOrigins": [
                                "EYIP",
                                "Op model"
                                ],
                                "associatedProcesses": [
                                10001,
                                10002
                                ],
                                "associatedSystems": [
                                10001,
                                10002
                                ],
                                "associatedFacilities": [
                                10001,
                                10002
                                ],
                                "associatedAssets": [
                                10001,
                                10002
                                ],
                                "associatedTPAs": [
                                10001,
                                10002
                                ],
                                "associatedAITs": [
                                10001,
                                10002
                                ]
                            },
                            {
                                "title": "Cycle Counting and Stock Audits",
                                "serviceInScopeDescription": "- Provision of detailed reports on the value of inventory, including stock levels, product conditions, and aging inventory. - Support in performing stock audits and valuations for accounting and financial reporting purposes. - Assistance in identifying obsolete or slow-moving stock to support post-merger planning.",
                                "assistantGeneratorOrigins": [
                                "EYIP",
                                "Op model"
                                ],
                                "associatedProcesses": [
                                10001,
                                10002
                                ],
                                "associatedSystems": [
                                10001,
                                10002
                                ],
                                "associatedFacilities": [
                                10001,
                                10002
                                ],
                                "associatedAssets": [
                                10001,
                                10002
                                ],
                                "associatedTPAs": [
                                10001,
                                10002
                                ],
                                "associatedAITs": [
                                10001,
                                10002
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
                            },
                            {
                                "sourceName": "op-model",
                                "sourceType": "Op Model App",
                                "content": "Final generated output to be used to generate workplan for HR",
                                "sourceValue": [
                                {
                                    "Categoy": "PROCESSES",
                                    "rowCount": 33
                                },
                                {
                                    "Categoy": "SYSTEMS",
                                    "rowCount": 10
                                },
                                {
                                    "Categoy": "FACILITIES",
                                    "rowCount": 33
                                },
                                {
                                    "Categoy": "THIRD_PARTY_AGREEMENTS",
                                    "rowCount": 10
                                },
                                {
                                    "Categoy": "ASSETS",
                                    "rowCount": 33
                                }
                                ]
                            },
                            {
                                "sourceName": "app-inventory-tracker",
                                "sourceType": "App Inventory Tracker App",
                                "content": "Final generated output to be used to generate workplan for HR",
                                "sourceValue": [
                                {
                                    "tableName": "AppInventory",
                                    "rowCount": 33,
                                    "sqlQuery": "select * from AppInventory"
                                }
                                ]
                            }
                            ]
                        },
                        {
                            "projectTeam": {
                            "title": "HR",
                            "id": 10001
                            },
                            "content": [
                            {
                                "title": "Ensure IT Systems Alignment",
                                "serviceInScopeDescription": "- Support for the transfer of inventory records, including physical stock and digital inventory management systems. - Coordination of inventory count and reconciliation to ensure accurate transition between the parties. - Integration of inventory systems between the seller and buyer to maintain continuity in tracking and management.",
                                "assistantGeneratorOrigins": [
                                "EYIP",
                                "Op model"
                                ],
                                "associatedProcesses": [
                                10001,
                                10002
                                ],
                                "associatedSystems": [
                                10001,
                                10002
                                ],
                                "associatedFacilities": [
                                10001,
                                10002
                                ],
                                "associatedAssets": [
                                10001,
                                10002
                                ],
                                "associatedTPAs": [
                                10001,
                                10002
                                ],
                                "associatedAITs": [
                                10001,
                                10002
                                ]
                            },
                            {
                                "title": "Stock Valuation and Reporting",
                                "serviceInScopeDescription": "- Provision of detailed reports on the value of inventory, including stock levels, product conditions, and aging inventory. - Support in performing stock audits and valuations for accounting and financial reporting purposes. - Assistance in identifying obsolete or slow-moving stock to support post-merger planning.",
                                "assistantGeneratorOrigins": [
                                "EYIP",
                                "Op model"
                                ],
                                "associatedProcesses": [
                                10001,
                                10002
                                ],
                                "associatedSystems": [
                                10001,
                                10002
                                ],
                                "associatedFacilities": [
                                10001,
                                10002
                                ],
                                "associatedAssets": [
                                10001,
                                10002
                                ],
                                "associatedTPAs": [
                                10001,
                                10002
                                ],
                                "associatedAITs": [
                                10001,
                                10002
                                ]
                            },
                            {
                                "title": "Cycle Counting and Stock Audits",
                                "serviceInScopeDescription": "- Provision of detailed reports on the value of inventory, including stock levels, product conditions, and aging inventory. - Support in performing stock audits and valuations for accounting and financial reporting purposes. - Assistance in identifying obsolete or slow-moving stock to support post-merger planning.",
                                "assistantGeneratorOrigins": [
                                "EYIP",
                                "Op model"
                                ],
                                "associatedProcesses": [
                                10001,
                                10002
                                ],
                                "associatedSystems": [
                                10001,
                                10002
                                ],
                                "associatedFacilities": [
                                10001,
                                10002
                                ],
                                "associatedAssets": [
                                10001,
                                10002
                                ],
                                "associatedTPAs": [
                                10001,
                                10002
                                ],
                                "associatedAITs": [
                                10001,
                                10002
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
                            },
                            {
                                "sourceName": "op-model",
                                "sourceType": "Op Model App",
                                "content": "Final generated output to be used to generate workplan for HR",
                                "sourceValue": [
                                {
                                    "Categoy": "PROCESSES",
                                    "rowCount": 33
                                },
                                {
                                    "Categoy": "SYSTEMS",
                                    "rowCount": 10
                                },
                                {
                                    "Categoy": "FACILITIES",
                                    "rowCount": 33
                                },
                                {
                                    "Categoy": "THIRD_PARTY_AGREEMENTS",
                                    "rowCount": 10
                                },
                                {
                                    "Categoy": "ASSETS",
                                    "rowCount": 33
                                }
                                ]
                            },
                            {
                                "sourceName": "app-inventory-tracker",
                                "sourceType": "App Inventory Tracker App",
                                "content": "Final generated output to be used to generate workplan for HR",
                                "sourceValue": [
                                {
                                    "tableName": "AppInventory",
                                    "rowCount": 33,
                                    "sqlQuery": "select * from AppInventory"
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
