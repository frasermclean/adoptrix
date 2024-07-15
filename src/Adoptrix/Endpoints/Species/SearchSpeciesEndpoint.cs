using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Endpoints.Species;

[HttpGet("species"), AllowAnonymous]
public class SearchSpeciesEndpoint(ISpeciesRepository speciesRepository) : Endpoint<SearchSpeciesRequest, IEnumerable<SpeciesMatch>>
{
    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        return await speciesRepository.SearchAsync(request, cancellationToken);
    }
}
