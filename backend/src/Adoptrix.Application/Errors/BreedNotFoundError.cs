using FluentResults;

namespace Adoptrix.Application.Errors;

public class BreedNotFoundError : Error, INotFoundError
{
    public BreedNotFoundError(Guid id)
        : base($"Breed with id: {id} was not found.")
    {
    }

    public BreedNotFoundError(string name)
        : base($"Breed with name: {name} was not found.")
    {
    }
}
