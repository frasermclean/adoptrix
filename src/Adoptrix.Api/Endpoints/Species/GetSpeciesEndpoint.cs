using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Species;

public class GetSpeciesEndpoint(AdoptrixDbContext dbContext)
    : EndpointWithoutRequest<Results<Ok<SpeciesResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("species/{speciesName}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<SpeciesResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var speciesName = Route<string>("speciesName");

        var species = await dbContext.Species.Where(species => species.Name == speciesName)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return species is not null
            ? TypedResults.Ok(species.ToResponse())
            : TypedResults.NotFound();
    }
}
