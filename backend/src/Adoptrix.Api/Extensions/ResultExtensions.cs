using Adoptrix.Application.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Extensions;

public static class ResultExtensions
{
    public static ProblemDetails ToProblemDetails(this List<IError> errors)
    {
        if (errors.Any(error => error is IValidationError))
        {
            return GetValidationProblemDetails(errors.OfType<IValidationError>());
        }

        var problemDetails = new ProblemDetails
        {
            Type = null,
            Title = null,
            Status = null,
            Detail = null,
            Instance = null,
            Extensions = null
        };

        return problemDetails;
    }

    private static ValidationProblemDetails GetValidationProblemDetails(IEnumerable<IValidationError> errors)
    {
        var validationProblemDetails = new ValidationProblemDetails
        {
            Type = null,
            Title = null,
            Status = null,
            Detail = null,
            Instance = null,
            Extensions = null,
            Errors = null
        };

        return validationProblemDetails;
    }
}
