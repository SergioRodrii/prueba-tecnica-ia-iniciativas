using System.ComponentModel.DataAnnotations;

namespace BackendDotnet.DTOs;

public sealed class CreateInitiativeRequest
{
    [Required]
    [StringLength(255)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Description { get; init; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; init; } = "pending";

    public string? BusinessProblem { get; init; }
    public string? ExpectedBenefit { get; init; }
}
