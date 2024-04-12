using FluentResults;

namespace Adoptrix.Application.Errors;

public class AnimalImageNotFoundError(Guid imageId, Guid animalId)
    : Error($"Could not find image with ID {imageId} for animal with ID {animalId}");
