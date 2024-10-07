namespace Adoptrix.Api.Endpoints.Species;

public class SpeciesResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int BreedCount { get; init; }
    public required int AnimalCount { get; init; }
}
