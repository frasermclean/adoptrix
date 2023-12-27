// using Adoptrix.Application.Services;
// using Adoptrix.Domain;
// using FastEndpoints;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
//
// namespace Adoptrix.Application.Events;
//
// public class AnimalDeletedEventHandler(
//     IServiceScopeFactory serviceScopeFactory,
//     ILogger<AnimalDeletedEventHandler> logger)
//     : IEventHandler<AnimalDeletedEvent>
// {
//     public async Task HandleAsync(AnimalDeletedEvent eventModel, CancellationToken cancellationToken)
//     {
//         // create a new scope to resolve scoped dependencies
//         await using var scope = serviceScopeFactory.CreateAsyncScope();
//         var imageManager = scope.ServiceProvider.GetRequiredService<IAnimalImageManager>();
//
//         var animal = eventModel.Animal;
//
//         // delete all images associated with the animal
//         foreach (var image in animal.Images)
//         {
//             var results = await Task.WhenAll(
//                 imageManager.DeleteImageAsync(animal.Id, image.Id, ImageCategory.Original, cancellationToken),
//                 imageManager.DeleteImageAsync(animal.Id, image.Id, ImageCategory.Thumbnail, cancellationToken),
//                 imageManager.DeleteImageAsync(animal.Id, image.Id, ImageCategory.Preview, cancellationToken),
//                 imageManager.DeleteImageAsync(animal.Id, image.Id, ImageCategory.FullSize, cancellationToken));
//
//             if (results.All(r => r.IsSuccess))
//             {
//                 logger.LogInformation("Deleted {VersionCount} versions of image {ImageId} for animal {AnimalId}",
//                     results.Length, image.Id, animal.Id);
//                 return;
//             }
//
//             foreach (var result in results.Where(r => r.IsFailed))
//             {
//                 logger.LogWarning("Failed to delete image {ImageId} for animal {AnimalId}: {Message}",
//                     image.Id, animal.Id, result.Errors.First().Message);
//             }
//         }
//     }
// }