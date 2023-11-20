using Adoptrix.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals.Search;

public class SearchAnimalsEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<SearchAnimalsRequest, SearchAnimalsResponse>
{
    private readonly AdoptrixDbContext dbContext = dbContext;
    public override void Configure()
    {
        Get("/animals");
        AllowAnonymous();
    }

    public override async Task<SearchAnimalsResponse> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var animals = await dbContext.Animals.ToListAsync(cancellationToken);
        return new SearchAnimalsResponse
        {
            Animals = animals
        };
    }
}