using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using FastEndpoints;
using MediatR;

namespace Adoptrix.Endpoints.Animals;

public class SearchAnimalsEndpoint(ISender sender) : Endpoint<SearchAnimalsQuery, IEnumerable<AnimalMatch>>
{
    public override void Configure()
    {
        Get("animals");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
