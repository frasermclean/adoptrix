using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Services;
using FastEndpoints;

namespace Adoptrix.Endpoints.Animals;

public class SearchAnimalsEndpoint(IAnimalsService animalsService) : Endpoint<SearchAnimalsRequest, IEnumerable<AnimalMatch>>
{
    public override void Configure()
    {
        Get("animals");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        return await animalsService.SearchAsync(request, cancellationToken);
    }
}
