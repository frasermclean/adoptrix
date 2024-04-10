using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Handlers.Breeds;

public class GetBreedHandler(IBreedsRepository breedsRepository) : IRequestHandler<GetBreedRequest, Result<Breed>>
{
    public async Task<Result<Breed>> Handle(GetBreedRequest request, CancellationToken cancellationToken)
    {
        var breed = Guid.TryParse(request.BreedIdOrName, out var breedId)
            ? await breedsRepository.GetByIdAsync(breedId, cancellationToken)
            : await breedsRepository.GetByNameAsync(request.BreedIdOrName, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(breedId);
    }
}
