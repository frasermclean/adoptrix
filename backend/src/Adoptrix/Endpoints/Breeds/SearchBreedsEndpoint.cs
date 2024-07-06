using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Services;
using FastEndpoints;

namespace Adoptrix.Endpoints.Breeds;

public class SearchBreedsEndpoint(IBreedsService breedsService) : Endpoint<SearchBreedsRequest, IEnumerable<BreedMatch>>
{
    public override void Configure()
    {
        Get("breeds");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<BreedMatch>> ExecuteAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        return await breedsService.SearchAsync(request, cancellationToken);
    }
}
