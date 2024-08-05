namespace Adoptrix.Persistence.Responses;

public class SearchBreedsItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
    public required int AnimalCount { get; init; }
}
