using BackendDotnet.DTOs;

namespace BackendDotnet.Services;

public interface IInitiativeService
{
    Task<InitiativeResponse> CreateAsync(CreateInitiativeRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<InitiativeResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<InitiativeResponse> GetByIdAsync(int initiativeId, CancellationToken cancellationToken);
    Task<AnalyzeInitiativeResponse> AnalyzeAsync(int initiativeId, CancellationToken cancellationToken);
}
