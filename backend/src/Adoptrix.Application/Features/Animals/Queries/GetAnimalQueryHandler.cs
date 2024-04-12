using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class GetAnimalQueryHandler(IAnimalsRepository animalsRepository) : IRequestHandler<GetAnimalQuery, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(GetAnimalQuery query, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(query.AnimalId, cancellationToken);

        return animal is not null
            ? animal
            : new AnimalNotFoundError(query.AnimalId);
    }
}
