using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Queries;

public class GetBreedQueryHandler(IBreedsRepository breedsRepository) : IRequestHandler<GetBreedQuery, Result<Breed>>
{
    public async Task<Result<Breed>> Handle(GetBreedQuery query, CancellationToken cancellationToken)
    {
        var breed = Guid.TryParse(query.BreedIdOrName, out var breedId)
            ? await breedsRepository.GetByIdAsync(breedId, cancellationToken)
            : await breedsRepository.GetByNameAsync(query.BreedIdOrName, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(breedId);
    }
}
