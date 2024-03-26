using Adoptrix.Application.Models;
using Adoptrix.Domain;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class ImageResponseMapper
{
    public static partial ImageResponse ToResponse(this ImageInformation imageInformation);
}
