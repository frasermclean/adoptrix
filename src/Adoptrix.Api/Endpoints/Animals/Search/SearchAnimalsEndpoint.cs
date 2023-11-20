using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.Search;

public class SearchAnimalsEndpoint : Endpoint<SearchAnimalsRequest, SearchAnimalsResponse>
{
    public override void Configure()
    {
        Get("/animals");
        AllowAnonymous();
    }

    public override Task<SearchAnimalsResponse> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var response = new SearchAnimalsResponse();
        return Task.FromResult(response);
    }
}