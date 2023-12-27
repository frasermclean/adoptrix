// using Adoptrix.Application.Models;
// using Adoptrix.Application.Services;
// using Adoptrix.Application.Services.Repositories;
// using Adoptrix.Domain;
// using FastEndpoints;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
//
// namespace Adoptrix.Application.Events;
//
// public class AnimalImageAddedEventHandler(
//     ILogger<AnimalDeletedEventHandler> logger,
//     IServiceScopeFactory serviceScopeFactory,
//     IImageProcessor imageProcessor)
//     : IEventHandler<AnimalImageAddedEvent>
// {
//     public async Task HandleAsync(AnimalImageAddedEvent eventModel, CancellationToken cancellationToken)
//     {
//         // resolve scoped services
//         await using var scope = serviceScopeFactory.CreateAsyncScope();
//         var animalImageManager = scope.ServiceProvider.GetRequiredService<IAnimalImageManager>();
//         var animalsRepository = scope.ServiceProvider.GetRequiredService<IAnimalsRepository>();
//
//         var (animalId, imageId) = eventModel;
//
//         // get original image stream
//         await using var originalReadStream = await animalImageManager.GetImageReadStreamAsync(
//             animalId, imageId, ImageCategory.Original, cancellationToken);
//
//         // process original image
//         await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream, cancellationToken);
//
//         await UploadImagesAsync(animalImageManager, animalId, imageId, bundle, cancellationToken);
//
//         // update database
//         await UpdateAnimalAsync(animalsRepository, animalId, imageId, cancellationToken);
//     }
//
//     private async Task UploadImagesAsync(IAnimalImageManager animalImageManager,
//         Guid animalId, Guid imageId, ImageStreamBundle bundle, CancellationToken cancellationToken)
//     {
//         // upload processed images
//         await Task.WhenAll(
//             animalImageManager.UploadImageAsync(animalId, imageId, bundle.ThumbnailWriteStream,
//                 ImageProcessor.OutputContentType, ImageCategory.Thumbnail, cancellationToken),
//             animalImageManager.UploadImageAsync(animalId, imageId, bundle.PreviewWriteStream,
//                 ImageProcessor.OutputContentType, ImageCategory.Preview, cancellationToken),
//             animalImageManager.UploadImageAsync(animalId, imageId, bundle.FullSizeWriteStream,
//                 ImageProcessor.OutputContentType, ImageCategory.FullSize, cancellationToken)
//         );
//
//         logger.LogInformation("Uploaded processed images for animal with ID: {AnimalId}", animalId);
//     }
//
//     private async Task UpdateAnimalAsync(IAnimalsRepository animalsRepository, Guid animalId, Guid imageId,
//         CancellationToken cancellationToken)
//     {
//         var result = await animalsRepository.GetAsync(animalId, cancellationToken);
//         if (result.IsFailed)
//         {
//             logger.LogError("Failed to get animal with ID: {AnimalId}", animalId);
//             return;
//         }
//
//         var animal = result.Value;
//         var image = animal.Images.First(image => image.Id == imageId);
//         image.IsProcessed = true;
//
//         logger.LogInformation("Set IsProcessed flag for image ID: {ImageId} on animal {AnimalId}", image.Id, animalId);
//         await animalsRepository.UpdateAsync(result.Value, cancellationToken);
//     }
// }