using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class SearchAnimalsQueryHandler(IAnimalsRepository animalsRepository, IAnimalImageManager animalImageManager)
    : IRequestHandler<SearchAnimalsQuery, IEnumerable<AnimalMatch>>
{
    public async Task<IEnumerable<AnimalMatch>> Handle(SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        var results = await animalsRepository.SearchAsync(query, cancellationToken);
        var resultsList = results as List<AnimalMatch> ?? results.ToList();

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
        var previewBlobName = AnimalImage.GetBlobName(animalId, imageResponse.Id, AnimalImageCategory.Preview);
        var thumbnailBlobName = AnimalImage.GetBlobName(animalId, imageResponse.Id, AnimalImageCategory.Thumbnail);
        var fullSizeBlobName = AnimalImage.GetBlobName(animalId, imageResponse.Id, AnimalImageCategory.FullSize);

        imageResponse.PreviewUrl = $"{containerUri}/{previewBlobName}";
        imageResponse.ThumbnailUrl = $"{containerUri}/{thumbnailBlobName}";
        imageResponse.FullSizeUrl = $"{containerUri}/{fullSizeBlobName}";
    }
}
