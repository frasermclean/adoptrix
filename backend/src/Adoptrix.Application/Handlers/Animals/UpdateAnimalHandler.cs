using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Handlers.Animals;

public class UpdateAnimalHandler(IAnimalsRepository animalsRepository, IBreedsRepository breedsRepository)
    : IRequestHandler<UpdateAnimalRequest, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(UpdateAnimalRequest request, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(request.AnimalId);
        }

        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;

        await animalsRepository.UpdateAsync(animal, cancellationToken);
        return animal;
    }
}
