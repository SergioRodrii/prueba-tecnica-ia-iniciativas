from abc import ABC, abstractmethod


class AnalysisProvider(ABC):
    @abstractmethod
    async def analyze(self) -> None:
        raise NotImplementedError
