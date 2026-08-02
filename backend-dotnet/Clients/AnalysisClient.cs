using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BackendDotnet.DTOs;

namespace BackendDotnet.Clients;

public sealed class AnalysisClient(HttpClient httpClient) : IAnalysisClient
{
    public async Task<AnalyzeInitiativeResponse> AnalyzeAsync(int initiativeId, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PostAsync($"/initiatives/{initiativeId}/analyze", null, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AnalysisClientResponseException("FastAPI no encontró la iniciativa solicitada.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new AnalysisClientUnavailableException("FastAPI no está disponible para realizar el análisis.");
            }

            var analysis = await response.Content.ReadFromJsonAsync<AnalyzeInitiativeResponse>(cancellationToken: cancellationToken);
            if (analysis is null || !analysis.IsValid())
            {
                throw new AnalysisClientResponseException("FastAPI devolvió un análisis inválido.");
            }

            return analysis;
        }
        catch (OperationCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            throw new AnalysisClientUnavailableException("FastAPI agotó el tiempo de espera.", exception);
        }
        catch (HttpRequestException exception)
        {
            throw new AnalysisClientUnavailableException("No fue posible conectar con FastAPI.", exception);
        }
        catch (JsonException exception)
        {
            throw new AnalysisClientResponseException("FastAPI devolvió una respuesta no válida.", exception);
        }
    }
}

public sealed class AnalysisClientUnavailableException(string message, Exception? innerException = null) : Exception(message, innerException);

public sealed class AnalysisClientResponseException(string message, Exception? innerException = null) : Exception(message, innerException);
