﻿namespace Adoptrix.Contracts.Requests;

public class SearchBreedsRequest
{
    public string? SpeciesName { get; init; }
    public bool? WithAnimals { get; init; }
}
