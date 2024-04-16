using FluentResults;

namespace Adoptrix.Application.Errors;

public class SpeciesNotFoundError : Error, INotFoundError
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
