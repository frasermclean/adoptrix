namespace Adoptrix.Core.Contracts.Responses;

public class BreedMatch
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid SpeciesId { get; init; }
    public required int AnimalCount { get; init; }
}
