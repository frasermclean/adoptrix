namespace Adoptrix.Contracts.Responses;

public class SpeciesMatch
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int BreedCount { get; init; }
    public required int AnimalCount { get; init; }
}
