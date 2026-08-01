from .provider import AnalysisProvider


class OllamaAnalysisProvider(AnalysisProvider):
    async def analyze(self) -> None:
        raise NotImplementedError
