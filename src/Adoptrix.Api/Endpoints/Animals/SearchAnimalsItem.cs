using Adoptrix.Core;

namespace Adoptrix.Api.Endpoints.Animals;

public class SearchAnimalsItem
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
    public required Sex Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string? PreviewImageUrl { get; init; }
}
