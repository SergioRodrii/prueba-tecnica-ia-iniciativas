using BackendDotnet.Models;

namespace BackendDotnet.Repositories;

public interface IInitiativeRepository
{
    Task<Initiative> CreateAsync(Initiative initiative, CancellationToken cancellationToken);
    Task<IReadOnlyList<Initiative>> GetAllAsync(CancellationToken cancellationToken);
    Task<Initiative?> GetByIdAsync(int initiativeId, CancellationToken cancellationToken);
    Task SaveAnalysisAsync(Initiative initiative, string analysisResult, CancellationToken cancellationToken);
}
