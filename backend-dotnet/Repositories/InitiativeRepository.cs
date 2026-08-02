using BackendDotnet.Data;
using BackendDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotnet.Repositories;

public sealed class InitiativeRepository(InitiativesDbContext database) : IInitiativeRepository
{
    public async Task<Initiative> CreateAsync(Initiative initiative, CancellationToken cancellationToken)
    {
        database.Initiatives.Add(initiative);
        await database.SaveChangesAsync(cancellationToken);
        return initiative;
    }

    public async Task<IReadOnlyList<Initiative>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await database.Initiatives
            .AsNoTracking()
            .OrderByDescending(initiative => initiative.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Initiative?> GetByIdAsync(int initiativeId, CancellationToken cancellationToken)
    {
        return database.Initiatives.FirstOrDefaultAsync(initiative => initiative.Id == initiativeId, cancellationToken);
    }

    public async Task SaveAnalysisAsync(Initiative initiative, string analysisResult, CancellationToken cancellationToken)
    {
        initiative.AnalysisResult = analysisResult;
        await database.SaveChangesAsync(cancellationToken);
    }
}
