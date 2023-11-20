using FluentResults;

namespace Adoptrix.Application.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string? message = null)
        : base(message)
    {
    }

    public static readonly NotFoundError Instance = new();
}