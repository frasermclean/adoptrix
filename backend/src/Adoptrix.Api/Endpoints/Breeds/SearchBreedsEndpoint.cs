using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Services;

namespace Adoptrix.Api.Endpoints.Breeds;

public class SearchBreedsEndpoint
{
    public static async Task<IEnumerable<BreedResponse>> ExecuteAsync(
        [AsParameters] SearchBreedsRequest request,
        IBreedsService breedsService,
        CancellationToken cancellationToken = default)
    {
        var results = await breedsService.SearchAsync(request, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}
