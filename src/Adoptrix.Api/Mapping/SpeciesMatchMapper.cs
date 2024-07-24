using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class SpeciesMatchMapper
{
    public static partial SpeciesMatch ToMatch(this SearchSpeciesItem item);
}
