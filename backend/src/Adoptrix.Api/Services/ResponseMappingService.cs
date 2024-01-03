using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Application.Models;
using Adoptrix.Domain;

namespace Adoptrix.Api.Services;

public interface IResponseMappingService
{
    BreedResponse Map(Breed breed);
    BreedResponse Map(SearchBreedsResult result);
}

public class ResponseMappingService : IResponseMappingService
{
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