﻿namespace Adoptrix.Persistence.Responses;

public class SearchBreedsItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }
    public required int AnimalCount { get; init; }
}
