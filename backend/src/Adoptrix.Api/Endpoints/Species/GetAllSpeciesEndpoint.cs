using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;

namespace Adoptrix.Api.Endpoints.Species;

public static class GetAllSpeciesEndpoint
{
    public static async Task<IEnumerable<SpeciesResponse>> ExecuteAsync(
        ISpeciesRepository repository, CancellationToken cancellationToken)
    {
        var species = await repository.GetAllAsync(cancellationToken);
        return species.Select(s => s.ToResponse());
    }
}
