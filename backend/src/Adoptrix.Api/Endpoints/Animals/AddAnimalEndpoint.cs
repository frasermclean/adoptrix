using System.Security.Claims;
using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Animals.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class AddAnimalEndpoint
{
    public static async Task<Results<Created<AnimalResponse>, ValidationProblem>> ExecuteAsync(
        SetAnimalData data,
        ClaimsPrincipal claimsPrincipal,
        ILogger<AddAnimalEndpoint> logger,
        ISender sender,
        LinkGenerator linkGenerator,
        CancellationToken cancellationToken)
    {
        // dispatch command
        var command = new AddAnimalCommand(data.Name, data.Description, data.BreedId, data.Sex, data.DateOfBirth,
            claimsPrincipal.GetUserId());
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>());
        }

        var response = result.Value.ToResponse();
        return TypedResults.Created(linkGenerator.GetPathByName(GetAnimalEndpoint.EndpointName, new
        {
            animalId = response.Id
        }), response);
    }
}
