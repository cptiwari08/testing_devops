# This is the only file which do not follow pep8
# to expose pascal case to API Clients
# app/pmo_workplan/schemas/workplan_output.py
from __future__ import annotations

from datetime import datetime
from typing import List, Optional, Union

from pydantic import BaseModel, UUID4, Field
from app.core.schemas import RuntimeStatus
from app.pmo_workplan.schemas.workplan_input import WorkplanInput

"""
title sometimes arrives as a nested dict	
    -> Introduced RichTitle and made Task.title a Union[str, RichTitle].
"""
class RichTitle(BaseModel):
    """When the generator returns a dict instead of a plain string."""
    description: str
    StartDate: Optional[str] = None
    TaskDueDate: Optional[str] = None

    model_config = {"extra": "ignore"}


class Task(BaseModel):
    """
    A Task, Summary Task or Milestone.  
    `title` can be either a simple string or the richer object above.
    """
    title: Union[str, RichTitle]
    workPlanTaskType: str
    StartDate: Optional[str] = None
    TaskDueDate: Optional[str] = None
    children: Optional[List["Task"]] = None  # type: ignore[self]

    model_config = {"extra": "ignore"}


Task.model_rebuild()  # enable forward refs

class ProjectTeam(BaseModel):
    id: int
    title: str


class SourceValuePrjDcs(BaseModel):
    documentId: str
    documentName: str
    chunk_id: str
    pages: List[int]
    chunk_text: str


class SourceValueEYIP(BaseModel):
    tableName: str
    rowCount: int
    sqlQuery: str


SourceValue = Union[SourceValuePrjDcs, SourceValueEYIP]


class CitingSources(BaseModel):        
    sourceName: str
    sourceType: str
    content: str
    sourceValue: List[SourceValue] = []
    model_config = {"extra": "ignore"}

class ResponseOutput(BaseModel):
    projectTeam: ProjectTeam
    content: List[Task]
    status: Optional[str] = None
    citingSources: Optional[List[CitingSources]] = None
    chainOfThoughts: Optional[str] = None  # present in your sample

    model_config = {"extra": "ignore"}


class Output(BaseModel):
    response: List[ResponseOutput] = Field(default_factory=list)
    citingSources: Optional[List[CitingSources]] = None

    model_config = {"extra": "ignore"}


class WorkplanOutput(BaseModel):
    """Top-level message returned by the Durable Function orchestrator."""
    name: str
    instanceId: UUID4
    runtimeStatus: RuntimeStatus
    errorMessage: Optional[str] = None
    input: WorkplanInput
    output: Output
    createdTime: Optional[datetime] = None
    lastUpdatedTime: datetime

    model_config = {
        "extra": "ignore",
         "json_schema_extra": {
            "examples": [
                {
                    "name": "WorkPlanGenerator",
                    "instanceId": "3ea287ad968c44fdb3b4ca6e93c1b17a",
                    "runtimeStatus": "Completed",
                    "input": {
                        "projectTeams": ["HR", "IT"],
                        "durationInMonth": 12,
                        "projectDetails": {
                            "sector": "Information Technology",
                            "subSector": "Software",
                            "transactionType": "Buy&Integrate",
                        },
                        "projectDocs": [100001, 100002, 10000003],
                    },
                    "output": {
                        "response": [
                            {
                                "projectTeam": {"id": 10001, "title": "HR"},
                                "content": [
                                    {
                                        "title": "Ensure IT Systems Alignment",
                                        "workPlanTaskType": "Summary Task",
                                        "StartDate": "2025-02-11T12:00:00+00:00",
                                        "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                        "children": [
                                            {
                                                "title": "Migrate All User Data by Q2",
                                                "workPlanTaskType": "Milestone",
                                                 "StartDate": "2025-02-11T12:00:00+00:00",
                                                 "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            },
                                            {
                                                "title": "Complete Data Audit Before Migration",
                                                "workPlanTaskType": "Task",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            },
                                            {
                                                "title": "Identify Systems for Data Migration",
                                                "workPlanTaskType": "Task",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            }
                                        ],
                                    }
                                ],
                            },
                            {
                                "projectTeam": {"id": 10002, "title": "IT"},
                                "content": [
                                    {
                                        "title": "Align Employee Benefits and Policies",
                                        "workPlanTaskType": "Summary Task",
                                        "StartDate": "2025-02-11T12:00:00+00:00",
                                        "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                        "children": [
                                            {
                                                "title": "Finalize New Compensation Structure",
                                                "workPlanTaskType": "Milestone",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            },
                                            {
                                                "title": "Benchmark Compensation Against Industry Standards",
                                                "workPlanTaskType": "Task",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            },
                                            {
                                                "title": "Roll Out New Compensation Plans",
                                                "workPlanTaskType": "Task",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            },
                                        ],
                                    }
                                ],
                            },
                            {
                                "name": "Finance",
                                "data": [
                                    {
                                        "title": "Integrate Financial Reporting Systems",
                                        "workPlanTaskType": "Summary Task",
                                        "StartDate": "2025-02-11T12:00:00+00:00",
                                        "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                        "children": [
                                            {
                                                "title": "Complete Consolidation of Financial Systems by Q3",
                                                "workPlanTaskType": "Milestone",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            },
                                            {
                                                "title": "Map Existing Financial Systems",
                                                "workPlanTaskType": "Task",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            },
                                            {
                                                "title": "Establish Consolidation Protocols",
                                                "workPlanTaskType": "Task",
                                                "StartDate": "2025-02-11T12:00:00+00:00",
                                                "TaskDueDate": "2025-02-11T12:00:00+00:00",
                                            }
                                        ],
                                    }
                                ],
                            },
                        ]
                    },
                    "createdTime": "2024-06-06T22:02:17Z",
                    "lastUpdatedTime": "2024-06-06T22:02:30Z",
                }
            ]
        }
    }   