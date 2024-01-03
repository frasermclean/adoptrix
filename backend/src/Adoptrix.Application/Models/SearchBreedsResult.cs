namespace Adoptrix.Application.Models;

public class SearchBreedsResult
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
    public required IEnumerable<Guid> AnimalIds { get; init; }
}