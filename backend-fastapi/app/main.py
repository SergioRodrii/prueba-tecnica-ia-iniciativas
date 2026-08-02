from contextlib import asynccontextmanager

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from app.core.config import settings
from app.core.database import create_tables
from app.models.initiative import Initiative
from app.routers.initiatives import router as initiatives_router


@asynccontextmanager
async def lifespan(_: FastAPI):
    create_tables()
    yield


def create_app() -> FastAPI:
    application = FastAPI(lifespan=lifespan)
    allowed_origins = [origin.strip() for origin in settings.cors_allowed_origins.split(",") if origin.strip()]
    application.add_middleware(
        CORSMiddleware,
        allow_origins=allowed_origins,
        allow_credentials=False,
        allow_methods=["*"],
        allow_headers=["*"],
    )
    application.include_router(initiatives_router)
    return application


app = create_app()
