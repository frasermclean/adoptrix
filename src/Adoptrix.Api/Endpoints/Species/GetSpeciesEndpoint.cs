using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Species;

[HttpGet("species/{speciesId:guid}"), AllowAnonymous]
public class GetSpeciesEndpoint(ISpeciesRepository speciesRepository)
    : Endpoint<GetSpeciesRequest, Results<Ok<SpeciesResponse>, NotFound>>
{
    public override async Task<Results<Ok<SpeciesResponse>, NotFound>> ExecuteAsync(GetSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);

        return species is not null
            ? TypedResults.Ok(SpeciesResponseMapper.ToResponse(species))
            : TypedResults.NotFound();
    }
}
