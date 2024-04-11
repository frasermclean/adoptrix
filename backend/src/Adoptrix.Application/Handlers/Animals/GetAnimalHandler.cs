using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Handlers.Animals;

public class GetAnimalHandler(IAnimalsRepository animalsRepository) : IRequestHandler<GetAnimalRequest, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(GetAnimalRequest request, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);

        return animal is not null
            ? animal
            : new AnimalNotFoundError(request.AnimalId);
    }
}
