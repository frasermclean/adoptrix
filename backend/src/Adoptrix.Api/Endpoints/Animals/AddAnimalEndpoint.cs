using System.Security.Claims;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class AddAnimalEndpoint
{
    public static async Task<Results<Created<AnimalResponse>, BadRequest<ValidationFailedResponse>>> ExecuteAsync(
        AddAnimalRequest request,
        ClaimsPrincipal claimsPrincipal,
        IValidator<AddAnimalRequest> validator,
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
            return TypedResults.BadRequest(new ValidationFailedResponse());
        }

        // get species and breed (should be validated by validator)
        var species = (await speciesRepository.GetByNameAsync(request.SpeciesName, cancellationToken)).Value;
        var breed = request.BreedName is not null
            ? (await breedsRepository.GetByNameAsync(request.BreedName, cancellationToken)).Value
            : null;

        // add animal to database
        var addAnimalResult = await animalsRepository.AddAsync(new Animal
        {
            Name = request.Name,
            Description = request.Description,
            Species = species,
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
