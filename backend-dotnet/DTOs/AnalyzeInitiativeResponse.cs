using System.Text.Json.Serialization;

namespace BackendDotnet.DTOs;

public sealed class AnalyzeInitiativeResponse
{
    [JsonPropertyName("business_problem")]
    public string BusinessProblem { get; init; } = string.Empty;

    [JsonPropertyName("suggested_objectives")]
    public List<string>? SuggestedObjectives { get; init; } = new();

    [JsonPropertyName("expected_benefits")]
    public List<string>? ExpectedBenefits { get; init; } = new();

    [JsonPropertyName("risks")]
    public List<string>? Risks { get; init; } = new();

    [JsonPropertyName("open_questions")]
    public List<string>? OpenQuestions { get; init; } = new();

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(BusinessProblem)
            && SuggestedObjectives is not null && SuggestedObjectives.All(item => !string.IsNullOrWhiteSpace(item))
            && ExpectedBenefits is not null && ExpectedBenefits.All(item => !string.IsNullOrWhiteSpace(item))
            && Risks is not null && Risks.All(item => !string.IsNullOrWhiteSpace(item))
            && OpenQuestions is not null && OpenQuestions.All(item => !string.IsNullOrWhiteSpace(item));
    }
}
