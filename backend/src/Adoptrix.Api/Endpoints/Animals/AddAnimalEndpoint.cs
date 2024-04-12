using System.Security.Claims;
using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Animals.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class AddAnimalEndpoint
{
    public static async Task<Results<Created<AnimalResponse>, ValidationProblem>> ExecuteAsync(
        SetAnimalData data,
        ClaimsPrincipal claimsPrincipal,
        IValidator<SetAnimalData> validator,
        ILogger<AddAnimalEndpoint> logger,
        ISender sender,
        LinkGenerator linkGenerator,
        CancellationToken cancellationToken)
    {
        // validate request
        var validationResult = await validator.ValidateAsync(data, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", data);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        // add animal to database
        var addAnimalResult = await sender.Send(new AddAnimalCommand(
                data.Name,
                data.Description,
                data.BreedId,
                data.Sex,
                data.DateOfBirth,
                claimsPrincipal.GetUserId()),
            cancellationToken);

        var response = addAnimalResult.Value.ToResponse();
        return TypedResults.Created(linkGenerator.GetPathByName(GetAnimalEndpoint.EndpointName, new
        {
            animalId = response.Id
        }), response);
    }
}
