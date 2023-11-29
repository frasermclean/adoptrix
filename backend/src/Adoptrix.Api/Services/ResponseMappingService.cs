using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Application.Services;
using Adoptrix.Domain;

namespace Adoptrix.Api.Services;

public interface IResponseMappingService
{
    AnimalResponse MapAnimal(Animal animal);
}

public class ResponseMappingService(ISqidConverter sqidConverter, IAnimalImageManager animalImageManager)
    : IResponseMappingService
{
    public AnimalResponse MapAnimal(Animal animal) => new()
    {
        Id = sqidConverter.ConvertToSqid(animal.Id),
        Name = animal.Name,
        Description = animal.Description,
        Species = animal.Species.Name,
        DateOfBirth = animal.DateOfBirth,
        Images = animal.Images.Select(image => new AnimalImageResponse
        {
            Id = image.Id,
            Uri = animalImageManager.GetImageUri(animal.Id, image.FileName),
            Description = image.Description
        })
    };
}