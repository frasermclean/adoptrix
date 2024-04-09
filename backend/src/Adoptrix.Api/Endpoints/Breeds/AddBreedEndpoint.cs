using System.Security.Claims;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedEndpoint
{
    public static async Task<Results<Created<BreedResponse>, ValidationProblem>> ExecuteAsync(
        SetBreedRequest request,
        ClaimsPrincipal claimsPrincipal,
        ILogger<AddBreedEndpoint> logger,
        IValidator<SetBreedRequest> validator,
        IBreedsService breedsService,
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

        // attach user id to request
        request.UserId = claimsPrincipal.GetUserId();

        var result = await breedsService.AddAsync(request, cancellationToken);

        var response = result.Value.ToResponse();
        return TypedResults.Created(linkGenerator.GetPathByName(GetBreedEndpoint.EndpointName, new
        {
            breedIdOrName = response.Id
        }), response);
    }
}
