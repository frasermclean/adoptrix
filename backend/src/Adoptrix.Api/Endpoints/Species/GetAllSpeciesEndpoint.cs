using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Species.Queries;
using MediatR;

namespace Adoptrix.Api.Endpoints.Species;

public static class GetAllSpeciesEndpoint
{
    public static async Task<IEnumerable<SpeciesResponse>> ExecuteAsync(
        ISender sender, CancellationToken cancellationToken)
    {
        var allSpecies = await sender.Send(new GetAllSpeciesQuery(), cancellationToken);
        return allSpecies.Select(species => species.ToResponse());
    }
}
