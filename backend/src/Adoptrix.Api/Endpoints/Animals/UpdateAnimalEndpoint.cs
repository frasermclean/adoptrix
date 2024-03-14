using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, ValidationProblem, NotFound>> ExecuteAsync(
        Guid animalId,
        SetAnimalRequest request,
        IValidator<SetAnimalRequest> validator,
        ILogger<UpdateAnimalEndpoint> logger,
        IAnimalsRepository animalsRepository,
        ISpeciesRepository speciesRepository,
        IBreedsRepository breedsRepository,
        CancellationToken cancellationToken)
    {
        // validate request
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for request: {Request}", request);
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        // get animal from database
        var getResult = await animalsRepository.GetAsync(animalId, cancellationToken);
        if (getResult.IsFailed)
        {
            logger.LogError("Could not find animal with Id {AnimalId} to update", animalId);
            TypedResults.NotFound();
        }

        // get species and breed (should be validated by validator)
        var species = (await speciesRepository.GetByNameAsync(request.SpeciesName, cancellationToken)).Value;
        var breed = request.BreedName is not null
            ? (await breedsRepository.GetByNameAsync(request.BreedName, cancellationToken)).Value
            : null;

        var animal = getResult.Value;

        // update properties on the animal
        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Species = species;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;

        var updateResult = await animalsRepository.UpdateAsync(animal, cancellationToken);
        if (updateResult.IsFailed)
        {
            logger.LogWarning("Failed to update animal with Id {AnimalId} - Error: {Error}", animalId,
                updateResult.GetFirstErrorMessage());
        }

        return TypedResults.Ok(animal.ToResponse());
    }
}
