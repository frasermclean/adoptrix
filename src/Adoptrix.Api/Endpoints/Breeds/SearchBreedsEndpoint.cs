using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Api.Endpoints.Breeds;

[HttpGet("breeds"), AllowAnonymous]
public class SearchBreedsEndpoint(IBreedsRepository breedsRepository)
    : Endpoint<SearchBreedsRequest, IEnumerable<BreedMatch>>
{
    public override async Task<IEnumerable<BreedMatch>> ExecuteAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        var items = await breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);

        return items.Select(item => item.ToMatch());
    }
}
