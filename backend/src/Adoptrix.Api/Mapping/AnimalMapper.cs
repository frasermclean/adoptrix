using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Application.Models;
using Adoptrix.Domain;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
[UseStaticMapper(typeof(ImageInformationMapper))]
public static partial class AnimalMapper
{
    public static partial AnimalResponse ToResponse(this Animal animal);
    public static partial AnimalResponse ToResponse(this SearchAnimalsResult result);

    private static DateTime ConvertToUtc(DateTime dateTime) => dateTime.ToUniversalTime();
}