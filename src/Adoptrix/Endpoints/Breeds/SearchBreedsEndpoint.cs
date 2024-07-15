using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Endpoints.Breeds;

[HttpGet("breeds"), AllowAnonymous]
public class SearchBreedsEndpoint(IBreedsRepository breedsRepository)
    : Endpoint<SearchBreedsRequest, IEnumerable<BreedMatch>>
{
    public override async Task<IEnumerable<BreedMatch>> ExecuteAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        return await breedsRepository.SearchAsync(request, cancellationToken);
    }
}
