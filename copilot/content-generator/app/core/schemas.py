from enum import Enum
from typing import Optional

from pydantic import BaseModel, Field


class PromptTemplate(BaseModel):
    text: str = Field(..., description="Prompt data")
    few_shots: Optional[list[str]] = Field(None, description="Few shots data")



class RuntimeStatus(str, Enum):
    in_progress = "In Progress"
    completed = "Completed"
    failed = "Failed"
    aborted = "Aborted"
