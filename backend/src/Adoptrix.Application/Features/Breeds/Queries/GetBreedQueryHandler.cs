using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Queries.Breeds;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Queries;

public class GetBreedQueryHandler(IBreedsRepository breedsRepository) : IRequestHandler<GetBreedQuery, Result<Breed>>
{
    public async Task<Result<Breed>> Handle(GetBreedQuery query, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(query.BreedId, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(query.BreedId);
    }
}
