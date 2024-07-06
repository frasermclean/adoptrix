using FluentResults;

namespace Adoptrix.Core.Errors;

public class ImageUploadError(Guid animalId)
    : Error($"Could not upload images for animal with ID {animalId}");
