using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Application.Commands.Animals;

public class SearchAnimalsCommandHandler(IAnimalsRepository repository)
    : ICommandHandler<SearchAnimalsCommand, IEnumerable<Animal>>
{
    public async Task<IEnumerable<Animal>> ExecuteAsync(SearchAnimalsCommand command,
        CancellationToken cancellationToken)
    {
        return await repository.SearchAsync(command.Name, command.Species, cancellationToken);
    }
}