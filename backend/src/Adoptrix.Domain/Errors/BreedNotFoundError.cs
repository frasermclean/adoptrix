using FluentResults;

namespace Adoptrix.Domain.Errors;

public class BreedNotFoundError(Guid breedId)
    : Error($"Breed with id: {breedId} was not found."), INotFoundError;
