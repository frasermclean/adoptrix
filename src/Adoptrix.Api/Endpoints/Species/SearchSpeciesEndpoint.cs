using Adoptrix.Contracts.Requests;
using Adoptrix.Persistence.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Api.Endpoints.Species;

[HttpGet("species"), AllowAnonymous]
public class SearchSpeciesEndpoint(ISpeciesRepository speciesRepository) : Endpoint<SearchSpeciesRequest, IEnumerable<SearchSpeciesItem>>
{
    public override async Task<IEnumerable<SearchSpeciesItem>> ExecuteAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        return await speciesRepository.SearchAsync(request.WithAnimals, cancellationToken);
    }
}
