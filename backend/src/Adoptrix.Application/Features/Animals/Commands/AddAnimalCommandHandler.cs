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
    ISlugGenerator slugGenerator)
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

        var slugResult = await slugGenerator.GenerateAsync(command.Name, breed, command.DateOfBirth, cancellationToken);
        if (slugResult.IsFailed)
        {
            logger.LogError("Failed to generate slug for animal with name {AnimalName}", command.Name);
            return slugResult.ToResult();
        }

        var animal = new Animal
        {
            Name = command.Name,
            Description = command.Description,
            Breed = breed,
            Sex = command.Sex,
            Slug = slugResult.Value,
            DateOfBirth = command.DateOfBirth,
            CreatedBy = command.UserId
        };

        await animalsRepository.AddAsync(animal, cancellationToken);

        logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return animal;
    }
}
