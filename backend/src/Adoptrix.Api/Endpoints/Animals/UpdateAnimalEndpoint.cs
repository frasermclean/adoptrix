using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, ValidationProblem, NotFound>> ExecuteAsync(
        Guid animalId,
        SetAnimalRequest request,
        IValidator<SetAnimalRequest> validator,
        ILogger<UpdateAnimalEndpoint> logger,
        IAnimalsService animalsService,
        CancellationToken cancellationToken)
    {
        // validate request
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var updateResult = await animalsService.UpdateAsync(animalId, request, cancellationToken);

        return updateResult.IsSuccess
            ? TypedResults.Ok(updateResult.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
