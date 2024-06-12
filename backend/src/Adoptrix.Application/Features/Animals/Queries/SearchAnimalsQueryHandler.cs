using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Support;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class SearchAnimalsQueryHandler(IAnimalsRepository animalsRepository, IAnimalImageManager animalImageManager)
    : IRequestHandler<SearchAnimalsQuery, IEnumerable<SearchAnimalsResult>>
{
    public async Task<IEnumerable<SearchAnimalsResult>> Handle(SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        var results = await animalsRepository.SearchAsync(query, cancellationToken);
        var resultsList = results as List<SearchAnimalsResult> ?? results.ToList();

        foreach (var result in resultsList)
        {
            SetImageUrls(result.Id, result.Image);
        }

        return resultsList;
    }

    private void SetImageUrls(Guid animalId, AnimalImageResponse? imageResponse)
    {
        if (imageResponse is null || !imageResponse.IsProcessed)
        {
            return;
        }

        var containerUri = animalImageManager.ContainerUri;
        var previewBlobName = animalImageManager.GetBlobName(animalId, imageResponse.Id, ImageCategory.Preview);
        var thumbnailBlobName = animalImageManager.GetBlobName(animalId, imageResponse.Id, ImageCategory.Thumbnail);
        var fullSizeBlobName = animalImageManager.GetBlobName(animalId, imageResponse.Id, ImageCategory.FullSize);

        imageResponse.PreviewUrl = $"{containerUri}/{previewBlobName}";
        imageResponse.ThumbnailUrl = $"{containerUri}/{thumbnailBlobName}";
        imageResponse.FullSizeUrl = $"{containerUri}/{fullSizeBlobName}";
    }
}
