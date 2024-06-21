using FluentResults;

namespace Adoptrix.Domain.Errors;

public class ImageUploadError(Guid animalId)
    : Error($"Could not upload images for animal with ID {animalId}");
