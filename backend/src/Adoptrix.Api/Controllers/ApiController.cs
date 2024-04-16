using Adoptrix.Application.Errors;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected ObjectResult Problem(IEnumerable<IError> errors)
    {
        var (statusCode, detail) = errors.First() switch
        {
            ValidationError => (StatusCodes.Status400BadRequest, "A validation error occurred"),
            INotFoundError notFoundError => (StatusCodes.Status404NotFound, notFoundError.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unhandled error occurred")
        };

        return Problem(
            detail: detail,
            statusCode: statusCode);
    }
}
