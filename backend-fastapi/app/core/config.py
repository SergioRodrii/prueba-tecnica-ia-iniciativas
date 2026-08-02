from pathlib import Path

from pydantic_settings import BaseSettings, SettingsConfigDict


project_root = Path(__file__).resolve().parents[3]


class Settings(BaseSettings):
    model_config = SettingsConfigDict(env_file=project_root / ".env", extra="ignore")

    database_url: str
    
    ollama_base_url: str
    ollama_model: str
    ollama_timeout_seconds: float
    
    cors_allowed_origins: str


settings = Settings()
