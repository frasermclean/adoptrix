using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;

namespace Adoptrix.Api.Endpoints.Species;

public static class GetAllSpeciesEndpoint
{
    public static async Task<IEnumerable<SpeciesResponse>> ExecuteAsync(
        ISpeciesService speciesService, CancellationToken cancellationToken)
    {
        var allSpecies = await speciesService.GetAllAsync(cancellationToken);
        return allSpecies.Select(species => species.ToResponse());
    }
}
