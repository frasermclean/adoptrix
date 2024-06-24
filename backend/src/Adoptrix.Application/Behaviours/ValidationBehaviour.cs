using Adoptrix.Domain.Errors;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Adoptrix.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // no registered validator
        if (validator is null)
        {
            return await next();
        }

        // validate request
        var result = await validator.ValidateAsync(request, cancellationToken);
        if (result.IsValid)
        {
            return await next();
        }

        var errors = result.Errors.Select(MapFailureToValidationError);

        return (dynamic)Result.Fail(errors);
    }

    private static ValidationError MapFailureToValidationError(ValidationFailure failure)
        => new(failure.ErrorMessage, failure.PropertyName, failure.AttemptedValue);
}
