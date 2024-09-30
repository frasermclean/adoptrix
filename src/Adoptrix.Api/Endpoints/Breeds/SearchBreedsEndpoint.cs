using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;

namespace Adoptrix.Api.Endpoints.Breeds;

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
        var matches = await breedsService.SearchAsync(request, cancellationToken);
        return matches;
    }
}
