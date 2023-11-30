using FluentResults;

namespace Adoptrix.Domain.Errors;

public class BreedNotFoundError : Error
{
    public BreedNotFoundError(string name)
        : base($"Breed with name: {name} was not found.")
    {
    }
}