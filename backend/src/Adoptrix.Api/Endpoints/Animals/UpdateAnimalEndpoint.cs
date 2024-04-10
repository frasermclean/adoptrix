using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Animals;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, ValidationProblem, NotFound>> ExecuteAsync(
        Guid animalId,
        SetAnimalData data,
        IValidator<SetAnimalData> validator,
        ILogger<UpdateAnimalEndpoint> logger,
        ISender sender,
        CancellationToken cancellationToken)
    {
        // validate request
        var validationResult = await validator.ValidateAsync(data, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", data);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        // update animal in database
        var updateResult = await sender.Send(new UpdateAnimalRequest(
            animalId,
            data.Name,
            data.Description,
            data.BreedId,
            data.Sex,
            data.DateOfBirth
        ), cancellationToken);

        return updateResult.IsSuccess
            ? TypedResults.Ok(updateResult.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
