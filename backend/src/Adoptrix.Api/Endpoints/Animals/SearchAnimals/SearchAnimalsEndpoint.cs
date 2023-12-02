using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands.Animals;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.SearchAnimals;

public class SearchAnimalsEndpoint(IResponseMappingService mappingService)
    : Endpoint<SearchAnimalsCommand, IEnumerable<AnimalResponse>>
{
    public override void Configure()
    {
        Get("animals");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<AnimalResponse>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        var results = await command.ExecuteAsync(cancellationToken);
        return results.Select(mappingService.Map);
    }
}