using Adoptrix.Api.Mapping;
using Adoptrix.Api.Security;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<AddBreedRequest, Results<Created<BreedResponse>, ErrorResponse>>
{
    public override void Configure()
    {
        Post("breeds");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<Created<BreedResponse>, ErrorResponse>> ExecuteAsync(AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var species = await dbContext.Species.Where(species => species.Name == request.SpeciesName)
            .Include(species => species.Breeds)
            .FirstOrDefaultAsync(cancellationToken);

        // ensure species exists
        if (species is null)
        {
            AddError(r => r.SpeciesName, "Invalid species name");
            return new ErrorResponse(ValidationFailures);
        }

        // ensure breed does not already exist
        if (species.Breeds.Any(b => b.Name == request.Name))
        {
            AddError(r => r.Name, "Breed already exists");
            return new ErrorResponse(ValidationFailures, StatusCodes.Status409Conflict);
        }

        // save new breed
        var breed = MapToBreed(request, species);
        species.Breeds.Add(breed);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/breeds/{breed.Id}", breed.ToResponse());
    }

    private static Breed MapToBreed(AddBreedRequest request, Core.Species species) => new()
    {
        Name = request.Name,
        Species = species,
        CreatedBy = request.UserId
    };
}
