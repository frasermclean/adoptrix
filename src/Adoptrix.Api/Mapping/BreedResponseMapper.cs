using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Core;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class BreedResponseMapper
{
    [MapProperty("Animals.Count", "AnimalCount")]
    public static partial BreedResponse ToResponse(this Breed breed);
}
