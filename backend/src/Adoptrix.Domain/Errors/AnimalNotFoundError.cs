using FluentResults;

namespace Adoptrix.Domain.Errors;

public class AnimalNotFoundError : Error
{
    public AnimalNotFoundError(int animalId)
        : base($"Could not find animal with ID {animalId}")
    {
    }
}