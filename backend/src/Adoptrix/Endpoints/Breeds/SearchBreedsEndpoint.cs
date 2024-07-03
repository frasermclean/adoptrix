using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
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
