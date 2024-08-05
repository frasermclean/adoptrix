using Adoptrix.Api.Security;
using Adoptrix.Core.Events;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class DeleteAnimalEndpoint(IAnimalsRepository animalsRepository, IEventPublisher eventPublisher)
    : Endpoint<DeleteAnimalRequest, Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("animals/{animalId:int}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            Logger.LogError("Could not find animal with ID: {AnimalId} to delete", request.AnimalId);
            return TypedResults.NotFound();
        }

        animal.IsDeleted = true;
        await animalsRepository.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Marked animal with ID: {AnimalId} as deleted", request.AnimalId);

        await eventPublisher.PublishAsync(new AnimalDeletedEvent(request.AnimalId), cancellationToken);

        return TypedResults.NoContent();
    }
}
