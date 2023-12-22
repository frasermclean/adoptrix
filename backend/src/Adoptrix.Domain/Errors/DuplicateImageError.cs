using FluentResults;

namespace Adoptrix.Domain.Errors;

public class DuplicateImageError(string originalFileName)
    : Error($"Image with original filename {originalFileName} already exists");