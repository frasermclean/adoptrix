using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Models;
using MediatR;

namespace Adoptrix.Api.Endpoints.Animals;

public static class SearchAnimalsEndpoint
{
    public static async Task<IEnumerable<SearchAnimalsResult>> ExecuteAsync(
        [AsParameters] SearchAnimalsRequest request,
        ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }
}
