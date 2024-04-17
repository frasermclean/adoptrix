using FluentResults;

namespace Adoptrix.Application.Errors;

public interface IValidationError : IError
{
    string PropertyName { get; }
    object AttemptedValue { get; }
}

public class ValidationError : Error, IValidationError
{
    public ValidationError(string message, string propertyName, object attemptedValue)
        : base(message)
    {
        Metadata.Add("PropertyName", propertyName);
        Metadata.Add("AttemptedValue", attemptedValue);
    }

    public string PropertyName => Metadata["PropertyName"] as string ?? string.Empty;
    public object AttemptedValue => Metadata["AttemptedValue"];
}
