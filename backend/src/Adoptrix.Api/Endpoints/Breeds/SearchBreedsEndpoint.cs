using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Breeds;
using MediatR;

namespace Adoptrix.Api.Endpoints.Breeds;

public class SearchBreedsEndpoint
{
    public static async Task<IEnumerable<BreedResponse>> ExecuteAsync(
        [AsParameters] SearchBreedsRequest request,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        var results = await sender.Send(request, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}
