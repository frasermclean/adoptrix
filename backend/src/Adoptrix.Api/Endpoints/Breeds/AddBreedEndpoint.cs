using System.Security.Claims;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Breeds.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedEndpoint
{
    public static async Task<Results<Created<BreedResponse>, ValidationProblem>> ExecuteAsync(
        SetBreedRequest request,
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        ILogger<AddBreedEndpoint> logger,
        IValidator<SetBreedRequest> validator,
        LinkGenerator linkGenerator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(
            new AddBreedCommand(request.Name, request.SpeciesId, claimsPrincipal.GetUserId()),
            cancellationToken);

        var response = result.Value.ToResponse();
        return TypedResults.Created(linkGenerator.GetPathByName(GetBreedEndpoint.EndpointName, new
        {
            breedIdOrName = response.Id
        }), response);
    }
}
