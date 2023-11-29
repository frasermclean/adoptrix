using Adoptrix.Application.Commands.Species;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Species.GetAllSpecies;

[HttpGet("species")]
public class GetAllSpeciesEndpoint : Endpoint<GetAllSpeciesCommand, IEnumerable<Domain.Species>>
{
    public override async Task<IEnumerable<Domain.Species>> ExecuteAsync(GetAllSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        return await command.ExecuteAsync(cancellationToken);
    }
}