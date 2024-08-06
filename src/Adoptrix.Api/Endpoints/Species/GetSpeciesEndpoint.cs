using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Species;

[HttpGet("species/{speciesName}"), AllowAnonymous]
public class GetSpeciesEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<GetSpeciesRequest, Results<Ok<SpeciesResponse>, NotFound>>
{
    public override async Task<Results<Ok<SpeciesResponse>, NotFound>> ExecuteAsync(GetSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var species = await dbContext.Species.Where(species => species.Name == request.SpeciesName)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return species is not null
            ? TypedResults.Ok(species.ToResponse())
            : TypedResults.NotFound();
    }
}
