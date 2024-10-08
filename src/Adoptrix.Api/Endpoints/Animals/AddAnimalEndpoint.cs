using Adoptrix.Api.Security;
using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<AddAnimalRequest, Results<Created<AnimalResponse>, ErrorResponse>, AnimalResponseMapper>
{
    public override void Configure()
    {
        Post("animals");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<Created<AnimalResponse>, ErrorResponse>> ExecuteAsync(AddAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var breed = await dbContext.Breeds.Where(b => b.Id == request.BreedId)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(b => b.Id == request.BreedId, cancellationToken);

        if (breed is null)
        {
            Logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            AddError(r => r.BreedId, "Breed not found");
            return new ErrorResponse(ValidationFailures);
        }

        var animal = MapToAnimal(request, breed);

        dbContext.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return TypedResults.Created($"/api/animals/{animal.Id}", Map.FromEntity(animal));
    }

    private static Animal MapToAnimal(AddAnimalRequest request, Breed breed) => new()
    {
        Name = request.Name,
        Description = request.Description,
        Breed = breed,
        Sex = request.Sex,
        DateOfBirth = request.DateOfBirth,
        Slug = Animal.CreateSlug(request.Name, request.DateOfBirth),
        LastModifiedBy = request.UserId
    };
}
