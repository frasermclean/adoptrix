using Adoptrix.Application.Errors;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Adoptrix.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Problem(IList<IError> errors)
    {
        if (errors.Any(error => error is ValidationError))
        {
            return ValidationProblem(errors.OfType<ValidationError>());
        }

        var (statusCode, detail) = errors.First() switch
        {
            INotFoundError notFoundError => (StatusCodes.Status404NotFound, notFoundError.Message),
            _ => (StatusCodes.Status500InternalServerError, null)
        };

        return Problem(
            detail: detail,
            statusCode: statusCode);
    }

    private ActionResult ValidationProblem(IEnumerable<ValidationError> errors)
    {
        var dictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            if (error.Metadata[ValidationError.PropertyNameKey] is not string key)
            {
                continue;
            }

            dictionary.AddModelError(key, error.Message);
        }

        return ValidationProblem(dictionary);
    }
}
