using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Handlers.Breeds;

public class DeleteBreedHandler(IBreedsRepository breedsRepository) : IRequestHandler<DeleteBreedRequest, Result>
{
    public async Task<Result> Handle(DeleteBreedRequest request, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        await breedsRepository.DeleteAsync(breed, cancellationToken);
        return Result.Ok();
    }
}
