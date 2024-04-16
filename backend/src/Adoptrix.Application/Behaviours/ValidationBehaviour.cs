using FluentResults;
using FluentValidation;
using MediatR;

namespace Adoptrix.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResultBase
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

        return await next();
    }
}
