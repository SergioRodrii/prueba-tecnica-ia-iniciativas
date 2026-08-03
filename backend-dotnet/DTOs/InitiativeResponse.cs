using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackendDotnet.DTOs;

public sealed class InitiativeResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? BusinessProblem { get; init; }
    public string? ExpectedBenefit { get; init; }
    [JsonConverter(typeof(UtcDateTimeConverter))]
    public DateTime CreatedAt { get; init; }
    public JsonElement? AnalysisResult { get; init; }
}
