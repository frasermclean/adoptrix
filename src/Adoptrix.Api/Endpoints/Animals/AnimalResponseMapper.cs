using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;

namespace Adoptrix.Api.Endpoints.Animals;

public class AnimalResponseMapper(
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager blobContainerManager)
    : ResponseMapper<AnimalResponse, Animal>
{
    public override AnimalResponse FromEntity(Animal animal) => new()
    {
        Id = animal.Id,
        Slug = animal.Slug,
        Name = animal.Name,
        Description = animal.Description,
        SpeciesName = animal.Breed.Species.Name,
        BreedName = animal.Breed.Name,
        Sex = animal.Sex,
        DateOfBirth = animal.DateOfBirth,
        LastModifiedUtc = animal.LastModifiedUtc,
        Images = animal.Images.Select(image => new AnimalImageResponse
        {
            Id = image.Id,
            Description = image.Description,
            PreviewUrl = image.IsProcessed
                ? $"{blobContainerManager.ContainerUri}/{image.PreviewBlobName}"
                : null,
            ThumbnailUrl = image.IsProcessed
                ? $"{blobContainerManager.ContainerUri}/{image.ThumbnailBlobName}"
                : null,
            FullSizeUrl = image.IsProcessed
                ? $"{blobContainerManager.ContainerUri}/{image.FullSizeBlobName}"
                : null
        })
    };
}
