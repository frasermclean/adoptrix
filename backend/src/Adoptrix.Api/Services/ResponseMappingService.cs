using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain;

namespace Adoptrix.Api.Services;

public interface IResponseMappingService
{
    AnimalResponse MapToResponse(Animal animal);
    AnimalResponse MapToResponse(SearchAnimalsResult result);
}

public class ResponseMappingService(ISqidConverter sqidConverter, IAnimalImageManager animalImageManager)
    : IResponseMappingService
{
    public AnimalResponse MapToResponse(Animal animal) => new()
    {
        Id = sqidConverter.ConvertToSqid(animal.Id),
        Name = animal.Name,
        Description = animal.Description,
        Species = animal.Species.Name,
        Breed = animal.Breed?.Name,
        DateOfBirth = animal.DateOfBirth,
        Images = animal.Images.Select(image => new AnimalImageResponse
        {
            Id = image.Id,
            Uri = animalImageManager.GetImageUri(animal.Id, image.FileName),
            Description = image.Description
        })
    };

    public AnimalResponse MapToResponse(SearchAnimalsResult result) => new()
    {
        Id = sqidConverter.ConvertToSqid(result.Id),
        Name = result.Name,
        Description = result.Description,
        Species = result.Species,
        Breed = result.Breed,
        DateOfBirth = result.DateOfBirth,
        Images = result.PrimaryImage is not null
            ? new[]
            {
                new AnimalImageResponse
                {
                    Id = result.PrimaryImage.Id,
                    Uri = animalImageManager.GetImageUri(result.Id, result.PrimaryImage.FileName),
                    Description = result.PrimaryImage.Description
                }
            }
            : Enumerable.Empty<AnimalImageResponse>()
    };
}