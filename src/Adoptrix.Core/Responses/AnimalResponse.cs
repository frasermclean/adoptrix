namespace Adoptrix.Core.Responses;

public class AnimalResponse
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
    public required Sex Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required DateTime LastModifiedUtc { get; init; }
    public required IEnumerable<AnimalImageResponse> Images { get; init; }
}
