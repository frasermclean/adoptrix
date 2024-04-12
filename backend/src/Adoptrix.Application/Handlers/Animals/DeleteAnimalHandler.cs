using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Handlers.Animals;

public class DeleteAnimalHandler(
    IAnimalsRepository animalsRepository,
    ILogger<DeleteAnimalHandler> logger,
    IEventPublisher eventPublisher)
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

        // publish domain event
        await eventPublisher.PublishAsync(new AnimalDeletedEvent(request.AnimalId), cancellationToken);

        return Result.Ok();
    }
}
