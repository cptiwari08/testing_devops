# This is the only file which do not follow pep8
# to expose pascal case to API Clients
from typing import List
from uuid import UUID

from pydantic import BaseModel


class ProjectTeam(BaseModel):
    title: str
    id: int


class ProjectDetails(BaseModel):
    sector: str
    subSector: str
    transactionType: str


class ItemData(BaseModel):
    id: int


class ScopeGeneratorInput(BaseModel):
    instanceId: UUID
    projectTeams: List[ProjectTeam]
    projectDetails: ProjectDetails
    itemData: ItemData


scope_generator_input_example = {
    "instanceId": "3c389a84-6490-4dc1-a8e0-c1ad893c9534",
    "projectTeams": [
        {
            "title": "HR",
            "id": 10001
        }
    ],
    "itemData": {
        "id": 100001
    },
    "projectDetails": {
                    "sector": "Information Technology",
                    "subSector": "Software",
                    "transactionType": "Integrate"
    }
}
