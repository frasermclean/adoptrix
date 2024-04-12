using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
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
