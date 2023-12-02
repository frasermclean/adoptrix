using Adoptrix.Application.Commands.Species;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Species.GetAllSpecies;

public class GetAllSpeciesEndpoint : Endpoint<GetAllSpeciesCommand, IEnumerable<Domain.Species>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<Domain.Species>> ExecuteAsync(GetAllSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        return await command.ExecuteAsync(cancellationToken);
    }
}