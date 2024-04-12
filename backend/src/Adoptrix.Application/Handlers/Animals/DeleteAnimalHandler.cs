using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Notifications.Animals;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Handlers.Animals;

public class DeleteAnimalHandler(
    IAnimalsRepository animalsRepository,
    ILogger<DeleteAnimalHandler> logger,
    IPublisher publisher)
    : IRequestHandler<DeleteAnimalRequest, Result>
{
    public async Task<Result> Handle(DeleteAnimalRequest request, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Could not find animal with ID: {AnimalId} to delete", request.AnimalId);
            return new AnimalNotFoundError(request.AnimalId);
        }

        await animalsRepository.DeleteAsync(animal, cancellationToken);
        logger.LogInformation("Deleted animal with ID: {AnimalId}", request.AnimalId);

        // publish notification
        await publisher.Publish(new AnimalDeletedNotification(animal.Id), cancellationToken);

        return Result.Ok();
    }
}
