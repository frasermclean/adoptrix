using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using FastEndpoints;

namespace Adoptrix.Endpoints.Animals;

public class SearchAnimalsEndpoint(IAnimalsRepository animalsRepository) : Endpoint<SearchAnimalsRequest, IEnumerable<AnimalMatch>>
{
    public override void Configure()
    {
        Get("animals");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await animalsRepository.SearchAsync(request, cancellationToken);
        return matches;
    }
}
