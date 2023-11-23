using Adoptrix.Application.Commands;
using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals")]
public class SearchAnimalsEndpoint : Endpoint<SearchAnimalsCommand, IEnumerable<AnimalSearchResult>>
{
    public override async Task<IEnumerable<AnimalSearchResult>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        return await command.ExecuteAsync(cancellationToken);
    }
}