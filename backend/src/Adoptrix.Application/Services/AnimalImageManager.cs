using Adoptrix.Application.Mapping;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Events;
using Adoptrix.Domain.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Uri ContainerUri { get; }

    Task<Result<AnimalImage>> UploadOriginalAsync(Guid animalId, Guid userId, string fileName, string description,
        string contentType, Stream stream, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> AddImagesToAnimalAsync(Animal animal, IReadOnlyCollection<AnimalImage> images,
        CancellationToken cancellationToken = default);

    Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default);

    Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original,
        CancellationToken cancellationToken = default);
}

public sealed class AnimalImageManager(
    ILogger<AnimalImageManager> logger,
    IAnimalsRepository animalsRepository,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager containerManager,
    IEventPublisher eventPublisher)
    : IAnimalImageManager
{
    public Uri ContainerUri => containerManager.ContainerUri;

    public async Task<Result<AnimalImage>> UploadOriginalAsync(Guid animalId, Guid userId, string fileName,
        string description, string contentType, Stream stream, CancellationToken cancellationToken = default)
    {
        var image = new AnimalImage
        {
            Description = description,
            OriginalFileName = fileName,
            OriginalContentType = contentType,
            UploadedBy = userId
        };

        var uploadResult = await UploadImageAsync(animalId, image.Id, stream, contentType, AnimalImageCategory.Original,
            cancellationToken);

        return uploadResult.IsSuccess
            ? image
            : uploadResult;
    }

    public async Task<Result<AnimalResponse>> AddImagesToAnimalAsync(Animal animal,
        IReadOnlyCollection<AnimalImage> images, CancellationToken cancellationToken = default)
    {
        animal.Images.AddRange(images);
        await animalsRepository.SaveChangesAsync(cancellationToken);

        foreach (var image in images)
        {
            await eventPublisher.PublishAsync(new AnimalImageAddedEvent(animal.Id, image.Id), cancellationToken);
        }

        return animal.ToResponse();
    }

    public async Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return await containerManager.UploadBlobAsync(blobName, imageStream, contentType, cancellationToken);
    }

    public async Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var blobNames = await containerManager.GetBlobNamesAsync($"{animalId}/", cancellationToken);
        var count = 0;

        foreach (var blobName in blobNames)
        {
            var deleteResult = await containerManager.DeleteBlobAsync(blobName, cancellationToken);
            if (deleteResult.IsFailed) continue;

            logger.LogInformation("Deleted blob {BlobName}", blobName);
            count++;
        }

        return count;
    }

    public async Task<Result> DeleteImageAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return await containerManager.DeleteBlobAsync(blobName, cancellationToken);
    }

    public Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original,
        CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return containerManager.OpenReadStreamAsync(blobName, cancellationToken);
    }
}
