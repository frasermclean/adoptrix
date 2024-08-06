namespace Adoptrix.Persistence.Responses;

public class SearchSpeciesItem
{
    public required string Name { get; init; }
    public required int BreedCount { get; init; }
    public required int AnimalCount { get; init; }
}
