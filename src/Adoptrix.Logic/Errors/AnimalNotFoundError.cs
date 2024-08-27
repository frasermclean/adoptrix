using FluentResults;

namespace Adoptrix.Logic.Errors;

public class AnimalNotFoundError : Error
{
    public AnimalNotFoundError(Guid animalId) : base($"Animal with ID {animalId} not found.")
    {
    }

    public AnimalNotFoundError(string animalSlug) : base($"Animal with slug {animalSlug} not found.")
    {
    }
}
