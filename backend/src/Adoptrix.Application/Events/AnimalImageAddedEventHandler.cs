using Adoptrix.Application.Services;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.Events;

public class AnimalImageAddedEventHandler(IServiceScopeFactory serviceScopeFactory, IImageProcessor imageProcessor)
    : IEventHandler<AnimalImageAddedEvent>
{
    public async Task HandleAsync(AnimalImageAddedEvent eventModel, CancellationToken ct)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var animalImageManager = scope.ServiceProvider.GetRequiredService<IAnimalImageManager>();

        await using var imageStream =
            await animalImageManager.GetOriginalImageAsync(eventModel.AnimalId, eventModel.ImageId, ct);

        imageProcessor.CreateThumbnail(imageStream);
    }
}