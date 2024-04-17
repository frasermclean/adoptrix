using FluentResults;

namespace Adoptrix.Application.Errors;

public class AnimalNotFoundError(Guid animalId)
    : Error($"Could not find animal with ID {animalId}"), INotFoundError;
