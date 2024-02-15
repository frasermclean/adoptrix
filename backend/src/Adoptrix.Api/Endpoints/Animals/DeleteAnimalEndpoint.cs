using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Events;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class DeleteAnimalEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(Guid animalId, IAnimalsRepository repository,
        ILogger<DeleteAnimalEndpoint> logger, IEventPublisher eventPublisher, CancellationToken cancellationToken)
    {
        // find existing animal
        var getResult = await repository.GetAsync(animalId, cancellationToken);
        if (getResult.IsFailed)
        {
            logger.LogError("Could not find animal with ID: {AnimalId}", animalId);
            return TypedResults.NotFound();
        }

        // delete animal from database
        var animal = getResult.Value;
        await repository.DeleteAsync(animal, cancellationToken);

        // publish domain event
        var domainEvent = new AnimalDeletedEvent(animal.Id);
        await eventPublisher.PublishDomainEventAsync(domainEvent, cancellationToken);

        return TypedResults.NoContent();
    }
}
