using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedEndpoint
{
    public static async
        Task<Results<Ok<BreedResponse>, NotFound, ValidationProblem>> ExecuteAsync(
            Guid breedId,
            SetBreedRequest request,
            IValidator<SetBreedRequest> validator,
            ILogger<UpdateBreedEndpoint> logger,
            IBreedsService breedsService,
            CancellationToken cancellationToken)
    {
        // find the breed by id
        var getResult = await breedsService.GetByIdAsync(breedId, cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

        // validate request
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await breedsService.UpdateAsync(breedId, request, cancellationToken);

        return TypedResults.Ok(result.Value.ToResponse());
    }
}
