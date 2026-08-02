from typing import Any

from fastapi import HTTPException, status
from sqlalchemy.orm import Session

from app.ai.provider import AIProvider, AIProviderResponseError, AIProviderUnavailableError
from app.models.initiative import Initiative
from app.repositories.initiative_repository import InitiativeRepository
from app.schemas.analysis import AnalysisResult
from app.schemas.initiative import InitiativeCreate


class InitiativeService:
    def __init__(self, database_session: Session, ai_provider: AIProvider | None = None) -> None:
        self.repository = InitiativeRepository(database_session)
        self.ai_provider = ai_provider

    def create(self, payload: InitiativeCreate) -> Initiative:
        initiative = Initiative(**payload.model_dump())
        return self.repository.create(initiative)

    def get_all(self) -> list[Initiative]:
        return self.repository.get_all()

    def get_by_id(self, initiative_id: int) -> Initiative:
        initiative = self.repository.get_by_id(initiative_id)
        if initiative is None:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail=f"La iniciativa con id {initiative_id} no existe.",
            )
        return initiative

    def analyze(self, initiative_id: int) -> AnalysisResult:
        initiative = self.get_by_id(initiative_id)
        if self.ai_provider is None:
            raise HTTPException(
                status_code=status.HTTP_503_SERVICE_UNAVAILABLE,
                detail="El proveedor de análisis no está configurado.",
            )
        try:
            analysis = self.ai_provider.analyze(self._to_analysis_payload(initiative))
        except AIProviderUnavailableError as error:
            raise HTTPException(
                status_code=status.HTTP_503_SERVICE_UNAVAILABLE,
                detail="El servicio de análisis no está disponible.",
            ) from error
        except AIProviderResponseError as error:
            raise HTTPException(
                status_code=status.HTTP_502_BAD_GATEWAY,
                detail="El servicio de análisis devolvió una respuesta inválida.",
            ) from error

        self.repository.save_analysis_result(initiative, analysis.model_dump(mode="json"))
        return analysis

    @staticmethod
    def _to_analysis_payload(initiative: Initiative) -> dict[str, Any]:
        return {
            "id": initiative.id,
            "name": initiative.name,
            "description": initiative.description,
            "status": initiative.status,
            "business_problem": initiative.business_problem,
            "expected_benefit": initiative.expected_benefit,
        }
