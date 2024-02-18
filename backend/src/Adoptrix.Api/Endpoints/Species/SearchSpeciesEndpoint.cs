using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;

namespace Adoptrix.Api.Endpoints.Species;

public static class SearchSpeciesEndpoint
{
    public static async Task<IEnumerable<SpeciesResponse>> ExecuteAsync(
        ISpeciesRepository repository, CancellationToken cancellationToken)
    {
        var species = await repository.SearchSpeciesAsync(cancellationToken);
        return species.Select(s => s.ToResponse());
    }
}
