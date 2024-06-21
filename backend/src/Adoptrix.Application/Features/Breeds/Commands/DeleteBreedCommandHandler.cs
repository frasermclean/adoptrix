using Adoptrix.Application.Services;
using Adoptrix.Domain.Commands.Breeds;
using Adoptrix.Domain.Errors;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public class DeleteBreedCommandHandler(IBreedsRepository breedsRepository) : IRequestHandler<DeleteBreedCommand, Result>
{
    public async Task<Result> Handle(DeleteBreedCommand command, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(command.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(command.BreedId);
        }

        await breedsRepository.DeleteAsync(breed, cancellationToken);
        return Result.Ok();
    }
}
