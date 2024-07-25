using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class SpeciesResponseMapper
{
    public static partial SpeciesResponse ToResponse(this Species species);
}
