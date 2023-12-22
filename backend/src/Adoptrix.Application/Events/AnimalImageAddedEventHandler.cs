using Adoptrix.Application.Services;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.Events;

public class AnimalImageAddedEventHandler(IServiceScopeFactory serviceScopeFactory, IImageProcessor imageProcessor)
    : IEventHandler<AnimalImageAddedEvent>
{
    public async Task HandleAsync(AnimalImageAddedEvent eventModel, CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var animalImageManager = scope.ServiceProvider.GetRequiredService<IAnimalImageManager>();

        var (animalId, imageId) = eventModel;

        // get original image stream
        await using var originalReadStream = await animalImageManager.GetImageReadStreamAsync(
            animalId, imageId, ImageCategory.Original, cancellationToken);

        // process original image
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream, cancellationToken);

        // upload processed images
        await Task.WhenAll(
            animalImageManager.UploadImageAsync(animalId, imageId, bundle.ThumbnailWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.Thumbnail, cancellationToken),
            animalImageManager.UploadImageAsync(animalId, imageId, bundle.PreviewWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.Preview, cancellationToken),
            animalImageManager.UploadImageAsync(animalId, imageId, bundle.FullSizeWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.FullSize, cancellationToken)
        );
    }
}