using Adoptrix.Domain;

namespace Adoptrix.Application.Models;

public class SearchAnimalsResult
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
    public required Sex Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required ImageResult? Image { get; init; }
}
