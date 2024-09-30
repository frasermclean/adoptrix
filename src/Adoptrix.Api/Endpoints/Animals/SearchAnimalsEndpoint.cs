using Adoptrix.Contracts.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals"), AllowAnonymous]
public class SearchAnimalsEndpoint(IAnimalsService animalsService)
    : Endpoint<SearchAnimalsRequest, IEnumerable<AnimalMatch>>
{
    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await animalsService.SearchAsync(request, cancellationToken);
        return matches;
    }
}
