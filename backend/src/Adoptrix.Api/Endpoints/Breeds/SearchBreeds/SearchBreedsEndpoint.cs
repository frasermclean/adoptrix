using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Commands.Breeds;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Breeds.SearchBreeds;

public class SearchBreedsEndpoint : Endpoint<SearchBreedsCommand, IEnumerable<BreedResponse>>
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
        return results.Select(result => result.ToResponse());
    }
}