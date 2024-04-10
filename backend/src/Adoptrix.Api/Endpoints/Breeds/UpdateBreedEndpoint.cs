using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Breeds;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedEndpoint
{
    public static async Task<Results<Ok<BreedResponse>, NotFound, ValidationProblem>> ExecuteAsync(
        Guid breedId,
        SetBreedData data,
        IValidator<SetBreedData> validator,
        ILogger<UpdateBreedEndpoint> logger,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(data, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", data);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(new UpdateBreedRequest(breedId, data.Name, data.SpeciesId),
            cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
