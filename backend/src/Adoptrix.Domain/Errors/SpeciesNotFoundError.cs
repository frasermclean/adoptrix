using FluentResults;

namespace Adoptrix.Domain.Errors;

public class SpeciesNotFoundError : Error
{
    public SpeciesNotFoundError(string speciesName)
        : base($"Species with name: {speciesName} was not found")
    {
    }
}