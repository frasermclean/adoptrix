using Adoptrix.Api.Security;
using Adoptrix.Core.Events;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class DeleteAnimalEndpoint(AdoptrixDbContext dbContext, IEventPublisher eventPublisher)
    : EndpointWithoutRequest<Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("animals/{animalId:int}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<int>("animalId");

        var animal = await dbContext.Animals.FirstOrDefaultAsync(animal => animal.Id == animalId, cancellationToken);
        if (animal is null)
        {
            Logger.LogError("Could not find animal with ID: {AnimalId} to delete", animalId);
            return TypedResults.NotFound();
        }

        dbContext.Animals.Remove(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Marked animal with ID: {AnimalId} as deleted", animalId);

        await eventPublisher.PublishAsync(new AnimalDeletedEvent(animal.Id), cancellationToken);

        return TypedResults.NoContent();
    }
}
