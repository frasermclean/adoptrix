using Adoptrix.Api.Security;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<AddBreedRequest, Results<Created<BreedResponse>, ErrorResponse>, BreedResponseMapper>
{
    public override void Configure()
    {
        Post("breeds");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<Created<BreedResponse>, ErrorResponse>> ExecuteAsync(AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var species = await dbContext.Species.FirstOrDefaultAsync(s => s.Name == request.SpeciesName,
            cancellationToken);
        if (species is null)
        {
            Logger.LogError("Species with name {SpeciesName} not found", request.SpeciesName);
            AddError(r => r.SpeciesName, "Invalid species name");
            return new ErrorResponse(ValidationFailures);
        }

        var breed = Breed.Create(request.Name, species);
        species.Breeds.Add(breed);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Added breed with name {BreedName} and species {SpeciesName}",
                request.Name, request.SpeciesName);

            return TypedResults.Created($"/api/breeds/{breed.Id}", Map.FromEntity(breed));
        }
        catch (UniqueConstraintException exception) when (exception.ConstraintProperties.Contains(nameof(Breed.Name)))
        {
            Logger.LogError(exception, "Breed with name {BreedName} already exists", request.Name);
            AddError(r => r.Name, $"Breed with name '{request.Name}' already exists");
            return new ErrorResponse(ValidationFailures, StatusCodes.Status409Conflict);
        }
    }
}
