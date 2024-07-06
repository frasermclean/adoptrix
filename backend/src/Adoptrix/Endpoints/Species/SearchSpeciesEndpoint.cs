using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Services;
using FastEndpoints;

namespace Adoptrix.Endpoints.Species;

public class SearchSpeciesEndpoint(ISpeciesService speciesService) : Endpoint<SearchSpeciesRequest, IEnumerable<SpeciesMatch>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        return await speciesService.SearchAsync(request, cancellationToken);
    }
}
