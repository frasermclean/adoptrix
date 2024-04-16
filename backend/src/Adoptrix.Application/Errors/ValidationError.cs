using FluentResults;

namespace Adoptrix.Application.Errors;

public class ValidationError : Error
{
    public const string PropertyNameKey = "PropertyName";

    public ValidationError(string message, string propertyName)
        : base(message)
    {
        Metadata.Add(PropertyNameKey, propertyName);
    }
}
