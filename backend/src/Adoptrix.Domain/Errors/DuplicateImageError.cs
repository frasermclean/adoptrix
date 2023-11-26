using FluentResults;

namespace Adoptrix.Domain.Errors;

public class DuplicateImageError : Error
{
    public DuplicateImageError(string fileName)
        : base($"Image with filename {fileName} already exists")
    {
        Metadata.Add(nameof(fileName), fileName);
    }
}