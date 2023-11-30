using FluentResults;

namespace Adoptrix.Domain.Errors;

public class BreedNotFoundError : Error
{
    public BreedNotFoundError(int id)
        : base($"Breed with id: {id} was not found.")
    {
    }

    public BreedNotFoundError(string name)
        : base($"Breed with name: {name} was not found.")
    {
    }
}