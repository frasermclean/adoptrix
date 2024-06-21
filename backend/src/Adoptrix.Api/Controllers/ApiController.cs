using Adoptrix.Domain.Errors;
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
        if (errors.Any(error => error is IValidationError))
        {
            return ValidationProblem(errors.OfType<IValidationError>());
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

    private ActionResult ValidationProblem(IEnumerable<IValidationError> errors)
    {
        var dictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            dictionary.AddModelError(error.PropertyName, error.Message);
        }

        return ValidationProblem(dictionary);
    }
}
