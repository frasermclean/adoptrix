using Adoptrix.Api.Security;
using Adoptrix.Core.Events;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class DeleteAnimalEndpoint(AdoptrixDbContext dbContext, IEventPublisher eventPublisher)
    : EndpointWithoutRequest<Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("animals/{animalId:guid}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<Guid>("animalId");

        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken);
        if (animal is null)
        {
            return TypedResults.NotFound();
        }

        dbContext.Remove(animal);
        await dbContext.SaveChangesAsync(cancellationToken);
        Logger.LogInformation("Deleted animal with ID {BreedId}", animalId);

        // TODO: Move event publishing to EF Core interceptor
        await eventPublisher.PublishAsync(new AnimalDeletedEvent(animal.Slug), cancellationToken);

        return TypedResults.NoContent();
    }
}
