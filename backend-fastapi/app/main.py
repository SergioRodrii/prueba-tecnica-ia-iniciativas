from contextlib import asynccontextmanager

from fastapi import FastAPI

from app.core.database import create_tables
from app.models.initiative import Initiative
from app.routers.initiatives import router as initiatives_router


@asynccontextmanager
async def lifespan(_: FastAPI):
    create_tables()
    yield


def create_app() -> FastAPI:
    application = FastAPI(lifespan=lifespan)
    application.include_router(initiatives_router)
    return application


app = create_app()
