using FluentResults;

namespace Adoptrix.Application.Errors;

public class BreedNotFoundError(Guid breedId)
    : Error($"Breed with id: {breedId} was not found."), INotFoundError;
