using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpPost("animals")]
public class AddAnimalEndpoint(IAnimalsRepository animalsRepository, IBreedsRepository breedsRepository)
    : Endpoint<AddAnimalRequest, Results<Created<AnimalResponse>, ErrorResponse>>
{
    public override async Task<Results<Created<AnimalResponse>, ErrorResponse>> ExecuteAsync(AddAnimalRequest request, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            Logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            AddError(r => r.BreedId, "Breed not found");
            return new ErrorResponse(ValidationFailures);
        }

        var animal = MapToAnimal(request, breed);
        await animalsRepository.AddAsync(animal, cancellationToken);

        Logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return TypedResults.Created($"/api/animals/{animal.Id}", AnimalResponseMapper.ToResponse(animal));
    }

    private static Animal MapToAnimal(AddAnimalRequest request, Breed breed) => new()
    {
        Name = request.Name,
        Description = request.Description,
        Breed = breed,
        Sex = request.Sex,
        DateOfBirth = request.DateOfBirth,
        CreatedBy = request.UserId
    };
}
