namespace Adoptrix.Application.Features.Breeds.Responses;

public class SearchBreedsResult
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid SpeciesId { get; init; }
    public required int AnimalCount { get; init; }
}
