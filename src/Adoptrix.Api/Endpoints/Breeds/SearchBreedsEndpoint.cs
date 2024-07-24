using Adoptrix.Contracts.Requests;
using Adoptrix.Persistence.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Api.Endpoints.Breeds;

[HttpGet("breeds"), AllowAnonymous]
public class SearchBreedsEndpoint(IBreedsRepository breedsRepository)
    : Endpoint<SearchBreedsRequest, IEnumerable<SearchBreedsItem>>
{
    public override async Task<IEnumerable<SearchBreedsItem>> ExecuteAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        return await breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
    }
}
