using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;

namespace Adoptrix.Api.Endpoints.Animals;

public static class SearchAnimalsEndpoint
{
    public static async Task<IEnumerable<SearchAnimalsResult>> ExecuteAsync(
        [AsParameters] SearchAnimalsRequest request,
        IAnimalsService animalsService, CancellationToken cancellationToken)
    {
        return await animalsService.SearchAsync(request, cancellationToken);
    }
}
