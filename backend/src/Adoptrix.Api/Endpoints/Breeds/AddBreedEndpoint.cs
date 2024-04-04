using System.Security.Claims;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
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
        ISpeciesRepository speciesRepository,
        IBreedsRepository breedsRepository,
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

        // create new breed and add to database
        var breed = new Breed
        {
            Name = request.Name,
            Species = (await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken)).Value,
            CreatedBy = claimsPrincipal.GetUserId()
        };
        await breedsRepository.AddAsync(breed, cancellationToken);
        var response = breed.ToResponse();

        return TypedResults.Created(linkGenerator.GetPathByName(GetBreedEndpoint.EndpointName, new
        {
            breedIdOrName = response.Id
        }), response);
    }
}
