using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Application.Commands;
using Adoptrix.Application.Services;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.Search;

[HttpGet("animals")]
public class SearchAnimalsEndpoint(ISqidConverter sqidConverter)
    : Endpoint<SearchAnimalsCommand, IEnumerable<AnimalResponse>>
{
    public override async Task<IEnumerable<AnimalResponse>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        var animals = await command.ExecuteAsync(cancellationToken);
        return animals.Select(animal => animal.ToResponse(sqidConverter));
    }
}