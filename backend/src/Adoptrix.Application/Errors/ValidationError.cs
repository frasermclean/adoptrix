using FluentResults;

namespace Adoptrix.Application.Errors;

public class ValidationError : Error
{
    public const string PropertyNameKey = "PropertyName";
    private const string AttemptedValueKey = "AttemptedValue";

    public ValidationError(string message, string propertyName, object attemptedValue)
        : base(message)
    {
        Metadata.Add(PropertyNameKey, propertyName);
        Metadata.Add(AttemptedValueKey, attemptedValue);
    }
}
