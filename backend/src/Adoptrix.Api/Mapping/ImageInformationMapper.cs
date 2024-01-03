using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Domain;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class ImageInformationMapper
{
    public static partial AnimalImageResponse ToResponse(this ImageInformation imageInformation);
}