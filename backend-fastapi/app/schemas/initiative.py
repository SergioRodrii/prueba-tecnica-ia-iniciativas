from datetime import datetime
from typing import Any

from pydantic import BaseModel, ConfigDict, Field


class InitiativeCreate(BaseModel):
    name: str = Field(min_length=1, max_length=255)
    description: str = Field(min_length=1)
    status: str = Field(default="pending", min_length=1, max_length=50)
    business_problem: str | None = None
    expected_benefit: str | None = None


class InitiativeResponse(BaseModel):
    model_config = ConfigDict(from_attributes=True)

    id: int
    name: str
    description: str
    status: str
    business_problem: str | None
    expected_benefit: str | None
    created_at: datetime
    analysis_result: dict[str, Any] | None
