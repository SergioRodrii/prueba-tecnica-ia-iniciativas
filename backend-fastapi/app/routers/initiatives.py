from typing import Annotated

from fastapi import APIRouter, Depends, Path, status
from sqlalchemy.orm import Session

from app.core.database import get_db
from app.schemas.initiative import InitiativeCreate, InitiativeResponse
from app.services.initiative_service import InitiativeService


router = APIRouter(prefix="/initiatives", tags=["initiatives"])

DatabaseSession = Annotated[Session, Depends(get_db)]


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
