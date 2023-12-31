﻿using System.Text.Json.Serialization;

namespace Adoptrix.Api.Contracts.Responses;

public class BreedResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    [JsonPropertyName("species")] public required string SpeciesName { get; init; }
    public required IEnumerable<Guid> AnimalIds { get; init; }
}