using System.Security.Claims;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class AddAnimalEndpoint
{
    public static async Task<Results<Created<AnimalResponse>, ValidationProblem>> ExecuteAsync(
        SetAnimalRequest request,
        ClaimsPrincipal claimsPrincipal,
        IValidator<SetAnimalRequest> validator,
        ILogger<AddAnimalEndpoint> logger,
        IAnimalsRepository animalsRepository,
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

        // get species and breed (should be validated by validator)
        var breed = (await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken)).Value;

        // add animal to database
        var addAnimalResult = await animalsRepository.AddAsync(new Animal
        {
            Name = request.Name,
            Description = request.Description,
            Breed = breed,
            Sex = request.Sex,
            DateOfBirth = request.DateOfBirth,
            CreatedBy = claimsPrincipal.GetUserId()
        }, cancellationToken);

        logger.LogInformation("Added animal with id {Id}", addAnimalResult.Value.Id);

        var response = addAnimalResult.Value.ToResponse();
        return TypedResults.Created(linkGenerator.GetPathByName(GetAnimalEndpoint.EndpointName, new
        {
            animalId = response.Id
        }), response);
    }
}
