using Adoptrix.Application.Events;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Services;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands;

public class DeleteAnimalCommandHandler(
        ILogger<DeleteAnimalCommandHandler> logger,
        ISqidConverter sqidConverter,
        IAnimalsRepository repository,
        IEventQueueService eventQueueService)
    : ICommandHandler<DeleteAnimalCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {
        var animalId = sqidConverter.ConvertToInt(command.Id);

        // find existing animal
        var result = await repository.GetAsync(animalId, cancellationToken);
        if (result.IsFailed)
        {
            logger.LogError("Could not delete animal with id {Id}", animalId);
            return result.ToResult();
        }

        var animal = result.Value;
        await repository.DeleteAsync(animal, cancellationToken);

        eventQueueService.PushDomainEvent(new AnimalDeletedEvent(animal));

        logger.LogInformation("Deleted animal with id {Id}", animalId);
        return Result.Ok();
    }
}