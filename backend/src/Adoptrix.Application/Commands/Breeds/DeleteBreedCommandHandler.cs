using Adoptrix.Application.Services.Repositories;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Breeds;

public class DeleteBreedCommandHandler(IBreedsRepository breedsRepository)
    : ICommandHandler<DeleteBreedCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteBreedCommand command, CancellationToken ct)
    {
        return await breedsRepository.DeleteAsync(command.Id, ct);
    }
}