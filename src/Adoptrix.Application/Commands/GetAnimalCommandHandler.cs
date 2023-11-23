using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands;

public class GetAnimalCommandHandler(IAnimalsRepository repository)
    : ICommandHandler<GetAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> ExecuteAsync(GetAnimalCommand command, CancellationToken cancellationToken)
    {
        return await repository.GetAsync(command.Id, cancellationToken);
    }
}