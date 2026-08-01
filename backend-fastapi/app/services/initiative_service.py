from fastapi import HTTPException, status
from sqlalchemy.orm import Session

from app.models.initiative import Initiative
from app.repositories.initiative_repository import InitiativeRepository
from app.schemas.initiative import InitiativeCreate


class InitiativeService:
    def __init__(self, database_session: Session) -> None:
        self.repository = InitiativeRepository(database_session)

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
