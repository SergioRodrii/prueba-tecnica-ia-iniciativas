from datetime import datetime, timezone
from typing import Any

from pydantic import BaseModel, ConfigDict, Field, field_serializer


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

    @field_serializer("created_at")
    def serialize_created_at(self, value: datetime) -> str:
        normalized_value = value if value.tzinfo is not None else value.replace(tzinfo=timezone.utc)
        return normalized_value.astimezone(timezone.utc).isoformat().replace("+00:00", "Z")
