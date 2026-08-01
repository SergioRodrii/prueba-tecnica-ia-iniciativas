from contextlib import asynccontextmanager

from fastapi import FastAPI

from app.core.database import create_tables
from app.models.initiative import Initiative


@asynccontextmanager
async def lifespan(_: FastAPI):
    create_tables()
    yield


def create_app() -> FastAPI:
    return FastAPI(lifespan=lifespan)


app = create_app()
