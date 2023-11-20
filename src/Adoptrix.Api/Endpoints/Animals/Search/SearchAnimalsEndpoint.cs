using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.Search;

public class SearchAnimalsEndpoint(IAnimalsRepository animalsRepository)
    : Endpoint<SearchAnimalsRequest, SearchAnimalsResponse>
{
    public override void Configure()
    {
        Get("/animals");
        AllowAnonymous();
    }

    public override async Task<SearchAnimalsResponse> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var animals = await animalsRepository.SearchAsync(request.Name, request.Species, cancellationToken);
        return new SearchAnimalsResponse { Animals = animals };
    }
}