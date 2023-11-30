using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Animals;

public class SearchAnimalsCommandHandler(IAnimalsRepository repository)
    : ICommandHandler<SearchAnimalsCommand, IEnumerable<SearchAnimalsResult>>
{
    public async Task<IEnumerable<SearchAnimalsResult>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        return await repository.SearchAnimalsAsync(command.Name, command.Species, cancellationToken);
    }
}