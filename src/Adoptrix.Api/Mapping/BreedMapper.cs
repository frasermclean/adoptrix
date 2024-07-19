﻿using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class BreedMapper
{
    public static partial BreedResponse ToResponse(this Breed breed);
}