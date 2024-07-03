namespace Adoptrix.Domain.Contracts.Responses;

public class SpeciesMatch
{
    public required Guid SpeciesId { get; init; }
    public required string SpeciesName { get; init; }
    public required int BreedCount { get; init; }
    public required int AnimalCount { get; init; }
}
