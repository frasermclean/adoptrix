namespace Adoptrix.Contracts.Responses;

public class SpeciesMatch
{
    public required string Name { get; init; }
    public required int BreedCount { get; init; }
    public required int AnimalCount { get; init; }
}
