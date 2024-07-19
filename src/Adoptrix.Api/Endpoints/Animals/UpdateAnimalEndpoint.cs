using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using AnimalMapper = Adoptrix.Api.Mapping.AnimalMapper;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpPut("animals/{animalId:guid}")]
public class UpdateAnimalEndpoint(IAnimalsRepository animalsRepository, IBreedsRepository breedsRepository)
    : Endpoint<UpdateAnimalRequest, Results<Ok<AnimalResponse>, NotFound, ErrorResponse>>
{
    public override async Task<Results<Ok<AnimalResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        UpdateAnimalRequest request, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            Logger.LogError("Animal with ID {AnimalId} was not found", request.AnimalId);
            return TypedResults.NotFound();
        }

        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            Logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            AddError(r => r.BreedId, "Breed not found");
            return new ErrorResponse(ValidationFailures);
        }

        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;

        await animalsRepository.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Updated animal with ID: {AnimalId}", animal.Id);

        return TypedResults.Ok(AnimalMapper.ToResponse(animal));
    }
}
