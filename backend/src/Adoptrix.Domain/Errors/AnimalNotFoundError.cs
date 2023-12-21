using FluentResults;

namespace Adoptrix.Domain.Errors;

public class AnimalNotFoundError(Guid animalId)
    : Error($"Could not find animal with ID {animalId}");