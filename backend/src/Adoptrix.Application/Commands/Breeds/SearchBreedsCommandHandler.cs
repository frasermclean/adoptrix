using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Breeds;

public class SearchBreedsCommandHandler(ISpeciesRepository speciesRepository, IBreedsRepository breedsRepository)
    : ICommandHandler<SearchBreedsCommand, IEnumerable<SearchBreedsResult>>
{
    public async Task<IEnumerable<SearchBreedsResult>> ExecuteAsync(SearchBreedsCommand command,
        CancellationToken cancellationToken)
    {
        var speciesResult = command.Species is not null
            ? await speciesRepository.GetByNameAsync(command.Species, cancellationToken)
            : null;

        return await breedsRepository.SearchAsync(speciesResult?.ValueOrDefault, command.WithAnimals,
            cancellationToken);
    }
}