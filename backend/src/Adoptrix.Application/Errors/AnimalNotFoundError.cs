using FluentResults;

namespace Adoptrix.Application.Errors;

public class AnimalNotFoundError : Error, INotFoundError
{
    public AnimalNotFoundError(Guid animalId)
        : base($"Could not find animal with ID: {animalId}")
    {
    }

    public AnimalNotFoundError(string animalSlug)
        : base($"Could not find animal with slug: {animalSlug}")
    {
    }
}
