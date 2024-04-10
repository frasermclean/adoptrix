using System.Security.Claims;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Animals;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class AddAnimalEndpoint
{
    public static async Task<Results<Created<AnimalResponse>, ValidationProblem>> ExecuteAsync(
        SetAnimalRequest request,
        ClaimsPrincipal claimsPrincipal,
        IValidator<SetAnimalRequest> validator,
        ILogger<AddAnimalEndpoint> logger,
        ISender sender,
        LinkGenerator linkGenerator,
        CancellationToken cancellationToken)
    {
        // validate request
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        // add animal to database
        var addAnimalResult = await sender.Send(new AddAnimalRequest(
                request.Name,
                request.Description,
                request.BreedId,
                request.Sex,
                request.DateOfBirth,
                claimsPrincipal.GetUserId()),
            cancellationToken);

        var response = addAnimalResult.Value.ToResponse();
        return TypedResults.Created(linkGenerator.GetPathByName(GetAnimalEndpoint.EndpointName, new
        {
            animalId = response.Id
        }), response);
    }
}
