using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Species;

public class GetAllSpeciesCommandHandler (ISpeciesRepository speciesRepository)
    : ICommandHandler<GetAllSpeciesCommand, IEnumerable<Domain.Species>>
{
    public async Task<IEnumerable<Domain.Species>> ExecuteAsync(GetAllSpeciesCommand command, CancellationToken cancellationToken)
    {
        return await speciesRepository.GetAllSpeciesAsync(cancellationToken);
    }
}