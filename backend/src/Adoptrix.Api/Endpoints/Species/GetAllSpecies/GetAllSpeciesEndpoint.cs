using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Commands.Species;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Species.GetAllSpecies;

public class GetAllSpeciesEndpoint : Endpoint<GetAllSpeciesCommand, IEnumerable<SpeciesResponse>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<SpeciesResponse>> ExecuteAsync(GetAllSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var results = await command.ExecuteAsync(cancellationToken);
        return results.Select(species => species.ToResponse());
    }
}