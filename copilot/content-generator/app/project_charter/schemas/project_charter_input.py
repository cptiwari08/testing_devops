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
   
class ProjectCharterInput(BaseModel):
    instanceId: UUID
    projectTeam: ProjectTeam
    projectDetails: ProjectDetails
    projectDocs: List[str]
    eyIP: List[str]
    sections: List[str]

    model_config: ConfigDict = {
        "json_schema_extra": {
            "examples": [
                {
                    "instanceId": "3c389a84-6490-4dc1-a8e0-c1ad893c9534",
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
                }
            ]
        }
    }
