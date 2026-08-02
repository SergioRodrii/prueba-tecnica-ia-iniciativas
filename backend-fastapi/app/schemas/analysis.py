from pydantic import BaseModel, ConfigDict


class AnalysisResult(BaseModel):
    model_config = ConfigDict(extra="forbid")

    business_problem: str
    suggested_objectives: list[str]
    expected_benefits: list[str]
    risks: list[str]
    open_questions: list[str]
