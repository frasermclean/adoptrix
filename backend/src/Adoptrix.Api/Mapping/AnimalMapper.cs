using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Domain;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
[UseStaticMapper(typeof(SpeciesMapper))]
[UseStaticMapper(typeof(BreedMapper))]
[UseStaticMapper(typeof(ImageResponseMapper))]
public static partial class AnimalMapper
{
    public static partial AnimalResponse ToResponse(this Animal animal);

    private static DateTime ConvertToUtc(DateTime dateTime) => dateTime.ToUniversalTime();
}
