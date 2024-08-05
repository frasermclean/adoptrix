namespace Adoptrix.Contracts.Responses;

public class AnimalResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required string SpeciesName { get; init; }
    public required Guid BreedId { get; init; }
    public required string BreedName { get; init; }
    public required string Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string Age { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required IEnumerable<AnimalImageResponse> Images { get; init; }
}
