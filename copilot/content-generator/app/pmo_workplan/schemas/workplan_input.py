# This is the only file which do not follow pep8
# to expose pascal case to API Clients
from uuid import UUID
from typing import List
from pydantic import BaseModel, ConfigDict


class ProjectDetails(BaseModel):
    sector: str | None
    subSector: str | None
    transactionType: str


class ProjectTeams(BaseModel):
    id: int | None
    title: str | None


class NodeMapping(BaseModel):
    projectTeamId: int
    mappedNodeId: List[int]


class OpModel(BaseModel):
    includedComponents: List[str]
    nodeMappings: List[NodeMapping]


class CEApps(BaseModel):
    opModel: OpModel


class ProjectCharterParam(BaseModel):
    charterIds: List[int]


class ProjectCharterApp(BaseModel):
    name: str = "projectCharter"
    param: ProjectCharterParam


class OpModelApp(BaseModel):
    name: str = "opModel"
    param: OpModel


CEAppType = OpModelApp | ProjectCharterApp


class WorkplanInput(BaseModel):
    instanceId: UUID
    projectTeams: list[ProjectTeams]
    durationInMonth: int | None
    projectDetails: ProjectDetails
    projectDocs: list[UUID] = []
    ceApps: list[CEAppType] | None  # Now a list of apps

    model_config: ConfigDict = {
        "json_schema_extra": {
            "examples": [
                {
                    "durationInMonth": 12,
                    "instanceId": "deb954ee-e7c1-4b6e-8eff-15c8dad74bdf",
                    "projectDetails": {
                        "subSector": "Healthcare Technology",
                        "sector": "Healthcare",
                        "transactionType": "Sell & Separate",
                    },
                    "projectDocs": [
                        "b3cad6d7-aa82-45d6-97af-17f22484d7ac",
                        "ca2ef4ae-778e-487e-a4c0-4e7a45612494",
                        "40f0c817-9259-4ce4-b3c7-4ca821006774",
                        "4497bc30-6e73-4729-a153-fba9bdde9d95",
                    ],
                    "projectTeams": [
                        {"title": "HR", "id": 59},
                        {"title": "IT", "id": 63},
                    ],
                    "ceApps": [
                        {
                            "name": "opModel",
                            "param": {
                                "includedComponents": ["PROCESSES"],
                                "nodeMappings": [
                                    {
                                        "projectTeamId": 59,
                                        "mappedNodeId": [501, 10001, 1000001, 1000002, 1000003, 1000004],
                                    },
                                    {
                                        "projectTeamId": 63,
                                        "mappedNodeId": [602, 1000001, 1000002, 1000003, 1000004],
                                    },
                                ],
                            },
                        },
                        {
                            "name": "projectCharter",
                            "param": {"charterIds": [1000001]},
                        },
                    ],
                }
            ]
        }
    }