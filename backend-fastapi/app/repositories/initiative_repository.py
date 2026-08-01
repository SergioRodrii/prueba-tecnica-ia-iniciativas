from sqlalchemy import select
from sqlalchemy.orm import Session

from app.models.initiative import Initiative


class InitiativeRepository:
    def __init__(self, database_session: Session) -> None:
        self.database_session = database_session

    def create(self, initiative: Initiative) -> Initiative:
        self.database_session.add(initiative)
        self.database_session.commit()
        self.database_session.refresh(initiative)
        return initiative

    def get_all(self) -> list[Initiative]:
        statement = select(Initiative).order_by(Initiative.created_at.desc())
        return list(self.database_session.scalars(statement))

    def get_by_id(self, initiative_id: int) -> Initiative | None:
        return self.database_session.get(Initiative, initiative_id)
