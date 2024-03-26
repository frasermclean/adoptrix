using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
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
            return TypedResults.NotFound();
        }

        // get species and breed (should be validated by validator)
        var breed = (await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken)).Value;

        var animal = getResult.Value;

        // update properties on the animal
        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;

        await animalsRepository.UpdateAsync(animal, cancellationToken);

        return TypedResults.Ok(animal.ToResponse());
    }
}
