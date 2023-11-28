using Adoptrix.Application.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Services;

public class AnimalImageManager(
        ILogger<AnimalImageManager> logger,
        IHashGenerator hashGenerator,
        ISqidConverter sqidConverter,
        [FromKeyedServices(AnimalImageManager.ContainerName)]
        BlobContainerClient containerClient)
    : IAnimalImageManager
{
    public const string ContainerName = "animal-images";

    public string GenerateFileName(int animalId, string contentType, string originalFileName)
    {
        var baseName = hashGenerator.ComputeHash(animalId.ToString(), contentType, originalFileName);
        var fileExtension = GetFileExtension(contentType);

        return $"{baseName}.{fileExtension}";
    }

    public Uri GetImageUri(int animalId, string fileName)
    {
        var blobName = GetBlobName(animalId, fileName);
        return new Uri(containerClient.Uri, $"{ContainerName}/{blobName}");
    }

    public async Task<string> UploadImageAsync(string blobName, Stream imageStream, string contentType,
        CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient(blobName);

        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        var response = await blobClient.UploadAsync(imageStream, options, cancellationToken);

        logger.LogInformation("Uploaded image {BlobName} with content type {ContentType} to blob storage",
            blobName, contentType);

        return Convert.ToBase64String(response.Value.ContentHash);
    }

    public async Task<Result> DeleteImageAsync(int animalId, string fileName, CancellationToken cancellationToken)
    {
        var blobName = GetBlobName(animalId, fileName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);

        return Result.OkIf(response.Value, "Specified blob was not found");
    }

    private string GetBlobName(int animalId, string fileName)
    {
        var sqid = sqidConverter.ConvertToSqid(animalId);
        return $"{sqid}/{fileName}";
    }

    private static string GetFileExtension(string contentType)
        => contentType switch
        {
            "image/jpeg" => "jpg",
            "image/png" => "png",
            "image/gif" => "gif",
            _ => throw new InvalidOperationException($"Unsupported content type: {contentType}")
        };
}