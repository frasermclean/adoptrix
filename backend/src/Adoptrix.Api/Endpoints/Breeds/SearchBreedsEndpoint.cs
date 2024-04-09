using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;

namespace Adoptrix.Api.Endpoints.Breeds;

public class SearchBreedsEndpoint
{
    public static async Task<IEnumerable<BreedResponse>> ExecuteAsync(
        [AsParameters] SearchBreedsRequest request,
        IBreedsService breedsService,
        CancellationToken cancellationToken = default)
    {
        var results = await breedsService.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}
