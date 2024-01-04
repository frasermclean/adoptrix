using System.Text.Json.Serialization;
using Adoptrix.Domain;

namespace Adoptrix.Api.Contracts.Responses;

public class AnimalResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    [JsonPropertyName("species")] public required string SpeciesName { get; init; }
    [JsonPropertyName("breed")] public required string? BreedName { get; init; }
    public required Sex? Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required IEnumerable<AnimalImageResponse> Images { get; init; }
}