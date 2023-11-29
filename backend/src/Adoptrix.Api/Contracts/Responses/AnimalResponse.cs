using Adoptrix.Domain;

namespace Adoptrix.Api.Contracts.Responses;

public class AnimalResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required string SpeciesName { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required IEnumerable<AnimalImageResponse> Images { get; init; }
}