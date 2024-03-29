﻿using FluentResults;

namespace Adoptrix.Domain.Errors;

public class SpeciesNotFoundError : Error
{
    public SpeciesNotFoundError(Guid speciesId)
        : base($"Species with ID: {speciesId} was not found")
    {
    }

    public SpeciesNotFoundError(string speciesName)
        : base($"Species with name: {speciesName} was not found")
    {
    }
}
