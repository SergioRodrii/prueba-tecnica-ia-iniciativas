using BackendDotnet.DTOs;

namespace BackendDotnet.Clients;

public interface IAnalysisClient
{
    Task<AnalyzeInitiativeResponse> AnalyzeAsync(int initiativeId, CancellationToken cancellationToken);
}
