using Adoptrix.Api.Mapping;
using Adoptrix.Api.Security;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<UpdateAnimalRequest, Results<Ok<AnimalResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Put("animals/{animalId:int}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        UpdateAnimalRequest request, CancellationToken cancellationToken)
    {
        var breed = await dbContext.Breeds.Where(breed => breed.Id == request.BreedId)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        if (breed is null)
        {
            Logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            AddError(r => r.BreedId, "Breed not found");
            return new ErrorResponse(ValidationFailures);
        }

        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == request.AnimalId, cancellationToken);
        if (animal is null)
        {
            Logger.LogError("Animal with ID {AnimalId} was not found", request.AnimalId);
            return TypedResults.NotFound();
        }

        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;

        await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Updated animal with ID: {AnimalId}", animal.Id);

        return TypedResults.Ok(animal.ToResponse());
    }
}
