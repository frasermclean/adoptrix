using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class BreedMatchMapper
{
    public static partial BreedMatch ToMatch(this SearchBreedsItem item);
}
