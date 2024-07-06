using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;

namespace Adoptrix.Application.Extensions;

public static class AnimalImageResponseExtensions
{
    public static void SetImageUrls(this AnimalImageResponse response, Guid animalId, Uri containerUri)
    {
        var previewBlobName = AnimalImage.GetBlobName(animalId, response.Id, AnimalImageCategory.Preview);
        var thumbnailBlobName = AnimalImage.GetBlobName(animalId, response.Id, AnimalImageCategory.Thumbnail);
        var fullSizeBlobName = AnimalImage.GetBlobName(animalId, response.Id, AnimalImageCategory.FullSize);

        response.PreviewUrl = $"{containerUri}/{previewBlobName}";
        response.ThumbnailUrl = $"{containerUri}/{thumbnailBlobName}";
        response.FullSizeUrl = $"{containerUri}/{fullSizeBlobName}";
    }
}
