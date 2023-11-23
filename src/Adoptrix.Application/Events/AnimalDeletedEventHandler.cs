using Adoptrix.Application.Services;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Events;

public class AnimalDeletedEventHandler(IServiceScopeFactory serviceScopeFactory,
        ILogger<AnimalDeletedEventHandler> logger)
    : IEventHandler<AnimalDeletedEvent>
{
    public async Task HandleAsync(AnimalDeletedEvent eventModel, CancellationToken cancellationToken)
    {
        // create a new scope to resolve scoped dependencies
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var imageManager = scope.ServiceProvider.GetRequiredService<IAnimalImageManager>();

        var animal = eventModel.Animal;

        // delete all images associated with the animal
        foreach (var image in animal.Images)
        {
            var result = await imageManager.DeleteImageAsync(animal.Id, image.FileName, cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Deleted image {FileName} for animal {AnimalId}", image.FileName, animal.Id);
            }
            else
            {
                logger.LogError("Failed to delete image {FileName} for animal {AnimalId}: {Error}",
                    image.FileName, animal.Id, result.Errors.First());
            }
        }
    }
}