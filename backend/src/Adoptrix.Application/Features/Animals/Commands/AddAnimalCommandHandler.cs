using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Features.Animals.Commands;

public class AddAnimalCommandHandler(
    ILogger<AddAnimalCommandHandler> logger,
    IAnimalsRepository animalsRepository,
    IBreedsRepository breedsRepository,
    IAnimalAssistant animalAssistant)
    : IRequestHandler<AddAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(AddAnimalCommand command, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(command.BreedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", command.BreedId);
            return new BreedNotFoundError(command.BreedId);
        }

        var description = command.ShouldGenerateDescription
            ? await animalAssistant.GenerateDescriptionAsync(command.Name, breed, command.Sex, command.DateOfBirth)
            : command.Description;

        var animal = new Animal
        {
            Name = command.Name,
            Description = description,
            Breed = breed,
            Sex = command.Sex,
            DateOfBirth = command.DateOfBirth,
            CreatedBy = command.UserId
        };

        await animalsRepository.AddAsync(animal, cancellationToken);

        logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return animal;
    }
}
