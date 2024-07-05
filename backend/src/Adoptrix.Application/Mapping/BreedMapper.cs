using Adoptrix.Domain;
using Adoptrix.Domain.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Application.Mapping;

[Mapper]
public static partial class BreedMapper
{
    [MapProperty(nameof(Breed.Animals), nameof(BreedResponse.AnimalIds))]
    public static partial BreedResponse ToResponse(this Breed breed);
}
