using System.Text.Json.Serialization;
using Adoptrix.Domain;

namespace Adoptrix.Api.Contracts.Requests;

public class SetAnimalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    [JsonPropertyName("species")] public required string SpeciesName { get; init; }
    [JsonPropertyName("breed")] public string? BreedName { get; init; }
    public Sex? Sex { get; init; }
    public required DateOnly DateOfBirth { get; init; }

    public override string ToString() => $"{Name} ({SpeciesName})";
}
