namespace Adoptrix.Api.Contracts.Responses;

public class BreedResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Species { get; init; }
    public required IEnumerable<string> AnimalIds { get; init; }
}