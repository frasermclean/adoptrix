namespace Adoptrix.Api.Contracts.Responses;

public class BreedResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid SpeciesId { get; init; }
    public required IEnumerable<Guid> AnimalIds { get; init; }
}
