using Adoptrix.Application.Errors;
using FluentResults;
using FluentValidation;
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

        var errors = result.Errors.Select(failure => new ValidationError(failure.ErrorMessage, failure.PropertyName));

        return (dynamic)Result.Fail(errors);
    }
}
