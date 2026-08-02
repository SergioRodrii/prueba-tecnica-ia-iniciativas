from abc import ABC, abstractmethod
from typing import Any

from app.schemas.analysis import AnalysisResult


class AIProviderError(Exception):
    pass


class AIProviderUnavailableError(AIProviderError):
    pass


class AIProviderResponseError(AIProviderError):
    pass


class AIProvider(ABC):
    @abstractmethod
    def analyze(self, initiative: dict[str, Any]) -> AnalysisResult:
        raise NotImplementedError
