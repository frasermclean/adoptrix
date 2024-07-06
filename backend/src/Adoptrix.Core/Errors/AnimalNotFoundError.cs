using FluentResults;

namespace Adoptrix.Core.Errors;

public class AnimalNotFoundError(Guid animalId)
    : Error($"Could not find animal with ID {animalId}"), INotFoundError;
