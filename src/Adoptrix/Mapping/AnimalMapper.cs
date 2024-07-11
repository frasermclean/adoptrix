using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Mapping;

[Mapper]
[UseStaticMapper(typeof(AnimalImageMapper))]
public static partial class AnimalMapper
{
    [MapProperty("Breed.Species.Id", "SpeciesId")]
    [MapProperty("Breed.Species.Name", "SpeciesName")]
    public static partial AnimalResponse ToResponse(this Animal animal);

    private static DateTime ConvertToUtc(DateTime dateTime) => dateTime.ToUniversalTime();
}
