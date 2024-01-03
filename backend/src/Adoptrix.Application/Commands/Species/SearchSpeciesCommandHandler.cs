using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Species;

public class SearchSpeciesCommandHandler (ISpeciesRepository speciesRepository)
    : ICommandHandler<SearchSpeciesCommand, IEnumerable<Domain.Species>>
{
    public async Task<IEnumerable<Domain.Species>> ExecuteAsync(SearchSpeciesCommand command, CancellationToken cancellationToken)
    {
        return await speciesRepository.SearchSpeciesAsync(cancellationToken);
    }
}