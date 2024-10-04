using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Logic.Mapping;

[Mapper]
public static partial class BreedResponseMapper
{
    public static partial BreedResponse ToResponse(this Breed breed);
}
