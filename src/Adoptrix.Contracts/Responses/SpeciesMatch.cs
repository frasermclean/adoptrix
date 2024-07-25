namespace Adoptrix.Contracts.Responses;

public class SpeciesMatch
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required int BreedCount { get; init; }
    public required int AnimalCount { get; init; }
}
