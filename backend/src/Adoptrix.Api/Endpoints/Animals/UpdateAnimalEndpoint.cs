using Adoptrix.Api.Security;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<UpdateAnimalRequest, Results<Ok<AnimalResponse>, NotFound, ErrorResponse>, AnimalResponseMapper>
{
    public override void Configure()
    {
        Put("animals/{animalId:guid}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        UpdateAnimalRequest request, CancellationToken cancellationToken)
    {
        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == request.AnimalId, cancellationToken);
        if (animal is null)
        {
            Logger.LogError("Could not update animal with ID {AnimalId} as it was not found", request.AnimalId);
            return TypedResults.NotFound();
        }

        var breed = await dbContext.Breeds.Where(breed => breed.Name == request.BreedName)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        if (breed is null)
        {
            Logger.LogError("Breed with name {BreedId} was not found", request.BreedName);
            AddError(r => r.BreedName, "Breed not found");
            return new ErrorResponse(ValidationFailures);
        }

        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;
        animal.LastModifiedBy = request.UserId;
        animal.LastModifiedUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        Logger.LogInformation("Updated animal with ID: {AnimalId}", animal.Id);

        return TypedResults.Ok(Map.FromEntity(animal));
    }
}
