using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class DeleteAnimalEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(Guid animalId, ISender sender,
       IEventPublisher eventPublisher, CancellationToken cancellationToken)
    {
        // delete animal from database
        var result = await sender.Send(new DeleteAnimalRequest(animalId), cancellationToken);
        if (result.IsFailed)
        {
            return TypedResults.NotFound();
        }

        // publish domain event
        await eventPublisher.PublishDomainEventAsync( new AnimalDeletedEvent(animalId), cancellationToken);

        return TypedResults.NoContent();
    }
}
