from typing import Annotated

from fastapi import APIRouter, Depends, Path, status
from sqlalchemy.orm import Session

from app.ai.ollama_provider import OllamaProvider
from app.ai.provider import AIProvider
from app.core.config import settings
from app.core.database import get_db
from app.schemas.analysis import AnalysisResult
from app.schemas.initiative import InitiativeCreate, InitiativeResponse
from app.services.initiative_service import InitiativeService


router = APIRouter(prefix="/initiatives", tags=["initiatives"])

DatabaseSession = Annotated[Session, Depends(get_db)]


def get_ai_provider() -> AIProvider:
    return OllamaProvider(
        base_url=settings.ollama_base_url,
        model=settings.ollama_model,
        timeout_seconds=settings.ollama_timeout_seconds,
    )


AIProviderDependency = Annotated[AIProvider, Depends(get_ai_provider)]


@router.post("", response_model=InitiativeResponse, status_code=status.HTTP_201_CREATED)
def create_initiative(payload: InitiativeCreate, database_session: DatabaseSession) -> InitiativeResponse:
    return InitiativeService(database_session).create(payload)


@router.get("", response_model=list[InitiativeResponse])
def get_initiatives(database_session: DatabaseSession) -> list[InitiativeResponse]:
    return InitiativeService(database_session).get_all()


@router.get("/{initiative_id}", response_model=InitiativeResponse)
def get_initiative(
    initiative_id: Annotated[int, Path(gt=0)], database_session: DatabaseSession
) -> InitiativeResponse:
    return InitiativeService(database_session).get_by_id(initiative_id)


@router.post("/{initiative_id}/analyze", response_model=AnalysisResult)
def analyze_initiative(
    initiative_id: Annotated[int, Path(gt=0)],
    database_session: DatabaseSession,
    ai_provider: AIProviderDependency,
) -> AnalysisResult:
    return InitiativeService(database_session, ai_provider).analyze(initiative_id)
