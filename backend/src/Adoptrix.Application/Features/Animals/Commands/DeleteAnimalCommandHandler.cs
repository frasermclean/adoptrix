using Adoptrix.Application.Services;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Events;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Features.Animals.Commands;

public class DeleteAnimalCommandHandler(
    IAnimalsRepository animalsRepository,
    ILogger<DeleteAnimalCommandHandler> logger,
    IEventPublisher eventPublisher)
    : IRequestHandler<DeleteAnimalCommand, Result>
{
    public async Task<Result> Handle(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(command.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Could not find animal with ID: {AnimalId} to delete", command.AnimalId);
            return new AnimalNotFoundError(command.AnimalId);
        }

        await animalsRepository.DeleteAsync(animal, cancellationToken);
        logger.LogInformation("Deleted animal with ID: {AnimalId}", command.AnimalId);

        // publish domain event
        await eventPublisher.PublishAsync(new AnimalDeletedEvent(command.AnimalId), cancellationToken);

        return Result.Ok();
    }
}
