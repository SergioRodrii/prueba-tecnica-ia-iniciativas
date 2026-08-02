namespace BackendDotnet.Services;

public sealed class InitiativeNotFoundException(int initiativeId) : Exception($"La iniciativa con id {initiativeId} no existe.");

public sealed class AnalysisServiceUnavailableException(string message, Exception innerException) : Exception(message, innerException);

public sealed class AnalysisServiceFailureException(string message, Exception innerException) : Exception(message, innerException);
