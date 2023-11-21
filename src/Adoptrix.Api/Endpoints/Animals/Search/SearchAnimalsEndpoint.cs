using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.Search;

[HttpGet("animals")]
public class SearchAnimalsEndpoint(IAnimalsRepository repository)
    : Endpoint<SearchAnimalsRequest, IEnumerable<Animal>>
{
    public override async Task<IEnumerable<Animal>> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        return await repository.SearchAsync(request.Name, request.Species, cancellationToken);
    }
}