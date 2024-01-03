using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Commands.Species;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Species.SearchSpecies;

public class SearchSpeciesEndpoint : Endpoint<SearchSpeciesCommand, IEnumerable<SpeciesResponse>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<SpeciesResponse>> ExecuteAsync(SearchSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var results = await command.ExecuteAsync(cancellationToken);
        return results.Select(species => species.ToResponse());
    }
}