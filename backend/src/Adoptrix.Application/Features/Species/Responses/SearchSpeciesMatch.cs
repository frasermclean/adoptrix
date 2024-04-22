namespace Adoptrix.Application.Features.Species.Responses;

public class SearchSpeciesMatch
{
    public required Guid SpeciesId { get; init; }
    public required string SpeciesName { get; init; }
    public required int BreedCount { get; init; }
    public required int AnimalCount { get; init; }
}
