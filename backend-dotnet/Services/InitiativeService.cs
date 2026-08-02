using System.Text.Json;
using BackendDotnet.Clients;
using BackendDotnet.DTOs;
using BackendDotnet.Models;
using BackendDotnet.Repositories;

namespace BackendDotnet.Services;

public sealed class InitiativeService(IInitiativeRepository repository, IAnalysisClient analysisClient) : IInitiativeService
{
    public async Task<InitiativeResponse> CreateAsync(CreateInitiativeRequest request, CancellationToken cancellationToken)
    {
        var initiative = new Initiative
        {
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            BusinessProblem = request.BusinessProblem,
            ExpectedBenefit = request.ExpectedBenefit,
            CreatedAt = DateTime.UtcNow,
        };

        return ToResponse(await repository.CreateAsync(initiative, cancellationToken));
    }

    public async Task<IReadOnlyList<InitiativeResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var initiatives = await repository.GetAllAsync(cancellationToken);
        return initiatives.Select(ToResponse).ToList();
    }

    public async Task<InitiativeResponse> GetByIdAsync(int initiativeId, CancellationToken cancellationToken)
    {
        return ToResponse(await GetExistingInitiativeAsync(initiativeId, cancellationToken));
    }

    public async Task<AnalyzeInitiativeResponse> AnalyzeAsync(int initiativeId, CancellationToken cancellationToken)
    {
        var initiative = await GetExistingInitiativeAsync(initiativeId, cancellationToken);
        AnalyzeInitiativeResponse analysis;
        try
        {
            analysis = await analysisClient.AnalyzeAsync(initiativeId, cancellationToken);
        }
        catch (AnalysisClientUnavailableException exception)
        {
            throw new AnalysisServiceUnavailableException("El servicio de análisis no está disponible.", exception);
        }
        catch (AnalysisClientResponseException exception)
        {
            throw new AnalysisServiceFailureException("El servicio de análisis devolvió una respuesta inválida.", exception);
        }

        await repository.SaveAnalysisAsync(initiative, JsonSerializer.Serialize(analysis), cancellationToken);
        return analysis;
    }

    private async Task<Initiative> GetExistingInitiativeAsync(int initiativeId, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(initiativeId, cancellationToken)
            ?? throw new InitiativeNotFoundException(initiativeId);
    }

    private static InitiativeResponse ToResponse(Initiative initiative)
    {
        JsonElement? analysisResult = string.IsNullOrWhiteSpace(initiative.AnalysisResult)
            ? null
            : JsonSerializer.Deserialize<JsonElement>(initiative.AnalysisResult);

        return new InitiativeResponse
        {
            Id = initiative.Id,
            Name = initiative.Name,
            Description = initiative.Description,
            Status = initiative.Status,
            BusinessProblem = initiative.BusinessProblem,
            ExpectedBenefit = initiative.ExpectedBenefit,
            CreatedAt = initiative.CreatedAt,
            AnalysisResult = analysisResult,
        };
    }
}
