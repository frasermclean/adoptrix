using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;

namespace Adoptrix.Api.Endpoints.Species;

public class SearchSpeciesEndpoint(ISpeciesService speciesService)
    : Endpoint<SearchSpeciesRequest, IEnumerable<SpeciesMatch>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await speciesService.SearchAsync(request, cancellationToken);
        return matches;
    }
}
