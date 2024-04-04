using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class DeleteAnimalEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(Guid animalId, IAnimalsRepository repository,
        ILogger<DeleteAnimalEndpoint> logger, IEventPublisher eventPublisher, CancellationToken cancellationToken)
    {
        // delete animal from database
        var result = await repository.DeleteAsync(animalId, cancellationToken);
        if (result.IsFailed)
        {
            logger.LogError("Could not find animal with ID: {AnimalId}", animalId);
            return TypedResults.NotFound();
        }

        // publish domain event
        await eventPublisher.PublishDomainEventAsync( new AnimalDeletedEvent(animalId), cancellationToken);

        return TypedResults.NoContent();
    }
}
