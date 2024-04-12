using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using MediatR;

namespace Adoptrix.Api.Endpoints.Animals;

public static class SearchAnimalsEndpoint
{
    public static async Task<IEnumerable<SearchAnimalsResult>> ExecuteAsync(
        [AsParameters] SearchAnimalsQuery query,
        ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
