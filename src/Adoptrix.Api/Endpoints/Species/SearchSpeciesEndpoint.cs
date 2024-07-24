using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Api.Endpoints.Species;

[HttpGet("species"), AllowAnonymous]
public class SearchSpeciesEndpoint(ISpeciesRepository speciesRepository) : Endpoint<SearchSpeciesRequest, IEnumerable<SpeciesMatch>>
{
    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var items = await speciesRepository.SearchAsync(request.WithAnimals, cancellationToken);

        return items.Select(item => item.ToMatch());
    }
}
