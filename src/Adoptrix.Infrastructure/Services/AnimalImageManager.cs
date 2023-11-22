using Adoptrix.Application.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Services;

public class AnimalImageManager(ILogger<AnimalImageManager> logger, IHashGenerator hashGenerator,
        [FromKeyedServices("animal-images")] BlobContainerClient containerClient)
    : IAnimalImageManager
{
    public string GenerateFileName(string contentType, string originalFileName)
    {
        var fileName = hashGenerator.ComputeHash(contentType, originalFileName);
        var fileExtension = CalculateFileExtension(contentType);

        return $"{fileName}.{fileExtension}";
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

    public async Task<Result> DeleteImageAsync(string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);

        return Result.OkIf(response.Value, "Specified blob was not found");
    }

    private static string CalculateFileExtension(string contentType)
        => contentType switch
        {
            "image/jpeg" => "jpg",
            "image/png" => "png",
            "image/gif" => "gif",
            _ => throw new InvalidOperationException($"Unsupported content type: {contentType}")
        };
}