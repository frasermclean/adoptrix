using FluentResults;

namespace Adoptrix.Application.Errors;

public class ImageUploadError(Guid animalId)
    : Error($"Could not upload images for animal with ID {animalId}");
