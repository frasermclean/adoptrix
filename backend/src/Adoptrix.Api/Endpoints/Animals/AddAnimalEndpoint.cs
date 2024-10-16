using Adoptrix.Api.Security;
using Adoptrix.Core;
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
        var breed = await dbContext.Breeds.Where(b => b.Name == request.BreedName)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        if (breed is null)
        {
            Logger.LogError("Breed with name {BreedName} was not found", request.BreedName);
            AddError(r => r.BreedName, "Breed not found");
            return new ErrorResponse(ValidationFailures);
        }

        var animal = Animal.Create(request.Name, request.Description, breed, request.Sex, request.DateOfBirth);

        breed.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return TypedResults.Created($"/api/animals/{animal.Id}", Map.FromEntity(animal));
    }
}
