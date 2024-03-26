using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;

namespace Adoptrix.Api.Endpoints.Breeds;

public class SearchBreedsEndpoint
{
    public static async Task<IEnumerable<BreedResponse>> ExecuteAsync(
        [AsParameters] SearchBreedsRequest request,
        IBreedsRepository breedsRepository,
        ISpeciesRepository speciesRepository,
        CancellationToken cancellationToken = default)
    {
        var results = await breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}
