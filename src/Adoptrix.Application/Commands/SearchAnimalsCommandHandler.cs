using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Application.Commands;

public class SearchAnimalsCommandHandler(IAnimalsRepository repository)
    : ICommandHandler<SearchAnimalsCommand, IEnumerable<AnimalSearchResult>>
{
    public async Task<IEnumerable<AnimalSearchResult>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        return await repository.SearchAsync(command.Name, command.Species, cancellationToken);
    }
}