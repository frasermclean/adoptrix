using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Breeds;

public class SearchBreedsCommandHandler(IBreedsRepository breedsRepository)
    : ICommandHandler<SearchBreedsCommand, IEnumerable<SearchBreedsResult>>
{
    public async Task<IEnumerable<SearchBreedsResult>> ExecuteAsync(SearchBreedsCommand command,
        CancellationToken cancellationToken)
    {
        return await breedsRepository.SearchAsync(command.WithAnimals, cancellationToken);
    }
}