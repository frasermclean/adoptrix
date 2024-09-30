using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Logic.Mapping;

[Mapper]
public static partial class AnimalImageResponseMapper
{
    public static partial AnimalImageResponse ToResponse(this AnimalImage animalImage);
}

