using Adoptrix.Api.Mapping;
using Adoptrix.Api.Security;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<AddAnimalRequest, Results<Created<AnimalResponse>, ErrorResponse>>
{
    public override void Configure()
    {
        Post("animals");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<Created<AnimalResponse>, ErrorResponse>> ExecuteAsync(AddAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var breed = await dbContext.Breeds.Where(breed => breed.Id == request.BreedId)
            .Include(breed => breed.Animals)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        if (breed is null)
        {
            Logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            AddError(r => r.BreedId, "Breed not found");
            return new ErrorResponse(ValidationFailures);
        }

        var animal = MapToAnimal(request, breed);
        breed.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return TypedResults.Created($"/api/animals/{animal.Id}", animal.ToResponse());
    }

    private static Animal MapToAnimal(AddAnimalRequest request, Breed breed) => new()
    {
        Name = request.Name,
        Description = request.Description,
        Breed = breed,
        Sex = request.Sex,
        DateOfBirth = request.DateOfBirth,
        Slug = $"{request.Name.ToLower()}-{request.DateOfBirth:O}",
        CreatedBy = request.UserId
    };
}
