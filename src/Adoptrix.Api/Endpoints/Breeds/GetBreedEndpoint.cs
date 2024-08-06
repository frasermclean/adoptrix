using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Breeds;

public class GetBreedEndpoint(AdoptrixDbContext dbContext)
    : EndpointWithoutRequest<Results<Ok<BreedResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("breeds/{breedId:int}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound>> ExecuteAsync(
        CancellationToken cancellationToken)
    {
        var breedId = Route<int>("breedId");

        var response = await dbContext.Breeds
            .AsNoTracking()
            .Where(breed => breed.Id == breedId)
            .Select(breed => new BreedResponse
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesName = breed.Species.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        return response is not null
            ? TypedResults.Ok(response)
            : TypedResults.NotFound();
    }
}
