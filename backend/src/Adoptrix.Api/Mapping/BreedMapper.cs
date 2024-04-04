using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Application.Models;
using Adoptrix.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class BreedMapper
{
    [MapProperty(nameof(Breed.Animals), nameof(BreedResponse.AnimalIds))]
    public static partial BreedResponse ToResponse(this Breed breed);
    public static partial BreedResponse ToResponse(this SearchBreedsResult result);


    private static IEnumerable<Guid> MapAnimalIds(ICollection<Animal> animals) => animals.Select(animal => animal.Id);
}