using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class GetAnimalQueryHandler(IAnimalsRepository animalsRepository)
    : IRequestHandler<GetAnimalQuery, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(GetAnimalQuery query, CancellationToken cancellationToken)
    {
        var result = Guid.TryParse(query.AnimalIdOrSlug, out var animalId)
            ? await animalsRepository.GetAsync(animalId, cancellationToken)
            : await animalsRepository.GetAsync(query.AnimalIdOrSlug, cancellationToken);

        return result;
    }
}
