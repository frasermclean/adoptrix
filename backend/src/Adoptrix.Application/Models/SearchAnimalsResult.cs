using Adoptrix.Domain;

namespace Adoptrix.Application.Models;

public class SearchAnimalsResult
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required string Species { get; init; }
    public required string? Breed { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required ImageInformation? PrimaryImage { get; init; }
}