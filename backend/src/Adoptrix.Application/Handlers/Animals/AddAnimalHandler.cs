using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Notifications.Animals;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Handlers.Animals;

public class AddAnimalHandler(
    ILogger<AddAnimalHandler> logger,
    IAnimalsRepository animalsRepository,
    IBreedsRepository breedsRepository,
    IPublisher publisher)
    : IRequestHandler<AddAnimalRequest, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(AddAnimalRequest request, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            return new BreedNotFoundError(request.BreedId);
        }

        var animal = new Animal
        {
            Name = request.Name,
            Description = request.Description,
            Breed = breed,
            Sex = request.Sex,
            DateOfBirth = request.DateOfBirth,
            CreatedBy = request.UserId
        };

        await animalsRepository.AddAsync(animal, cancellationToken);

        // publish notification
        await publisher.Publish(new AnimalAddedNotification(animal.Id), cancellationToken);

        logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return animal;
    }
}
