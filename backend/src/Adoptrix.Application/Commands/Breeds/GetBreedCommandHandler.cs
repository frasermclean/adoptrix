using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Breeds;

public class GetBreedCommandHandler(IBreedsRepository breedsRepository)
    : ICommandHandler<GetBreedCommand, Result<Breed>>
{
    public async Task<Result<Breed>> ExecuteAsync(GetBreedCommand command, CancellationToken cancellationToken)
    {
        return await breedsRepository.GetByIdAsync(command.Id, cancellationToken);
    }
}