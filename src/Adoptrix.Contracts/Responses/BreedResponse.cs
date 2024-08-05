namespace Adoptrix.Contracts.Responses;

public class BreedResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
}
