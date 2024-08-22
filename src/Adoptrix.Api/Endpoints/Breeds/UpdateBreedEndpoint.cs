using Adoptrix.Api.Mapping;
using Adoptrix.Api.Security;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
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
        var species = await dbContext.Species.FirstOrDefaultAsync(species => species.Name == request.SpeciesName,
            cancellationToken);

        if (species is null)
        {
            AddError(r => r.SpeciesName, "Invalid species name");
            return new ErrorResponse(ValidationFailures);
        }

        var breed = await dbContext.Breeds.FirstOrDefaultAsync(breed => breed.Id == request.BreedId, cancellationToken);
        if (breed is null)
        {
            return TypedResults.NotFound();
        }

        breed.Name = request.Name;
        breed.Species = species;
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(breed.ToResponse());
    }
}
