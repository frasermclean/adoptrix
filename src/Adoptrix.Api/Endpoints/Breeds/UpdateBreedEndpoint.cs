using Adoptrix.Api.Mapping;
using Adoptrix.Api.Security;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<UpdateBreedRequest, Results<Ok<BreedResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Put("breeds/{breedId:int}");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        UpdateBreedRequest request, CancellationToken cancellationToken)
    {
        // ensure breed exists
        var breed = await dbContext.Breeds.FirstOrDefaultAsync(b => b.Id == request.BreedId, cancellationToken);
        if (breed is null)
        {
            return TypedResults.NotFound();
        }

        // ensure species exists
        var species =
            await dbContext.Species.FirstOrDefaultAsync(s => s.Name == request.SpeciesName, cancellationToken);
        if (species is null)
        {
            AddError(r => r.SpeciesName, "Invalid species name");
            return new ErrorResponse(ValidationFailures);
        }

        // update breed
        breed.Name = request.Name;
        breed.Species = species;
        breed.LastModifiedBy = request.UserId;
        breed.LastModifiedUtc = DateTime.UtcNow;

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            Logger.LogInformation("Updated breed with ID {BreedId}", request.BreedId);
            return TypedResults.Ok(breed.ToResponse());
        }
        catch (UniqueConstraintException exception) when (exception.ConstraintProperties.Contains(nameof(Breed.Name)))
        {
            Logger.LogError(exception, "Could not update as breed with name {BreedName} already exists", request.Name);
            AddError(r => r.Name, $"Breed with name '{request.Name}' already exists");
            return new ErrorResponse(ValidationFailures, StatusCodes.Status409Conflict);
        }
    }
}
