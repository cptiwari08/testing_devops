# This is the only file which do not follow pep8
# to expose pascal case to API Clients
from typing import List
from uuid import UUID

from pydantic import BaseModel, ConfigDict


class ProjectTeam(BaseModel):
    title: str
    id: int


class ProjectDetails(BaseModel):
    sector: str
    subSector: str
    transactionType: str


class OpModel(BaseModel):
    includedComponents: list
    nodeMappings: list


class AIT(BaseModel):
    included: bool


class CEApp(BaseModel):
    name: str
    param: OpModel | AIT


class EYIPItem(BaseModel):
    projectTeam: str
    templates: list[int]
    mappedIPProjectTeams: list[str]


class TSAGeneratorInput(BaseModel):
    instanceId: UUID
    projectTeams: List[ProjectTeam]
    projectDetails: ProjectDetails
    projectDocs: List[str]
    eyIP: List[EYIPItem]
    ceApps: List[CEApp] | list


tsa_generator_input_example = {
                "instanceId": "3c389a84-6490-4dc1-a8e0-c1ad893c9534",
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
                "ceApps": [
                    {
                    "name": "opModel",
                    "param": {
                        "includedComponents": [
                        "PROCESSES",
                        "SYSTEMS",
                        "FACILITIES",
                        "THIRD_PARTY_AGREEMENTS",
                        "ASSETS"
                        ],
                        "nodeMappings": [
                        {
                            "projectTeamId": 59,
                            "mappedNodeId": [
                            501,
                            10001
                            ]
                        },
                        {
                            "projectTeamId": 63,
                            "mappedNodeId": [
                            602
                            ]
                        }
                        ]
                    }
                    },
                    {
                    "name": "appInventoryTracker",
                    "param": {
                            "included": True
                            }
                    }
                ],
                "eyIP": [
                    {
                        "projectTeam": "HR",
                        "templates": [10001, 10002],
                        "mappedIPProjectTeams": ["Human Resource", "HR"]
                    },
                    {
                        "projectTeam": "IT",
                        "templates": [10001, 10002],
                        "mappedIPProjectTeams": ["Information Technology", "IT"]
                    }
                ]
            }
