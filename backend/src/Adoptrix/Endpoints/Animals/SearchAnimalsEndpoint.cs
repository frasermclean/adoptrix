using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
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
