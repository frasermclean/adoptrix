using Adoptrix.Application.Commands.Breeds;
using Adoptrix.Application.Models;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Breeds.SearchBreeds;

[HttpGet("/breeds")]
public class SearchBreedsEndpoint : Endpoint<SearchBreedsCommand, IEnumerable<SearchBreedsResult>>
{
    public override async Task<IEnumerable<SearchBreedsResult>> ExecuteAsync(SearchBreedsCommand command,
        CancellationToken cancellationToken)
    {
        return await command.ExecuteAsync(cancellationToken);
    }
}