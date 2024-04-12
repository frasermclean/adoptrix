using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Features.Breeds.Queries;
using MediatR;

namespace Adoptrix.Api.Endpoints.Breeds;

public class SearchBreedsEndpoint
{
    public static async Task<IEnumerable<BreedResponse>> ExecuteAsync(
        [AsParameters] SearchBreedsQuery query,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        var results = await sender.Send(query, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}
