using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands.Breeds;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Breeds.SearchBreeds;

public class SearchBreedsEndpoint(IResponseMappingService mappingService)
    : Endpoint<SearchBreedsCommand, IEnumerable<BreedResponse>>
{
    public override void Configure()
    {
        Get("breeds");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<BreedResponse>> ExecuteAsync(SearchBreedsCommand command,
        CancellationToken cancellationToken)
    {
        var results = await command.ExecuteAsync(cancellationToken);
        return results.Select(mappingService.Map);
    }
}