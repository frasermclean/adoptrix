using FluentResults;

namespace Adoptrix.Domain.Errors;

public class InvalidContentTypeError : Error
{
    public InvalidContentTypeError(string? contentType)
        : base($"Invalid content type: {contentType}")
    {
    }
}