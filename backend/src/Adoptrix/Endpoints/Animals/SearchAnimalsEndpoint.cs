using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Extensions;
using Adoptrix.Storage;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Endpoints.Animals;

[HttpGet("animals"), AllowAnonymous]
public class SearchAnimalsEndpoint(
    IAnimalsRepository animalsRepository,
    [FromKeyedServices(BlobContainerNames.AnimalImages)] IBlobContainerManager blobContainerManager)
    : Endpoint<SearchAnimalsRequest, IEnumerable<AnimalMatch>>
{
    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await animalsRepository.SearchAsync(request, cancellationToken);

        foreach (var match in matches)
        {
            match.Image?.SetImageUrls(match.Id, blobContainerManager.ContainerUri);
        }

        return matches;
    }
}
