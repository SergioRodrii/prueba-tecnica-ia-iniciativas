namespace BackendDotnet.Models;

public sealed class Initiative
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "pending";
    public string? BusinessProblem { get; set; }
    public string? ExpectedBenefit { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? AnalysisResult { get; set; }
}
