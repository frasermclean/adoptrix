using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Application.Models;
using Adoptrix.Domain;

namespace Adoptrix.Api.Services;

public interface IResponseMappingService
{
    AnimalResponse Map(Animal animal);
    AnimalResponse Map(SearchAnimalsResult result);
    BreedResponse Map(Breed breed);
    BreedResponse Map(SearchBreedsResult result);
}

public class ResponseMappingService : IResponseMappingService
{
    public AnimalResponse Map(Animal animal) => new()
    {
        Id = animal.Id,
        Name = animal.Name,
        Description = animal.Description,
        Species = animal.Species.Name,
        Breed = animal.Breed?.Name,
        Sex = animal.Sex,
        DateOfBirth = animal.DateOfBirth,
        CreatedAt = animal.CreatedAt.ToUtc(),
        Images = animal.Images.Select(image => new AnimalImageResponse
        {
            Id = image.Id,
            Description = image.Description,
            HasThumbnail = image.HasThumbnail,
            HasPreview = image.HasPreview,
            HasFullSize = image.HasFullSize
        })
    };

    public AnimalResponse Map(SearchAnimalsResult result) => new()
    {
        Id = result.Id,
        Name = result.Name,
        Description = result.Description,
        Species = result.Species,
        Breed = result.Breed,
        Sex = result.Sex,
        DateOfBirth = result.DateOfBirth,
        CreatedAt = result.CreatedAt.ToUtc(),
        Images = result.Images.Select(image => new AnimalImageResponse
        {
            Id = image.Id,
            Description = image.Description,
            HasThumbnail = image.HasThumbnail,
            HasPreview = image.HasPreview,
            HasFullSize = image.HasFullSize
        })
    };

    public BreedResponse Map(Breed breed) => new()
    {
        Id = breed.Id,
        Name = breed.Name,
        Species = breed.Species.Name,
        AnimalIds = breed.Animals.Select(animal => animal.Id)
    };

    public BreedResponse Map(SearchBreedsResult result) => new()
    {
        Id = result.Id,
        Name = result.Name,
        Species = result.Species,
        AnimalIds = result.AnimalIds
    };
}