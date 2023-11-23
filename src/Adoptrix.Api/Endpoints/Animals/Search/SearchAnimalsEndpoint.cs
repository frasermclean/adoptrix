using Adoptrix.Application.Commands;
using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.Search;

[HttpGet("animals")]
public class SearchAnimalsEndpoint : Endpoint<SearchAnimalsCommand, IEnumerable<Animal>>
{
    public override async Task<IEnumerable<Animal>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        return await command.ExecuteAsync(cancellationToken);
    }
}