using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Commands;

public class UpdateAnimalCommandHandler(IAnimalsRepository animalsRepository, IBreedsRepository breedsRepository)
    : IRequestHandler<UpdateAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(UpdateAnimalCommand command, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(command.AnimalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(command.AnimalId);
        }

        var breed = await breedsRepository.GetByIdAsync(command.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(command.BreedId);
        }

        animal.Name = command.Name;
        animal.Description = command.Description;
        animal.Breed = breed;
        animal.Sex = command.Sex;
        animal.DateOfBirth = command.DateOfBirth;

        await animalsRepository.UpdateAsync(animal, cancellationToken);
        return animal;
    }
}
